using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.RequestParams
{
    public class ProjectCaseParams:CommonParams
    {
        /// <summary>
        /// 场地类型
        /// </summary>
        public string SiteCategory
        { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Location
        { get; set; }
    }
}
