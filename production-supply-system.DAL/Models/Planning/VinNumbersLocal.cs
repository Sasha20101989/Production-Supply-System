using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Contracts;

namespace DAL.Models.Planning
{
    [Table("tbd_VIN_Numbers_Local", Schema = "Planning")]
    public partial class VinNumbersLocal : IEntity
    {
        [Key]
        [Column("VIN_Number_Local_Id")]
        public int Id { get; set; }

        [Column("Order_Count")]
        public int? OrderCount { get; set; }

        [Column("Plant")]
        public int? Plant { get; set; }

        [Column("Line")]
        public int? Line { get; set; }

        [MaxLength(5, ErrorMessage = "The length of Model should not exceed 5 characters.")]
        public string? Model { get; set; }

        [Required(ErrorMessage = "Local VIN is required.")]
        [MaxLength(17, ErrorMessage = "The length of Local VIN should be 17 characters.")]
        [Column("Local_VIN")]
        public string LocalVin { get; set; } = null!;

        [Column("Date_Position")]
        public DateTime DatePosition { get; set; }

        [MaxLength(20, ErrorMessage = "The length of PJI should not exceed 20 characters.")]
        public string? Pji { get; set; }

        [Required(ErrorMessage = "Local Code is required.")]
        [MaxLength(17, ErrorMessage = "The length of Local Code should not exceed 17 characters.")]
        [Column("Local_Code")]
        public string LocalCode { get; set; } = null!;

        [MaxLength(5, ErrorMessage = "The length of Local Color should not exceed 5 characters.")]
        [Column("Local_Color")]
        public string? LocalColor { get; set; }

        [MaxLength(50, ErrorMessage = "The length of BC Label should not exceed 50 characters.")]
        [Column("BC_Label")]
        public string? BcLabel { get; set; }

        [MaxLength(255, ErrorMessage = "The length of MOFF should not exceed 255 characters.")]
        [Column("MOFF")]
        public string? Moff { get; set; }

        [Column("Status")]
        public int? Status { get; set; }

        [Column("N_In_Day")]
        public int? NInDay { get; set; }

        [Column("Next_Status")]
        public long? NextStatus { get; set; }

        [MaxLength(20, ErrorMessage = "The length of Version should not exceed 20 characters.")]
        public string? Version { get; set; }

        [Column("Order_Number")]
        public int? OrderNumber { get; set; }

        [MaxLength(10, ErrorMessage = "The length of Market Code should not exceed 10 characters.")]
        [Column("Market_Code")]
        public string? MarketCode { get; set; }

        [Column("Countermark")]
        public int? Countermark { get; set; }

        [MaxLength(10, ErrorMessage = "The length of Millenium should not exceed 10 characters.")]
        public string? Millenium { get; set; }

        [MaxLength(8, ErrorMessage = "The length of Week Position should not exceed 8 characters.")]
        [Column("Week_Position")]
        public string? WeekPosition { get; set; }

        [Column("Day_Position")]
        public int? DayPosition { get; set; }

        [Column("Order_ID")]
        public int OrderId { get; set; }

        [MaxLength(10, ErrorMessage = "The length of VCD should not exceed 10 characters.")]
        public string? Vcd { get; set; }

        public virtual BoundVin? BoundVin { get; set; }
    }
}