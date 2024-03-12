using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DAL.Models.Contracts;

namespace DAL.Models.Inbound
{
    [Table("tbd_Terms_Of_Container_Use", Schema = "Inbound")]
    public class TermsOfContainerUse : IEntity
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Init_Date")]
        public DateTime InitDate { get; set; }

        [Required(ErrorMessage = "Carrier Id is required.")]
        [Column("Carrier_Id")]
        public int CarrierId { get; set; }

        [Column("Detention")]
        public int? Detention { get; set; }

        [Column("Storage")]
        public int? Storage { get; set; }

        [ForeignKey("CarrierId")]
        public virtual Carrier? Carrier { get; set; }
    }
}
