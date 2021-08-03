using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 库存
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        readonly IStockBillServices _stockBillServices;
        readonly IInventoryServices _inventoryServices;
        readonly IGoodsInfoOverviewServices _goodsInfoOverviewServices;
        public InventoryController(
         IGoodsInfoOverviewServices goodsInfoOverviewServices,
         IInventoryServices inventoryServices,
         IStockBillServices stockBillServices

         )
        {
            _goodsInfoOverviewServices = goodsInfoOverviewServices;
            _inventoryServices = inventoryServices;
            _stockBillServices = stockBillServices;
        }

        /// <summary>
        /// 由商品最小分类获取商品列表(SPU列表)
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsInfoOverviews(string categoryId)
        {
            var res = await _goodsInfoOverviewServices.GetGoodsInfoOverviews(categoryId);
            return Ok(res);
        }

        /// <summary>
        /// 保存出入库单据
        /// </summary>
        /// <param name="stockBill"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveStockBill(StockBill stockBill)
        {
            var res = await _stockBillServices.SaveStockBill(stockBill);
            return Ok(res);
        }

        /// <summary>
        /// 获取出入库单据信息
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStockBill(string billId)
        {
            var res = await _stockBillServices.GetStockBill(billId);
            return Ok(res);
        }

        /// <summary>
        /// 分页查询出入库单据
        /// </summary>
        /// <param name="stockBillParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SearchStockBill(StockBillParams stockBillParams)
        {
            var res = await _stockBillServices.SearchStockBill(stockBillParams);

            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// 删除出入库单据某行
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteStockBillRow(string id)
        {
            var res = await _stockBillServices.DeleteStockBillRow(id);
            return Ok(res);
        }

        /// <summary>
        /// 删除出入库单据
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteStockBill(string billId)
        {
            var res = await _stockBillServices.DeleteStockBill(billId);
            return Ok(res);
        }

        /// <summary>
        /// 审核出入库单据
        /// </summary>
        /// <param name="billId">单据编号</param>
        /// <param name="isAudit">审核标记(1审核,0反审)</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AuditStockBill(string billId, int isAudit)
        {
            var res = await _stockBillServices.AuditStockBill(billId, isAudit);
            return Ok(res);
        }

        /// <summary>
        /// 获取某商品(SPU)下的库存(SKU)列表
        /// </summary>
        /// <param name="goodsId">商品ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInventories(string goodsId)
        {
            var res = await _inventoryServices.GetInventories(goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="inventoryParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SearchInventories(InventoryParams inventoryParams)
        {
            var res = await _inventoryServices.SearchInventories(inventoryParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }
    }
}
