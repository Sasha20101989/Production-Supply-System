using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Models.Contracts;

namespace DAL.Models.Document
{
    /// <summary>
    /// Представляет информацию о колонке.
    /// </summary>
    [Table("tbd_Docmapper_Columns", Schema = "Docmapper")]
    public class DocmapperColumn : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Docmapper_Column_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Element Name is required.")]
        [MaxLength(50, ErrorMessage = "Element Name must not exceed 50 characters.")]
        [Column("Element_Name")]
        public string ElementName { get; set; } = null!;

        [Required(ErrorMessage = "System Column Name is required.")]
        [MaxLength(50, ErrorMessage = "System Column Name must not exceed 50 characters.")]
        [Column("System_Column_Name")]
        public string SystemColumnName { get; set; } = null!;
    }
}
