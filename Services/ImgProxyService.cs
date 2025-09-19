using Microsoft.Extensions.DependencyInjection;
using Zorro.Modules.ImgProxy;

namespace Zorro.Services;

public static class ImgProxyService
{
    public delegate ImgProxySettings ImgProxySettingsBuilder(ImgProxySettings settings);
    public static ImgProxySettingsBuilder? SettingsMaster { get; set; }

    public static IServiceCollection AddImgProxy(this IServiceCollection services)
    {
        if (SettingsMaster is null)
            throw new ArgumentNullException(nameof(SettingsMaster));
        ImgProxySettings settings = SettingsMaster.Invoke(new());

        services.AddScoped(factory =>
        {
            return new ImgProxySigner(settings.key, settings.salt, settings.endpoint);
        });

        return services;
    }
}