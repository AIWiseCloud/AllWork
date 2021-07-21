using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.Sys;
using AllWork.Services.Base;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class UserServices : BaseServices<UserInfo>, IUserServices
    {
        readonly IUserRepository _dal;

        //以依赖注入的形式使用_dal
        public UserServices(IUserRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        //获取用户信息
        public async Task<UserInfo> GetUserInfo(string unionId)
        {
            var res = await _dal.GetUserInfo(unionId);
            return res;
        }

        //验证是否为有效用户
        public async Task<bool> IsValidUser(LoginRequestDTO req)
        {
            var res = await _dal.IsValidUser(req);
            return res;
        }

        //保存用户信息
        public async Task<bool> SaveUserInfo(UserInfo userInfo)
        {
            var res = await _dal.SaveUserInfo(userInfo);
            return res;
        }
    }
}
