using Host.Domain.Localization.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Host.Domain
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpValidationModule),
        typeof(AbpSettingsModule),
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpLocalizationModule),
        typeof(AbpVirtualFileSystemModule)
    )]
    public class HostDomainMobule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                // "YourRootNameSpace" is the root namespace of your project. It can be empty if your root namespace is empty.
                options.FileSets.AddEmbedded<HostDomainMobule>("Host.Domain");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                //Define a new localization resource (WorkBoxResources)
                options.Resources
                    .Add<HostResources>("zh-Hans")
                    .AddVirtualJson("/Localization/Resources/Host");
            });
        }
    }
}
