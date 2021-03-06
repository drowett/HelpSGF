﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using Services;
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

        public IActionResult Create(String id)
        {
            var model = new TagModel()
            {
                TagTypeID = id
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
                await _tagsService.SaveTagAsync(tag.TagModelDTO(true));

                return RedirectToAction("Edit", "TagTypes", new { id = tag.TagTypeID });
            }

            return View(tag);
        }

        public async Task<IActionResult> Edit(String id)
        {
            if (id == null) return NotFound();

            var tag = await _tagsService.GetTagAsync(id);

            if (tag == null) return NotFound();

            var tagModel = new TagModel(tag);

            return View(tagModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(String id, [Bind("ID,TagTypeID,Name")] TagModel tag)
        {
            if (id != tag.ID) return NotFound();

            if (ModelState.IsValid)
            {
                var i = await _tagsService.UpdateTagAsync(tag.TagModelDTO());

                return RedirectToAction("Edit", "TagTypes", new { id = tag.TagTypeID });
            }
            
            return View(tag);
        }

        public async Task<IActionResult> Delete(String id)
        {
            if (String.IsNullOrEmpty(id)) return NotFound();

            var tag = await _tagsService.GetTagAsync(id);

            if (tag == null) return NotFound();

            var model = new TagModel(tag);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(String id, String tagTypeID)
        {
            await _tagsService.DeleteTagAsync(id);

            return RedirectToAction("Edit", "TagTypes", new { id = tagTypeID });
        }
    }
}
