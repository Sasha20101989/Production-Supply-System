using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Enums;

namespace DAL.Models.Master
{
    /// <summary>
    /// Представляет информацию о процессе.
    /// </summary>
    [Table("tbd_Processes", Schema = "Master")]
    public class Process
    {
        [Key]
        public int ProcessId { get; set; }

        [Required]
        [MaxLength(300, ErrorMessage ="Колличество символов не должно превышать 300")]
        public AppProcess ProcessName { get; set; }
    }
}
