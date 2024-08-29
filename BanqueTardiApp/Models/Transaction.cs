using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanqueTardiApp.Models
{
    public class Transaction
    {
        public Transaction()
        {
            DateOperation = DateTime.Now;
        }

        public Transaction(string noCompte) : this()
        {
            NumeroCompte = noCompte ?? throw new ArgumentNullException(nameof(noCompte), "Le numéro de compte ne peut pas être null.");
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Identifiant { get; set; }

        [Display(Name = "Type d'opération")]
        public TypeOperation TypeOperation { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à zéro.")]
        public decimal Montant { get; set; }

        [Display(Name = "Date d'opération")]
        public DateTime DateOperation { get; set; }

        [Required(ErrorMessage = "Le libellé est obligatoire.")]
        [Display(Name = "Libellé")]
        public string Libelle { get; set; }
        public string NumeroCompte { get; set; }
        public CompteBancaire? CompteBancaire { get; set; }
    }
    public enum TypeOperation
    {
        [Display(Name = "Débit")]
        Debit,

        [Display(Name = "Crédit")]
        Credit
    }
}
