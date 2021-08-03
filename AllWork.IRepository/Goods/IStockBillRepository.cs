using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IStockBillRepository:Base.IBaseRepository<StockBill>
    {
        Task<OperResult> SaveStockBill(StockBill stockBill);

        Task<StockBill> GetStockBill(string billId);

        Task<Tuple<IEnumerable<StockBill>,int>> SearchStockBill(StockBillParams stockBillParams);

        Task<bool> DeleteStockBillRow(string id);

        Task<OperResult> DeleteStockBill(string billId);

        Task<OperResult> AuditStockBill(string billId, int isAdit);
    }
}
