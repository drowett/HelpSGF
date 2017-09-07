using System;
using System.ComponentModel.DataAnnotations;

namespace WWW.Models
{
    public class ContactModel
    {
        [Required]
        public Guid ID { get; set; }

        [Required]
        public Guid EntityID { get; set; }

        [Required]
        public String TagID { get; set; }

        [Required]
        public String Value { get; set; }

        public ContactModel() { }

        public ContactModel(DataAccess.Models.Contact contact)
        {
            ID = contact.ID;
            EntityID = contact.EntityID;
            TagID = contact.TagID;
            Value = contact.Value;
        }

        public DataAccess.Models.Contact ContactModelDTO()
        {
            return new DataAccess.Models.Contact()
            {
                ID = this.ID,
                EntityID = this.EntityID,
                TagID = this.TagID,
                Value = this.Value
            };
        }
    }
}
