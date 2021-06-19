using System;
using System.Collections.Generic;
using System.Text;
using AllWork.IRepository.Sys;
using System.Threading.Tasks;
using AllWork.Model.Sys;
using AllWork.Services.Base;
using AllWork.IServices.Sys;

namespace AllWork.Services.Sys
{
    public class UserServices : BaseServices<User>, IUserServices
    {
        readonly IUserRepository _dal;

        public UserServices(IUserRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<IEnumerable<User>> QueryUser(string userName, string password)
        {
            var result = await _dal.QueryList($"Select * from Sys_User Where UserName ='{userName}' and Password='{password}' ");

            return result;
        }

        //模拟测试，默认都是人为验证有效
        public bool IsValid(LoginRequestDTO req)
        {
            return true;
        }
    }
}
