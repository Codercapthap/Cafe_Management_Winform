using MySql.Data.MySqlClient;
using System.Data;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;
        private string connectDB = @"server=127.0.0.1;port=3306;uid=root;pwd=root;database=quanlyquancafe";

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set => instance = value;
        }


        private DataProvider() { }
        public DataTable ExecuteQuery(string query, bool isProcedure = false, string[] parameter = null, object[] value = null)
        {
            DataTable data = new DataTable();

            using (MySqlConnection connection = new MySqlConnection(connectDB))
            {

                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);

                if (isProcedure)
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameter != null)
                    {
                        int i = 0;
                        foreach (string para in parameter)
                        {
                            command.Parameters.AddWithValue(para, value[i]);
                            i++;
                        }
                    }
                }
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
                return data;
            }
        }

        public DataTable ExecuteProcMultiTime(string query, string[] parameter, object[] value)
        {
            DataTable data = new DataTable();

            using (MySqlConnection connection = new MySqlConnection(connectDB))
            {

                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                int i = 0;
                foreach (string para in parameter)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue(para, value[i]);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(data);
                    i++;
                }
                connection.Close();
                return data;
            }
        }

        public int ExecuteNonQuery(string query, bool isProcedure = false, string[] parameter = null, object[] values = null)
        {
            int data = 0;

            using (MySqlConnection connection = new MySqlConnection(connectDB))
            {

                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);

                if (isProcedure)
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameter != null)
                    {
                        int i = 0;
                        foreach (string para in parameter)
                        {
                            command.Parameters.AddWithValue(para, values[i]);
                            i++;
                        }
                    }

                }
                data = command.ExecuteNonQuery();
                return data;
            }
        }

        public object ExecuteScalar(string query, bool isProcedure = false, string[] parameter = null, object[] values = null)
        {

            using (MySqlConnection connection = new MySqlConnection(connectDB))
            {
                object data = 0;

                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);

                if (isProcedure)
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameter != null)
                    {
                        int i = 0;
                        foreach (string para in parameter)
                        {
                            command.Parameters.AddWithValue(para, values[i]);
                            i++;
                        }
                    }

                }
                data = command.ExecuteScalar();
                return data;
            }
        }
    }
}
