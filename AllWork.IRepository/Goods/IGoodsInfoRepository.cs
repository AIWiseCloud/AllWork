﻿using AllWork.Model;
using AllWork.Model.Goods;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IGoodsInfoRepository:Base.IBaseRepository<GoodsInfo>
    {
        Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo);

        Task<GoodsInfo> GetGoodsInfo(string goodsId);

        Task<bool> DeleteGoodsInfo(string goodsId);

        Task<Tuple<IEnumerable<GoodsInfo>, int>> SearchGoods(string keywords, PageModel pageModel);

        Task<Tuple<IEnumerable<GoodsInfo>, int>> GetGoodsInfos(string categoryId, PageModel pageModel);
    }
}
