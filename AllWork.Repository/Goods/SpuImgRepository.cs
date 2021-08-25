using AllWork.IRepository.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class SpuImgRepository : Base.BaseRepository<SpuImg>, ISpuImgRepository
    {
        public async Task<OperResult> SaveSpuImg(SpuImg spuImg)
        {
            var operResult = new OperResult();
            var instance = await base.QueryFirst("Select * from SpuImg Where ID = @ID", new { spuImg.ID });
            if (instance==null)
            {
                spuImg.ID = System.Guid.NewGuid().ToString();
                var insertSql = "Insert SpuImg (ID,GoodsId,FIndex,ImgUrl)values(@ID,@GoodsId,@FIndex,@ImgUrl)";
                operResult.Status = await base.Execute(insertSql, spuImg) > 0;
            }
            else
            {
                var updateSql = "Update SpuImg set ID = @ID,GoodsId = @GoodsId,FIndex = @FIndex,ImgUrl = @ImgUrl Where ID = @ID";
                operResult.Status = await base.Execute(updateSql, spuImg) > 0;
            }
            operResult.IdentityKey = spuImg.ID;
            return operResult;
        }

        public async Task<SpuImg> GetSpuImg(string id)
        {
            var res = await base.QueryFirst("Select * from SpuImg Where ID = @ID", new { ID = id });
            return res;
        }

        public async Task<IEnumerable<SpuImg>> GetSpuImgs(string goodsId)
        {
            var res = await base.QueryList("Select * from SpuImg Where GoodsId = @GoodsId", new { GoodsId = goodsId });
            return res;
        }

        public async Task<bool> DeleteSpuImg(string id)
        {
            var res = await base.Execute("Delete from SpuImg Where ID = @ID", new { ID = id }) > 0;
            return res;
        }
    }
}
