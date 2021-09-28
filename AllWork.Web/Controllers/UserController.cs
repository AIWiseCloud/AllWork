﻿using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Model.RequestParams;
using AllWork.Model.User;
using AllWork.Web.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserServices _userServices;
        readonly IUserCertificationServices _userCertificationServices;
        readonly IAuthenticateService _authService;

        public UserController(
            IUserServices userServices,
            IUserCertificationServices userCertificationServices,

            IAuthenticateService authService
            )
        {
            _userServices = userServices;
            _userCertificationServices = userCertificationServices;
            _authService = authService;
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
        [Authorize]
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
        /// 登出
        /// </summary>
        /// <param name="token">登录凭证</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Logout(string token)
        {
            var unionId = _authService.ParseToken(token);
            try
            {
                var res = await _userServices.Logout(unionId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 账号注销
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Logoff(string token)
        {
            if (token == null || token.Length < 272)
            {
                return BadRequest("请传入正确的token");
            }
            var unionId = _authService.ParseToken(token);
            try
            {
                var res = await _userServices.Logoff(unionId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="userParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryUsers(UserParams userParams)
        {
            var res = await _userServices.QueryUsers(userParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// 提交用户认证
        /// </summary>
        /// <param name="userCertification"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetUserCertification(string unionId)
        {
            var res = await _userCertificationServices.GetUserCertification(unionId);
            return Ok(res);
        }

        /// <summary>
        /// 提交企业认证
        /// </summary>
        /// <param name="corpCertification"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveCorpCertification(CorpCertification corpCertification)
        {
            var res = await _userCertificationServices.SaveCorpCertification(corpCertification);
            return Ok(res);
        }

        /// <summary>
        /// 获取企业认证信息
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCorpCertification(string unionId)
        {
            var res = await _userCertificationServices.GetCorpCertification(unionId);
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
        /// 查询认证信息
        /// </summary>
        /// <param name="userCertificationParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryCertification(UserCertificationParams userCertificationParams)
        {
            var res = await _userCertificationServices.QueryCertification(userCertificationParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// 审核认证信息
        /// </summary>
        /// <param name="unionId">用户标识</param>
        /// <param name="certificateType">认证类型:0个人认证 1企业认证</param>
        /// <param name="authState">状态:0反审、驳回 1审核</param>
        /// <param name="reason">审核不通过理由</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AuditCertificationInfo(string unionId, int certificateType, int authState, string reason = "")
        {
            var res = await _userCertificationServices.AuditCertificationInfo(unionId, certificateType, authState, reason);
            return Ok(res);
        }

        /// <summary>
        /// 设定账号、密码
        /// </summary>
        /// <param name="unionId">用户标识</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> SetUserAccount(string unionId, string userName, string password)
        {
            var result = new OperResult { Status = false };
            if (string.IsNullOrEmpty(unionId) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                result.ErrorMsg = "参数不完整";
            }
            else if (password.Length < 6)
            {
                result.ErrorMsg = "密码长度须6位以上";
            }
            else
            {
                var user = await _userServices.GetUserInfo(userName);
                if (user != null && user.UnionId != unionId)
                {
                    result.ErrorMsg = $"已有其他用户使用{userName}";
                }
                else
                {
                    result.Status = await _userServices.SetUserAccount(unionId, userName, password);
                }
            }
            return Ok(result);
        }
    }
}
