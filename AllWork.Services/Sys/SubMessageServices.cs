using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class SubMessageServices:Base.BaseServices<SubMessage>,ISubMessageServices
    {
        readonly ISubMessageRepository _dal;

        public SubMessageServices(ISubMessageRepository subMessageRepository)
        {
            _dal = subMessageRepository;
        }

        public async Task<OperResult> SaveSubmessage(SubMessage subMessage)
        {
            var res = await _dal.SaveSubmessage(subMessage);
            return res;
        }

        public async Task<SubMessage> GetSubMessage(string id)
        {
            var res = await _dal.GetSubMessage(id);
            return res;
        }

        public async Task<IEnumerable<SubMessage>> GetSubMessageList(string parentId)
        {
            var res = await _dal.GetSubMessageList(parentId);
            return res;
        }

        public async Task<bool> DeleteSubMessage(string id)
        {
            var res = await _dal.DeleteSubMessage(id);
            return res;
        }
    }
}
