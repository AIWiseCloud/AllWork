﻿using AllWork.Model;
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

        Task<StockBillExt> GetStockBill(string billId);

        //Task<Tuple<IEnumerable<StockBillExt>,int>> SearchStockBill(StockBillParams stockBillParams);

        Task<Tuple<IEnumerable<StockBillExt>, int>> SearchStockBill(StockBillParams stockBillParams);

        Task<bool> DeleteStockBillRow(string id);

        Task<OperResult> DeleteStockBill(string billId);

        Task<OperResult> AuditStockBill(string billId, int isAdit);

        //检查订单是否制作过不同交易单号的出库单
        Task<int> IsCreateOthBill(string billId, long orderId);

        //检查出库数量是否会导致负结存(保存出库单、审核出库单、反审核入库单时均可用此检查)
        Task<OperResult> CheckNegativeBalance(StockBill stockBill);

        //获取商品实际库存信息
        Task<decimal> GetInventoryDetail(string goodsId, string colorId, string specId, string stockNumber);

        //获取待发货订单列表
        Task<dynamic> GetToBeShipped(string orderId);
    }
}
