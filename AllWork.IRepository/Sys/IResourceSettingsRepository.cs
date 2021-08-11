using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IResourceSettingsRepository:Base.IBaseRepository<ResourceSettings>
    {
        Task<bool> SaveResourceSettings(ResourceSettings resourceSettings);

        Task<bool> DeleteResourceSettings(string sourceId);

        Task<ResourceSettings> GetResourceSettings(string sourceId);

        Task<dynamic> GetGroups();

        Task<List<ResourceSettings>> GetResourceSettingsByGroup(string groupNo);

        Task<Tuple<IEnumerable<ResourceSettings>, int>> QueryResourceSettings(ResourceParams resourceParams);
    }
}
