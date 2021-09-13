using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace AllWork.Nlog.Log
{
    public class NLogHelper : INLogHelper
    {
        //public static Logger logger { get; private set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<NLogHelper> _logger;

        public NLogHelper(IHttpContextAccessor httpContextAccessor, ILogger<NLogHelper> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public void LogError(Exception ex)
        {
            LogMessage logMessage = new LogMessage
            {
                IpAddress = _httpContextAccessor.HttpContext.Request.Host.Host,
                LogInfo = ex.InnerException != null ? ex.InnerException.Message : ex.Message,
                StackTrace = ex.StackTrace,
                OperationTime = DateTime.Now,
                OperationName = "admin"
            };
            _logger.LogError(LogFormat.ErrorFormat(logMessage));
        }
    }
}
