using Project.BLL.DesignPatterns.RepositoryPattern.ConcRep;
using Project.COMMON.Tools;
using Project.MODEL.Entities;
using Project.MVCUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductRepository pRep;
        CategoryRepository cRep;

        public ProductController()
        {
            pRep = new ProductRepository();
            cRep = new CategoryRepository();
        }
        // GET: Admin/Product
        public ActionResult ProductList(int? id)
        {
            if (id == null)
            {
                return View(pRep.GetAll());
            }
            return View(pRep.Where(x => x.CategoryID == id));
        }







        public ActionResult AddProduct()
        {
            ProductCategoryVM pcvm = new ProductCategoryVM();
            pcvm.Product = new Product();
            pcvm.Categories = cRep.GetAll();
            return View(pcvm);
        }

        [HttpPost]

        public ActionResult AddProduct(Product Product, HttpPostedFileBase resim)
        {
            Product.ImagePath = ImageUploader.UploadImage("~/Pictures/", resim);
            pRep.Add(Product);
            return RedirectToAction("ProductList");
        }


        public ActionResult UpdateProduct(int id)
        {
            ProductCategoryVM pcvm = new ProductCategoryVM();
            pcvm.Categories = cRep.GetAll();
            pcvm.Product = pRep.Find(id);
            return View(pcvm);
        }

        [HttpPost]

        public ActionResult UpdateProduct(Product Product)
        {
            pRep.Update(Product);
            return RedirectToAction("ProductList");
        }


        public ActionResult DeleteProduct(int id)
        {
            pRep.Delete(pRep.Find(id));
            return RedirectToAction("ProductList");
        }






    }
}