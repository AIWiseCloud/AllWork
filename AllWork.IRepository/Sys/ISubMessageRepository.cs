using AllWork.Model;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface ISubMessageRepository:Base.IBaseRepository<SubMessage>
    {
        Task<OperResult> SaveSubmessage(SubMessage subMessage);

        Task<SubMessage> GetSubMessage(string id);

        Task<IEnumerable<SubMessage>> GetSubMessageList(string parentId);

        Task<bool> DeleteSubMessage(string id);
    }
}
