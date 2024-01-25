using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о локации.
    /// </summary>
    [Table("tbd_Locations", Schema = "Inbound")]
    public class Location
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Location_Id")]
        public int LocationId { get; set; }

        [Required]
        public int LocationTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        [Column("Location_Name")]
        public string LocationName { get; set; }
     
        [MaxLength(50)]
        [ConcurrencyCheck]
        public string? City { get; set; }

        [ForeignKey("tbd_Types_Of_Location")]
        public virtual TypeOfLocation TypeOfLocation { get; set; }
    }
}
