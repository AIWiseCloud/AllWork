using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AllWork.Web.Filter
{
    /// <summary>
    /// 返回统一格式参数  (这里的返回格式要与全局异常过滤器中的格式结合起来考虑 roy注)
    /// </summary>
    public class WebApiResultMiddleware : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult)
            {
                var objectResult = (ObjectResult)context.Result;
                if (objectResult.Value == null)
                {
                    context.Result = new ObjectResult(new { code = 400, msg = "未找到资源", returnStatus = 0 });
                }
                else
                {
                    if (objectResult.StatusCode == 400)
                    {
                        context.Result = new ObjectResult(new { code = 400, msg = objectResult.Value, result = false, returnStatus = 0 });
                    }
                    else
                    {
                        context.Result = new ObjectResult(new { code = objectResult.StatusCode, msg = "", result = objectResult.Value, returnStatus = 1 });
                    }
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new { code = 404,  msg = "未找到资源", returnStatus = 0 });
            }
            else if (context.Result is ContentResult)
            {
                context.Result = new ObjectResult(new { code = 200, msg = "", result = (context.Result as ContentResult).Content, returnStatus = 1 });
            }
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(new { code = (context.Result as StatusCodeResult).StatusCode, sub_msg = "", msg = "", returnStatus = 1 });
            }
        }
    }
}
