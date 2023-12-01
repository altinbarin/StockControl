using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Entities;
using StokKontrolProje.Service.Abstract;

namespace KD_16_StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> service;

        public ProductController(IGenericService<Product> service)
        {
            this.service = service;
        }
        [HttpGet]
        public IActionResult TumUrunleriGetir() {

            return Ok(service.GetAll(x => x.Category, y => y.Supplier));
        }
        [HttpGet]
        public IActionResult AktifUrunleriGetir()
        {
            return Ok(service.GetActive(x => x.Category, y => y.Supplier));

        }
        [HttpGet("{id}")]
        public IActionResult IdyeGoreUrunGetir(int id)
        {
            return Ok(service.GetByID(id));
        }
        [HttpPost]
        public IActionResult UrunEkle(Product product)
        {
            service.Add(product);
            return CreatedAtAction("IdyeGoreUrunGetir", new { id = product.ID }, product);
        }
        [HttpPut("{id}")]
        public IActionResult UrunGuncelle(int id,Product product)
        {
            if (id != product.ID) return BadRequest();
            try
            {
                if (service.Update(product))
                {
                    return Ok(product);
                }
                else
                    return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!ProductExist(id))
                {
                    return NotFound();
                }
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult UrunSil(int id)
        {
            Product p = service.GetByID(id);
            if (p is null) return NotFound();
            try
            {
                service.Remove(p);
                return Ok("Ürün silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult UrunAktiflestir(int id)
        {
            Product p = service.GetByID(id);
            if(p is null) return NotFound();
            try
            {
                service.Activate(id);
                return Ok(service.GetByID(id));
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        private bool ProductExist(int id)
        {
            return service.Any(x => x.ID == id);
        }
    }
}
