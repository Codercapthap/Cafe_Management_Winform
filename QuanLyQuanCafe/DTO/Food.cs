using System;
using System.Data;

namespace QuanLyQuanCafe.DTO
{
    public class Food
    {
        private float price;
        private int categoryID;
        private string name;
        private int iD;
        public Food(int iD, string name, int categoryID, float price)
        {
            this.Price = price;
            this.CategoryID = categoryID;
            this.Name = name;
            this.ID = iD;
        }
        public Food(DataRow row)
        {
            this.ID = (int)row["id"];
            this.CategoryID = (int)row["idCategory"];
            this.Name = (string)row["name"];
            this.Price = (float)Convert.ToDouble(row["price"]);
        }

        public int ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
        public float Price { get => price; set => price = value; }
    }
}
