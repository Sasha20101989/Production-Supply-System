using System;
using DAL.Models;

namespace DAL.Parameters.Customs
{
    public class CreateCustomsPartParameters(CustomsPart entity)
    {
        public int PartNumberId { get; set; } = entity.PartNumberId;

        public string PartNumber { get; set; } = entity.PartNumber;

        public string PartNameEng { get; set; } = entity.PartNameEng;

        public int? PartTypeId { get; set; } = entity.PartTypeId;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
