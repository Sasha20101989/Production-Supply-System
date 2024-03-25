using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_VINs_In_Container", Schema = "Planning")]
public partial class VinsInContainer
{
    [Key]
    [Column("VIN_In_Container_Id")]
    public int VinInContainerId { get; set; }

    [Column("Container_In_Lot_Id")]
    public int ContainerInLotId { get; set; }

    [Column("Supplier_VIN_Number")]
    [MaxLength(50, ErrorMessage = "Supplier Vin Number must not exceed 50 characters.")]
    public string? SupplierVinNumber { get; set; }

    [Column("Modvar_Id")]
    public int ModvarId { get; set; }

    [Column("Lot_Id")]
    public int LotId { get; set; }

    [ForeignKey("ContainerInLotId")]
    [InverseProperty("VinsInContainers")]
    public virtual ContainersInLot ContainerInLot { get; set; } = null!;

    [InverseProperty("VinInContainer")]
    public virtual BoundVin? BoundVin { get; set; }

    [InverseProperty("VinInContainer")]
    public virtual PlannedSequence? PlannedSequence { get; set; }
}