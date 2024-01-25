using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Docmapper
{
    /// <summary>
    /// Представляет информацию о колонке.
    /// </summary>
    [Table("tbd_Docmapper_Columns", Schema = "Docmapper")]
    public class DocumentColumn
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Docmapper_Column_Id")]
        public int DocmapperColumnId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Element_Name")]
        public string ElementName { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("System_Column_Name")]
        public string SystemColumnName { get; set; }
    }
}
