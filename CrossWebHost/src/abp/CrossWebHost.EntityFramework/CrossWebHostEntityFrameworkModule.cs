using CrossWebHost.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Settings;

namespace CrossWebHost.EntityFramework
{
    [DependsOn(
        typeof(CrossWebHostDomainMobule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqliteModule),
        typeof(AbpAutofacModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule)
        )]
    public class CrossWebHostEntityFrameworkModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CrossWebHostDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlite();
            });
        }
    }
}
