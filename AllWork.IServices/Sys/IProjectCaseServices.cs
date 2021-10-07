using AllWork.Model;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IProjectCaseServices:Base.IBaseServices<ProjectCase>
    {
        Task<OperResult> SaveProjectCase(ProjectCase projectCase);

        Task<ProjectCase> GetProjectCase(string id);

        Task<bool> DeleteProjectCase(string id);

        Task<Tuple<IEnumerable<ProjectCase>, int>> QueryProjectCase(ProjectCaseParams projectCaseParams);
    }
}
