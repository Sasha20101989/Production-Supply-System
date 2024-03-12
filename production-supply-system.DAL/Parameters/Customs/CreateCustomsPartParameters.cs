using System;
using DAL.Models;

namespace DAL.Parameters.Customs
{
    public class CreateCustomsPartParameters
    {
        public CreateCustomsPartParameters(CustomsPart entity)
        {
            PartNumberId = entity.PartNumberId;
            PartNumber = entity.PartNumber;
            PartNameEng = entity.PartNameEng;
            PartTypeId = entity.PartTypeId;
        }

        public int PartNumberId { get; set; }

        public string PartNumber { get; set; }

        public string PartNameEng { get; set; }

        public int? PartTypeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
