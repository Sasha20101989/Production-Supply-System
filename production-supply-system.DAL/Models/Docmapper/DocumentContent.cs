using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Attributes;

namespace DAL.Models.Docmapper
{
    /// <summary>
    /// Представляет информацию о расположении контента в документе.
    /// </summary>
    [Table("tbd_Docmapper_Content", Schema = "Docmapper")]
    public class DocumentContent
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Docmapper_Content_Id")]
        public int DocmapperContentId { get; set; }

        [Required]
        [Column("Docmapper_Id")]
        public int DocmapperId { get; set; }

        [Required]
        [Column("Docmapper_Column_Id")]
        public int DocmapperColumnId { get; set; }

        [Min(1)]
        [Column("Row_Number")]
        public int? RowNumber { get; set; }

        [Required]
        [Min(1)]
        [Column("Column_Number")]
        public int ColumnNumber { get; set; }

        [ForeignKey("Docmapper_Id")]
        public virtual Document Document { get; set; }

        [ForeignKey("Docmapper_Column_Id")]
        public virtual DocumentColumn DocumentColumn { get; set; }
    }
}
