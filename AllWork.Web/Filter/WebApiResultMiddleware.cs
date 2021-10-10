using AllWork.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AllWork.Web.Filter
{
    /// <summary>
    /// 返回统一格式参数  (这里的返回格式要与全局异常过滤器中的格式结合起来考虑 roy注)
    /// </summary>
    public class WebApiResultMiddleware : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //var name = context.ActionDescriptor.AttributeRouteInfo.Name; //2021-10-10个别action忽略全局过滤器，将route的name标识为ignore开头（不能重名）
            //if (!string.IsNullOrEmpty(name) && name.StartsWith("ignore")) return; //暂时用这个方法，以后有好方法再作改善

            if (context.Result is ObjectResult objectResult)//写法：使用模式匹配来避免后跟强制转换的“is”检查
            {
                if (objectResult.Value == null)
                {
                    context.Result = new ObjectResult(new { code = 200, msg = "未找到资源", returnStatus = 0 });
                }
                else
                {
                    if (objectResult.StatusCode == 400)
                    {
                        context.Result = new ObjectResult(new { code = 400, msg = objectResult.Value, result = false, returnStatus = 0 });
                    }
                    else
                    {
                        if (objectResult.Value is OperResult)
                        {
                            var value = objectResult.Value as OperResult;
                            if (value.Status)
                                context.Result = new ObjectResult(new { code = objectResult.StatusCode, msg = "", result = value, returnStatus = 1 });
                            else
                                context.Result = new ObjectResult(new { code = objectResult.StatusCode, msg = value.ErrorMsg, result = false, returnStatus = 0 });
                        }
                        else
                        {
                            context.Result = new ObjectResult(new { code = objectResult.StatusCode, msg = "", result = objectResult.Value, returnStatus = 1 });
                        }
                    }
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new { code = 404, msg = "未找到资源", returnStatus = 0 });
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

    public class SkipMyGlobalActionFilterAttribute : Attribute
    {
    }
}
