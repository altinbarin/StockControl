using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StokKontrolProje.Entities.Entities;
using StokKontrolProje.Service.Abstract;

namespace KD_16_StokKontrolProje.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> service;

        public UserController(IGenericService<User> service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Login(string email,string parola)
        {
            if(service.Any(x=>x.Email.Equals(email)&&x.Password.Equals(parola)))
            {
                User logged = service.GetByDefault(x => x.Email.Equals(email) && x.Password.Equals(parola));
                return Ok(logged);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult KullaniciEkle(User user)
        {
            if (service.Add(user))
                return CreatedAtAction("IdyeGoreKullaniciGetir",new { id = user.ID },user);
            return BadRequest();
        }
        [HttpGet("{id}")]
        public IActionResult IdyeGoreKullaniciGetir(int id)
        {
            return Ok(service.GetByID(id));
        }
        [HttpGet]
        public IActionResult TumkullanicilariGetir()
        {
            return Ok(service.GetAll());
        }
        [HttpGet]
        public IActionResult AktifKullaniilariGetir()
        {
            return Ok(service.GetActive());
        }
        [HttpPut("{id}")]
        public IActionResult KullaniciGuncelle(int id,User user)
        {
            if (id != user.ID) return BadRequest();
            if (service.Update(user))
                return Ok(user);
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public IActionResult KullaniciSil(int id)
        {
            User u = service.GetByID(id);
            if (u is null) return NotFound();
            if (service.Remove(u))
                return Ok("Kayıt silindi");
            return BadRequest();
        }
        [HttpGet("{id}")]
        public IActionResult KullaniciAktiflestir(int id)
        {
            User user = service.GetByID(id);
            if (user is null) return NotFound();
            service.Activate(id);
            return Ok(user);
        }
    }
}
