using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class SalesmanServices:Base.BaseServices<Salesman>,ISalesmanServices
    {
        readonly ISalesmanRepository _dal;
        public SalesmanServices(ISalesmanRepository salesmanRepository)
        {
            _dal = salesmanRepository;
        }

        public async Task<OperResult> ImportSalesma(List<Salesman> salesmen)
        {
            var res = await _dal.ImportSalesma(salesmen);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }

        public async Task<IEnumerable<Salesman>> GetSalesmen(string keywords = "", bool ignoreStop = false)
        {
            var res = await _dal.GetSalesmen(keywords, ignoreStop);
            return res;
        }
    }
}
