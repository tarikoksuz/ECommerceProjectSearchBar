using Project.BLL.DesignPatterns.RepositoryPattern.ConcRep;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoryRepository cRep;
        public CategoryController()
        {
            cRep = new CategoryRepository();
        }
        // GET: Admin/Category
        public ActionResult CategoryList()
        {
            return View(cRep.GetAll());
        }

        public ActionResult CategoryByID(int id)
        {
            return View(cRep.Find(id));
        }

       //todo: Ödev Kategori Sil ve Güncelle

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]

        public ActionResult AddCategory(Category item)
        {
            cRep.Add(item);
            return RedirectToAction("CategoryList");
        }


        public ActionResult UpdateCategory(int id)
        {
            return View(cRep.Find(id));
        }

        [HttpPost]

        public ActionResult UpdateCategory(Category item)
        {
            cRep.Update(item);
            return RedirectToAction("CategoryList");
        }


        public ActionResult DeleteCategory(int id)
        {
            cRep.Delete(cRep.Find(id));
            return RedirectToAction("CategoryList");
        }







    }
}