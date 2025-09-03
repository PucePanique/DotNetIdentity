// AppDbContextSqLite.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotNetIdentity.Models;
using DotNetIdentity.Models.DataModels;
using DotNetIdentity.Models.Identity;

namespace DotNetIdentity.Data
{
    public class AppDbContextSqLite : AppDbContext
    {
        public AppDbContextSqLite(DbContextOptions<AppDbContextSqLite> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationSettings>().HasKey(x => new { x.Name, x.Type });
            builder.Entity<ApplicationSettings>().Property(x => x.Value);

            builder.Entity<ApplicationSettings>().HasData(
                new ApplicationSettings { Name = "SessionTimeoutWarnAfter", Type = "GlobalSettings", Value = "50000" },
                new ApplicationSettings { Name = "SessionTimeoutRedirAfter", Type = "GlobalSettings", Value = "70000" },
                new ApplicationSettings { Name = "SessionCookieExpiration", Type = "GlobalSettings", Value = "7" },
                new ApplicationSettings { Name = "ShowMfaEnableBanner", Type = "GlobalSettings", Value = "true" }
            );

            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = "6fbfb682-568c-4f5b-a298-85937ca4f7f3",
                    UserName = "super.admin",
                    NormalizedUserName = "SUPER.ADMIN",
                    PasswordHash = hasher.HashPassword(null!, "Test1000!"),
                    Email = "super.admin@local.app",
                    NormalizedEmail = "SUPER.ADMIN@LOCAL.APP",
                    EmailConfirmed = true,
                    FirstName = "Super",
                    LastName = "Admin",
                    RolesCombined = "Admin",
                    PhoneNumber = "111",
                    Gender = Gender.Unknown
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "dffc6dd5-b145-41e9-a861-c87ff673e9ca",
                    UserId = "6fbfb682-568c-4f5b-a298-85937ca4f7f3"
                }
            );
        }
    }
}
