using CefSharp;
using Microsoft.Win32;
using nCov_Patient_Tracer.DSA;
using nCov_Patient_Tracer.Forms;
using nCov_Patient_Tracer.Strcture;
using nCov_Patient_Tracer.Tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.VisualBasic;
using TimeSpan = nCov_Patient_Tracer.Strcture.TimeSpan;

namespace nCov_Patient_Tracer
{
    public partial class MainWindow : Window
    {
        //对四个子窗体的引用
        frmModifySite ModifySiteFrm;
        frmModifyPerson ModifyPersonFrm;
        frmModifyTimeSpan ModifyTimeSpanFrm;
        frmDisplayResult DisplayResultFrm;
        public MainWindow()
        {
            InitializeComponent(); //构造函数
            Test.testMain();
            web.Address = Global.WebURL;
        }
        private void btnAbout_Click(object sender, RoutedEventArgs e) //btnAbout的Click事件，关于按钮
        {
            web.Address = Global.WebURL + "about.html";
        }
        private void frmMain_SizeChanged(object sender, SizeChangedEventArgs e) //frmMain的SizeChanged事件
        {
            web.Height = grdContainer.ActualHeight - 25;
        }
        private void btnEditSite_Click(object sender, RoutedEventArgs e) //btnEditSite的Click事件，添加、更改地点按钮
        {
            ModifySiteFrm = new frmModifySite();
            ModifySiteFrm.Show();
        }

