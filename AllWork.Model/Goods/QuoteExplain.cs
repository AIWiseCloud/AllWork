using System;

namespace AllWork.Model.Goods
{
    public class QuoteExplain
    {
        public string ID
        { get; set; }

        public DateTime? UpdateDate
        { get; set; }

        public DateTime? EffectiveDate
        { get; set; }

        public string QuoteDesc
        { get; set; }

        public string Remark
        { get; set; }

        public string Creator
        { get; set; }
    }
}
