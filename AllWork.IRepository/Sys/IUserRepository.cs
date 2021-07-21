

using AllWork.Model.Sys;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IUserRepository:Base.IBaseRepository<AllWork.Model.Sys.UserInfo>
    {
        Task<UserInfo> GetUserInfo(string unionId);

        Task<bool> IsValidUser(LoginRequestDTO req);

        Task<bool> SaveUserInfo(UserInfo userInfo);
    }
}
