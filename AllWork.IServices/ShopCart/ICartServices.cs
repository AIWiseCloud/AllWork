using AllWork.Model;
using AllWork.Model.ShopCart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.ShopCart
{
    public interface ICartServices:Base.IBaseServices<Cart>
    {
        Task<OperResult> SaveCart(Cart cart);

        Task<IEnumerable<CartEx>> GetCarts(string unionId);

        Task<bool> EditCartQuantity(string id, int quantity);

        Task<bool> ChangeCartItemSelected(string id, int selected);

        Task<bool> DeleteCartItems(IList<string> cartIdList);
    }
}
