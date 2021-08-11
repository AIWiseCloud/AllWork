﻿using AllWork.IServices.Sys;
using AllWork.Model.Sys;
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
    }
}
