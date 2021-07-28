using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Goods
{
    public class InventoryRepository:Base.BaseRepository<Inventory>,IInventoryRepository
    {
        public InventoryRepository(IConfiguration configuration) : base(configuration)
        {

        }

     
    }
}
