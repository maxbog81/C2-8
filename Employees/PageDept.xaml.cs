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
    /// Логика взаимодействия для PageDept.xaml
    /// </summary>
    public partial class PageDept : Page
    {
        SqlConnection connection;
        SqlDataAdapter adapter;
        DataTable dt;
        public PageDept()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);

            adapter = new SqlDataAdapter();

            #region select

            SqlCommand command =new SqlCommand("SELECT dept FROM Department order by dept", connection);
            adapter.SelectCommand = command;

            #endregion

            #region insert

            command = new SqlCommand(@"INSERT INTO Department (dept) 
                          VALUES (@Dept);",
                          connection);

            command.Parameters.Add("@Dept", SqlDbType.NVarChar, -1, "dept");
            adapter.InsertCommand = command;

            #endregion

            #region delete


            command = new SqlCommand("DELETE FROM Department WHERE dept = @Dept", connection);
            SqlParameter param = command.Parameters.Add("@Dept", SqlDbType.NVarChar, 0, "dept");
            param.SourceVersion = DataRowVersion.Original;
            adapter.DeleteCommand = command;

            #endregion

            #region update

            command = new SqlCommand(@"UPDATE Department SET dept = @Dept WHERE dept = @OldDept", connection);

            command.Parameters.Add("@Dept", SqlDbType.NVarChar, -1, "dept");
            param = command.Parameters.Add("@OldDept", SqlDbType.NVarChar, 0, "dept");
            param.SourceVersion = DataRowVersion.Original;

            adapter.UpdateCommand = command;


            #endregion

            dt = new DataTable();

            adapter.Fill(dt);

            DeptDataGrid.DataContext = dt.DefaultView;
            DepartCombo.ItemsSource = dt.DefaultView;
        }

        private void DeptbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            DataRow newRow = dt.NewRow();
            newRow["dept"] = DepartCombo.Text;
            dt.Rows.Add(newRow);
            adapter.Update(dt);
        }

        private void DeptbtnChg_Click(object sender, RoutedEventArgs e)
        {
            string dept = DepartCombo.Text;
            DataRowView newRow = (DataRowView)DeptDataGrid.SelectedItem;
            newRow.BeginEdit();

            if (DeptDataGrid.SelectedItem != null && dept != null)
            {
                newRow["dept"] = DepartCombo.Text;
                newRow.EndEdit();
                adapter.Update(dt);
            }
            else
            {
                newRow.CancelEdit();
            }
        }

        private void DeptbtnDel_Click(object sender, RoutedEventArgs e)
        {
            DataRowView newRow = (DataRowView)DeptDataGrid.SelectedItem;
            if (DeptDataGrid.SelectedItem != null)
            {
                newRow.Row.Delete();
                adapter.Update(dt);
            }
        }
    }
}
