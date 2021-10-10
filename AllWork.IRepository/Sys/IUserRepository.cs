using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using AllWork.Model.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IUserRepository:Base.IBaseRepository<UserInfo>
    {

        Task<UserInfo> GetUserInfo(string unionIdOrUserName);

        Task<UserInfo> IsValidUser(LoginRequestDTO req);

        Task<bool> SaveUserInfo(UserInfo userInfo);

        Task<bool> Logout(string unionId);

        Task<bool> Logoff(string unionId);

        Task<Tuple<IEnumerable<UserInfo>, int>> QueryUsers(UserParams userParams);

        Task<bool> SetUserPassword(string unionId, string password);

        Task<bool> BindPhoeNumber(string unionId, string phoneNumber);

        Task<bool> SetUserRoles(string unionId, string roles);
    }
}
