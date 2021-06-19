using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.IRepository.Base
{
    public interface IBaseRepository<TEntity> where TEntity:class,new()
    {
        Task<TEntity> QueryFirst(string sql, object param = null, IDbTransaction dbTransaction = null, int? commandTimeOut = null, CommandType? commandType = null);

        Task<IEnumerable<TEntity>> QueryList(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
    }
}
