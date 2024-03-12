using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Contracts;
using DAL.Models.Partscontrol;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о заказе поставщику(закупке товаров)
    /// </summary>
    [Table("tbd_Purchase_Orders", Schema = "Partscontrol")]
    public class PurchaseOrder : IEntity
    {
        private Shipper _shipper;
        private TypesOfOrder _orderType;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Purchase_Order_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Purchase Order Number is required.")]
        [MaxLength(10)]
        [Column("Purchase_Order_Number")]
        public string PurchaseOrderNumber { get; set; } = null!;

        [Column("Purchase_Order_Date")]
        public DateTime PurchaseOrderDate { get; set; }

        [Required(ErrorMessage = "Order Type Id is required.")]
        [Column("Order_Type_Id")]
        public int OrderTypeId { get; set; }

        [ForeignKey("OrderTypeId")]
        public virtual TypesOfOrder OrderType
        {
            get => _orderType;
            set
            {
                _orderType = value;
                OrderTypeId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Shipper Id is required.")]
        [Column("Shipper_Id")]
        public int ShipperId { get; set; }

        [ForeignKey("ShipperId")]
        public virtual Shipper Shipper
        {
            get => _shipper;
            set
            {
                _shipper = value;
                ShipperId = value?.Id ?? 0;
            }
        }
    }
}
