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
    public class TagsController : Controller
    {
        private readonly TagsService _tagsService;

        public TagsController(HelpSGFContext context)
        {
            _tagsService = new TagsService(context);
        }

        public IActionResult Create(String tagTypeID)
        {
            var model = new TagModel()
            {
                TagTypeID = tagTypeID
            };

            return View(model);
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagTypeID,Name")] TagModel tag)
        {
            if (ModelState.IsValid)
            {
                await _tagsService.SaveTagAsync(tag.TagModelDTO());

                return RedirectToAction("Edit", "TagTypes", new { id = tag.ID });
            }

            return View(tag);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();


            var tag = await _tagsService.GetTagAsync(id);

            if (tag == null) return NotFound();

            var tagModel = new TagModel(tag);

            return View(tagModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IDTagTypeID,Name")] TagModel tag)
        {
            if (id != tag.ID) return NotFound();

            if (ModelState.IsValid)
            {
                var i = await _tagsService.UpdateTagAsync(tag.TagModelDTO());

                return RedirectToAction("Edit", "TagTypes", new { id = tag.ID });
            }
            
            return View(tag);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var tag = await _tagsService.GetTagAsync(id);

            if (tag == null) return NotFound();

            var model = new TagModel(tag);

            return View(tag);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _tagsService.DeleteTagAsync(id);

            return RedirectToAction("Edit", "TagTypes", new { id = id });
        }
    }
}
