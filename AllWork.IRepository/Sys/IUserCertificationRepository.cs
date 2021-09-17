using AllWork.Model.RequestParams;
using AllWork.Model.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IUserCertificationRepository : Base.IBaseRepository<UserCertification>
    {
        Task<UserCertification> GetUserCertification(string unionId);

        Task<bool> SaveUserCertification(UserCertification userCertification);

        Task<CorpCertification> GetCorpCertification(string unionId);

        Task<bool> SaveCorpCertification(CorpCertification corpCertification);

        Task<Tuple<IEnumerable<UserCertification>, int>> QueryCertification(UserCertificationParams userCertificationParams);

        Task<bool> AuditCertificationInfo(string unionId, int certificateType, int authState, string reason = "");
    }
}
