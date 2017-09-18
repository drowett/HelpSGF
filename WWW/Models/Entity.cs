using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WWW.Models
{
    public class EntityModel
    {
        public Guid ID { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        [StringLength(500)]
        public String Address1 { get; set; }

        [StringLength(500)]
        public String Address2 { get; set; }

        [Display(Name = "Address")]
        public String GetCombinedAddress
        {
            get
            {
                return Address1 + "\n" + Address2;
            }
        }

        [StringLength(100)]
        public String City { get; set; }

        [StringLength(100)]
        public String County { get; set; }

        [StringLength(100)]
        public String State { get; set; }

        [StringLength(20)]
        public String Zip { get; set; }

        [Display(Name = "Location")]
        public String GetCombinedLocation
        {
            get
            {
                return County + "\n" + City + "\n" + State + "\n" + Zip;
            }
        }

        [Required]
        [StringLength(20)]
        public String Type { get; set; }

        public Boolean IsSuppressed { get; set; }

        public IList<ContactModel> Contacts { get; set; }

        public IList<TagModel> AvailableTags { get; set; }

        public String[] SelectedTags { get; set; }

        private String SearchContainer { get; set; }

        public Boolean Contains(String search)
        {
            return SearchContainer.Contains(search);
        }

        public EntityModel()
        {
            Contacts = new List<ContactModel>();
            AvailableTags = new List<TagModel>();
        }

        public EntityModel(DataAccess.Models.Entity entity, IList<TagModel> tags)
        {
            ID = entity.ID;
            Name = entity.Name;
            Description = entity.Description;
            Address1 = entity.Address1;
            Address2 = entity.Address2;
            City = entity.City;
            State = entity.State;
            County = entity.County;
            Zip = entity.State;
            Type = entity.Type;
            IsSuppressed = entity.IsSuppressed;

            Contacts = (entity.Contacts != null) ?
                entity.Contacts.Select(S => new ContactModel(S)).ToList() :
                new List<ContactModel>();

            SelectedTags = (entity.Entity_To_Tags != null) ?
                entity.Entity_To_Tags.Select(S => S.TagID).ToArray<String>() :
                new String[] { };

            AvailableTags = tags;

            SearchContainer = Name.ToLower() + " " + Description.ToLower() + " " + Address1.ToLower() + " " + Address2.ToLower();
        }

        public  DataAccess.Models.Entity EntityModelDTO(Boolean generateID = false)
        {
            if (generateID)
                this.ID = Guid.NewGuid();

            return new DataAccess.Models.Entity()
            {
                ID = this.ID,
                Name = this.Name,
                Description = this.Description,
                Address1 = this.Address1,
                Address2 = this.Address2,
                City = this.City,
                State = this.State,
                County = this.County,
                Zip = this.State,
                Type = this.Type,
                IsSuppressed = this.IsSuppressed

                //Current thinking about how we return tags for this entity
            };
        }

    }
}
