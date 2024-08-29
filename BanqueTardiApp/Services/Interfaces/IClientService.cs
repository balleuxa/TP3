using BanqueTardiApp.Models;

namespace BanqueTardiApp.Services.Interfaces
{
    public interface IClientService
    {
        public abstract Task<string> GenererCodeClientAsync();
        public abstract Task<List<Client>> RechercherListeClientsAsync(string? nomRechercher);
        public abstract Task<Client?> ObtenirClientParCodeAsync(string? code);
        public abstract Task<Client> CreerNouveauClientAsync();
        public abstract Task<bool> EnregistrerNouveauClientAsync(Client client);
        public abstract Task<bool> ModifierClientAsync(string? code, Client client);
        public abstract Task<bool> SupprimerClientAsync(string? code, Client client);
    }
}
