using AllWork.IServices.Base;
using AllWork.Model.Sys;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IUserCertificationServices : IBaseServices<UserCertification>
    {
        Task<UserCertification> GetUserCertification(string unionId);

        Task<bool> SaveUserCertification(UserCertification userCertification);
    }
}
