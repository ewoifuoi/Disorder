using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Disorder.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Login : Page
{
    public Login()
    {
        this.InitializeComponent();
    }

    private async void start_other(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();
        
        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "悲报";
        dialog.Content = "该功能程序员小哥哥正在开发中qwq";
        dialog.PrimaryButtonText = "确定";
        dialog.DefaultButton = ContentDialogButton.Primary;
        var result = await dialog.ShowAsync();
    }

    private void start_bcz(object sender, RoutedEventArgs e)
    {
        AppWindow.GetFromWindowId(MainWindow.myWndId).Resize(new Windows.Graphics.SizeInt32(Convert.ToInt32(MainWindow.target.Width), Convert.ToInt32(MainWindow.target.Height*1.5)));
        MainWindow.contentframe.NavigateToType(typeof(Options), null, null);
    }
}
