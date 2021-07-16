using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Goods
{
    public class GoodsCategoryRepository:Base.BaseRepository<GoodsCategory>,IGoodsCategoryRepository
    {
        public GoodsCategoryRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
