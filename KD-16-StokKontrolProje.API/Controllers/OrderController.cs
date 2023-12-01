using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StokKontrolProje.Entities;
using StokKontrolProje.Entities.Entities;
using StokKontrolProje.Entities.Enums;
using StokKontrolProje.Service.Abstract;

namespace KD_16_StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> orderService;
        private readonly IGenericService<OrderDetails> odService;
        private readonly IGenericService<Product> productService;

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderDetails> odService, IGenericService<Product> productService)
        {
            this.orderService = orderService;
            this.odService = odService;
            this.productService = productService;
        }
        [HttpGet]
        public IActionResult TumSiparisleriGetir()
        {
            return Ok(orderService.GetAll(x => x.OrderDetails, y => y.User));
        }
        [HttpGet]
        public IActionResult AktifSiparisleriGetir()
        {
            return Ok(orderService.GetActive());
        }
        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisleriGetir(int id)
        {
            return Ok(orderService.GetByID(id, x => x.OrderDetails, y => y.User));
        }
        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisDetaylariniGetir(int id)
        {
            return Ok(odService.GetAll(x => x.OrderId == id, y => y.Product));
        }
        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir()
        {
            return Ok(orderService.GetDefault(x => x.Status == Status.Pending).ToList());
        }
        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir()
        {
            return Ok(orderService.GetDefault(x => x.Status == Status.Confirmed).ToList());
        }
        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir()
        {
            return Ok(orderService.GetDefault(x => x.Status == Status.Cancelled).ToList());
        }
        [HttpPost]
        public IActionResult SiparisEkle(int userID, [FromQuery] int[] productIds, [FromQuery] short[] quantities)
        {
            Order yeniSiparis = new Order();
            yeniSiparis.UserID = userID;
            yeniSiparis.Status = Status.Pending;
            yeniSiparis.IsActive = true;
            orderService.Add(yeniSiparis);
            for (int i = 0; i < productIds.Length; i++)
            {
                OrderDetails yeniDetay = new OrderDetails();
                yeniDetay.OrderId = yeniSiparis.ID;
                yeniDetay.ProductId = productIds[i];
                yeniDetay.Quantity = quantities[i];
                yeniDetay.UnitPrice = productService.GetByID(productIds[i]).UnitPrice;
                yeniDetay.IsActive = true;
                odService.Add(yeniDetay);
            }
            return Ok(yeniSiparis);
        }
        [HttpGet("{id}")]
        public IActionResult SiparisiOnayla(int id)
        {
            Order order = orderService.GetByID(id);
            if (order is null) return NotFound();
            else
            {
                List<OrderDetails> detaylar = odService.GetDefault(x => x.OrderId == order.ID).ToList();

                foreach (OrderDetails item in detaylar)
                {
                    Product productInOrder = productService.GetByID(item.ProductId);
                    if (productInOrder.Stock >= item.Quantity)
                    {
                        productInOrder.Stock -= item.Quantity;
                        productService.Update(productInOrder);
                    }
                    else
                        return BadRequest();

                }
                order.Status = Status.Confirmed;
                order.IsActive = false;
                orderService.Update(order);
                return Ok(order);
            }
        }
        [HttpGet("{id}")]
        public IActionResult SiparisiReddet(int id)
        {
            Order cancelledOrder = orderService.GetByID(id);
            if (cancelledOrder is null) return NotFound();
            else
            {
                cancelledOrder.Status = Status.Cancelled;
                cancelledOrder.IsActive = false;
                orderService.Update(cancelledOrder);
                return Ok(cancelledOrder);
            }
        }
    }
}
