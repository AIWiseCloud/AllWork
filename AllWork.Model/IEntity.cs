using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model
{
    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface IEntity<TPrimarykey>
    {
        TPrimarykey Id { get; set; }
    }
}
