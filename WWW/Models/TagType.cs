using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WWW.Classes;
using System.Linq;

namespace WWW.Models
{
    public class TagTypeModel
    {
        public String ID { get; set; }

        [Required]
        [StringLength(50)]
        public String Name { get; set; }
        public String[] AppliesTo { get; set; }
        public IList<TagModel> AvailableAppliesTo { get; set; }

        public IList<TagModel> Tags { get; set; }

        public TagTypeModel()
        {
            AppliesTo = new String[] { };

            AvailableAppliesTo = new List<TagModel>();
            Tags = new List<TagModel>();
        }

        public TagTypeModel(DataAccess.Models.TagType tagType)
        {
            ID = tagType.ID;
            Name = tagType.Name;
            AppliesTo = tagType.AppliesTo.Split(",");
            Tags = tagType.Tags.Select(S => new TagModel(S)).OrderBy(O => O.Name).ToList();
        }

        public DataAccess.Models.TagType TagTypeModelDTO(Boolean generateID = false)
        {
            if (generateID)
                this.ID = this.Name.RemoveSpecialCharacters();

            return new DataAccess.Models.TagType()
            {
                ID = this.ID,
                Name = this.Name,
                AppliesTo = String.Join(',', this.AppliesTo)
            };
        }
    }
}
