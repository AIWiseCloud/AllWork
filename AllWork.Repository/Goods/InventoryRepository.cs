using AllWork.IRepository.Goods;
using AllWork.Model;
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
              }, new {  inventoryParams.CategoryId, inventoryParams.PageModel.Skip, inventoryParams.PageModel.PageSize }, "id1,id2,id3,id4");
            return res;

        }

        /// <summary>
        /// 比对商品需求数量与可用库存数量
        /// </summary>
        /// <param name="requireItems"></param>
        /// <returns></returns>
        public async Task<OperResult> ComparisonActiveQuantity(List<RequireItem> requireItems)
        {
            //将需求项拼成一个union连接的sql语句
            var sb = new StringBuilder();
            foreach (var item in requireItems)
            {
                sb.AppendFormat("{0} Select '{1}' as GoodsId,'{2}' as ColorId,'{3}' as SpecId, {4} as Quantity ",
                     sb.Length > 0 ? " union " : string.Empty, item.GoodsId, item.ColorId, item.SpecId, item.Quantity);
            }
            //完整sql
            var sql = string.Format(@"select t1.*, g.GoodsName , c.ColorName ,s.SpecName 
from(
select GoodsId, ColorId, SpecId ,sum(quantity) as Quantity
from(
{0}
)t group by GoodsId, ColorId, SpecId
)t1 left join Inventory t2
on t1.GoodsId = t2.GoodsId  and t1.ColorId = t2.ColorId  and t1.SpecId = t2.SpecId 
left join GoodsInfo g on g.GoodsId = t1.GoodsId
left join colorinfo c on c.ColorId = t1.ColorId
left join SpecInfo s on s.SpecId  = t1.SpecId
where t1.Quantity - ifnull(t2.ActiveQuantity ,0) > 0 ", sb);
            var res = await base.QueryList<RequireItemExt>(sql);
            var msg = new StringBuilder("以下项目可用库存不足：");
            foreach (var item in res)
            {
                msg.Append(item.ToString() + ",");
            }
            return new OperResult { Status = res.Count == 0, ErrorMsg = res.Count > 0 ? msg.ToString() : "ok" };
        }
        //获取sku商品可用库存
        public async Task<decimal> GetSKUActiveQuantity(string goodsId, string colorId, string specId)
        {
            var sql = "Select ActiveQuantity from Inventory Where GoodsId = @GoodsId and ColorId = @ColorId and SpecId = @SpecId";
            var res = await base.ExecuteScalar<decimal>(sql, new { GoodsId = goodsId, ColorId = colorId, SpecId = specId });
            return res;
        }
    }
}
