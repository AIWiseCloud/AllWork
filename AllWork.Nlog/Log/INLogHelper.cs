using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Nlog.Log
{
    /// <summary>
    /// 为使用依赖注入的方式，所以我们首先定义一个接口
    /// </summary>
    public interface INLogHelper
    {
        void LogError(Exception ex);
    }
}
