using MySql.Data.MySqlClient;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;

namespace AllWork.Repository.Base
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {
        //ConcurrentDictionary线程安全的字典缓存. 可以用于并发场景.
        public static ConcurrentDictionary<string, string> connStrDict = new ConcurrentDictionary<string, string>();

        public static IDbConnection GetDbConnection(ConnType connType = ConnType.MYSQL)
        {
            IDbConnection conn = default(IDbConnection);
            switch (connType)
            {
                case ConnType.MYSQL:
                    {
                        conn = new MySqlConnection(connStrDict[ConnType.MYSQL.ToString()]);
                        break;
                    }
                case ConnType.SQLSERVER:
                    {
                        conn = new SqlConnection(connStrDict[ConnType.SQLSERVER.ToString()]);
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
            //conn.Open();
            return conn;

        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum ConnType
    {
        MYSQL = 1,
        SQLSERVER = 2
    }
}
