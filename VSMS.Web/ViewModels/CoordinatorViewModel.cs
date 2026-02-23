using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VSMS.Web.Models;

namespace VSMS.Web.ViewModels
{
    public class CoordinatorViewModel
    {
        public Coordinator Coordinator { get; set; } = new Coordinator();
        public IEnumerable<SelectListItem>? Organizations { get; set; }
    }
}
