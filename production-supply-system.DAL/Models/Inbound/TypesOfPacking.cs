using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе грузоместа.
    /// </summary>
    [Table("tbd_Types_Of_Packing", Schema = "Inbound")]
    public class TypesOfPacking : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Packing_Type_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Supplier Packing Type is required.")]
        [StringLength(150, ErrorMessage = "The length of Supplier Packing Type should not exceed 150 characters.")]
        [Display(Name = "Supplier Packing Type")]
        [Column("Supplier_Packing_Type")]
        public string SupplierPackingType { get; set; }

        [Required(ErrorMessage = "Packing Type is required.")]
        [StringLength(150, ErrorMessage = "The length of Packing Type should not exceed 150 characters.")]
        [Display(Name = "Packing Type")]
        [Column("Packing_Type")]
        public string PackingType { get; set; }
    }
}
