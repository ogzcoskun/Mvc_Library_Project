using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestMvc.Models;
using TestMvc.Models.Services;
using TestMvc.Utility;

namespace TestMvc.Controllers
{
    
    public class KitapController : Controller
    {
        private readonly IKitapRepository _kitapRepo;
        private readonly IKitapTuruRepository _kitapTuruRepo;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KitapController(IKitapRepository kitapRepo, IKitapTuruRepository kitapTuruRepo, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepo = kitapRepo;
            _kitapTuruRepo = kitapTuruRepo;
            _webHostEnvironment = webHostEnvironment;
        }


        [Authorize(Roles = "Admin,Ogrenci")]
        public IActionResult Index()
        {


            List<Kitap> objKitapList = _kitapRepo.GetAll(includeProps: "KitapTuru").ToList();
            return View(objKitapList);
        }

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {

            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepo.GetAll().Select(k => new SelectListItem
            {
                Text = k.Ad,
                Value = k.Id.ToString()
            });

            ViewBag.KitapTuruList = KitapTuruList;

            if(id == null || id == 0)
            {

                //Ekle
                return View();
            }
            else
            {
                //Guncelle
                Kitap? kitapVt = _kitapRepo.Get(kt => kt.Id == id);

                if (kitapVt == null)
                {
                    return NotFound();
                }

                return View(kitapVt);
            }

            
        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(Kitap kitap, IFormFile file)
        {

            var errors = ModelState.Values.SelectMany(x => x.Errors);

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var kitapPath = Path.Combine(wwwRootPath, @"img");

            if(file != null)
            {
                using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                kitap.ResimUrl = @"\img\" + file.FileName;

            }



            

            var kitapTuru = _kitapTuruRepo.Get(kt => kt.Id == kitap.KitapTuruId);

            kitap.KitapTuru = kitapTuru;


            if(kitap.Id == 0)
            {
                _kitapRepo.Ekle(kitap);
                TempData["basarili"] = "Yeni Kitap Başarıyla Eklendi.";
            }
            else
            {
                _kitapRepo.Guncelle(kitap);
                TempData["basarili"] = "Kitap Başarıyla Güncellendi.";
            }

            
            _kitapRepo.Kaydet();

            

            return RedirectToAction("Index", "Kitap"); //Action name and Controller
            

            return View();

        }

        //public IActionResult Guncelle(int? id )
        //{


        //    if(id == null || id < 0)
        //    {
        //        return NotFound();
        //    }

        //    Kitap? kitapVt = _kitapRepo.Get(kt => kt.Id == id);

        //    if(kitapVt == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(kitapVt);
        //}

        //[HttpPost]
        //public IActionResult Guncelle(Kitap kitap)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _kitapRepo.Guncelle(kitap);
        //        _kitapRepo.Kaydet();
        //        TempData["basarili"] = "Yeni Kitap Başarıyla Güncellendi.";

        //        return RedirectToAction("Index", "Kitap"); //Action name and Controller
        //    }

        //    return View();

        //}

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {


            if (id == null || id < 0)
            {
                return NotFound();
            }

            Kitap? kitapVt = _kitapRepo.Get(kt => kt.Id == id);

            if (kitapVt == null)
            {
                return NotFound();
            }

            return View(kitapVt);
        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(Kitap? kitap)
        {

            if (ModelState.IsValid)
            {
                _kitapRepo.Sil(kitap);
                _kitapRepo.Kaydet();

                TempData["basarili"] = "Yeni Kitap Başarıyla Silindi.";

                return RedirectToAction("Index", "Kitap"); //Action name and Controller
            }

            return View();


        }


    }
}
