//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DotNetIdentity.Data;
//using DotNetIdentity.Models.CesiZenModels.DiagnosticModels;
//using DotNetIdentity.Models.CesiZenModels.Respiration;
//using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
//using DotNetIdentity.Models.DataModels;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace DotNetIdentity.Data.Tests
//{
//    [TestClass()]
//    public class AppDbContextTests
//    {
//        public class TestAppDbContext : AppDbContext
//        {
//            public TestAppDbContext(DbContextOptions<TestAppDbContext> options, IConfiguration config)
//                : base(config)
//            {
//            }

//            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//            {
//                if (!optionsBuilder.IsConfigured)
//                {
//                    optionsBuilder.UseInMemoryDatabase("DefaultTestDb");
//                }
//            }

//            protected override void OnModelCreating(ModelBuilder modelBuilder)
//            {
//                base.OnModelCreating(modelBuilder);

//                // On ignore les entités inutiles pour nos tests
//                modelBuilder.Ignore<ApplicationSettings>();
//                modelBuilder.Ignore<AppLogs>();
//                modelBuilder.Ignore<AppLogsSqLite>();
//                modelBuilder.Ignore<Tags>();
//                modelBuilder.Ignore<RessourcesTags>();
//                modelBuilder.Ignore<Sessions>();
//                modelBuilder.Ignore<ExerciceConfigurations>();
//                modelBuilder.Ignore<Exercices>();
//                modelBuilder.Ignore<DiagnosticSessions>();
//                modelBuilder.Ignore<DiagnosticReponses>();
//                modelBuilder.Ignore<DiagnosticEvenements>();
//            }

//            // ⚠️ Ici on garde les DbSet utiles pour ton test
//            public DbSet<Ressources> Ressources { get; set; }
//            public DbSet<Images> Images { get; set; }
//            public DbSet<RessourcesImages> RessourcesImages { get; set; }
//        }
//    }
//}