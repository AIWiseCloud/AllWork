using AllWork.Model;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface ISMSServices
    {
        Task<OperResult> SendOrderMsg(string phoneNumbers, string receiver, string orderId);

        Task<OperResult> GetVerifyCode(string unionId, string phoneNumber);

        Task<OperResult> SendMSG(string phoneNumber, string templateCode, string templateParam, string signName);
    }
}
