using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Contracts;
using DAL.Models.Inbound;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе транспорта.
    /// </summary>
    [Table("tbd_Types_Of_Transport", Schema = "Inbound")]
    public class TypesOfTransport : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Transport_Type_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Transport Type is required.")]
        [StringLength(10, ErrorMessage = "The length of Transport Type should not exceed 10 characters.")]
        [Display(Name = "Transport Type")]
        [Column("Transport_Type")]
        public string TransportType { get; set; } = null!;

        [Required(ErrorMessage = "Transport Document Name is required.")]
        [StringLength(10, ErrorMessage = "The length of Transport Document Name should not exceed 10 characters.")]
        [Display(Name = "Transport Document Name")]
        [Column("Transport_Document_Name")]
        public string TransportDocumentName { get; set; } = null!;
    }
}
