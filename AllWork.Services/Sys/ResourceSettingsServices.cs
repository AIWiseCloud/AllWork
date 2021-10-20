using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class ResourceSettingsServices : Base.BaseServices<ResourceSettings>, IResourceSettingsServices
    {
        readonly IResourceSettingsRepository _dal;

        public ResourceSettingsServices(IResourceSettingsRepository resourceSettingRepository)
        {
            _dal = resourceSettingRepository;
        }

        public async Task<OperResult> SaveResourceSettings(ResourceSettings resourceSettings)
        {
            if (string.IsNullOrWhiteSpace(resourceSettings.SourceId))
            {
                resourceSettings.SourceId = System.Guid.NewGuid().ToString();
            }
            var res = await _dal.SaveResourceSettings(resourceSettings);
            return new OperResult { Status = res, IdentityKey = resourceSettings.SourceId };
        }

        public async Task<bool> DeleteResourceSettings(string sourceId)
        {
            var res = await _dal.DeleteResourceSettings(sourceId);
            return res;
        }

        public async Task<ResourceSettings> GetResourceSettings(string sourceId)
        {
            var res = await _dal.GetResourceSettings(sourceId);
            return res;
        }

        public async Task<dynamic> GetGroups()
        {
            var res = await _dal.GetGroups();
            return res;
        }

        public async Task<List<ResourceSettings>> GetResourceSettingsByGroup(string groupNo)
        {
            var res = await _dal.GetResourceSettingsByGroup(groupNo);
            return res;
        }

        public async Task<Tuple<IEnumerable<ResourceSettings>, int>> QueryResourceSettings(ResourceParams resourceParams)
        {
            var res = await _dal.QueryResourceSettings(resourceParams);
            return res;
        }
    }
}
