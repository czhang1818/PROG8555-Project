using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VSMS.Web.Models;

namespace VSMS.Web.ViewModels
{
    public class ApplicationViewModel
    {
        public Application Application { get; set; } = new Application();
        public IEnumerable<SelectListItem>? Volunteers { get; set; }
        public IEnumerable<SelectListItem>? Opportunities { get; set; }
    }
}
