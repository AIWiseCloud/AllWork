using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsSpecRepository:Base.BaseRepository<GoodsSpec>,IGoodsSpecRepository
    {
        public GoodsSpecRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<bool> SaveGoodsSpec(GoodsSpec goodsSpec)
        {
            var instance = await base.QueryFirst("Select * from GoodsSpec Where ID = @ID", goodsSpec);
            if (instance == null)
            {
                var insertSql = "Insert GoodsSpec (ID,GoodsId,SpecId,Price,DiscountPrice,Findex,Creator)values(@ID,@GoodsId,@SpecId,@Price,@DiscountPrice,@Findex,@Creator)";
                return await base.Execute(insertSql, goodsSpec) > 0;
            }
            else
            {
                var updateSql = "Update GoodsSpec set SpecId = @SpecId,Price = @Price,DiscountPrice = @DiscountPrice,Findex = @Findex,Creator = @Creator Where ID = @ID";
                return await base.Execute(updateSql, goodsSpec) > 0;
            }
        }

        public async Task<GoodsSpec> GetGoodsSepc(string id)
        {
            var sql = "Select * from GoodsSpec Where ID = @ID";
            return await base.QueryFirst(sql, new { ID = id });
        }

        public async Task<IEnumerable<GoodsSpec>> GetGoodsSpecs(string goodsId)
        {
            var res = await base.QueryList("Select * from GoodsSpec Where GoodsId = @GoodsId", new { GoodsId = goodsId });
            return res;
        }

        public async Task<bool> DeleteGoodsSpec(string id)
        {
            var sql = "Delete from GoodsSpec Where ID = @ID";
            return await base.Execute(sql, new { ID = id })>0;
        }
    }
}
