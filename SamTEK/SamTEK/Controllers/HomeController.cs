using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SamTEK.Models;
using System.Diagnostics;

namespace SamTEK.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly appDbContext _context;

        public HomeController(ILogger<HomeController> logger, appDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(IFormCollection datas)
        {
            var kPosta = datas["kAdi"].ToString();
            var kSifre = datas["kSifre"].ToString();

            var kullanici = await _context.kullaniciTablosu.FirstOrDefaultAsync(k => k.eposta == kPosta && k.sifre == kSifre);

            if (kullanici != null)
            {
                TempData["id"] = kullanici.kullaniciID;

                if (!kullanici.rank)
                {
                    news.adminIntID = -1;
                    news.kullaniciIntID = kullanici.kullaniciID;
                    return RedirectToAction("servisOlustur", "home");
                }
                else
                {
                    news.kullaniciIntID = -1;
                    news.adminIntID = kullanici.kullaniciID;
                    return RedirectToAction("servisler", "admin");
                }
            }
            else
            {
                return RedirectToAction("giris", "home");
            }
        }


        public IActionResult Kayit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Kayit(IFormCollection datas)
        {
            var kAdi = datas["kAdi"].ToString();
            var kPosta = datas["kPosta"].ToString();
            var kSifre = datas["kSifre"].ToString();


            var kullaniciVarMi = _context.kullaniciTablosu.Any(k => k.eposta == kPosta);
            if (kullaniciVarMi)
            {
                return RedirectToAction("kayit", "Home");
            }

            var yeniKullanici = new kullaniciTablosu
            {
                rank = false,
                adi = kAdi,
                eposta = kPosta,
                sifre = kSifre
            };
            _context.kullaniciTablosu.Add(yeniKullanici);
            _context.SaveChanges();

            return RedirectToAction("giris", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> ServisOlustur()
        {
            if (news.kullaniciIntID > -1)
            {
                var kullanici = _context.kullaniciTablosu.FirstOrDefault(k => k.kullaniciID == news.kullaniciIntID);
                var cihaz = await _context.cihazTablosu.Where(c => c.kullaniciID == news.kullaniciIntID).ToListAsync();

                if (kullanici == null)
                {
                    return NotFound();
                }

                var model = new kullaniciCihazModel
                {
                    kullanici = kullanici,
                    cihaz = cihaz,
                    servis = null
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
            
            return View();
        }
            
        [HttpPost]
        public IActionResult ServisOlustur(string cihaz, string sikayet)
        {
            
            var yeniCihaz = new cihazTablosu
            {
                kullaniciID = news.kullaniciIntID,
                cihaz = cihaz,
                sorun = sikayet,
                servis = false
            };
            _context.cihazTablosu.Add(yeniCihaz);
            _context.SaveChanges();

            return RedirectToAction("ServisOlustur");
        }


        public async Task<IActionResult> ServisKontrol()
        {
            if (news.kullaniciIntID > -1)
            {
                var kullanici = _context.kullaniciTablosu.FirstOrDefault(k => k.kullaniciID == news.kullaniciIntID);
                var cihaz = await _context.cihazTablosu.Where(c => c.kullaniciID == news.kullaniciIntID).ToListAsync();

                var cihazIDList = cihaz.Select(c => c.cihazID).ToList();
                var servis = await _context.servisTablosu.Where(s => cihazIDList.Contains(s.cihazID)).ToListAsync();

                if (kullanici == null)
                {
                    return NotFound();
                }

                var model = new kullaniciCihazModel
                {
                    kullanici = kullanici,
                    cihaz = cihaz,
                    servis = servis
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("index","home");
            }
            return View();
            
        }






        
    }
}