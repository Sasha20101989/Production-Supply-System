using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace production_supply_system.EntityFramework.DAL.LotContext.Models;

[Table("tbd_Types_Of_Transport", Schema = "Inbound")]
public partial class TypesOfTransport
{
    [Key]
    [Column("Transport_Type_Id")]
    public int TransportTypeId { get; set; }

    [Column("Transport_Type")]
    [StringLength(10)]
    public string? TransportType { get; set; }

    [Column("Transport_Document_Name")]
    [StringLength(10)]
    public string? TransportDocumentName { get; set; }
}