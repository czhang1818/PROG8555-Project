using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSMS.Web.Models;

namespace VSMS.Web.Services.Interfaces
{
    public interface ICoordinatorService
    {
        Task<IEnumerable<Coordinator>> GetAllCoordinatorsAsync();
        Task<Coordinator?> GetCoordinatorByIdAsync(Guid id);
        Task AddCoordinatorAsync(Coordinator coordinator);
        Task UpdateCoordinatorAsync(Coordinator coordinator);
        Task DeleteCoordinatorAsync(Guid id);
        bool CoordinatorExists(Guid id);
    }
}
