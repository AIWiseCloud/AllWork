using AllWork.Model.DataCenter;
using System.Threading.Tasks;

namespace AllWork.IServices.DataCenter
{
    public interface IAppVisitsServices:Base.IBaseServices<AppVisits>
    {
        Task<bool> WriteAppVisits();

        Task<int> GetAppVisits(int rangType = 0);
    }
}
