using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using production_supply_system.EntityFramework.DAL.Models.MasterSchema;
using production_supply_system.EntityFramework.DAL.Models.UsersSchema;

namespace production_supply_system.EntityFramework.DAL.Models.dboSchema;

[Table("tbd_Sections")]
public partial class Section
{
    [Key]
    [Column("Section_Id")]
    public int SectionId { get; set; }

    [Required(ErrorMessage = "Section Name is required.")]
    [MaxLength(300)]
    [Column("Section_Name")]
    public string SectionName { get; set; } = null!;

    [InverseProperty("Section")]
    public virtual ICollection<ProcessesStep> ProcessesSteps { get; set; } = new List<ProcessesStep>();

    [InverseProperty("Section")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}