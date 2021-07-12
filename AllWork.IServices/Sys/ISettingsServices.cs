using AllWork.IServices.Base;
using AllWork.Model.Sys;
using AllWork.Model;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface ISettingsServices:IBaseServices<Settings>
    {
        Task<OperResult> SaveSettings(Settings model);

        Task<Settings> GetSettings();
    }
}
