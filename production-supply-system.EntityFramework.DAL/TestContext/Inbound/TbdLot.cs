﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using production_supply_system.EntityFramework.DAL.TestContext.CustomsSchema;
using production_supply_system.EntityFramework.DAL.TestContext.dboSchema;
using production_supply_system.EntityFramework.DAL.TestContext.DocmapperSchema;
using production_supply_system.EntityFramework.DAL.TestContext.InboundSchema;
using production_supply_system.EntityFramework.DAL.TestContext.MasterSchema;
using production_supply_system.EntityFramework.DAL.TestContext.PartscontrolSchema;
using production_supply_system.EntityFramework.DAL.TestContext.PlanningSchema;
using production_supply_system.EntityFramework.DAL.TestContext.UsersSchema;


namespace production_supply_system.EntityFramework.DAL.TestContext.InboundSchema;

[Table("tbd_Lots", Schema = "Inbound")]
public partial class TbdLot
{
    [Key]
    [Column("Lot_Id")]
    public int LotId { get; set; }

    [Column("Lot_Number")]
    [StringLength(20)]
    public string LotNumber { get; set; } = null!;

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
    [StringLength(50)]
    public string? LotTransportDocument { get; set; }

    [Column("Lot_ETD")]
    public DateOnly? LotEtd { get; set; }

    [Column("Lot_ATD")]
    public DateOnly? LotAtd { get; set; }

    [Column("Lot_ETA")]
    public DateOnly LotEta { get; set; }

    [Column("Lot_ATA")]
    public DateOnly? LotAta { get; set; }

    [Column("Lot_Departure_Location_Id")]
    public int LotDepartureLocationId { get; set; }

    [Column("Lot_Customs_Location_Id")]
    public int? LotCustomsLocationId { get; set; }

    [Column("Lot_Arrival_Location_Id")]
    public int LotArrivalLocationId { get; set; }

    [Column("Close_Date")]
    public DateOnly? CloseDate { get; set; }

    [Column("Lot_Comment")]
    [StringLength(250)]
    public string? LotComment { get; set; }

    [ForeignKey("CarrierId")]
    [InverseProperty("TbdLots")]
    public virtual TbdCarrier Carrier { get; set; } = null!;

    [ForeignKey("DeliveryTermsId")]
    [InverseProperty("TbdLots")]
    public virtual TbdTermsOfDelivery DeliveryTerms { get; set; } = null!;

    [ForeignKey("LotArrivalLocationId")]
    [InverseProperty("TbdLotLotArrivalLocations")]
    public virtual TbdLocation LotArrivalLocation { get; set; } = null!;

    [ForeignKey("LotCustomsLocationId")]
    [InverseProperty("TbdLotLotCustomsLocations")]
    public virtual TbdLocation? LotCustomsLocation { get; set; }

    [ForeignKey("LotDepartureLocationId")]
    [InverseProperty("TbdLotLotDepartureLocations")]
    public virtual TbdLocation LotDepartureLocation { get; set; } = null!;

    [ForeignKey("LotInvoiceId")]
    [InverseProperty("TbdLots")]
    public virtual TbdInvoice LotInvoice { get; set; } = null!;

    [ForeignKey("LotPurchaseOrderId")]
    [InverseProperty("TbdLots")]
    public virtual TbdPurchaseOrder LotPurchaseOrder { get; set; } = null!;

    [ForeignKey("LotTransportId")]
    [InverseProperty("TbdLots")]
    public virtual TbdTransport? LotTransport { get; set; }

    [ForeignKey("LotTransportTypeId")]
    [InverseProperty("TbdLots")]
    public virtual TbdTypesOfTransport LotTransportType { get; set; } = null!;

    [ForeignKey("ShipperId")]
    [InverseProperty("TbdLots")]
    public virtual TbdShipper Shipper { get; set; } = null!;

    [InverseProperty("Lot")]
    public virtual ICollection<TbdContainersInLot> TbdContainersInLots { get; set; } = new List<TbdContainersInLot>();
}