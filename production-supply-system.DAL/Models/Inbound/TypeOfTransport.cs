using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о типе транспорта.
    /// </summary>
    [Table("tbd_Types_Of_Transport", Schema = "Inbound")]
    public class TypeOfTransport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Transport_Type_Id")]
        public int TransportTypeId { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("Transport_Type")]
        public string TransportType { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("Transport_Document_Name")]
        public string TransportDocumentName { get; set; }
    }
}
