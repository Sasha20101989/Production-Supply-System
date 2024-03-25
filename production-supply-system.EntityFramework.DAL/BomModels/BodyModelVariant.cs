namespace production_supply_system.EntityFramework.DAL.BomModels
{
    public class BodyModelVariant
    {
        public int Id { get; set; }

        public int PartNumberId { get; set; }

        public string? PartNumber { get; set; }

        public int ModelVariantId { get; set; }

        public int PartTypeId { get; set; }
    }
}

