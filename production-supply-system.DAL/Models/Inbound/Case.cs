using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о грузосместе.
    /// </summary>
    [Table("tbd_Cases", Schema = "Inbound")]
    public class Case : IEntity
    {
        private TypesOfPacking _packingType;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Case_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Case Number is required.")]
        [MaxLength(10, ErrorMessage = "Case Number must not exceed 10 characters.")]
        [Column("Case_No")]
        public string CaseNo { get; set; } = null!;

        [Required(ErrorMessage = "Net Weight is required.")]
        [Column("Net_Weight")]
        public decimal NetWeight { get; set; }

        [Required(ErrorMessage = "Gross Weight is required.")]
        [Column("Gross_Weight")]
        public decimal GrossWeight { get; set; }

        [Column("Length")]
        public decimal? Length { get; set; }

        [Column("Width")]
        public decimal? Width { get; set; }

        [Column("Height")]
        public decimal? Height { get; set; }

        [Column("Volume")]
        public decimal? Volume { get; set; }

        [Column("Packing_Type_Id")]
        public int? PackingTypeId { get; set; }

        [ForeignKey("PackingTypeId")]
        public virtual TypesOfPacking PackingType
        {
            get => _packingType;
            set
            {
                _packingType = value;
                PackingTypeId = value?.Id ?? null;
            }
        }
    }
}
