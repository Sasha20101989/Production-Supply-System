using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Enums;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе локации.
    /// </summary>
    [Table("tbd_Types_Of_Location", Schema = "Inbound")]
    public class TypesOfLocation : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Location Type is required.")]
        [MaxLength(20, ErrorMessage = "The length of Location Type should not exceed 20 characters.")]
        [Display(Name = "Location Type")]
        [Column("Location_Type")]
        public string LocationType { get; set; }
    }
}
