using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BanqueTardiApp.Donnees;
using BanqueTardiApp.Models;
using BanqueTardiApp.Services.Interfaces;

namespace BanqueTardiApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ICompteBancaireService _compteBancaireService;

        public TransactionsController(ITransactionService transactionService, ICompteBancaireService compteBancaireService)
        {
            _transactionService = transactionService;
            _compteBancaireService = compteBancaireService;
        }

        public async Task<IActionResult> Historique(string? noCompteBancaire, TypeOperation? typeOperation = null)
        {
            var compteBancaire = await _compteBancaireService.ObtenirCompteParNumeroAsync(noCompteBancaire);

            if (compteBancaire == null)
            {
                return NotFound($"Aucun compte bancaire avec numero {noCompteBancaire} n'a été trouvé.");
            }

            var transactions = await _transactionService.ObtenirListeTransactionsAsync(noCompteBancaire);

            if (typeOperation.HasValue)
            {
                transactions = transactions.Where(t => t.TypeOperation == typeOperation.Value).ToList();
            }

            ViewBag.ClientCode = compteBancaire.ClientCode;
            ViewBag.NumeroCompte = compteBancaire.NumeroCompte;
            ViewBag.SelectedTypeOperation = typeOperation;

            return View(transactions);
        }

        public async Task<IActionResult> Creer(string? noCompteBancaire)
        {
            var transaction = await _transactionService.CreerNouvelleTransactionAsync(noCompteBancaire);

            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creer(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var compteBancaire = await _compteBancaireService.ObtenirCompteParNumeroAsync(transaction.NumeroCompte);

                if (compteBancaire == null)
                {
                    ModelState.AddModelError("", "Le compte bancaire associé n'a pas été trouvé. Veuillez vérifier le numéro de compte.");
                    return View(transaction);
                }

                var (succes, messageErreur) = await _transactionService.EnregistrerNouvelleTransactionAsync(transaction);

                if (succes)
                {
                    return RedirectToAction("Gerer", "Clients", new { code = compteBancaire.ClientCode });
                }
                else
                {
                    ModelState.AddModelError("", messageErreur);
                }
            }

            transaction.CompteBancaire = await _compteBancaireService.ObtenirCompteParNumeroAsync(transaction.NumeroCompte);

            return View(transaction);
        }

    }
}
