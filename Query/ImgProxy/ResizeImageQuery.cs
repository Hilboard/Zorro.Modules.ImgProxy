using Zorro.Modules.ImgProxy;

namespace Zorro.Query.SignalR;

public static class ResizeImageQuery
{
    public static HttpQueryContext ResizeImage(
        this HttpQueryContext context,
        string imagePath,
        ThumbSizePresets thumbSize,
        out string requestQueryUri,
        out string thumbFileName,
        string outputFormat = "webp"
    )
    {
        int edgeSize = (int)thumbSize;
        return context.ResizeImage(imagePath, (edgeSize, edgeSize), out requestQueryUri, out thumbFileName, outputFormat);
    }

    public static HttpQueryContext ResizeImage(
        this HttpQueryContext context,
        string imagePath,
        (int, int) resizeDimensions,
        out string requestQueryUri,
        out string newFileName,
        string outputFormat = "webp"
    )
    {
        int x = resizeDimensions.Item1,
            y = resizeDimensions.Item2;
        string queryPath = $"/rs:fit:{x}:{y}/plain/{imagePath}@{outputFormat}";

        var signer = context.GetService<ImgProxySigner>();
        string signedQueryPath = signer.SignPath(queryPath);

        requestQueryUri = Path.Combine(signer.GetEndpoint(), signedQueryPath);
        newFileName = $"{Path.GetFileNameWithoutExtension(imagePath)}_rs{x}x{y}.{outputFormat}";

        context.TryLogElapsedTime(nameof(ResizeImageQuery));
        return context;
    }
}