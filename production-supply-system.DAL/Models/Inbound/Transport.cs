using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о транспорте.
    /// </summary>
    [Table("tbd_Transports", Schema = "Inbound")]
    public class Transport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransportId { get; set; }

        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        [Column("Transport_Name")]
        public string TransportName { get; set; }
    }
}
