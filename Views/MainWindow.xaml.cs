using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Disorder.Views.Pages;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Disorder;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public static IntPtr hWnd;
    public static WindowId myWndId;
    public static SizeInt32 target;
    public static Frame contentframe;
    public MainWindow()
    {
        this.InitializeComponent();
        hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        SizeInt32 t = AppWindow.GetFromWindowId(myWndId).Size;
        target = new SizeInt32(Convert.ToInt32(t.Width / 2.5), Convert.ToInt32(t.Height / 2));
        var apw = AppWindow.GetFromWindowId(myWndId).Presenter as OverlappedPresenter;
        apw.IsResizable = false;
        apw.IsMinimizable = true;
       
        AppWindow.GetFromWindowId(myWndId).Resize(MainWindow.target);

        this.ExtendsContentIntoTitleBar = true;  // 指定自定义标题栏
        this.SetTitleBar(AppTitleBar);
        contentframe = contentFrame;
        contentFrame.NavigateToType(typeof(Login), null, null);
    }

}
