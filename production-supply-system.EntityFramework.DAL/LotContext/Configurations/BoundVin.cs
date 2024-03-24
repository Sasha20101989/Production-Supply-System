using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Bound_VINs", Schema = "Planning")]
[Index("VinInContainerId", Name = "IX_tbd_Bound_VINs_Uniq_VIN_In_Cont", IsUnique = true)]
[Index("VinNumberLocalId", Name = "IX_tbd_Bound_VINs_Uniq_VIN_Number_Local", IsUnique = true)]
public partial class BoundVin
{
    [Key]
    [Column("Bound_VINs_Id")]
    public int BoundVinsId { get; set; }

    [Column("VIN_In_Container_Id")]
    public int VinInContainerId { get; set; }

    [Column("VIN_Number_Local_Id")]
    public int VinNumberLocalId { get; set; }

    [ForeignKey("VinInContainerId")]
    [InverseProperty("BoundVin")]
    public virtual VinsInContainer VinInContainer { get; set; } = null!;

    [ForeignKey("VinNumberLocalId")]
    [InverseProperty("BoundVin")]
    public virtual VinNumbersLocal VinNumberLocal { get; set; } = null!;
}