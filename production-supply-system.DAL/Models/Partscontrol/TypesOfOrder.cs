using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Contracts;

namespace DAL.Models.Partscontrol
{
    [Table("tbd_Types_Of_Order", Schema = "Partscontrol")]
    public partial class TypesOfOrder : IEntity
    {
        [Key]
        [Column("Order_Type_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Order Type is required.")]
        [StringLength(10, ErrorMessage = "The length of Order Type should not exceed 10 characters.")]
        [Display(Name = "Order Type")]
        [Column("Order_Type")]
        public string OrderType { get; set; }
    }
}
