using Microsoft.Win32;
using nCov_Patient_Tracer.DSA;
using nCov_Patient_Tracer.Forms;
using nCov_Patient_Tracer.Strcture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace nCov_Patient_Tracer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        frmModifySite ModifySiteFrm;
        frmModifyPerson ModifyPersonFrm;
        frmModifyTimeSpan ModifyTimeSpanFrm;
        public MainWindow()
        {
            InitializeComponent();
            web.Address = Global.WebURL;
        }
        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            web.Address = Global.WebURL+"about.html";
        }
        private void frmMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            web.Height = grdContainer.ActualHeight - 25;
        }
        private void btnEditSite_Click(object sender, RoutedEventArgs e)
        {
            ModifySiteFrm = new frmModifySite();
            ModifySiteFrm.Show();
        }

        private void btnNewConfig_Click(object sender, RoutedEventArgs e)
        {
            Global.storage = new Storage();
            Global.configPath = null;
        }
        private void btnLoadConfig_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "nCov-Patient-Tracer配置文件(*.ncov.bin)|*.ncov.bin|所有文件(*.*)|*.*";
            if (dialog.ShowDialog()==false)
                return;
            Global.configPath = dialog.FileName;
            Global.storage = Storage.read(
                new System.IO.BinaryReader(
                    new System.IO.FileStream(Global.configPath,System.IO.FileMode.Open)
                    )
                );
            MessageBox.Show("加载成功！", "提示信息");
        }
        private void btnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            if(Global.configPath == null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "nCov-Patient-Tracer配置文件(*.ncov.bin)|*.ncov.bin|所有文件(*.*)|*.*";
                if (dialog.ShowDialog() == false)
                    return;
                Global.configPath = dialog.FileName;
            }
            Global.storage.write(
                new System.IO.BinaryWriter(
                    new System.IO.FileStream(Global.configPath, System.IO.FileMode.Create)
                    )
                );
            MessageBox.Show("保存成功！", "提示信息");
        }

        private void frmMain_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnEditPerson_Click(object sender, RoutedEventArgs e)
        {
            ModifyPersonFrm = new frmModifyPerson();
            ModifyPersonFrm.Show();
        }

        private void btnEditTimeSpan_Click(object sender, RoutedEventArgs e)
        {
            ModifyTimeSpanFrm = new frmModifyTimeSpan();
            ModifyTimeSpanFrm.Show();
        }

        private void btnGenData_Click(object sender, RoutedEventArgs e)
        {
            const int PeopleNumber = 50, SiteNumber = 20;
            Global.configPath = null;
            Global.storage = new Storage();
            Storage storage = Global.storage;
            storage.personIncCnt = PeopleNumber;
            storage.siteIncCnt = SiteNumber;
            storage.timespanIncCnt = PeopleNumber * SiteNumber;
            for (int i=0;i< PeopleNumber; i++)
            {
                Person p = new Person(i, "测试人员" + i.ToString(),
                    "", "", "");
                storage.Persons.append(p);
                for(int j = 0; j < SiteNumber; j++)
                {
                    p.timeSpanCollection.append(i * SiteNumber + j);
                }
            }
            Random ra = new Random();
            for (int i = 0; i < SiteNumber; i++)
            {
                Site s = new Site(i, new Coordinate(
                    116.4+ra.NextDouble()-0.5,39.9+ra.NextDouble()-0.5
                    ),
                    "测试地点" + i.ToString()
                    );
                storage.Sites.append(s);
                for (int j = 0; j < PeopleNumber; j++)
                {
                    s.timeSpanCollection.append(j * SiteNumber + i);
                }
            }
            for(int i=0;i< PeopleNumber* SiteNumber; i++)
            {
                int t1=ra.Next(0,100), t2 = ra.Next(0, 100);
                double t3=ra.NextDouble();
                Strcture.TimeSpan t = new Strcture.TimeSpan(i,
                    Math.Min(t1, t2), Math.Max(t1, t2),i/ SiteNumber, t3 > 0.5);
                storage.TimeSpans.append(t);
            }
            MessageBox.Show("随机生成成功！", "提示信息");
        }

        private void btnVerifyData_Click(object sender, RoutedEventArgs e)
        {
            Storage storage = Global.storage;
            for(int i = 0; i < storage.Persons.size(); i++)
            {
                for(int j = 0; j < storage.Persons[i].timeSpanCollection.size(); j++)
                {
                    if (storage.Persons[i].timeSpanCollection[j] > storage.TimeSpans.size())
                        MessageBox.Show(
                            String.Format("验证Persons[{0}]的timeSpanCollection[{1}]->{2}时TimeSpans.size()不通过！",
                                i,j, storage.Persons[i].timeSpanCollection[j]
                            ));
                    if (storage.TimeSpans[storage.Persons[i].timeSpanCollection[j]].personID != i)
                        MessageBox.Show(
                                String.Format("验证Persons[{0}]的timeSpanCollection[{1}].personID->{2}时不通过！",
                                    i, j, storage.TimeSpans[storage.Persons[i].timeSpanCollection[j]].personID
                                ));
                }
            }
            for (int i = 0; i < storage.Sites.size(); i++)
            {
                for (int j = 0; j < storage.Sites[i].timeSpanCollection.size(); j++)
                {
                    if (storage.Sites[i].timeSpanCollection[j] > storage.TimeSpans.size())
                        MessageBox.Show(
                            String.Format("验证Sites[{0}].timeSpanCollection[{1}]->{2}时TimeSpans.size()不通过！",
                                i, j, storage.Sites[i].timeSpanCollection[j]
                            ));
                }
            }
            for(int i = 0; i < storage.TimeSpans.size(); i++)
            {
                if (storage.TimeSpans[i].personID > storage.Persons.size())
                    MessageBox.Show(
                            String.Format("验证TimeSpans[{0}].personID->{1}时Persons.size()->{2}不通过！",
                                i, storage.TimeSpans[i].personID, storage.Persons.size()
                            ));
            }
        }
    }
}
