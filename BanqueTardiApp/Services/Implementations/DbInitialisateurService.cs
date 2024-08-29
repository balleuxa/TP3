using BanqueTardiApp.Donnees;
using BanqueTardiApp.Models;
using BanqueTardiApp.Services.Interfaces;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace BanqueTardiApp.Services.Implementations
{
    public class DbInitialisateurService : IDbInitalisateurService
    {
        private readonly BanqueContexte? _contexte;
        private readonly IClientService _clientService;
        private readonly ICompteBancaireService _compteBancaireService;
        private readonly ITransactionService _transactionService;

        public DbInitialisateurService(BanqueContexte contexte, IClientService clientService, ICompteBancaireService compteBancaireService, ITransactionService transactionService)
        {
            _contexte = contexte;
            _clientService = clientService;
            _compteBancaireService = compteBancaireService;
            _transactionService = transactionService;

        }
        public async Task InitialiserAsync()
        {
            if (_contexte == null)
            {
                throw new InvalidOperationException("Le contexte de base de données n'a pas été correctement initialisé.");
            }

            await _contexte.Database.EnsureCreatedAsync();

            if (await _contexte.Clients.AnyAsync())
            {
                Console.WriteLine("La liste de clients a déjà été initialisée.");
                return;
            }

            var clients = new List<Client>();

            var faker = new Faker("fr"); 

            for (int i = 0; i < 10; i++)
            {
                var client = await _clientService.CreerNouveauClientAsync();

                client.Nom = faker.Name.LastName();
                client.Prenom = faker.Name.FirstName();
                client.DateNaissance = faker.Date.Past(50, DateTime.Now.AddYears(-15));
                client.Adresse = faker.Address.StreetAddress();
                client.CodePostal = faker.Address.ZipCode("H#A 1A#");
                client.Telephone = faker.Phone.PhoneNumber("514-###-####");    
                    
                var age = DateTime.Today.Year - client.DateNaissance.Year;
                if (client.DateNaissance > DateTime.Today.AddYears(-age)) age--;

                if (age >= 15 && age < 18)
                {
                    client.NomParent = faker.Name.FullName();
                    client.TelephoneParent = faker.Phone.PhoneNumber("514-###-####");
                }
                await _clientService.EnregistrerNouveauClientAsync(client);
            }

            Random random = new Random();

            foreach (var client in _contexte.Clients)
            {
                int nombreComptes = random.Next(1, 6);

                for (int i = 0; i < nombreComptes; i++)
                {
                    var compteBancaire = _compteBancaireService.CreerNouveauCompte(client.Code);

                    compteBancaire.TypeCompte = (TypeCompte)random.Next(0, 3);
                    compteBancaire.Solde = random.Next(1000, 50000);
                    compteBancaire.Client = client;

                    await _compteBancaireService.EnregistrerNouveauCompteAsync(compteBancaire);
                }
            }

            foreach (var compte in _contexte.ComptesBancaires)
            {
                int nombreTransactions = random.Next(5, 50);

                for (int i = 0; i < nombreTransactions; i++)
                {
                    
                    var transaction = await _transactionService.CreerNouvelleTransactionAsync(compte.NumeroCompte);

                    transaction.Libelle = faker.Lorem.Words(3)[0];
                    transaction.Montant = faker.Random.Number(1, 1000);
                    transaction.TypeOperation = (TypeOperation)random.Next(0, 2);

                    await _transactionService.EnregistrerNouvelleTransactionAsync(transaction);
                }
            }

            await _contexte.SaveChangesAsync();
        }

        public async Task ReinitialiserAsync()
        {
            await _contexte.Database.EnsureDeletedAsync();
            await InitialiserAsync();
        }
    }
}
