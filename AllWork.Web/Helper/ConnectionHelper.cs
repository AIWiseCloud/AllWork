using AllWork.Common;
using AllWork.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace AllWork.Web.Helper
{
    /// <summary>
    /// 从配置文件读取数据库连接信息到连接集合中 
    /// 说明：使用了C#的扩展方法，在方法参数中使用this, 这里扩展了IConfiguration, 然后要在startup中ConfigureServices
    /// </summary>
    public static class ConnectionHelper
    {
        //方法中的this参数, 这就是C#中的扩展方法
        //扩展方法可以写入最初没有提供该方法的类中。还可以把方法添加到实现某个接口的任何类中，这样多个类可以使用相同的实现代码
        //这样在startup中


        /// <summary>
        /// MySql数据库连接
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void LoadMySqlConnection(this IConfiguration configuration)
        {
            var server = configuration.GetSection("MySqlCon:server").Value.ToString();
            var port = configuration.GetSection("MySqlCon:port").Value.ToString();
            var database = configuration.GetSection("MySqlCon:database").Value.ToString();
            var SslMode = configuration.GetSection("MySqlCon:SslMode").Value.ToString();
            var uid = configuration.GetSection("MySqlCon:uid").Value.ToString();
            var pwd = DesEncrypt.Decrypt(configuration.GetSection("MySqlCon:pwd").Value.ToString());
            var connstr = $"server={server};port={port};database={database};SslMode={SslMode};uid={uid};pwd={pwd}";
            DbConfig.connStrDict.TryAdd(ConnType.MYSQL.ToString(), connstr);
        }

        public static void LoadSqlServerConnection(this IConfiguration configuration)
        {
            var server = configuration.GetSection("SqlServerCon:server").Value.ToString();
            var user = configuration.GetSection("SqlServerCon:user").Value.ToString();
            var password = DesEncrypt.Decrypt(configuration.GetSection("SqlServerCon:password").Value.ToString());
            var integrated = configuration.GetSection("SqlServerCon:integrated").Value.ToString();
            var database = configuration.GetSection("SqlServerCon:database").Value.ToString();
            var connstr = $"Server={server};user={user};password={password};integrated security={integrated};database={database};";
            DbConfig.connStrDict.TryAdd(ConnType.SQLSERVER.ToString(), connstr);
        }
    }
}
