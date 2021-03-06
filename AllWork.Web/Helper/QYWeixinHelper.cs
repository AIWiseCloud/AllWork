using System.ComponentModel.DataAnnotations;

namespace AllWork.Web.Helper.WXWork
{
    /// <summary>
    /// 企业微信发送消息的基础消息内容
    /// </summary>
    class CorpSendBase
    {
        /// <summary>
        /// UserID列表（消息接收者，多个接收者用‘|’分隔）。特殊情况：指定为@all，则向关注该企业应用的全部成员发送
        /// </summary>
        public string touser { get; set; }
        
        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }
        /// <summary>
        /// 企业应用的id，整型。可在应用的设置页面查看
        /// </summary>
        public string agentid { get; set; }
        /// <summary>
        /// 表示是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string safe { get; set; }
        public CorpSendBase()
        {
            //this.agentid = System.Configuration.ConfigurationManager.AppSettings["CorpSendBaseAgentID"].ToString();
            this.safe = "0";
        }
    }

    class Markdown
    {
        public string content
        {
            get; set;
        }
    }

    class CorpSendMarkdown : CorpSendBase
    {
        //markdown消息必有属性
        public Markdown markdown 
        { get; set; }

        public CorpSendMarkdown(string content)
        {
            base.msgtype = "markdown";
            this.markdown = new Markdown
            {
                content = content
            };
        }
    }

    class Text
    {
        public string content
        { get; set; }

    }
    class CorpSendText : CorpSendBase
    {
        //文本消息必有属性
        public Text text
        { get; set; }

        public CorpSendText(string content)
        {
            base.msgtype = "text";
            this.text = new Text
            {
                content = content
            };
        }
    }

    class File
    {
        public string media_id { get; set; }
    }

    class CorpSendFile : CorpSendBase
    {
        public File file { get; set; }

        public CorpSendFile(string mediaId)
        {
            base.msgtype = "file";
            this.file = new File { media_id = mediaId };
        }
    }

    public class KVItem
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    /// <summary>
    /// 消息参数实体
    /// </summary>
    public class MsgEntity
    {
        private string msgType = "text";
        /// <summary>
        /// 指定接收消息的成员，成员ID列表（多个接收者用‘|’分隔，最多支持1000个）。特殊情况：指定为”@all”，则向该企业应用的全部成员发送
        /// </summary>
        public string ToUser { get; set; }

        /// <summary>
        /// 文件id，可以调用上传临时素材接口获取
        /// </summary>
        public string Media_Id { get; set; }

        /// <summary>
        /// 消息内容（发送text、markdown消息时的内容)
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息类型（官方支持多种，目前已对接的有text, markdown, file, miniprogram_notice)
        /// </summary>
        [Display(Name = "消息类型")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string MsgType
        {
            get { return msgType; }
            set { msgType = value; }
        }
    }
}
