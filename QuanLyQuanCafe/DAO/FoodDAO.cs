using QuanLyQuanCafe.DTO;
using System.Collections.Generic;
using System.Data;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        private FoodDAO() { }

        public static FoodDAO Instance { get { if (instance == null) instance = new FoodDAO(); return instance; } private set => instance = value; }
        public List<Food> GetFoodByCategoryId(int categoryId)
        {
            List<Food> list = new List<Food>();
            string query = "select * from food where idCategory = " + categoryId;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                list.Add(food);
            }
            return list;
        }
        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();
            string query = "select * from food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                list.Add(food);
            }
            return list;
        }
        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();
            string query = string.Format("select * from food where name like \"%{0}%\"", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                list.Add(food);
            }
            return list;
        }
        public bool InsertFood(string name, int idCategory, float price)
        {
            string query = string.Format("insert into food (name, idCategory, price) values (\"{0}\", {1}, {2});", name, idCategory, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateFood(string name, int idCategory, float price, int idFood)
        {
            string query = string.Format("update food set name = \"{0}\", idCategory = {1}, price = {2} where id = {3};", name, idCategory, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodId(idFood);
            string query = string.Format("delete from food where id = {0};", idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public void DeleteFoodByCategoryId(int categoryId)
        {
            DataTable foodIdList = DataProvider.Instance.ExecuteQuery("select * from food where idCategory = " + categoryId);
            foreach (DataRow row in foodIdList.Rows)
            {
                Food food = new Food(row);
                DeleteFood(food.ID);
            }
        }
    }
}
