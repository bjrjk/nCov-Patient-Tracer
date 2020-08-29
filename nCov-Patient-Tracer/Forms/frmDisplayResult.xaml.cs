using CefSharp;
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

namespace nCov_Patient_Tracer.Forms
{
    /// <summary>
    /// frmDisplayResult.xaml 的交互逻辑
    /// </summary>
    public partial class frmDisplayResult : Window
    {
        public frmDisplayResult()
        {
            InitializeComponent();
            web.Address = Global.WebURL + "displaymotion.php";
        }
        private void executeJavaScript(string s)
        {
            web.ExecuteScriptAsync(s);
        }
        private void mapClearOverlay()
        {
            executeJavaScript("map.clearOverlays();");
        }
        private void mapCreateInfoWindow(string title,string content,Coordinate coordinate)
        {
            executeJavaScript(String.Format(@"
                var point = new BMap.Point({0},{1});
                var marker = new BMap.Marker(point);
                map.addOverlay(marker);
                var opts = {{width : 0, height: 0, title : ""{2}""}};
                var infoWindow = new BMap.InfoWindow("" {3} "", opts);
                map.openInfoWindow(infoWindow, point);
            ", coordinate.longitude,coordinate.latitude,title,content));
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
        private void mapDrawCircle(string strokeColor,string fillColor, Coordinate coordinate)
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
            Coordinate c = new Coordinate(116+ra.NextDouble(), 40 + ra.NextDouble());
            mapClearOverlay();
            mapCenterAndZoom(c);
            mapMark(c);
            mapDrawCircle("red","blue", c);
            mapCreateInfoWindow("高风险地区", "请勿进入！", c);
        }
    }
}
