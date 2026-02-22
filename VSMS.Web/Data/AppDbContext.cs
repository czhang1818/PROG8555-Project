using Microsoft.EntityFrameworkCore;
using VSMS.Web.Models;

namespace VSMS.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        
    }
}
