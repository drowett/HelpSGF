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
    public class ContactsController : Controller
    {
        private readonly EntitiesService _entitiesService;
        private readonly TagsService _tagsService;

        public ContactsController(HelpSGFContext context)
        {
            _entitiesService = new EntitiesService(context);
            _tagsService = new TagsService(context);
        }

        // GET: Contacts/Create
        public async Task<IActionResult> Create(Guid id)
        {
            var tagsAsync = await _tagsService.GetTagsAsync();
            var tags = tagsAsync.Where(W => W.TagType.AppliesTo.Contains("contact")).Select(S => new TagModel(S)).ToList();

            var model = new ContactModelWithContacts()
            {
                EntityID = id,
                Tags = tags
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EntityID,TagID,Value")] ContactModelWithTags contact)
        {
            if (ModelState.IsValid)
            {
                await _entitiesService.SaveContactAsync(contact.ContactModelDTO(true));

                return RedirectToAction("Edit", "Entities", new { id = contact.EntityID });
            }

            var tagsAsync = await _tagsService.GetTagsAsync();
            var tags = tagsAsync.Where(W => W.TagType.AppliesTo.Contains("contact")).Select(S => new TagModel(S)).ToList();

            ContactModelWithContacts model = (ContactModelWithContacts)contact;
            model.Tags = tags;

            return View(model);
        }

    }
}