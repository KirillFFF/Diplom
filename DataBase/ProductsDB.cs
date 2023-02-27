using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProductsServer.DataBase
{
    public class ProductsDB
    {
        //private readonly SemaphoreSlim _semaphore = new(1);
        private static MySqlConnection _mySqlConnection;
        private static volatile ProductsDB _class;
        public static ProductsDB Instance
        {
            get
            {
                return _class ??= new ProductsDB();
                //if (_class == null)
                //{
                //    lock (_obj)
                //    {
                //        if (_class == null)
                //            return _class = new ProductsDB();
                //    }
                //}

                //return _class;
            }
        }
        public static string SetConnection { private get; set; }
        public static MySqlConnection ConnectionDB => _mySqlConnection ??= new(SetConnection);

        private void ConnectionOpenAsync()
        {
            //await _semaphore.WaitAsync();
            if (ConnectionDB.State == ConnectionState.Closed)
                ConnectionDB.Open();
            //_semaphore.Release();
        }

        private void ConnectionCloseAsync()
        {
            //await _semaphore.WaitAsync();
            if (ConnectionDB.State == ConnectionState.Open)
                ConnectionDB.Close();
            //_semaphore.Release();
        }

        public async Task<DataTable> GetDataAsync(MySqlCommand mySqlCommand)
        {
            //await _semaphore.WaitAsync();
            using MySqlDataAdapter adapter = new();
            using DataTable table = new();
            mySqlCommand.Connection = ConnectionDB;
            ConnectionOpenAsync();
            adapter.SelectCommand = mySqlCommand;
            await adapter.FillAsync(table);
            ConnectionCloseAsync();
            //_semaphore.Release();
            return table;
        }

        public async Task SetDataAsync(MySqlCommand mySqlCommand)
        {
            //await _semaphore.WaitAsync();
            using MySqlDataAdapter adapter = new();
            mySqlCommand.Connection = ConnectionDB;
            ConnectionOpenAsync();
            await mySqlCommand.ExecuteNonQueryAsync();
            ConnectionCloseAsync();
            //_semaphore.Release();
        }

        public async Task<string> ExpirationDate(DateTime date)
        {
            //await _semaphore.WaitAsync();
            using MySqlCommand command = new();
            command.CommandText = $"SELECT DATEDIFF(STR_TO_DATE(@startDate, '%d.%m.%Y'), NOW()) AS Days";
            command.Parameters.AddWithValue("@startDate", date);
            command.Connection = ConnectionDB;
            ConnectionOpenAsync(); 
            string result = (await command.ExecuteScalarAsync()).ToString();
            ConnectionCloseAsync();
            //_semaphore.Release();
            return result;
        }
        public async Task<string> DateFormat(DateTime date, string format)
        {
            //await _semaphore.WaitAsync();
            using MySqlCommand command = new();
            command.CommandText = $"SELECT DATE_FORMAT(STR_TO_DATE(@date, '%d.%m.%Y'), @format);";
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@format", format);
            command.Connection = ConnectionDB;
            ConnectionOpenAsync();
            string result = (await command.ExecuteScalarAsync()).ToString();
            ConnectionCloseAsync();
            //_semaphore.Release();
            return result;
        }
    }
}