using System.Diagnostics;
using FufuLauncher.Models;
using FufuLauncher.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace FufuLauncher.Views;

public sealed partial class AccountPage : Page
{
    public AccountViewModel ViewModel
    {
        get;
    }

    public AccountPage()
    {
        ViewModel = App.GetService<AccountViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
        Debug.WriteLine("AccountPage initialized");
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        EntranceStoryboard.Begin();
        
        await Task.Delay(600);
        await ViewModel.LoadUserInfoAsync();
    }
    
    private void AvatarPicture_Loaded(object sender, RoutedEventArgs e)
    {
        AvatarEntranceStoryboard.Begin();
    }

    private void OnSwitchAccountClicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is AccountInfo account)
        {
            ViewModel.SwitchAccountCommand.Execute(account);
        }
    }

    private async void OnDeleteSavedAccountClicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is AccountInfo account)
        {
            var dialog = new ContentDialog
            {
                Title = "删除账号",
                Content = $"确定要删除账号 {account.Nickname} ({account.GameUid}) 吗？\n\n此操作将删除该账号的所有相关数据，包括凭证、祈愿记录和云游戏凭证，且无法恢复。",
                PrimaryButtonText = "删除",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.DeleteSavedAccountCommand.Execute(account);
            }
        }
    }

    private async void OnDeleteCurrentAccountClicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.CurrentAccount == null) return;

        var dialog = new ContentDialog
        {
            Title = "删除当前账号",
            Content = $"确定要删除当前账号 {ViewModel.CurrentAccount.Nickname} ({ViewModel.CurrentAccount.GameUid}) 吗？\n此操作将删除该账号的所有相关数据，且无法恢复。",
            PrimaryButtonText = "删除",
            CloseButtonText = "取消",
            DefaultButton = ContentDialogButton.Close,
            XamlRoot = XamlRoot
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            ViewModel.DeleteAccountCommand.Execute(null);
        }
    }

    private void OnGachaAnalysisClicked(object sender, RoutedEventArgs e)
    {
        // var dialog = new GachaDialog();
        // dialog.XamlRoot = XamlRoot;
        // await dialog.ShowAsync();

        var window = new GachaAnalysisWindow();
        window.Activate();
    }

    private async void OnCopyGameUidClicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string uid && !string.IsNullOrEmpty(uid))
        {
            var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
            dataPackage.SetText(uid);
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);

            var icon = button.Content as FontIcon;
            if (icon != null)
            {
                var originalGlyph = icon.Glyph;
                icon.Glyph = "";
                await Task.Delay(800);
                icon.Glyph = originalGlyph;
            }
        }
    }
}