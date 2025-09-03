using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotNetIdentity.Data;

public class AppDbContextSqlServerFactory : IDesignTimeDbContextFactory<AppDbContextSqlServer>
{
    public AppDbContextSqlServer CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContextSqlServer>();

        // Chaine design-time: n’a pas besoin d’être joignable, juste valide syntactiquement
        var conn = Environment.GetEnvironmentVariable("EF_DESIGNTIME_CONN")
                   ?? "Server=localhost,1433;Database=DesignTime;User Id=sa;Password=GAfGKe5h4TPng@K&;Encrypt=True;TrustServerCertificate=True";

        options.UseSqlServer(conn);
        return new AppDbContextSqlServer(options.Options);
    }
}
