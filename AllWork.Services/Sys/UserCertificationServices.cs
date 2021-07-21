using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.Sys;
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
    }
}
