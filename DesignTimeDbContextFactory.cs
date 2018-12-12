using System.IO;
using ConsoleApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp
{
    /**
    https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dbcontext-creation
    This is to maintain dotnet ef tool working.

    Like running: 
    dotnet ef database update

    it basically provides Db context to the ef tools configured and ready to roll.
    
     */
    public class BloggingContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var config = builder.Build();

    
            var dbConfig  = new DbContextOptionsBuilder<MyContext>()
                .UseSqlServer(config.GetConnectionString("default"));


            return new MyContext(dbConfig.Options);
        }
    }
}