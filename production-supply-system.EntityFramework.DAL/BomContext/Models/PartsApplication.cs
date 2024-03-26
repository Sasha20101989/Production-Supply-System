using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.BomContext.Models;

[Table("tbd_PartsApplication")]
public partial class PartsApplication
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ModelID")]
    public int? ModelId { get; set; }

    [Column("PartID")]
    public int PartId { get; set; }

    [Column(TypeName = "decimal(6, 3)")]
    public decimal? Quantity { get; set; }

    [StringLength(100)]
    public string? MtcCriteria { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Adopt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Abolish { get; set; }

    public int? LineNr { get; set; }

    [Column("PrevLineID")]
    public int? PrevLineId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateAdd { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReleaseDate { get; set; }

    [StringLength(150)]
    public string? Comment { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("PartsApplications")]
    public virtual Model? Model { get; set; }

    [ForeignKey("PartId")]
    public virtual Part? Part { get; set; }

    [InverseProperty("PartsApplication")]
    public virtual ICollection<BomProduction> BomProductions { get; set; } = [];

    [InverseProperty("PartApplication")]
    public virtual ICollection<MtcForPartsApplication> MtcForPartsApplications { get; set; } = [];
}