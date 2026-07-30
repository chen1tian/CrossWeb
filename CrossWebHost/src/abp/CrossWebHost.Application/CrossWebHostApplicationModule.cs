using CrossWebHost.Application.Contracts;
using CrossWebHost.Domain;
using CrossWebHost.EntityFramework;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace CrossWebHost.Application
{
    [DependsOn(
        typeof(CrossWebHostDomainMobule),
        typeof(CrossWebHostEntityFrameworkModule),
        typeof(CrossWebHostApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpSettingManagementApplicationModule)
        )]
    public class CrossWebHostApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            
        }
    }
}
