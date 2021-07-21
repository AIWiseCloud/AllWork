using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace AllWork.Repository.Base
{
    public class BaseRepository<TEntity> : IRepository.Base.IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly IConfiguration _ccnfiguration;

        public BaseRepository(IConfiguration configuration) => this._ccnfiguration = configuration;

        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetOpenConn()
        {
            IDbConnection con;
            //string connectionString = _ccnfiguration["ConnectionStrings:SqlServerConn"]; //从appsettings.json读取连接配置
            //con = new SqlConnection(connectionString);

            string connectionString = _ccnfiguration["ConnectionStrings:MySqlConn"]; //从appsettings.json读取连接配置
            con = new MySqlConnection(connectionString);

            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("数据库连接错误:" + ex.Message);
            }
            return con;
        }

        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="sql">查询Sql语句或存储过程名称</param>
        /// <param name="param">参数值（可选）</param>
        /// <param name="transaction">事务名称（可选）</param>
        /// <param name="commandTimeout">超时时间（可选）</param>
        /// <param name="commandType">指定如果解释sql字符串：语句/存储过程（可选）</param>
        /// <returns>返回指定实体泛型</returns>
        public async Task<TEntity> QueryFirst(string sql, object param = null, IDbTransaction transaction = null, int? commandTmeout = null, CommandType? commandType = null)
        {
            using var con = GetOpenConn();
            //以下开始用con.QueryFirstAsync 报错sequence containts no elements
            return await con.QueryFirstOrDefaultAsync<TEntity>(sql, param, transaction, commandTmeout, commandType);
        }

        public async Task<DataSet> Query(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var con = GetOpenConn();
            IDataReader reader = await con.ExecuteReaderAsync(sql, param, transaction, commandTimeout, commandType);
            DataSet ds = new XDataSet();
            ds.Load(reader, LoadOption.OverwriteChanges, null, new DataTable[] { });
            return ds;
        }

        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="sql">查询Sql语句或存储过程名称</param>
        /// <param name="param">参数值（可选）</param>
        /// <param name="transaction">事务名称（可选）</param>
        /// <param name="buffered">是否将查询结果缓存到内存中（可选）</param>
        /// <param name="commandTimeout">超时时间（可选）</param>
        /// <param name="commandType">指定如果解释sql字符串：语句/存储过程（可选）</param>
        /// <returns>返回指定泛型集合</returns>
        public async Task<IEnumerable<TEntity>> QueryList(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var con = GetOpenConn();
            return await con.QueryAsync<TEntity>(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// 简单分页，返回分页后的泛型集合
        /// </summary>
        /// <typeparam name="T">分页后的泛型集合</typeparam>
        /// <param name="sql">查询Sql语句或存储过程名称</param>
        /// <param name="totalCount">返回 总记录数</param>
        /// <param name="param">参数值（可选）</param>
        /// <param name="transaction">事务名称（可选）</param>
        /// <param name="buffered">是否将查询结果缓存到内存中（可选）</param>
        /// <param name="commandTimeout">超时时间（可选）</param>
        /// <param name="commandType">指定如果解释sql字符串：语句/存储过程（可选）</param>
        /// <returns>返回分页后的泛型集合</returns>
        public async Task<Tuple<IEnumerable<TEntity>, int>> QueryPagination(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var con = GetOpenConn();
            var multi = await con.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
            int totalCount = multi.Read<long>().AsList().Count;
            return Tuple.Create<IEnumerable<TEntity>, int>(multi.Read<TEntity>(), totalCount);
        }

        /// <summary>
        /// 2条Sql语句查询
        /// </summary>
        /// <typeparam name="TFirst">实体集合一</typeparam>
        /// <typeparam name="TSecond">实体集合二</typeparam>
        /// <param name="sql">2条查询语句</param>
        /// <param name="tfList">返回第一条语句的实体集合</param>
        /// <param name="tsList">返回第二条语句的实体集合</param>
        /// <param name="param">参数值（可选）</param>
        public async Task<Tuple<List<TFirst>, List<TSecond>>> QueryMultiple<TFirst, TSecond>(string sql, object param = null)
        {
            using var con = GetOpenConn();
            var multi = await con.QueryMultipleAsync(sql, param);
            var tfList = new List<TFirst>();
            var tsList = new List<TSecond>();
            if (!multi.IsConsumed)
            {
                tfList = multi.Read<TFirst>().AsList();
                tsList = multi.Read<TSecond>().AsList();
            }
            return Tuple.Create<List<TFirst>, List<TSecond>>(tfList, tsList);
        }

        /// <summary>
        /// 3条Sql语句查询
        /// </summary>
        /// <typeparam name="TFirst">实体集合一</typeparam>
        /// <typeparam name="TSecond">实体集合二</typeparam>
        /// <typeparam name="TThird">实体集合三</typeparam>
        /// <param name="sql">5条查询语句</param>
        /// <param name="tfList">返回第一条语句的实体集合</param>
        /// <param name="tsList">返回第二条语句的实体集合</param>
        /// <param name="ttList">返回第三条语句的实体集合</param>
        /// <param name="param">参数值（可选）</param>
        public async Task<Tuple<List<TFirst>, List<TSecond>, List<TThird>>> QueryMultiple<TFirst, TSecond, TThird>(string sql, object param = null)
        {
            using var con = GetOpenConn();
            var multi = await con.QueryMultipleAsync(sql, param);
            var tfList = new List<TFirst>();
            var tsList = new List<TSecond>();
            var ttList = new List<TThird>();
            if (!multi.IsConsumed)
            {
                tfList = multi.Read<TFirst>().AsList();
                tsList = multi.Read<TSecond>().AsList();
                ttList = multi.Read<TThird>().AsList();
            }
            return Tuple.Create<List<TFirst>, List<TSecond>, List<TThird>>(tfList, tsList, ttList);
        }

        /// <summary>
        /// 4条Sql语句查询
        /// </summary>
        /// <typeparam name="TFirst">实体集合一</typeparam>
        /// <typeparam name="TSecond">实体集合二</typeparam>
        /// <typeparam name="TThird">实体集合三</typeparam>
        /// <typeparam name="TFour">实体集合四</typeparam>
        /// <param name="sql">5条查询语句</param>
        /// <param name="tfList">返回第一条语句的实体集合</param>
        /// <param name="tsList">返回第二条语句的实体集合</param>
        /// <param name="ttList">返回第三条语句的实体集合</param>
        /// <param name="tfourList">返回第四条语句的实体集合</param>
        /// <param name="param">参数值（可选）</param>
        public async Task<Tuple<List<TFirst>, List<TSecond>, List<TThird>, List<TFour>>> QueryMultiple<TFirst, TSecond, TThird, TFour>(string sql, object param = null)
        {
            using var con = GetOpenConn();
            var multi = await con.QueryMultipleAsync(sql, param);
            var tfList = new List<TFirst>();
            var tsList = new List<TSecond>();
            var ttList = new List<TThird>();
            var tfourList = new List<TFour>();
            if (!multi.IsConsumed)
            {
                tfList = multi.Read<TFirst>().AsList();
                tsList = multi.Read<TSecond>().AsList();
                ttList = multi.Read<TThird>().AsList();
                tfourList = multi.Read<TFour>().AsList();
            }
            return Tuple.Create<List<TFirst>, List<TSecond>, List<TThird>, List<TFour>>(tfList, tsList, ttList, tfourList);
        }

        /// <summary>
        /// 5条Sql语句查询
        /// </summary>
        /// <typeparam name="TFirst">实体集合一</typeparam>
        /// <typeparam name="TSecond">实体集合二</typeparam>
        /// <typeparam name="TThird">实体集合三</typeparam>
        /// <typeparam name="TFour">实体集合四</typeparam>
        /// <typeparam name="TFive">实体集合五</typeparam>
        /// <param name="sql">5条查询语句</param>
        /// <param name="tfList">返回第一条语句的实体集合</param>
        /// <param name="tsList">返回第二条语句的实体集合</param>
        /// <param name="ttList">返回第三条语句的实体集合</param>
        /// <param name="tfourList">返回第四条语句的实体集合</param>
        /// <param name="tfiveList">返回第五条语句的实体集合</param>
        /// <param name="param">参数值（可选）</param>
        public async Task<Tuple<List<TFirst>, List<TSecond>, List<TThird>, List<TFour>, List<TFive>>> QueryMultiple<TFirst, TSecond, TThird, TFour, TFive>(string sql, object param = null)
        {
            using var con = GetOpenConn();
            var multi = await con.QueryMultipleAsync(sql, param);
            var tfList = new List<TFirst>();
            var tsList = new List<TSecond>();
            var ttList = new List<TThird>();
            var tfourList = new List<TFour>();
            var tfiveList = new List<TFive>();
            if (!multi.IsConsumed)
            {
                tfList = multi.Read<TFirst>().AsList();
                tsList = multi.Read<TSecond>().AsList();
                ttList = multi.Read<TThird>().AsList();
                tfourList = multi.Read<TFour>().AsList();
                tfiveList = multi.Read<TFive>().AsList();
            }
            return Tuple.Create<List<TFirst>, List<TSecond>, List<TThird>, List<TFour>, List<TFive>>(tfList, tsList, ttList, tfourList, tfiveList);
        }

        /// <summary>
        /// 查询单个实体类型
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">查询Sql语句或存储过程名称</param>
        /// <param name="param">参数值（可选）</param>
        /// <param name="transaction">事务名称（可选）</param>
        /// <param name="buffered">是否将查询结果缓存到内存中（可选）</param>
        /// <param name="commandTimeout">超时时间（可选）</param>
        /// <param name="commandType">指定如果解释sql字符串：语句/存储过程（可选）</param>
        /// <returns>泛型实体类型</returns>
        public async Task<TEntity> QueryOne(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var dataResult = await QueryList(sql, param, transaction, buffered, commandTimeout, commandType);
            return dataResult != null && dataResult.AsList().Count > 0 ? dataResult.AsList()[0] : new TEntity();
        }

        /// <summary>
        /// 执行sql语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">查询Sql语句或存储过程名称</param>
        /// <param name="param">参数值（可选）</param>
        /// <param name="transaction">事务名称（可选）</param>
        /// <param name="commandTimeout">超时时间（可选）</param>
        /// <param name="commandType">指定如果解释sql字符串：语句/存储过程（可选）</param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var con = GetOpenConn();
            return await con.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行sql语句，返回第一行第一列
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="sql">查询Sql语句</param>
        /// <param name="param">参数值（可选）</param>
        /// <param name="transaction">事务名称（可选）</param>
        /// <param name="commandTimeout">超时时间（可选）</param>
        /// <param name="commandType">指定如果解释sql字符串：语句/存储过程（可选）</param>
        /// <returns>返回返回第一行第一列</returns>
        public async Task<TEntity> ExecuteScalar(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var con = GetOpenConn();
            return await con.ExecuteScalarAsync<TEntity>(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// 执行存储过程，返回第一行第一列
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="command">存储过程名称</param>
        /// <param name="paras">参数键值对</param>
        /// <returns>返回第一行第一列</returns>
        public TEntity Execute(string command, Dictionary<string, object> paras)
        {
            using var con = GetOpenConn();
            IDbCommand com = con.CreateCommand();
            com.CommandText = command;
            com.CommandType = CommandType.StoredProcedure;

            if (paras != null)
            {
                foreach (var item in paras.Keys)
                {
                    IDbDataParameter para = com.CreateParameter();
                    para.Value = paras[item];
                    para.ParameterName = item;
                    com.Parameters.Add(para);
                }
            }

            return (TEntity)com.ExecuteScalar();
        }

        /// <summary>
        /// 数据适配器，扩展Fill方法
        /// .NET的DataSet.Load方法，底层调用DataAdapter.Fill(DataTable[], IDataReader, int, int)
        /// Dapper想要返回DataSet，需要重写Load方法，不必传入DataTable[]，因为数组长度不确定
        /// </summary>
        public class XLoadAdapter : DataAdapter
        {
            /// <summary>
            /// 数据适配器
            /// </summary>
            public XLoadAdapter()
            {
            }

            /// <summary>
            /// 读取dataReader
            /// </summary>
            /// <param name="ds"></param>
            /// <param name="dataReader"></param>
            /// <param name="startRecord"></param>
            /// <param name="maxRecords"></param>
            /// <returns></returns>
            public int FillFromReader(DataSet ds, IDataReader dataReader, int startRecord, int maxRecords)
            {
                return this.Fill(ds, "Table", dataReader, startRecord, maxRecords);
            }
        }

        /// <summary>
        /// 扩展Load方法
        /// </summary>
        public class XDataSet : DataSet
        {
            /// <summary>
            /// Dapper想要返回DataSet，需要重写Load方法
            /// </summary>
            /// <param name="reader">IDataReader</param>
            /// <param name="loadOption">LoadOption</param>
            /// <param name="handler">FillErrorEventHandler</param>
            /// <param name="tables">DataTable</param>
            public override void Load(IDataReader reader, LoadOption loadOption, FillErrorEventHandler handler, params DataTable[] tables)
            {
                XLoadAdapter adapter = new XLoadAdapter
                {
                    FillLoadOption = loadOption,
                    MissingSchemaAction = MissingSchemaAction.AddWithKey
                };
                if (handler != null)
                {
                    adapter.FillError += handler;
                }
                adapter.FillFromReader(this, reader, 0, 0);
                if (!reader.IsClosed && !reader.NextResult())
                {
                    reader.Close();
                }
            }
        }
    }
}



