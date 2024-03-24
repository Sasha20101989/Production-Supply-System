using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using production_supply_system.EntityFramework.DAL.Attributes;
using production_supply_system.EntityFramework.DAL.Extensions;

namespace production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

[Table("tbd_Docmapper", Schema = "Docmapper")]
public partial class Docmapper
{
    [Key]
    [Column("Docmapper_Id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Docmapper Name is required.")]
    [MaxLength(50, ErrorMessage = "Docmapper Name must not exceed 50 characters.")]
    [Column("Docmapper_Name")]
    [StringLength(50)]
    public string DocmapperName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Default Folder must not exceed 100 characters.")]
    [Column("Default_Folder")]
    public string? DefaultFolder { get; set; }

    [Required(ErrorMessage = "Sheet Name is required.")]
    [MaxLength(50, ErrorMessage = "Sheet Name must not exceed 50 characters.")]
    [Column("Sheet_Name")]
    public string SheetName { get; set; } = null!;

    [Required(ErrorMessage = "First Data Row is required.")]
    [Display(Name = "First Data Row")]
    [DefaultValue(1)]
    [Min(1, ErrorMessage = "Minimum value is 1")]
    [Column("First_Data_Row")]
    public int? FirstDataRow { get; set; }

    [Display(Name = "Is Active")]
    [DefaultValue(true)]
    [Column("Is_Active")]
    public bool IsActive { get; set; }

    public virtual ICollection<DocmapperContent> DocmapperContents { get; set; } = [];

    [NotMapped]
    public virtual string? Folder { get; set; }

    [NotMapped]
    public virtual string? NgFolder { get; set; }

    [NotMapped]
    public virtual object[,]? Data { get; set; }

    public object? GetValue(Type modelType, string nameOfProperty, int? row = null)
    {
        string systemName = modelType.GetSystemColumnName(nameOfProperty);

        DocmapperContent? content = DocmapperContents?.FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == systemName);

        if (content is not null && content.RowNr is not null)
        {
            return Data?.GetValue((int)content.RowNr - 1, content.ColumnNr - 1);
        }
        else if (content is not null && row is not null)
        {
            return Data?.GetValue((int)row, content.ColumnNr - 1);
        }

        return null;
    }
}