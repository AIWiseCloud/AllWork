using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    /// <summary>
    /// 报表项目
    /// </summary>
    public class ReportItem 
    {
        /// <summary>
        /// 报表ID
        /// </summary>
        [Required(ErrorMessage ="报表ID不能为空")]
        public string ReportId
        { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        [Required(ErrorMessage = "模板名称不能为空")]
        public string TemplateName
        { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "报表标题不能为空")]
        public string Title
        { get; set; }

        /// <summary>
        /// 分组码
        /// </summary>
        [Required(ErrorMessage = "分组码不能为空")]
        public string GroupNo
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        { get; set; }

        /// <summary>
        /// 作废(0有效,1作废)
        /// </summary>
        public int IsCancellation
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int FIndex
        { get; set; }

        /// <summary>
        /// 数据来源(0: jsonFile, 1: database)
        /// </summary>
        public int SourceType
        { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public string DbConId
        { get; set; }

        /// <summary>
        /// 数据源SQL
        /// </summary>
        public string SourceSql
        { get; set; }

        /// <summary>
        /// 测试单号
        /// </summary>
        public string TestId
        { get; set; }
    }

}
