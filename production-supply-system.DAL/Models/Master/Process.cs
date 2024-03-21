using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;
using DAL.Models.Contracts;

namespace DAL.Models.Master
{
    /// <summary>
    /// Представляет информацию о процессе.
    /// </summary>
    [Table("tbd_Processes", Schema = "Master")]
    public class Process : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300, ErrorMessage = "Process Name must not exceed 300 characters.")]
        public AppProcess ProcessName { get; set; }
    }
}
