using BanqueTardiApp.Donnees;

namespace BanqueTardiApp.Services.Interfaces
{
    public interface IDbInitalisateurService
    {
        public abstract Task InitialiserAsync();
        public abstract Task ReinitialiserAsync();
    }
}
