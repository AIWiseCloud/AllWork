using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Services.Base
{
    public class BaseServices<TEntity> where TEntity:class,new()
    {
        public AllWork.IRepository.Base.IBaseRepository<TEntity> BaseDal;

        public async Task<TEntity> QueryFirst(string sql,object param=null,IDbTransaction transaction=null, int? commandTime=null,CommandType? commandType=null)
        {
            return await BaseDal.QueryFirst(sql, param,transaction, commandTime, commandType);
        }

        public async Task<IEnumerable<TEntity>> QueryList(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await BaseDal.QueryList(sql, param, transaction, buffered, commandTimeout, commandType);
        }
    }
}
