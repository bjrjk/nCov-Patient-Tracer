using CefSharp;
using CefSharp.Wpf;
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

namespace nCov_Patient_Tracer.Forms
{
    public partial class frmModifySite : Window
    {
        private class WebCallBack //JavaScript的C#回调类
        {
            public frmModifySite frm;
            public WebCallBack(frmModifySite frm) //构造函数，需要传主窗体引用进来
            {
                this.frm = frm;
            }
            public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
            {
            }

            public void transmit(string str) //JavaScript调用C#的回调函数，传入坐标
            {
                string[] arr=str.Split();
                frm.txtLongitude.Dispatcher.Invoke(
                    new Action(
                        delegate
                        {
                            frm.txtLongitude.Text = arr[0];
                        }
                        )
                    );
                frm.txtLatitude.Dispatcher.Invoke(
                    new Action(
                        delegate
                        {
                            frm.txtLatitude.Text = arr[1];
                        }
                        )
                    );
            }
        }
        public frmModifySite() //主窗体构造函数
        {
            InitializeComponent();
            web.Address = Global.WebURL + "coordinate.php";
            var boundObj = new WebCallBack(this);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            web.JavascriptObjectRepository.Register("bound", boundObj, isAsync: false);
            web.FrameLoadEnd += boundObj.OnFrameLoadEnd;
            RefreshList();
            CreateSiteGUI();
        }

        public void executeJavaScript(string s) //在浏览器中执行JavaScript
        {
            if(web.CanExecuteJavascriptInMainFrame)web.ExecuteScriptAsync(s);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void btnLocate_Click(object sender, RoutedEventArgs e) //btnLocate的Click事件
        {
            LocateOnMap();
        }
        private void LocateOnMap() //根据GUI中的坐标在百度地图中定位
        {
            executeJavaScript(String.Format(@"
                map.clearOverlays();
                var p = new BMap.Point({0}, {1});
                map.centerAndZoom(p, 14);
                var marker = new BMap.Marker(p); 
                map.addOverlay(marker);
                marker.setAnimation(BMAP_ANIMATION_BOUNCE);
            ", txtLongitude.Text, txtLatitude.Text));
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e) //btnSubmit的Click事件
        {
            if (!Algorithm.IsDouble(txtLongitude.Text))
            {
                MessageBox.Show("您输入的经度不是浮点数！请重新输入！","提示信息");
                return;
            }
            if (!Algorithm.IsDouble(txtLatitude.Text))
            {
                MessageBox.Show("您输入的纬度不是浮点数！请重新输入！", "提示信息");
                return;
            }
            Storage storage = Global.storage;
            if (lstLocations.SelectedItems.Count == 0)
            {
                Coordinate coordItem = new Coordinate(
                    double.Parse(txtLongitude.Text), double.Parse(txtLatitude.Text));
                Site siteItem = new Site(storage.siteIncCnt++, coordItem, txtName.Text);
                siteItem.timeSpanCollection = VectorHelper.Str2IntVector(txtTimeSpanID.Text);
                storage.Sites.append(siteItem);
            }
            else
            {
                Site siteItem = Global.storage.Sites[int.Parse(txtID.Text)];
                UpdateSite(siteItem);
            }
            RefreshList();
            CreateSiteGUI();
        }

        private void btnCreateNew_Click(object sender, RoutedEventArgs e) //btnCreateNew的Click事件
        {
            CreateSiteGUI();
        }
        private void CreateSiteGUI() //更新界面GUI到添加新地点
        {
            lstLocations.SelectedItem = null ;
            ClearTXT();
            txtID.Text = (Global.storage.siteIncCnt).ToString();
        }
        private void UpdateSite(Site site) //用GUI信息去更新site中的值
        {
            site.coordinate.longitude = double.Parse(txtLongitude.Text);
            site.coordinate.latitude = double.Parse(txtLatitude.Text);
            site.name = txtName.Text;
            site.timeSpanCollection = VectorHelper.Str2IntVector(txtTimeSpanID.Text);
        }
        private void LoadSite(Site site) //加载site中的信息到窗体GUI中
        {
            txtID.Text = site.ID.ToString();
            txtLongitude.Text = site.coordinate.longitude.ToString();
            txtLatitude.Text = site.coordinate.latitude.ToString();
            txtName.Text = site.name;
            txtTimeSpanID.Text = VectorHelper.IntVector2Str(site.timeSpanCollection);
        }

        private void RefreshList() //更新地点列表
        {
            lstLocations.Items.Clear();
            Storage storage = Global.storage;
            Vector<Site> v = storage.Sites;
            for(int i = 0; i < v.size(); i++)
            {
                ListBoxItem lstItem = new ListBoxItem();
                lstItem.Content = String.Format("[{0},{1}]", v[i].ID, v[i].name);
                lstItem.DataContext = v[i];
                lstLocations.Items.Add(lstItem);
            }
        }
        private void ClearTXT() //清除GUI中文本信息
        {
            txtID.Clear();
            txtName.Clear();
            txtLongitude.Clear();
            txtLatitude.Clear();
            txtTimeSpanID.Clear();
        }
        
        private void lstLocations_SelectionChanged(object sender, SelectionChangedEventArgs e) //lstLocations的SelectionChanged事件
        {
            if (e.AddedItems.Count != 0)
            {
                LoadSite((Site)((ListBoxItem)e.AddedItems[0]).DataContext);
                LocateOnMap();
            }
        }

    }

    
}
