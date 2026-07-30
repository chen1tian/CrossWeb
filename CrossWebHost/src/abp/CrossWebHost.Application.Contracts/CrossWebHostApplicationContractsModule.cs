using CrossWebHost.Domain;
using System;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace CrossWebHost.Application.Contracts
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(CrossWebHostDomainMobule),
        typeof(AbpSettingManagementApplicationContractsModule)
        )]
    public class CrossWebHostApplicationContractsModule : AbpModule
    {
    }
}
