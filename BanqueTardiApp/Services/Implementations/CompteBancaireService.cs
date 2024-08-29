using BanqueTardiApp.Controllers;
using BanqueTardiApp.Donnees;
using BanqueTardiApp.Models;
using BanqueTardiApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BanqueTardiApp.Services.Implementations
{
    public class CompteBancaireService : ICompteBancaireService
    {
        private readonly BanqueContexte _contexte;
        private readonly ITransactionService _transactionService;

        public CompteBancaireService(ILogger<CompteBancairesController> logger, BanqueContexte contexte, ITransactionService transactionService) 
        {
            _logger = logger;
            _contexte = contexte;
            _transactionService = transactionService;
        }
        private readonly ILogger<CompteBancairesController> _logger;
        private const int NO_TRANSIT = 00234;
        private const int NO_INSTITUTION_BANCAIRE = 001;

        public async Task<string> GenererNumeroCompteBancaireAsync(CompteBancaire compte)
        {
            var dernierCompte = await _contexte.ComptesBancaires
                .AsNoTracking()
                .Where(c => c.NumeroCompte != null && c.NumeroCompte.StartsWith($"{NO_TRANSIT:D5}-{NO_INSTITUTION_BANCAIRE:D3}-{compte.Identifiant}"))
                .OrderByDescending(c => c.NumeroCompte != null ? c.NumeroCompte.Substring(12, 5) : "00000")
                .FirstOrDefaultAsync();

            int dernierNoCompte = dernierCompte?.NumeroCompte != null
                ? int.Parse(dernierCompte.NumeroCompte.Substring(12, 5))
                : 0;

            string prochainNumeroCompte = $"{compte.Identifiant}{(dernierNoCompte + 1):D5}";

            return $"{NO_TRANSIT:D5}-{NO_INSTITUTION_BANCAIRE:D3}-{prochainNumeroCompte}";
        }

        public async Task<bool> PeutModifierCompteBancaire(string? numeroCompte)
        {
            var compte = await _contexte.ComptesBancaires
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.NumeroCompte == numeroCompte);

            if (compte == null)
            {
                return false;
            }

            return compte.Transactions.Count == 1 && compte.Transactions.All(t => t.Libelle.Contains("ouverture"));
        }

        public async Task<List<CompteBancaire>> ObtenirListeComptesAsync(string? codeClient)
        {
            var comptes = await _contexte.ComptesBancaires
                .Where(c => c.ClientCode == codeClient).ToListAsync();

            return comptes;
        }

        public async Task<CompteBancaire?> ObtenirCompteParNumeroAsync(string? noCompte)
        {
            if (!string.IsNullOrWhiteSpace(noCompte))
            {
                var compte = await _contexte.ComptesBancaires
                            .Include(cb => cb.Client)
                            .FirstOrDefaultAsync(cb => cb.NumeroCompte == noCompte);
                return compte;
            }
            
            return null;
        }

        public CompteBancaire? CreerNouveauCompte(string? codeClient)
        {
            if (!string.IsNullOrWhiteSpace(codeClient))
            {
                var compte = new CompteBancaire(codeClient);
                return compte;
            }

            return null;
        }
        public async Task<bool> EnregistrerNouveauCompteAsync(CompteBancaire compteBancaire)
        {
            if (compteBancaire == null)
            {
                _logger.LogWarning("Tentative d'enregistrement d'un compte bancaire null.");
                throw new ArgumentNullException(nameof(compteBancaire));
            }

            if (await _contexte.ComptesBancaires.AnyAsync(c => c.NumeroCompte == compteBancaire.NumeroCompte))
            {
                _logger.LogWarning($"Un compte avec le numéro {compteBancaire.NumeroCompte} existe déjà.");
                return false;
            }

            try
            {
                compteBancaire.NumeroCompte = await GenererNumeroCompteBancaireAsync(compteBancaire);
                _contexte.ComptesBancaires.Add(compteBancaire);
                await _contexte.SaveChangesAsync();

                if (compteBancaire.Solde > 0)
                {
                    var transaction = await _transactionService.CreerNouvelleTransactionAsync(compteBancaire.NumeroCompte);

                    if (transaction != null)
                    {
                        transaction.Libelle = "Solde d’ouverture";
                        transaction.TypeOperation = TypeOperation.Credit;
                        transaction.Montant = compteBancaire.Solde;

                        _contexte.Transactions.Add(transaction);
                    }
                }
                await _contexte.SaveChangesAsync();
                _logger.LogInformation($"Compte bancaire {compteBancaire.NumeroCompte} enregistré avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur est survenue lors de l'enregistrement du compte bancaire.");
                return false;
            }
        }

        public async Task<bool> ModifierCompteAsync(string? noCompte, CompteBancaire compteBancaire)
        {
            if (compteBancaire == null)
            {
                _logger.LogWarning("Tentative de modification d'un compte bancaire null.");
                throw new ArgumentNullException(nameof(compteBancaire));
            }

            try
            {
                if (string.IsNullOrWhiteSpace(noCompte))
                {
                    _logger.LogWarning($"Le numéro de compte ne peut pas être null.");
                    return false;
                }

                var compteAModifier = await _contexte.ComptesBancaires.FindAsync(noCompte);

                if (compteAModifier == null)
                {
                    _logger.LogWarning($"Compte bancaire avec le numéro {noCompte} n'existe pas.");
                    return false;
                }

                compteAModifier.TypeCompte = compteBancaire.TypeCompte;
                compteAModifier.Solde = compteBancaire.Solde;

                await _contexte.SaveChangesAsync();
                _logger.LogInformation($"Compte bancaire {noCompte} modifié avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur est survenue lors de la modification du compte bancaire.");
                return false;
            }
        }
    }
}
