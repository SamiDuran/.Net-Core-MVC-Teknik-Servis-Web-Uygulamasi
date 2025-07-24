using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SamTEK.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SamTEK.Controllers
{
    public class AdminController : Controller
    {
        

        private readonly appDbContext _context;


        public AdminController(appDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Servisler()
        {
            if (news.adminIntID > -1)
            {
                var kullanicilar = await _context.kullaniciTablosu.Where(k => k.rank == false).ToListAsync();

                var model = new kcsListModel
                {
                    kcsListModels = new List<kullaniciCihazModel>()
                };

                foreach (var kullanici in kullanicilar)
                {
                    var cihazlar = await _context.cihazTablosu.Where(c => c.kullaniciID == kullanici.kullaniciID && c.servis == true).ToListAsync();
                    var cihazIDList = cihazlar.Select(c => c.cihazID).ToList();
                    var servisler = await _context.servisTablosu.Where(s => cihazIDList.Contains(s.cihazID)).ToListAsync();

                    var yeniKCS = new kullaniciCihazModel
                    {
                        kullanici = kullanici,
                        cihaz = cihazlar,
                        servis = servisler
                    };

                    model.kcsListModels.Add(yeniKCS);
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("index", "home");
            }


            return View();
        }




        [HttpPost]
        [Route("/admin/teslimEt")]
        public async Task<IActionResult> TeslimEt(int servisID, int cihazID)
        {
            var kayit = await _context.servisTablosu.FirstOrDefaultAsync(s => s.servisID == servisID);
            if (kayit != null)
            {
                kayit.teslim = true;
                kayit.surec = "Teslim Edildi";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Servisler", "Admin");
        }


        [HttpPost]
        [Route("/admin/servisDuzenle")]
        public async Task<IActionResult> ServisDuzenle(int cihazID, string surec)
        {
            var kayit = await _context.servisTablosu.FirstOrDefaultAsync(c => c.cihazID == cihazID);
            if (kayit != null)
            {
                kayit.surec = surec;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Servisler", "Admin");
        }







        [HttpGet]
        public async Task<IActionResult> YeniServis()
        {
            if (news.adminIntID > -1)
            {
                var kullanicilar = await _context.kullaniciTablosu.Where(k => k.rank == false).ToListAsync();

                var model = new kcsListModel
                {
                    kcsListModels = new List<kullaniciCihazModel>()
                };

                foreach (var kullanici in kullanicilar)
                {
                    var cihazlar = await _context.cihazTablosu.Where(c => c.kullaniciID == kullanici.kullaniciID && c.servis == false).ToListAsync();
                    var cihazIDList = cihazlar.Select(c => c.cihazID).ToList();
                    var servisler = await _context.servisTablosu.Where(s => cihazIDList.Contains(s.cihazID) && s.teslim == false).ToListAsync();

                    var yeniKCS = new kullaniciCihazModel
                    {
                        kullanici = kullanici,
                        cihaz = cihazlar,
                        servis = servisler
                    };

                    model.kcsListModels.Add(yeniKCS);
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> YeniServisAsync(IFormCollection datas)
        {
            var cID = Convert.ToInt32(datas["cihazID"].ToString());
            var surec = datas["surec"].ToString();

            var cihaz = await _context.cihazTablosu.FindAsync(cID);

            
            var yeniServisKayit = new servisTablosu
            {
                cihazID = cID,
                surec = surec,
                teslim = false
            };

            cihaz.servis = true;
            _context.cihazTablosu.Update(cihaz);
            _context.servisTablosu.Add(yeniServisKayit);
            

            await _context.SaveChangesAsync();

            return RedirectToAction("servisler", "admin");
        }





        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Giris()
        {
            return RedirectToAction("Giris", "Home");
        }






        //public List<string> selectKullaniciVeriler()
        //{
        //    List<string> kveri = new List<string>();

        //    SqlConnection con = new SqlConnection(sqlAdresi);
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("select kullaniciID from kullaniciTablosu where servisteMi = 0", con);
        //    SqlDataReader dr = cmd.ExecuteReader();
        //    while (dr.Read())
        //    {
        //        kveri.Add(dr["kullaniciID"].ToString());
        //    }

        //    return kveri;
        //}

        //public List<string> selectAdminVeriler()
        //{
        //    List<string> averi = new List<string>();

        //    SqlConnection con = new SqlConnection(sqlAdresi);
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("select adminID from adminTablosu", con);
        //    SqlDataReader dr = cmd.ExecuteReader();
        //    while (dr.Read())
        //    {
        //        averi.Add(dr["adminID"].ToString());
        //    }

        //    return averi;
        //}

        //public List<string> teslim()
        //{
        //    List<string> tveri = new List<string>();

        //    SqlConnection con = new SqlConnection(sqlAdresi);
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("select kullaniciID from kullaniciTablosu where teslim = 1", con);
        //    SqlDataReader dr = cmd.ExecuteReader();
        //    while (dr.Read())
        //    {
        //        tveri.Add(dr["kullaniciID"].ToString());
        //    }

        //    return tveri;
        //}
    }
}
