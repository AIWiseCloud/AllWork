using AllWork.IRepository.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using AllWork.Model.User;
using AllWork.Repository.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        //获取用户信息
        public async Task<UserInfo> GetUserInfo(string unionId)
        {
            var res = await base.QueryFirst($"Select * from UserInfo Where UnionId = '{unionId}' OR UserName = '{unionId}' ");
            return res;
        }

        //验证是否为有效用户
        public async Task<bool> IsValidUser(LoginRequestDTO req)
        {
            if (req.Username.Length == 28)
            {
                var res = await base.QueryFirst($"Select * from UserInfo Where UnionId = '{req.Username}' and UserState != -1 ");
                return res != null;
            }
            else
            {
                var res = await base.QueryFirst($"Select * from UserInfo Where UserName = '{req.Username}' and Password = '{req.Password}' and UserState != -1 ");
                return res != null;
            }


        }

        //保存用户信息
        public async Task<bool> SaveUserInfo(UserInfo userInfo)
        {
            var model = await base.QueryFirst("Select * from UserInfo Where UnionId = @UnionId", userInfo);
            userInfo.UserState = 1;
            userInfo.Roles = string.IsNullOrEmpty(userInfo.Roles) ? "ediror" : userInfo.Roles;
            if (model == null)
            {
                var insertSql = @"Insert UserInfo (UnionId,OpenId,NickName,PhoneNumber,Email,Avatar,Province,City,County,Gender,UserState,Roles)values
(@UnionId,@OpenId, @NickName,@PhoneNumber,@Email,@Avatar,@Province,@City,@County,@Gender,@UserState,@Roles)";
                return await base.Execute(insertSql, userInfo) > 0;
            }
            else
            {
                var updateSql = @"Update UserInfo set OpenId = @OpenId,NickName = @NickName,PhoneNumber = @PhoneNumber,Email = @Email,Avatar = @Avatar,Province = @Province,City = @City,County = @County,Gender = @Gender,UserState = @UserState,Roles = @Roles Where UnionId = @UnionId";
                return await base.Execute(updateSql, userInfo) > 0;
            }
        }

        //登出
        public async Task<bool> Logout(string unionId)
        {
            var sql = "Update UserInfo set UserState = 0 Where UnionId = @UnionId";
            var res = await base.Execute(sql, new { UnionId = unionId });
            return res > 0;
        }

        //账号注销
        public async Task<bool> Logoff(string unionId)
        {
            var sql = "Delete from UserInfo  Where UnionId = @UnionId";
            var res = await base.Execute(sql, new { UnionId = unionId });
            return res > 0;
        }

        //分页查询用户
        public async Task<Tuple<IEnumerable<UserInfo>, int>> QueryUsers(UserParams userParams)
        {
            //(1) sql公共部分
            var sqlpub = new StringBuilder(" from UserInfo a Where (1=1)");
            if (!string.IsNullOrEmpty(userParams.Keywords))
            {
                sqlpub.AppendFormat(" and (UnionId = @UnionId or UserName like '%{0}%' or NickName like '%{0}%' ) ", userParams.Keywords);
            }
            if (!string.IsNullOrEmpty(userParams.StartDate) && !string.IsNullOrEmpty(userParams.EndDate))
            {
                sqlpub.Append(" and  a.CreateDate between @StartDate and @EndDate ");
            }
            //(2) 固定排序
            string sqlorder = " Order by CreateDate desc ";
            //(3) 求记录数
            var sql1 = "Select count(a.UnionId)  " + sqlpub.ToString();
            //(4) 分页获取数据
            var sql2 = " Select * " + sqlpub.ToString() + sqlorder + " limit @Skip, @PageSize ";
            //(5) 合并sql, 
            var sql = sql1 + ";" + sql2;
            //(6) 执行查询
            var res = await base.QueryPagination(sql, new
            {
                UnionId = userParams.Keywords,
                userParams.PageModel.Skip,
                userParams.PageModel.PageSize
            });


            return res;
        }

    }
}
