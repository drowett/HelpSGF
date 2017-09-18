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
        public async Task<IActionResult> Create([Bind("EntityID,TagID,Value")] ContactModel contact)
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

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null) return NotFound();

            var contact = await _entitiesService.GetContactAsync(id);

            if (contact == null) return NotFound();

            var tagsAsync = await _tagsService.GetTagsAsync();
            var tags = tagsAsync.Where(W => W.TagType.AppliesTo.Contains("contact")).Select(S => new TagModel(S)).ToList();

            var model = new ContactModelWithContacts(contact, tags);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ID, EntityID,TagID,Value")] ContactModel contact)
        {
            if(ModelState.IsValid)
            {
                var i =await _entitiesService.UpdateContactAsync(contact.ContactModelDTO());

                return RedirectToAction("Edit", "Entities", new { id = contact.EntityID });
            }

            var tagsAsync = await _tagsService.GetTagsAsync();
            var tags = tagsAsync.Where(W => W.TagType.AppliesTo.Contains("contact")).Select(S => new TagModel(S)).ToList();

            ContactModelWithContacts model = (ContactModelWithContacts)contact;
            model.Tags = tags;

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null) return NotFound();

            var contact = await _entitiesService.GetContactAsync(id);

            if (contact == null) return NotFound();

            var tagsAsync = await _tagsService.GetTagsAsync();
            var tags = tagsAsync.Where(W => W.TagType.AppliesTo.Contains("contact")).Select(S => new TagModel(S)).ToList();

            var model = new ContactModelWithContacts(contact, tags);

            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, [Bind("ID,EntityID")] ContactModel contact)
        {
            if (id != contact.ID) return NotFound();

            await _entitiesService.DeleteContactAsync(id);

            return RedirectToAction("Edit", "Entities", new { id = contact.EntityID });
        }
    }
}