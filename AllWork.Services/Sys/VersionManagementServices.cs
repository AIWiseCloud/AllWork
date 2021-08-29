using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class VersionManagementServices:Base.BaseServices<VersionManagement>,IVersionManagementServices
    {
        readonly IVersionManagementRepository _dal;
        public VersionManagementServices(IVersionManagementRepository versionManagementRepository)
        {
            _dal = versionManagementRepository;
        }
        public async Task<bool> SaveVersionManagement(VersionManagement versionManagement)
        {
            var res = await _dal.SaveVersionManagement(versionManagement);
            return res;
        }

        public async Task<bool> DeleteVersionManagement(string versionId)
        {
            var res = await _dal.DeleteVersionManagement(versionId);
            return res;
        }

        public async Task<Tuple<IEnumerable<VersionManagement>, int>> QueryVersionManagerments(VersionParams versionParams)
        {
            var res = await _dal.QueryVersionManagerments(versionParams);
            return res;
        }

        public async Task<VersionManagement> GetNewestVersionManagement()
        {
            var res = await _dal.GetNewestVersionManagement();
            return res;
        }
    }
}
