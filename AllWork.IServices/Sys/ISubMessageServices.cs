using AllWork.Model;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface ISubMessageServices:Base.IBaseServices<SubMessage>
    {
        Task<OperResult> SaveSubmessage(SubMessage subMessage);

        Task<SubMessage> GetSubMessage(string id);

        Task<IEnumerable<SubMessage>> GetSubMessageList(string parentId);

        Task<bool> DeleteSubMessage(string id);
    }
}
