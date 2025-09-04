using DotNetIdentity.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DotNetIdentity.Models.DataModels;
using System.ComponentModel.DataAnnotations.Schema;
using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
using DotNetIdentity.Models.CesiZenModels.Respiration;
using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;

namespace DotNetIdentity.Data
{
    /// <summary>
    /// Application Database context class
    /// </summary>
    [NotMapped]
    public abstract class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        /// <summary>
        /// Property AppLogs
        /// </summary>
        /// <value></value>
        public DbSet<AppLogsSqLite>? AppLogsSqLite { get; set; }
        /// <summary>
        /// Property AppLogs
        /// </summary>
        /// <value></value>
        public DbSet<AppLogs>? AppLogs { get; set; }
        /// <summary>
        /// property AppSettings
        /// </summary>
        /// <value></value>
        public DbSet<ApplicationSettings>? AppSettings { get; set; }
        public DbSet<Ressources> Ressources { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<RessourcesImages> RessourcesImages { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<RessourcesTags> RessourcesTags { get; set; }

        public DbSet<Sessions> Sessions { get; set; }
        public DbSet<ExerciceConfigurations> ExerciceConfigurations { get; set; }
        public DbSet<Exercices> Exercices { get; set; }

        public DbSet<DiagnosticSessions> DiagnosticSessions { get; set; }
        public DbSet<DiagnosticReponses> DiagnosticReponses { get; set; }
        public DbSet<DiagnosticEvenements> DiagnosticEvenements { get; set; }
        /// <summary>
        /// IConfiguration property
        /// </summary>
        protected readonly IConfiguration Configuration;

        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="cnf">type IConfiguration</param>
        /// <returns></returns>
        public AppDbContext(IConfiguration cnf)
        {
            Configuration = cnf;
        }

    }


}