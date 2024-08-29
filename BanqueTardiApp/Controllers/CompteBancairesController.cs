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
    public class CompteBancairesController : Controller
    {
        private readonly ICompteBancaireService _compteBancaireService;

        public CompteBancairesController(ICompteBancaireService compteBancaireService)
        {
            _compteBancaireService = compteBancaireService;
        }

        public IActionResult Creer(string? clientCode)
        {
            var compte = _compteBancaireService.CreerNouveauCompte(clientCode);

            return View(compte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creer(CompteBancaire? compteBancaire)
        {
            if (ModelState.IsValid)
            {
                await _compteBancaireService.EnregistrerNouveauCompteAsync(compteBancaire);
                return RedirectToAction("Gerer", "Clients", new { code = compteBancaire?.ClientCode });

            }

            return View(compteBancaire);
        }

        public async Task<IActionResult> Modifier(string? noCompteBancaire)
        {
            var compteBancaire = await _compteBancaireService.ObtenirCompteParNumeroAsync(noCompteBancaire);

            if (compteBancaire == null)
            {
                return NotFound($"Impossible de trouver le compte {noCompteBancaire}");
            }

            ViewBag.ContientSeulementOuverture = await _compteBancaireService.PeutModifierCompteBancaire(noCompteBancaire);

            return View(compteBancaire);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(string? noCompte, CompteBancaire compteBancaire)
        {
            if (ModelState.IsValid)
            {
                bool resultat = await _compteBancaireService.ModifierCompteAsync(noCompte, compteBancaire);

                if (resultat)
                {
                    return RedirectToAction("Gerer", "Clients", new { code = compteBancaire?.ClientCode });
                }
                else
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de la modification du client.");
                }
            }

            return View(compteBancaire);
        }
    }
}
