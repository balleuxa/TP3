using System.ComponentModel.DataAnnotations;

namespace BanqueTardiApp.Models
{
    public class CompteBancaire
    {
        public CompteBancaire()
        {
            Transactions = new List<Transaction>();
        }

        public CompteBancaire(string clientCode) : this()
        {
            ClientCode = clientCode ?? throw new ArgumentNullException(nameof(clientCode), "Le code du client ne peut pas être null.");
        }

        [Key]
        public string? NumeroCompte { get; set; }
        public TypeCompte TypeCompte { get; set; }
        
        [Required(ErrorMessage = "Le solde d'ouverture est requis.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à zéro.")]
        public decimal Solde { get; set; }
        public string? ClientCode { get; set; }


        public decimal TauxInteret
        {
            get
            {
                return TypeCompte switch
                {
                    TypeCompte.Cheque => 0,
                    TypeCompte.Epargne => 2,
                    TypeCompte.Placement => 4,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public decimal TauxInteretDecouvert
        {
            get
            {
                return TypeCompte switch
                {
                    TypeCompte.Cheque => 7,
                    TypeCompte.Epargne => 0,
                    TypeCompte.Placement => 0,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        public int Identifiant
        {
            get
            {
                return TypeCompte switch
                {
                    TypeCompte.Cheque => 10,
                    TypeCompte.Epargne => 11,
                    TypeCompte.Placement => 16,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        public Client? Client { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
    public enum TypeCompte
    {

        [Display(Name = "Chèque")]
        Cheque,

        [Display(Name = "Épargne")]
        Epargne,

        [Display(Name = "Placement garanti")]
        Placement
    }
}
