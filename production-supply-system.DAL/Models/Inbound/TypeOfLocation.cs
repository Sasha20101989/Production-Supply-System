using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе локации.
    /// </summary>
    [Table("tbd_Types_Of_Location", Schema = "Inbound")]
    public class TypeOfLocation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationTypeId { get; set; }

        [Required]
        [MaxLength(20)]
        [ConcurrencyCheck]
        [Column("Location_Type")]
        public string LocationType { get; set; }
    }
}
