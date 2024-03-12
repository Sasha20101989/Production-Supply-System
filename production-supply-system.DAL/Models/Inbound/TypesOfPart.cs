using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Enums;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о таможеном обозначении детали.
    /// </summary>
    [Table("tbd_Customs_Parts", Schema = "Inbound")]
    public class TypesOfPart : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Part_Type_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Part Type is required.")]
        [MaxLength(10, ErrorMessage = "The length of Part Type should not exceed 10 characters.")]
        [Display(Name = "Part Type")]
        [Column("Part_Type")]
        public PartTypes PartType { get; set; }
    }
}
