using BanqueTardiApp.Donnees;
using BanqueTardiApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BanqueTardiApp.Models;
using System.Data;

namespace BanqueTardiApp.Services.Implementations
{
    public class ClientService : IClientService
    {
        public ClientService(BanqueContexte contexte, ILogger<ClientService> logger) 
        {
            _contexte = contexte;
            _logger = logger;
        }
        private readonly BanqueContexte _contexte;
        private readonly ILogger<ClientService> _logger;

        public async Task<string> GenererCodeClientAsync()
        {
            Random hasard = new Random();
            int nombreAleatoire;
            string codeGenerer;

            do
            {
                nombreAleatoire = hasard.Next(1, 10000000);
                codeGenerer = $"CL{nombreAleatoire:D7}";
            }
            while (await _contexte.Clients.AnyAsync(c => c.Code == codeGenerer));

            return codeGenerer;
        }

        public async Task<List<Client>> RechercherListeClientsAsync(string? nomRechercher)
        {
            nomRechercher = nomRechercher?.Trim();
            var clientList = await _contexte.Clients
                .Where(client => string.IsNullOrWhiteSpace(nomRechercher) ||
                                 EF.Functions.Like(client.Nom, $"%{nomRechercher}%") ||
                                 EF.Functions.Like(client.Prenom, $"%{nomRechercher}%"))
                .ToListAsync();

            return clientList;
        }

        public async Task<Client?> ObtenirClientParCodeAsync(string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            var client = await _contexte.Clients
                                .Include(c => c.Comptes)
                                .FirstOrDefaultAsync(c => c.Code == code);

            return client;
        }

        public async Task<Client> CreerNouveauClientAsync()
        {
            var client = new Client
            {
                Code = await GenererCodeClientAsync()
            };

            return client;
        }

        public async Task<bool> EnregistrerNouveauClientAsync(Client client)
        {
            if (client == null)
            {
                _logger.LogWarning("Tentative d'enregistrement d'un client null.");
                throw new ArgumentNullException(nameof(client));
            }

            if (await _contexte.Clients.AnyAsync(c => c.Code == client.Code)) 
            {
                _logger.LogWarning($"Un client avec le code {client.Code} existe déjà.");
                return false;
            }

            try
            {
                await _contexte.Clients.AddAsync(client);
                await _contexte.SaveChangesAsync();
                _logger.LogInformation($"Client {client.Code} enregistré avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur est survenue lors de l'enregistrement du client.");
                return false;
            }
        }

        public async Task<bool> ModifierClientAsync(string? code, Client client)
        {
            if (client == null)
            {
                _logger.LogWarning("Tentative de modification d'un client null.");
                throw new ArgumentNullException(nameof(client));
            }

            try
            {
                if (string.IsNullOrWhiteSpace(code)) 
                {
                    _logger.LogWarning($"Le code client ne peut pas être null.");
                    return false;
                }

                var clientAModifier = await _contexte.Clients.FindAsync(code);

                if (clientAModifier == null)
                {
                    _logger.LogWarning($"Client avec le code {code} n'existe pas.");
                    return false;
                }

                foreach (var propriete in typeof(Client).GetProperties())
                {
                    if (propriete.CanWrite)
                    {
                        var nouvelleValeur = propriete.GetValue(client);
                        var ancienneValeur = propriete.GetValue(clientAModifier);

                        if (!Equals(ancienneValeur, nouvelleValeur))
                        {
                            propriete.SetValue(clientAModifier, nouvelleValeur);
                        }
                    }
                }

                await _contexte.SaveChangesAsync();
                _logger.LogInformation($"Client {code} modifié avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur est survenue lors de la modification du client.");
                return false;
            }
        }

        public async Task<bool> SupprimerClientAsync(string? code, Client client)
        {
            try
            {
                var clientASupprimer = await _contexte.Clients.FindAsync(code);
                if (clientASupprimer == null)
                {
                    _logger.LogWarning($"Client avec le code {code} n'existe pas.");
                    return false;
                }

                _contexte.Clients.Remove(clientASupprimer);
                await _contexte.SaveChangesAsync();
                _logger.LogInformation($"Client {code} supprimé avec succès.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une erreur est survenue lors de la suppression du client.");
                return false;
            }
        }

    }
}
