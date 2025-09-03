// AppDbContextSqlServer.cs
using DotNetIdentity.Models;
using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;
using DotNetIdentity.Models.CesiZenModels.Respiration;
using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
using DotNetIdentity.Models.DataModels;
using DotNetIdentity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNetIdentity.Data
{
    public class AppDbContextSqlServer : AppDbContext
    {
        public AppDbContextSqlServer(DbContextOptions<AppDbContextSqlServer> options) : base(options) { }

        public new DbSet<AppLogsSqLite>? AppLogsSqLite { get; set; }
        public new DbSet<AppLogs>? AppLogs { get; set; }
        public new DbSet<ApplicationSettings>? AppSettings { get; set; }

        public new DbSet<Ressources> Ressources { get; set; } = default!;
        public new DbSet<Images> Images { get; set; } = default!;
        public new DbSet<RessourcesImages> RessourcesImages { get; set; } = default!;
        public new DbSet<Tags> Tags { get; set; } = default!;
        public new DbSet<RessourcesTags> RessourcesTags { get; set; } = default!;

        public new DbSet<Sessions> Sessions { get; set; } = default!;
        public new DbSet<ExerciceConfigurations> ExerciceConfigurations { get; set; } = default!;
        public new DbSet<Exercices> Exercices { get; set; } = default!;

        public new DbSet<DiagnosticSessions> DiagnosticSessions { get; set; } = default!;
        public new DbSet<DiagnosticReponses> DiagnosticReponses { get; set; } = default!;
        public new DbSet<DiagnosticEvenements> DiagnosticEvenements { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

         
        builder.Entity<ApplicationSettings>().HasKey(x => new { x.Name, x.Type });
            builder.Entity<ApplicationSettings>().Property(x => x.Value);

            // Global settings
            builder.Entity<ApplicationSettings>().HasData(
                new ApplicationSettings { Name = "SessionTimeoutWarnAfter", Type = "GlobalSettings", Value = "50000" },
                new ApplicationSettings { Name = "SessionTimeoutRedirAfter", Type = "GlobalSettings", Value = "70000" },
                new ApplicationSettings { Name = "SessionCookieExpiration", Type = "GlobalSettings", Value = "7" },
                new ApplicationSettings { Name = "ShowMfaEnableBanner", Type = "GlobalSettings", Value = "true" },

                new ApplicationSettings { Name = "Username", Type = "MailSettings", Value = "YOUR_Smtp_Username" },
                new ApplicationSettings { Name = "Password", Type = "MailSettings", Value = "YOUR_SmtpPassword" },
                new ApplicationSettings { Name = "SmtpServer", Type = "MailSettings", Value = "YOUR_SmtpServer" },
                new ApplicationSettings { Name = "SmtpPort", Type = "MailSettings", Value = "587" },
                new ApplicationSettings { Name = "SmtpUseTls", Type = "MailSettings", Value = "true" },
                new ApplicationSettings { Name = "SmtpFromAddress", Type = "MailSettings", Value = "YOUR_From_Address" },

                new ApplicationSettings { Name = "LdapDomainController", Type = "LdapSettings", Value = "YOUR_Domaincontroller_FQDN" },
                new ApplicationSettings { Name = "LdapDomainName", Type = "LdapSettings", Value = "YOUR_Domainname" },
                new ApplicationSettings { Name = "LdapBaseDn", Type = "LdapSettings", Value = "DC=YOUR,DC=Domain,DC=com" },
                new ApplicationSettings { Name = "LdapGroup", Type = "LdapSettings", Value = "YOUR_Ldap_Group" },
                new ApplicationSettings { Name = "LdapEnabled", Type = "LdapSettings", Value = "true" }
            );

            // Roles
            builder.Entity<AppRole>().HasData(
                new AppRole { Id = "dffc6dd5-b145-41e9-a861-c87ff673e9ca", Name = "Admin", NormalizedName = "ADMIN" },
                new AppRole { Id = "f8a527ac-d7f6-4d9d-aca6-46b2261b042b", Name = "User", NormalizedName = "USER" },
                new AppRole { Id = "g7a527ac-d7t6-4d7z-aca6-45t2261b042b", Name = "Editor", NormalizedName = "EDITOR" },
                new AppRole { Id = "p9a527ac-d77w-4d3r-aca6-35b2261b042b", Name = "Moderator", NormalizedName = "MODERATOR" }
            );

            // Admin user
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
                    IsMfaForce = false,
                    IsLdapLogin = false,
                    IsEnabled = true,
                    RolesCombined = "Admin",
                    PhoneNumber = "111",
                    Gender = Gender.Unknown
                }
            );

            // User  Role
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
