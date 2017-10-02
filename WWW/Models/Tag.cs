using System;
using System.ComponentModel.DataAnnotations;
using WWW.ExtensionMethods;

namespace WWW.Models
{
    public class TagModel
    {
        public String ID { get; set; }

        [Required]
        public String Name { get; set; }

        public String TagTypeID { get; set; }

        public String TagTypeName { get; set; }

        public TagModel() { }

        public TagModel(DataAccess.Models.Tag tag)
        {
            ID = tag.ID;
            Name = tag.Name;
            TagTypeID = tag.TagTypeID;
            TagTypeName = tag.TagType.Name;
        }

        public TagModel(String id, String name, String tagTypeID)
        {
            ID = id;
            Name = name;
            TagTypeID = tagTypeID;
        }

        public DataAccess.Models.Tag TagModelDTO(Boolean generateID = false)
        {
            if (generateID)
                this.ID = this.Name.RemoveSpecialCharacters();

            return new DataAccess.Models.Tag()
            {
                ID = this.ID,
                Name = this.Name,
                TagTypeID = this.TagTypeID
            };
        }
    }
}
