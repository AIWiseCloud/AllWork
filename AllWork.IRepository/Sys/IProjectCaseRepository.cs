using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IProjectCaseRepository:Base.IBaseRepository<ProjectCase>
    {
        Task<int> SaveProjectCase(ProjectCase projectCase);

        Task<ProjectCase> GetProjectCase(string id);

        Task<int> DeleteProjectCase(string id);

        Task<Tuple<IEnumerable<ProjectCase>, int>> QueryProjectCase(ProjectCaseParams projectCaseParams);
    }
}
