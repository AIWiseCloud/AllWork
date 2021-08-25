using AllWork.Model.User;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IUserCertificationRepository : Base.IBaseRepository<UserCertification>
    {
        Task<UserCertification> GetUserCertification(string unionId);

        Task<bool> SaveUserCertification(UserCertification userCertification);

        Task<CorpCertification> GetCorpCertification(string unionId);

        Task<bool> SaveCorpCertification(CorpCertification corpCertification);
    }
}
