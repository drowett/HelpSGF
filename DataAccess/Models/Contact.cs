using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Contact
    {
        [Key]
        [StringLength(50)]
        public Guid ID { get; set; }
        public Guid EntityID { get; set; }
        [StringLength(50)]
        public String TagID { get; set; }
        [StringLength(500)]
        public String Value { get; set; }

        public Entity Entity { get; set; }
        public Tag Tags { get; set; }

    }
}
