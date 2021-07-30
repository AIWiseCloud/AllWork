using AllWork.IServices.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        readonly IGoodsInfoOverviewServices _goodsInfoOverviewServices;
        public InventoryController(
         IGoodsInfoOverviewServices goodsInfoOverviewServices,
         IStockBillServices stockBillServices

         )
        {
            _goodsInfoOverviewServices = goodsInfoOverviewServices;
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
    }
}
