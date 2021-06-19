using System;
using System.Collections.Generic;
using System.Text;
using AllWork.IRepository.Base;
using AllWork.IRepository.Sys;
using AllWork.Model.Sys;
using AllWork.Repository.Base;
using Microsoft.Extensions.Configuration;

namespace AllWork.Repository.Sys
{
    public class UserRepository:BaseRepository<User>,IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
