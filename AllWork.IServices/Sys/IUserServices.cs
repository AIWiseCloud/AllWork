﻿using AllWork.IServices.Base;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using AllWork.Model.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IUserServices : IBaseServices<UserInfo>
    {
        Task<UserInfo> GetUserInfo(string unionId);

        Task<bool> IsValidUser(LoginRequestDTO req);

        Task<bool> SaveUserInfo(UserInfo userInfo);

        Task<bool> Logout(string unionId);

        Task<bool> Logoff(string unionId);

        Task<Tuple<IEnumerable<UserInfo>, int>> QueryUsers(UserParams userParams);
    }
}
