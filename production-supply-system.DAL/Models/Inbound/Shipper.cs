using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о грузоотправителе.
    /// </summary>
    [Table("tbd_Shippers", Schema = "Inbound")]
    public class Shipper
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipperId { get; set; }

        [Required]
        [MaxLength(20)]
        [ConcurrencyCheck]
        [Column("Shipper_Name")]
        public string ShipperName { get; set; }

        [MaxLength(100)]
        [ConcurrencyCheck]
        [Column("Shipper_Full_Name")]
        public string? ShipperFullName { get; set; }

        public int? ShipperDefaultDeliveryLocationId { get; set; }

        [ForeignKey("Shipper_Default_Delivery_Location_Id")]
        public virtual Location Location { get; set; }
    }
}
