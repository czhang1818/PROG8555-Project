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
    public class OpportunitiesController : Controller
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IOrganizationService _organizationService;
        private readonly ISkillService _skillService;

        public OpportunitiesController(IOpportunityService opportunityService, IOrganizationService organizationService, ISkillService skillService)
        {
            _opportunityService = opportunityService;
            _organizationService = organizationService;
            _skillService = skillService;
        }

        // GET: Opportunities
        public async Task<IActionResult> Index()
        {
            return View(await _opportunityService.GetAllOpportunitiesAsync());
        }

        // GET: Opportunities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _opportunityService.GetOpportunityByIdAsync(id.Value);
            if (opportunity == null)
            {
                return NotFound();
            }

            var allSkills = await _skillService.GetAllSkillsAsync();
            var selectedSkillIds = opportunity.OpportunitySkills?.Select(os => os.SkillId).ToList() ?? new List<Guid>();

            var viewModel = new VSMS.Web.ViewModels.OpportunitySkillViewModel
            {
                Opportunity = opportunity,
                SelectedSkillIds = selectedSkillIds,
                AvailableSkills = allSkills.ToList()
            };

            return View(viewModel);
        }

        // GET: Opportunities/Create
        public async Task<IActionResult> Create()
        {
            var organizations = await _organizationService.GetAllOrganizationsAsync();
            var allSkills = await _skillService.GetAllSkillsAsync();

            // Re-using the same ViewBag/SelectList approach for organizations for simplicity,
            // but wrapped in the new OpportunitySkillViewModel
            ViewBag.Organizations = new SelectList(organizations, "OrganizationId", "Name");

            var viewModel = new VSMS.Web.ViewModels.OpportunitySkillViewModel
            {
                AvailableSkills = allSkills.ToList()
            };
            return View(viewModel);
        }

        // POST: Opportunities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VSMS.Web.ViewModels.OpportunitySkillViewModel viewModel)
        {
            ModelState.Remove("Opportunity.Organization");

            // Allow empty SelectedSkillIds without failing validation
            ModelState.Remove("SelectedSkillIds");
            ModelState.Remove("AvailableSkills");

            if (ModelState.IsValid)
            {
                viewModel.Opportunity.OpportunityId = Guid.NewGuid();
                await _opportunityService.AddOpportunityAsync(viewModel.Opportunity);

                if (viewModel.SelectedSkillIds != null && viewModel.SelectedSkillIds.Any())
                {
                    await _opportunityService.AddOrUpdateOpportunitySkillsAsync(viewModel.Opportunity.OpportunityId, viewModel.SelectedSkillIds);
                }

                return RedirectToAction(nameof(Index));
            }

            var organizations = await _organizationService.GetAllOrganizationsAsync();
            ViewBag.Organizations = new SelectList(organizations, "OrganizationId", "Name", viewModel.Opportunity.OrganizationId);
            viewModel.AvailableSkills = (await _skillService.GetAllSkillsAsync()).ToList();

            return View(viewModel);
        }

        // GET: Opportunities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _opportunityService.GetOpportunityByIdAsync(id.Value);
            if (opportunity == null)
            {
                return NotFound();
            }

            var organizations = await _organizationService.GetAllOrganizationsAsync();
            var allSkills = await _skillService.GetAllSkillsAsync();
            var selectedSkillIds = opportunity.OpportunitySkills?.Select(os => os.SkillId).ToList() ?? new List<Guid>();

            ViewBag.Organizations = new SelectList(organizations, "OrganizationId", "Name", opportunity.OrganizationId);

            var viewModel = new VSMS.Web.ViewModels.OpportunitySkillViewModel
            {
                Opportunity = opportunity,
                SelectedSkillIds = selectedSkillIds,
                AvailableSkills = allSkills.ToList()
            };
            return View(viewModel);
        }

        // POST: Opportunities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, VSMS.Web.ViewModels.OpportunitySkillViewModel viewModel)
        {
            if (id != viewModel.Opportunity.OpportunityId)
            {
                return NotFound();
            }

            ModelState.Remove("Opportunity.Organization");

            // Allow empty SelectedSkillIds without failing validation
            ModelState.Remove("SelectedSkillIds");
            ModelState.Remove("AvailableSkills");

            if (ModelState.IsValid)
            {
                try
                {
                    await _opportunityService.UpdateOpportunityAsync(viewModel.Opportunity);
                    await _opportunityService.AddOrUpdateOpportunitySkillsAsync(id, viewModel.SelectedSkillIds ?? new List<Guid>());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpportunityExists(viewModel.Opportunity.OpportunityId))
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
            ViewBag.Organizations = new SelectList(organizations, "OrganizationId", "Name", viewModel.Opportunity.OrganizationId);
            viewModel.AvailableSkills = (await _skillService.GetAllSkillsAsync()).ToList();

            return View(viewModel);
        }

        // GET: Opportunities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _opportunityService.GetOpportunityByIdAsync(id.Value);
            if (opportunity == null)
            {
                return NotFound();
            }

            var allSkills = await _skillService.GetAllSkillsAsync();
            var selectedSkillIds = opportunity.OpportunitySkills?.Select(os => os.SkillId).ToList() ?? new List<Guid>();

            var viewModel = new VSMS.Web.ViewModels.OpportunitySkillViewModel
            {
                Opportunity = opportunity,
                SelectedSkillIds = selectedSkillIds,
                AvailableSkills = allSkills.ToList()
            };

            return View(viewModel);
        }

        // POST: Opportunities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _opportunityService.DeleteOpportunityAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool OpportunityExists(Guid id)
        {
            return _opportunityService.OpportunityExists(id);
        }
    }
}
