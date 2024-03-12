using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Attributes;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о лоте.
    /// </summary>
    [Table("tbd_Lots", Schema = "Inbound")]
    public class Lot : IEntity
    {
        private Invoice _lotInvoice;
        private Carrier _carrier;
        private TermsOfDelivery _deliveryTerms;
        private Location _lotArrivalLocation;
        private Location _lotCustomsLocation;
        private Location _lotDepartureLocation;
        private PurchaseOrder _lotPurchaseOrder;
        private TypesOfTransport _lotTransportType;
        private Transport _lotTransport;
        private Shipper _shipper;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Lot_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lot Number is required.")]
        [MaxLength(20, ErrorMessage = "Lot Number must not exceed 20 characters.")]
        [Column("Lot_Number")]
        public string LotNumber { get; set; } = "-";


        [MaxLength(50, ErrorMessage = "Transport Document must not exceed 50 characters.")]
        [Column("Lot_Transport_Document")]
        public string? LotTransportDocument { get; set; }

        [DataType(DataType.Date)]
        [Column("Lot_ETD")]
        public DateTime? LotEtd { get; set; }

        [DataType(DataType.Date)]
        [Column("Lot_ATD")]
        public DateTime? LotAtd { get; set; }

        [Required(ErrorMessage = "Lot Expected time Arrival is required.")]
        [DataType(DataType.Date)]
        [Column("Lot_ETA")]
        public DateTime? LotEta { get; set; }

        [DataType(DataType.Date)]
        [Column("Lot_ATA")]
        public DateTime? LotAta { get; set; }

        [Column("Close_Date")]
        [DataType(DataType.Date)]
        public DateTime? CloseDate { get; set; }

        [MaxLength(250, ErrorMessage = "Lot Comment must not exceed 250 characters.")]
        [Column("Lot_Comment")]
        public string? LotComment { get; set; }

        [Required(ErrorMessage = "Invoice Id is required.")]
        [Column("Lot_Invoice_Id")]
        [Min(1)]
        public int LotInvoiceId { get; set; }

        [ForeignKey("LotInvoiceId")]
        public virtual Invoice LotInvoice
        {
            get => _lotInvoice;
            set
            {
                _lotInvoice = value;
                LotInvoiceId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Carrier Id is required.")]
        [Column("Carrier_Id")]
        [Min(1)]
        public int CarrierId { get; set; }

        [ForeignKey("CarrierId")]
        public virtual Carrier Carrier
        {
            get => _carrier;
            set
            {
                _carrier = value;
                CarrierId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Delivery Terms Id is required.")]
        [Column("Delivery_Terms_Id")]
        [Min(1)]
        public int DeliveryTermsId { get; set; }

        [ForeignKey("DeliveryTermsId")]
        public virtual TermsOfDelivery DeliveryTerms
        {
            get => _deliveryTerms;
            set
            {
                _deliveryTerms = value;
                DeliveryTermsId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Arrival Location Id is required.")]
        [Column("Lot_Arrival_Location_Id")]
        [Min(1)]
        public int LotArrivalLocationId { get; set; }

        [ForeignKey("LotArrivalLocationId")]
        public virtual Location LotArrivalLocation
        {
            get => _lotArrivalLocation;
            set
            {
                _lotArrivalLocation = value;
                LotArrivalLocationId = value?.Id ?? 0;
            }
        }

        [Column("Lot_Customs_Location_Id")]
        public int? LotCustomsLocationId { get; set; }

        [ForeignKey("LotCustomsLocationId")]
        public virtual Location LotCustomsLocation
        {
            get => _lotCustomsLocation;
            set
            {
                _lotCustomsLocation = value;
                LotCustomsLocationId = value?.Id ?? null;
            }
        }

        [Required(ErrorMessage = "Departure Location Id is required.")]
        [Column("Lot_Departure_Location_Id")]
        [Min(1)]
        public int LotDepartureLocationId { get; set; }

        [ForeignKey("LotDepartureLocationId")]
        [Column("Lot_Departure_Location")]
        public virtual Location LotDepartureLocation
        {
            get => _lotDepartureLocation;
            set
            {
                _lotDepartureLocation = value;
                LotDepartureLocationId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Purchase Order Id is required.")]
        [Column("Lot_Purchase_Order_Id")]
        [Min(1)]
        public int LotPurchaseOrderId { get; set; }

        [ForeignKey("LotPurchaseOrderId")]
        public virtual PurchaseOrder LotPurchaseOrder
        {
            get => _lotPurchaseOrder;
            set
            {
                _lotPurchaseOrder = value;
                LotPurchaseOrderId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Transport Type Id is required.")]
        [Column("Lot_Transport_Type_Id")]
        [Min(1)]
        public int LotTransportTypeId { get; set; }

        [ForeignKey("LotTransportTypeId")]
        public virtual TypesOfTransport LotTransportType
        {
            get => _lotTransportType;
            set
            {
                _lotTransportType = value;
                LotTransportTypeId = value?.Id ?? 0;
            }
        }

        [Column("Lot_Transport_Id")]
        public int? LotTransportId { get; set; }

        [ForeignKey("LotTransportId")]
        public virtual Transport LotTransport
        {
            get => _lotTransport;
            set
            {
                _lotTransport = value;
                LotTransportId = value?.Id ?? null;
            }
        }

        [Required(ErrorMessage = "Shipper Id is required.")]
        [Column("Shipper_Id")]
        [Min(1)]
        public int ShipperId { get; set; }

        [ForeignKey("ShipperId")]
        public virtual Shipper Shipper
        {
            get => _shipper;
            set
            {
                _shipper = value;
                ShipperId = value?.Id ?? 0;
            }
        }
    }
}
