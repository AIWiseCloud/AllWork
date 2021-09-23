using AllWork.IRepository.Sys;
using AllWork.Model;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class ShopRepository:Base.BaseRepository<Shop>,IShopRepository
    {
        public async Task<OperResult> SaveShop(Shop shop)
        {
            var instance = await base.QueryFirst("Select * from Shop Where ShopId = @ShopId", new { shop.ShopId });
            string sql;
            if (instance == null)
            {
                sql = @"Insert Shop (ShopId,ShopName,ImgUrl,Contacter,PhoneNumber,ListBySpuShow,Introduction,Announcement,AccountName,BankCardNo,DepositBank,CnapsCode)values
(@ShopId,@ShopName,@ImgUrl,@Contacter,@PhoneNumber,@ListBySpuShow,@Introduction,@Announcement,@AccountName,@BankCardNo,@DepositBank,@CnapsCode)";
            }
            else
            {
                sql = @"Update Shop set ShopId = @ShopId,ShopName = @ShopName,ImgUrl = @ImgUrl,Contacter = @Contacter,PhoneNumber = @PhoneNumber,ListBySpuShow = @ListBySpuShow,
AccountName=@AccountName,BankCardNo=@BankCardNo,DepositBank=@DepositBank,CnapsCode=@CnapsCode,
Introduction = @Introduction,Announcement = @Announcement Where ShopId = @ShopId";
            }
            var res = await base.Execute(sql, shop);
            return new OperResult { Status = res > 0 };
        }

        public async Task<IEnumerable<Shop>> GetShops()
        {
            var sql = "Select * from Shop";
            var res = await base.QueryList(sql);
            return res;
        }

        public async Task<Shop> GetShop(string shopId)
        {
            var sql = "Select * from Shop Where ShopId = @ShopId";
            var res = await base.QueryFirst(sql, new { ShopId = shopId });
            return res;
        }

        public async Task<bool> DeleteShop(string shopId)
        {
            var sql = $"Delete  from Shop Where ShopId = @ShopId";
            var res = await base.Execute(sql, new { ShopId = shopId }) > 0;
            return res;
        }
    }
}
