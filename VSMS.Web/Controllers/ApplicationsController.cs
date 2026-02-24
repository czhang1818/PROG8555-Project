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
    public class ApplicationsController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IOpportunityService _opportunityService;
        private readonly IVolunteerService _volunteerService;

        public ApplicationsController(
            IApplicationService applicationService,
            IOpportunityService opportunityService,
            IVolunteerService volunteerService)
        {
            _applicationService = applicationService;
            _opportunityService = opportunityService;
            _volunteerService = volunteerService;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            return View(await _applicationService.GetAllApplicationsAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _applicationService.GetApplicationByIdAsync(id.Value);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public async Task<IActionResult> Create()
        {
            var opportunities = await _opportunityService.GetAllOpportunitiesAsync();
            var volunteers = await _volunteerService.GetAllVolunteersAsync();
            var viewModel = new VSMS.Web.ViewModels.ApplicationViewModel
            {
                Opportunities = new SelectList(opportunities, "OpportunityId", "Title"),
                Volunteers = new SelectList(volunteers, "UserId", "Name")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OpportunityId,VolunteerId,Status")] Application application)
        {
            ModelState.Remove("Opportunity");
            ModelState.Remove("Volunteer");

            if (ModelState.IsValid)
            {
                application.AppId = Guid.NewGuid();
                application.SubmissionDate = DateTime.Now;
                await _applicationService.AddApplicationAsync(application);
                return RedirectToAction(nameof(Index));
            }

            var opportunities = await _opportunityService.GetAllOpportunitiesAsync();
            var volunteers = await _volunteerService.GetAllVolunteersAsync();
            var viewModel = new VSMS.Web.ViewModels.ApplicationViewModel
            {
                Application = application,
                Opportunities = new SelectList(opportunities, "OpportunityId", "Title", application.OpportunityId),
                Volunteers = new SelectList(volunteers, "UserId", "Name", application.VolunteerId)
            };
            return View(viewModel);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _applicationService.GetApplicationByIdAsync(id.Value);
            if (application == null)
            {
                return NotFound();
            }

            var opportunities = await _opportunityService.GetAllOpportunitiesAsync();
            var volunteers = await _volunteerService.GetAllVolunteersAsync();
            var viewModel = new VSMS.Web.ViewModels.ApplicationViewModel
            {
                Application = application,
                Opportunities = new SelectList(opportunities, "OpportunityId", "Title", application.OpportunityId),
                Volunteers = new SelectList(volunteers, "UserId", "Name", application.VolunteerId)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OpportunityId,VolunteerId,Status,AppId")] Application application)
        {
            if (id != application.AppId)
            {
                return NotFound();
            }

            ModelState.Remove("Opportunity");
            ModelState.Remove("Volunteer");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _applicationService.GetApplicationByIdAsync(id);
                    if (existing == null) return NotFound();

                    existing.OpportunityId = application.OpportunityId;
                    existing.VolunteerId = application.VolunteerId;
                    existing.Status = application.Status;

                    await _applicationService.UpdateApplicationAsync(existing);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.AppId))
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

            var opportunities = await _opportunityService.GetAllOpportunitiesAsync();
            var volunteers = await _volunteerService.GetAllVolunteersAsync();
            var viewModel = new VSMS.Web.ViewModels.ApplicationViewModel
            {
                Application = application,
                Opportunities = new SelectList(opportunities, "OpportunityId", "Title", application.OpportunityId),
                Volunteers = new SelectList(volunteers, "UserId", "Name", application.VolunteerId)
            };
            return View(viewModel);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _applicationService.GetApplicationByIdAsync(id.Value);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _applicationService.DeleteApplicationAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(Guid id)
        {
            return _applicationService.ApplicationExists(id);
        }
    }
}
