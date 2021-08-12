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
    public class StockBillServices : Base.BaseServices<StockBill>, IStockBillServices
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

        public async Task<StockBillExt> GetStockBill(string billId)
        {
            var res = await _dal.GetStockBill(billId);
            return res;
        }

        public async Task<Tuple<IEnumerable<StockBillExt>, int>> SearchStockBill(StockBillParams stockBillParams)
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

        public async Task<OperResult> AuditStockBill(string billId, int isAdit)
        {
            var res = await _dal.AuditStockBill(billId, isAdit);
            return res;
        }

        //检查订单是否制作过不同交易单号的出库单
        public async Task<bool> IsCreateOthBill(string billId, long? orderId)
        {
            var res = await _dal.IsCreateOthBill(billId, orderId);
            return res > 0;
        }

        //检查出库数量是否会导致负结存(保存出库单、审核出库单、反审核入库单时均可用此检查)
        public async Task<OperResult> CheckNegativeBalance(StockBillExt stockBill)
        {
            var res = await _dal.CheckNegativeBalance(stockBill);
            return res;
        }

        //根据单据的业务类型判断出入库
        public bool IsOutStock(string transTypeId)
        {
            return transTypeId == "transtype_othout" || transTypeId == "transtype_sale";
        }

        //获取商品实际库存信息
        public async Task<decimal> GetInventoryDetail(string goodsId, string colorId, string specId, string stockNumber)
        {
            var res = await _dal.GetInventoryDetail(goodsId, colorId, specId, stockNumber);
            return res;
        }

        //获取待发货订单列表
        public async Task<dynamic> GetToBeShipped(string orderId)
        {
            var res = await _dal.GetToBeShipped(orderId);
            return res;
        }
    }
}
