using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.Sys;
using AllWork.Model.User;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class UserCertificationServices:Base.BaseServices<UserCertification>,IUserCertificationServices
    {
        readonly IUserCertificationRepository _dal;
        public UserCertificationServices(IUserCertificationRepository userCertificationRepository)
        {
            _dal = userCertificationRepository;
        }

        //提交用户验证
        public async Task<bool> SaveUserCertification(UserCertification userCertification)
        {
            var res = await _dal.SaveUserCertification(userCertification);
            return res;
        }

        //获取用户验证信息
        public async Task<UserCertification> GetUserCertification(string unionId)
        {
            var res = await _dal.GetUserCertification(unionId);
            return res;
        }

        //提交企业认证
        public async Task<bool> SaveCorpCertification(CorpCertification  corpCertification)
        {
            var res = await _dal.SaveCorpCertification(corpCertification);
            return res;
        }

        //获取企业认证信息
        public async Task<CorpCertification> GetCorpCertification(string unionId)
        {
            var res = await _dal.GetCorpCertification(unionId);
            return res;
        }
    }
}
