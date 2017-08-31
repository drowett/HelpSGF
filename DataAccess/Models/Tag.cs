using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Tag
    {
        [Required]
        [StringLength(50)]
        public String ID { get; set; }

        [Required]
        [StringLength(50)]
        public String TagTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public String Name { get; set; }

        public TagType TagType { get; set; }

        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Entity_To_Tag> Entity_To_Tag { get; set; }

    }
}
