using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию об условиях поставки по Internation comercial terms
    /// </summary>
    [Table("tbd_Terms_Of_Delivery", Schema = "Inbound")]
    public class TermsOfDelivery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Delivery_Terms_Id")]
        public int DeliveryTermsId { get; set; }

        [Required]
        [MaxLength(5)]
        [Column("Delivery_Term")]
        public string DeliveryTerm { get; set; }
    }
}
