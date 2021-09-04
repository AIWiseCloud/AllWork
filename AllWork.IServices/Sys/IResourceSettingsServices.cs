using AllWork.Model;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IResourceSettingsServices:Base.IBaseServices<ResourceSettings>
    {
        Task<OperResult> SaveResourceSettings(ResourceSettings resourceSettings);

        Task<bool> DeleteResourceSettings(string sourceId);

        Task<ResourceSettings> GetResourceSettings(string sourceId);

        Task<dynamic> GetGroups();

        Task<List<ResourceSettings>> GetResourceSettingsByGroup(string groupNo);

        Task<Tuple<IEnumerable<ResourceSettings>, int>> QueryResourceSettings(CommonParams resourceParams);
    }
}
