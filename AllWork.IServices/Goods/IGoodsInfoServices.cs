﻿using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IGoodsInfoServices
    {
        Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo);

        Task<GoodsInfo> GetGoodsInfo(string goodsId);

        Task<bool> DeleteGoodsInfo(string goodsId);

        Task<Tuple<IEnumerable<GoodsInfo>, int>> QueryGoods(GoodsQueryParams goodsQueryParams);
    }
}
