using System.Security.Cryptography;
using System.Text;

namespace Zorro.Modules.ImgProxy;

public class ImgProxySigner
{
    private string key { get; init; }
    private string salt { get; init; }
    private string endpoint { get; init; }

    public ImgProxySigner(string key, string salt, string endpoint)
    {
        this.key = key;
        this.salt = salt;
        this.endpoint = endpoint;
    }

    public string GetEndpoint() => endpoint;

    public string SignPath(string path)
    {
        var keyBin = HexadecimalStringToByteArray(key);
        var saltBin = HexadecimalStringToByteArray(salt);

        var passwordWithSaltBytes = new List<byte>();
        passwordWithSaltBytes.AddRange(saltBin);
        passwordWithSaltBytes.AddRange(Encoding.UTF8.GetBytes(path));

        using var hmac = new HMACSHA256(keyBin);
        byte[] digestBytes = hmac.ComputeHash(passwordWithSaltBytes.ToArray());
        var urlSafeBase64 = EncodeBase64URLSafeString(digestBytes);
        return $"{urlSafeBase64}{path}";
    }

    static byte[] HexadecimalStringToByteArray(string input)
    {
        var outputLength = input.Length / 2;
        var output = new byte[outputLength];
        using (var sr = new StringReader(input))
        {
            for (var i = 0; i < outputLength; i++)
                output[i] = Convert.ToByte(new string([(char)sr.Read(), (char)sr.Read()]), 16);
        }
        return output;
    }

    static string EncodeBase64URLSafeString(byte[] stream)
        => Convert.ToBase64String(stream).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}