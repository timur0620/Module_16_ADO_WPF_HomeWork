using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using Module_16_ADO_WPF_HomeWork.View;

namespace Module_16_ADO_WPF_HomeWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connecrion;
        SqlDataAdapter adapterClient;
        SqlDataAdapter adapterProduct;
        DataTable dtClient;
        DataTable dtProduct;
        DataRowView row;
        public MainWindow()
        {
            InitializeComponent();
            Preparing();
        }
        private void Preparing()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = @"DataBaseSQL",
                IntegratedSecurity = true,
                Pooling = false,
                UserID = "Admin",
                Password = "Admin"
            };

            adapterClient = new SqlDataAdapter();
            adapterProduct = new SqlDataAdapter();

            connecrion = new SqlConnection(connectionStringBuilder.ConnectionString);
            dtClient = new DataTable();
            dtProduct = new DataTable();    

            var sqlClient = @"Select * from Client order by Client.Id";
            adapterClient.SelectCommand = new SqlCommand(sqlClient, connecrion);

            var sqlProduct = @"Select * from Product order by Product.Id";
            adapterProduct.SelectCommand = new SqlCommand(sqlProduct, connecrion);

            sqlClient = @"insert into Client (WorkerNam, IdBoss, IdDescriptions) 
                                 VALUES (@WorkerName, @IdBoss, @IdDescriptions);
                                 SET @Id = @@IDENTITY;";

            adapterClient.InsertCommand = new SqlCommand(sqlClient, connecrion);

            adapterClient.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id").Direction = ParameterDirection.Output;
            adapterClient.InsertCommand.Parameters.Add("@WorkerName", SqlDbType.Int, 20, "WorkerName");
            adapterClient.InsertCommand.Parameters.Add("@IdBoss", SqlDbType.Int, 4, "IdBoss");
            adapterClient.InsertCommand.Parameters.Add("@IdDescriptions", SqlDbType.Int, 4, "Id");

            adapterClient.Fill(dtClient);
            adapterProduct.Fill(dtProduct);
            gridViewClient.DataContext = dtClient.DefaultView;
            gridViewProduct.DataContext = dtProduct.DefaultView;
        }

        private void gridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (row == null) return;
            row.EndEdit();
            adapterClient.Update(dtClient);
        }

        private void gridView_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            row = (DataRowView)gridViewClient.SelectedItem;
            row.BeginEdit();
            adapterClient.Update(dtClient);
        }

        private void MenuItemAddClick(object sender, RoutedEventArgs e)
        {
            DataRow r = dtClient.NewRow();
            AddWindow add = new AddWindow(r);
            add.ShowDialog();

            if (add.DialogResult.Value)
            {
                dtClient.Rows.Add(r);
                adapterClient.Update(dtClient);
            }
        }
        private void MenuItemDeleteClick(object sender, RoutedEventArgs e)
        {
            row = (DataRowView)gridViewClient.SelectedItem;
            row.Row.Delete();
            adapterClient.Update(dtClient);
        }

    }
}
