using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class ShopServices : Base.BaseServices<Shop>, IShopServices
    {
        readonly IShopRepository _dal;

        public ShopServices(IShopRepository shopRepository)
        {
            _dal = shopRepository;
        }

        public async Task<OperResult> SaveShop(Shop shop)
        {
            OperResult operResult = new OperResult();
            try
            {
                var instance = await _dal.QueryFirst("Select * from Shop Where ShopId = @ShopId", new { ShopId = shop.ShopId });
                if (instance == null)
                {
                    var insertSql = "Insert Shop (ShopId,ShopName,ImgUrl,Contacter,PhoneNumber,ListBySpuShow,Introduction,Announcement)values(@ShopId,@ShopName,@ImgUrl,@Contacter,@PhoneNumber,@ListBySpuShow,@Introduction,@Announcement)";
                    operResult.Status = await _dal.Execute(insertSql, shop) > 0;
                }
                else
                {
                    var updateSql = "Update Shop set ShopId = @ShopId,ShopName = @ShopName,ImgUrl = @ImgUrl,Contacter = @Contacter,PhoneNumber = @PhoneNumber,ListBySpuShow = @ListBySpuShow,Introduction = @Introduction,Announcement = @Announcement Where ShopId = @ShopId";
                    operResult.Status = await _dal.Execute(updateSql, shop) > 0;
                }
            }
            catch (Exception ex)
            {
                operResult.ErrorMsg = ex.Message;
            }
            return operResult; ;
        }

        public async Task<IEnumerable<Shop>> GetShops()
        {
            var sql = "Select * from Shop";
            var res = await _dal.QueryList(sql);
            return res;
        }

        public async Task<Shop> GetShop(string shopId)
        {
            var sql = "Select * from Shop Where ShopId = @ShopId";
            var res = await _dal.QueryFirst(sql, new { ShopId = shopId });
            return res;
        }

        public async Task<bool> DeleteShop(string shopId)
        {
            var sql = $"Delete * from Shop Where ShopId = @ShopId";
            var res = await _dal.Execute(sql, new { ShopId = shopId}) > 0;
            return res;
        }
    }
}
