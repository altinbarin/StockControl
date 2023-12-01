using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StokKontrolProje.Entities.Entities;
using StokKontrolProje.Service.Abstract;

namespace KD_16_StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> service;

        public SupplierController(IGenericService<Supplier> service)
        {
            this.service = service;
        }
        [HttpGet]
        public IActionResult TumTedarikcileriGetir()
        {
            return Ok(service.GetAll());
        }
        [HttpGet]
        public IActionResult AktifTedarikcileriGetir()
        {
            return Ok(service.GetActive());
        }
        [HttpGet("{id}")]
        public IActionResult IdyeGoreTedarikciGetir(int id)
        {
            return Ok(service.GetByID(id));
        }
        [HttpPost]
        public IActionResult TedarikciEkle(Supplier supplier)
        {
           if(service.Add(supplier))
            {
                return CreatedAtAction("IdyeGoreTedarikciGetir", new { id = supplier.ID }, supplier);
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        public IActionResult TedarikciGuncelle(int id,Supplier supplier)
        {
            if (id != supplier.ID)
                return BadRequest();
            try
            {
                if(service.Update(supplier))
                    return Ok(supplier);
                else
                    return BadRequest();
            }
            catch (Exception)
            {

                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult TedarikciSil(int id)
        {
            var supplier = service.GetByID(id);
            if (supplier is null)
            {
                return NotFound();
            }

            if (service.Remove(supplier))
                return Ok("Tedarikçi Silindi");
            else return BadRequest("Tedarikçi silinemedi");

        }
        [HttpGet("{id}")]
        public IActionResult TedarikciAktiflestir(int id)
        {
            Supplier s = service.GetByID(id);
            if (s is null) return NotFound();
            service.Activate(id);
            return Ok(service.GetByID(id));
        }
    }
}
