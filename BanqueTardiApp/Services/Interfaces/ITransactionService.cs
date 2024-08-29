using BanqueTardiApp.Models;

namespace BanqueTardiApp.Services.Interfaces
{
    public interface ITransactionService
    {
        public abstract Task<List<Transaction>> ObtenirDernieresTransactionsAsync(string? noCompte, int nombre);
        public abstract Task<List<Transaction>> ObtenirListeTransactionsAsync(string? noCompteBancaire);
        public abstract Task<Transaction?> CreerNouvelleTransactionAsync(string? noCompteBancaire);
        public abstract Task<(bool succes, string messageErreur)> EnregistrerNouvelleTransactionAsync(Transaction transaction);
    }
}
