using AllWork.IRepository.DataCenter;
using AllWork.IServices.DataCenter;
using AllWork.Model.DataCenter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.DataCenter
{
    public class MallDataServices:Base.BaseServices<MallData>,IMallDataServices
    {
        readonly IMallDataRepository _dal;
        public MallDataServices(IMallDataRepository mallDataRepository)
        {
            _dal = mallDataRepository;
        }

        public async Task<IEnumerable<MallData>> GetMallData()
        {
            var res = await _dal.GetMallData();
            return res;
        }
    }
}
