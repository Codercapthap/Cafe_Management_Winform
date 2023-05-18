using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class AccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount { get => loginAccount; set { loginAccount = value; ChangeAccount(); } }
        public AccountProfile(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
        }
        #region methods

        private void ChangeAccount()
        {
            tabUsername.Text = LoginAccount.Username;
            tabDisplayName.Text = LoginAccount.DisplayName;
        }
        private void UpdateAccount()
        {
            string dislayName = tabDisplayName.Text;
            string password = tabPassword.Text;
            string newPassword = tabNewPassword.Text;
            string confirmNewPassword = tabConfirmPassword.Text;
            string username = tabUsername.Text;
            if (!newPassword.Equals(confirmNewPassword))
            {
                MessageBox.Show("Your new password and confirm password does not match!");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(username, password, newPassword, dislayName))
                {
                    MessageBox.Show("Update successfully!");
                    if (updateAccountEvent != null)
                    {
                        updateAccountEvent(this, new AccountEvent(AccountDAO.Instance.LoadAccountByUsername(username)));
                    }
                }
                else
                {
                    MessageBox.Show("Your password wrong!");
                }
            }
        }
        #endregion
        #region events
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AccountProfile_Load(object sender, EventArgs e)
        {

        }
        private event EventHandler<AccountEvent> updateAccountEvent;
        public event EventHandler<AccountEvent> UpdateAccountEvent
        {
            add { updateAccountEvent += value; }
            remove { updateAccountEvent -= value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }
        #endregion
    }
    public class AccountEvent : EventArgs
    {
        private Account account;

        public Account Account { get => account; set => account = value; }
        public AccountEvent(Account account)
        {
            this.Account = account;
        }
    }
}
