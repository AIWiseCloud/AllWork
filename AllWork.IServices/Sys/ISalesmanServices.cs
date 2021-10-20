using AllWork.Model;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface ISalesmanServices
    {
        Task<OperResult> ImportSalesma(List<Salesman> salesmen);

        Task<IEnumerable<Salesman>> GetSalesmen(string keywords = "", bool ignoreStop = false);
    }
}
