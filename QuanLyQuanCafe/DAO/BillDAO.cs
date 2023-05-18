using QuanLyQuanCafe.DTO;
using System;
using System.Data;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance { get { if (instance == null) instance = new BillDAO(); return instance; } private set => instance = value; }
        private BillDAO() { }
        public int GetUncheckedBillIdByTableId(int tableId)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from bill where idTable = " + tableId + " and status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("InsertBill", true, new string[] { "idTable" }, new object[] { id });
        }
        public DataTable GetListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            DataTable listBill = DataProvider.Instance.ExecuteQuery("GetListBillByDate", true, new string[] { "startDate", "endDate" }, new object[] { checkIn, checkOut });
            return listBill;
        }
        public DataTable GetListBillByDateAndPage(DateTime checkIn, DateTime checkOut, int page)
        {
            DataTable listBill = DataProvider.Instance.ExecuteQuery("GetListBillByDateAndPage", true, new string[] { "startDate", "endDate", "page" }, new object[] { checkIn, checkOut, page });
            return listBill;
        }
        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return Convert.ToInt32(DataProvider.Instance.ExecuteScalar("GetNumBillByDate", true, new string[] { "startDate", "endDate" }, new object[] { checkIn, checkOut }));
        }
        public int GetMaxIdBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select max(id) from bill");
            }
            catch
            {
                return 1;
            }
        }

        public void CheckOut(int billId, int discount, float totalPrice)
        {
            string query = "update bill set dateCheckOut = current_timestamp(), status = 1, discount = " + discount + ", totalPrice = " + totalPrice + " where id = " + billId;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void DeleteBillByIdTable(int idTable)
        {
            DataTable billIdList = DataProvider.Instance.ExecuteQuery("select * from bill where idTable = " + idTable);
            foreach (DataRow row in billIdList.Rows)
            {
                Bill bill = new Bill(row);
                BillInfoDAO.Instance.DeleteBillInfoByIdBill(bill.ID);
                DataProvider.Instance.ExecuteNonQuery("delete from bill where id = " + bill.ID);
            }
        }
    }

}
