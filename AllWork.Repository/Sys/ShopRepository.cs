using AllWork.IRepository.Sys;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Sys
{
    public class ShopRepository:Base.BaseRepository<Shop>,IShopRepository
    {
        public ShopRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
