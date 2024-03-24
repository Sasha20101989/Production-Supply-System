using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.Models
{
    public class PartPrice
    {
        [Column("Part_Number")]
        public string PartNumber { get; set; }

        [Column("Price")]
        public decimal Price { get; set; }
    }
}
