using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class TagType
    {
        [Required]
        [StringLength(50)]
        public String ID { get; set; }

        [Required]
        [StringLength(50)]
        public String Name { get; set; }

        [Required]
        [StringLength(50)]
        public String AppliesTo { get; set; }
    }
}
