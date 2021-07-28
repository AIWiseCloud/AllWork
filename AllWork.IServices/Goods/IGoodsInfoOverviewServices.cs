using AllWork.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IGoodsInfoOverviewServices:Base.IBaseServices<GoodsInfoOverview>
    {
        Task<IEnumerable<GoodsInfoOverview>> GetGoodsInfoOverviews(string categoryId);
    }
}
