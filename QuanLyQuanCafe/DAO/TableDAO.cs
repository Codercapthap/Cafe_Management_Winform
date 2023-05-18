using QuanLyQuanCafe.DTO;
using System.Collections.Generic;
using System.Data;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        private TableDAO() { }
        public static TableDAO Instance { get { if (instance == null) instance = new TableDAO(); return instance; } private set => instance = value; }
        public static int tableWidth = 90;
        public static int tableHeight = 90;
        public void Switch_Table(int idTable1, int idTable2)
        {
            DataProvider.Instance.ExecuteNonQuery("Switch_Table", true, new string[] { "idTable1", "idTable2" }, new object[] { idTable1, idTable2 });
        }
        public List<Table> loadTableList()
        {
            List<Table> tableList = new List<Table>();
            DataTable data = DataProvider.Instance.ExecuteQuery("GetTableList", true);

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;
        }

        public bool InsertTable(string name)
        {
            string query = string.Format("insert into tablefood (name) values (\"{0}\");", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateTable(string name, int idTable)
        {
            string query = string.Format("update tablefood set name = \"{0}\" where id = {1};", name, idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteTable(int idTable)
        {
            BillDAO.Instance.DeleteBillByIdTable(idTable);
            string query = string.Format("delete from tablefood where id = {0};", idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
