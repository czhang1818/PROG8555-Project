using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VSMS.Web.Data;
using VSMS.Web.Models;
using VSMS.Web.Services.Interfaces;

namespace VSMS.Web.Controllers
{
    public class VolunteersController : Controller
    {
        private readonly IVolunteerService _volunteerService;

        public VolunteersController(IVolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
        }

        // GET: Volunteers
        public async Task<IActionResult> Index()
        {
            return View(await _volunteerService.GetAllVolunteersAsync());
        }

        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _volunteerService.GetVolunteerByIdAsync(id.Value);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // GET: Volunteers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PhoneNumber,TotalHours,UserId,Email")] Volunteer volunteer)
        {
            ModelState.Remove("PasswordHash");
            ModelState.Remove("Role");

            if (ModelState.IsValid)
            {
                volunteer.UserId = Guid.NewGuid();
                volunteer.PasswordHash = "DefaultHash!";
                volunteer.Role = "Volunteer";
                volunteer.CreatedAt = DateTime.Now;
                await _volunteerService.AddVolunteerAsync(volunteer);
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _volunteerService.GetVolunteerByIdAsync(id.Value);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,PhoneNumber,TotalHours,UserId,Email")] Volunteer volunteer)
        {
            if (id != volunteer.UserId)
            {
                return NotFound();
            }

            ModelState.Remove("PasswordHash");
            ModelState.Remove("Role");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _volunteerService.GetVolunteerByIdAsync(id);
                    if (existing == null) return NotFound();

                    existing.Name = volunteer.Name;
                    existing.PhoneNumber = volunteer.PhoneNumber;
                    existing.TotalHours = volunteer.TotalHours;
                    existing.Email = volunteer.Email;

                    await _volunteerService.UpdateVolunteerAsync(existing);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.UserId))
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
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _volunteerService.GetVolunteerByIdAsync(id.Value);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _volunteerService.DeleteVolunteerAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerExists(Guid id)
        {
            return _volunteerService.VolunteerExists(id);
        }
    }
}
