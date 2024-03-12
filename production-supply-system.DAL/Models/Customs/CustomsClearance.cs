using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Enums;
using DAL.Models.Contracts;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о таможенной очистке груза.
    /// </summary>
    /// 
    [Table("tbd_Customs_Clearance", Schema = "Customs")]
    public class CustomsClearance : IEntity
    {
        private TypesOfPart _partType;

        private ContainersInLot _container;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Customs_Clearance_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Container In Lot Id is required.")]
        [Column("Container_In_Lot_Id")]
        public int ContainerInLotId { get; set; }

        [MaxLength(50, ErrorMessage = "Invoice Number must not exceed 50 characters.")]
        [Column("Invoce_Number")]
        public string? InvoceNumber { get; set; }

        [Display(Name = "Docs To Customs Date")]
        [Column("Docs_To_Customs_Date")]
        public DateTime? DocsToCustomsDate { get; set; }

        [MaxLength(20, ErrorMessage = "AEO Obligation Number must not exceed 20 characters.")]
        [Column("AEO_Obbligation_Number")]
        public string? AeoObbligationNumber { get; set; }

        [Display(Name = "AEO Obligation Release Date")]
        [Column("AEO_Obbligation_Release_Date")]
        public DateTime? AeoObbligationReleaseDate { get; set; }

        [MaxLength(20, ErrorMessage = "CCD Number must not exceed 20 characters.")]
        [Column("CCD_Number")]
        public string? CcdNumber { get; set; }

        [Display(Name = "CCD Release Date")]
        [Column("CCD_Release_Date")]
        public DateTime? CcdReleaseDate { get; set; }

        [Display(Name = "Customs Inspection Need")]
        [Column("Customs_Inpection_Need")]
        public bool? CustomsInpectionNeed { get; set; }

        [Display(Name = "EDocuments To Be Provided Date")]
        [Column("EDocuments_To_Be_Provided_Date")]
        public DateTime? EdocumentsToBeProvidedDate { get; set; }

        [Display(Name = "EDocuments To Be Received Date")]
        [Column("EDocuments_To_Be_Received_Date")]
        public DateTime? EdocumentsToBeReceivedDate { get; set; }

        [Display(Name = "Part Type")]
        [ForeignKey("PartTypeId")]
        public int? PartTypeId { get; set; }

        [ForeignKey("ContainerInLotId")]
        public virtual ContainersInLot ContainersInLot
        {
            get => _container;
            set
            {
                _container = value;
                ContainerInLotId = value?.Id ?? 0;
            }
        }

        [ForeignKey("PartTypeId")]
        public virtual TypesOfPart PartType
        {
            get => _partType;
            set
            {
                _partType = value;
                PartTypeId = value?.Id ?? null;
            }
        }
    }
}
