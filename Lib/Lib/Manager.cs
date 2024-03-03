using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Manager
    {
        private Dictionary<string, Exception> exceptions;
        private Data data;
        private User _CurrentUser;
        

        public Manager()
        {
            
            data = new Data();
            exceptions = new Dictionary<string, Exception>();
            exceptions.Add("name", new Exception("the name is not legal"));
            exceptions.Add("description", new Exception("the description is not legal"));
            exceptions.Add("publisher", new Exception("the publisher is not legal"));
            exceptions.Add("publish_date", new Exception("the publish_date is not legal"));
            exceptions.Add("price", new Exception("the price is not legal"));
            exceptions.Add("discout", new Exception("the discout is not legal"));
            exceptions.Add("genres", new Exception("the genres is not legal"));
            exceptions.Add("id", new Exception("the id is not legal"));
            exceptions.Add("parameter", new Exception("the parameter is not legal"));
            exceptions.Add("paramVal", new Exception("the parameter value is not legal"));
            exceptions.Add("username", new Exception("the username is not legal"));
            exceptions.Add("password", new Exception("password must contains at least 3 digits!"));
            exceptions.Add("returnItem", new Exception("only the renter can return the item"));
        }
        public void AddItem(string name, string desc, string publisher, DateTime publish_date, string price, string discout, string genres, string author)
        {
            BaseItem item = MakeNewItem(name, desc, publisher, publish_date, price, discout, genres, author);
            data.Add(item);
        }
        public void UpdateItem(string id,string name, string desc, string publisher, DateTime publish_date, string price, string discout, string genres, string author)
        {
            BaseItem item = MakeNewItem(name, desc, publisher, publish_date, price, discout, genres, author);
            if (!int.TryParse(id, out int itemId))
            {
                throw exceptions["id"];
            }
            data.UpdateItem(item, itemId);
        }
        public BaseItem MakeNewItem(string name, string desc, string publisher, DateTime publish_date, string price, string discout, string genres,string author)
        {   
            if(string.IsNullOrWhiteSpace(name))
            {
                throw exceptions["name"];
            }
            //if (string.IsNullOrEmpty(desc))
            //{
            //    throw exceptions["description"];
            //}
            if (string.IsNullOrWhiteSpace(publisher))
            {
                throw exceptions["publisher"];
            }
            if (DateTime.Now < publish_date)
            {
                throw exceptions["publish_date"];
            }
            if (!float.TryParse(price,out float itemPrice))
            {
                throw exceptions["price"];
            }
            if (!float.TryParse(discout, out float itemDiscount))
            {
                throw exceptions["discout"];
            }
            string[] genresItem = genres.Split(',');
            Genre ItemGenre = new Genre();
            for (int i = 0; i < genresItem.Length; i++)
            {
                if (Enum.TryParse(genresItem[i], out Genre result))
                    ItemGenre |= result;
                else
                    throw exceptions["genres"];
            }
            BaseItem item;
            if (string.IsNullOrWhiteSpace(author))
            {
                item = new Journal(name, desc,publisher,publish_date, itemPrice, itemDiscount,ItemGenre);
            }
            else
            {
                item = new Book(name, desc,author, publisher, publish_date, itemPrice, itemDiscount, ItemGenre);
            }
            return item;

        }
        public DataTable ShowAllSales()=> data.ShowAllItems("Sales");
        public DataTable ShowAllItems(string s) => data.ShowAllItems(s);
        //public DataTable ShowAllRented() => data.ShowAllItems("rented");
        public DataTable ShowItemByParam(string name, string param, string table) => data.ShowItemByParam(name, param, table);

        public void DeleteFromItems(string id) => data.RemoveItems(int.Parse(id),"items");
        //string name, string desc,string publisher, DateTime publish_date, float price, float discout, string[] genres
        public DataTable ShowAllBooks() => data.ShowAllBooks("items");
        public DataTable ShowAllJournals() => data.ShowAllJournals("items");
        public DataTable ShowAllRented() =>data.ShowAllItems("rented");
        public string DailyReportCount() => $"Books count: {data.CountBooks()}, Journals count: {data.CountJournals()}, Rented items count: {data.CountRented()}";
        public void RentItem(string id) => data.RentBook(int.Parse(id),_CurrentUser.UserName);
        public void ReturnItem(string id)
        {
            bool canReturn = data.ReturnBook(int.Parse(id), _CurrentUser.UserName);
                if (!canReturn)
            {
                throw exceptions["returnItem"];
            } 
                    
        }
        public void UpdateLates() => data.UpdateLates();

        public void AddSale(string parameter, string paramVal, string discount)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw exceptions["parameter"];
            }
            if (string.IsNullOrWhiteSpace(paramVal))
            {
                throw exceptions["paramVal"];
            }
            if (parameter=="Genre")
            {
                if (!Enum.TryParse(paramVal, out Genre result))             
                    throw exceptions["genres"]; 
            }
            if (!float.TryParse(discount, out float saleDiscount))
            {
                throw exceptions["discout"];
            }
            if (parameter == "Publish Date")
                parameter = "PublishDate";
            data.AddSale(new Sale(parameter, paramVal, saleDiscount));
        }
        public void DeleteSale(string saleId)=>data.DeleteSale(int.Parse(saleId));
        public bool UserIsAdmin()
        {
            if(_CurrentUser.IsAdmin) return true;
            return false;
        }
        public void Register(string username, SecureString password, bool _isEmployee)
        {
            ValidationUser(username, password);
            User user = new User(username, password, _isEmployee);
            data.AddUser(user);
        }
        public bool Login(string userName, SecureString password) {

                ValidationUser(userName, password);
                if (data.Login(userName, password))
                {
                    _CurrentUser = new User(userName, password, data.IsAdmin(userName, password));
                    return true;
                }

            return false;
            
        }
        private void ValidationUser(string userName, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw exceptions["username"];
            }
            if (password.Length < 3)
            {
                throw exceptions["password"];
            }
        }


    }

}

//public DataTable ShowItemByName(string name,string table) => data.ShowItemByName(name, table);
//public DataTable ShowItemByAuthor(string name, string table) => data.ShowItemByAuthor(name, table);
//public DataTable ShowItemByPublisher(string name, string table) => data.ShowItemByPublisher(name, table);
//public DataTable ShowItemByGenre(string name, string table) => data.ShowItemByGenre(name, table);
//public DataTable ShowItemByID(string id, string table){

// if (!int.TryParse(id, out int itemId))
//{
// throw exceptions["id"];
// }
//return data.ShowItemById(itemId,table);
//}


