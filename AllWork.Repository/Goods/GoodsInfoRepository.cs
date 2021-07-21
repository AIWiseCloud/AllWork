using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsInfoRepository:Base.BaseRepository<GoodsInfo>,IGoodsInfoRepository
    {
        private object _dal;

        public GoodsInfoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo)
        {
            var instance = await base.QueryFirst("Select * from GoodsInfo Where GoodsId = @GoodsId", new { GoodsId = goodsInfo.GoodsId });
            if (instance == null)
            {
                var insertSql = "Insert GoodsInfo (GoodsId,CategoryId,ProdNumber,GoodsName,GoodsDesc,SalesTimes,IsRecommend,IsNew,IsUnder,Creator)values(@GoodsId,@CategoryId,@ProdNumber,@GoodsName,@GoodsDesc,@SalesTimes,@IsRecommend,@IsNew,@IsUnder,@Creator)";
                return await base.Execute(insertSql, goodsInfo) > 0;
            }
            else
            {
                var updateSql = "Update GoodsInfo set CategoryId = @CategoryId,ProdNumber = @ProdNumber,GoodsName = @GoodsName,GoodsDesc = @GoodsDesc,SalesTimes = @SalesTimes,IsRecommend = @IsRecommend,IsNew = @IsNew,IsUnder = @IsUnder,Creator = @Creator Where GoodsId = @GoodsId";
                return await base.Execute(updateSql, goodsInfo) > 0;
            }
        }

        public async Task<GoodsInfo> GetGoodsInfo(string goodsId)
        {
            var sql = new StringBuilder("Select * from GoodsInfo Where GoodsId = @GoodsId;");
            sql.Append("Select * from GoodsColor Where GoodsId = @GoodsId order by FDndex; ");
            sql.Append("Select * from GoodsSpec Where GoodsId = @GoodsId order by FIndex");

            var res = await base.QueryMultiple<GoodsInfo, GoodsColor, GoodsSpec>(sql.ToString());
            var goodsInfo = res.Item1[0];
            goodsInfo.GoodsColors = res.Item2;
            goodsInfo.GoodsSpecs = res.Item3;
            return goodsInfo;
        }

        /// <summary>
        /// 删除商品信息（包括颜色与规格设置）
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteGoodsInfo(string goodsId)
        {
            var sql = new StringBuilder("Delete from GoodsColor Where GoodsId = @GoodsId;");
            sql.Append("Delete from GoodsSpec Where GoodsId = @GoodsId; ");
            sql.Append("Delete from GoodsInfo Where GoodsId = @GoodsId");
            return await base.Execute(sql.ToString(), new { GoodsId = goodsId }) > 0;

        }
    }
}
