using AllWork.IRepository.Sys;
using AllWork.Model.Sys;
using AllWork.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Sys
{
    public class UserCertificationRepository:BaseRepository<UserCertification>,IUserCertificationRepository
    {
        public UserCertificationRepository(IConfiguration configuration) : base(configuration)
        {
           
        }
    }
}
