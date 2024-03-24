using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Terms_Of_Delivery", Schema = "Inbound")]
public partial class TermsOfDelivery
{
    [Key]
    [Column("Delivery_Terms_Id")]
    public int DeliveryTermsId { get; set; }

    [Column("Delivery_Term")]
    [MaxLength(5, ErrorMessage = "DeliveryTerm must not exceed 5 characters.")]
    public string? DeliveryTerm { get; set; }
}