using AllWork.IRepository.DataCenter;
using AllWork.Model.DataCenter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.DataCenter
{
    public class MallDataRepository:Base.BaseRepository<MallData>,IMallDataRepository
    {
        public async Task<IEnumerable<MallData>> GetMallData()
        {
            var sql = "Select * from MallData";
            var res = await base.QueryList(sql);
            return res;
        }
    }
}
