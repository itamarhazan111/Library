using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Data
    {
        public void MoveFromTable(int itemID, string from, string where)
        {

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"insert into {where} (ID, Name, Description, Publisher, PublishDate, Price, Discount, Genre, Author, SaleId ) " +
                   $"select ID, Name, Description, Publisher, PublishDate, Price, Discount, Genre, Author, SaleId from {from} where id={itemID}";
                cnn.Execute(txtSqlQuery);

            }
        }

        public void RentBook(int itemID,string userName)
        {
            MoveFromTable(itemID, "items", "rented");
            // adding the rent dates
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"Update RENTED set RentDate='{DateTime.Now.ToString("yyyy-MM-dd")}', ReturnDate='{DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd")}', Renter='{userName}' " +
                  $"where ID = {itemID}";

                cnn.Execute(txtSqlQuery);

            }
            RemoveItems(itemID, "Items");
        }

        public bool ReturnBook(int itemID, string userName)
        {
            
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string checkIsExist = $"select Renter FROM Rented WHERE ID = {itemID} and Renter='{userName}'";
                string renterName=Convert.ToString(cnn.ExecuteScalar(checkIsExist));
                if (renterName==userName)
                {
                    MoveFromTable(itemID, "rented", "items");
                    string txtSqlQuery = $"DELETE FROM Rented WHERE ID = {itemID} and Renter='{userName}'";
                    int rows = cnn.Execute(txtSqlQuery);
                    return rows > 0;
                }
                return false;
            }
        }

        public void Add(BaseItem item)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = "INSERT INTO Items ( Name, Description, Publisher, PublishDate, Price, Discount, Genre, Author) ";
                if (item is Book)
                {
                    Book book = (Book)item;
                    txtSqlQuery += $"VALUES ('{book.Name}','{book.Description}','{book.Publisher}','{book.PublishDate.Year}-{book.PublishDate.Month}-{book.PublishDate.Day}',{book.Price},{book.Discount},'{book.Genres}','{book.Author}');";

                }
                else
                {
                    txtSqlQuery += $"VALUES ('{item.Name}','{item.Description}','{item.Publisher}','{item.PublishDate.Year}-{item.PublishDate.Month}-{item.PublishDate.Day}',{item.Price},{item.Discount},'{item.Genres}',null);";
                }
                cnn.Execute(txtSqlQuery);
            }
        }
        public void AddSale(Sale sale)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"INSERT INTO Sales (Parameter, ParamValue, Discount) VALUES ('{sale.Parameter}', '{sale.ParamValue}', {sale.Discount})";
                cnn.Execute(txtSqlQuery);
                ApplySale(sale, "items");
                ApplySale(sale, "rented");
            }

        }

        public void RemoveSaleFromTable(int saleID,string table)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"UPDATE {table} SET Discount = 0, SaleID = null WHERE saleID = {saleID}";
                cnn.Execute(txtSqlQuery);
            }

        }

        public void DeleteSale(int saleID)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"DELETE FROM Sales WHERE ID = {saleID}";
                cnn.Execute(txtSqlQuery);
                RemoveSaleFromTable(saleID, "Items");
                RemoveSaleFromTable(saleID, "Rented");
            }

        }

        public void ApplySale(Sale sale, string table) // adding % to the start and the end of the parameter value if the parameter is genre
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"UPDATE {table} SET Discount = Sales.Discount, SaleID = Sales.ID FROM Sales WHERE {table}.{sale.Parameter} like '{sale.ParamValue}' AND {table}.Discount < Sales.Discount " +
                    $"AND Sales.ID = (SELECT ID From Sales order by ID desc LIMIT 1)";
                cnn.Execute(txtSqlQuery);
            }
        }
       

        public void UpdateItem(BaseItem item, int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = "Update items set ";
                if (item is Book)
                {
                    Book book = (Book)item;
                    txtSqlQuery += $"Name='{book.Name}', Description='{book.Description}', Publisher='{book.Publisher}', PublishDate='{book.PublishDate.Year}-{book.PublishDate.Month}-{book.PublishDate.Day}', Price={book.Price}, Discount={book.Discount}, Genre='{book.Genres}', Author='{book.Author}' ";

                }
                else
                {
                    txtSqlQuery += $"Name='{item.Name}', Description='{item.Description}', Publisher='{item.Publisher}', PublishDate='{item.PublishDate.Year}-{item.PublishDate.Month}-{item.PublishDate.Day}', Price={item.Price}, Discount={item.Discount},Genre='{item.Genres}','Author=null'";
                }
                txtSqlQuery += $"where ID = {id};";
                cnn.Execute(txtSqlQuery);
            }
        }
        public void RemoveItems(int ID, string table)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"DELETE FROM {table} WHERE ID = {ID}";
                cnn.Execute(txtSqlQuery);
            }

        }

        public DataTable ShowItem(string text)
        {
            DataTable dataTable = new DataTable();
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = text;
                dataTable.Load(cnn.ExecuteReader(txtSqlQuery));
            }
            return dataTable;
        }

        public DataTable ShowAllItems(string table)
        {
            return ShowItem($"select * from {table}");
        }
        public DataTable ShowAllBooks(string table)
        {
            return ShowItem($"select * from {table} where Author is not null");
        }
        public DataTable ShowAllJournals(string table)
        {
            return ShowItem($"select * from {table} where Author is null");
        }
        public DataTable ShowItemByParam(string name, string param, string table)
        {
            if (param == "Genre")
                return ShowItem($"Select * from {table} where Genre like '%{name}%'");
            return ShowItem($"Select * from {table} where {param} = '{name}'");
        }

        public int CountRows(string text)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                int RowCount = 0;
                RowCount = Convert.ToInt32(cnn.ExecuteScalar(text));
                return RowCount;
            }
        }

        public int CountRented()
        {
            return CountRows("select Count(*) from rented");
        }
        public int CountBooks()
        {
            return CountRows("select Count(*) from items where Author is not null");
        }
        public int CountJournals()
        {
            return CountRows("select Count(*) from items where Author is null");
        }
        public void UpdateTable(string text)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = text;
                cnn.Execute(txtSqlQuery);
            }
        }

        public void UpdateLates()
        {

                UpdateTable($"Update RENTED set lateinfo='The renter is late!' WHERE ReturnDate < '{DateTime.Now.ToString("yyyy-MM-dd")}'");
        }



        public void AddUser(User user)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"INSERT INTO Users (UserName, Password, IsAdmin) VALUES ('{user.UserName}', {user.GetHashCode()}, {Convert.ToInt32(user.IsAdmin)})";
                cnn.Execute(txtSqlQuery);
            }

        }
        public bool Login(string userName,SecureString password)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"Select password from Users where UserName= '{userName}'";
                int result= Convert.ToInt32(cnn.ExecuteScalar(txtSqlQuery));
                if (result == new NetworkCredential("", password).Password.GetHashCode())
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsAdmin(string userName, SecureString password)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string txtSqlQuery = $"Select IsAdmin from Users where UserName= '{userName}'";
                int result = Convert.ToInt32(cnn.ExecuteScalar(txtSqlQuery));
                if (result == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}



