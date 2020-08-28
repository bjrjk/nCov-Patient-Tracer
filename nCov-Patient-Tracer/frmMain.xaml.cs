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
        public MainWindow()
        {
            InitializeComponent();
            ModifySiteFrm = new frmModifySite();
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
            ModifySiteFrm.Close();
        }
    }
}
