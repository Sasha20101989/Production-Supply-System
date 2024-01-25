using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе грузоместа.
    /// </summary>
    [Table("tbd_Types_Of_Packing", Schema = "Inbound")]
    public class TypeOfPacking
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Packing_Type_Id")]
        public int PackingTypeId { get; set; }

        [Column("Packing_Type_Id")]
        public string SupplierPackingType { get; set; }

        [Column("Packing_Type")]
        public string PackingType { get; set; }
    }
}
