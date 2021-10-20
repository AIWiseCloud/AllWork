using AllWork.Model.DataCenter;
using System.Threading.Tasks;

namespace AllWork.IRepository.DataCenter
{
    public interface IAppVisitsRepository:Base.IBaseRepository<AppVisits>
    {
        Task<int> WriteAppVisits();

        Task<int> GetAppVisits(int rangType=0);
    }
}
