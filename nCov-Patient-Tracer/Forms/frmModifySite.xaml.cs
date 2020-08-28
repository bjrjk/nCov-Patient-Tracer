using CefSharp;
using CefSharp.Wpf;
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
    public partial class frmModifySite : Window
    {
        private class WebCallBack
        {
            public frmModifySite frm;
            public WebCallBack(frmModifySite frm)
            {
                this.frm = frm;
            }
            public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
            {
                if (e.Frame.IsMain)
                {
                }
            }

            public void transmit(string str)
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
        public frmModifySite()
        {
            InitializeComponent();
            web.Address = Global.WebURL + "coordinate.php";
            var boundObj = new WebCallBack(this);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            web.JavascriptObjectRepository.Register("bound", boundObj, isAsync: false);
            web.FrameLoadEnd += boundObj.OnFrameLoadEnd;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }

    
}
