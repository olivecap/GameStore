using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GameStore.API.Data
{
    public static class DataExtension
    {
        //Allow to start automatically migration when you start App
        public static async Task MigrateDBAsync(this WebApplication app)
        {
            // Create scope
            using var scope = app.Services.CreateScope();

            // Get service register in Program?cs file using injection
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

            // Migrate
            await dbContext.Database.MigrateAsync();
        }
    }
}
