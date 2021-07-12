using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class Settings
    {
        [Display(Name ="编号")]
        [Required(ErrorMessage ="编号不能为空")]
        public string ID { get; set; }

        [Display(Name ="维护中")]
        public int IsMaintain { get; set; }

        /// <summary>
        /// 轮插图1
        /// </summary>
        public string ImgUrl1 { get; set; }

        public string Nav1 { get; set; }

        public string ImgUrl2 { get; set; }

        public string Nav2 { get; set; }

        public string ImgUrl3 { get; set; }

        public string Nav3 { get; set; }

        public string Notication { get; set; }

        public int ShowNotice { get; set; }

    }
}
