using AllWork.IRepository.Sys;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class SalesmanRepository:Base.BaseRepository<Salesman>,ISalesmanRepository
    {
        public async Task<Tuple<bool, string>> ImportSalesma(List<Salesman> salesmen)
        {
            var sql1 = "Delete from Salesman";
            var sql2 = "Insert Salesman(OpenUserId, Name,Mobile, ProfileImageUrl, IsStop)values(@OpenUserId, @Name,@Mobile, @ProfileImageUrl, @IsStop)";
            var tranitems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql1, null)
            };
            foreach (var item in salesmen)
            {
                tranitems.Add(new Tuple<string, object>(sql2, item));
            }
            var res = await base.ExecuteTransaction(tranitems);
            return res;
        }

        public async Task<IEnumerable<Salesman>> GetSalesmen(string keywords = "", bool ignoreStop = false)
        {
            var sql = new System.Text.StringBuilder("Select * from Salesman Where 1=1");
            if (ignoreStop)
            {
                sql.Append(" and IsStop = 0 ");
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                sql.AppendFormat(" and name like '%{0}%' or Mobile = @Mobile ", keywords);
            }
           
            var res = await base.QueryList(sql.ToString(), new { Mobile = keywords });
            return res;
        }
    }
}
