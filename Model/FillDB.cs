using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.OleDb;
using Bogus;
//dotnet add package System.Data.OleDb --version 8.0.0

namespace Module_16_ADO_WPF_HomeWork.Model
{
    class FillDB
    {
        public static void AddFakeData()
        {
            string str = @"Data Source=(localdb)\\MSSQLLocalDB;
                            Initial Catalog=DataBaseSQL;
                            Integrated Security=True;
                            Pooling=False;
                            Encrypt=True;
                            Trust Server 
                            Certificate=False";
            SqlConnectionStringBuilder strCon = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = @"DataBaseSQL",
                IntegratedSecurity = true,
                Pooling = true,
                UserID = "admin",
                Password = "admin"
            };
            SqlConnection sqlConnection = new SqlConnection() { ConnectionString = strCon.ConnectionString };
            try
            {
                sqlConnection.Open();

                for (int i = 0; i < 100; i++)
                {
                    Faker faker = new Faker();
                    Random random = new Random();

                    var sqlRecClient = $@"Insert into Client ( [LastName], [Name], [Surname], [NumberPhone], [Email] ) 
                                          VALUES ('{faker.Person.LastName}', '{faker.Person.FirstName}', '{faker.Person.UserName}', 
                                          '{faker.Person.Phone}', '{faker.Person.Email}')";

                    var sqlRecProduct = $@"insert into Product ([ProductCod], [ProductName], [Email])
                                           Values ('{random.Next(100, 1000)}', '{faker.Commerce.ProductName()}', '{faker.Person.Email}')";

                    SqlCommand RecordCommandClient = new SqlCommand(sqlRecClient, sqlConnection);
                    RecordCommandClient.ExecuteNonQuery();

                    SqlCommand RecordCommandProduct = new SqlCommand(sqlRecProduct, sqlConnection);
                    RecordCommandProduct.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }

}
