using AllWork.IServices.Base;
using AllWork.Model.Sys;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IUserServices : IBaseServices<UserInfo>
    {
        Task<UserInfo> GetUserInfo(string unionId);

        Task<bool> IsValidUser(LoginRequestDTO req);

        Task<bool> SaveUserInfo(UserInfo userInfo);

    }
}
