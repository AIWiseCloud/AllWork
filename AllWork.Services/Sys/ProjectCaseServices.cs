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
    public class ProjectCaseServices:Base.BaseServices<ProjectCase>,IProjectCaseServices
    {
        readonly IProjectCaseRepository _dal;
        public ProjectCaseServices(IProjectCaseRepository projectCaseRepository)
        {
            _dal = projectCaseRepository;
        }

        public async Task<OperResult> SaveProjectCase(ProjectCase projectCase)
        {
            var result = new OperResult { Status = false };
            if (string.IsNullOrEmpty(projectCase.ID))
            {
                projectCase.ID = Guid.NewGuid().ToString();
            }
            result.IdentityKey = projectCase.ID;
            var res = await _dal.SaveProjectCase(projectCase);
            result.Status = res > 0;
            return result;
        }

        public async Task<ProjectCase> GetProjectCase(string id)
        {
            var res = await _dal.GetProjectCase(id);
            return res;
        }

        public async Task<bool> DeleteProjectCase(string id)
        {
            var res = await _dal.DeleteProjectCase(id);
            return res > 0;
        }

        public async Task<Tuple<IEnumerable<ProjectCase>, int>> QueryProjectCase(ProjectCaseParams projectCaseParams)
        {
            var res = await _dal.QueryProjectCase(projectCaseParams);
            return res;
        }
    }
}
