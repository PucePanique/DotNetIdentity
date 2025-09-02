//using System.Web.Mvc;
//using DotNetIdentity.Data;
//using DotNetIdentity.Models.CesiZenModels.RessourcesModels;
//using DotNetIdentity.Models.CesiZenModels.ViewModels;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
//using static System.Net.Mime.MediaTypeNames;
//using static DotNetIdentity.Data.Tests.AppDbContextTests;

//namespace DotNetIdentity.Controllers.Tests
//{
//    [TestClass()]
//    public class RessourcesControllerTests
//    {

//        private readonly ILogger<UserController> _logger;

//        private TestAppDbContext GetDbContext(string dbName)
//        {
//            var options = new DbContextOptionsBuilder<TestAppDbContext>()
//                .UseInMemoryDatabase(databaseName: dbName)
//                .Options;

//            var config = new ConfigurationBuilder().AddInMemoryCollection().Build();

//            var context = new TestAppDbContext(options, config);

//            // seed
//            context.Ressources.AddRange(
//                new Ressources { Id = 97, Title = "C# Guide", Description = "Learn C#", Url = "url1", Category = "Dev", CreatedBy = "Admin", CreatedAt = DateTime.Now.AddDays(-10), UpdatedBy = "Admin", UpdatedAt = DateTime.Now, Status = true },
//                new Ressources { Id = 98, Title = "ASP.NET Core", Description = "Web Framework", Url = "url2", Category = "Web", CreatedBy = "User", CreatedAt = DateTime.Now.AddDays(-5), UpdatedBy = "User", UpdatedAt = DateTime.Now, Status = true },
//                new Ressources { Id = 99, Title = "EF Core", Description = "ORM", Url = "url3", Category = "DB", CreatedBy = "User", CreatedAt = DateTime.Now, UpdatedBy = "User", UpdatedAt = DateTime.Now, Status = false }
//            );

//            //context.Images.AddRange(
//            //    new Images { Id = 97, Image = "/img1.png" },
//            //    new Images { Id = 98, Image = "/img2.png" },
//            //    new Images { Id = 99, Image = "/img3.png" }
//            //);

//            //context.RessourcesImages.AddRange(
//            //    new RessourcesImages { RessourceId = 97, ImageId = 97 },
//            //    new RessourcesImages { RessourceId = 98, ImageId = 98 },
//            //    new RessourcesImages { RessourceId = 99, ImageId = 99 }
//            //);

//            context.SaveChanges();

//            return context;
//        }


//        [TestMethod]
//        public async Task Index_ShouldReturnAllActiveResources()
//        {
//            var context = GetDbContext("Db1");
//            var controller = new RessourcesController(context, _logger);

//            var result = await controller.Index();

//            var viewResult = result as ViewResult;
//            Assert.IsNotNull(viewResult);

//            var model = viewResult.Model as List<RessourcesVM>;
//            Assert.IsNotNull(model);

//            Assert.AreEqual(2, model.Count); // uniquement actives
//            Assert.AreEqual("", controller.ViewBag.Search);
//            Assert.AreEqual(6, controller.ViewBag.Take);
//            Assert.AreEqual(2, controller.ViewBag.Total);
//        }

//        [TestMethod]
//        public async Task Index_ShouldFilterBySearch()
//        {
//            var context = GetDbContext("Db2");
//            var controller = new RessourcesController(context, _logger);

//            var result = await controller.Index(search: "ASP");

//            var viewResult = result as ViewResult;
//            var model = viewResult.Model as List<RessourcesVM>;

//            Assert.AreEqual(1, model.Count);
//            Assert.AreEqual("ASP.NET Core", model.First().Title);
//        }

//        [TestMethod]
//        public async Task Index_ShouldRespectTakeLimit()
//        {
//            var context = GetDbContext("Db3");
//            var controller = new RessourcesController(context, _logger);

//            var result = await controller.Index(take: 1);

//            var viewResult = result as ViewResult;
//            var model = viewResult.Model as List<RessourcesVM>;

//            Assert.AreEqual(1, model.Count); // limité à 1
//        }
//    }
//}