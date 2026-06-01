using System.Linq;
using FufuLauncher.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinRT.Interop;

namespace FufuLauncher.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (ViewModel != null)
        {
            await ViewModel.ReloadSettingsAsync();
        }
    }

    private void OnEasterEggClick(object sender, RoutedEventArgs e)
    {
        var window = new Window();
        var page = new EasterEggPage();
        window.Content = page;

        window.ExtendsContentIntoTitleBar = true;
        window.SetTitleBar(page.AppTitleBarElement);

        window.Title = "Philia093";

        IntPtr hWnd = WindowNative.GetWindowHandle(window);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow != null)
        {
            string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico");
            if (File.Exists(iconPath))
            {
                appWindow.SetIcon(iconPath);
            }

            var size = new Windows.Graphics.SizeInt32(1300, 850);
            appWindow.Resize(size);

            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                var centeredX = (displayArea.WorkArea.Width - size.Width) / 2;
                var centeredY = (displayArea.WorkArea.Height - size.Height) / 2;
                appWindow.Move(new Windows.Graphics.PointInt32(centeredX, centeredY));
            }
        }

        window.Closed += (s, args) =>
        {
            page.Cleanup();
        };

        window.Activate();
    }
    
    private void OnOpenAchievementUpdaterClick(object sender, RoutedEventArgs e)
    {
        var updaterWindow = new AchievementUpdaterWindow();
        updaterWindow.ExtendsContentIntoTitleBar = true;

        IntPtr hWnd = WindowNative.GetWindowHandle(updaterWindow);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow != null)
        {
            string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico");
            if (File.Exists(iconPath))
            {
                appWindow.SetIcon(iconPath);
            }

            var size = new Windows.Graphics.SizeInt32(1100, 750);
            appWindow.Resize(size);

            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                var centeredX = (displayArea.WorkArea.Width - size.Width) / 2;
                var centeredY = (displayArea.WorkArea.Height - size.Height) / 2;
                appWindow.Move(new Windows.Graphics.PointInt32(centeredX, centeredY));
            }
        }

        updaterWindow.Activate();
    }
    
    private void OnOpenDatabaseEditorClick(object sender, RoutedEventArgs e)
    {
        var editorWindow = new DatabaseEditorWindow();

        editorWindow.ExtendsContentIntoTitleBar = true;
    
        IntPtr hWnd = WindowNative.GetWindowHandle(editorWindow);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow != null)
        {
            string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico");
            if (File.Exists(iconPath))
            {
                appWindow.SetIcon(iconPath);
            }
            
            var size = new Windows.Graphics.SizeInt32(800, 550);
            appWindow.Resize(size);
            
            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                var centeredX = (displayArea.WorkArea.Width - size.Width) / 2;
                var centeredY = (displayArea.WorkArea.Height - size.Height) / 2;
                appWindow.Move(new Windows.Graphics.PointInt32(centeredX, centeredY));
            }
        }
        
        editorWindow.Activate();
    }
    
    private void OnOpenSponsorWindowClick(object sender, RoutedEventArgs e)
    {
        var sponsorWindow = new SponsorWindow();

        IntPtr hWnd = WindowNative.GetWindowHandle(sponsorWindow);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow != null)
        {
            string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico");
            if (File.Exists(iconPath))
            {
                appWindow.SetIcon(iconPath);
            }

            var size = new Windows.Graphics.SizeInt32(640, 520);
            appWindow.Resize(size);

            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                var centeredX = (displayArea.WorkArea.Width - size.Width) / 2;
                var centeredY = (displayArea.WorkArea.Height - size.Height) / 2;
                appWindow.Move(new Windows.Graphics.PointInt32(centeredX, centeredY));
            }

            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter != null)
            {
                presenter.IsMaximizable = false;
                presenter.IsResizable = false;
            }
        }

        sponsorWindow.Activate();
    }

    private void OnOpenAboutWindowClick(object sender, RoutedEventArgs e)
    {
        var window = new Window();
        var page = new AboutPage();
        window.Content = page;

        window.ExtendsContentIntoTitleBar = true;
        window.SetTitleBar(page.AppTitleBar);

        window.Title = "关于 FufuLauncher";

        try { window.SystemBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop(); } catch { }

        IntPtr hWnd = WindowNative.GetWindowHandle(window);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow != null)
        {
            appWindow.SetIcon("Assets/WindowIcon.ico");

            var size = new Windows.Graphics.SizeInt32(1350, 850);
            appWindow.Resize(size);

            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                var centeredX = (displayArea.WorkArea.Width - size.Width) / 2;
                var centeredY = (displayArea.WorkArea.Height - size.Height) / 2;
                appWindow.Move(new Windows.Graphics.PointInt32(centeredX, centeredY));
            }

            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter != null)
            {
                presenter.IsMaximizable = false;
                presenter.IsResizable = false;
            }
        }

        window.Activate();
    }

    private void OnOpenSecurityAuthClick(object sender, RoutedEventArgs e)
    {
        var authWindow = new SecurityWebWindow("", "https://fu1.fun/dev-auth");
        
        IntPtr hWnd = WindowNative.GetWindowHandle(authWindow);
        var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        if (appWindow != null)
        {
            string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico");
            if (File.Exists(iconPath))
            {
                appWindow.SetIcon(iconPath);
            }

            var size = new Windows.Graphics.SizeInt32(1280, 720);
            appWindow.Resize(size);

            var displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                var centeredX = (displayArea.WorkArea.Width - size.Width) / 2;
                var centeredY = (displayArea.WorkArea.Height - size.Height) / 2;
                appWindow.Move(new Windows.Graphics.PointInt32(centeredX, centeredY));
            }
        }

        authWindow.Activate();
    }

    private bool _isNavigatingFromMenu;
    private DispatcherTimer? _navLockTimer;

    private static readonly string[] _sectionTags =
        { "AppearanceItem", "HomeTextItem", "LanguageItem", "LaunchConfigItem",
          "BackgroundItem", "WindowEffectsItem", "StartupSoundItem",
          "AdvancedOptionsItem", "UpdateItem", "AboutItem", "SecurityAuthItem" };

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        EntranceStoryboard.Begin();

        _isNavigatingFromMenu = true;
        if (SettingsNavigationView.SelectedItem == null)
        {
            SettingsNavigationView.SelectedItem = SettingsNavigationView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault();
        }
        _isNavigatingFromMenu = false;
    }

    private void SettingsNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (_isNavigatingFromMenu) return;

        if (args.SelectedItem is NavigationViewItem selectedItem &&
            selectedItem.Tag is string tag)
        {
            _isNavigatingFromMenu = true;

            // Safety net: clear lock if ViewChanged never fires
            // (happens when element is already visible but can't scroll to top)
            _navLockTimer?.Stop();
            _navLockTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(600) };
            _navLockTimer.Tick += (s, e) =>
            {
                ((DispatcherTimer)s).Stop();
                _isNavigatingFromMenu = false;
            };
            _navLockTimer.Start();

            var element = FindName(tag) as FrameworkElement;
            if (element != null)
            {
                if (element.ActualHeight > 0)
                {
                    BringElementIntoView(element);
                }
                else
                {
                    RoutedEventHandler loadedHandler = null;
                    loadedHandler = (s, e) =>
                    {
                        BringElementIntoView(element);
                        element.Loaded -= loadedHandler;
                    };
                    element.Loaded += loadedHandler;
                }
            }
        }
    }

    private void SettingsScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        if (e.IsIntermediate) return;

        // Scroll from nav click completed — release lock, skip sync
        if (_isNavigatingFromMenu)
        {
            _navLockTimer?.Stop();
            _isNavigatingFromMenu = false;
            return;
        }

        var scrollViewer = (ScrollViewer)sender;
        double anchor = scrollViewer.Padding.Top + 1;
        var visibleTag = (string?)null;

        foreach (var tag in _sectionTags)
        {
            var element = FindName(tag) as FrameworkElement;
            if (element == null) continue;

            var transform = element.TransformToVisual(scrollViewer);
            var position = transform.TransformPoint(new Windows.Foundation.Point(0, 0));

            if (position.Y <= anchor)
            {
                visibleTag = tag;
            }
        }

        if (visibleTag != null)
        {
            _isNavigatingFromMenu = true;
            var targetItem = SettingsNavigationView.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == visibleTag);
            if (targetItem != null && SettingsNavigationView.SelectedItem != targetItem)
            {
                SettingsNavigationView.SelectedItem = targetItem;
            }
            _isNavigatingFromMenu = false;
        }
    }
    private async void OnOpenHDRSettingsClick(object sender, RoutedEventArgs e)
    {
        var dialog = new GenshinHDRLuminanceSettingDialog();
        dialog.XamlRoot = this.XamlRoot;
        await dialog.ShowAsync();
    }
    private void BringElementIntoView(FrameworkElement element)
    {
        if (element == null) return;

        var bringIntoViewOptions = new BringIntoViewOptions
        {
            AnimationDesired = true,
            VerticalAlignmentRatio = 0.0
        };

        element.StartBringIntoView(bringIntoViewOptions);
    }

    public async Task NavigateToUpdateSectionAsync()
    {
        var updateNavItem = SettingsNavigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag?.ToString() == "UpdateItem");

        if (updateNavItem != null)
        {
            SettingsNavigationView.SelectedItem = updateNavItem;
        }

        await Task.Delay(120);

        if (UpdateItem != null)
        {
            BringElementIntoView(UpdateItem);
        }

        await Task.Delay(120);

        CheckUpdateButton?.Focus(FocusState.Programmatic);
    }
}