using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о грузоотправителе.
    /// </summary>
    [Table("tbd_Shippers", Schema = "Inbound")]
    public class Shipper : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Shipper Name is required.")]
        [MaxLength(20, ErrorMessage = "Shipper Name must not exceed 20 characters.")]
        [Column("Shipper_Name")]
        public string ShipperName { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Shipper Full Name must not exceed 100 characters.")]
        [Column("Shipper_Full_Name")]
        public string? ShipperFullName { get; set; }

        [Column("Shipper_Default_Delivery_Location_Id")]
        public int? ShipperDefaultDeliveryLocationId { get; set; }

        [ForeignKey("ShipperDefaultDeliveryLocationId")]
        public virtual Location ShipperDefaultDeliveryLocation { get; set; }
    }
}
