using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о грузосместе.
    /// </summary>
    [Table("tbd_Cases", Schema = "Inbound")]
    public class Case
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Case_Id")]
        public int CaseId { get; set; }

        [Required]
        [Column("Case_No")]
        [MaxLength(10)]
        public string CaseNo { get; set; }

        [Column("Net_Weight")]
        public decimal? NetWeight { get; set; }

        [Column("Gross_Wheight")]
        public decimal? GrossWheight { get; set; }

        [Column("Packing_Type_Id")]
        public int? PackingTypeId { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public decimal? Volume { get; set; }

        [ForeignKey("Packing_Type_Id")]
        public virtual TypeOfPacking TypeOfPacking { get; set; }
    }
}
