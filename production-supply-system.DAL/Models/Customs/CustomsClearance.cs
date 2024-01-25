using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о таможенной очистке груза.
    /// </summary>
    /// 
    [Table("tbd_Customs_Clearance", Schema = "Customs")]
    public class CustomsClearance
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Customs_Clearance_Id")]
        public int CustomsClearanceId { get; set; }

        [Required(ErrorMessage = "Поле 'Container_In_Lot_Id' обязательно для заполнения.")]
        [Column("Container_In_Lot_Id")]
        public int ContainerInLotId { get; set; }

        [Column("Invoce_Number")]
        [MaxLength(50, ErrorMessage = "Максимальная длина 'Invoce_Number' - 50 символов.")]
        public string InvoiceNumber { get; set; }

        [DataType(DataType.Date)]
        [Column("Docs_To_Customs_Date")]
        public DateTime? DocsToCustomsDate { get; set; }

        [Column("AEO_Obbligation_Number")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 'AEO_Obbligation_Number' - 20 символов.")]
        public string? AEOObbligationNumber { get; set; }

        [DataType(DataType.Date)]
        [Column("AEO_Obbligation_Release_Date")]
        public DateTime? AEOObbligationReleaseDate { get; set; }

        [Column("CCD_Number")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 'CCD_Number' - 20 символов.")]
        public string? CCDNumber { get; set; }

        [DataType(DataType.Date)]
        [Column("CCD_Release_Date")]
        public DateTime? CCDReleaseDate { get; set; }

        [Column("Customs_Inpection_Need")]
        public bool? CustomsInpectionNeed { get; set; }

        [DataType(DataType.Date)]
        [Column("EDocuments_To_Be_Provided_Date")]
        public DateTime? EDocumentsToBeProvidedDate { get; set; }

        [DataType(DataType.Date)]
        [Column("EDocuments_To_Be_Received_Date")]
        public DateTime? EDocumentsToBeReceivedDate { get; set; }

        [Column("Part_Type_Id")]
        public int? PartTypeId { get; set; }

        [ForeignKey("Container_In_Lot_Id")]
        public virtual ContainerInLot ContainerInLot { get; set; }

        [ForeignKey("Part_Type_Id")]
        public virtual TypeOfPart PartType { get; set; }
    }
}
