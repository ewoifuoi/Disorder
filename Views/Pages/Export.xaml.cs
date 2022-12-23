using System;
using System.Collections.Generic;
using Disorder.DAL;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Disorder.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Export : Page
{
    public static string dict_name;
    public Export()
    {
        this.InitializeComponent();
        List<List<string>> res = DataAccess.Query("select * \r\nfrom history.sqlite_master\r\nwhere name like 'ts_learn_offline_dotopic_sync_ids%';");
        for(int i = 0; i < res.Count; i++)
        {
            dict.Items.Add(res[i][1]);
        }
    }

    private void back(object sender, RoutedEventArgs e)
    {
        MainWindow.contentframe.NavigateToType(typeof(Options), null, null);
        AppWindow.GetFromWindowId(MainWindow.myWndId).Resize(new Windows.Graphics.SizeInt32(Convert.ToInt32(MainWindow.target.Width), Convert.ToInt32(MainWindow.target.Height * 1.5)));
    }

    private void dict_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string s = dict.SelectedValue.ToString();
        word_count.Text = DataAccess.Query("select count(*)\r\nfrom history." + s + ";")[0][0];

    }

    private async void Next(object sender, RoutedEventArgs e)
    {
        if(dict.SelectedValue == null)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "提示";
            dialog.Content = "请选出待导出词表";
            dialog.PrimaryButtonText = "确定";
            dialog.DefaultButton = ContentDialogButton.Primary;
            var result = await dialog.ShowAsync();
        }
        else
        {
            dict_name = dict.SelectedItem.ToString();
            MainWindow.contentframe.NavigateToType(typeof(ShowList), null, null);
            AppWindow.GetFromWindowId(MainWindow.myWndId).Resize(new Windows.Graphics.SizeInt32(Convert.ToInt32(MainWindow.target.Width * 1.5), Convert.ToInt32(MainWindow.target.Height * 1.6)));
        }
        
    }
}
