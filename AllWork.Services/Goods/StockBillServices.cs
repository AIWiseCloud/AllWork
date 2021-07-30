using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Goods
{
    public class StockBillServices:Base.BaseServices<StockBill>,IStockBillServices
    {
        readonly IStockBillRepository _dal;
        public StockBillServices(IStockBillRepository stockBillRepository)
        {
            _dal = stockBillRepository;
        }
        public async Task<OperResult> SaveStockBill(StockBill stockBill)
        {
            var res = await _dal.SaveStockBill(stockBill);
            return res;
        }

        public async Task<StockBill> GetStockBill(string billId)
        {
            var res = await _dal.GetStockBill(billId);
            return res;
        }

        public async Task<Tuple<IEnumerable<StockBill>, int>> SearchStockBill(StockBillParams stockBillParams)
        {
            var res = await _dal.SearchStockBill(stockBillParams);
            return res;
        }

        public async Task<bool> DeleteStockBillRow(string id)
        {
            var res = await _dal.DeleteStockBillRow(id);
            return res;
        }

        public async Task<OperResult> DeleteStockBill(string billId)
        {
            var res = await _dal.DeleteStockBill(billId);
            return res;
        }
    }
}
