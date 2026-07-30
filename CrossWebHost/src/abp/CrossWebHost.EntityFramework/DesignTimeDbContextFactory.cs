using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWebHost.EntityFramework
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CrossWebHostDbContext>
    {
        public CrossWebHostDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connString = configuration.GetConnectionString("Default");

            var optionsBuilder = new DbContextOptionsBuilder<CrossWebHostDbContext>();
            optionsBuilder.UseSqlite(connString);
            return new CrossWebHostDbContext(optionsBuilder.Options);
        }
    }
}
