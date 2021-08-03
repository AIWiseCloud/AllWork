﻿using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Goods
{
    public class GoodsInfoServices:Base.BaseServices<GoodsInfo>,IGoodsInfoServices
    {
        readonly IGoodsInfoRepository _dal;
        public GoodsInfoServices(IGoodsInfoRepository goodsInfoRepository)
        {
            _dal = goodsInfoRepository;
        }

        public async Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo)
        {
            var res = await _dal.SaveGoodsInfo(goodsInfo);
            return res;
        }

        public async Task<GoodsInfo> GetGoodsInfo(string goodsId)
        {
            var res = await _dal.GetGoodsInfo(goodsId);
            return res;
        }

        /// <summary>
        /// 删除商品信息（包括颜色与规格设置）
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteGoodsInfo(string goodsId)
        {
            var res = await _dal.DeleteGoodsInfo(goodsId);
            return res;
        }

        public async Task<Tuple<IEnumerable<GoodsInfo>, int>> QueryGoods(GoodsQueryParams goodsQueryParams)
        {
            var res = await _dal.QueryGoods(goodsQueryParams);
            return res;
        }
    }
}
