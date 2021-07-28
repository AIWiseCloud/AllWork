using AllWork.IRepository.Goods;
using AllWork.Model.ViewModel;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsInfoOverviewRepository:Base.BaseRepository<GoodsInfoOverview>,IGoodsInfoOverviewRepository
    {
        public GoodsInfoOverviewRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<GoodsInfoOverview>> GetGoodsInfoOverviews(string categoryId)
        {
            var sql = "Select * from GoodsInfoOverview Where CategoryId = @CategoryId";
            return await base.QueryList(sql, new { CategoryId = categoryId });
        }
    }
}
