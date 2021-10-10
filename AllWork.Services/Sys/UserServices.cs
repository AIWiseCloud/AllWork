using AllWork.Common;
using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using AllWork.Model.User;
using AllWork.Services.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class UserServices : BaseServices<UserInfo>, IUserServices
    {
        readonly IUserRepository _dal;

        //以依赖注入的形式使用_dal
        public UserServices(IUserRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        //获取用户信息
        public async Task<UserInfo> GetUserInfo(string unionIdOrUserName)
        {
            var res = await _dal.GetUserInfo(unionIdOrUserName);
            return res;
        }

        //验证是否为有效用户
        public async Task<UserInfo> IsValidUser(LoginRequestDTO req)
        {
            //前台输的明文加密后再与系统中的比对
            if (req.Username.Length != 28 && !string.IsNullOrEmpty(req.Password))
            {
                req.Password = DesEncrypt.Encrypt(req.Password);
            }

            var res = await _dal.IsValidUser(req);
         
            return res;
        }

        //保存用户信息
        public async Task<bool> SaveUserInfo(UserInfo userInfo)
        {
            var res = await _dal.SaveUserInfo(userInfo);
            return res;
        }

        public async Task<bool> Logout(string unionId)
        {
            var res = await _dal.Logout(unionId);
            return res;
        }

        public async Task<bool> Logoff(string unionId)
        {
            var res = await _dal.Logoff(unionId);
            return res;
        }

        public async Task<Tuple<IEnumerable<UserInfo>, int>> QueryUsers(UserParams userParams)
        {
            var res = await _dal.QueryUsers(userParams);
            return res;
        }

        public async Task<bool> SetUserPassword(string unionId, string password)
        {
            //加密
            var pw = AllWork.Common.DesEncrypt.Encrypt(password);
            var res = await _dal.SetUserPassword(unionId, pw);
            return res;
        }

        public async Task<bool> BindPhoeNumber(string unionId, string phoneNumber)
        {
            var res = await _dal.BindPhoeNumber(unionId, phoneNumber);
            return res;
        }

        public async Task<bool> SetUserRoles(string unionId, string roles)
        {
            var res = await _dal.SetUserRoles(unionId, roles);
            return res;
        }
    }
}
