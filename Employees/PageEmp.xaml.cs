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
    /// Логика взаимодействия для PageEmp.xaml
    /// </summary>
    public partial class PageEmp : Page
    {
        static HttpClient client = new HttpClient();
        public PageEmp()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("http://localhost:11263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //ObservableCollection<Employee> ListEmp = await GetProductsAsync(client.BaseAddress);
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Employee> ListEmp = await GetEmpsAsync(client.BaseAddress + "getlist");
            EmpDataGrid.ItemsSource = ListEmp;
        }

        static async Task<ObservableCollection<Employee>> GetEmpsAsync(string path)
        {
            ObservableCollection<Employee> ListEmp = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    ListEmp = await response.Content.ReadAsAsync<ObservableCollection<Employee>>();
                }
            }
            catch (Exception)
            {
            }
            return ListEmp;
        }


        private void DeptViewBut_Click(object sender, RoutedEventArgs e)
        {
            PageDept DepartmentPage = new PageDept();
            this.NavigationService.Navigate(DepartmentPage);
        }

        private void EmpbtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EmpbtnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EmpbtnChg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeptComboFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
