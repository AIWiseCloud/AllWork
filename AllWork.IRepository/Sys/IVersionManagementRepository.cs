using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IVersionManagementRepository:Base.IBaseRepository<VersionManagement>
    {
        Task<bool> SaveVersionManagement(VersionManagement versionManagement);

        Task<bool> DeleteVersionManagement(string versionId);

        Task<Tuple<IEnumerable<VersionManagement>, int>> QueryVersionManagerments(CommonParams versionParams);

        Task<VersionManagement> GetNewestVersionManagement();
    }
}
