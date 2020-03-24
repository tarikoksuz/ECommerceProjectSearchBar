using PagedList;
using Project.BLL.DesignPatterns.RepositoryPattern.ConcRep;
using Project.COMMON.Tools;
using Project.MODEL.Entities;
using Project.MVCUI.Models;
using Project.MVCUI.Models.ShoppingTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {

        OrderRepository oRep;
        ProductRepository pRep;
        CategoryRepository cRep;
        OrderDetailRepository odRep;
        public ShoppingController()
        {
            oRep = new OrderRepository();
            pRep = new ProductRepository();
            cRep = new CategoryRepository();
            odRep = new OrderDetailRepository();
        }

        // GET: Shopping
        public ActionResult ShoppingList(int? page, int? categoryID, string range) //nullable int vermemizin sebebi aslında buradaki int'in sayfa sayımızı temsil edecek olmasıdır. Ancak birisi direkt alısveriş sayfasına ulastıgında sayfa sayısını göndermeyebilir de böylece bu sekilde de Action'in calısabilmesini istiyoruz...
        {



            PAVM pavm = new PAVM();
            if (range != null)
            {
                pavm.PagedProducts = categoryID == null ? pRep.GetActives().Where(x => x.ProductName.ToLower().Contains(range.ToLower())).ToPagedList(page ?? 1, 9) : pRep.Where(x => x.CategoryID == categoryID && x.ProductName.ToLower().Contains(range.ToLower())).ToPagedList(page ?? 1, 9);
            }
            else
            {
                pavm.PagedProducts = categoryID == null ? pRep.GetActives().ToPagedList(page ?? 1, 9) : pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9);
            }
            pavm.Categories = cRep.GetActives();

            if (categoryID != null) TempData["catID"] = categoryID;
            return View(pavm);
        }

        public ActionResult RangeProduct(int? page, int? categoryID, string range)
        {



            PAVM pavm = new PAVM();
            if (range != null)
            {
                pavm.PagedProducts = categoryID == null ? pRep.GetActives().Where(x => x.ProductName.ToLower().Contains(range.ToLower())).ToPagedList(page ?? 1, 9) : pRep.Where(x => x.CategoryID == categoryID && x.ProductName.ToLower().Contains(range.ToLower())).ToPagedList(page ?? 1, 9);
            }
            else
            {
                pavm.PagedProducts = categoryID == null ? pRep.GetActives().ToPagedList(page ?? 1, 9) : pRep.Where(x => x.CategoryID == categoryID).ToPagedList(page ?? 1, 9);
            }
            pavm.Categories = cRep.GetActives();


            if (categoryID != null) TempData["catID"] = categoryID;
            return View("RangeProduct", pavm);


        }


        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;

            Product eklenecekUrun = pRep.Find(id);

            CartItem ci = new CartItem
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProductName,
                Price = eklenecekUrun.UnitPrice,
                ImagePath = eklenecekUrun.ImagePath
            };

            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");
        }



        public ActionResult CartPage()
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                return View(c);
            }
            TempData["sepetBos"] = "Sepetinizde ürün bulunmamaktadır";
            return RedirectToAction("ShoppingList");
        }


        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenSil(id);
                if (c.Sepetim.Count == 0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "Sepetinizde ürün bulunmamaktadır";
                    return RedirectToAction("ShoppingList");
                }
                return RedirectToAction("CartPage");
            }

            return RedirectToAction("ShoppingList");
        }

        //http://localhost:64422/api/Payment/ReceivePayment


        public ActionResult SiparisiOnayla()
        {
            AppUser mevcutKullanici;
            if (Session["member"] != null)
            {
                mevcutKullanici = Session["member"] as AppUser;
            }
            else
            {
                TempData["anonim"] = "Kullanıcı üye degil";
            }
            return View();
        }

        [HttpPost]
        public ActionResult SiparisiOnayla(OrderVM ovm)
        {
            bool result;
            Cart sepet = Session["scart"] as Cart;
            ovm.Order.TotalPrice = ovm.PaymentVM.ShoppingPrice = sepet.TotalPrice.Value;

            //WebApiClient kütüphanesini indirmeyi unutmayın...Yoksa API'ya client istek gönderemezsiniz...
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:64422/api/");

                Task<HttpResponseMessage> postTask = client.PostAsJsonAsync("Payment/ReceivePayment", ovm.PaymentVM);

                HttpResponseMessage sonuc;
                try
                {
                    sonuc = postTask.Result;
                }
                catch (Exception ex)
                {

                    TempData["baglantiRed"] = "Banka baglantıyı reddetti";
                    return RedirectToAction("ShoppingList");
                }

                if (sonuc.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            if (result)
            {
                if (Session["member"] != null)
                {
                    AppUser kullanici = Session["member"] as AppUser;
                    ovm.Order.AppUserID = kullanici.ID;
                    ovm.Order.UserName = kullanici.UserName;
                }
                else
                {

                    ovm.Order.AppUserID = null;
                    ovm.Order.UserName = TempData["anonim"].ToString();
                }


                oRep.Add(ovm.Order); //tam bu noktada Order'in ID'si identity tarafından olusturuluyor...

                foreach (CartItem item in sepet.Sepetim)
                {

                    OrderDetail od = new OrderDetail();
                    od.OrderID = ovm.Order.ID;
                    od.ProductID = item.ID;
                    od.TotalPrice = item.SubTotal;
                    od.Quantity = item.Amount;
                    odRep.Add(od);
                    //Stoktan düsmek icin yazılan kodlar
                    Product stokdus = pRep.Find(item.ID);
                    stokdus.UnitsInStock -= item.Amount;
                    pRep.Update(stokdus);
                }

                TempData["odeme"] = "Siparişiniz bize ulasmıstır..Tesekkür ederiz";
                MailSender.Send(ovm.Order.Email, body: $"Siparişiniz basarıyla alındı..{ovm.Order.TotalPrice}");
                return RedirectToAction("ShoppingList");

            }

            else
            {
                TempData["sorun"] = "Odeme ile ilgili bir sorun olustu..Lütfen bankanızla iletişime geciniz";
                return RedirectToAction("ShoppingList");
            }

        }



    }
}