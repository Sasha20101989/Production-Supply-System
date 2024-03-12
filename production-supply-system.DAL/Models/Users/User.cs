using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о пользователе.
    /// </summary>
    [Table("tbd_users", Schema = "Users")]
    public class User : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [NotMapped]
        public string? Photo { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section? Section { get; set; }
    }
}
