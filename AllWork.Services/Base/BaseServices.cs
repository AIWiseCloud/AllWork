using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using System;

namespace AllWork.Services.Base
{
    public class BaseServices<TEntity> where TEntity:class,new()
    {
        public AllWork.IRepository.Base.IBaseRepository<TEntity> BaseDal;

        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="sql">查询Sql语句或存储过程名称</param>
        /// <param name="param">参数值（可选）</param>
        /// <param name="transaction">事务名称（可选）</param>
        /// <param name="commandTimeout">超时时间（可选）</param>
        /// <param name="commandType">指定如果解释sql字符串：语句/存储过程（可选）</param>
        /// <returns>返回指定实体泛型</returns>
        public async Task<TEntity> QueryFirst(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await BaseDal.QueryFirst(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<DataSet> Query(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return BaseDal.Query(sql, param, transaction, commandTimeout, commandType);
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
            return await BaseDal.QueryList(sql, param, transaction, buffered, commandTimeout, commandType);
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

            return await BaseDal.QueryPagination(sql, param, transaction, buffered, commandTimeout, commandType);
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

            return await BaseDal.QueryMultiple<TFirst, TSecond>(sql, param);
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
            return await BaseDal.QueryMultiple<TFirst, TSecond, TThird>(sql, param);
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

            return await BaseDal.QueryMultiple<TFirst, TSecond, TThird, TFour>(sql, param);
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
            return await BaseDal.QueryMultiple<TFirst, TSecond, TThird, TFour, TFive>(sql, param);
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
            return await BaseDal.QueryOne(sql, param, transaction, buffered, commandTimeout, commandType);
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
            return await BaseDal.Execute(sql, param, transaction, commandTimeout, commandType);
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
            return await ExecuteScalar(sql, param, transaction, commandTimeout, commandType);
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
            return  BaseDal.Execute(command, paras);
        }
    }
}
