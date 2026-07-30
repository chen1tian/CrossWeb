using CrossWebHost.Application;
using CrossWebHost.Application.Contracts;
using CrossWebHost.Domain;
using CrossWebHost.EntityFramework;
using CrossWebHost.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace CrossWebHost
{
    [
        DependsOn(typeof(CrossWebHostDomainMobule)),
        DependsOn(typeof(CrossWebHostEntityFrameworkModule)),
        DependsOn(typeof(CrossWebHostApplicationContractsModule)),
        DependsOn(typeof(CrossWebHostApplicationModule)),
        DependsOn(typeof(AbpAutoMapperModule)),
    ]
    public class CrossWebHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            // 配置数据库
            services.AddAbpDbContext<CrossWebHostDbContext>();
            context.Services.AddDbContext<CrossWebHostDbContext>();

            // 配置本地 Web API 服务
            services.AddSingleton<PicoServerService>();

            // 配置AutoMapper
            ConfigureAutoMap(context.Services);
        }

        /// <summary>
        /// 配置AutoMapper
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureAutoMap(IServiceCollection services)
        {
            // 对象映射
            services.AddAutoMapperObjectMapper<CrossWebHostModule>();
            services.AddAutoMapperObjectMapper<CrossWebHostApplicationModule>();

            var logFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory();

            AutoMapper.IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CrossWebHostAutoMapProfile>();
                cfg.AddProfile<CrossWebHostApplicationAutoMapProfile>();
            }, logFactory);
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CrossWebHostModule>(validate: true);
                options.AddMaps<CrossWebHostApplicationModule>(validate: true);
            });
        }
    }
}
