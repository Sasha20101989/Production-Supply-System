using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Enums;
using DAL.Models.Contracts;
using DAL.Models.Document;

namespace DAL.Models.Master
{
    /// <summary>
    /// Представляет информацию о шаге в процессе.
    /// </summary>
    [Table("tbd_Processes_Steps", Schema = "Master")]
    public class ProcessStep : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Process Id is required.")]
        [Column("Process_Id")]
        public int ProcessId { get; set; }

        [Required(ErrorMessage = "Step is required.")]
        [Column("Step")]
        public int Step { get; set; }

        [Required(ErrorMessage = "Docmapper Id is required.")]
        [Column("Docmapper_Id")]
        public int DocmapperId { get; set; }

        [Required(ErrorMessage = "Section Id is required.")]
        [Column("Section_Id")]
        public int SectionId { get; set; }

        [Required(ErrorMessage = "Step name is required.")]
        [Column("Step_Name")]
        [MaxLength(50, ErrorMessage = "Step name must not exceed 50 characters.")]
        public Steps StepName { get; set; }

        [ForeignKey("DocmapperId")]
        public virtual Docmapper Docmapper { get; set; } = null!;

        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; } = null!;

        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; } = null!;

        public virtual Dictionary<string, CellInfo> ValidationErrors { get; set; } = new();
    }
}
