using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class Admin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource account = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        public Account loginAccount;
        public Admin()
        {
            InitializeComponent();
            Load();
        }

        #region methods
        private void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = account;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadAccount();
            LoadListTable();
            LoadCategoryIntoCombobox(cbCategory);
            LoadListCategory();
            AddFoodBinding();
            AddAccountBinding();
            AddCategoryBinding();
            AddTableBinding();
        }
        private void LoadAccount()
        {
            account.DataSource = AccountDAO.Instance.GetListAccount();
        }
        private void AddAccountBinding()
        {
            tabUsername.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Username"));
            tabDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Display Name"));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type"));
        }
        private void LoadListBillByDate(DateTime dateCheckIn, DateTime dateCheckOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dateCheckIn, dateCheckOut, 1);
        }
        private void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        private void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        private void AddFoodBinding()
        {
            tabFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            tabFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "id", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        private void AddCategoryBinding()
        {
            tabCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "id", true, DataSourceUpdateMode.Never));
            tabCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        private void AddTableBinding()
        {
            tabTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "id", true, DataSourceUpdateMode.Never));
            tabTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            cbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));
        }
        private void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        private void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        private void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.loadTableList();
        }
        private List<Food> SearchFoodByName(string name)
        {
            List<Food> foods = FoodDAO.Instance.SearchFoodByName(name);
            return foods;
        }
        private void AddAccount(string username, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(username, displayName, type))
            {
                MessageBox.Show("Add account successfully!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
            LoadAccount();
        }
        private void UpdateAccount(string username, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(username, displayName, type))
            {
                MessageBox.Show("Update account successfully!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
            LoadAccount();
        }
        private void DeleteAccount(string username)
        {
            if (loginAccount.Username.Equals(username))
            {
                MessageBox.Show("Please do not delete your account!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(username))
            {
                MessageBox.Show("Delete account successfully!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
            LoadAccount();
        }
        private void ResetPassword(string username)
        {
            if (AccountDAO.Instance.ResetPassword(username))
            {
                MessageBox.Show("Reset password successfully!");
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }
        }
        #endregion
        #region events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }


        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void tabFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryById(id);
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbCategory.Items)
                    {
                        if (category != null && item.ID == category.ID)
                        {
                            index = i; break;
                        }
                        i++;
                    }
                    cbCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = tabFoodName.Text;
            int idCategory = (cbCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            if (FoodDAO.Instance.InsertFood(name, idCategory, price))
            {
                MessageBox.Show("Insert successfully!");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {

            string name = tabFoodName.Text;
            int idCategory = (cbCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int idFood = Convert.ToInt32(tabFoodID.Text);
            if (FoodDAO.Instance.UpdateFood(name, idCategory, price, idFood))
            {
                MessageBox.Show("Update successfully!");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(tabFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                MessageBox.Show("Delete successfully!");
                LoadListFood();
                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            dtgvFood.DataSource = SearchFoodByName(tabSearchFoodName.Text);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string username = tabUsername.Text;
            string displayName = tabDisplayName.Text;
            int type = (int)nmAccountType.Value;
            AddAccount(username, displayName, type);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string username = tabUsername.Text;
            string displayName = tabDisplayName.Text;
            int type = (int)nmAccountType.Value;
            UpdateAccount(username, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = tabUsername.Text;
            DeleteAccount(username);
        }
        #endregion

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = tabUsername.Text;
            ResetPassword(username);
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            tabBillPage.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
            {
                lastPage++;
            }
            tabBillPage.Text = lastPage.ToString();
        }

        private void tabBillPage_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(tabBillPage.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(tabBillPage.Text);
            if (page > 1)
            {
                page--;
            }
            tabBillPage.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(tabBillPage.Text);
            if (page < BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value))
            {
                page++;
            }
            tabBillPage.Text = page.ToString();
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string categoryName = tabCategoryName.Text;
            int categoryId = Convert.ToInt32(tabCategoryID.Text);
            if (CategoryDAO.Instance.UpdateCategory(categoryName, categoryId))
            {
                MessageBox.Show("Update successfully!");
                LoadListCategory();
                if (updateCategory != null)
                {
                    updateCategory(this, new EventArgs());
                    LoadCategoryIntoCombobox(cbCategory);
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {

            int categoryId = Convert.ToInt32(tabCategoryID.Text);
            if (CategoryDAO.Instance.DeleteCategory(categoryId))
            {
                MessageBox.Show("Delete successfully!");
                LoadListCategory();
                if (deleteCategory != null)
                {
                    deleteCategory(this, new EventArgs());
                    LoadListFood();
                    LoadCategoryIntoCombobox(cbCategory);
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string categoryName = tabCategoryName.Text;
            if (CategoryDAO.Instance.InsertCategory(categoryName))
            {
                MessageBox.Show("Insert successfully!");
                LoadListCategory();
                if (insertCategory != null)
                {
                    insertCategory(this, new EventArgs());
                    LoadCategoryIntoCombobox(cbCategory);
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }


        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }
        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string tableName = tabTableName.Text;
            int tableId = Convert.ToInt32(tabTableID.Text);
            if (TableDAO.Instance.UpdateTable(tableName, tableId))
            {
                MessageBox.Show("Update successfully!");
                LoadListTable();
                if (updateTable != null)
                {
                    updateTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int tableId = Convert.ToInt32(tabTableID.Text);
            if (TableDAO.Instance.DeleteTable(tableId))
            {
                MessageBox.Show("Delete successfully!");
                LoadListTable();
                if (deleteTable != null)
                {
                    deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string tableName = tabTableName.Text;
            if (TableDAO.Instance.InsertTable(tableName))
            {
                MessageBox.Show("Insert successfully!");
                LoadListTable();
                if (insertTable != null)
                {
                    insertTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }
        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
    }
}
