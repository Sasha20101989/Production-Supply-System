using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Attributes;
using DAL.Enums;
using DAL.Models.Contracts;
using DAL.Models.Inbound;
using DAL.Models.Planning;

namespace DAL.Models
{
    /// <summary>
    /// Представляет информацию о контейнерах в лоте.
    /// Модель заполняется исходя из способа доставки, для 
    /// </summary>
    [Table("tbd_Containers_In_Lot", Schema = "Inbound")]
    public class ContainersInLot : IEntity
    {
        private TypesOfContainer _containerType;

        private Lot _lot;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Container_In_Lot_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Lot Id is required.")]
        [Column("Lot_Id")]
        [Min(1)]
        public int LotId { get; set; }

        [Required(ErrorMessage = "Container Number is required.")]
        [MaxLength(11, ErrorMessage = "Container Number must not exceed 11 characters.")]
        [Column("Container_Number")]
        public string ContainerNumber { get; set; } = null!;

        [MaxLength(20, ErrorMessage = "Seal Number must not exceed 20 characters.")]
        [Column("Seal_Number")]
        public string? SealNumber { get; set; }

        [Required(ErrorMessage = "Container Type Id  is required.")]
        [ForeignKey("ContainerTypeId")]
        [Min(1)]
        public int ContainerTypeId { get; set; }

        [Display(Name = "IMO Cargo")]
        [Column("IMO_Cargo")]
        public bool? ImoCargo { get; set; } = null!;

        [Display(Name = "Storage Last Free Day")]
        [Column("Storage_Last_Free_Day")]
        public DateTime? StorageLastFreeDay { get; set; }

        [Display(Name = "Detention Last Free Day")]
        [Column("Detention_Last_Free_Day")]
        public DateTime? DetentionLastFreeDay { get; set; }

        [MaxLength(250, ErrorMessage = "Container Comment must not exceed 250 characters.")]
        [Column("Container_Comment")]
        public string? ContainerComment { get; set; }

        [Display(Name = "Last Tracing Update")]
        [Column("Last_Tracing_Update")]
        public DateTime? LastTracingUpdate { get; set; }

        [Required(ErrorMessage = "CI On The Way is required.")]
        [Display(Name = "CI On The Way")]      
        [Column("CI_OnTheWay")]
        public bool CiOnTheWay { get; set; }

        [ForeignKey("ContainerTypeId")]
        public virtual TypesOfContainer ContainerType
        {
            get => _containerType;
            set
            {
                _containerType = value;
                ContainerTypeId = value?.Id ?? 0;
            }
        }

        [ForeignKey("LotId")]
        public virtual Lot Lot
        {
            get => _lot;
            set
            {
                _lot = value;
                LotId = value?.Id ?? 0;
            }
        }

        [Column("Cargo_Type")]
        public virtual CargoTypes CargoType { get; set; }
    }
}
