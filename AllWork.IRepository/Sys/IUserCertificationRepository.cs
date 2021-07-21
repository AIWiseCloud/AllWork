

using AllWork.Model.Sys;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IUserCertificationRepository : Base.IBaseRepository<AllWork.Model.Sys.UserCertification>
    {
        Task<UserCertification> GetUserCertification(string unionId);

        Task<bool> SaveUserCertification(UserCertification userCertification);
    }
}
