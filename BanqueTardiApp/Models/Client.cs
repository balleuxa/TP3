using BanqueTardiApp.Validations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BanqueTardiApp.Models
{
    public class Client
    {
        public Client() 
        {
            Comptes = new List<CompteBancaire>();
            NbDecouverts = 0;
        }
        [Key]
        public required string Code { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [MaxLength(150)]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        [MaxLength(150)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "La date de naissance est requise.")]
        [AgeMinimum(15, ErrorMessage = "L'âge minimum requis est 15 ans.")]
        [Display(Name = "Date de naissance")]
        public DateTime DateNaissance { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "L'adresse est requise.")]
        [MaxLength(250)]
        [Display(Name = "Adresse")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Le code postal est requis.")]
        [ZipCodeCA(ErrorMessage = "Le code postal doit être au format A1A 1A1.")]
        [Display(Name = "Code postal")]
        public string CodePostal { get; set; }

        [Required(ErrorMessage = "Le téléphone est requis.")]
        [Phone(ErrorMessage = "Ce numéro de téléphone n'est pas valide.")]
        [Display(Name = "Téléphone")]
        public string Telephone { get; set; }

        [Display(Name = "Nombre de comptes découverts")]
        public int NbDecouverts { get; set; }
        public ICollection<CompteBancaire>? Comptes { get; set; }

        [MaxLength(150)]
        [RequisPourMineur(ErrorMessage = "Le nom du parent est requis.")]
        [Display(Name = "Nom du parent")]
        public string? NomParent { get; set; }
        
        [RequisPourMineur(ErrorMessage = "Le téléphone du parent est requis.")]
        [Phone(ErrorMessage = "Ce numéro de téléphone n'est pas valide.")]
        [Display(Name = "Téléphone du parent")]
        public string? TelephoneParent { get; set; }
    }

}
