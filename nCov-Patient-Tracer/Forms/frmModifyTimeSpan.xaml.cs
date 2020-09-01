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
    /// <summary>
    /// frmModifySite.xaml 的交互逻辑
    /// </summary>
    public partial class frmModifyTimeSpan : Window
    {
        public frmModifyTimeSpan()
        {
            InitializeComponent();
            RefreshList();
            CreateTimeSpanGUI();
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Storage storage = Global.storage;
            if (int.Parse(txtPersonID.Text) >= storage.Persons.size())
            {
                MessageBox.Show("输入人员ID错误！请核对后重试！", "提示信息");
                return;
            }
            if (int.Parse(txtSiteID.Text) >= storage.Sites.size())
            {
                MessageBox.Show("输入地点ID错误！请核对后重试！", "提示信息");
                return;
            }
            if (lstPeople.SelectedItems.Count == 0)
            {
                Strcture.TimeSpan p = new Strcture.TimeSpan(storage.timespanIncCnt++,
                    int.Parse(txtStartHour.Text), int.Parse(txtEndHour.Text),
                    int.Parse(txtPersonID.Text), int.Parse(txtSiteID.Text),
                    chkIsProtected.IsChecked == true);
                storage.TimeSpans.append(p);
                storage.Persons[int.Parse(txtPersonID.Text)].timeSpanCollection.append(p.ID);
                storage.Sites[int.Parse(txtSiteID.Text)].timeSpanCollection.append(p.ID);
            }
            else
            {
                Strcture.TimeSpan p = Global.storage.TimeSpans[int.Parse(txtID.Text)];
                UpdateTimeSpan(p);
            }
            RefreshList();
            CreateTimeSpanGUI();
        }
        private void btnCreateNew_Click(object sender, RoutedEventArgs e)
        {
            CreateTimeSpanGUI();
        }
        private void CreateTimeSpanGUI()
        {
            lstPeople.SelectedItem = null;
            ClearTXT();
            txtID.Text = (Global.storage.timespanIncCnt).ToString();
        }
        private void UpdateTimeSpan(Strcture.TimeSpan t)
        {
            Vector<int> PersonTimeSpanArr = Global.storage.Persons[t.personID].timeSpanCollection;
            for(int i=0;i< PersonTimeSpanArr.size(); i++)
            {
                if (PersonTimeSpanArr[i] == t.ID)
                {
                    PersonTimeSpanArr.remove(i);
                    break;
                }
            }
            Vector<int> SiteTimeSpanArr = Global.storage.Sites[t.siteID].timeSpanCollection;
            for (int i = 0; i < SiteTimeSpanArr.size(); i++)
            {
                if (SiteTimeSpanArr[i] == t.ID)
                {
                    SiteTimeSpanArr.remove(i);
                    break;
                }
            }
            t.startHour = int.Parse(txtStartHour.Text);
            t.endHour = int.Parse(txtEndHour.Text);
            t.personID = int.Parse(txtPersonID.Text);
            t.siteID = int.Parse(txtSiteID.Text);
            t.isProtected = chkIsProtected.IsChecked == true;
            PersonTimeSpanArr = Global.storage.Persons[t.personID].timeSpanCollection;
            PersonTimeSpanArr.append(t.ID);
            SiteTimeSpanArr = Global.storage.Sites[t.siteID].timeSpanCollection;
            SiteTimeSpanArr.append(t.ID);
        }
        private void LoadTimeSpan(Strcture.TimeSpan t)
        {
            txtID.Text = t.ID.ToString();
            txtStartHour.Text = t.startHour.ToString();
            txtEndHour.Text = t.endHour.ToString();
            txtPersonID.Text = t.personID.ToString();
            txtSiteID.Text = t.siteID.ToString();
            chkIsProtected.IsChecked = t.isProtected;
        }
        private void RefreshList()
        {
            lstPeople.Items.Clear();
            Storage storage = Global.storage;
            Vector<Strcture.TimeSpan> v = storage.TimeSpans;
            for (int i = 0; i < v.size(); i++)
            {
                ListBoxItem lstItem = new ListBoxItem();
                lstItem.Content = String.Format("[{0},{1},{3}时到{4}时停留{2},{5}]", v[i].ID,
                    (v[i].personID < storage.Persons.size()) ? storage.Persons[v[i].personID].name : "未定义的人名",
                    (v[i].siteID < storage.Sites.size()) ? storage.Sites[v[i].siteID].name : "未定义的地点",
                    v[i].startHour, v[i].endHour,
                    v[i].isProtected ? "防护" : "未防护"
                    );
                lstItem.DataContext = v[i];
                lstPeople.Items.Add(lstItem);
            }
        }
        private void ClearTXT()
        {
            txtID.Clear();
            txtStartHour.Clear();
            txtEndHour.Clear();
            txtPersonID.Clear();
            txtSiteID.Clear();
            chkIsProtected.IsChecked = false;
        }
        private void lstPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                LoadTimeSpan((Strcture.TimeSpan)((ListBoxItem)e.AddedItems[0]).DataContext);
            }
        }
    }
}
