using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о таможеном обозначении детали.
    /// </summary>
    [Table("tbd_Customs_Parts", Schema = "Inbound")]
    public class TypeOfPart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Part_Type_Id")]
        public int PartTypeId { get; set; }

        [Required(ErrorMessage = "Поле 'Part_Type' обязательно для заполнения.")]
        [Column("Part_Type")]
        [MaxLength(10, ErrorMessage = "Максимальная длина 'Part_Type' - 10 символов.")]
        public string PartType { get; set; }
    }
}
