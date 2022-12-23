using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using Disorder.DAL;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Disorder.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ShowList : Page
{

    private static List<List<string>> GetDisruptedItems(List<List<string>> source)
    {
        List<List<string>> temp = new List<List<string>>();
        for (int i = 0; i < source.Count; i++)
        {
            temp.Add(source[i]);
        }
        Random rand = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < temp.Count; i++)
        {
            for (int j = i + 1; j < temp.Count; j++)
            {

                if (rand.Next(2) == 1)
                {
                    List<string> t = temp[i];
                    temp[i] = temp[j];
                    temp[j] = t;
                }
            }
        }
        return temp;
    }

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


    public string filePath = "";
    public async void SaveTXTFile(string a, string b)
    {
        var savePicker = new FileSavePicker();
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, MainWindow.hWnd);
        savePicker.SuggestedStartLocation =
            Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
        savePicker.FileTypeChoices.Add(a, new List<string>() { b });
        Random rnd = new Random();
        savePicker.SuggestedFileName = "乱序单词检测卷(汉译英)" + ddtt;

        // 打开文件选择对话框
        var file = await savePicker.PickSaveFileAsync();
        if (file != null)
        {
            filePath = file.Path; // 暂存绝对路径
            
            isok = true;
            canceled = false;
        }
        else canceled = true;

    }
    public bool isok = false;
    private bool canceled = false;
    public string ddtt;
    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "提示";
        dialog.Content = "该操作将生成两个pdf文件";

        dialog.PrimaryButtonText = "确定";
        dialog.CloseButtonText = "取消";

        dialog.DefaultButton = ContentDialogButton.Primary;
        var result = await dialog.ShowAsync();
        BackgroundWorker worker = new BackgroundWorker();
        PdfDocument doc = new PdfDocument();
        PdfDocument doc2 = new PdfDocument();
        bool isfirst = true;
        ddtt = DateTime.Now.ToString("ffffff");
        if (result == ContentDialogResult.Primary)
        {
            isok = false;
            worker.Dispose();

            worker.DoWork += (s, e) => {
                //Some work...
                SaveTXTFile("PDF文件", ".pdf");

                while (isok == false) { if (canceled) { break; } Thread.Sleep(100); };
            };
            worker.RunWorkerCompleted += (s, e) => {
                if (!canceled)
                {
                    int i = 0;
                    while(i < res.Count)
                    {
                        PdfPageBase page = doc.Pages.Add();
                        PdfPageBase page2 = doc2.Pages.Add();
                        PdfTrueTypeFont pdfTrueTypeFont = new PdfTrueTypeFont(new Font("Microsoft Yahei", 10), true);
                        PdfTrueTypeFont pdfTrueTypeFont0 = new PdfTrueTypeFont(new Font("Microsoft Yahei", 25), true);
                        PdfTrueTypeFont pdfTrueTypeFont1 = new PdfTrueTypeFont(new Font("Microsoft Yahei", 8), true);
                        PdfSolidBrush pdfSolidBrush = new PdfSolidBrush(Color.Black);
                        
                        res = GetDisruptedItems(res);
                        string s1 = ""; string s2 = ""; string s3 = ""; string s4 = "";
                        for (int j = 0; j < 50 && i < res.Count; i++,j++)
                        {
                            if (j <= 24)
                            {
                                s1 += (i + 1).ToString() + ". " + (res[i][1].Length <= 15 ? res[i][1] : res[i][1].Substring(0, 15)) + "\n\n";
                                s3 += (i + 1).ToString() + ". " + res[i][0] + "\n\n";
                            }
                            else
                            {
                                s2 += (i + 1).ToString() + ". " + (res[i][1].Length <= 15 ? res[i][1] : res[i][1].Substring(0, 15)) + "\n\n";
                                s4 += (i + 1).ToString() + ". " + res[i][0] + "\n\n";
                            }
                        }
                        if(isfirst)
                        {
                            page.Canvas.DrawString("编号：" + ddtt, pdfTrueTypeFont1, PdfBrushes.Black, new RectangleF(400, 10, page.GetClientSize().Width, page.GetClientSize().Height));
                            page.Canvas.DrawString("百词斩乱序单词拼写检测卷(汉译英)", pdfTrueTypeFont0, PdfBrushes.Black, new RectangleF(70, 30, page.GetClientSize().Width, page.GetClientSize().Height));
                            page.Canvas.DrawString("生成日期: " + DateTime.Now.ToString("yyyy-MM-dd dddd"), pdfTrueTypeFont, PdfBrushes.Black, new RectangleF(300, 70, page.GetClientSize().Width, page.GetClientSize().Height));
                            page2.Canvas.DrawString("编号：" + ddtt, pdfTrueTypeFont1, PdfBrushes.Black, new RectangleF(400, 10, page.GetClientSize().Width, page.GetClientSize().Height));
                            page2.Canvas.DrawString("百词斩乱序单词拼写检测卷(英译汉)", pdfTrueTypeFont0, PdfBrushes.Black, new RectangleF(70, 30, page2.GetClientSize().Width, page2.GetClientSize().Height));
                            page2.Canvas.DrawString("生成日期: " + DateTime.Now.ToString("yyyy-MM-dd dddd"), pdfTrueTypeFont, PdfBrushes.Black, new RectangleF(300, 70, page.GetClientSize().Width, page2.GetClientSize().Height));
                            isfirst = false;
                        }
                        page.Canvas.DrawString(s1, pdfTrueTypeFont, PdfBrushes.Black, new RectangleF(0, 100, page.GetClientSize().Width, page.GetClientSize().Height));
                        page.Canvas.DrawString(s2, pdfTrueTypeFont, PdfBrushes.Black, new RectangleF(page.GetClientSize().Width / 2 + 2f, 100, page.GetClientSize().Width, page.GetClientSize().Height));
                        page2.Canvas.DrawString(s3, pdfTrueTypeFont, PdfBrushes.Black, new RectangleF(0, 100, page2.GetClientSize().Width / 2 - 2f, page2.GetClientSize().Height));
                        page2.Canvas.DrawString(s4, pdfTrueTypeFont, PdfBrushes.Black, new RectangleF(page2.GetClientSize().Width / 2 + 2f, 100, page2.GetClientSize().Width / 2, page2.GetClientSize().Height));

                    }
                    doc.SaveToFile(filePath);
                    doc.Close();
                    doc2.SaveToFile(filePath.Substring(0,11) + "(英译汉)"+ddtt+".pdf");
                    doc2.Close();

                }
                else
                {
                    canceled = false;
                }
            };
            worker.RunWorkerAsync();

        }
        //创建PdfDocument类的对象，并加载PDF文档


    }
}
