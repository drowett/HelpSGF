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

        //Save

        //Delete
        public async Task<int> DeleteEntityAsync(Guid id)
        {
            var entity = await GetEntityAsync(id);

            entity.IsSuppressed = false;

            return await _context.SaveChangesAsync();
        }   


        public int DeleteEntity(Guid id)
        {
            var entity = GetEntity(id);

            entity.IsSuppressed = true;

            return _context.SaveChanges();
        }

    }
}
