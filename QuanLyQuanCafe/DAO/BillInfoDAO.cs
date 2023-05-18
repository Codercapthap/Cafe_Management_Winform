using QuanLyQuanCafe.DTO;
using System.Collections.Generic;
using System.Data;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;
        private BillInfoDAO() { }

        public static BillInfoDAO Instance { get { if (instance == null) instance = new BillInfoDAO(); return instance; } private set => instance = value; }
        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from billinfo where idBill = " + id);
            foreach (DataRow row in data.Rows)
            {
                BillInfo info = new BillInfo(row);
                listBillInfo.Add(info);
            }
            return listBillInfo;
        }

        public void DeleteBillInfoByFoodId(int foodId)
        {
            DataProvider.Instance.ExecuteQuery("delete from billinfo where idFood = " + foodId);
        }
        public void DeleteBillInfoByIdBill(int idBill)
        {
            DataProvider.Instance.ExecuteQuery("delete from billinfo where idBill = " + idBill);
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("InsertBillInfo", true, new string[] { "idBill", "idFood", "count" }, new object[] { idBill, idFood, count });
        }
    }
}
