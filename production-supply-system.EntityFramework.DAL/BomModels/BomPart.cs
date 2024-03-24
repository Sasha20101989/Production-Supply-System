namespace production_supply_system.EntityFramework.DAL.BomModels
{
    public class BomPart
    {
        public int Id { get; set; }

        public string PartNumber { get; set; } = null!;

        public string? PartName { get; set; }

        public string? SupplierPartCode { get; set; }

        public string? SupplierPartName { get; set; }

        public string? AdditionalPartCode { get; set; }

        public int? SupplierCodeID { get; set; }

        public int? IntColor { get; set; }

        public int? ExtColor { get; set; }

        public int? HSCode { get; set; }

        public DateTime? DateAdd { get; set; }
    }
}
