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
        readonly IUserCertificationServices _userCertificationServices;
        readonly IAuthenticateService _authService;
        readonly ISettingsServices _settingsServices;
        readonly IShopServices _shopServices;
        readonly ISubMesTypeServices _subMesTypeServices;
        readonly ISubMessageServices _subMessageServices;

        public SysController(
            IUserServices userServices,
            IUserCertificationServices userCertificationServices,
            ISettingsServices settingsServices,
            IAuthenticateService authService,
            IShopServices shopServices,
            IConfiguration configuration,
            ISubMesTypeServices subMesTypeServices,
            ISubMessageServices subMessageServices
            )
        {
            _userServices = userServices;
            _userCertificationServices = userCertificationServices;
            _settingsServices = settingsServices;
            _authService = authService;
            _shopServices = shopServices;
            _subMesTypeServices = subMesTypeServices;
            _subMessageServices = subMessageServices;
        }

        /// <summary>
        /// 获取用户信息(需先token认证)
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserInfo(string unionId)
        {
            //var unionId = _authService.ParseToken(accessToken);
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
        /// 通过Access_token获取用户信息(后端平台用)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserInfoByToken(string token)
        {
            var unionId = _authService.ParseToken(token);
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
        /// 提交用户认证
        /// </summary>
        /// <param name="userCertification"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveUserCertification(UserCertification userCertification)
        {
            var res = await _userCertificationServices.SaveUserCertification(userCertification);
            return Ok(res);
        }

        /// <summary>
        /// 获取用户认证信息
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserCertification(string unionId)
        {
            var res = await _userCertificationServices.GetUserCertification(unionId);
            return Ok(res);
        }


        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveUserInfo(UserInfo userInfo)
        {
            var res = await _userServices.SaveUserInfo(userInfo);
            return Ok(res);
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

        /// <summary>
        /// 保存辅助资料分类
        /// </summary>
        /// <param name="subMesType"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveSubMesType(SubMesType subMesType)
        {
            var res = await _subMesTypeServices.SaveSubMesType(subMesType);
            return Ok(res);
        }

        /// <summary>
        /// 获取辅助资料分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubMesType(string id)
        {
            var res = await _subMesTypeServices.GetSubMesType(id);
            return Ok(res);
        }

        /// <summary>
        /// 获取辅助资料所有分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubMesTypes()
        {
            var res = await _subMesTypeServices.GetSubMesTypes();
            return Ok(res);
        }

        /// <summary>
        /// 删除辅助资料分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteSubMesType(string id)
        {
            var res = await _subMesTypeServices.DeleteSubMesType(id);
            return Ok(res);
        }

        /// <summary>
        /// 保存辅助资料项目
        /// </summary>
        /// <param name="subMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveSubMessage(SubMessage subMessage)
        {
            var res = await _subMessageServices.SaveSubmessage(subMessage);
            return Ok(res);
        }

        /// <summary>
        /// 获取辅助资料项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubMessage(string id)
        {
            var res = await _subMessageServices.GetSubMessage(id);
            return Ok(res);
        }

        /// <summary>
        /// 获取指定分类下的辅助资料列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubMessageList(string parentId)
        {
            var res = await _subMessageServices.GetSubMessageList(parentId);
            return Ok(res);
        }

        /// <summary>
        /// 删除辅助资料项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteSubMessage(string id)
        {
            var res = await _subMessageServices.DeleteSubMessage(id);
            return Ok(res);
        }
}
}
