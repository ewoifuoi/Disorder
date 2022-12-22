using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Disorder.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Options : Page
{
    public Options()
    {
        this.InitializeComponent();
    }

    private void back(object sender, RoutedEventArgs e)
    {
        MainWindow.contentframe.NavigateToType(typeof(Login), null, null);
        AppWindow.GetFromWindowId(MainWindow.myWndId).Resize(MainWindow.target);
    }

    public bool isok = false;
    public string filePath;
    public bool isCancel = false;
    private void Open1(object sender, RoutedEventArgs e)
    {
        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) => {
            //Some work...
            OpenFile("数据库文件", ".db");
            while (isok == false) { if (isCancel) {isCancel = false; break; } Thread.Sleep(100); };
        };
        worker.RunWorkerCompleted += (s, e) => {
            //e.Result"returned" from thread
            if (isCancel == false)
            {

                box1.Text = filePath;
                isok = false;
            }
        };
        worker.RunWorkerAsync();
    }

    private void Open2(object sender, RoutedEventArgs e)
    {
        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) => {
            //Some work...
            OpenFile("数据库文件", ".db");
            while (isok == false) { if (isCancel) { isCancel = false; break; } Thread.Sleep(100); };
        };
        worker.RunWorkerCompleted += (s, e) => {
            //e.Result"returned" from thread
            if (isCancel == false)
            {

                box2.Text = filePath;
                isok = false;
            }
        };
        worker.RunWorkerAsync();
    }

    public async void OpenFile(string a, string b)
    {
        var picker = new FileOpenPicker();
        // 传入MainWindow实例，获取窗口句柄。

        // 将句柄用于初始化Picker。
        WinRT.Interop.InitializeWithWindow.Initialize(picker, MainWindow.hWnd);
        picker.SuggestedStartLocation = PickerLocationId.Desktop;
        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.FileTypeFilter.Add(".db");
        

        // 打开文件选择对话框
        var file = await picker.PickSingleFileAsync();

        // 用户选择图片
        if (file != null)
        {
            filePath = file.Path;
            isok = true;
        }
        else
        {
            isCancel = true;
        }

    }
}
