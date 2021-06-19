using System;
using System.Collections.Generic;
using System.Text;
using AllWork.IServices.Base;
using AllWork.Model.Sys;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IUserServices: IBaseServices<User>
    {
        Task<IEnumerable<User>> QueryUser(string userName, string password);

        bool IsValid(LoginRequestDTO req);
    }
}
