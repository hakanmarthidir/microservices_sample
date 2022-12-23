using Microsoft.EntityFrameworkCore;

namespace identityservice.Infrastructure.Persistence
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IdentityContext>().Database.Migrate();
            }

        }
    }
}
