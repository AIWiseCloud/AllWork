using AllWork.IRepository.Sys;
using AllWork.Model.Sys;
using AllWork.Repository.Base;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class UserRepository:BaseRepository<UserInfo>,IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        //获取用户信息
        public async Task<UserInfo> GetUserInfo(string unionId)
        {
            var res = await base.QueryFirst($"Select * from UserInfo Where UnionId = '{unionId}'");
            return res;
        }

        //验证是否为有效用户
        public async Task<bool> IsValidUser(LoginRequestDTO req)
        {
            var res = await base.QueryFirst($"Select * from UserInfo Where UnionId = '{req.Username}'");
            return res != null;
        }

        //保存用户信息
        public async Task<bool> SaveUserInfo(UserInfo userInfo)
        {
            var model = await base.QueryFirst("Select * from UserInfo Where UnionId = @UnionId", userInfo);
            userInfo.UserState = 1;
            userInfo.Roles = string.IsNullOrEmpty(userInfo.Roles) ? "ediror" : userInfo.Roles;
            if (model == null)
            {
                var insertSql = @"Insert UserInfo (UnionId,NickName,Password,PhoneNumber,Email,Avatar,Province,City,County,Gender,UserState,Roles)values
(@UnionId,@NickName,@Password,@PhoneNumber,@Email,@Avatar,@Province,@City,@County,@Gender,@UserState,@Roles)";
                return await base.Execute(insertSql, userInfo) > 0;
            }
            else
            {
                var updateSql = @"Update UserInfo set UnionId = @UnionId,NickName = @NickName,Password = @Password,PhoneNumber = @PhoneNumber,Email = @Email,Avatar = @Avatar,Province = @Province,City = @City,County = @County,Gender = @Gender,UserState = @UserState,Roles = @Roles Where UnionId = @UnionId";
                return await base.Execute(updateSql, userInfo) > 0;
            }
        }

    }
}
