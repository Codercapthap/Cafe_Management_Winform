using QuanLyQuanCafe.DTO;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance { get { if (instance == null) instance = new AccountDAO(); return instance; } private set => instance = value; }

        private AccountDAO() { }

        public bool login(string username, string password)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hashPass = "";
            foreach (byte b in hashData)
            {
                hashPass += b;
            }
            string query = "Login";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, true, new string[] { "inputUsername", "inputPassword" }, new object[] { username, hashPass });
            return result.Rows.Count == 1;
        }
        public bool UpdateAccount(string username, string password, string newPassword, string displayName)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("UpdateAccount", true, new string[] { "username", "displayName", "password", "newPassword" }, new object[] { username, displayName, password, newPassword });
            return result > 0;
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select username as \"Username\", displayName as \"Display Name\", type as \"Type\" from account");
        }
        public Account LoadAccountByUsername(string username)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from account where username = '" + username + "'");
            Account result = new Account(data.Rows[0]);
            return result;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("insert into account (username, displayName, type) values (\"{0}\", \"{1}\", {2});", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("update account set displayName = \"{0}\", type = {1} where username = \"{2}\";", displayName, type, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string name)
        {
            string query = string.Format("delete from account where username = \"{0}\";", name);

            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPassword(string name)
        {
            string query = string.Format("update account set password = \"0\" where username = \"{0}\";", name);

            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
