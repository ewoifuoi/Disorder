using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Disorder.DAL;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Disorder.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ShowList : Page
{
    List<List<string>> res;
    public ShowList()
    {
        this.InitializeComponent();
        res = DataAccess.Query("select word,mean_cn from "+Export.dict_name+" as t,\r\n(select * from main.dict_a_b\r\nunion select * from main.dict_c\r\nunion select * from main.dict_d_f\r\nunion select * from main.dict_g_k\r\nunion select * from main.dict_l_o\r\nunion select * from main.dict_p_r\r\nunion select * from main.dict_s\r\nunion select * from main.dict_t_z) A\r\nwhere t.topic_id=A.topic_id and t.topic_day < 1 and t.is_today_new=1;");
        for(int i = 0; i < res.Count; i++)
        {
            TextBlock tb = new TextBlock { Text = res[i][0] + "\t\t" + res[i][1], Foreground = new SolidColorBrush(Colors.LightGray) };
            lv.Items.Add(tb);
        }
        count.Text = res.Count.ToString();
    }
    private void back(object sender, RoutedEventArgs e)
    {
        MainWindow.contentframe.NavigateToType(typeof(Export), null, null);
        AppWindow.GetFromWindowId(MainWindow.myWndId).Resize(new Windows.Graphics.SizeInt32(Convert.ToInt32(MainWindow.target.Width * 0.9), Convert.ToInt32(MainWindow.target.Height * 1.8)));
    }

    private void cal_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
    {
        lv.Items.Clear();
        res = new List<List<string>>();
        for(int i = 0; i < cal.SelectedDates.Count; i++)
        {
            DateTime dt = cal.SelectedDates[i].DateTime;
            int t = (Convert.ToInt32(DateTime.Now.ToString("dd")) - Convert.ToInt32(dt.ToString("dd")));
            if(t == 0)
            {
                List<List<string>> l = DataAccess.Query("select word,mean_cn,create_at from "+Export.dict_name+" as t,\r\n(select * from main.dict_a_b\r\nunion select * from main.dict_c\r\nunion select * from main.dict_d_f\r\nunion select * from main.dict_g_k\r\nunion select * from main.dict_l_o\r\nunion select * from main.dict_p_r\r\nunion select * from main.dict_s\r\nunion select * from main.dict_t_z) A\r\nwhere t.topic_id=A.topic_id and is_today_new=1;");
                for(int j = 0; j < l.Count; j++)
                {
                    res.Add(l[j]);
                }
            }
            else if(t==1)
            {
                List<List<string>> l = DataAccess.Query("select word,mean_cn,create_at from "+Export.dict_name+" as t,\r\n(select * from main.dict_a_b\r\nunion select * from main.dict_c\r\nunion select * from main.dict_d_f\r\nunion select * from main.dict_g_k\r\nunion select * from main.dict_l_o\r\nunion select * from main.dict_p_r\r\nunion select * from main.dict_s\r\nunion select * from main.dict_t_z) A\r\n,(select A.topic_id, min(create_at) as ct from ts_learn_offline_dotopic_sync_ids_559 as t,\r\n(select * from main.dict_a_b\r\nunion select * from main.dict_c\r\nunion select * from main.dict_d_f\r\nunion select * from main.dict_g_k\r\nunion select * from main.dict_l_o\r\nunion select * from main.dict_p_r\r\nunion select * from main.dict_s\r\nunion select * from main.dict_t_z) A\r\nwhere t.topic_id=A.topic_id and is_today_new=1) B\r\nwhere t.topic_id=A.topic_id  and B.ct-t.create_at <= 100000000 and B.ct-t.create_at >= 50000000 ;");
                for (int j = 0; j < l.Count; j++)
                {
                    res.Add(l[j]);
                }
            }
            else
            {
                string s1 = t.ToString();
                string s2 = (t + 1).ToString();
                List<List<string>> l = DataAccess.Query("select word,mean_cn,create_at from "+Export.dict_name+" as t,\r\n(select * from main.dict_a_b\r\nunion select * from main.dict_c\r\nunion select * from main.dict_d_f\r\nunion select * from main.dict_g_k\r\nunion select * from main.dict_l_o\r\nunion select * from main.dict_p_r\r\nunion select * from main.dict_s\r\nunion select * from main.dict_t_z) A\r\n,(select A.topic_id, min(create_at) as ct from ts_learn_offline_dotopic_sync_ids_559 as t,\r\n(select * from main.dict_a_b\r\nunion select * from main.dict_c\r\nunion select * from main.dict_d_f\r\nunion select * from main.dict_g_k\r\nunion select * from main.dict_l_o\r\nunion select * from main.dict_p_r\r\nunion select * from main.dict_s\r\nunion select * from main.dict_t_z) A\r\nwhere t.topic_id=A.topic_id and is_today_new=1) B\r\nwhere t.topic_id=A.topic_id  and B.ct-t.create_at <= "+s2+"00000000 and B.ct-t.create_at >= "+s1+"00000000 ;");
                for (int j = 0; j < l.Count; j++)
                {
                    res.Add(l[j]);
                }
            }
        }
        for (int i = 0; i < res.Count; i++)
        {
            TextBlock tb = new TextBlock { Text = res[i][0] + "\t\t" + res[i][1], Foreground = new SolidColorBrush(Colors.LightGray) };
            lv.Items.Add(tb);
        }
        count.Text = res.Count.ToString();
    }
}
