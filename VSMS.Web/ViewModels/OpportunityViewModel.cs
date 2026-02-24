using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VSMS.Web.Models;

namespace VSMS.Web.ViewModels
{
    public class OpportunityViewModel
    {
        public Opportunity Opportunity { get; set; } = new Opportunity();
        public IEnumerable<SelectListItem>? Organizations { get; set; }
    }
}
