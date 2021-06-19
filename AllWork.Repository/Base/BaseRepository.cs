using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

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
            string connectionString = _ccnfiguration["ConnectionStrings:LocalConn"]; //从appsettings.json读取连接配置
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch(Exception ex)
            {
                throw new Exception("数据库连接错误:" + ex.Message);
            }
            return con;
        }

        public async Task<TEntity> QueryFirst(string sql, object param = null, IDbTransaction transaction = null,int? commandTmeout=null,CommandType? commandType=null)
        {
            using var con = GetOpenConn();
            return await con.QueryFirstAsync<TEntity>(sql, param, transaction, commandTmeout, commandType);
        }

        public async Task<IEnumerable<TEntity>> QueryList(string sql,object param=null, IDbTransaction transaction=null,bool buffered=true,int?commandTimeout=null, CommandType? commandType = null)
        {
            using var con = GetOpenConn();
            return await con.QueryAsync<TEntity>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
