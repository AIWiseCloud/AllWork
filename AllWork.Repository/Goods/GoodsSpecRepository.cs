using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsSpecRepository:Base.BaseRepository<GoodsSpec>,IGoodsSpecRepository
    {
        public async Task<bool> SaveGoodsSpec(GoodsSpec goodsSpec)
        {
            var instance = await base.QueryFirst("Select * from GoodsSpec Where ID = @ID", goodsSpec);
            string sql;
            if (instance == null)
            {
                sql = @"Insert GoodsSpec (ID,GoodsId,SpecName,SaleUnit,UnitConverter,BaseUnitPrice,Price,DiscountPrice,Findex,Creator, SpecDes1, SpecDes2, SpecDes3)
values(@ID,@GoodsId,@SpecName,@SaleUnit,@UnitConverter,@BaseUnitPrice,@Price,@DiscountPrice,@Findex,@Creator, @SpecDes1, @SpecDes2, @SpecDes3)";
            }
            else
            {
                sql = @"Update GoodsSpec set GoodsId = @GoodsId,SpecName = @SpecName,SaleUnit = @SaleUnit,UnitConverter = @UnitConverter,BaseUnitPrice = @BaseUnitPrice,Price = @Price,
DiscountPrice = @DiscountPrice,Findex = @Findex,Creator = @Creator , SpecDes1 = @SpecDes1, SpecDes2 = @SpecDes2, SpecDes3 = @SpecDes3
Where ID = @ID";
            }
            var res = await base.Execute(sql, goodsSpec);
            return res > 0;
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

        public async Task<bool> ExistInventory(string specId)
        {
            var res = await base.ExecuteScalar<int>("Select Count(*) from Inventory Where SpecId = @SpecId", new { SpecId = specId });
            return res > 0;
        }
    }
}
