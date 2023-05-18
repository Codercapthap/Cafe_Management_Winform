using QuanLyQuanCafe.DTO;
using System.Collections.Generic;
using System.Data;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;
        private CategoryDAO() { }

        public static CategoryDAO Instance { get { if (instance == null) instance = new CategoryDAO(); return instance; } private set => instance = value; }
        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();
            string query = "select * from foodcategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Category category = new Category(row);
                list.Add(category);
            }
            return list;
        }
        public Category GetCategoryById(int categoryId)
        {
            Category category = null;
            string query = "select * from foodcategory where id = " + categoryId;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                category = new Category(row);
                return category;
            }
            return category;
        }
        public bool InsertCategory(string name)
        {
            string query = string.Format("insert into foodcategory (name) values (\"{0}\");", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateCategory(string name, int idCategory)
        {
            string query = string.Format("update foodcategory set name = \"{0}\" where id = {1};", name, idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteCategory(int idCategory)
        {
            FoodDAO.Instance.DeleteFoodByCategoryId(idCategory);
            string query = string.Format("delete from foodcategory where id = {0};", idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
