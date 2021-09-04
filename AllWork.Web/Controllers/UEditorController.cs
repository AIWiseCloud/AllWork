using Microsoft.AspNetCore.Mvc;
using UEditorNetCore;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 百度编辑器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UEditorController : ControllerBase
    {
        readonly UEditorService _uEditorService;
        public UEditorController(UEditorService uEditorService)
        {
            _uEditorService = uEditorService;
        }

        /// <summary>
        /// Do
        /// </summary>
        [HttpGet,HttpPost]
        public void Do()
        {
            _uEditorService.DoAction(HttpContext);
        }
    }
}
