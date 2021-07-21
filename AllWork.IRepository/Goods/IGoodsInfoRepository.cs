using AllWork.Model.Goods;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IGoodsInfoRepository:Base.IBaseRepository<GoodsInfo>
    {
        Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo);

        Task<GoodsInfo> GetGoodsInfo(string goodsId);

        Task<bool> DeleteGoodsInfo(string goodsId);
    }
}
