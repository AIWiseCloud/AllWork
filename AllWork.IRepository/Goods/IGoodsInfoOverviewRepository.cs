using AllWork.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IGoodsInfoOverviewRepository:Base.IBaseRepository<GoodsInfoOverview>
    {
        Task<IEnumerable<GoodsInfoOverview>> GetGoodsInfoOverviews(string categoryId);
    }
}
