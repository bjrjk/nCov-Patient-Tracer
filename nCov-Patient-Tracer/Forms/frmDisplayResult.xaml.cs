using CefSharp;
using nCov_Patient_Tracer.DSA;
using nCov_Patient_Tracer.Strcture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TimeSpan = nCov_Patient_Tracer.Strcture.TimeSpan;

namespace nCov_Patient_Tracer.Forms
{
    /// <summary>
    /// frmDisplayResult.xaml 的交互逻辑
    /// </summary>
    public partial class frmDisplayResult : Window
    {
        int personID = 0;
        int siteID = 0;
        static bool queried = false;
        public frmDisplayResult()
        {
            InitializeComponent();
            web.Address = Global.WebURL + "displaymotion.php";
            txtNames.Text = "";
            for (int i = 0; i < Global.personArr.size(); i++)
            {
                txtNames.Text += Global.personArr[i].name + " ";
            }
            bool flag = false;
            for(int i = 0; i < Global.timeSpanArr.size(); i++)
            {
                for(int j = 0; j < Global.timeSpanArr[i].size(); j++)
                {
                    if (Global.timeSpanArr[i][j].size() != 0)
                    {
                        flag = true;
                        break;
                    }
                }
               
            }
            if (!flag)
            {
                MessageBox.Show("没有任何密切接触者信息！", "密接追踪器");
                btnNext.IsEnabled = false;
                btnPrevious.IsEnabled = false;
            }
        }
        private void executeJavaScript(string s)
        {
            web.ExecuteScriptAsync(s);
        }
        private void mapClearOverlay()
        {
            executeJavaScript("map.clearOverlays();");
        }
        private void mapCreateInfoWindow(string title, string content, Coordinate coordinate)
        {
            executeJavaScript(String.Format(@"
                var point = new BMap.Point({0},{1});
                var marker = new BMap.Marker(point);
                map.addOverlay(marker);
                var opts = {{width : 0, height: 0, title : ""{2}""}};
                var infoWindow = new BMap.InfoWindow("" {3} "", opts);
                map.openInfoWindow(infoWindow, point);
            ", coordinate.longitude, coordinate.latitude, title, content));
        }
        private void mapCenterAndZoom(Coordinate coordinate)
        {
            executeJavaScript(String.Format(@"
                var point = new BMap.Point({0},{1});
                map.centerAndZoom(point, 15);
            ", coordinate.longitude, coordinate.latitude));
        }
        private void mapMark(Coordinate coordinate)
        {
            executeJavaScript(String.Format(@"
                var point = new BMap.Point({0},{1});
                var marker = new BMap.Marker(point);
                map.addOverlay(marker);
            ", coordinate.longitude, coordinate.latitude));
        }
        private void mapDrawCircle(string strokeColor, string fillColor, Coordinate coordinate)
        {
            executeJavaScript(String.Format(@"
                var point = new BMap.Point({0},{1});
                var circle = new BMap.Circle(point,100,{{strokeColor:""{2}"",fillColor:""{3}"",strokeWeight:5,strokeOpacity:1,fillOpacity:0.5}});
                map.addOverlay(circle);
            ", coordinate.longitude, coordinate.latitude, strokeColor, fillColor));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random ra = new Random();
            Coordinate c = new Coordinate(116 + ra.NextDouble(), 40 + ra.NextDouble());
            mapClearOverlay();
            mapCenterAndZoom(c);
            mapMark(c);
            mapDrawCircle("red", "blue", c);
            mapCreateInfoWindow("高风险地区", "请勿进入！", c);
        }
        private void LoadNewSite(int personID, int siteID)
        {

            TimeSpan t = Global.timeSpanArr[personID][siteID][0];
            Person p = Global.personArr[personID];
            mapClearOverlay();
            Site s = Global.storage.Sites[t.siteID];
            Coordinate c = s.coordinate;
            mapCenterAndZoom(c);
            mapMark(c);
            mapDrawCircle("red", "blue", c);
            string content = "";
            content += "密切接触者信息：<br>";
            content += "姓名，公司，地址，手机号<br>";
            for (int i = 0; i < Global.timeSpanArr[personID][siteID].size(); i++)
            {
                content += Global.storage.Persons[Global.timeSpanArr[personID][siteID][i].personID].name + "，";
                content += Global.storage.Persons[Global.timeSpanArr[personID][siteID][i].personID].company + "，";
                content += Global.storage.Persons[Global.timeSpanArr[personID][siteID][i].personID].address + "，";
                content += Global.storage.Persons[Global.timeSpanArr[personID][siteID][i].personID].telephone + "<br>";
            }
            mapCreateInfoWindow("<strong>查询“" + p.name + "”在“" + s.name + "”处的密切接触者信息</strong>",
                content, c);
            txtInfos.Text = "当前查询：" + System.Environment.NewLine +
                "姓名：" + p.name + System.Environment.NewLine +
                "地点：" + s.name + System.Environment.NewLine
                ;
        }
        private void QueryIDPlus()
        {
            if (siteID != Global.timeSpanArr[personID].size() - 1)
            {
                siteID++;
                return;
            }
            else siteID = 0;
            if (personID != Global.timeSpanArr.size() - 1)
            {
                personID++;
                return;
            }
            else personID = 0;
        }
        private void QueryIDMinus()
        {
            if (siteID != 0)
            {
                siteID--;
                return;
            }
            else if (personID == 0) //siteID==0
            {
                personID = Global.timeSpanArr.size() - 1;
                siteID = Global.timeSpanArr[personID].size() - 1;
                return;
            }
            else
            {
                personID--;
                siteID = Global.timeSpanArr[personID].size() - 1;
                return;
            }
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (queried) QueryIDMinus();
            queried = true;
            
            while (Global.timeSpanArr[personID][siteID].size() == 0) QueryIDMinus();
            LoadNewSite(personID, siteID);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (queried) QueryIDPlus();
            queried = true;
            while (Global.timeSpanArr[personID][siteID].size() == 0) QueryIDPlus();
            LoadNewSite(personID, siteID);

        }
    }
}
