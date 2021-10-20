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
    public class GoodsInfoServices : Base.BaseServices<GoodsInfo>, IGoodsInfoServices
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

        public async Task<bool> ExistSKU(string goodsId)
        {
            var res = await _dal.ExistSKU(goodsId);
            return res;
        }

        //发布（上架）或取消发布（下架）商品
        public async Task<OperResult> ReleaseGoods(bool isRelease, string goodsId)
        {
            var operResult = new OperResult { Status = false, IdentityKey = goodsId };
            if (isRelease)
            {
                var instance = await _dal.GetGoodsInfo(goodsId);
                if (instance == null)
                {
                    operResult.ErrorMsg = $"{goodsId}的商品信息不存在";
                    return operResult;
                }
                if (instance.BaseUnitPrice == 0)
                {
                    operResult.ErrorMsg = "基本单位单价不能为0";
                    return operResult;
                }
                if (instance.GoodsColors.Count == 0)
                {
                    operResult.ErrorMsg = "未设定“颜色及图片”";
                    return operResult;
                }
                //因盛天商品不用传图片，所以目前注销(roy 2021-9-17)
                //if (instance.GoodsColors.FindAll(x => String.IsNullOrEmpty(x.ImgFront)).Count > 0)
                //{
                //    operResult.ErrorMsg = "颜色项的正面图片必须设置";
                //    return operResult;
                //}
                if (instance.GoodsSpecs.Count == 0)
                {
                    operResult.ErrorMsg = "必须设定规格及价格信息";
                    return operResult;
                }
                foreach (var item in instance.GoodsSpecs)
                {
                    if (instance.UnitName != item.SaleUnit && item.UnitConverter == 1)
                    {
                        operResult.ErrorMsg = "销售单位与表头的基本单位不相同时，请设定单位转换系数（即1销售单位等于多少基本单位），此时转换系数不能为1";
                        return operResult;
                    }
                    if (instance.UnitName == item.SaleUnit && item.UnitConverter != 1)
                    {
                        operResult.ErrorMsg = "销售单位与表头的基本单位相同，但二者的转换系数不为1，请检查是否正确！";
                        return operResult;
                    }

                    if (item.DiscountPrice > item.Price)
                    {
                        operResult.ErrorMsg = "折扣价不能大于销售单价";
                        return operResult;
                    }
                    if (string.IsNullOrEmpty(instance.Mixture) && instance.Mixture.IndexOf(':') != -1 && (item.SpecName.IndexOf('甲') == -1 && item.SpecName.IndexOf('乙') == -1))
                    {
                        operResult.ErrorMsg = "若商品信息设定了配比，则需要在规格描述栏位注明甲组或乙组";
                        return operResult;
                    }
                }
            }
            var res = await _dal.ReleaseGoods(isRelease, goodsId);
            operResult.Status = res;
            return operResult;
        }

        /// <summary>
        /// 删除商品信息（包括颜色与规格设置）
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<OperResult> DeleteGoodsInfo(string goodsId)
        {
            var happenBS = await _dal.ExistSKU(goodsId);
            if (happenBS)
            {
                return new OperResult { Status = false, ErrorMsg = "此商品存在库存记录!" };
            }
            happenBS = await _dal.ExistOrders(goodsId);
            if (happenBS)
            {
                return new OperResult { Status = false, ErrorMsg = "此商品存已有订单记录!" };
            }
            var res = await _dal.DeleteGoodsInfo(goodsId);
            return new OperResult { Status = res };
        }

        public async Task<Tuple<IEnumerable<GoodsInfoExt>, int>> QueryGoods(GoodsQueryParams goodsQueryParams)
        {
            var res = await _dal.QueryGoods(goodsQueryParams);
            return res;
        }

        public async Task<IEnumerable<GoodsInfo>> GetGoodsList(string categoryId)
        {
            var res = await _dal.GetGoodsList(categoryId);
            return res;
        }

        public async Task<Tuple<List<QuoteExplain>, List<GoodsQuote>>> GetGoodsQuotes()
        {
            var res = await _dal.GetGoodsQuotes();
            return res;
        }

        public async Task<int> UpdateQuoteExplain(QuoteExplain quoteExplain)
        {
            var res = await _dal.UpdateQuoteExplain(quoteExplain);
            return res;
        }

        public async Task<IEnumerable<GoodsInfo>> GetAllGoodsInfo()
        {
            var res = await _dal.GetAllGoodsInfo();
            return res;
        }

        public async Task<OperResult> BatchUpdatePrice(List<GoodsInfo> goodsInfos)
        {
            var res = await _dal.BatchUpdatePrice(goodsInfos);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }
    }
}
