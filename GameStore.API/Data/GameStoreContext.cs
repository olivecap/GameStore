using GameStore.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data
{
    public class GameStoreContext(DbContextOptions<GameStoreContext> options)
        : DbContext(options)
    {
        // Create Game DB table
        public DbSet<Game> Games => Set<Game>();

        // Create Genre DB table
        public DbSet<Genre> Genres => Set<Genre>();

        //Pre fill some table in data base
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new { Id = 1,Name = "Fighting" },
                new { Id = 2, Name = "RolePlaying" },
                new { Id = 3, Name = "Sports" },
                new { Id = 4, Name = "Racing" },
                new { Id = 5, Name = "Kids and Family" }
            );
        }
    }
}
