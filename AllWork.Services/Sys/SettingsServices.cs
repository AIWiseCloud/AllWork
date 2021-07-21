using AllWork.Model;
using AllWork.Model.Sys;
using System;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class SettingsServices: Base.BaseServices<Settings>,IServices.Sys.ISettingsServices
    {
        readonly IRepository.Sys.ISettingsRepository _dal;
        //依赖注入
        public SettingsServices(IRepository.Sys.ISettingsRepository settingsRepository)
        {
            _dal = settingsRepository;
        }

        public async Task<OperResult> SaveSettings(Settings model)
        {
            var res = await _dal.SaveSettings(model);
            return res;
        }
    
        public async Task<Settings> GetSettings()
        {
            var res = await _dal.GetSettings();
            return res;
        }
    }
}
