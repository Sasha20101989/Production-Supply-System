using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using production_supply_system.EntityFramework.DAL.Enums;

namespace production_supply_system.EntityFramework.DAL.Models.MasterSchema;

[Table("tbd_Processes", Schema = "Master")]
public partial class Process
{
    [Key]
    [Column("Process_Id")]
    public int ProcessId { get; set; }

    [Column("Process_Name")]
    [MaxLength(300)]
    public AppProcess ProcessName { get; set; }

    [InverseProperty("Process")]
    public virtual ICollection<ProcessesStep> ProcessesSteps { get; set; } = [];
}