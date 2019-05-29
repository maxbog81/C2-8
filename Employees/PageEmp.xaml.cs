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

namespace Employees
{
    /// <summary>
    /// Логика взаимодействия для PageEmp.xaml
    /// </summary>
    public partial class PageEmp : Page
    {
        SqlConnection connection;
        SqlDataAdapter adapter;
        DataTable dt;
        SqlDataAdapter EmpAdapter;
        DataTable Empdt;
        SqlDataAdapter DeptAdapter;
        DataTable Deptdt;

        public PageEmp()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            adapter = new SqlDataAdapter();
            EmpAdapter = new SqlDataAdapter();
            DeptAdapter = new SqlDataAdapter();

            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);

            #region select

            SqlCommand command = new SqlCommand("SELECT id, name, dept FROM Employee", connection);
            adapter.SelectCommand = command;

            SqlCommand Empcommand = new SqlCommand("SELECT name FROM Employee order by name", connection);
            EmpAdapter.SelectCommand = Empcommand;

            SqlCommand Deptcommand = new SqlCommand("SELECT dept FROM Department order by dept", connection);
            DeptAdapter.SelectCommand = Deptcommand;

            #endregion

            #region insert

            command = new SqlCommand(@"INSERT INTO Employee (name,dept) 
                          VALUES (@Name, @Dept); SET @ID = @@IDENTITY;",
                          connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "name");
            command.Parameters.Add("@Dept", SqlDbType.NVarChar, -1, "dept");

            SqlParameter param = command.Parameters.Add("@ID", SqlDbType.Int, 0, "id");

            param.Direction = ParameterDirection.Output;

            adapter.InsertCommand = command;

            #endregion

            #region delete


            command = new SqlCommand("DELETE FROM Employee WHERE ID = @ID", connection);
            param = command.Parameters.Add("@ID", SqlDbType.Int, 0, "ID");
            param.SourceVersion = DataRowVersion.Original;
            adapter.DeleteCommand = command;

            #endregion

            #region update

            command = new SqlCommand(@"UPDATE Employee SET name = @Name,dept = @Dept WHERE ID = @ID", connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "name");
            command.Parameters.Add("@Dept", SqlDbType.NVarChar, -1, "dept");
            param = command.Parameters.Add("@ID", SqlDbType.Int, 0, "id");

            param.SourceVersion = DataRowVersion.Original;

            adapter.UpdateCommand = command;


            #endregion

            dt = new DataTable();
            Empdt = new DataTable();
            Deptdt = new DataTable();

            adapter.Fill(dt);
            EmpAdapter.Fill(Empdt);
            DeptAdapter.Fill(Deptdt);

            EmpDataGrid.DataContext = dt.DefaultView;

            EmpCombo.ItemsSource = Empdt.DefaultView;
            DeptCombo.ItemsSource = Deptdt.DefaultView;
            DeptComboFiltr.ItemsSource = Deptdt.DefaultView;

        }

        private void DeptViewBut_Click(object sender, RoutedEventArgs e)
        {
            PageDept DepartmentPage = new PageDept();
            this.NavigationService.Navigate(DepartmentPage);
        }

        private void EmpbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // добавим новую строку
            DataRow newRow = dt.NewRow();
            newRow["name"] = EmpCombo.Text;
            newRow["dept"] = DeptCombo.Text;
            dt.Rows.Add(newRow);
            adapter.Update(dt);
        }

        private void EmpbtnDel_Click(object sender, RoutedEventArgs e)
        {
            DataRowView newRow = (DataRowView)EmpDataGrid.SelectedItem;
            if (EmpDataGrid.SelectedItem != null)
            {
                newRow.Row.Delete();
                adapter.Update(dt);
            }

        }

        private void EmpbtnChg_Click(object sender, RoutedEventArgs e)
        {
            string emp = EmpCombo.Text;
            string dept = DeptCombo.Text;
            DataRowView newRow = (DataRowView)EmpDataGrid.SelectedItem;
            newRow.BeginEdit();

            if (EmpDataGrid.SelectedItem != null && (emp != null || dept != null))
            {
                newRow["name"] = EmpCombo.Text;
                newRow["dept"] = DeptCombo.Text;
                newRow.EndEdit();
                adapter.Update(dt);
            }
            else
            {
                newRow.CancelEdit();
            }

        }

        private void DeptComboFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            SqlParameter param = new SqlParameter();

            if (DeptComboFiltr.SelectedIndex > -1)
            {

                command = new SqlCommand("SELECT id, name, dept FROM Employee where dept =@Dept", connection);
                param = new SqlParameter("@Dept", SqlDbType.NVarChar, -1);
                param.Value = ((DataRowView)DeptComboFiltr.SelectedItem).Row["dept"].ToString();
                command.Parameters.Add(param);
            }
            else
            {
                command = new SqlCommand("SELECT id, name, dept FROM Employee", connection);
            }

            adapter = new SqlDataAdapter();
            dt = new DataTable();
            adapter.SelectCommand = command;

            #region insert

            command = new SqlCommand(@"INSERT INTO Employee (name,dept) 
                          VALUES (@Name, @Dept); SET @ID = @@IDENTITY;",
                          connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "name");
            command.Parameters.Add("@Dept", SqlDbType.NVarChar, -1, "dept");

            param = command.Parameters.Add("@ID", SqlDbType.Int, 0, "id");

            param.Direction = ParameterDirection.Output;

            adapter.InsertCommand = command;

            #endregion

            #region delete


            command = new SqlCommand("DELETE FROM Employee WHERE ID = @ID", connection);
            param = command.Parameters.Add("@ID", SqlDbType.Int, 0, "ID");
            param.SourceVersion = DataRowVersion.Original;
            adapter.DeleteCommand = command;

            #endregion

            #region update

            command = new SqlCommand(@"UPDATE Employee SET name = @Name,dept = @Dept WHERE ID = @ID", connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "name");
            command.Parameters.Add("@Dept", SqlDbType.NVarChar, -1, "dept");
            param = command.Parameters.Add("@ID", SqlDbType.Int, 0, "id");

            param.SourceVersion = DataRowVersion.Original;

            adapter.UpdateCommand = command;


            #endregion

            adapter.Fill(dt);
            EmpDataGrid.DataContext = dt.DefaultView;
        }
    }
}
