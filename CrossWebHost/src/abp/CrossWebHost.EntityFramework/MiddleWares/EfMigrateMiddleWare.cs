using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossWebHost.EntityFramework.MiddleWares
{
    public static class EfMigrateMiddleWare
    {
        /// <summary>
        /// 自动迁移数据库
        /// </summary>
        /// <param name="app"></param>
        /// <param name="db"></param>
        public static void UseAutoMigration(this IServiceCollection services, DbContext db)
        {
            MigrationDb(db);
        }

        /// <summary>
        /// 自动迁移数据库
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="app"></param>
        public static void UseAutoMigration<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            using (var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
                services.UseAutoMigration(dbContext);
            }
        }

        /// <summary>
        /// 自动迁移数据库
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="servicesProvider"></param>
        public static void UseAutoMigration<TDbContext>(this IServiceProvider servicesProvider)
            where TDbContext : DbContext
        {
            using (var serviceScope = servicesProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = servicesProvider.GetRequiredService<TDbContext>();
                MigrationDb(dbContext);
            }
        }

        public static void MigrationDb(DbContext db)
        {
            // 自动迁移数据库 
            if (db.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine($"{db.GetType().Name} Migrating...");

                db.Database.Migrate();

                Console.WriteLine($"{db.GetType().Name} Database Migrated");
            }
            else
            {
                Console.WriteLine($"{db.GetType().Name} No Need Migrate");
            }

            Console.WriteLine($"{db.GetType().Name} Check Migrations Complete!");
            Console.WriteLine("");
        }
    }
}
