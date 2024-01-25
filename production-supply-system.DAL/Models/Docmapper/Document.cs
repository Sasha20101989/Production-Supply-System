using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Attributes;

namespace DAL.Models.Docmapper
{
    /// <summary>
    /// Представляет информацию о документе.
    /// </summary>
    [Table("tbd_Docmapper", Schema = "Docmapper")]
    public class Document
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Docmapper_Id")]
        public int DocmapperId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Docmapper_Name")]
        [Display(Prompt = "Необходимо указать имя")]
        public string DocmapperName { get; set; }

        [MaxLength(100)]
        [Column("Default_Folder")]
        public string? DefaultFolder { get; set; }

        public virtual string? Folder { get; set; }

        public virtual object[,] Data { get; set; }

        [Required(ErrorMessage = "Это поле является обязательным для заполнения")]
        [MaxLength(50)]
        [Column("Sheet_Name")]
        public string SheetName { get; set; }

        [Required(ErrorMessage = "Это поле является обязательным для заполнения")]
        [Min(1, ErrorMessage = "Минимальное значение для этого поля, 1")]
        [Column("First_Data_Row")]
        public int FirstDataRow { get; set; }

        [Required]
        [Column("Is_Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Контент документа
        /// </summary>
        public virtual List<DocumentContent> Content { get; set; } = new();
    }
}
