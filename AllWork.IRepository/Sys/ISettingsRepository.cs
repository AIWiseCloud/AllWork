using AllWork.Model;
using AllWork.Model.Sys;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface ISettingsRepository:Base.IBaseRepository<Settings>
    {
        Task<OperResult> SaveSettings(Settings model);

        Task<Settings> GetSettings();
    }
}
