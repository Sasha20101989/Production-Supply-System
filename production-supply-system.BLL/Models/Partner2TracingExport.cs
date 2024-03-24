using System.ComponentModel.DataAnnotations.Schema;

using DocumentFormat.OpenXml.Wordprocessing;

using production_supply_system.EntityFramework.DAL.Enums;

namespace BLL.Models
{
    public class Partner2TracingExport
    {
        [Column("Carrier_Name")]
        public string CarrierName { get; set; }

        [Column("Cargo_Type")]
        public CargoTypes CargoType { get; set; }

        [Column("Lot_Number")]
        public string LotNumber { get; set; }

        [Column("Container_Number")]
        public string ContainerNumber { get; set; }

        [Column("Invoice_Number")]
        public string InvoiceNumber { get; set; }

        [Column("Transport_Type")]
        public string TransportType { get; set; }

        [Column("IMO_Cargo")]
        public bool? ImoCargo { get; set; }

        [Column("Seal_Number")]
        public string SealNumber { get; set; }

        [Column("Lot_ETD")]
        public string LotEtd { get; set; }

        [Column("Lot_ATD")]
        public string LotAtd { get; set; }

        [Column("Lot_Departure_Location")]
        public string LotDepartureLocation { get; set; }

        [Column("Transshipment_Port")]
        public string TransshipmentPort { get; set; }

        [Column("Transshipment_ETD")]
        public string TransshipmentEtd { get; set; }

        [Column("Transshipment_ATD")]
        public string TransshipmentAtd { get; set; }

        [Column("Transshipment_ETA")]
        public string TransshipmentEta { get; set; }

        [Column("Transshipment_ATA")]
        public string TransshipmentAta { get; set; }

        [Column("STP_Terminal")]
        public string StpTerminal { get; set; }

        [Column("STP_ETA")]
        public string StpEta { get; set; }

        [Column("STP_ATA")]
        public string StpAta { get; set; }

        [Column("Storage_Last_Free_Day")]
        public string StorageLastFreeDay { get; set; }

        [Column("Detention_Last_Free_Day")]
        public string DetentionLastFreeDay { get; set; }

        [Column("Customs_Clearance_Terminal")]
        public string CustomsClearanceTerminal { get; set; }

        [Column("Customs_Clearance_ETA")]
        public string CustomsClearanceEta { get; set; }

        [Column("Customs_Clearance_ATA")]
        public string CustomsClearanceAta { get; set; }

        [Column("Docs_To_Customs_Date")]
        public string DocstoCsbDate { get; set; }

        [Column("Container_Yard_ATA")]
        public string ContainerYardAta { get; set; }

        [Column("Container_Yard_ATD")]
        public string ContainerYardAtd { get; set; }

        [Column("ATA_NMGR")]
        public string AtaNmgr { get; set; }

        [Column("Target_ETA")]
        public string TargetEta { get; internal set; }

        [Column("CI_OnTheWay")]
        public bool CiOnTheWay { get; set; }

        [Column("Comment")]
        public string Comment { get; set; }
    }
}
