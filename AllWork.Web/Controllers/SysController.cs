using AllWork.IServices.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AllWork.Model.Sys;
using System.Threading.Tasks;
using System;
using AllWork.Web.Auth;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class SysController : BaseController
    {
        readonly IUserServices _userServices;
        readonly IAuthenticateService _authService;
        readonly ISettingsServices _settingsServices;
        readonly IConfiguration _configuration;
        readonly IShopServices _shopServices;


        public SysController(
            IUserServices userServices,
            ISettingsServices settingsServices,
            IAuthenticateService authService,
            IShopServices shopServices,
            IConfiguration configuration
            )
        {
            _userServices = userServices;
            _settingsServices = settingsServices;
            _configuration = configuration;
            _authService = authService;
            _shopServices = shopServices;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserInfo(string accessToken)
        {
            var unionId = _authService.ParseToken(accessToken);
            try
            {
                var res = await _userServices.GetUserInfo(unionId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 保存App配置及首页轮播图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveSettings([FromBody] Settings model)
        {
            var res = await _settingsServices.SaveSettings(model);
            return Ok(res);
        }

        /// <summary>
        /// 获取App配置及首页轮播图等
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            try
            {
                var res = await _settingsServices.GetSettings();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 保存店铺设置
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveShop([FromBody] Shop shop)
        {
            var res = await _shopServices.SaveShop(shop);
            return Ok(res);
        }

        /// <summary>
        /// 获取店铺设置
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetShop(string shopId)
        {
            var res = await _shopServices.GetShop(shopId);
            return Ok(res);
        }

        /// <summary>
        /// 获取所有店铺记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            var res = await _shopServices.GetShops();
            return Ok(res);
        }

        /// <summary>
        /// 删除店铺设置
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteShop(string shopId)
        {
            var res = await _shopServices.DeleteShop(shopId);
            return Ok(res);
        }
}
}
