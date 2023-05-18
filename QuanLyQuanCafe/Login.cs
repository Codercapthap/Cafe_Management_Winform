using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tabPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = tabUsername.Text;
            string password = tabPassword.Text;
            if (login(username, password))
            {
                Account acc = AccountDAO.Instance.LoadAccountByUsername(username);
                TableManager f = new TableManager(acc);
                this.Hide();
                f.ShowDialog();
                //show dialog chan cac hanh dong 
                //ben duoi, dung show binh thuong thi no se chay 
                //hide xong show lai luon
                this.Show();
            }
            else
            {
                MessageBox.Show("Wrong username or password");
            }
        }

        private bool login(string username = "", string password = "")
        {
            return AccountDAO.Instance.login(username, password);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you really want to exit?", "Warning", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
