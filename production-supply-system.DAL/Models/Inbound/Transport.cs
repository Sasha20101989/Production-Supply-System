using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Contracts;
using DAL.Models.Inbound;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о транспорте.
    /// </summary>
    [Table("tbd_Transports", Schema = "Inbound")]
    public class Transport : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Transport Name is required.")]
        [MaxLength(50, ErrorMessage = "Transport Name must not exceed 50 characters.")]
        [Column("Transport_Name")]
        public string TransportName { get; set; } = null!;

        public string TransportNameGroup => string.IsNullOrEmpty(TransportName) ? "" : TransportName.Substring(0, 1).ToUpper();
    }
}
