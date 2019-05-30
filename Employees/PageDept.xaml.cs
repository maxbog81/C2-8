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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.ObjectModel;

namespace Employees
{
    /// <summary>
    /// Логика взаимодействия для PageDept.xaml
    /// </summary>
    public partial class PageDept : Page
    {
        static HttpClient client = new HttpClient();
        string url = "http://localhost:11263/";
        public PageDept()
        {
            InitializeComponent();
            //client.BaseAddress = new Uri("http://localhost:11263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Department> ListDept = await GetDeptsAsync(url + "deptlist");
            DeptDataGrid.ItemsSource = ListDept;
        }

        static async Task<ObservableCollection<Department>> GetDeptsAsync(string path)
        {
            ObservableCollection<Department> ListDept = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    ListDept = await response.Content.ReadAsAsync<ObservableCollection<Department>>();
                }
            }
            catch (Exception)
            {
            }
            return ListDept;
        }

        private async void DeptViewbtn_Click(object sender, RoutedEventArgs e)
        {
            if (DeptTextBox.Text != String.Empty)
            {
                ObservableCollection<Department> ListDept = new ObservableCollection<Department>();
                ListDept.Add(await GetDeptAsync(url + $"deptlist/{DeptTextBox.Text}"));
                DeptDataGrid.ItemsSource = ListDept;
            }
            else
            {
                ObservableCollection<Department> ListDept = await GetDeptsAsync(url + "deptlist");
                DeptDataGrid.ItemsSource = ListDept;
            }
        }

        static async Task<Department> GetDeptAsync(string path)
        {
            Department Dept = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    Dept = await response.Content.ReadAsAsync<Department>();
                }
            }
            catch (Exception)
            {
            }
            return Dept;
        }

    }
}
