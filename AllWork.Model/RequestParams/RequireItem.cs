namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 需求项（用于添加购物车、生成订单前比对可用库存）
    /// </summary>
    public class RequireItem
    {
        public string GoodsId { get; set; }
       
        public string ColorId { get; set; }
       
        public string SpecId { get; set; }
       
        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal Quantity { get; set; }

       
    }

    public class RequireItemExt : RequireItem
    {
        public string GoodsName { get; set; }
        public string ColorName { get; set; }
        public string SpecName { get; set; }
        public override string ToString()
        {
            //return base.ToString();
            return this.GoodsName + this.ColorName + this.SpecName;
        }
    }
}
