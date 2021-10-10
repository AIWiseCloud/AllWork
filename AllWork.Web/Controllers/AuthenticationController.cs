using AllWork.Model.Sys;
using AllWork.Web.Auth;
using AllWork.Web.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing; //如果在Linux上运行，要改引用using System.DrawingCore;  using System.DrawingCore.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 用户认证和签发token
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        readonly IAuthenticateService _authService;

        public AuthenticationController(IAuthenticateService authService)
        {
            this._authService = authService;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost,Route("requestToken")]
        public async Task<ActionResult> RequestToken([FromBody] LoginRequestDTO request)
        {
            //从数据库验证用户，结果返回为元组
            var res = await _authService.IsAuthenticated(request);//返回值用元组Tuple保存的，第一个为bool即是否为有效用户, 第二个为string即token
            if (res.Item1)
            {
                return Ok(res.Item2);
            }
            else
            {
                return BadRequest("Invalid request");
            }
        }

        /// <summary>
        /// 解析accessToken
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpGet]
        public string ParseToken(string accessToken)
        {
            var data = _authService.ParseToken(accessToken);
            return data;
        }

      

     
    }
}
