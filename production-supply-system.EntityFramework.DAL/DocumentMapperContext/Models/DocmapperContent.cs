using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using production_supply_system.EntityFramework.DAL.Attributes;

namespace production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

[Table("tbd_Docmapper_Content", Schema = "Docmapper")]
public partial class DocmapperContent
{
    [Key]
    [Column("Docmapper_Content_Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Docmapper Id is required.")]
    [Column("Docmapper_Id")]
    public int DocmapperId { get; set; }

    [Required(ErrorMessage = "Docmapper Column Id is required.")]
    [Column("Docmapper_Column_Id")]
    public int DocmapperColumnId { get; set; }

    [Display(Name = "Row Number")]
    [Column("Row_Nr")]
    [Min(1, ErrorMessage = "Minimum value is 1")]
    public int? RowNr { get; set; }

    [Required(ErrorMessage = "Column Number is required.")]
    [Display(Name = "Column Number")]
    [Column("Column_Nr")]
    [Min(1, ErrorMessage = "Minimum value is 1")]
    public int ColumnNr { get; set; }

    [ForeignKey("DocmapperColumnId")]
    public virtual DocmapperColumn DocmapperColumn { get; set; } = null!;
}