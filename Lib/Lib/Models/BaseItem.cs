using Lib;
using System;

public abstract class BaseItem
    {

        // is ID property needed here when using sqlite?
        // public int ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Publisher { get; private set; }
        public DateTime PublishDate { get; private set; }
        public DateTime? RentDate { get; set; } // if null the book isn't rented
        public DateTime? ReturnDate { get; set; }// if null the book isn't return
        public float Price { get; private set; }
        public float Discount { get; private set; }
        public Genre Genres { get; private set; }



        public BaseItem(string name, string desc ,string publisher, DateTime publish_date, float price, float discout , Genre genre)
        {
            Name = name;
            Publisher = publisher;
            PublishDate = publish_date;
            Price = price;
            Discount = discout;
            Description = desc;
            Genres = genre;
            //for (int i = 0; i < genres.Length; i++)
            //{
            //    if (Enum.TryParse(genres[i], out Genre result))
            //        Genres |= result;
            //}
            //for (int i = 0; i < genres.Length; i++)
  
                //Genres |= genres[i];

        }


        // add ctor for not recieving desc and discounyt

    }

