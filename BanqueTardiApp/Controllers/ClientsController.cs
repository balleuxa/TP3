using BanqueTardiApp.Donnees;
using BanqueTardiApp.Models;
using BanqueTardiApp.Services.Implementations;
using BanqueTardiApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BanqueTardiApp.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICompteBancaireService _compteBancaireService;
        private readonly ITransactionService _transactionService;
        public ClientsController(IClientService clientService, ICompteBancaireService compteBancaireSerivce, ITransactionService transactionService)
        {
            _clientService = clientService;
            _compteBancaireService = compteBancaireSerivce;
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index(string? nomRechercher)
        {
            ViewData["Filtre"] = nomRechercher;

            var clients = await _clientService.RechercherListeClientsAsync(nomRechercher);

            return View(clients);
        }

        public async Task<IActionResult> Gerer(string? code)
        {
            var client = await _clientService.ObtenirClientParCodeAsync(code);

            var comptesBancaires = await _compteBancaireService.ObtenirListeComptesAsync(code);

            foreach (var compte in comptesBancaires)
            {
                compte.Transactions = await _transactionService.ObtenirDernieresTransactionsAsync(compte.NumeroCompte, 10);
            }

            if (client == null)
            {
                return NotFound($"Aucun client avec le code {code} n'a été trouvé.");
            }

            return View(client);
        }

        public async Task<IActionResult> Creer()
        {
            var client = await _clientService.CreerNouveauClientAsync();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creer(Client client)
        {
            if (ModelState.IsValid)
            {
                bool resultat = await _clientService.EnregistrerNouveauClientAsync(client);

                if (resultat)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de l'enregistrement du client.");
                }
            }

            return View(client);
        }

        public async Task<IActionResult> FicheDetaillee(string? code)
        {
            var client = await _clientService.ObtenirClientParCodeAsync(code);

            if (client == null)
            {
                return NotFound($"Impossible de trouver le client {code}");
            }

            return View(client);
        }

        public async Task<IActionResult> Modifier(string? code)
        {
            var client = await _clientService.ObtenirClientParCodeAsync(code);

            if (client == null)
            {
                return NotFound($"Impossible de trouver le client {code}");
            }

            return View(client);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(string? code, Client client)
        {
            if (ModelState.IsValid)
            {
                bool resultat = await _clientService.ModifierClientAsync(code, client);

                if (resultat)
                {
                    return RedirectToAction(nameof(Gerer), new {code = client.Code});
                }
                else
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de la modification du client.");
                }
            }

            return View(client);
        }

        public async Task<IActionResult> Supprimer(string? code)
        {
            var client = await _clientService.ObtenirClientParCodeAsync(code);

            if (client == null)
            {
                return NotFound($"Impossible de trouver le client {code}");
            }

            return View(client);
        }

        [HttpPost, ActionName("Supprimer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmerSuppression(string? code, Client client)
        {
            bool resultat = await _clientService.SupprimerClientAsync(code, client);

            if (resultat)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Une erreur est survenue lors de la suppression du client.");
                var clientASupprimer = await _clientService.ObtenirClientParCodeAsync(client?.Code);
                if (clientASupprimer == null)
                {
                    return NotFound();
                }
                return View("Supprimer", clientASupprimer);
            }
        }
    }
}
