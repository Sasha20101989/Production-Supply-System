using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Cases", Schema = "Inbound")]
public partial class Case
{
    [Key]
    [Column("Case_Id")]
    public int CaseId { get; set; }

    [Column("Case_No")]
    [StringLength(10)]
    public string CaseNo { get; set; } = null!;

    [Column("Net_Weight", TypeName = "decimal(6, 3)")]
    public decimal NetWeight { get; set; }

    [Column("Gross_Weight", TypeName = "decimal(6, 3)")]
    public decimal GrossWeight { get; set; }

    [Column("Packing_Type_Id")]
    public int? PackingTypeId { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Length { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Width { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Height { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? Volume { get; set; }

    [ForeignKey("PackingTypeId")]
    public virtual TypesOfPacking? PackingType { get; set; }
}