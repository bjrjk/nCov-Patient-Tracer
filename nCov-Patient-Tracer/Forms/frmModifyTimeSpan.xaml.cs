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
            if (lstPeople.SelectedItems.Count == 0)
            {
                Strcture.TimeSpan p = new Strcture.TimeSpan(storage.timespanIncCnt++,
                    int.Parse(txtStartHour.Text),int.Parse(txtEndHour.Text),
                    int.Parse(txtPersonID.Text),int.Parse(txtSiteID.Text), 
                    chkIsProtected.IsChecked==true);
                storage.TimeSpans.append(p);
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
            lstPeople.SelectedItems.Clear();
            ClearTXT();
            txtID.Text = (Global.storage.timespanIncCnt).ToString();
        }
        private void UpdateTimeSpan(Strcture.TimeSpan t)
        {
            t.startHour = int.Parse(txtStartHour.Text);
            t.endHour = int.Parse(txtEndHour.Text);
            t.personID = int.Parse(txtPersonID.Text);
            t.siteID = int.Parse(txtSiteID.Text);
            t.isProtected = chkIsProtected.IsChecked==true;
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
            for(int i = 0; i < v.size(); i++)
            {
                ListBoxItem lstItem = new ListBoxItem();
                lstItem.Content = String.Format("[{0},{1},{2},{3},{4}]", v[i].ID,
                    (v[i].personID < storage.Persons.size()) ? storage.Persons[v[i].personID].name : "Undefined",
                    v[i].startHour,v[i].endHour,
                    v[i].isProtected?"防护":"未防护"
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
