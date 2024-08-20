using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestMvc.Models;
using TestMvc.Models.Services;
using TestMvc.Utility;

namespace TestMvc.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepo;
        private readonly IKitapRepository _kitapRepo;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public KiralamaController(IKitapRepository kitapRepo, IKiralamaRepository kiralamaRepo, IWebHostEnvironment webHostEnvironment)
        {
            _kitapRepo = kitapRepo;
            _kiralamaRepo = kiralamaRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            //List<Kitap> objKitapList = _kitapRepo.GetAll().ToList();
            List<Kiralama> objKiralamaList = _kiralamaRepo.GetAll(includeProps: "Kitap").ToList();

            return View(objKiralamaList);
        }

        public IActionResult EkleGuncelle(int? id)
        {

            IEnumerable<SelectListItem> KitapList = _kitapRepo.GetAll().Select(k => new SelectListItem
            {
                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });

            ViewBag.KitapList = KitapList;

            if(id == null || id == 0)
            {

                //Ekle
                return View();
            }
            else
            {
                //Guncelle
                Kiralama? kiralamaVt = _kiralamaRepo.Get(kt => kt.Id == id);

                if (kiralamaVt == null)
                {
                    return NotFound();
                }

                return View(kiralamaVt);
            }

            
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {

            var errors = ModelState.Values.SelectMany(x => x.Errors);

            var kitap = _kitapRepo.Get(kt => kt.Id == kiralama.KitapId);

            kiralama.Kitap = kitap;


            if(kiralama.Id == 0)
            {
                _kiralamaRepo.Ekle(kiralama);
                TempData["basarili"] = "Yeni Kiralama Başarıyla Gerçekleştirildi.";
            }
            else
            {
                _kiralamaRepo.Guncelle(kiralama);
                TempData["basarili"] = "Kiralama Başarıyla Güncellendi.";
            }

            
            _kitapRepo.Kaydet();

            

            return RedirectToAction("Index", "Kiralama"); //Action name and Controller
            

            return View();

        }

        public IActionResult Sil(int? id)
        {


            if (id == null || id < 0)
            {
                return NotFound();
            }

            Kiralama? kiralamaVt = _kiralamaRepo.Get(kt => kt.Id == id, includeProps: "Kitap");

            if (kiralamaVt == null)
            {
                return NotFound();
            }

            return View(kiralamaVt);
        }


        [HttpPost]
        public IActionResult Sil(Kiralama? kiralama)
        {

            if (ModelState.IsValid)
            {
                _kiralamaRepo.Sil(kiralama);
                _kiralamaRepo.Kaydet();

                TempData["basarili"] = "Kiralama Başarıyla Silindi.";

                return RedirectToAction("Index", "Kiralama"); //Action name and Controller
            }

            return View();


        }


    }
}
