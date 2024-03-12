using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о перевозчике.
    /// </summary>
    [Table("tbd_Carriers", Schema = "Inbound")]
    public class Carrier : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Carrier_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Carrier Name is required.")]
        [MaxLength(50)]
        [ConcurrencyCheck]
        [Column("Carrier_Name")]
        public string CarrierName { get; set; }
    }
}
