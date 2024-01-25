using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о пользователе.
    /// </summary>
    [Table("tbd_users", Schema = "Users")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Surname { get; set; }

        [MaxLength(30)]
        public string Patronymic { get; set; }

        [Required]
        [MaxLength(10)]
        public string Account { get; set; }

        [Required]
        public int SectionId { get; set; }

        [NotMapped]
        public string? Mail { get; set; }

        [NotMapped]
        public string? MobilePhone { get; set; }

        [NotMapped]
        public object Department { get; set; }

        [NotMapped]
        public string? Photo { get; set; }

        [ForeignKey("Section_Id")]
        public virtual Section Section { get; set; }
    }
}
