using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using production_supply_system.EntityFramework.DAL.MasterProcessContext.Models;
using production_supply_system.EntityFramework.DAL.Models.dboSchema;

namespace production_supply_system.EntityFramework.DAL.Models.UsersSchema;

[Table("tbd_Users", Schema = "Users")]
[Index("Account", Name = "UQ_tbd_Users_Account", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Account is required.")]
    [StringLength(10, ErrorMessage = "The length of Account should not exceed 10 characters.")]
    public string Account { get; set; } = null!;

    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(30, ErrorMessage = "The length of Name should not exceed 30 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Surname is required.")]
    [MaxLength(30, ErrorMessage = "The length of Surname should not exceed 30 characters.")]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Patronymic is required.")]
    [MaxLength(30, ErrorMessage = "The length of Patronymic should not exceed 30 characters.")]
    public string Patronymic { get; set; } = null!;

    [Required(ErrorMessage = "Section Id is required.")]
    [Column("Section_Id")]
    public int SectionId { get; set; }

    [ForeignKey("SectionId")]
    public virtual Section Section { get; set; } = null!;

    [NotMapped]
    public string? Photo { get; set; }
}