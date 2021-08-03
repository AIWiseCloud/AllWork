using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class InventoryRepository : Base.BaseRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<Inventory>> GetInventories(string goodsId)
        {
            var sql = "Select * from Inventory Where GoodsId = @GoodsId";
            var res = await base.QueryList(sql, new { GoodsId = goodsId });
            return res;
        }

        public async Task<Tuple<IEnumerable<Inventory>, int>> SearchInventories(InventoryParams inventoryParams)
        {
            //sql公共部分
            var sqlpub = new StringBuilder(@"from Inventory a left join GoodsInfo b
on a.GoodsId = b.GoodsId
left join ColorInfo c
on a.ColorId = c.ColorId
left join SpecInfo d
on a.SpecId = d.SpecId
left join GoodsCategory e
on e.CategoryId = b.CategoryId Where (1=1) ");
            if (!string.IsNullOrEmpty(inventoryParams.CategoryId))
            {
                sqlpub.Append(" and b.CategoryId = @CategoryId ");
            }
            if (!string.IsNullOrEmpty(inventoryParams.GoodsName))
            {
                sqlpub.AppendFormat(" and b.GoodsName like '%{0}%' ", inventoryParams.GoodsName);
            }
            if (inventoryParams.GoodsState != 0)
            {
                sqlpub.AppendFormat(" and IsUnder = {0} ", inventoryParams.GoodsState == 1 ? 0 : 1);
            }
            //sql排序部分
            string sqlOrder = string.Empty;
            if (!string.IsNullOrEmpty(inventoryParams.PageModel.OrderField))
            {
                sqlOrder = string.Format(" Order by {0} {1} ", inventoryParams.PageModel.OrderField, inventoryParams.PageModel.OrderWay);
            }
            //sql求记录数部分
            var sql1 = "Select count(a.SkuId) as TotalCount " + sqlpub.ToString();
            //sql分页取数部分
            var sql2 = new StringBuilder(@" Select a.*,
'' as id1, b.*,
'' as id2, c.*,
'' as id3, d.*,
'' as id4, e.* ");
            sql2.Append(sqlpub);
            sql2.Append(sqlOrder);
            sql2.Append(" limit @Skip, @PageSize ");
            //sql语句合并
            var sqlfull = sql1 + ";" + sql2;
            //调用分页接口
            var res = await base.QueryPagination<Inventory, GoodsInfo, ColorInfo, SpecInfo, GoodsCategory>(sqlfull, (iv, gi, ci, si, gc) =>
              {
                  iv.GoodsInfo = gi;
                  iv.GoodsCategory = gc;
                  iv.ColorInfo = ci;
                  iv.SpecInfo = si;
                  return iv;
              }, new { CategoryId = inventoryParams.CategoryId, Skip = inventoryParams.PageModel.Skip, PageSize = inventoryParams.PageModel.PageSize }, "id1,id2,id3,id4");
            return res;

        }

    }
}
