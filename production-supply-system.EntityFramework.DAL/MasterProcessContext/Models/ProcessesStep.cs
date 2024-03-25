using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;
using production_supply_system.EntityFramework.DAL.Enums;
using production_supply_system.EntityFramework.DAL.Models.dboSchema;

namespace production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;

[Table("tbd_Processes_Steps", Schema = "Master")]
public partial class ProcessesStep
{
    [Key]
    [Column("Process_Step_Id")]
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
    public string? StepName { get; set; }

    [ForeignKey("DocmapperId")]
    public virtual Docmapper Docmapper { get; set; } = null!;

    [ForeignKey("ProcessId")]
    [InverseProperty("ProcessesSteps")]
    public virtual Process Process { get; set; } = null!;

    [ForeignKey("SectionId")]
    [InverseProperty("ProcessesSteps")]
    public virtual Section Section { get; set; } = null!;

    [NotMapped]
    public virtual Dictionary<string, CellInfo> ValidationErrors { get; set; } = [];
}