        private void btnNewConfig_Click(object sender, RoutedEventArgs e) //btnNewConfig的Click事件，新建配置
        {
            Global.storage = new Storage();
            Global.configPath = null;
            Global.processedStorage = null;
            progress.Value = 0;
            lstPeople.Items.Clear();
            btnQuery.IsEnabled = false;
        }
        private void btnLoadConfig_Click(object sender, RoutedEventArgs e) //btnLoadConfig的Click事件，加载配置
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "nCov-Patient-Tracer配置文件(*.ncov.bin)|*.ncov.bin|所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == false)
                return;
            Global.configPath = dialog.FileName;
            Global.storage = Storage.read(
                new System.IO.BinaryReader(
                    new System.IO.FileStream(Global.configPath, System.IO.FileMode.Open)
                    )
                );
            MessageBox.Show("加载成功！", "提示信息");
        }
        private void btnSaveConfig_Click(object sender, RoutedEventArgs e) //btnSaveConfig的Click事件，保存配置
        {
            if (Global.configPath == null)
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

        private void frmMain_Closed(object sender, EventArgs e) //frmMain的Closed事件
        {
            Environment.Exit(0);
        }

        private void btnEditPerson_Click(object sender, RoutedEventArgs e) //btnEditPerson的Click事件，展示添加、变更人员窗体
        {
            ModifyPersonFrm = new frmModifyPerson();
            ModifyPersonFrm.Show();
        }

        private void btnEditTimeSpan_Click(object sender, RoutedEventArgs e) //btnEditTimeSpan的Click事件，展示添加、变更时间段窗体
        {
            ModifyTimeSpanFrm = new frmModifyTimeSpan();
            ModifyTimeSpanFrm.Show();
        }

        private void btnGenData_Click(object sender, RoutedEventArgs e) //btnGenData的Click事件，数据生成器按钮
        {
            int PeopleNumber = int.Parse(Interaction.InputBox("请输入随机生成的人员数目：", "数据生成器", "50"));
            int SiteNumber = int.Parse(Interaction.InputBox("请输入随机生成的地点数目：", "数据生成器", "20"));
            double protectPobability = double.Parse(Interaction.InputBox("请输入防护概率（0-1之间的小数，0为全都不防护，1为全都防护）：", "数据生成器", "0.5"));
            Global.configPath = null;
            Global.storage = new Storage();
            Storage storage = Global.storage;
            storage.personIncCnt = PeopleNumber;
            storage.siteIncCnt = SiteNumber;
            storage.timespanIncCnt = PeopleNumber * SiteNumber;
            Random ra = new Random();
            for (int i = 0; i < PeopleNumber; i++)
            {
                Person p = new Person(i, "测试人员" + i.ToString(),
                    "测试公司" + ra.Next(0, 100).ToString(), "测试地址" + ra.Next(0, 100).ToString(), ra.Next(10000000, 1999999999).ToString());
                storage.Persons.append(p);
                for (int j = 0; j < SiteNumber; j++)
                {
                    p.timeSpanCollection.append(i * SiteNumber + j);
                }
            }

            for (int i = 0; i < SiteNumber; i++)
            {
                Site s = new Site(i, new Coordinate(
                    116.4 + ra.NextDouble() - 0.5, 39.9 + ra.NextDouble() - 0.5
                    ),
                    "测试地点" + i.ToString()
                    );
                storage.Sites.append(s);
                for (int j = 0; j < PeopleNumber; j++)
                {
                    s.timeSpanCollection.append(j * SiteNumber + i);
                }
            }
            for (int i = 0; i < PeopleNumber * SiteNumber; i++)
            {
                int t1 = ra.Next(0, 100), t2 = ra.Next(0, 100);
                double t3 = ra.NextDouble();
                Strcture.TimeSpan t = new Strcture.TimeSpan(i,
                    Math.Min(t1, t2), Math.Max(t1, t2), i / SiteNumber, i % SiteNumber, t3 < protectPobability);
                storage.TimeSpans.append(t);
            }
            MessageBox.Show("随机生成成功！", "提示信息");
        }

        private void btnVerifyData_Click(object sender, RoutedEventArgs e) //btnVerifyData的Click事件，数据验证器按钮
        {
            Storage storage = Global.storage;
            for (int i = 0; i < storage.Persons.size(); i++)
            {
                for (int j = 0; j < storage.Persons[i].timeSpanCollection.size(); j++)
                {
                    if (storage.Persons[i].timeSpanCollection[j] > storage.TimeSpans.size())
                        MessageBox.Show(
                            String.Format("验证Persons[{0}].timeSpanCollection[{1}]->{2}时TimeSpans.size()不通过！",
                                i, j, storage.Persons[i].timeSpanCollection[j]
                            ));
                    if (storage.TimeSpans[storage.Persons[i].timeSpanCollection[j]].personID != i)
                        MessageBox.Show(
                                String.Format("验证Persons[{0}].timeSpanCollection[{1}].personID->{2}时不通过！",
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
                    if (storage.TimeSpans[storage.Sites[i].timeSpanCollection[j]].siteID != i)
                        MessageBox.Show(
                                String.Format("验证Sites[{0}].timeSpanCollection[{1}].siteID->{2}时不通过！",
                                    i, j, storage.TimeSpans[storage.Persons[i].timeSpanCollection[j]].siteID
                                ));
                }
            }
            for (int i = 0; i < storage.TimeSpans.size(); i++)
            {
                if (storage.TimeSpans[i].personID > storage.Persons.size())
                    MessageBox.Show(
                            String.Format("验证TimeSpans[{0}].personID->{1}时Persons.size()->{2}不通过！",
                                i, storage.TimeSpans[i].personID, storage.Persons.size()
                            ));
                if (storage.TimeSpans[i].siteID > storage.Sites.size())
                    MessageBox.Show(
                            String.Format("验证TimeSpans[{0}].siteID->{1}时Sites.size()->{2}不通过！",
                                i, storage.TimeSpans[i].siteID, storage.Sites.size()
                            ));
            }
        }

        private void btnViewResult_Click(object sender, RoutedEventArgs e) //btnViewResult的Click事件，展示查看结果窗体
        {
            DisplayResultFrm = new frmDisplayResult();
            DisplayResultFrm.Show();
        }

        private void btnPrepare_Click(object sender, RoutedEventArgs e) //btnPrepare的Click事件，处理数据按钮
        {
            progress.Value = 0;
            Global.processedStorage = new ProcessedStorage(Global.storage);
            progress.Value = 80;
            lstPeople.Items.Clear();
            Vector<Person> v = Global.storage.Persons;
            for (int i = 0; i < v.size(); i++)
            {
                ListBoxItem lstItem = new ListBoxItem();
                lstItem.Content = String.Format("[{0},{1},{2}]", v[i].ID, v[i].name, v[i].company);
                lstItem.DataContext = v[i];
                lstPeople.Items.Add(lstItem);
            }
            lstPeople.IsEnabled = true;
            btnQuery.IsEnabled = true;
            btnSave.IsEnabled = true;
            progress.Value = 100;
            web.Address = Global.WebURL + "displayinfo.html";
        }
        private void runJavaScript(string command) //在浏览器中执行Javascript
        {
            if (web.CanExecuteJavascriptInMainFrame) web.ExecuteScriptAsync(command);
        }
        private void showDebugInformationInWeb(string content) //在浏览器中显示内容
        {
            runJavaScript(String.Format(
                "document.body.innerHTML='{0}';", content
                ));
        }
        private void btnQuery_Click(object sender, RoutedEventArgs e) //btnQuery的Click事件，查询结果按钮
        {
            if (lstPeople.SelectedItems.Count == 0)
            {
                MessageBox.Show("您未选中任何一人进行追踪！", "提示信息");
                return;
            }
            string s = "<p>风险等级标准：低风险地区（绿色），密切接触者小于等于2人；中风险地区（黄色），密切接触者在3人到10人之间；高风险地区（红色），密切接触者大于10人。</p>";
            Vector<Person> personArr = new Vector<Person>();
            Global.personArr = personArr;
            Vector<Vector<Vector<TimeSpan>>> timeSpanArr = new Vector<Vector<Vector<TimeSpan>>>();
            Global.timeSpanArr = timeSpanArr;
            for (int k = 0; k < lstPeople.SelectedItems.Count; k++)
            {
                Person p = (Person)(((ListBoxItem)lstPeople.SelectedItems[k]).DataContext);
                personArr.append(p);
                Vector<Vector<Strcture.TimeSpan>> arr = Global.processedStorage.query(p);
                /*DEBUG BEGIN
                Vector<Vector<Strcture.TimeSpan>> arrBF = Global.processedStorage.queryBruteForce(p);
                Debug.Assert(arr.size() == arrBF.size());
                for(int i = 0; i < arr.size(); i++)
                {
                    Algorithm.quickSort(arr[i], new TimeSpanComparerByContent());
                    Algorithm.quickSort(arrBF[i], new TimeSpanComparerByContent());
                    for(int j = 0; j < arr[i].size(); j++)
                    {
                        if (!object.ReferenceEquals(arr[i][j], arrBF[i][j]))
                            Debug.Assert(false);
                    }
                }
                DEBUG END*/
                timeSpanArr.append(arr);

                s += "<h1>查询“" + p.name + "”的密切接触者信息</h1><br>";
                int totalNumber = 0;
                for (int i = 0; i < arr.size(); i++)
                {

                    if (arr[i].size() == 0) continue;
                    s += "在地点“<strong style=\"color: " + Site.getRiskLevel(arr[i].size()) + "\">" + Global.storage.Sites[arr[i][0].siteID].name + "</strong>”：<br>";
                    s += "本人停留时间：" + Global.storage.TimeSpans[p.timeSpanCollection[i]].startHour.ToString() +
                        "小时至" + Global.storage.TimeSpans[p.timeSpanCollection[i]].endHour.ToString() + "小时<br>";
                    s += "密切接触者信息：<br>";
                    s += "<table class=\"table\"><thead><tr><th>姓名</th><th>停留时间</th><th>公司</th><th>地址</th><th>手机号</th></tr></thead><tbody>";
                    for (int j = 0; j < arr[i].size(); j++)
                    {
                        totalNumber++;
                        s += "<tr>";
                        s += "<th>" + Global.storage.Persons[arr[i][j].personID].name + "</th>";
                        s += "<th>" + arr[i][j].startHour.ToString() + "小时至" + arr[i][j].endHour.ToString() + "小时</th>";
                        s += "<th>" + Global.storage.Persons[arr[i][j].personID].company + "</th>";
                        s += "<th>" + Global.storage.Persons[arr[i][j].personID].address + "</th>";
                        s += "<th>" + Global.storage.Persons[arr[i][j].personID].telephone + "</th>";
                        s += "</tr>";
                    }
                    s += "</tbody></table>";
                }
                if (totalNumber == 0)
                    s += "<strong>没有任何密切接触者！</strong>";

            }
            showDebugInformationInWeb(s);
            DisplayResultFrm = new frmDisplayResult();
            DisplayResultFrm.Show();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e) //btnSave的Click事件，保存结果按钮
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "HTML网页(*.html)|*.html|所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == false)
                return;
            var task = web.GetSourceAsync();
            task.ContinueWith(t =>
            {
                using (StreamWriter sw = new StreamWriter(dialog.FileName))
                {
                    sw.Write(t.Result);
                }
                MessageBox.Show("保存成功！", "提示信息");
            });
        }

        private void btnGenData2_Click(object sender, RoutedEventArgs e) //btnGenData2的Click事件，数据生成器2按钮
        {
            int PeopleNumber = int.Parse(Interaction.InputBox("请输入随机生成的人员数目：", "数据生成器", "10"));
            int SiteNumber = int.Parse(Interaction.InputBox("请输入随机生成的地点数目：", "数据生成器", "8"));
            double protectPobability = double.Parse(Interaction.InputBox("请输入防护概率（0-1之间的小数，0为全都不防护，1为全都防护）：", "数据生成器", "0.5"));
            Global.configPath = null;
            Global.storage = new Storage();
            Storage storage = Global.storage;
            storage.personIncCnt = PeopleNumber;
            storage.siteIncCnt = SiteNumber;
            storage.timespanIncCnt = PeopleNumber * SiteNumber;
            Random ra = new Random();
            for (int i = 0; i < PeopleNumber; i++)
            {
                Person p = new Person(i, "测试人员" + i.ToString(),
                    "测试公司" + ra.Next(0, 100).ToString(), "测试地址" + ra.Next(0, 100).ToString(), ra.Next(10000000, 1999999999).ToString());
                storage.Persons.append(p);
                for (int j = 0; j < SiteNumber; j++)
                {
                    p.timeSpanCollection.append(j * PeopleNumber + i);
                }
            }

            for (int i = 0; i < SiteNumber; i++)
            {
                Site s = new Site(i, new Coordinate(
                    116.4 + ra.NextDouble() - 0.5, 39.9 + ra.NextDouble() - 0.5
                    ),
                    "测试地点" + i.ToString()
                    );
                storage.Sites.append(s);
                for (int j = 0; j < PeopleNumber; j++)
                {
                    s.timeSpanCollection.append(i * PeopleNumber + j);
                }
            }
            Vector<int> curPersonHour = new Vector<int>();
            curPersonHour.reserve(PeopleNumber);
            for (int i = 0; i < curPersonHour.size(); i++) curPersonHour[i] = 0;
            for (int i = 0; i < PeopleNumber * SiteNumber; i++)
            {
                int t1 = ra.Next(1, 4);

                double t3 = ra.NextDouble();
                Strcture.TimeSpan t = new Strcture.TimeSpan(i,
                    curPersonHour[i % PeopleNumber], curPersonHour[i % PeopleNumber] + t1,
                    i % PeopleNumber, i / PeopleNumber, t3 < protectPobability);
                storage.TimeSpans.append(t);
                curPersonHour[i % PeopleNumber] += t1 + 1;
            }
            MessageBox.Show("随机生成成功！", "提示信息");
        }
    }
}
