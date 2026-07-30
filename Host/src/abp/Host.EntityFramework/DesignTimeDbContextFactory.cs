using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.EntityFramework
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<HostDbContext>
    {
        public HostDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connString = configuration.GetConnectionString("Default");

            var optionsBuilder = new DbContextOptionsBuilder<HostDbContext>();
            optionsBuilder.UseSqlite(connString);
            return new HostDbContext(optionsBuilder.Options);
        }
    }
}
