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

        public Task<Entity> GetEntityAsync(Guid id) => _context.Entities.SingleOrDefaultAsync(SODA => SODA.ID == id);

        public Entity GetEntity(Guid id)
        {
            return _context.Entities.SingleOrDefault(SOD => SOD.ID == id);
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
            await _context.AddAsync(contact);

            return await _context.SaveChangesAsync();
        }

        public int SaveContact(Contact contact)
        {
            _context.Add(contact);

            return _context.SaveChanges();
        }

        //Modify
        public async Task<int> UpdateEntityAsync(Entity entity)
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

                i = await _context.SaveChangesAsync();
            }

            return i;
        }

        public int UpdateEntity(Entity entity)
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

                i =  _context.SaveChanges();
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

    }
}
