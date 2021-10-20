using AllWork.IRepository.DataCenter;
using AllWork.IServices.DataCenter;
using AllWork.Model.DataCenter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Services.DataCenter
{
    public class AppVisitsServices:Base.BaseServices<AppVisits>,IAppVisitsServices
    {
        readonly IAppVisitsRepository _dal;
        public AppVisitsServices(IAppVisitsRepository appVisitsRepository)
        {
            _dal = appVisitsRepository;
        }

        public async Task<bool> WriteAppVisits()
        {
            var res = await _dal.WriteAppVisits();
            return res > 0;
        }

        public async Task<int> GetAppVisits(int rangType = 0)
        {
            var res = await _dal.GetAppVisits(rangType);
            return res;
        }
    }
}
