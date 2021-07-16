using AllWork.Nlog.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace AllWork.Web.Filter
{
    /// <summary>
    /// 异步版本自定义全局异常过滤器  
    /// </summary>
    public class CustomerGlobalExceptionFilterAsync : IAsyncExceptionFilter
    {

        private readonly INLogHelper _logHelper;

        public CustomerGlobalExceptionFilterAsync(INLogHelper logHelper)
        {
            _logHelper = logHelper;
        }

        /// <summary>
        /// 重新OnExceptionAsync方法
        /// </summary>
        /// <param name="context">异常信息</param>
        /// <returns></returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            // 如果异常没有被处理，则进行处理
            if (context.ExceptionHandled == false)
            {
                // 记录错误信息到日志
                _logHelper.LogError(context.Exception);

                //定义异常返回及格式 （这里的格式要与全局过滤器返回格式结合起来考虑 roy）
                context.Result = new ObjectResult(new
                {
                    code = 200,  //虽然是异常，也定义200，表示成功
                    msg = context.Exception.Message,
                    result = false,
                    returnStatus = 0 //0表示异常
                });

                // 设置为true，表示异常已经被处理了，其它捕获异常的地方就不会再处理了
                context.ExceptionHandled = true;
            }

            return Task.CompletedTask;
        }
    }
}
