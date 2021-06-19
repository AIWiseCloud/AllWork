using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.IServices.Base
{
    public interface IBaseServices<TEntity> where TEntity : class
    {
        Task<TEntity> QueryFirst(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<IEnumerable<TEntity>> QueryList(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
    }
}
