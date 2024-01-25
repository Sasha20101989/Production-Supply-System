using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе заказа(IPO, SPO, KD)
    /// </summary>
    [Table("tbd_Purchase_Orders", Schema = "Partscontrol")]
    public class TypeOfOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Order_Type_Id")]
        public int OrderTypeId { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("Order_Type")]
        public string OrderType { get; set; }
    }
}
