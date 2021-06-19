using AllWork.Model.Sys;
using AllWork.Web.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 用户认证和签发token的控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authService;
        public AuthenticationController(IAuthenticateService authService)
        {
            this._authService = authService;
        }

        [AllowAnonymous]
        [HttpPost,Route("requestToken")]
        public ActionResult RequestToken([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }
            if (_authService.IsAuthenticated(request, out string token))
            {
                return Ok(token);
            }

            return BadRequest("Invalid Request");
        }
    }
}
