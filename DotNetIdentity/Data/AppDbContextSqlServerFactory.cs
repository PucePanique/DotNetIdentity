//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace DotNetIdentity.Data;

//public class AppDbContextSqlServerFactory : IDesignTimeDbContextFactory<AppDbContextSqlServer>
//{
//    public AppDbContextSqlServer CreateDbContext(string[] args)
//    {
//        // Construire la configuration comme dans Program.cs
//        var configuration = new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile("appsettings.json", optional: false)
//            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
//            .AddEnvironmentVariables()
//            .Build();

//        var options = new DbContextOptionsBuilder<AppDbContextSqlServer>();

//        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

//        string conn;

//        if (environment.Equals("Production", StringComparison.OrdinalIgnoreCase))
//        {
//            // En production, essayer d'abord la config, puis localhost par défaut
//            conn = configuration.GetConnectionString("SqlServer")
//                   ?? Environment.GetEnvironmentVariable("ConnectionStrings__Default")
//                   ?? "Server=localhost,1433;Database=CesiZEN;User Id=sa;Password=MySecurePassword123!;Encrypt=True;TrustServerCertificate=True";
//        }
//        else
//        {
//            // En development, utiliser l'IP du VPS
//            conn = configuration.GetConnectionString("SqlServer")
//                   ?? Environment.GetEnvironmentVariable("EF_DESIGNTIME_CONN")
//                   ?? "Server=135.125.205.178,1433;Database=CesiZEN;User Id=sa;Password=MySecurePassword123!;Encrypt=True;TrustServerCertificate=True";
//        }

//        options.UseSqlServer(conn);
//        return new AppDbContextSqlServer(options.Options);
//    }
//}

