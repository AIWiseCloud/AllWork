﻿using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IStockBillServices:Base.IBaseServices<StockBill>
    {
        Task<OperResult> SaveStockBill(StockBill stockBill);

        Task<StockBillExt> GetStockBill(string billId);

        Task<Tuple<IEnumerable<StockBillExt>, int>> SearchStockBill(StockBillParams stockBillParams);

        Task<bool> DeleteStockBillRow(string id);

        Task<OperResult> DeleteStockBill(string billId);

        Task<OperResult> AuditStockBill(string billId, int isAdit);
    }
}
