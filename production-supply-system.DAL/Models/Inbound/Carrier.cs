using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о перевозчике.
    /// </summary>
    [Table("tbd_Carriers", Schema = "Inbound")]
    public class Carrier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Carrier_Id")]
        public int CarrierId { get; set; }

        [Required]
        [MaxLength(50)]
        [ConcurrencyCheck]
        [Column("Carrier_Name")]
        public string CarrierName { get; set; }
    }
}
