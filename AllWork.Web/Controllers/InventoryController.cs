using AllWork.IServices.Goods;
using AllWork.IServices.Order;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        readonly IOrderServices _orderServices;
        public InventoryController(
         IInventoryServices inventoryServices,
         IStockBillServices stockBillServices,
         IOrderServices orderServices
         )
        {
            _inventoryServices = inventoryServices;
            _stockBillServices = stockBillServices;
            _orderServices = orderServices;
        }

        /// <summary>
        /// 保存出入库单据
        /// </summary>
        /// <param name="stockBill"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveStockBill(StockBill stockBill)
        {
            if (stockBill.StockBillDetail.Count == 0)
            {
                return BadRequest("未指定出入库明细");
            }
            //若是出库要检查是否会导致负结存
            if (_stockBillServices.IsOutStock(stockBill.TransTypeId))
            {
                var operResult = await _stockBillServices.CheckNegativeBalance(stockBill);
                if (!operResult.Status)
                {
                    return BadRequest(operResult.ErrorMsg);
                }
            }
            //业务类型为销售出库时的验证
            if (stockBill.TransTypeId == "transtype_sale")
            {
                if (stockBill.OrderId == 0)
                {
                    return BadRequest("订单号不正确!");
                }
                //同一订单是否生成过不同的销售出库单
                var existOthBill = await _stockBillServices.IsCreateOthBill(stockBill.BillId, stockBill.OrderId);
                if (existOthBill)
                {
                    return BadRequest("当前订单已生成过销售出库单！");
                }
                //订单实体
                var orderModel = await _orderServices.GetOrderInfo(stockBill.OrderId);
                if (orderModel == null)
                {
                    return BadRequest("订单不存在！");
                }
                if (orderModel.StatusId != 1)
                {
                    return BadRequest("只能针对待发货状态的订单拣货制作销售出库单");
                }
                //验证订单数量与出库数量是否一致
                foreach (var item in orderModel.OrderList)
                {
                    decimal qty = 0;
                    foreach (var detail in stockBill.StockBillDetail.FindAll(x => x.ColorId == item.ColorId && x.SpecId == item.SpecId))
                    {
                        qty = qty + detail.Quantity;
                    }
                    if (item.Quantity != qty)
                    {
                        return BadRequest($"{item.GoodsInfo.GoodsName}订单数量{item.Quantity},发货数量{qty},二者不一致");
                    }
                }

            }
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
            var stockBill = await _stockBillServices.GetStockBill(billId);
            var isOutBill = _stockBillServices.IsOutStock(stockBill.TransTypeId);
            //审核出库单或是反审核入库单要检查是否会导致负结存
            if ((isOutBill && isAudit == 1) || (!isOutBill && isAudit == 0))
            {
                var operResult = await _stockBillServices.CheckNegativeBalance(stockBill);
                if (!operResult.Status)
                {
                    return BadRequest(operResult.ErrorMsg);
                }
            }
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

        /// <summary>
        /// 比对商品需求数量与可用库存数量
        /// </summary>
        /// <param name="reuireItems"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ComparisonActiveQuantity(List<RequireItem> reuireItems)
        {
            var res = await _inventoryServices.ComparisonActiveQuantity(reuireItems);
            return Ok(res);
        }

        /// <summary>
        /// 获取SKU商品的可用库存
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="colorId"></param>
        /// <param name="specId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSKUActiveQuantity(string goodsId, string colorId, string specId)
        {
            var res = await _inventoryServices.GetSKUActiveQuantity(goodsId, colorId, specId);
            return Ok(res);
        }

        /// <summary>
        /// 获取指定商品在某仓库的实际库存
        /// </summary>
        /// <param name="goodsId">商品ID</param>
        /// <param name="colorId">颜色ID</param>
        /// <param name="specId">规格ID</param>
        /// <param name="stockNumber">仓库代码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInventoryDetail(string goodsId,string colorId, string specId, string stockNumber)
        {
            var res = await _stockBillServices.GetInventoryDetail(goodsId, colorId, specId, stockNumber);
            return Ok(res);
        }

        /// <summary>
        /// 获取待发货订单列表
        /// </summary>
        /// <param name="orderid">订单号(非必录)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetToBeShipped(string orderid="")
        {
            var res = await _stockBillServices.GetToBeShipped(orderid);
            return Ok(res);
        }
    }
}
