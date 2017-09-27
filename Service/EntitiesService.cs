using System;
using System.Collections.Generic;
using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Service
{
    public class EntitiesService : BaseService
    {
        public EntitiesService(HelpSGFContext content) : base(content) { }

        //Gets
        public Task<List<Entity>> GetEntitiesAsync() => _context.Entities
                    .Include(I => I.Contacts)
                    .Include(I2 => I2.Entity_To_Tags)
                    .ToListAsync();

        public IEnumerable<Entity> GetEntities()
        {
            return _context.Entities.Include(I => I.Contacts).Include(I2 => I2.Entity_To_Tags);
        }

        public Task<Entity> GetEntityAsync(Guid id) => _context.Entities
                    .Include(I => I.Contacts)
                    .Include(I2 => I2.Entity_To_Tags)
                    .SingleOrDefaultAsync(SODA => SODA.ID == id);

        public Entity GetEntity(Guid id)
        {
            return _context.Entities.Include(I => I.Contacts).Include(I2 => I2.Entity_To_Tags).SingleOrDefault(SOD => SOD.ID == id);
        }

        public Task<Contact> GetContactAsync(Guid id) => _context.Contacts.SingleOrDefaultAsync(SODA => SODA.ID == id);
        
        public Contact GetContact(Guid id)
        {
            return _context.Contacts.SingleOrDefault(SOD => SOD.ID == id);
        }

        //Sets
        public async Task<int> SaveEntityAsync(Entity entity)
        {
            await _context.AddAsync(entity);

            return await _context.SaveChangesAsync();
        }

        public int SaveEntity(Entity entity)
        {
            _context.Add(entity);

            return _context.SaveChanges();
        }

        public async Task<int> SaveContactAsync(Contact contact)
        {
            var ii = await _context.AddAsync(contact);

            var i = await _context.SaveChangesAsync();
            return i;
        }

        public int SaveContact(Contact contact)
        {
            _context.Add(contact);

            return _context.SaveChanges();
        }

        //Modify
        public async Task<int> UpdateEntityAsync(Entity entity, String[] entityToTags)
        {
            var entityToSave = await GetEntityAsync(entity.ID);
            var i = -1;

            if (entityToSave != null)
            {
                entityToSave.Name = entity.Name;
                entityToSave.Description = entity.Description;
                entityToSave.Address1 = entity.Address1;
                entityToSave.Address2 = entity.Address2;
                entityToSave.City = entity.City;
                entityToSave.County = entity.County;
                entityToSave.State = entity.State;
                entityToSave.Zip = entity.Zip;
                entityToSave.Type = entity.Type;
                entityToSave.IsSuppressed = entity.IsSuppressed;

                //Remove the old
                if (entityToSave.Entity_To_Tags != null)
                {
                    foreach (var ett in entityToSave.Entity_To_Tags)
                    {
                        _context.Entities_To_Tags.Remove(ett);
                    }
                }
                //Add the new
                foreach (var ett in entityToTags)
                {
                    var newEtt = new Entity_To_Tag()
                    {
                        EntityID = entityToSave.ID,
                        TagID = ett
                    };
                    
                    _context.Entities_To_Tags.Add(newEtt);
                }

                i = await _context.SaveChangesAsync();
            }

            return i;
        }

        public int UpdateEntity(Entity entity, String[] entityToTags)
        {
            var entityToSave = GetEntity(entity.ID);
            var i = -1;

            if (entityToSave != null)
            {
                entityToSave.Name = entity.Name;
                entityToSave.Description = entity.Description;
                entityToSave.Address1 = entity.Address1;
                entityToSave.Address2 = entity.Address2;
                entityToSave.City = entity.City;
                entityToSave.County = entity.County;
                entityToSave.State = entity.State;
                entityToSave.Zip = entity.Zip;
                entityToSave.Type = entity.Type;
                entityToSave.IsSuppressed = entity.IsSuppressed;

                //Remove the old
                if (entityToSave.Entity_To_Tags != null)
                {
                    foreach (var ett in entityToSave.Entity_To_Tags)
                    {
                        _context.Entities_To_Tags.Remove(ett);
                    }
                }
                //Add the new
                foreach (var ett in entityToTags)
                {
                    var newEtt = new Entity_To_Tag()
                    {
                        EntityID = entityToSave.ID,
                        TagID = ett
                    };

                    _context.Entities_To_Tags.Add(newEtt);
                }

                i = _context.SaveChanges();
            }

            return i;
        }

        public async Task<int> UpdateContactAsync(Contact contact)
        {
            var contactToSave = await GetContactAsync(contact.ID);
            var i = -1;

            if(contactToSave != null)
            {
                contactToSave.Value = contact.Value;
                contactToSave.TagID = contact.TagID;

                i = await _context.SaveChangesAsync();
            }

            return i;
        }

        public int UpdateContact(Contact contact)
        {
            var contactToSave = GetContact(contact.ID);
            var i = -1;

            if (contactToSave != null)
            {
                contactToSave.Value = contact.Value;
                i = _context.SaveChanges();
            }

            return i;
        }

        public async Task<int> SuppressEntityAsync(Guid id)
        {
            var entity = await GetEntityAsync(id);

            entity.IsSuppressed = false;

            return await _context.SaveChangesAsync();
        }   

        public int SuppressEntity(Guid id)
        {
            var entity = GetEntity(id);

            entity.IsSuppressed = true;

            return _context.SaveChanges();
        }

        //Delete
        public async Task<int> DeleteContactAsync(Guid id)
        {
            var contact = await GetContactAsync(id);
            var i = -1;

            if(contact != null)
            {
                _context.Remove(contact);

                i = await _context.SaveChangesAsync();
            }

            return i;
        }

        public int DeleteContact(Guid id)
        {
            var contact = GetContact(id);
            var i = 1;

            if(contact != null)
            {
                _context.Remove(contact);
                i =_context.SaveChanges();
            }

            return i;
        }
    }
}
