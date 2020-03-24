using Project.BLL.DesignPatterns.RepositoryPattern.ConcRep;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class EntityAttributeController : Controller
    {
        EARepository eaRep;
        public EntityAttributeController()
        {
            eaRep = new EARepository();
        }
        // GET: Admin/EntityAttribute
        public ActionResult EaList()
        {
            return View(eaRep.GetAll());
        }


        public ActionResult AddEA()
        {
            return View();
        }

        [HttpPost]

        public ActionResult AddEA(EntityAttribute item)
        {
            eaRep.Add(item);
            return RedirectToAction("EaList");
        }


        public ActionResult DeleteEA(int id)
        {
            eaRep.Delete(eaRep.Find(id));
            return RedirectToAction("EaList");
             
        }



        public ActionResult UpdateEA(int id)
        {
            return View(eaRep.Find(id));
        }

        [HttpPost]

        public ActionResult UpdateEA(EntityAttribute item)
        {
            eaRep.Update(item);
            return RedirectToAction("EaList");
        }










    }
}