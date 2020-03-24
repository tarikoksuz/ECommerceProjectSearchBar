using Project.BLL.DesignPatterns.RepositoryPattern.ConcRep;
using Project.COMMON.Tools;
using Project.MODEL.Entities;
using Project.MODEL.Enums;
using Project.MVCUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    [ActFilter, ResFilter]
    public class HomeController : Controller
    {

        AppUserRepository apRep;
        public HomeController()
        {
            apRep = new AppUserRepository();
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(AppUser item)
        {
            AppUser yakalanan = apRep.Default(x => x.UserName == item.UserName);

            string decrypted = DantexCrypt.DeCrypt(yakalanan.Password);

            if (item.Password == decrypted && yakalanan != null && yakalanan.Role == MODEL.Enums.UserRole.Admin)
            {

          
                    if (!yakalanan.IsActive)
                    {
                        AktifKontrol();

                    }
                    Session["admin"] = yakalanan;
                    Session["LogMember"] = yakalanan;
                    return RedirectToAction("CategoryList", "Category", new { area = "Admin" });
               

            }

            else if (item.Password == decrypted && yakalanan != null && yakalanan.Role == MODEL.Enums.UserRole.Member)
            {
                if (!yakalanan.IsActive)
                {
                    AktifKontrol();

                }

                Session["member"] = yakalanan;
                Session["LogMember"] = yakalanan;
                return RedirectToAction("ShoppingList", "Shopping");
            }

            if (yakalanan != null)
            {
                LogRepository lrep = new LogRepository();
                Log sifreYanlis = new Log();
                sifreYanlis.Description = KeyWord.Exit;
                sifreYanlis.Information = $"{item.UserName} adlı kullanıcı şifresini {DateTime.Now} tarihinde yanlış girdi.";
                lrep.Add(sifreYanlis);

            }




            TempData["KullaniciYok"] = "Kullanıcı veya şifre yanlış.";
            return View();
        }


        private ActionResult AktifKontrol()
        {
            ViewBag.AktifDegil = "Lutfen hesabınızı aktif hale getiriniz...Mailinizi kontrol ediniz...";
            return View("Login");
        }


    }
}