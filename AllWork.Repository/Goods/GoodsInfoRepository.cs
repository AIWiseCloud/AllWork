using AllWork.IRepository.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsInfoRepository : Base.BaseRepository<GoodsInfo>, IGoodsInfoRepository
    {
        public GoodsInfoRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo)
        {
            var instance = await base.QueryFirst("Select * from GoodsInfo Where GoodsId = @GoodsId", new { GoodsId = goodsInfo.GoodsId });
            if (instance == null)
            {
                var insertSql = "Insert GoodsInfo (GoodsId,CategoryId,ProdNumber,GoodsName,GoodsDesc,UnitName,SalesTimes,IsRecommend,IsNew,IsUnder,Creator)values(@GoodsId,@CategoryId,@ProdNumber,@GoodsName,@GoodsDesc,@UnitName,@SalesTimes,@IsRecommend,@IsNew,@IsUnder,@Creator)";
                return await base.Execute(insertSql, goodsInfo) > 0;
            }
            else
            {
                var updateSql = "Update GoodsInfo set CategoryId = @CategoryId,ProdNumber = @ProdNumber,GoodsName = @GoodsName,GoodsDesc = @GoodsDesc,UnitName=@UnitName,SalesTimes = @SalesTimes,IsRecommend = @IsRecommend,IsNew = @IsNew,IsUnder = @IsUnder,Creator = @Creator Where GoodsId = @GoodsId";
                return await base.Execute(updateSql, goodsInfo) > 0;
            }
        }


        public async Task<GoodsInfo> GetGoodsInfo(string goodsId)
        {
            Dictionary<string, GoodsInfo> pairs = new Dictionary<string, GoodsInfo>();
            var sql = @"Select a.*,'' as pid, b.*, '' as pid, c.* ,'' as pid, d.*, '' as pid, e.*
from GoodsInfo a left join GoodsSpec b
on a.GoodsId = b.GoodsId
left join SubMessage c
on c.ParentId = 'pdspec' and b.SpecId = c.FNumber
left join GoodsColor d
on d.GoodsId = a.GoodsId
left join SubMessage e
on e.ParentId = 'pdcolor' and d.ColorId = e.FNumber
where a.GoodsId = @GoodsId";
            //注：split
            var res = await base.QueryAsync<GoodsInfo, GoodsSpec, SubMessage,GoodsColor, SubMessage>(sql, (goodsInfo, goodsSpec, subMessage, goodsColor, subMessage2) =>
            {
                GoodsInfo tempgoods;
                if (!pairs.TryGetValue(goodsInfo.GoodsId, out tempgoods))
                {
                    tempgoods = goodsInfo;
                    pairs.Add(tempgoods.GoodsId, tempgoods);
                }
                GoodsSpec tempSpec = tempgoods.GoodsSpecs.Find(list => list.GoodsId == goodsSpec.GoodsId && list.SpecId == goodsSpec.SpecId);
                if (tempSpec == null)
                {
                    //子表信息
                    tempSpec = goodsSpec;
                    //子表中的规格信息
                    //goodsSpec.SpecId = subMessage.FNumber;//不能少
                    goodsSpec.Spec = subMessage;
                    tempgoods.GoodsSpecs.Add(tempSpec);
                }
                GoodsColor tempColor = tempgoods.GoodsColors.Find(x => x.GoodsId == goodsColor.GoodsId && x.ColorId == goodsColor.ColorId);
                if (tempColor == null)
                {
                    tempColor = goodsColor;
                    goodsColor.ColorInfo = subMessage2;
                    tempgoods.GoodsColors.Add(tempColor);
                }
                return goodsInfo;
            }, new { GoodsId = goodsId }, "pid, pid, pid, pid");
            return pairs.Values.Count > 0 ? pairs[goodsId] : null;
        }

        /// <summary>
        /// 删除商品信息（包括颜色与规格设置）
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteGoodsInfo(string goodsId)
        {
            var sql = new StringBuilder("Delete from GoodsColor Where GoodsId = @GoodsId;");
            sql.Append("Delete from GoodsSpec Where GoodsId = @GoodsId; ");
            sql.Append("Delete from SpuImg Where GoodsId = @GoodsId");
            sql.Append("Delete from GoodsInfo Where GoodsId = @GoodsId");
            return await base.Execute(sql.ToString(), new { GoodsId = goodsId }) > 0;
        }

        public async Task<Tuple<IEnumerable<GoodsInfo>, int>> SearchGoods(string keywords, PageModel pageModel)
        {
            //条件语句
            var wheresql = new StringBuilder(string.Format(" Where GoodsName like '%{0}%' or GoodsId = '{0}' or GoodsName like '%{0}%' ", keywords));
            if (!string.IsNullOrEmpty(pageModel.OrderField))
            {
                wheresql.Append(" order by @OrderField @OrderWay");
            }
            wheresql.Append(" limit @Skip,@PageSize");
            //sql语句
            var sql = $"Select count(*) from GoodsInfo {wheresql}; Select * from GoodsInfo {wheresql}";
            var options = new
            {
                OrderField = pageModel.OrderField,
                Skip = pageModel.Skip,
                PageSize = pageModel.PageSize,
                OrderWay = pageModel.OrderWay
            };
            var res = await base.QueryPagination(sql, options);
            return res;
        }

    }
}
