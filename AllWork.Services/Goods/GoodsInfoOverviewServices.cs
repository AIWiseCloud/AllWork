using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Services.Goods
{
    public class GoodsInfoOverviewServices:Base.BaseServices<GoodsInfoOverview>,IGoodsInfoOverviewServices
    {
        readonly IGoodsInfoOverviewRepository _dal;
        public GoodsInfoOverviewServices(IGoodsInfoOverviewRepository goodsInfoOverviewRepository)
        {
            _dal = goodsInfoOverviewRepository;
        }

        public async Task<IEnumerable<GoodsInfoOverview>> GetGoodsInfoOverviews(string categoryId)
        {
            var res = await _dal.GetGoodsInfoOverviews(categoryId);
            return res;
        }
    }
}
