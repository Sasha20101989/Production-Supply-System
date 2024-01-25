using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о лоте.
    /// </summary>
    [Table("tbd_Lots", Schema = "Inbound")]
    public class Lot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Lot_Id")]
        public int LotId { get; set; }

        [Required]
        [MaxLength(8)]
        [Column("Lot_Number")]
        public string LotNumber { get; set; }

        [Required]
        [Column("Shipper_Id")]
        public int ShipperId { get; set; }

        [Required]
        [Column("Lot_Invoice_Id")]
        public int LotInvoiceId { get; set; }

        [Required]
        [Column("Purchase_Order_Id")]
        public int PurchaseOrderId { get; set; }

        [Required]
        [Column("Carrier_Id")]
        public int CarrierId { get; set; }

        [Required]
        [Column("Delivery_Terms_Id")]
        public int DeliveryTermsId { get; set; }

        [Required]
        [Column("Lot_Transpot_Id")]
        public int LotTranspotId { get; set; }

        [Column("Lot_Transport_Type_Id")]
        public int? LotTransportTypeId { get; set; }

        [MaxLength(50)]
        [Column("Lot_Transport_Document")]
        public string? LotTransportDocument { get; set; }

        [DataType(DataType.Date)]
        [Column("Lot_ETD")]
        public DateTime? LotETD { get; set; }

        [DataType(DataType.Date)]
        [Column("Lot_ATD")]
        public DateTime? LotATD { get; set; }

        [DataType(DataType.Date)]
        [Column("Lot_ETA")]
        public DateTime? LotETA { get; set; }

        [DataType(DataType.Date)]
        [Column("Lot_ATA")]
        public DateTime? LotATA { get; set; }

        [Required]
        [Column("Lot_Departure_Location_Id")]
        public int LotDepartureLocationId { get; set; }

        [Column("Lot_Customs_Location_Id")]
        public int? LotCustomsLocationId { get; set; }

        [Required]
        [Column("Lot_Arrival_Location_Id")]
        public int LotArrivalLocationId { get; set; }

        [DataType(DataType.Date)]
        [Column("Close_Date")]
        public DateTime? CloseDate { get; set; }

        [MaxLength(250)]
        [Column("Lot_Comment")]
        public string? LotComment { get; set; }

        [NotMapped]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("Containers_Quantity")]
        public int ContainersQuantity { get; set; }


        [ForeignKey("Shipper_Id")]
        public virtual Shipper Shipper { get; set; }

        [ForeignKey("Lot_Invoice_Id")]
        public virtual Invoice Invoice { get; set; }

        [ForeignKey("Purchase_Order_Id")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        [ForeignKey("Carrier_Id")]
        public virtual Carrier Carrier { get; set; }

        [ForeignKey("Delivery_Terms_Id")]
        public virtual TermsOfDelivery TermsOfDelivery { get; set; }

        [ForeignKey("Lot_Transpot_Id")]
        public virtual Transport LotTransport { get; set; }

        [ForeignKey("Lot_Transport_Type_Id")]
        public virtual TypeOfTransport TypeOfTransport { get; set; }

        [ForeignKey("Lot_Departure_Location_Id")]
        public virtual Location LotDepartureLocation { get; set; }

        [ForeignKey("Lot_Location_Id")]
        public virtual Location LotCustomsLocation { get; set; }

        [ForeignKey("Lot_Arrival_Location_Id")]
        public virtual Location LotArrivalLocation { get; set; }
    }
}
