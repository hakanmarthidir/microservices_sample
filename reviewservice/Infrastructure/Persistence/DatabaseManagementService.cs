using Microsoft.EntityFrameworkCore;
using reviewservice.Infrastructure.Persistence;

namespace reviewservice.Infrastructure.Persistence
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ReviewContext>().Database.Migrate();
            }

        }
    }
}
