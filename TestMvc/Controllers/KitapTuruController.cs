using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMvc.Models;
using TestMvc.Models.Services;
using TestMvc.Utility;

namespace TestMvc.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KitapTuruController : Controller
    {
        private readonly IKitapTuruRepository _kitapTuruRepo;

        public KitapTuruController(IKitapTuruRepository kitapTuruRepo)
        {
            _kitapTuruRepo = kitapTuruRepo;
        }

        public IActionResult Index()
        {

            List<KitapTuru> objKitapTuruList = _kitapTuruRepo.GetAll().ToList();

            return View(objKitapTuruList);
        }

        public IActionResult Ekle()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {

            if (ModelState.IsValid)
            {
                _kitapTuruRepo.Ekle(kitapTuru);
                _kitapTuruRepo.Kaydet();

                TempData["basarili"] = "Yeni Kitap Türü Başarıyla Eklendi.";

                return RedirectToAction("Index", "KitapTuru"); //Action name and Controller
            }

            return View();

        }

        public IActionResult Guncelle(int? id )
        {


            if(id == null || id < 0)
            {
                return NotFound();
            }

            KitapTuru? kitapTuruVt = _kitapTuruRepo.Get(kt => kt.Id == id);

            if(kitapTuruVt == null)
            {
                return NotFound();
            }

            return View(kitapTuruVt);
        }

        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {

            if (ModelState.IsValid)
            {
                _kitapTuruRepo.Guncelle(kitapTuru);
                _kitapTuruRepo.Kaydet();
                TempData["basarili"] = "Yeni Kitap Türü Başarıyla Güncellendi.";

                return RedirectToAction("Index", "KitapTuru"); //Action name and Controller
            }

            return View();

        }


        public IActionResult Sil(int? id)
        {


            if (id == null || id < 0)
            {
                return NotFound();
            }

            KitapTuru? kitapTuruVt = _kitapTuruRepo.Get(kt => kt.Id == id);

            if (kitapTuruVt == null)
            {
                return NotFound();
            }

            return View(kitapTuruVt);
        }

        //[HttpPost, ActionName("Sil")]
        //public IActionResult SilPost(int? id)
        //{

        //    KitapTuru? kitapTuru = _uygulamaDbContext.KitapTurleri.Find(id);

        //    if(kitapTuru == null)
        //    {
        //        return NotFound();
        //    }

        //    _uygulamaDbContext.KitapTurleri.Remove(kitapTuru);
        //    _uygulamaDbContext.SaveChanges();

        //    return RedirectToAction("Index", "KitapTuru"); //Action name and Controller

        //}

        [HttpPost]
        public IActionResult Sil(KitapTuru? kitapTuru)
        {

            if (ModelState.IsValid)
            {
                _kitapTuruRepo.Sil(kitapTuru);
                _kitapTuruRepo.Kaydet();

                TempData["basarili"] = "Yeni Kitap Türü Başarıyla Silindi.";

                return RedirectToAction("Index", "KitapTuru"); //Action name and Controller
            }

            return View();


        }


    }
}
