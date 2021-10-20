using AllWork.IRepository.DataCenter;
using AllWork.Model.DataCenter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.DataCenter
{
    public class AppVisitsRepository : Base.BaseRepository<AppVisits>, IAppVisitsRepository
    {
        public async Task<int> WriteAppVisits()
        {
            var res = await base.Execute("call WriteVisits()");
            return res;
        }

        //默认0表示总访问量，1表示今天访问量
        public async Task<int> GetAppVisits(int rangType = 0)
        {
            var sql = new StringBuilder("Select sum(Visits) as Visits from AppVisits Where 1 = 1");
            if (rangType == 1)
            {
                sql.Append(" and Date(FDate) = curdate() ");
            }
            var res = await base.ExecuteScalar<int>(sql.ToString());
            return res;
        }
    }
}
