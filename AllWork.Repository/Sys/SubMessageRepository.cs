using AllWork.IRepository.Sys;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Sys
{
    public class SubMessageRepository:Base.BaseRepository<SubMessage>, ISubMessageRepository
    {
        public SubMessageRepository(IConfiguration configuration):base(configuration)
        {

        }
    }
}
