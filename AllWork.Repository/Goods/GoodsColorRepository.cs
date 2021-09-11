using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsColorRepository : Base.BaseRepository<GoodsColor>, IGoodsColorRepository
    {
        public async Task<bool> SaveGoodsColor(GoodsColor goodsColor)
        {
            var instance = await base.QueryFirst("Select * from GoodsColor Where ID = @ID", new { goodsColor.ID });
            if (instance == null)
            {
                var insertSql = "Insert GoodsColor (ID,GoodsId,ColorName,ImgFront,ImgBack,ImgRight,ImgLeft,Findex,Creator)values(@ID,@GoodsId,@ColorName,@ImgFront,@ImgBack,@ImgRight,@ImgLeft,@Findex,@Creator)";
                return await base.Execute(insertSql, goodsColor) > 0;
            }
            else
            {
                var updateSql = "Update GoodsColor set ColorName = @ColorName,ImgFront = @ImgFront,ImgBack = @ImgBack,ImgRight = @ImgRight,ImgLeft = @ImgLeft,Findex = @Findex,Creator = @Creator Where ID = @ID";
                return await base.Execute(updateSql, goodsColor) > 0;
            }
        }

        public async Task<GoodsColor> GetGoodsColor(string id)
        {
            var res = await base.QueryFirst("Select * from GoodsColor Where ID = @ID", new { ID = id });
            return res;
        }

        public async Task<IEnumerable<GoodsColor>> GetGoodsColors(string goodsId)
        {
            var res = await base.QueryList("Select * from GoodsColor Where GoodsId = @GoodsId", new { GoodsId = goodsId });
            return res;
        }

        public async Task<bool> DeleteGoodsColor(string id)
        {
            var res = await base.Execute("Delete from GoodsColor Where ID = @ID", new { ID = id }) > 0;
            return res;
        }

        public async Task<bool> ExistInventory(string colorId)
        {
            var res = await base.ExecuteScalar<int>("Select Count(*) from Inventory Where ColorId = @ColorId", new { ColorId = colorId });
            return res > 0;
        }
    }
}
