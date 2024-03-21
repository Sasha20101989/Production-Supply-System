using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Models.Contracts;
using DAL.Models.Master;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о размещении на предприятии.
    /// </summary>
    [Table("tbd_Sections", Schema = "dbo")]
    public class Section : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Section Name is required.")]
        [MaxLength(300)]
        [Column("Section_Name")]
        public string SectionName { get; set; } = null!;
    }
}
