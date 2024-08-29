using BanqueTardiApp.Donnees;
using BanqueTardiApp.Models;
using BanqueTardiApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BanqueTardiApp.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly BanqueContexte _contexte;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(BanqueContexte contexte, ILogger<TransactionService> logger)
        {
            _contexte = contexte;
            _logger = logger;
        }

        public async Task<List<Transaction>> ObtenirListeTransactionsAsync(string noCompteBancaire)
        {
            var transactions = await _contexte.Transactions
                .Where(t => t.NumeroCompte == noCompteBancaire)
                .ToListAsync();

            return transactions;
        }

        public async Task<List<Transaction>> ObtenirDernieresTransactionsAsync(string? noCompte, int nombre) 
        {
            var transaction = await _contexte.Transactions
                .Where(t => t.NumeroCompte == noCompte)
                .OrderByDescending(t => t.DateOperation)
                .Take(nombre)
                .ToListAsync();

            return transaction;
        }

        public async Task<Transaction?> CreerNouvelleTransactionAsync(string? noCompteBancaire)
        {
            if (string.IsNullOrWhiteSpace(noCompteBancaire))
            {
                throw new ArgumentException(nameof(noCompteBancaire), "Le numéro de compte bancaire ne peut pas être null ou vide.");
            }

            var compteBancaire = await _contexte.ComptesBancaires
                .FirstOrDefaultAsync(c => c.NumeroCompte == noCompteBancaire);

            if (compteBancaire == null)
            {
                throw new InvalidOperationException($"Aucun compte bancaire trouvé avec le numéro {noCompteBancaire}.");
            }

            var transaction = new Transaction
            {
                NumeroCompte = noCompteBancaire,
                CompteBancaire = compteBancaire,                                         
            };

            return transaction;
        }

        public async Task<(bool succes, string messageErreur)> EnregistrerNouvelleTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                return (false, "La transaction ne peut pas être null.");
            }

            try
            {
                var compteBancaire = await _contexte.ComptesBancaires
                    .Include(cb => cb.Client)
                    .FirstOrDefaultAsync(cb => cb.NumeroCompte == transaction.NumeroCompte);

                if (compteBancaire == null)
                {
                    return (false, $"Aucun compte bancaire trouvé avec le numéro {transaction.NumeroCompte}.");
                }

                var nouvelleBalance = transaction.TypeOperation == TypeOperation.Credit
                    ? compteBancaire.Solde + transaction.Montant
                    : compteBancaire.Solde - transaction.Montant;

                if (nouvelleBalance < 0 && compteBancaire.TypeCompte != TypeCompte.Cheque)
                {
                    return (false, "Le solde du compte ne peut pas devenir négatif pour ce type de compte.");
                }

                if (nouvelleBalance < 0 && compteBancaire.TypeCompte == TypeCompte.Cheque)
                {
                    var decouvertTransaction = new Transaction
                    {
                        NumeroCompte = compteBancaire.NumeroCompte,
                        TypeOperation = TypeOperation.Debit,
                        Montant = 10,
                        Libelle = "Découvert",
                        DateOperation = DateTime.Now
                    };

                    _contexte.Transactions.Add(decouvertTransaction);
                    compteBancaire.Solde = nouvelleBalance - 10;
                    compteBancaire.Client.NbDecouverts++;
                }
                else
                {
                    compteBancaire.Solde = nouvelleBalance;
                }

                _contexte.Transactions.Add(transaction);
                await _contexte.SaveChangesAsync();

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur est survenue lors de l'enregistrement de la transaction.");
                return (false, "Une erreur est survenue lors de l'enregistrement de la transaction.");
            }
        }
    }
}
