using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Entity_To_Tag
    {
        public Guid EntityID { get; set; }
        [StringLength(50)]
        public String TagID { get; set; }

        public Entity Entity { get; set; }
        public Tag Tag { get; set; }
    }
}
