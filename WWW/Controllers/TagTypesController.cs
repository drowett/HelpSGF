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
    public class TagTypesController : Controller
    {
        private readonly TagsService _tagsService;

        public TagTypesController(HelpSGFContext context)
        {
            _tagsService = new TagsService(context);
        }

        // GET: TagTypes
        public async Task<IActionResult> Index()
        {
            var tagTypesAsync = await _tagsService.GetTagTypesAsync();
            var model = tagTypesAsync.Select(S => new TagTypeModel(S));

            return View(model);
        }

        // GET: TagTypes/Create
        public async Task<IActionResult> Create()
        {
            var availableApliesTo = await _tagsService.GetTagTypesAsync();
            var model = new TagTypeModel()
            {
                AvailableAppliesTo = availableApliesTo.Select(S => new TagModel(S.AppliesTo, S.AppliesTo)).Distinct().ToArray()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] TagTypeModel tagType, String[] SelectedTags)
        {
            if (ModelState.IsValid && SelectedTags.Count() > 0)
            {
                var tagTypesAsync = await _tagsService.GetTagTypesAsync();

                tagType.AppliesTo = SelectedTags;
                
                await _tagsService.SaveTagTypeAsync(tagType.TagTypeModelDTO(true));

                return RedirectToAction(nameof(Index));
            }

            var availableApliesTo = await _tagsService.GetTagTypesAsync();

            tagType.AvailableAppliesTo = availableApliesTo.Select(S => new TagModel(S.AppliesTo, S.AppliesTo)).Distinct().ToArray();

            return View(tagType);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var tagType = await _tagsService.GetTagTypeAsync(id);

            if (tagType == null) return NotFound();

            var tagTypeModel = new TagTypeModel(tagType);
            var availableApliesTo = await _tagsService.GetTagTypesAsync();

            tagTypeModel.AvailableAppliesTo = availableApliesTo.Select(S => new TagModel(S.AppliesTo, S.AppliesTo)).Distinct().ToArray();
            
            return View(tagTypeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name")] TagType tagType, String[] SelectedTags)
        {
            if (id != tagType.ID) return NotFound();

            if (ModelState.IsValid && SelectedTags.Count() > 0)
            {
                try
                {
                    _context.Update(tagType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagTypeExists(tagType.ID))
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
            return View(tagType);
        }


        //private bool TagTypeExists(string id)
        //{
        //    return _context.TagTypes.Any(e => e.ID == id);
        //}
    }
}
