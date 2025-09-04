using System.Configuration;
using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;
using DotNetIdentity.Models.CesiZenModels.Respiration;
using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
using DotNetIdentity.Models.DataModels;
using DotNetIdentity.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetIdentity.Data
{
    public abstract class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        // EF attend un ctor avec DbContextOptions
        protected AppDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<AppLogsSqLite>? AppLogsSqLite { get; set; }
        public virtual DbSet<AppLogs>? AppLogs { get; set; }
        public virtual DbSet<ApplicationSettings>? AppSettings { get; set; }

        public virtual DbSet<Ressources> Ressources { get; set; } = default!;
        public virtual DbSet<Images> Images { get; set; } = default!;
        public virtual DbSet<RessourcesImages> RessourcesImages { get; set; } = default!;
        public virtual DbSet<Tags> Tags { get; set; } = default!;
        public virtual DbSet<RessourcesTags> RessourcesTags { get; set; } = default!;

        public virtual DbSet<Sessions> Sessions { get; set; } = default!;
        public virtual DbSet<ExerciceConfigurations> ExerciceConfigurations { get; set; } = default!;
        public virtual DbSet<Exercices> Exercices { get; set; } = default!;

        public virtual DbSet<DiagnosticSessions> DiagnosticSessions { get; set; } = default!;
        public virtual DbSet<DiagnosticReponses> DiagnosticReponses { get; set; } = default!;
        public virtual DbSet<DiagnosticEvenements> DiagnosticEvenements { get; set; } = default!;
    }
}
