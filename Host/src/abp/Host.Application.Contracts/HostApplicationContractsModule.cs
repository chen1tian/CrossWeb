using Host.Domain;
using System;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Host.Application.Contracts
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(HostDomainMobule),
        typeof(AbpSettingManagementApplicationContractsModule)
        )]
    public class HostApplicationContractsModule : AbpModule
    {
    }
}
