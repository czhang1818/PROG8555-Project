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
        private readonly ISkillService _skillService;

        public VolunteersController(IVolunteerService volunteerService, ISkillService skillService)
        {
            _volunteerService = volunteerService;
            _skillService = skillService;
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

            // Also load the skills if we want to display them on details
            var allSkills = await _skillService.GetAllSkillsAsync();
            var selectedSkillIds = volunteer.VolunteerSkills?.Select(vs => vs.SkillId).ToList() ?? new List<Guid>();

            var viewModel = new VSMS.Web.ViewModels.VolunteerSkillViewModel
            {
                Volunteer = volunteer,
                SelectedSkillIds = selectedSkillIds,
                AvailableSkills = allSkills.ToList()
            };

            return View(viewModel);
        }

        // GET: Volunteers/Create
        public async Task<IActionResult> Create()
        {
            var allSkills = await _skillService.GetAllSkillsAsync();
            var viewModel = new VSMS.Web.ViewModels.VolunteerSkillViewModel
            {
                AvailableSkills = allSkills.ToList()
            };
            return View(viewModel);
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VSMS.Web.ViewModels.VolunteerSkillViewModel viewModel)
        {
            ModelState.Remove("Volunteer.PasswordHash");
            ModelState.Remove("Volunteer.Role");

            // Allow empty SelectedSkillIds without failing validation
            ModelState.Remove("SelectedSkillIds");
            ModelState.Remove("AvailableSkills");

            if (ModelState.IsValid)
            {
                var volunteer = viewModel.Volunteer;
                volunteer.UserId = Guid.NewGuid();
                volunteer.PasswordHash = "DefaultHash!";
                volunteer.Role = "Volunteer";
                volunteer.CreatedAt = DateTime.Now;

                await _volunteerService.AddVolunteerAsync(volunteer);

                if (viewModel.SelectedSkillIds != null && viewModel.SelectedSkillIds.Any())
                {
                    await _volunteerService.AddOrUpdateVolunteerSkillsAsync(volunteer.UserId, viewModel.SelectedSkillIds);
                }

                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailableSkills = (await _skillService.GetAllSkillsAsync()).ToList();
            return View(viewModel);
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

            var allSkills = await _skillService.GetAllSkillsAsync();
            // Fetch the currently selected skill ids from the junction table
            var selectedSkillIds = volunteer.VolunteerSkills?.Select(vs => vs.SkillId).ToList() ?? new List<Guid>();

            var viewModel = new VSMS.Web.ViewModels.VolunteerSkillViewModel
            {
                Volunteer = volunteer,
                AvailableSkills = allSkills.ToList(),
                SelectedSkillIds = selectedSkillIds
            };

            return View(viewModel);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, VSMS.Web.ViewModels.VolunteerSkillViewModel viewModel)
        {
            if (id != viewModel.Volunteer.UserId)
            {
                return NotFound();
            }

            ModelState.Remove("Volunteer.PasswordHash");
            ModelState.Remove("Volunteer.Role");

            // Allow empty SelectedSkillIds without failing validation
            ModelState.Remove("SelectedSkillIds");
            ModelState.Remove("AvailableSkills");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _volunteerService.GetVolunteerByIdAsync(id);
                    if (existing == null) return NotFound();

                    existing.Name = viewModel.Volunteer.Name;
                    existing.PhoneNumber = viewModel.Volunteer.PhoneNumber;
                    existing.TotalHours = viewModel.Volunteer.TotalHours;
                    existing.Email = viewModel.Volunteer.Email;

                    await _volunteerService.UpdateVolunteerAsync(existing);

                    // Update junction table records
                    await _volunteerService.AddOrUpdateVolunteerSkillsAsync(id, viewModel.SelectedSkillIds ?? new List<Guid>());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(viewModel.Volunteer.UserId))
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

            viewModel.AvailableSkills = (await _skillService.GetAllSkillsAsync()).ToList();
            return View(viewModel);
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

            // Also load the skills if we want to display them on details
            var allSkills = await _skillService.GetAllSkillsAsync();
            var selectedSkillIds = volunteer.VolunteerSkills?.Select(vs => vs.SkillId).ToList() ?? new List<Guid>();

            var viewModel = new VSMS.Web.ViewModels.VolunteerSkillViewModel
            {
                Volunteer = volunteer,
                SelectedSkillIds = selectedSkillIds,
                AvailableSkills = allSkills.ToList()
            };

            return View(viewModel);
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
