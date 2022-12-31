using Microsoft.EntityFrameworkCore;
using shelveservice.Infrastructure.Persistence;

namespace shelveservice.Infrastructure.Persistence
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ShelveContext>().Database.Migrate();
            }

        }
    }
}
