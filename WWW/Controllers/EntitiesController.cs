using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using Service;
using WWW.Models;

namespace WWW.Controllers
{
    public class EntitiesController : Controller
    {
        private readonly HelpSGFContext _context;
        private readonly EntitiesService _entitiesService;
        private readonly TagsService _tagsService;

        public EntitiesController(HelpSGFContext context)
        {
            _context = context;
            _entitiesService = new EntitiesService(context);
            _tagsService = new TagsService(context);
        }
        
        public async Task<IActionResult> Index(String sortOrder, String searchString, int page = 1, int pageSize = 10)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CitySortParm"] = sortOrder == "city" ? "city_desc" : "city";
            ViewData["StateSortParm"] = sortOrder == "state" ? "state_desc" : "state";
            ViewData["CountySortParm"] = sortOrder == "country" ? "county_desc" : "country";

            ViewData["CurrentFilter"] = searchString;

            var tagsAsync = await _tagsService.GetTagsAsync();
            var entitiesAsync = await _entitiesService.GetEntitiesAsync();

            var tags = tagsAsync.Select(S => new TagModel(S)).ToList();
            var entities = entitiesAsync.Select(S => new EntityModel(S, tags));

            if (!String.IsNullOrEmpty(searchString))
                entities = entities.Where(W => W.Contains(searchString));

            switch (sortOrder)
            {
                case "name_desc":
                    entities = entities.OrderByDescending(O => O.Name);
                    break;
                case "city":
                    entities = entities.OrderBy(O => O.City);
                    break;
                case "city_desc":
                    entities = entities.OrderByDescending(O => O.City);
                    break;
                case "state":
                    entities = entities.OrderBy(O => O.State);
                    break;
                case "state_desc":
                    entities = entities.OrderByDescending(O => O.State);
                    break;
                case "country":
                    entities = entities.OrderBy(O => O.County);
                    break;
                case "country_desc":
                    entities = entities.OrderByDescending(O => O.County);
                    break;
                default:
                    entities = entities.OrderBy(O => O.Name);
                    break;
            }

            return View(PaginatedList<EntityModel>.Create(entities.ToList(), page, pageSize));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var entity = await _entitiesService.GetEntityAsync((Guid)id);
            var tagsAsync = await _tagsService.GetTagsAsync();

            if (entity == null) return NotFound();
            
            var tags = tagsAsync.Select(S => new TagModel(S)).ToList();
            var model = new EntityModel(entity, tags);

            return View(model);
        }

        // GET: Entities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Address1,Address2,City,County,State,Zip,IsSuppressed")] Entity entity)
        {
            if (ModelState.IsValid)
            {
                entity.ID = Guid.NewGuid();
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        // GET: Entities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _context.Entities.SingleOrDefaultAsync(m => m.ID == id);
            if (entity == null)
            {
                return NotFound();
            }
            return View(entity);
        }

        // POST: Entities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Name,Description,Address1,Address2,City,County,State,Zip,IsSuppressed")] Entity entity)
        {
            if (id != entity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntityExists(entity.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var entity = await _entitiesService.GetEntityAsync((Guid)id);

            if (entity == null) return NotFound();

            var model = new EntityModel(entity, new List<TagModel>());

            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _entitiesService.DeleteEntityAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private bool EntityExists(Guid id)
        {
            return _context.Entities.Any(e => e.ID == id);
        }
    }
}
