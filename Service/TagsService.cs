using System;
using System.Collections.Generic;
using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Service
{
    public class TagsService : BaseService
    {
        public TagsService(HelpSGFContext context) : base(context) { }

        //Gets
        // Async
        public Task<List<TagType>> GetTagTypesAsync() => _context.TagTypes.Include(I => I.Tags).ToListAsync();

        public Task<TagType> GetTagTypeAsync(String id) => _context.TagTypes.Include(I => I.Tags).SingleOrDefaultAsync(SODA => SODA.ID == id);

        public async Task<List<String>> GetAllTagTypesAppliesToAsync()
        {
            var appliesTo = new List<String>();

            foreach(var tagType in await GetTagTypesAsync())
            {
                foreach (var item in tagType.AppliesTo.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    appliesTo.Add(item);
            }

            return appliesTo.Distinct().ToList();
        }

        public Task<List<Tag>> GetTagsAsync() => _context.Tags.ToListAsync();

        public Task<Tag> GetTagAsync(String id) => _context.Tags.SingleOrDefaultAsync(SODA => SODA.ID == id);
        
        //Sync
        public IEnumerable<TagType> GetTagTypes()
        {
            return _context.TagTypes;
        }

        public IEnumerable<Tag> GetTagAsEnumberable()
        {
            return _context.Tags;
        }

        public TagType GetTagType(String id)
        {
            return _context.TagTypes.SingleOrDefault(SOD => SOD.ID == id);
        }

        public Tag GetTag(String id)
        {
            return _context.Tags.SingleOrDefault(SOD => SOD.ID == id);
        }

        //Set
        // Async
        public async Task<int> SaveTagTypeAsync(TagType tagType)
        {
            _context.Add(tagType);

            return await _context.SaveChangesAsync();
        }

        public int SaveTagType(TagType tagType)
        {
            _context.Add(tagType);

            return _context.SaveChanges();
        }

        public async Task<int> UpdateTagTypeAsync(TagType tagType)
        {
            var tagTypeToUpdate = await GetTagTypeAsync(tagType.ID);
            var i = -1;

            if (tagTypeToUpdate != null)
            {
                tagTypeToUpdate.Name = tagType.Name;
                tagTypeToUpdate.AppliesTo = tagType.AppliesTo;

                i = await _context.SaveChangesAsync();
            }

            return i;
        }
    }
}
