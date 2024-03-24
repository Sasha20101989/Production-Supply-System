using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.Models.dboSchema;

[Table("tbd_Log")]
public partial class Log
{
    [Key]
    [Column("Log_Id")]
    public long LogId { get; set; }

    [Column("Machine_Name")]
    [StringLength(200)]
    public string MachineName { get; set; } = null!;

    [Column("Log_Date", TypeName = "datetime")]
    public DateTime LogDate { get; set; }

    [Column("Log_Level")]
    [StringLength(5)]
    public string LogLevel { get; set; } = null!;

    [Column("Log_Message")]
    public string LogMessage { get; set; } = null!;

    [Column("Domain_User")]
    [StringLength(300)]
    public string DomainUser { get; set; } = null!;

    [StringLength(300)]
    public string Logger { get; set; } = null!;
}