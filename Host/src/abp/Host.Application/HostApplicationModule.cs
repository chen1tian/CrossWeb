using Host.Application.Contracts;
using Host.Domain;
using Host.EntityFramework;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Host.Application
{
    [DependsOn(
        typeof(HostDomainMobule),
        typeof(HostEntityFrameworkModule),
        typeof(HostApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpSettingManagementApplicationModule)
        )]
    public class HostApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
        }
    }
}
