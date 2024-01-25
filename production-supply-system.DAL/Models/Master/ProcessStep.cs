using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Docmapper;

namespace DAL.Models.Master
{
    /// <summary>
    /// Представляет информацию о шаге в процессе.
    /// </summary>
    [Table("tbd_Processes_Steps", Schema = "Master")]
    public class ProcessStep
    {
        [Key]
        public int ProcessStepId { get; set; }

        [Required]
        public int ProcessId { get; set; }

        [Required]
        public int Step { get; set; }

        [Required]
        public int DocmapperId { get; set; }

        [Required]
        public int SectionId { get; set; }

        [ForeignKey("Process_Id")]
        public virtual Process Process { get; set; }

        [ForeignKey("Docmapper_Id")]
        public virtual Document Document { get; set; }

        [ForeignKey("Section_Id")]
        public virtual Section Section { get; set; }
    }
}
