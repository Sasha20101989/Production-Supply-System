using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе контейнера.
    /// </summary>
    [Table("tbd_Types_Of_Container", Schema = "Inbound")]
    public class TypeOfContainer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Container_Type_Id")]
        public int ContainerTypeId { get; set; }

        [Required]
        [MaxLength(10)]
        [ConcurrencyCheck]
        [Column("Container_Type")]
        public string ContainerType { get; set; }
    }
}
