using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о локации.
    /// </summary>
    [Table("tbd_Locations", Schema = "Inbound")]
    public class Location : IEntity
    {
        private TypesOfLocation _locationType;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Location_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Location Name is required.")]
        [MaxLength(50, ErrorMessage = "Location Name must not exceed 50 characters.")]
        [Column("Location_Name")]
        public string LocationName { get; set; } = null!;

        [MaxLength(50, ErrorMessage = "City must not exceed 50 characters.")]
        public string? City { get; set; } = null!;

        [Required(ErrorMessage = "Location Type Id is required.")]
        [Column("Location_Type_Id")]
        public int LocationTypeId { get; set; }

        [ForeignKey("LocationTypeId")]
        public virtual TypesOfLocation LocationType
        {
            get => _locationType;
            set
            {
                _locationType = value;
                LocationTypeId = value?.Id ?? 0;
            }
        }
    }
}