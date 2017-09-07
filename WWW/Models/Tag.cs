using System;
using System.ComponentModel.DataAnnotations;

namespace WWW.Models
{
    public class TagModel
    {
        [Required]
        public String ID { get; set; }

        [Required]
        public String Name { get; set; }

        public TagModel() { }

        public TagModel(DataAccess.Models.Tag tag)
        {
            ID = tag.ID;
            Name = tag.Name;
        }

        public TagModel(String id, String name)
        {
            ID = id;
            Name = name;
        }

        public DataAccess.Models.Tag TagModelDTO(String tagTypeID)
        {
            return new DataAccess.Models.Tag()
            {
                ID = this.ID,
                Name = this.Name,
                TagTypeID = tagTypeID
            };
        }
    }
}
