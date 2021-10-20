using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface ISalesmanRepository:Base.IBaseRepository<Salesman>
    {
        Task<Tuple<bool,string>> ImportSalesma(List<Salesman> salesmen);

        Task<IEnumerable<Salesman>> GetSalesmen(string keywords = "", bool ignoreStop=false);
    }
}
