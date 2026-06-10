using System.Diagnostics;
using FufuLauncher.Constants;
using FufuLauncher.Contracts.Services;
using FufuLauncher.Messages;

namespace FufuLauncher.Helpers
{
    public class RedeemCodeReminderService
    {
        private readonly ILocalSettingsService _localSettingsService;

        public RedeemCodeReminderService(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
        }

        public async Task CheckRedeemCodesForTodayAsync(Action<NotificationMessage> showNotificationAction)
        {
            try
            {
                var todayStr = DateTime.Now.ToString("yyyy-MM-dd");
                var lastRemindedObj = await _localSettingsService.ReadSettingAsync("LastRedeemCodeReminderDate");
                
                if (lastRemindedObj != null && lastRemindedObj.ToString() == todayStr)
                {
                    return; 
                }

                bool isOs = false;
                var gamePathObj = await _localSettingsService.ReadSettingAsync("GameInstallationPath");
                if (gamePathObj is string gamePath && !string.IsNullOrEmpty(gamePath))
                {
                    var dir = gamePath;
                    if (System.IO.File.Exists(dir))
                        dir = System.IO.Path.GetDirectoryName(dir) ?? dir;
                    isOs = dir != null && System.IO.File.Exists(System.IO.Path.Combine(dir, "GenshinImpact.exe"));
                }

                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip
                };

                List<RedeemCodeItem>? codesList = null;

                if (isOs)
                {
                    var json = await client.GetStringAsync(ApiEndpoints.RedeemCodesOsUrl);
                    var response = System.Text.Json.JsonSerializer.Deserialize<HoyoCodeResponse>(json, options);
                    codesList = response?.Codes?
                        .Where(c => string.Equals(c.Status, "OK", StringComparison.OrdinalIgnoreCase))
                        .Select(c => new RedeemCodeItem
                        {
                            Title = c.Rewards,
                            Codes = new List<string> { c.Code }
                        })
                        .ToList();
                }
                else
                {
                    var json = await client.GetStringAsync(ApiEndpoints.RedeemCodesUrl);
                    codesList = System.Text.Json.JsonSerializer.Deserialize<List<RedeemCodeItem>>(json, options);
                }

                if (codesList != null && codesList.Count > 0)
                {
                    List<RedeemCodeItem> todaysCodes;

                    if (isOs)
                    {
                        todaysCodes = codesList;
                    }
                    else
                    {
                        todaysCodes = codesList.Where(c =>
                            (!string.IsNullOrEmpty(c.Valid) && c.Valid.Contains(todayStr)) ||
                            (!string.IsNullOrEmpty(c.Time) && c.Time.Contains(todayStr))
                        ).ToList();
                    }

                    if (todaysCodes.Count > 0)
                    {
                        var titles = string.Join("、", todaysCodes.Select(c => c.Title));
                        var codesContent = string.Join("\n", todaysCodes.SelectMany(c => c.Codes));

                        var msg = new NotificationMessage(
                            isOs ? "新兑换码可用" : "兑换码失效提醒",
                            isOs
                                ? $"以下兑换码可用：\n{codesContent}"
                                : $"活动{titles}包含可用兑换码：\n{codesContent}\n请及时前往游戏内使用，否则将会在今天之后失效！",
                            NotificationType.Warning,
                            0
                        );

                        showNotificationAction?.Invoke(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RedeemCodes Reminder] 今日兑换码检查失败: {ex.Message}");
            }
        }
    }
}