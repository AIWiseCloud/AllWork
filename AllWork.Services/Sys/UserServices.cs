using System;
using System.Collections.Generic;
using System.Text;
using AllWork.IRepository.Sys;
using System.Threading.Tasks;
using AllWork.Model.Sys;
using AllWork.Services.Base;
using AllWork.IServices.Sys;

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
        public async Task<UserInfo> GetUserInfo(string unionId)
        {
            var res = await _dal.QueryFirst($"Select * from UserInfo Where UnionId = '{unionId}'");
            return res;
        }

        //验证是否为有效用户
        public async Task<bool> IsValidUser(LoginRequestDTO req)
        {
            var res = await _dal.QueryFirst($"Select * from UserInfo Where UnionId = '{req.Username}' or  Name = '{req.Username}'");
            return res != null;
        }

        //保存用户信息
        public async Task<bool> SaveUserInfo(UserInfo userInfo)
        {
            var model = await _dal.QueryFirst("Select * from UserInfo Where UnionId = @UnionId", userInfo);
            userInfo.UserState = 1;
            userInfo.Roles = string.IsNullOrEmpty(userInfo.Roles) ? "ediror" : userInfo.Roles;
            if (model == null)
            {
                var insertSql = @"Insert UserInfo (UnionId,NickName,Password,PhoneNumber,Email,Avatar,Province,City,County,Gender,UserState,Roles)values
(@UnionId,@NickName,@Password,@PhoneNumber,@Email,@Avatar,@Province,@City,@County,@Gender,@UserState,@Roles)";
                return await _dal.Execute(insertSql, userInfo) > 0;
            }
            else
            {
                var updateSql = @"Update UserInfo set UnionId = @UnionId,NickName = @NickName,Password = @Password,PhoneNumber = @PhoneNumber,Email = @Email,Avatar = @Avatar,Province = @Province,City = @City,County = @County,Gender = @Gender,UserState = @UserState,Roles = @Roles Where UnionId = @UnionId";
                return await _dal.Execute(updateSql, userInfo) > 0;
            }
        }


    }
}
