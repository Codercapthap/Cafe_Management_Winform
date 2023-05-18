using System.Data;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        public Account(string username, string password, int type, string displayName)
        {
            this.Username = username;
            this.Password = password;
            this.Type = type;
            this.DisplayName = displayName;
        }
        public Account(DataRow row)
        {
            this.Username = (string)row["username"];
            this.Password = (string)row["password"];
            this.Type = (int)row["type"];
            this.DisplayName = (string)row["displayName"];
        }
        private int type;
        private string password;
        private string displayName;
        private string username;

        public string Username { get => username; set => username = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string Password { get => password; set => password = value; }
        public int Type { get => type; set => type = value; }
    }
}
