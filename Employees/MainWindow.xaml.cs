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

//Богатов Максим

//1.	Создать таблицы Employee и Department в БД MSSQL Server и заполнить списки сущностей начальными данными.
//2.	Для списка сотрудников и списка департаментов предусмотреть визуализацию(отображение). Это можно сделать, например, с использованием ComboBox или ListView.
//3.	Предусмотреть редактирование сотрудников и департаментов.Должна быть возможность изменить департамент у сотрудника.Список департаментов для выбора можно выводить в ComboBox, и все это можно выводить на дополнительной форме.
//4.	Предусмотреть возможность создания новых сотрудников и департаментов.Реализовать данную возможность либо на форме редактирования, либо сделать новую форму.


namespace Employees
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var connectionString =ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string createExpression = @"IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[Employee]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)                                        
                                        CREATE TABLE[dbo].[Employee] (
                                        [id] INT IDENTITY(1, 1) NOT NULL,
                                        [name] NVARCHAR(MAX) COLLATE Cyrillic_General_CI_AS NOT NULL,
                                        [dept] NVARCHAR(MAX) COLLATE Cyrillic_General_CI_AS NOT NULL,
                                        CONSTRAINT[PK_dbo.Employee] PRIMARY KEY CLUSTERED([Id] ASC))                                   
                                        
                                        IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[Department]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)                                        
                                        CREATE TABLE[dbo].[Department] (
                                        [dept] NVARCHAR(4000) COLLATE Cyrillic_General_CI_AS NOT NULL,
                                        CONSTRAINT[PK_dbo.Dept] PRIMARY KEY CLUSTERED([dept] ASC))
                                        ;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(createExpression, connection);
                    int number = command.ExecuteNonQuery();

                }
                                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    var random = new Random();
                    string sqlExpression = "SELECT COUNT(*) FROM Employee";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int cnt = Convert.ToInt32(command.ExecuteScalar());
                    if (cnt == 0)
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            var emp = new Employee
                            {
                                EmpName = $"Имя_{i + 1}",
                                Dept= $"Подразделение_{random.Next(1, 5)}"
                            };
                            var sql = String.Format("INSERT INTO Employee (name, dept) " +
                                                    "VALUES (N'{0}', N'{1}')",
                                                    emp.EmpName,
                                                    emp.Dept);

                            command = new SqlCommand(sql, connection);
                            command.ExecuteNonQuery();

                        }
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlExpression = "SELECT COUNT(*) FROM Department";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int cnt = Convert.ToInt32(command.ExecuteScalar());
                    if (cnt == 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var dept = new Department
                            {
                                Dept = $"Подразделение_{i + 1}"
                            };
                            var sql = String.Format("INSERT INTO Department (dept) " +
                                                    "VALUES (N'{0}')",
                                                    dept.Dept);

                            command = new SqlCommand(sql, connection);
                            command.ExecuteNonQuery();

                        }
                    }
                }

            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message);
            }

        }
    }
}
