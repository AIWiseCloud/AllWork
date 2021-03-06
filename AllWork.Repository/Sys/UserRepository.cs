using AllWork.IRepository.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using AllWork.Model.User;
using AllWork.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        //获取用户信息
        public async Task<UserInfo> GetUserInfo(string unionIdOrUserName)
        {
            var res = await base.QueryFirst($"Select * from UserInfo Where UnionId = '{unionIdOrUserName}' or PhoneNumber = '{unionIdOrUserName}' OR UserId = '{unionIdOrUserName}' ");
            return res;
        }

        //验证是否为有效用户
        public async Task<UserInfo> IsValidUser(LoginRequestDTO req)
        {
            if (req.Username.Length == 28)
            {
                var res = await base.QueryFirst($"Select * from UserInfo Where UnionId = '{req.Username}' and UserState != -1 ");
                return res ;
            }
            else
            {
                var res = await base.QueryFirst($"Select * from UserInfo Where (UserId = '{req.Username}' or PhoneNumber = '{req.Username}' ) and Password = '{req.Password}' and UserState != -1 ");
                return res;
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
                var random = AllWork.Common.Utils.GetRandomNum(100000, 999999);
                userInfo.Password = AllWork.Common.DesEncrypt.Encrypt(random.ToString());//首次注册默认提供6位数的随机密码
                var insertSql = @"Insert UserInfo (UnionId,OpenId,Password, NickName,PhoneNumber,Email,Avatar,Province,City,County,Gender,UserState,Roles)values
(@UnionId,@OpenId, @Password, @NickName,@PhoneNumber,@Email,@Avatar,@Province,@City,@County,@Gender,@UserState,'user')";
                return await base.Execute(insertSql, userInfo) > 0;
            }
            else
            {
                var updateSql = @"Update UserInfo set OpenId = @OpenId,NickName = @NickName,Email = @Email,Avatar = @Avatar,Province = @Province,City = @City,County = @County,Gender = @Gender,UserState = @UserState Where UnionId = @UnionId";
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
            //var sql = "Delete from UserInfo  Where UnionId = @UnionId";
            var sql = "Update UserInfo set PhoneNumber = '',password='',UserState = 0,avatar = '',NickName=''  Where UnionId = @UnionId";
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
                sqlpub.AppendFormat(" and (UnionId = @UnionId or UserId = @UserName or NickName like '%{0}%' ) ", userParams.Keywords);
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
                UserName=userParams.Keywords,
                userParams.StartDate,
                userParams.EndDate,
                userParams.PageModel.Skip,
                userParams.PageModel.PageSize
            });


            return res;
        }

        public async Task<bool> SetUserPassword(string unionId, string password)
        {
            var sql = "Update UserInfo set Password = @Password Where UnionId = @UnionId";
            var res = await base.Execute(sql, new { UnionId = unionId, Password = password });
           
            return res> 0;
        }

        public async Task<bool> BindPhoeNumber(string unionId, string phoneNumber)
        {
            var sql = "Update UserInfo set PhoneNumber = @PhoneNumber Where UnionId = @UnionId";
            var res = await base.Execute(sql, new { UnionId = unionId, PhoneNumber = phoneNumber });
            return res > 0;
        }

        public async Task<bool> BindSalesman(string unionId, string openUserId, string salesman)
        {
            var sql = "Update UserInfo set OpenUserId = @OpenUserId, Salesman = @Salesman Where UnionId = @UnionId";
            var res = await base.Execute(sql, new { UnionId = unionId, OpenUserId = openUserId, Salesman = salesman });
            return res > 0;
        }

        public async Task<bool> SetUserRoles(string unionId, string roles)
        {
            var sql = "Update UserInfo set Roles = @Roles Where UnionId = @unionId";
            var res = await base.Execute(sql, new { UnionId = unionId, Roles = roles });
            return res > 0;
        }

        //检查手机号是否被其他账号绑定
        public async Task<bool> CheckPhoneNumberBindOther(string unionId, string phoneNumber)
        {
            var sql = "Select count(*) from UserInfo Where UnionID != @UnionId and PhoneNumber = @PhoneNumber";
            var res = await base.ExecuteScalar<int>(sql, new { UnionId = unionId, PhoneNumber = phoneNumber });
            return res > 0;
        }

        //所有客服电话
        public async Task<string> GetCustomerServicePhoneNumbers()
        {
            var sql = "select group_concat(PhoneNumber) from UserInfo where (Roles  like concat('%','cs','%') or Roles  like concat('%','admin','%')) and ifnull(PhoneNumber,'')!=''";
            var res = await base.ExecuteScalar<string>(sql);
            return res;
        }

        //我的业务员
        public async Task<object> GetSalesman(string unionId)
        {
            var sql = @"select a.PhoneNumber, a.Salesman,
IFNull(t2.Name,t3.Name )as LegalPerson, 
IFNull(t2.CorpName, t3.CorpName) as CorpName,
IFNull(t2.CorpAddress,t3.CorpAddress ) as CorpAddress,
IFNull(t2.PhoneNumber,t3.PhoneNumber ) as CorpPhone
from UserInfo a, SalesMan b, UserInfo c left join corpcertification t2 on c.UnionId = t2.UnionId 
left join usercertification t3 on c.UnionId = t3.UnionId 
where b.OpenUserId = c.OpenUserId and c.UnionId = @UnionId
and a.PhoneNumber = b.Mobile";
            var res = await base.QueryFirst<object>(sql, new { UnionId = unionId });
            return res;
        }

        //我的客户
        public async Task<Tuple<IEnumerable<object>, int>> GetMyCustomers(CustomerParams customerParams)
        {
            var sqlpub = @" from UserInfo u 
where OpenUserId in (select OpenUserId from userinfo where UnionId = @UnionId )";
            var sql1 = "Select count(UnionId) as TotalCount " + sqlpub;
            var sql2 = " select NickName , Avatar,PhoneNumber " + sqlpub + " Order by CreateDate desc limit @Skip, @PageSize ";
            var sqlfull = sql1 + ";" + sql2;
            var res = await base.QueryPagination<UserInfo>(sqlfull, new
            {
                customerParams.UnionId,
                customerParams.PageModel.Skip,
                customerParams.PageModel.PageSize
            });
            var items = new List<object>();
            foreach(var item in res.Item1)
            {
                items.Add(new { nickName = item.NickName, avatar = item.Avatar, phoneNumber = item.PhoneNumber });
            }
            var result = new Tuple<IEnumerable<object>, int>(items, res.Item2);
            return result;
        }
    }
}
