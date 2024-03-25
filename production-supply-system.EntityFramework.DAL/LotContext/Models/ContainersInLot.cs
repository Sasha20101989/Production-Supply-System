using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Containers_In_Lot", Schema = "Inbound")]
public partial class ContainersInLot
{
    [Key]
    [Column("Container_In_Lot_Id")]
    public int Id { get; set; }

    [Column("Lot_Id")]
    public int LotId { get; set; }

    [Column("Container_Number")]
    [StringLength(11)]
    public string ContainerNumber { get; set; } = null!;

    [Column("Seal_Number")]
    [StringLength(20)]
    public string? SealNumber { get; set; }

    [Column("Container_Type_Id")]
    public int ContainerTypeId { get; set; }

    [Column("IMO_Cargo")]
    public bool? ImoCargo { get; set; }

    [Column("Storage_Last_Free_Day")]
    public DateOnly? StorageLastFreeDay { get; set; }

    [Column("Detention_Last_Free_Day")]
    public DateOnly? DetentionLastFreeDay { get; set; }

    [Column("Container_Comment")]
    [StringLength(250)]
    public string? ContainerComment { get; set; }

    [Column("Last_Tracing_Update")]
    public DateOnly? LastTracingUpdate { get; set; }

    [Column("CI_OnTheWay")]
    public bool CiOnTheWay { get; set; }

    [ForeignKey("ContainerTypeId")]
    public virtual TypesOfContainer ContainerType { get; set; } = null!;

    [ForeignKey("LotId")]
    [InverseProperty("ContainersInLots")]
    public virtual Lot Lot { get; set; } = null!;

    [InverseProperty("ContainerInLot")]
    public virtual ICollection<PartsInContainer> PartsInContainers { get; set; } = [];

    [InverseProperty("ContainerInLot")]
    public virtual ICollection<VinsInContainer> VinsInContainers { get; set; } = [];
}