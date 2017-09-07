using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Entity
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(1000)]
        public String Name { get; set; }

        public String Description { get; set; }

        [StringLength(500)]
        public String Address1 { get; set; }

        [StringLength(500)]
        public String Address2 { get; set; }

        [StringLength(100)]
        public String City { get; set; }

        [StringLength(100)]
        public String County { get; set; }

        [StringLength(100)]
        public String State { get; set; }

        [StringLength(20)]
        public String Zip { get; set; }

        public Boolean IsSuppressed { get; set; }

        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Entity_To_Tag> Entity_To_Tags { get; set; }
    }
}
