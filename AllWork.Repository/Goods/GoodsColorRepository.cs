using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsColorRepository : Base.BaseRepository<GoodsColor>, IGoodsColorRepository
    {
        public GoodsColorRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<bool> SaveGoodsColor(GoodsColor goodsColor)
        {
            var instance = await base.QueryFirst("Select * from GoodsColor Where ID = @ID", new { ID = goodsColor.ID });
            if (instance == null)
            {
                var insertSql = "Insert GoodsColor (ID,GoodsId,ColorId,ImgFront,ImgBack,ImgRight,ImgLeft,Findex,Creator)values(@ID,@GoodsId,@ColorId,@ImgFront,@ImgBack,@ImgRight,@ImgLeft,@Findex,@Creator)";
                return await base.Execute(insertSql, goodsColor) > 0;
            }
            else
            {
                var updateSql = "Update GoodsColor set ColorId = @ColorId,ImgFront = @ImgFront,ImgBack = @ImgBack,ImgRight = @ImgRight,ImgLeft = @ImgLeft,Findex = @Findex,Creator = @Creator Where ID = @ID";
                return await base.Execute(updateSql, goodsColor) > 0;
            }
        }

        public async Task<GoodsColor> GetGoodsColor(string id)
        {
            var res = await base.QueryFirst("Select * from GoodsColor Where ID = @ID", new { ID = id });
            return res;
        }

        public async Task<bool> DeleteGoodsColor(string id)
        {
            var res = await base.Execute("Delete from GoodsColor Where ID = @ID", new { ID = id }) > 0;
            return res;
        }
    }
}
