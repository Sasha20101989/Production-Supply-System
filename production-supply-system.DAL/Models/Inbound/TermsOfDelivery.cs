using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию об условиях поставки по Internation comercial terms
    /// </summary>
    [Table("tbd_Terms_Of_Delivery", Schema = "Inbound")]
    public class TermsOfDelivery : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Delivery_Terms_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Delivery Term is required.")]
        [MaxLength(5, ErrorMessage = "Delivery Term must not exceed 100 characters.")]
        [Column("Delivery_Term")]
        public string DeliveryTerm { get; set; }
    }
}
