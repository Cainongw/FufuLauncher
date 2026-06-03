using System.Text.Json;
using FufuLauncher.Contracts.Services;
using FufuLauncher.Models;
using MihoyoBBS;

namespace FufuLauncher.Services;

public interface IUserConfigService
{
    Task<UserDisplayConfig> LoadDisplayConfigAsync();
    Task SaveDisplayConfigAsync(UserDisplayConfig config);
}

public class UserConfigService : IUserConfigService
{
    private readonly ILocalSettingsService _localSettingsService;

    public UserConfigService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task<UserDisplayConfig> LoadDisplayConfigAsync()
    {
        try
        {
            var activeFileObj = await _localSettingsService.ReadSettingAsync("ActiveConfigFile");
            string activeFile = activeFileObj?.ToString() ?? "config.json";
            var configPath = Path.Combine(Helpers.AppPaths.DataDir, activeFile);

            if (!File.Exists(configPath))
            {
                return new UserDisplayConfig();
            }

            var json = await File.ReadAllTextAsync(configPath);
            var config = JsonSerializer.Deserialize<Config>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (config?.Display == null)
            {
                return new UserDisplayConfig();
            }

            return new UserDisplayConfig
            {
                Nickname = config.Display.Nickname,
                GameUid = config.Display.GameUid,
                Server = config.Display.Server,
                AvatarUrl = config.Display.AvatarUrl,
                Level = config.Display.Level,
                Sign = config.Display.Sign,
                IpRegion = config.Display.IpRegion,
                Gender = config.Display.Gender,
                HasBoundRole = config.Display.HasBoundRole
            };
        }
        catch
        {
            return new UserDisplayConfig();
        }
    }

    public async Task SaveDisplayConfigAsync(UserDisplayConfig display)
    {
        try
        {
            var activeFileObj = await _localSettingsService.ReadSettingAsync("ActiveConfigFile");
            string activeFile = activeFileObj?.ToString() ?? "config.json";
            var configPath = Path.Combine(Helpers.AppPaths.DataDir, activeFile);

            if (!File.Exists(configPath))
            {
                return;
            }

            var json = await File.ReadAllTextAsync(configPath);
            var config = JsonSerializer.Deserialize<Config>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (config == null) return;

            config.Display = new DisplayConfig
            {
                Nickname = display.Nickname,
                GameUid = display.GameUid,
                Server = display.Server,
                AvatarUrl = display.AvatarUrl,
                Level = display.Level,
                Sign = display.Sign,
                IpRegion = display.IpRegion,
                Gender = display.Gender,
                HasBoundRole = display.HasBoundRole
            };

            var newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(configPath, newJson);
        }
        catch (Exception ex)
        {
            throw new IOException($"保存用户显示配置失败: {ex.Message}");
        }
    }
}