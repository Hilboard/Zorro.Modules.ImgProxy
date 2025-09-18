using Zorro.Modules.ImgProxy;
using Zorro.Services;

namespace Zorro.Query.SignalR;

public static class ResizeImageQuery
{
    public static HttpQueryContext ResizeImage(
        this HttpQueryContext context,
        string imagePath,
        (int, int) resizeDimensions,
        out string requestQueryPath,
        string outputFormat = "webp"
    )
    {                                       
        int x = resizeDimensions.Item1,
            y = resizeDimensions.Item2;
        string queryPath = $"/rs:fit:{x}:{y}/plain/{imagePath}@{outputFormat}";

        var signer = context.GetService<ImgProxySigner>();
        string signedQueryPath = signer.SignPath(queryPath);

        requestQueryPath = Path.Combine(ImgProxyService.DefaultSettings.endpoint, signedQueryPath);

        return context;
    }
}