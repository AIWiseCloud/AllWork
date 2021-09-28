using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsInfoRepository : Base.BaseRepository<GoodsInfo>, IGoodsInfoRepository
    {
        public async Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo)
        {
            var instance = await base.QueryFirst("Select * from GoodsInfo Where GoodsId = @GoodsId", new { goodsInfo.GoodsId });
            string sql;
            if (instance == null)
            {
                goodsInfo.IsUnder = 1; //默认是下架 （要上架需要一个发布动作)
                sql = "Insert GoodsInfo (GoodsId,CategoryId,ProdNumber,GoodsName, Brand ,Mixture, GoodsDesc,UnitName,BaseUnitPrice,SalesTimes,IsRecommend,IsNew,IsUnder,Creator)values(@GoodsId,@CategoryId,@ProdNumber,@GoodsName, @Brand,@Mixture, @GoodsDesc,@UnitName, @BaseUnitPrice,@SalesTimes,@IsRecommend,@IsNew,@IsUnder,@Creator)";
            }
            else
            {
                sql = "Update GoodsInfo set CategoryId = @CategoryId,ProdNumber = @ProdNumber,GoodsName = @GoodsName, Brand = @Brand, Mixture = @Mixture,  GoodsDesc = @GoodsDesc,UnitName=@UnitName, BaseUnitPrice = @BaseUnitPrice,SalesTimes = @SalesTimes,IsRecommend = @IsRecommend,IsNew = @IsNew,IsUnder = @IsUnder,Creator = @Creator Where GoodsId = @GoodsId";
            }
            return await base.Execute(sql, goodsInfo) > 0;
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="goodsId">商品ID</param>
        /// <returns></returns>
        public async Task<GoodsInfoExt> GetGoodsInfo(string goodsId)
        {
            Dictionary<string, GoodsInfoExt> pairs = new Dictionary<string, GoodsInfoExt>();
            var sql = @"Select a.*,'' as id1, b.*, '' as id2,  d.*, '' as id3,  f.*
from GoodsInfo a left join GoodsSpec b
on a.GoodsId = b.GoodsId
left join GoodsColor d on d.GoodsId = a.GoodsId
left join SpuImg f on a.GoodsId = f.GoodsId
where a.GoodsId = @GoodsId";

            var res = await base.QueryAsync<GoodsInfoExt, GoodsSpec,  GoodsColor,  SpuImg>(sql, (goodsInfo, goodsSpec, goodsColor, img) =>
             {
                 if (!pairs.TryGetValue(goodsInfo.GoodsId, out GoodsInfoExt tempgoods))
                 {
                     tempgoods = goodsInfo;
                     pairs.Add(tempgoods.GoodsId, tempgoods);
                 }
                 GoodsSpec tempSpec = tempgoods.GoodsSpecs.Find(list => list.ID == goodsSpec.ID);
                 if (tempSpec == null)
                 {
                     //子表信息
                     tempSpec = goodsSpec;
                     //子表中的规格信息
                     //goodsSpec.SpecId = subMessage.FNumber;//不能少
                     tempgoods.GoodsSpecs.Add(tempSpec);
                 }
                 GoodsColor tempColor = tempgoods.GoodsColors.Find(x => x.ID == goodsColor.ID);
                 if (tempColor == null)
                 {
                     tempColor = goodsColor;
                     tempgoods.GoodsColors.Add(tempColor);
                 }
                 SpuImg tempImg = tempgoods.SpuImgs.Find(x => x.ID == img.ID);
                 if (tempImg == null)
                 {
                     tempImg = img;
                     tempgoods.SpuImgs.Add(tempImg);
                 }
                 return goodsInfo;
             }, new { GoodsId = goodsId }, "id1, id2, id3");
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
            sql.Append(" Delete from GoodsSpec Where GoodsId = @GoodsId; ");
            sql.Append(" Delete from SpuImg Where GoodsId = @GoodsId;");
            sql.Append(" Delete from GoodsInfo Where GoodsId = @GoodsId");
            return await base.Execute(sql.ToString(), new { GoodsId = goodsId }) > 0;
        }

        public async Task<bool> ExistSKU(string goodsId)
        {
            var sql = "Select count(*) from Inventory Where GoodsId = @GoodsId";
            var res = await base.ExecuteScalar<int>(sql, new { GoodsId = goodsId });
            return res > 0;
        }

        public async Task<bool> ExistOrders(string goodsId)
        {
            var sql = "select Count(*) from OrderList where GoodsId = @GoodsId";
            var res = await base.ExecuteScalar<int>(sql, new { GoodsId = goodsId });
            return res > 0;
        }

        //发布（上架）或取消发布（下架）商品
        public async Task<bool> ReleaseGoods(bool isRelease, string goodsId)
        {
            var sql = "Update GoodsInfo set IsUnder = @IsUnder Where GoodsId = @GoodsId";
            var res = await base.Execute(sql, new { GoodsId = goodsId, IsUnder = isRelease ? 0 : 1 });
            return res > 0;
        }

        //商品分页查询  (查询方案queryScheme：0关键字查询 1商品分类 2推荐商品 3最新商品)
        public async Task<Tuple<IEnumerable<GoodsInfoExt>, int>> QueryGoods(GoodsQueryParams goodsQueryParams)
        {
            //(1) sql公共部分
            var sqlpub = new StringBuilder();
            // 如果是按商品类别查询，通过with语句查出所有相关类别（目前直接3级）
            if (goodsQueryParams.QueryScheme == 1)
            {
                sqlpub.Append(@"with tab as(
select CategoryId 
from goodsCategory g 
where CategoryId  = @CategoryId or ParentId = @CategoryId
)
,tab2 as(select CategoryId 
from GoodsCategory
where CategoryId in (select * from tab) or ParentId in (select * from tab) )");
            };
            //公共语句主体
            sqlpub.Append(@" select {0} 
from GoodsInfo a 
left join (select GoodsId, max(ID)as ID from GoodsColor group by GoodsId) c
on a.GoodsId = c.GoodsId
left join GoodsColor t1 on c.ID = t1.ID
left join (select GoodsId, max(ID) as ID from GoodsSpec group by GoodsId) s
on a.GoodsId = s.GoodsId
left join GoodsSpec t2 on t2.ID = s.ID  Where (1 = 1) ");
            //如果是按关键字搜索
            if (goodsQueryParams.QueryScheme == 0)
            {
                if (!string.IsNullOrEmpty(goodsQueryParams.QueryValue))
                {
                    sqlpub.AppendFormat(" and (a.GoodsName like '%{0}%' or a.GoodsId = @GoodsId or GoodsDesc like '%{0}%' or ProdNumber = @ProdNumber or a.Brand like '%{0}%') or CategoryId = @CategoryId", goodsQueryParams.QueryValue);
                }
            }
            // 如果是方案1，按商品分类查询，要把上面with中查出的类别在条件中体现
            if (goodsQueryParams.QueryScheme == 1)
            {
                sqlpub.Append(" and CategoryId in (select * from tab2) ");
            }
            //如果是查询推荐商品
            if (goodsQueryParams.QueryScheme == 2)
            {
                sqlpub.Append(" and IsRecommend = 1 ");
            }
            //如果是查最新商品
            if (goodsQueryParams.QueryScheme == 3)
            {
                sqlpub.Append(" and IsNew = 1 ");
            }
            //如果要隐藏下架商品
            if (goodsQueryParams.HideUnderGoods == 1)
            {
                sqlpub.Append(" and IsUnder = 0 ");
            }
            //(2) 排序sql
            string sqlorder = string.Empty;
            if (!string.IsNullOrEmpty(goodsQueryParams.PageModel.OrderField))
            {
                sqlorder = string.Format(" order by {0} {1} ", goodsQueryParams.PageModel.OrderField, goodsQueryParams.PageModel.OrderWay);
            }
            //(3) sql语句1（求总记录数）
            var sql1 = string.Format(sqlpub.ToString(), "count(a.GoodsId) as TotalCount");
            //sql语句2（求分页数据）
            var sql2 = string.Format(sqlpub.ToString(), " a.*,'' as id1, t1.*,'' as id2, t2.* ") + sqlorder + " limit @Skip, @PageSize ";
            //完整sql
            var sql = sql1 + ";" + sql2;
            var res = await base.QueryPagination<GoodsInfoExt, GoodsColor, GoodsSpec>(sql, (gi, gc, gs) =>
            {
                gi.GoodsColors.Add(gc);
                gi.GoodsSpecs.Add(gs);
                return gi;
            }, new
            {
                CategoryId = goodsQueryParams.QueryValue,
                GoodsId = goodsQueryParams.QueryValue,
                ProdNumber = goodsQueryParams.QueryValue,
                goodsQueryParams.PageModel.Skip,
                goodsQueryParams.PageModel.PageSize,
            }, "id1, id2");
            return res;
        }

        public async Task<IEnumerable<GoodsInfo>> GetGoodsList(string categoryId)
        {
            var sql = $"select GoodsId, GoodsName, UnitName, BaseUnitPrice, Mixture, CategoryId,Brand from GoodsInfo where CategoryId like '{categoryId}%' and IsUnder = 0";
            var res = await base.QueryList(sql);
            return res;
        }

        public async Task<Tuple<List<QuoteExplain>, List<GoodsQuote>>> GetGoodsQuotes()
        {
            var sql = "Select * from QuoteExplain;Select * from GoodsPivotView order by CategoryId ,GoodsName ";
            var res = await base.QueryMultiple<QuoteExplain, GoodsQuote>(sql);
            
            return res;
        }

        public async Task<int> UpdateQuoteExplain(QuoteExplain quoteExplain)
        {
            var instance = await base.QueryFirst<QuoteExplain>("Select * from QuoteExplain limit 1");
            string sql;
            if (instance == null)
            {
                sql = "Insert QuoteExplain (ID,UpdateDate,EffectiveDate,QuoteDesc,Remark,Creator)values(@ID,@UpdateDate,@EffectiveDate,@QuoteDesc,@Remark,@Creator)";
            }
            else
            {
                sql = "Update QuoteExplain set UpdateDate = current_timestamp,EffectiveDate = @EffectiveDate,QuoteDesc = @QuoteDesc,Remark = @Remark,Creator = @Creator Where ID = @ID";
                quoteExplain.ID = instance.ID;
            }
            var res = await base.Execute(sql, quoteExplain);
            return res;

        }

    }
}
