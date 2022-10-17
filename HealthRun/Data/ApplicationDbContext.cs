using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HealthRun.Models;

namespace HealthRun.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<UserFollowing> Followers { get; set; }
        public DbSet<UserFollowing> Followees{ get; set; }
        public DbSet<ClubAttendees> ClubAttendees { get; set; }
        public DbSet<RaceAttendees> RaceAttendees { get; set; }
    }
}
