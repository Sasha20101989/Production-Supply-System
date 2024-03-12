using System.ComponentModel.DataAnnotations.Schema;

using DAL.Models.Contracts;

namespace DAL.Models
{
    [Table("uvw_Bodies_Modvars", Schema = "dbo")]
    public class BodyModelVariant: IEntity
    {
        public int Id { get; set; }

        public int PartNumberId { get; set; }

        public string PartNumber { get; set; }

        public int ModelVariantId { get; set; }

        public int PartTypeId { get; set; }
    }
}
