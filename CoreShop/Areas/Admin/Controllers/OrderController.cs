using CoreShop.Areas.Models;
using CoreShop.CORE.Service;
using CoreShop.MODEL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ICoreService<Order> _orderService;
        private readonly ICoreService<OrderDetail> _orderDetailService;

        public OrderController(
            ICoreService<Order> orderService,
            ICoreService<OrderDetail> orderDetailService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }

        public IActionResult List(int? searchId)
        {
            var orders = _orderService.GetAll().OrderByDescending(x => x.ID).ToList();

            if (searchId.HasValue)
            {
                orders = orders.Where(x => x.ID == searchId.Value).ToList();
            }

            return View(orders);
        }

        public IActionResult Detail(int id)
        {
            var order = _orderService.GetById(id);

            if (order == null)
                return NotFound();

            var details = _orderDetailService.GetAll()
                .Where(x => x.OrderId == id)
                .ToList();

            var model = new AdminOrderDetailVM
            {
                Order = order,
                OrderDetails = details
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _orderService.GetById(id);

            if (order == null)
                return RedirectToAction("List");

            if (order.Status == "Teslim Edildi" || order.Status == "Cancelled")
                return RedirectToAction("List");

            order.Status = status;
            _orderService.Update(order);

            return RedirectToAction("List");
        }
    }
}