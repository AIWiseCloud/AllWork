using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface ISubMessageServices:Base.IBaseServices<SubMessage>
    {
        Task<bool> SaveSubmessage(SubMessage subMessage);

        Task<SubMessage> GetSubMessage(string id);

        Task<IEnumerable<SubMessage>> GetSubMessageList(string parentId);

        Task<bool> DeleteSubMessage(string id);
    }
}
