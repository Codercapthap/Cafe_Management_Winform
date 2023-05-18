using System.Data;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfo
    {
        public BillInfo(int iD, int billId, int foodId, int count)
        {
            this.ID = iD;
            this.BillId = billId;
            this.FoodId = foodId;
            this.Count = count;
        }

        public BillInfo(DataRow data)
        {
            this.ID = (int)data["id"];
            this.BillId = (int)data["idBill"];
            this.FoodId = (int)data["idFood"];
            this.Count = (int)data["count"];
        }
        private int iD;
        private int billId;
        private int foodId;
        private int count;

        public int ID { get => iD; set => iD = value; }
        public int BillId { get => billId; set => billId = value; }
        public int FoodId { get => foodId; set => foodId = value; }
        public int Count { get => count; set => count = value; }
    }
}
