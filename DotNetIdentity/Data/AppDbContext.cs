using DotNetIdentity.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DotNetIdentity.Models.DataModels;
using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
using DotNetIdentity.Models.CesiZenModels.Respiration;
using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;

namespace DotNetIdentity.Data
{
    public abstract class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        // EF attend un ctor avec DbContextOptions
        protected AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AppLogsSqLite>? AppLogsSqLite { get; set; }
        public DbSet<AppLogs>? AppLogs { get; set; }
        public DbSet<ApplicationSettings>? AppSettings { get; set; }

        public DbSet<Ressources> Ressources { get; set; } = default!;
        public DbSet<Images> Images { get; set; } = default!;
        public DbSet<RessourcesImages> RessourcesImages { get; set; } = default!;
        public DbSet<Tags> Tags { get; set; } = default!;
        public DbSet<RessourcesTags> RessourcesTags { get; set; } = default!;

        public DbSet<Sessions> Sessions { get; set; } = default!;
        public DbSet<ExerciceConfigurations> ExerciceConfigurations { get; set; } = default!;
        public DbSet<Exercices> Exercices { get; set; } = default!;

        public DbSet<DiagnosticSessions> DiagnosticSessions { get; set; } = default!;
        public DbSet<DiagnosticReponses> DiagnosticReponses { get; set; } = default!;
        public DbSet<DiagnosticEvenements> DiagnosticEvenements { get; set; } = default!;
    }
}
