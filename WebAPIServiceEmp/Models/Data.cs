using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//Создать базу Employees Создание таблиц и их наполнение происходит при запуске клиентской части
namespace WebAPIServiceEmp.Models
{
    
    public class Data
    {
        private SqlConnection sqlConnection;
        public Data()
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                                        Initial Catalog=Employees;
                                        Integrated Security=True;
                                        ";

            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        public List<Employee> GetList()
        {
            List<Employee> list = new List<Employee>();

            string sql = @"SELECT * FROM Employee";

            using (SqlCommand com = new SqlCommand(sql, sqlConnection))
            {
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(
                            new Employee()
                            {
                                Id = (int)reader["id"],
                                EmpName = reader["name"].ToString(),
                                Dept = reader["dept"].ToString()
                            });
                    }
                }

            }

            return list;
        }

        public Employee GetEmpById(int Id)
        {
            string sql = $@"SELECT * FROM Employee WHERE Id={Id}";
            Employee temp = new Employee();
            using (SqlCommand com = new SqlCommand(sql, sqlConnection))
            {
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        temp = new Employee()
                        {
                            Id = (int)reader["id"],
                            EmpName = reader["name"].ToString(),
                            Dept = reader["dept"].ToString()
                        };
                    }
                }

            }
            return temp;
        }

        public bool AddEmp(Employee Worker)
        {
            try
            {
                string sqlAdd = $@" INSERT INTO Employee(name,dept)
                               VALUES(N'{Worker.EmpName}',
                                      N'{Worker.Dept}') ";

                //Console.WriteLine(sqlAdd);

                using (var com = new SqlCommand(sqlAdd, sqlConnection))
                {
                    com.ExecuteNonQuery();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}