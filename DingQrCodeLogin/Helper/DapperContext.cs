using System;
using MySql.Data.MySqlClient;

namespace DingQrCodeLogin.Helper
{
    public class DapperContext
    {
        //连接字符串
        const string connectionString = "Server=gz-cdb-ihjz7tpf.sql.tencentcdb.com;port=62119;Database=alibb_tm;Uid=root;Pwd=alibb888;SslMode=None;";
        public static MySqlConnection Connection()
        {

            var mysql = new MySqlConnection(connectionString);
            mysql.Open();
            return mysql;

        }
    }
}
