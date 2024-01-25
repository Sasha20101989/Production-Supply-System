using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о контейнерах в лоте.
    /// </summary>
    [Table("tbd_Containers_In_Lot", Schema = "Inbound")]
    public class ContainerInLot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Container_In_Lot_Id")]
        public int ContainerInLotId { get; set; }

        [Required(ErrorMessage = "Поле 'Lot_Id' обязательно для заполнения.")]
        [Column("Lot_Id")]
        public int LotId { get; set; }

        [Required(ErrorMessage = "Поле 'Container_Number' обязательно для заполнения.")]
        [Column("Container_Number")]
        [MaxLength(11, ErrorMessage = "Максимальная длина 'Container_Number' - 11 символов.")]
        public string ContainerNumber { get; set; }

        [Column("Seal_Number")]
        [MaxLength(20, ErrorMessage = "Максимальная длина 'Seal_Number' - 20 символов.")]
        public string? SealNumber { get; set; }

        [Column("Container_Type_Id")]
        public int? ContainerTypeId { get; set; }

        [Column("IMO_Cargo")]
        public bool? IMOCargo { get; set; }

        [DataType(DataType.Date)]
        [Column("Storage_Last_Free_Day")]
        public DateTime? StorageLastFreeDay { get; set; }

        [DataType(DataType.Date)]
        [Column("Detention_Last_Free_Day")]
        public DateTime? DetentionLastFreeDay { get; set; }

        [Column("Container_Comment")]
        [MaxLength(250, ErrorMessage = "Максимальная длина 'Container_Comment' - 250 символов.")]
        public string? ContainerComment { get; set; }

        [DataType(DataType.Date)]
        [Column("Last_Tracing_Update")]
        public DateTime? LastTracingUpdate { get; set; }

        [Column("CI_OnTheWay")]
        public bool CIOnTheWay { get; set; }

        [ForeignKey("Lot_Id")]
        public virtual Lot Lot { get; set; }

        [ForeignKey("Container_Type_Id")]
        public virtual TypeOfContainer TypeOfContainer { get; set; }
    }
}
