using BanqueTardiApp.Donnees;
using BanqueTardiApp.Models;

namespace BanqueTardiApp.Services.Interfaces
{
    public interface ICompteBancaireService
    {
        public abstract Task<string> GenererNumeroCompteBancaireAsync(CompteBancaire compte);
        public abstract Task<List<CompteBancaire>> ObtenirListeComptesAsync(string? codeClient);
        public abstract Task<CompteBancaire?> ObtenirCompteParNumeroAsync(string? noCompte);
        public abstract CompteBancaire? CreerNouveauCompte(string? codeClient);
        public abstract Task<bool> EnregistrerNouveauCompteAsync(CompteBancaire compteBancaire);
        public abstract Task<bool> ModifierCompteAsync(string? noCompte, CompteBancaire compteBancaire);
        public abstract Task<bool> PeutModifierCompteBancaire(string? numeroCompte);
    }
}
