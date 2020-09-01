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
    public partial class frmModifyPerson : Window
    {
        public frmModifyPerson()
        {
            InitializeComponent();
            RefreshList();
            CreatePersonGUI();
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Storage storage = Global.storage;
            if (lstPeople.SelectedItems.Count == 0)
            {
                Person p = new Person(storage.personIncCnt++,txtName.Text,txtCompany.Text,
                    txtAddress.Text,txtTelephone.Text);
                p.timeSpanCollection = VectorHelper.Str2IntVector(txtTimeSpanID.Text);
                storage.Persons.append(p);
            }
            else
            {
                Person p = Global.storage.Persons[int.Parse(txtID.Text)];
                UpdatePerson(p);
            }
            RefreshList();
            CreatePersonGUI();
        }

        private void btnCreateNew_Click(object sender, RoutedEventArgs e)
        {
            CreatePersonGUI();
        }
        private void CreatePersonGUI()
        {
            lstPeople.SelectedItem = null;
            ClearTXT();
            txtID.Text = (Global.storage.personIncCnt).ToString();
        }
        private void UpdatePerson(Person p)
        {
            p.name = txtName.Text;
            p.company = txtCompany.Text;
            p.address = txtAddress.Text;
            p.telephone = txtTelephone.Text;
            p.timeSpanCollection = VectorHelper.Str2IntVector(txtTimeSpanID.Text);
        }
        private void LoadPerson(Person p)
        {
            txtID.Text = p.ID.ToString();
            txtName.Text = p.name;
            txtCompany.Text = p.company;
            txtAddress.Text = p.address;
            txtTelephone.Text = p.telephone;
            txtTimeSpanID.Text = VectorHelper.IntVector2Str(p.timeSpanCollection);
        }
        private void RefreshList()
        {
            lstPeople.Items.Clear();
            Storage storage = Global.storage;
            Vector<Person> v = storage.Persons;
            for(int i = 0; i < v.size(); i++)
            {
                ListBoxItem lstItem = new ListBoxItem();
                lstItem.Content = String.Format("[{0},{1},{2}]", v[i].ID, v[i].name,v[i].company);
                lstItem.DataContext = v[i];
                lstPeople.Items.Add(lstItem);
            }
        }
        private void ClearTXT()
        {
            txtID.Clear();
            txtName.Clear();
            txtCompany.Clear();
            txtAddress.Clear();
            txtTelephone.Clear();
            txtTimeSpanID.Clear();
        }
        
        private void lstPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                LoadPerson((Person)((ListBoxItem)e.AddedItems[0]).DataContext);
            }
        }

    }

    
}
