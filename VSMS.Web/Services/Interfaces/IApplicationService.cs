using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSMS.Web.Models;

namespace VSMS.Web.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task<Application?> GetApplicationByIdAsync(Guid id);
        Task AddApplicationAsync(Application application);
        Task UpdateApplicationAsync(Application application);
        Task DeleteApplicationAsync(Guid id);
        bool ApplicationExists(Guid id);
    }
}
