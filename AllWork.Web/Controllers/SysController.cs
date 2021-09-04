using AllWork.Common;
using AllWork.IServices.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class SysController : BaseController
    {
        readonly IResourceSettingsServices _resourceSettingsServices;
        readonly ISubMesTypeServices _subMesTypeServices;
        readonly ISubMessageServices _subMessageServices;
        public SysController(
            IResourceSettingsServices resourceSettingsServices,
            ISubMesTypeServices subMesTypeServices,
            ISubMessageServices subMessageServices
            )
        {
            _resourceSettingsServices = resourceSettingsServices;
            _subMesTypeServices = subMesTypeServices;
            _subMessageServices = subMessageServices;
        }

        /// <summary>
        /// 保存资源设置
        /// </summary>
        /// <param name="resourceSettings"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveResourceSettings(ResourceSettings resourceSettings)
        {
            var res = await _resourceSettingsServices.SaveResourceSettings(resourceSettings);
            return Ok(res);
        }

        /// <summary>
        /// 获取单个资源配置
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResourceSettings(string sourceId)
        {
            var res = await _resourceSettingsServices.GetResourceSettings(sourceId);
            return Ok(res);
        }

        /// <summary>
        /// 按分组获取资源配置
        /// </summary>
        /// <param name="groupNo">分组码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResourceSettingsByGroup(string groupNo)
        {
            var res = await _resourceSettingsServices.GetResourceSettingsByGroup(groupNo);
            return Ok(res);
        }

        /// <summary>
        /// 获取资源所有分组码（用于资源调用参考)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            var res = await _resourceSettingsServices.GetGroups();
            return Ok(res);
        }

        /// <summary>
        /// 删除资源配置
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteResourceSettings(string sourceId)
        {
            var res = await _resourceSettingsServices.DeleteResourceSettings(sourceId);
            return Ok(res);
        }

        /// <summary>
        /// 查询资源配置
        /// </summary>
        /// <param name="resourceParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryResourceSettings(CommonParams resourceParams)
        {
            var res = await _resourceSettingsServices.QueryResourceSettings(resourceParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// 保存辅助资料分类
        /// </summary>
        /// <param name="subMesType"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveSubMesType(SubMesType subMesType)
        {
            var res = await _subMesTypeServices.SaveSubMesType(subMesType);
            return Ok(res);
        }

        /// <summary>
        /// 获取辅助资料分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubMesType(string id)
        {
            var res = await _subMesTypeServices.GetSubMesType(id);
            return Ok(res);
        }

        /// <summary>
        /// 获取辅助资料所有分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubMesTypes()
        {
            var res = await _subMesTypeServices.GetSubMesTypes();
            return Ok(res);
        }

        /// <summary>
        /// 删除辅助资料分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteSubMesType(string id)
        {
            var res = await _subMesTypeServices.DeleteSubMesType(id);
            return Ok(res);
        }

        /// <summary>
        /// 保存辅助资料项目
        /// </summary>
        /// <param name="subMessage"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveSubMessage(SubMessage subMessage)
        {
            var res = await _subMessageServices.SaveSubmessage(subMessage);
            return Ok(res);
        }

        /// <summary>
        /// 获取辅助资料项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubMessage(string id)
        {
            var res = await _subMessageServices.GetSubMessage(id);
            return Ok(res);
        }

        /// <summary>
        /// 获取指定分类下的辅助资料列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSubMessageList(string parentId)
        {
            var res = await _subMessageServices.GetSubMessageList(parentId);
            return Ok(res);
        }

        /// <summary>
        /// 删除辅助资料项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteSubMessage(string id)
        {
            var res = await _subMessageServices.DeleteSubMessage(id);
            return Ok(res);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Encrypt(string text)
        {
            var res= DesEncrypt.Encrypt(text);
            return Ok(res);
        }
}
}
