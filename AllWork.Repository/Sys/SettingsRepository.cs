using AllWork.Model.Sys;
using AllWork.IRepository.Sys;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Sys
{
    public class SettingsRepository:Base.BaseRepository<Settings>,ISettingsRepository
    {
        public SettingsRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
