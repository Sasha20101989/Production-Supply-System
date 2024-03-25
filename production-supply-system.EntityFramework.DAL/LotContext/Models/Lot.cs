using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Lots", Schema = "Inbound")]
public partial class Lot
{
    [Key]
    [Column("Lot_Id")]
    public int Id { get; set; }

    [Column("Lot_Number")]
    [MaxLength(20, ErrorMessage = "Lot number must not exceed 20 characters.")]
    public string? LotNumber { get; set; }

    [Column("Shipper_Id")]
    public int ShipperId { get; set; }

    [Column("Lot_Invoice_Id")]
    public int LotInvoiceId { get; set; }

    [Column("Lot_Purchase_Order_Id")]
    public int LotPurchaseOrderId { get; set; }

    [Column("Carrier_Id")]
    public int CarrierId { get; set; }

    [Column("Delivery_Terms_Id")]
    public int DeliveryTermsId { get; set; }

    [Column("Lot_Transport_Id")]
    public int? LotTransportId { get; set; }

    [Column("Lot_Transport_Type_Id")]
    public int LotTransportTypeId { get; set; }

    [Column("Lot_Transport_Document")]
    [MaxLength(50, ErrorMessage = "Lot transport document must not exceed 50 characters.")]
    public string? LotTransportDocument { get; set; }

    [Column("Lot_ETD")]
    public DateTime? LotEtd { get; set; }

    [Column("Lot_ATD")]
    public DateTime? LotAtd { get; set; }

    [Column("Lot_ETA")]
    public DateTime LotEta { get; set; }

    [Column("Lot_ATA")]
    public DateTime? LotAta { get; set; }

    [Column("Lot_Departure_Location_Id")]
    public int LotDepartureLocationId { get; set; }

    [Column("Lot_Customs_Location_Id")]
    public int? LotCustomsLocationId { get; set; }

    [Column("Lot_Arrival_Location_Id")]
    public int LotArrivalLocationId { get; set; }

    [Column("Close_Date")]
    public DateTime? CloseDate { get; set; }

    [Column("Lot_Comment")]
    [MaxLength(250, ErrorMessage = "Lot comment must not exceed 250 characters.")]
    public string? LotComment { get; set; }

    [ForeignKey("CarrierId")]
    public virtual Carrier Carrier { get; set; } = null!;

    [ForeignKey("DeliveryTermsId")]
    public virtual TermsOfDelivery DeliveryTerms { get; set; } = null!;

    [ForeignKey("LotArrivalLocationId")]
    public virtual Location LotArrivalLocation { get; set; } = null!;

    [ForeignKey("LotCustomsLocationId")]
    public virtual Location? LotCustomsLocation { get; set; }

    [ForeignKey("LotDepartureLocationId")]
    public virtual Location LotDepartureLocation { get; set; } = null!;

    [ForeignKey("LotInvoiceId")]
    public virtual Invoice LotInvoice { get; set; } = null!;

    [ForeignKey("LotPurchaseOrderId")]
    public virtual PurchaseOrder LotPurchaseOrder { get; set; } = null!;

    [ForeignKey("LotTransportId")]
    public virtual Transport? LotTransport { get; set; }

    [ForeignKey("LotTransportTypeId")]
    public virtual TypesOfTransport LotTransportType { get; set; } = null!;

    [ForeignKey("ShipperId")]
    public virtual Shipper Shipper { get; set; } = null!;

    [InverseProperty("Lot")]
    public virtual ICollection<ContainersInLot> ContainersInLots { get; set; } = [];
}