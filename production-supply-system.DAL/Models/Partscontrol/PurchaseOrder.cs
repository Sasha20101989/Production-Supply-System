using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о заказе поставщику(закупке товаров)
    /// </summary>
    [Table("tbd_Purchase_Orders", Schema = "Partscontrol")]
    public class PurchaseOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Purchase_Order_Id")]
        public int PurchaseOrderId { get; set; }

        [Required]
        [Column("Order_Type_Id")]
        public int OrderTypeId { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("Purchase_Order_Number")]
        public string PurchaseOrderNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("Purchase_Order_Date")]
        public DateTime PurchaseOrderDate { get; set; }

        [ForeignKey("Order_Type_Id")]
        public virtual TypeOfOrder TypeOfOrder { get; set; }
    }
}
