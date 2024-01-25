using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о размещении на предприятии.
    /// </summary>
    [Table("tbd_Sections", Schema = "dbo")]
    public class Section
    {
        [Key]
        public int SectionId { get; set; }

        [Required]
        [MaxLength(300)]
        public string SectionName { get; set; }
}
}
