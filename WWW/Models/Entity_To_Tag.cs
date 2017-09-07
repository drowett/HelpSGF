using System;
using System.ComponentModel.DataAnnotations;

namespace WWW.Models
{
    public class EntityToTagModel
    {
        public Guid EntityID { get; set; }

        public String TagID { get; set; }
    }
}
