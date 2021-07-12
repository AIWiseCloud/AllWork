using AllWork.IRepository.Sys;
using AllWork.Model.Sys;
using AllWork.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Sys
{
    public class UserRepository:BaseRepository<UserInfo>,IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
           
        }
    }
}
