using AllWork.IServices.Address;
using AllWork.Model.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 收货地址
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        readonly IReceiveAddressServices _receiveAddressServices;
        public AddressController(IReceiveAddressServices receiveAddressServices)
        {
            _receiveAddressServices = receiveAddressServices;
        }

        /// <summary>
        /// 保存收货地址
        /// </summary>
        /// <param name="receiveAddress"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveReceiveAddress(ReceiveAddress receiveAddress)
        {
            if (string.IsNullOrEmpty(receiveAddress.AddrId))
            {
                receiveAddress.AddrId = System.Guid.NewGuid().ToString();
            }
            var res = await _receiveAddressServices.SaveReceiveAddress(receiveAddress);
            return Ok(res);
        }

        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="unionId"></param>
        /// <param name="addrId"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> SetDefaultAddress(string unionId, string addrId)
        {
            var res = await _receiveAddressServices.SetDefaultAddress(unionId, addrId);
            return Ok(res);
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="unionId"></param>
        /// <param name="addrId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteReceiveAddress(string unionId, string addrId)
        {
            var res = await _receiveAddressServices.DeleteReceiveAddress(unionId, addrId);
            return Ok(res);
        }

        /// <summary>
        /// 获取用户收货地址列表
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetReceiveAddresses(string unionId)
        {
            var res = await _receiveAddressServices.GetReceiveAddresses(unionId);
            return Ok(res);
        }

        /// <summary>
        /// 获取行政区域列表
        /// </summary>
        /// <param name="level">行政层级(1省级,2市级,3区县级,4乡镇级)</param>
        /// <param name="areaId">行政区域ID (层级为1时不用指定)</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdminAreas(int level, string areaId)
        {
            var res = await _receiveAddressServices.GetAreas(level, areaId);
            return Ok(res);
        }
    }
}
