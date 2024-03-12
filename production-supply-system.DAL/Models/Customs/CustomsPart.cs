using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Attributes;
using DAL.Enums;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о таможеном обозначении детали.
    /// </summary>
    [Table("tbd_Customs_Parts", Schema = "Customs")]
    public class CustomsPart : IEntity
    {
        private TypesOfPart _partType;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Part_Number_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Part Number is required.")]
        [MaxLength(50, ErrorMessage = "Part Number must not exceed 50 characters.")]
        [Column("Part_Number")]
        public string PartNumber { get; set; }

        [Required(ErrorMessage = "Part Name (English) is required.")]
        [MaxLength(150, ErrorMessage = "Part Name (English) must not exceed 150 characters.")]
        [Column("Part_Name_Eng")]
        public string PartNameEng { get; set; }

        [Column("Part_Name_Rus")]
        public string? PartNameRus { get; set; }

        [MaxLength(10, ErrorMessage = "HS Code must not exceed 10 characters.")]
        [Column("Hs_Code")]
        public string? HsCode { get; set; }

        [ForeignKey("PartTypeId")]
        public int PartTypeId { get; set; }

        [Column("Date_Add")]
        public DateTime CreatedAt { get; set; }

        public virtual TypesOfPart PartType
        {
            get => _partType;
            set
            {
                _partType = value;
                PartTypeId = value?.Id ?? 0;
            }
        }

        [Required(ErrorMessage = "Part Number id is required.")]
        [Min(1)]
        public int PartNumberId { get; set; }
    }
}
