using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using DAL.Attributes;
using DAL.Extensions;
using DAL.Models.Contracts;
using DAL.Models.Master;

namespace DAL.Models.Document
{
    /// <summary>
    /// Представляет информацию о документе.
    /// </summary>
    [Table("tbd_Docmapper", Schema = "Docmapper")]
    public class Docmapper : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Docmapper_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Docmapper Name is required.")]
        [MaxLength(50, ErrorMessage = "Docmapper Name must not exceed 50 characters.")]
        [Column("Docmapper_Name")]
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
        public int FirstDataRow { get; set; }

        [Display(Name = "Is Active")]
        [DefaultValue(true)]
        [Column("Is_Active")]
        public bool IsActive { get; set; }

        public virtual List<DocmapperContent> DocmapperContents { get; set; }

        public virtual string? Folder { get; set; }

        public virtual string? NgFolder { get; set; }

        public virtual object[,] Data { get; set; }

        public object GetValue(Type modelType, string nameOfProperty, int? row = null)
        {
            string systemName = modelType.GetSystemColumnName(nameOfProperty);

            DocmapperContent content = DocmapperContents
                .FirstOrDefault(dc => dc.DocmapperColumn.SystemColumnName == systemName);

            if (content != null && content.RowNr != null)
            {
                return Data.GetValue((int)content.RowNr - 1, content.ColumnNr - 1);
            }
            else if (content != null && row != null)
            {
                return Data.GetValue((int)row, content.ColumnNr - 1);
            }

            return null;
        }
    }
}