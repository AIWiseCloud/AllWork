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

      
        public SysController(IUserServices userServices, ISettingsServices settingsServices, IAuthenticateService authService, IConfiguration configuration)
        {
            _userServices = userServices;
            _settingsServices = settingsServices;
            _configuration = configuration;
            _authService = authService;
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
        public async Task<IActionResult> SaveSettings([FromBody]Settings model)
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
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
