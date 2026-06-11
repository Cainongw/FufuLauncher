using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using FufuLauncher.Models;

namespace FufuLauncher.Services;

public class DailyNoteCardService
{
    private const string WebSalt = "G1ktdwFL4IyGkHuuWSmz0wUe9Db9scyK";
    private readonly string _deviceId = Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper();
    private readonly Random _random = new();

    public async Task<DailyNoteCardData> LoadCardDataAsync(string roleId, string server, Dictionary<string, string> cookies)
    {
        string cookieStr = string.Join("; ", cookies.Select(kv => $"{kv.Key}={kv.Value}"));
        string json = await FetchApiJsonAsync(roleId, server, cookieStr);
        return DailyNoteParser.Parse(json);
    }

    private async Task<string> FetchApiJsonAsync(string roleId, string server, string cookieStr)
    {
        var apiUrl = $"https://api-takumi-record.mihoyo.com/game_record/app/genshin/api/dailyNote?server={server}&role_id={roleId}";
        Debug.WriteLine($"[DailyNoteCardService] 请求API: {apiUrl}");

        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

        request.Headers.TryAddWithoutValidation("Cookie", cookieStr);
        request.Headers.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");
        request.Headers.TryAddWithoutValidation("x-rpc-app_version", "2.41.0");
        request.Headers.TryAddWithoutValidation("x-rpc-client_type", "5");
        request.Headers.TryAddWithoutValidation("x-rpc-device_id", _deviceId);
        request.Headers.TryAddWithoutValidation("x-rpc-channel", "miyousheluodi");
        request.Headers.TryAddWithoutValidation("Origin", "https://act.mihoyo.com");
        request.Headers.TryAddWithoutValidation("Referer", "https://act.mihoyo.com/");
        request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

        request.Headers.TryAddWithoutValidation("DS", GenerateWebDS());

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private string GenerateWebDS()
    {
        long t = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string r = _random.Next(100001, 200000).ToString();
        string c = CreateMD5($"salt={WebSalt}&t={t}&r={r}");
        return $"{t},{r},{c}";
    }

    private static string CreateMD5(string input)
    {
        byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(input));
        StringBuilder sb = new();
        foreach (byte b in hash) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}