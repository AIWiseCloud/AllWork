using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Sys
{
    public class SubMesTypeRepository:Base.BaseRepository<SubMesType>, IRepository.Sys.ISubMesTypeRepository
    {
        public SubMesTypeRepository(IConfiguration configuration):base(configuration)
        {
            
        }
    }
}
