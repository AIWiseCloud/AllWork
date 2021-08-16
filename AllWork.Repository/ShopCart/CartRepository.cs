using AllWork.IRepository.ShopCart;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.ShopCart;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.ShopCart
{
    public class CartRepository : Base.BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task<OperResult> SaveCart(Cart cart)
        {
            var instance = await base.QueryFirst("Select * from Cart Where UnionId = @UnionId and GoodsId = @GoodsId and ColorId = @ColorId and SpecId = @SpecId", cart);
            var sql = string.Empty;
            if (instance == null)
            {
                //若不存在，则产生新的关键字段
                cart.ID = Guid.NewGuid().ToString();
                sql = "Insert Cart (ID,UnionId,GoodsId,ColorId,SpecId,Quantity,Selected)values(@ID,@UnionId,@GoodsId,@ColorId,@SpecId,@Quantity,1)";
            }
            else
            {
                //若存在，则合并数量
                sql = "Update Cart set Quantity = Quantity + @Quantity, Selected = 1 Where UnionId = @UnionId and GoodsId = @GoodsId and ColorId = @ColorId and SpecId = @SpecId";
            }
            var res = await base.Execute(sql, cart);
            return new OperResult { Status = res > 0, IdentityKey = cart.ID };
        }

        public async Task<IEnumerable<CartEx>> GetCarts(string unionId)
        {
            var sql = @"Select a.*,
'' as id1, b.*,
'' as id2, c.*,
'' as id3, d.*,
'' as id4, c2.*,
'' as id5, d2.*,
'' as id6, d3.*
from Cart a
left join GoodsInfo b on a.GoodsId = b.GoodsId
left join GoodsColor c on a.GoodsId = c.GoodsId and a.ColorId = c.ColorId
left join ColorInfo c2 on c2.ColorId = c.ColorId
left join GoodsSpec d on d.GoodsId = a.GoodsId and a.SpecId = d.SpecId
left join SpecInfo d2 on d2.SpecId = d.SpecId
left join Inventory d3 on d3.GoodsId = a.GoodsId and d3.ColorId = a.ColorId and d3.SpecId = a.SpecId
Where UnionId = @UnionId";
            var res = await base.QueryAsync<CartEx, GoodsInfo, GoodsColor, GoodsSpec, ColorInfo, SpecInfo, Inventory>(sql, (cart, gi, ci, si,ci2,si2,kc) =>
            {
                cart.GoodsInfo = gi;
                cart.GoodsColor = ci;
                ci.ColorInfo = ci2;
                cart.GoodsSpec = si;
                cart.GoodsSpec.Spec = si2;
                cart.Inventory = kc;
                return cart;
            }, new { unionId = unionId }, "id1,id2,id3,id4,id5,id6");
            return res;
        }

        public async Task<bool> EditCartQuantity(string id, int quantity)
        {
            var sql = $"Update Cart set Quantity = {quantity} Where id = '{id}' ";
            return await base.Execute(sql) > 0;
        }

        public async Task<bool> ChangeCartItemSelected(string id, int selected)
        {
            var sql = $"Update Cart set Selected = {selected} Where id = '{id}' ";
            return await base.Execute(sql) > 0;
        }

        public async Task<bool> DeleteCartItems(IList<string> cartIdList)
        {
            var idsb = new StringBuilder();
            foreach (var id in cartIdList)
            {
                idsb.AppendFormat("{0} '{1}'", (idsb.Length > 0 ? "," : string.Empty), id);
            }
            var sql = $"Delete from Cart Where ID in ({idsb})";
            var res = await base.Execute(sql)>0;
            return res;
        }
    }
}
