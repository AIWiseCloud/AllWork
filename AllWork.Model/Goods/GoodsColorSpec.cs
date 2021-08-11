using System;

namespace AllWork.Model.Goods
{
    /// <summary>
    /// 此类基于视图，为减少多表查询时过多的表关联（如查询订单列表要显示颜色及规格名，分别要join两个表）
    /// </summary>
    public class GoodsColorSpec
    {
        public string SkuId
        { get; set; }

        public string GoodsId
        { get; set; }

        public string ColorId
        { get; set; }

        public string ColorName
        { get; set; }

        public string SpecId
        { get; set; }

        public string SpecName
        { get; set; }

        public decimal ActiveQuantity
        { get; set; }
    }

}
