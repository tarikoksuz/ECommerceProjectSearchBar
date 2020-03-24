using Project.BLL.DesignPatterns.RepositoryPattern.ConcRep;
using Project.COMMON.Tools;
using Project.MODEL.Entities;
using Project.MVCUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class RegisterController : Controller
    {
        AppUserRepository apRep;
        AppUserDetailRepository apdRep;
        public RegisterController()
        {
            apdRep = new AppUserDetailRepository();
            apRep = new AppUserRepository();
        }
        // GET: Register
        public ActionResult RegisterNow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterNow(AppUser AppUser, AppUserDetail AppUserDetail)
        {
            AppUser.Password = DantexCrypt.Crypt(AppUser.Password);

            //AppUser.Password = DantexCrypt.DeCrypt(AppUser.Password);

            if (apRep.Any(x => x.UserName == AppUser.UserName))
            {
                ViewBag.ZatenVar = "Kullanıcı ismi daha önce alınmıs";
                return View();
            }
            else if (apRep.Any(x => x.Email == AppUser.Email))
            {
                ViewBag.ZatenVar = "Email zaten kayıtlı";
                return View();
            }
            //Kullanıcı basarılı bir sekilde register işlemini tamamlıyorsa ona mail gönder

            string gonderilecekMail = "Tebrikler.. Hesabınız olusturulmustur.. Hesabınızı aktive etmek icin http://localhost:55735/Register/Activation/" + AppUser.ActivationCode + " linkine tıklayabilirsiniz.";
            MailSender.Send(AppUser.Email, body: gonderilecekMail, subject: "Hesap aktivasyon!");
            apRep.Add(AppUser); //buradan sonra AppUser'in identity olan id'si olusmus oluyor... O yüzden AppUserDetail'nin id'sini verecek isek ve olusturacak isek buradan vermeliyiz.

            if (!string.IsNullOrEmpty(AppUserDetail.FirstName) || !string.IsNullOrEmpty(AppUserDetail.LastName) || !string.IsNullOrEmpty(AppUserDetail.Address))
            {
                AppUserDetail.ID = AppUser.ID;
                apdRep.Add(AppUserDetail);

            }

            return View("RegisterOk");
        }


        public ActionResult Activation(Guid id)
        {
            if (apRep.Any(x => x.ActivationCode == id))
            {
                AppUser aktiveEdilecek = apRep.Default(x => x.ActivationCode == id);
                aktiveEdilecek.IsActive = true;

                apRep.Update(aktiveEdilecek);

                TempData["HesapAktif"] = "Hesabınız aktif hale getirildi";
                return RedirectToAction("Register");
            }
            else
            {
                TempData["HesapAktif"] = "Aktif edilecek hesap bulunamadı";
                return RedirectToAction("Register");
            }
        }

        public ActionResult RegisterOk()
        {
            return View();
        }
    }
}