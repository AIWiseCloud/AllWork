using AllWork.IServices.Sys;
using AllWork.Model.Sys;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Web.Auth
{
    public class AuthenticationService : IAuthenticateService
    {
        private readonly IUserServices _userService;
        private readonly TokenManagement _tokenManagement;
        public AuthenticationService(IUserServices userService, IOptions<TokenManagement> tokenManagement)
        {
            _userService = userService;
            _tokenManagement = tokenManagement.Value;
        }

        //验证以获取token
        public async Task<Tuple<bool, string>> IsAuthenticated(LoginRequestDTO request)
        {
            //token = string.Empty;
            var user = await _userService.IsValidUser(request);
            if (user == null)
                return Tuple.Create(false, "");
            var rolesArr = user.Roles.Split(',');
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name,request.Username),
                 //new Claim("roy","易国轩") //可以添加一些键值对信息，然后解析token时可以取得
            };
            foreach (var role in rolesArr)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); //用户角色
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), //过期时间（当前时间  + 分钟分钟数）
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Tuple.Create(true, token);
        }

        //解析token得到账号（可能是unionId,也可能是账号、手机号，由登录参数决定）
        public string ParseToken(string accessToken)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(accessToken);
            var items = new List<string>();
            foreach (var item in jwtToken.Claims)
            {
                items.Add(item.Value);
            }
            //object role;
            //try
            //{
            //    jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
            return items.Count > 0 ? items[0] : string.Empty;
        }
    }
}
