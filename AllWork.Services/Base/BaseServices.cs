using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using System;

namespace AllWork.Services.Base
{
    public class BaseServices<TEntity> where TEntity:class,new()
    {
        public AllWork.IRepository.Base.IBaseRepository<TEntity> BaseDal;
    }
}
