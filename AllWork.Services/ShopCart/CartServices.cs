using AllWork.IRepository.ShopCart;
using AllWork.IServices.ShopCart;
using AllWork.Model;
using AllWork.Model.ShopCart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.ShopCart
{
    public class CartServices:Base.BaseServices<Cart>,ICartServices
    {
        readonly ICartRepository _dal;
        public CartServices(ICartRepository cartRepository)
        {
            _dal = cartRepository;
        }

        public async Task<OperResult> SaveCart(Cart cart)
        {
            var res = await _dal.SaveCart(cart);
            return res;
        }

        public async Task<IEnumerable<CartEx>> GetCarts(string unionId)
        {
            var res = await _dal.GetCarts(unionId);
            return res;
        }

        public async Task<bool> EditCartQuantity(string id, int quantity)
        {
            var res = await _dal.EditCartQuantity(id, quantity);
            return res;
        }

        public async Task<bool> ChangeCartItemSelected(string id, int selected)
        {
            var res = await _dal.ChangeCartItemSelected(id, selected);
            return res;
        }

        public async Task<bool> DeleteCartItems(IList<string> cartIdList)
        {
            var res = await _dal.DeleteCartItems(cartIdList);
            return res;
        }
    }
}
