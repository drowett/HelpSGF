using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WWW.Models
{
    public class ContactModel
    {
        public Guid ID { get; set; }

        [Required]
        public Guid EntityID { get; set; }

        [Required]
        public String TagID { get; set; }

        public String Name { get; set; }

        [Required]
        public String Value { get; set; }

        public ContactModel() { }

        public ContactModel(DataAccess.Models.Contact contact)
        {
            ID = contact.ID;
            EntityID = contact.EntityID;
            TagID = contact.TagID;
            Name = contact.Tags.Name;
            Value = contact.Value;
        }

        public DataAccess.Models.Contact ContactModelDTO(Boolean generateID = false)
        {
            if (generateID) ID = Guid.NewGuid();

            return new DataAccess.Models.Contact()
            {
                ID = this.ID,
                EntityID = this.EntityID,
                TagID = this.TagID,
                Value = this.Value
            };
        }
    }

    public class ContactModelWithContacts : ContactModel
    {
        public IList<TagModel> Tags { get; set; }
        public ContactModelWithContacts() : base() { }

        public ContactModelWithContacts(DataAccess.Models.Contact contact, IList<TagModel> tags) : base(contact)
        {
            Tags = tags;
        }
    }
}
