using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace DataAccess
{
    public static class DbInitializer 
    {
        private static HelpSGFContext _context;

        public static void Initialize(HelpSGFContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();

            if (_context.Entities.Any())
            {
                return;
            }

            BuildTagTypes();
            var tags = BuildTags();
            var entities = BuildEntities();

            BuildContacts(entities, tags.Where(W => W.TagType.AppliesTo == "contact").ToList());
            BuildEntityToTags(entities, tags.Where(W => W.TagType.AppliesTo == "company").ToList());
        }

        private static void BuildTagTypes()
        {
            var tagTypes = new List<TagType>()
            {
                new TagType(){ AppliesTo = "contact", ID = "contact", Name = "Contact" },
                new TagType(){ AppliesTo = "company", ID = "service", Name = "Service" },
                new TagType(){ AppliesTo = "person", ID = "person", Name = "Person" }
            };

            tagTypes.ForEach(FE => _context.TagTypes.Add(FE));
            _context.SaveChanges();
        }

        private static IList<Tag> BuildTags()
        {
            var tags = new List<Tag>()
            {
                new Tag() { ID = "contact_phone", Name = "Phone", TagTypeID = "contact"},
                new Tag() { ID = "contact_fax", Name = "Fax", TagTypeID = "contact"},
                new Tag() { ID = "contact_tty", Name = "TTY", TagTypeID = "contact"},
                new Tag() { ID = "contact_videophone", Name = "Video Phone", TagTypeID = "contact"},
                new Tag() { ID = "contact_tollfree", Name = "Toll Free", TagTypeID = "contact"},
                new Tag() { ID = "contact_hotline", Name = "Hotline", TagTypeID = "contact"},
                new Tag() { ID = "contact_email", Name = "Email", TagTypeID = "contact"},
                new Tag() { ID = "contact_webpage", Name = "Web Page", TagTypeID = "contact"},
                new Tag() { ID = "contact_facebookname", Name = "Facebook Name", TagTypeID = "contact"},
                new Tag() { ID = "contact_facebookurl", Name = "Facebook URL", TagTypeID = "contact"},
                new Tag() { ID = "contact_twitter", Name = "Twitter", TagTypeID = "contact"},
                new Tag() { ID = "contact_other", Name = "Other", TagTypeID = "contact"},
                new Tag() { ID = "service_homeless", Name = "Homeless", TagTypeID = "service"},
                new Tag() { ID = "service_foodpantry", Name = "Food Pantry", TagTypeID = "service"},
                new Tag() { ID = "service_medical", Name = "Medical", TagTypeID = "service"},
                new Tag() { ID = "service_dental", Name = "Dental", TagTypeID = "service"},
                new Tag() { ID = "service_substanceabuse", Name = "Substance Abuse", TagTypeID = "service"},
                new Tag() { ID = "service_adoption", Name = "Adoption", TagTypeID = "service"},
                new Tag() { ID = "service_legal", Name = "Legal", TagTypeID = "service"},
                new Tag() { ID = "service_veteran", Name = "Veteran", TagTypeID = "service"},
                new Tag() { ID = "service_men", Name = "Men", TagTypeID = "service"},
                new Tag() { ID = "service_women", Name = "Women", TagTypeID = "service"},
                new Tag() { ID = "service_children", Name = "Children", TagTypeID = "service"},
                new Tag() { ID = "service_senior", Name = "Seniors", TagTypeID = "service"}

            };

            tags.ForEach(FE => _context.Tags.Add(FE));
            _context.SaveChanges();

            return tags;
        }

        private static IList<KeyValuePair<String, Entity>> BuildEntities()
        {
            var entities = new List<KeyValuePair<String, Entity>>();

            foreach (String row in EntityData)
            {
                var ele = row.Split("||");
                var entity = new Entity()
                {
                    ID = Guid.NewGuid(),
                    Name = ele[1],
                    Description = ele[2],
                    Address1 = ele[3],
                    Address2 = ele[4],
                    City = ele[5],
                    State = ele[6],
                    Zip = ele[7],
                    County = ele[8],
                    IsSuppressed = false
                };

                entities.Add(new KeyValuePair<String, Entity>(ele[0], entity));
            }

            entities.ForEach(FE => _context.Entities.Add(FE.Value));
            _context.SaveChanges();

            return entities;
        }

        private static void BuildContacts(IList<KeyValuePair<String, Entity>> entities, IList<Tag> tags)
        {
            var contacts = new List<Contact>();

            foreach (String row in EntityData)
            {
                var ele = row.Split("||");

                if (ele.Count() < 1 && entities.Where(W => W.Key == ele[0]).Count() > 1)
                {
                    throw new IndexOutOfRangeException();
                }

                var entity = entities.SingleOrDefault(SOD => SOD.Key == ele[0]);

                if (IsValidString(ele[9]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_phone").ID, ele[9]));

                if (IsValidString(ele[10]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_fax").ID, ele[10]));

                if (IsValidString(ele[11]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_tty").ID, ele[11]));

                if (IsValidString(ele[12]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_videophone").ID, ele[12]));

                if (IsValidString(ele[13]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_tollfree").ID, ele[13]));

                if (IsValidString(ele[14]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_hotline").ID, ele[14]));

                if (IsValidString(ele[15]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_email").ID, ele[15]));

                if (IsValidString(ele[16]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_webpage").ID, ele[16]));

                if (IsValidString(ele[17]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_facebookname").ID, ele[17]));

                if (IsValidString(ele[18]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_facebookurl").ID, ele[18]));

                if (IsValidString(ele[19]))
                    contacts.Add(BuildContact(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "contact_twitter").ID, ele[19]));
            }

            contacts.ForEach(FE => _context.Contacts.Add(FE));
            _context.SaveChanges();
            
        }

        private static Boolean IsValidString(String ele)
        {
            return (!String.IsNullOrEmpty(ele) && !String.IsNullOrWhiteSpace(ele));
        }

        private static Contact BuildContact(Guid entityID, String tagID, String tagValue)
        {
            return new Contact()
            {
                ID = Guid.NewGuid(),
                EntityID = entityID,
                TagID = tagID,
                Value = tagValue
            };
        }

        private static void BuildEntityToTags(IList<KeyValuePair<String, Entity>> entities, IList<Tag> tags)
        {
            var entitytoTags = new List<Entity_To_Tag>();
            var homelessData = HomelessData;
            var medical = new String[] { "medical", "hospital", "clinic", "medicine" };
            var dental = new String[] { "dental" };
            var food = new String[] { "food", "pantry" };
            var substanceAbuse = new String[] { "substance" };
            var adopt = new String[] { "adopt" };
            var legal = new String[] { "legal", "civil", "criminal", "domestic", "traffic" };
            var veteran = new String[] { "veteran" };
            var men = new String[] { "men", "boys", "father", "sons" };
            var women = new String[] { "women", "woman", "female", "girl", "mother", "daughters" };
            var child = new String[] { "children", "child" };
            var senior = new String[] { "senior" };

            //These, with the exception of homeless are pretty arbitrary
            foreach (var entity in entities)
            {
                var entityDescription = entity.Value.Description.ToLower().Split(" ");

                // Add homeless
                if (HomelessData.Any(A => A == entity.Key))
                {
                    _context.Entities_To_Tags.Add(BuildEntityToTag(entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_homeless").ID));
                    _context.SaveChanges();
                }

                // Add Dental
                AddIfHasTag(entityDescription, dental, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_dental").ID);
                
                // Add Medical
                AddIfHasTag(entityDescription, medical, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_medical").ID);

                // Add Food
                AddIfHasTag(entityDescription, food, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_foodpantry").ID);

                // Add Substance abuse
                AddIfHasTag(entityDescription, substanceAbuse, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_substanceabuse").ID);

                //Add Adoption
                AddIfHasTag(entityDescription, adopt, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_adoption").ID);

                // Add Legal
                AddIfHasTag(entityDescription, legal, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_legal").ID);

                // Add Veteran
                AddIfHasTag(entityDescription, veteran, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_veteran").ID);

                // Add Men
                AddIfHasTag(entityDescription, men, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_men").ID);

                // Add Woman
                AddIfHasTag(entityDescription, women, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_women").ID);

                // Add Children
                AddIfHasTag(entityDescription, child, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_children").ID);

                // Add Senior
                AddIfHasTag(entityDescription, senior, entity.Value.ID, tags.SingleOrDefault(SOD => SOD.ID == "service_senior").ID);

            }
        }

        private static void AddIfHasTag(String[] description, String[] items, Guid entityID, String tagID)
        {
            // Add Children
            if (description.Any(A => items.Contains(A)))
            {
                _context.Entities_To_Tags.Add(BuildEntityToTag(entityID, tagID));
                _context.SaveChanges();
            }
        }

        private static Entity_To_Tag BuildEntityToTag(Guid entityID, String tagID)
        {
            return new Entity_To_Tag()
            {
                EntityID = entityID,
                TagID = tagID
            };
        }

        private static String[] GetData(String fullAbsPath)
        {
            return (File.Exists(fullAbsPath)) ?
                    File.ReadAllLines(fullAbsPath) :
                    null;
        }

        private static String[] EntityData
        {
            get
            {
                //resource_id || resource_name || short_description || address1 || address2 || city || state || zip || county || phone || fax || tty || toll_free || hotline || email || web_page || facebook_name || facebook_url || twitter
                //This needs to be updated like istudio base service
                // appsettings.json has this value, and startup adds this to services.Configure
                // but difficulties getting this infor info this class library
                return GetData(@"C:\inetpub\domains\HelpSGF\DataAccess\SeedData\entitydata.txt");
            }
        }

        private static String[] HomelessData
        {
            get
            {
                return GetData(@"C:\inetpub\domains\HelpSGF\DataAccess\SeedData\homelessdata.txt");
            }
        }

    }
}
