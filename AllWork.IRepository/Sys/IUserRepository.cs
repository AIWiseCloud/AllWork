

using AllWork.Model.Sys;
using AllWork.Model.User;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IUserRepository:Base.IBaseRepository<UserInfo>
    {
        Task<UserInfo> GetUserInfo(string unionId);

        Task<bool> IsValidUser(LoginRequestDTO req);

        Task<bool> SaveUserInfo(UserInfo userInfo);

        Task<bool> Logout(string unionId);
    }
}
