using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Entities.Entities;
using StokKontrolProje.Service.Abstract;

namespace KD_16_StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> service;

        public CategoryController(IGenericService<Category> service)
        {
            this.service = service;
        }
        [HttpGet]
        public IActionResult TumKategorileriGetir()
        {
            return Ok(service.GetAll());
        }
        [HttpGet]
        public IActionResult AktifKategorileriGetir()
        {
            return Ok(service.GetActive());
        }
        [HttpGet("{id}")]
        public IActionResult IdyeGoreKategorileriGetir(int id)
        {
            return Ok(service.GetByID(id));
        }
        [HttpPost]
        public IActionResult KategoriEkle(Category category)
        {
            service.Add(category);
            return CreatedAtAction("IdyeGoreKategorileriGetir", new { id = category.ID }, category);
        }
        [HttpPut("{id}")]
        public IActionResult KategoriGuncelle(int id, Category category)
        {
            if (id != category.ID)
            {
                return BadRequest();
            }
            try
            {

                if (service.Update(category))
                    return Ok(category);
                else
                    return NotFound();

            }
            catch (DbUpdateConcurrencyException)
            {

                if (!CategoryExist(id))
                {
                    return NotFound();
                }
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult KategoriSil(int id)
        {
            var category = service.GetByID(id);
            if (category == null) return NotFound();
            try
            {
                service.Remove(category);
                return Ok("KategoriSilindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult KategoriAktiflestir(int id)
        {
            var category = service.GetByID(id);
            if (category is null) return NotFound();
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
        private bool CategoryExist(int id)
        {
            return service.Any(x => x.ID == id);
        }
    }
}
