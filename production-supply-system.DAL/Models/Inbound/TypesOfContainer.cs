using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе контейнера.
    /// </summary>
    [Table("tbd_Types_Of_Container", Schema = "Inbound")]
    public class TypesOfContainer : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Container_Type_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Container Type is required.")]
        [MaxLength(10, ErrorMessage = "Container Type must not exceed 10 characters.")]
        [Column("Container_Type")]
        public string ContainerType { get; set; }
    }
}

