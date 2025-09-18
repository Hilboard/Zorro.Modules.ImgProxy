using Microsoft.Extensions.DependencyInjection;
using Zorro.Modules.ImgProxy;

namespace Zorro.Services;

public static class ImgProxyService
{
    public delegate ImgProxySettings ImgProxySettingsBuilder(ImgProxySettings settings);
    public static ImgProxySettingsBuilder? SettingsMaster { get; set; }

    public static ImgProxySettings DefaultSettings { get; set; } = new();

    public static IServiceCollection AddImgProxy(this IServiceCollection services)
    {
        ImgProxySettings settings = DefaultSettings;
        if (SettingsMaster is not null)
            DefaultSettings = SettingsMaster.Invoke(settings);

        services.AddScoped(factory =>
        {
            return new ImgProxySigner(settings.key, settings.salt);
        });

        return services;
    }
}