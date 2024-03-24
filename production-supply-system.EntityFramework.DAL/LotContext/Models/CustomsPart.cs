using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Customs_Parts", Schema = "Customs")]
[Index("PartNumberId", Name = "IX_tbd_CustomsParts", IsUnique = true)]
public partial class CustomsPart
{
    [Key]
    [Column("Part_Number_Id")]
    public int PartNumberId { get; set; }

    [Column("Part_Number")]
    [StringLength(50)]
    public string? PartNumber { get; set; }

    [Column("Part_Name_Eng")]
    [StringLength(150)]
    public string? PartNameEng { get; set; }

    [Column("Part_Name_Rus")]
    public string? PartNameRus { get; set; }

    [Column("Hs_Code")]
    [StringLength(50)]
    public string? HsCode { get; set; }

    [Column("Part_Type_Id")]
    public int PartTypeId { get; set; }

    [Column("Date_Add", TypeName = "datetime")]
    public DateTime? DateAdd { get; set; }

    [ForeignKey("PartTypeId")]
    [InverseProperty("CustomsParts")]
    public virtual TypesOfPart PartType { get; set; } = null!;
}