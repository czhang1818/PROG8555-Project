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
    public class CoordinatorsController : Controller
    {
        private readonly ICoordinatorService _coordinatorService;
        private readonly IOrganizationService _organizationService;

        public CoordinatorsController(ICoordinatorService coordinatorService, IOrganizationService organizationService)
        {
            _coordinatorService = coordinatorService;
            _organizationService = organizationService;
        }

        // GET: Coordinators
        public async Task<IActionResult> Index()
        {
            return View(await _coordinatorService.GetAllCoordinatorsAsync());
        }

        // GET: Coordinators/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _coordinatorService.GetCoordinatorByIdAsync(id.Value);
            if (coordinator == null)
            {
                return NotFound();
            }

            return View(coordinator);
        }

        // GET: Coordinators/Create
        public async Task<IActionResult> Create()
        {
            var organizations = await _organizationService.GetAllOrganizationsAsync();
            var viewModel = new VSMS.Web.ViewModels.CoordinatorViewModel
            {
                Organizations = new SelectList(organizations, "OrganizationId", "Name")
            };
            return View(viewModel);
        }

        // POST: Coordinators/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizationId,Name,JobTitle,UserId,Email,PhoneNumber")] Coordinator coordinator)
        {
            ModelState.Remove("Organization");
            ModelState.Remove("Role");
            ModelState.Remove("PasswordHash");

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
                Console.WriteLine("COORDINATOR CREATE MODEL VALIDATION ERRORS: " + errors);
            }

            if (ModelState.IsValid)
            {
                coordinator.UserId = Guid.NewGuid();
                coordinator.PasswordHash = "DefaultHash!";
                coordinator.Role = "Coordinator";
                coordinator.CreatedAt = DateTime.Now;
                await _coordinatorService.AddCoordinatorAsync(coordinator);
                return RedirectToAction(nameof(Index));
            }

            var organizations = await _organizationService.GetAllOrganizationsAsync();
            var viewModel = new VSMS.Web.ViewModels.CoordinatorViewModel
            {
                Coordinator = coordinator,
                Organizations = new SelectList(organizations, "OrganizationId", "Name", coordinator.OrganizationId)
            };
            return View(viewModel);
        }


        // GET: Coordinators/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _coordinatorService.GetCoordinatorByIdAsync(id.Value);
            if (coordinator == null)
            {
                return NotFound();
            }

            var organizations = await _organizationService.GetAllOrganizationsAsync();
            var viewModel = new VSMS.Web.ViewModels.CoordinatorViewModel
            {
                Coordinator = coordinator,
                Organizations = new SelectList(organizations, "OrganizationId", "Name", coordinator.OrganizationId)
            };
            return View(viewModel);
        }

        // POST: Coordinators/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OrganizationId,Name,JobTitle,UserId,Email,PhoneNumber")] Coordinator coordinator)
        {
            if (id != coordinator.UserId)
            {
                return NotFound();
            }

            ModelState.Remove("Organization");
            ModelState.Remove("Role");
            ModelState.Remove("PasswordHash");

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                Console.WriteLine("COORDINATOR EDIT VALIDATION ERRORS: " + errors);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _coordinatorService.GetCoordinatorByIdAsync(id);
                    if (existing == null) return NotFound();

                    existing.Name = coordinator.Name;
                    existing.JobTitle = coordinator.JobTitle;
                    existing.PhoneNumber = coordinator.PhoneNumber;
                    existing.OrganizationId = coordinator.OrganizationId;
                    existing.Email = coordinator.Email;

                    await _coordinatorService.UpdateCoordinatorAsync(existing);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoordinatorExists(coordinator.UserId))
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

            var organizations = await _organizationService.GetAllOrganizationsAsync();
            var viewModel = new VSMS.Web.ViewModels.CoordinatorViewModel
            {
                Coordinator = coordinator,
                Organizations = new SelectList(organizations, "OrganizationId", "Name", coordinator.OrganizationId)
            };
            return View(viewModel);
        }



        // GET: Coordinators/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _coordinatorService.GetCoordinatorByIdAsync(id.Value);
            if (coordinator == null)
            {
                return NotFound();
            }

            return View(coordinator);
        }

        // POST: Coordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _coordinatorService.DeleteCoordinatorAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CoordinatorExists(Guid id)
        {
            return _coordinatorService.CoordinatorExists(id);
        }
    }
}
