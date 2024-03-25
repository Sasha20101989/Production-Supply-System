using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Parts_In_Container", Schema = "Inbound")]
public partial class PartsInContainer
{
    [Key]
    [Column("Part_In_Container_Id")]
    public int Id { get; set; }

    [Column("Container_In_Lot_Id")]
    public int ContainerInLotId { get; set; }

    [Column("Case_Id")]
    public int? CaseId { get; set; }

    [Column("Part_Number_Id")]
    public int PartNumberId { get; set; }

    [Column("Quantity")]
    public int Quantity { get; set; }

    [Column("Part_Invoice_Id")]
    public int PartInvoiceId { get; set; }

    [ForeignKey("CaseId")]
    public virtual Case? Case { get; set; }

    [ForeignKey("ContainerInLotId")]
    [InverseProperty("PartsInContainers")]
    public virtual ContainersInLot ContainerInLot { get; set; } = null!;

    [ForeignKey("PartInvoiceId")]
    public virtual Invoice PartInvoice { get; set; } = null!;

    [ForeignKey("PartNumberId")]
    public virtual CustomsPart PartNumber { get; set; } = null!;
}