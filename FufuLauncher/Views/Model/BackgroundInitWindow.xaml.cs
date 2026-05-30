using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;
using FufuLauncher.Helpers;
using AppWindow = Microsoft.UI.Windowing.AppWindow;

namespace FufuLauncher.Views;

public sealed partial class BackgroundInitWindow : Window
{
    public BackgroundInitWindow()
    {
        InitializeComponent();
        
        // 1. 启用 Mica 材质背景
        SystemBackdrop = new MicaBackdrop();
        
        // 2. 沉浸式标题栏（保留系统边框结构以维持 Mica 正常渲染）
        ExtendsContentIntoTitleBar = true;
        
        // 3. 配置窗口属性
        if (AppWindow.Presenter is OverlappedPresenter presenter)
        {
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            presenter.IsResizable = false;
            presenter.IsAlwaysOnTop = true;
        }
        
        // 4. 强制指定窗口物理尺寸
        AppWindow.Resize(new SizeInt32(460, 280));
        
        // 5. 居中显示窗口
        WindowManagerHelper.CenterWindowOnScreen(AppWindow, 460, 280);
    }
}