using Host.Application;
using Host.Application.Contracts;
using Host.Domain;
using Host.EntityFramework;
using Host.Services;
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

namespace Host
{
    [
        DependsOn(typeof(HostDomainMobule)),
        DependsOn(typeof(HostEntityFrameworkModule)),
        DependsOn(typeof(HostApplicationContractsModule)),
        DependsOn(typeof(HostApplicationModule)),
        DependsOn(typeof(AbpAutoMapperModule)),
    ]
    public class HostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var services = context.Services;

            // 配置数据库
            services.AddAbpDbContext<HostDbContext>();
            context.Services.AddDbContext<HostDbContext>();

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
            services.AddAutoMapperObjectMapper<HostModule>();
            services.AddAutoMapperObjectMapper<HostApplicationModule>();

            var logFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory();

            AutoMapper.IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<HostAutoMapProfile>();
                cfg.AddProfile<HostApplicationAutoMapProfile>();
            }, logFactory);
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<HostModule>(validate: true);
                options.AddMaps<HostApplicationModule>(validate: true);
            });
        }
    }
}
