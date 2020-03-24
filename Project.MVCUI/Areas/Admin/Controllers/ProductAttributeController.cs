using Project.BLL.DesignPatterns.RepositoryPattern.ConcRep;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class ProductAttributeController : Controller
    {
        EARepository eaRep;
        PARepository paRep;
        ProductRepository pRep;
        public ProductAttributeController()
        {
            pRep = new ProductRepository();
            paRep = new PARepository();
            eaRep = new EARepository();
        }
        // GET: Admin/ProductAttribute
        public ActionResult ProductAttributeList(int id)
        {
          
            return View(pRep.Find(id));
        }

        [HttpPost]
        public ActionResult ProductAttributeListValue(int id,FormCollection collection)
        {
            List<ProductAttribute> currentData = paRep.Where(x => x.ProductID == id);
            int indexer = 0;
            foreach (ProductAttribute element in currentData)
            {
                element.Value = collection.GetValues("valueName")[indexer];
                indexer++;
                paRep.Update(element);
            }
            return RedirectToAction("ProductDetail", new { id = id });
        }



        public ActionResult ProductDetail(int id)
        {
            return View(paRep.Where(x=>x.ProductID==id));
        }



        public ActionResult ProductAttributeAdd(int id)
        {
            ViewBag.AttributeList = eaRep.GetAll();
            return View(pRep.Find(id));
        }

        [HttpPost]
        public ActionResult ProductAttributeAdd(Product item,FormCollection collection)
        {
            foreach (string element in collection.GetValues("checkbox"))
            {
                int id = Convert.ToInt32(element);
                ProductAttribute pa = new ProductAttribute();
                pa.ProductID = item.ID;
                pa.AttributeID = id;
                paRep.Add(pa);

            }

            return RedirectToAction("ProductAttributeList", new { id = item.ID });
        }
    }


    //Todo: İlgili ürün özelliklerini silme ve degiştirme durumlarını yapın... DeadLine: 18 Mart 2020
}