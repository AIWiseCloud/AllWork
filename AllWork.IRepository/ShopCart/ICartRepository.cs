using AllWork.Model;
using AllWork.Model.ShopCart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.ShopCart
{
    public interface ICartRepository:Base.IBaseRepository<Cart>
    {
        Task<OperResult> SaveCart(Cart cart);

        Task<bool> EditCartQuantity(string id, int quantity);

        Task<IEnumerable<CartEx>> GetCarts(string unionId);

        Task<bool> ChangeCartItemSelected(string id, int selected);

        Task<bool> DeleteCartItems(IList<string> cartIdList);
    }
}
