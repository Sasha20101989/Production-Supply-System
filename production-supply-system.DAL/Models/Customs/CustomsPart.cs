using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о таможеном обозначении детали.
    /// </summary>
    [Table("tbd_Customs_Parts", Schema = "Customs")]
    public class CustomsPart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Part_Number_Id")]
        public int PartNumberId { get; set; }

        [Required(ErrorMessage = "Поле 'Part_Number' обязательно для заполнения.")]
        [Column("Part_Number")]
        [MaxLength(50, ErrorMessage = "Максимальная длина 'Part_Number' - 50 символов.")]
        public string PartNumber { get; set; }

        [Required(ErrorMessage = "Поле 'Part_Name_Eng' обязательно для заполнения.")]
        [Column("Part_Name_Eng")]
        [MaxLength(150, ErrorMessage = "Максимальная длина 'Part_Name_Eng' - 150 символов.")]
        public string PartNameEng { get; set; }

        [Column("Part_Name_Rus")]
        [MaxLength(ErrorMessage = "Максимальная длина 'Part_Name_Rus' - 50 символов.")]
        public string? PartNameRus { get; set; }

        [Column("Hs_Code")]
        [MaxLength(10, ErrorMessage = "Максимальная длина 'Hs_Code' - 10 символов.")]
        public string? HsCode { get; set; }

        [Column("Part_Type_Id")]
        public int? PartTypeId { get; set; }

        [ForeignKey("Part_Type_Id")]
        public TypeOfPart TypeOfPart { get; set; }
    }
}
