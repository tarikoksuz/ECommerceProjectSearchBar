using Project.BLL.DesignPatterns.RepositoryPattern.ConcRep;
using Project.MODEL.Entities;
using Project.MODEL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Filters
{
    public class ResFilter : FilterAttribute, IResultFilter
    {
        LogRepository lrep;
        public ResFilter()
        {
           lrep =new LogRepository();
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {


            Log logging = new Log();

            if (filterContext.HttpContext.Session["LogMember"] != null)
            {
                logging.Information = "Kayıtlı kullanıcı";
                logging.Name = (filterContext.HttpContext.Session["LogMember"] as AppUser).UserName;

            }
            else
            {
                logging.Information = "Kayıt olmayan kullanıcı";
                logging.Name = "Anonim";
            }



            logging.ActionName = filterContext.RouteData.Values["Action"].ToString();
            logging.ControllerName = filterContext.RouteData.Values["Controller"].ToString();
            logging.Description = KeyWord.Exit;
            logging.Information += "- Veri,View calıstıktan sonra olusmustur";

            lrep.Add(logging);
        }
            

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
           
            Log logging = new Log();

            if (filterContext.HttpContext.Session["LogMember"] != null)
            {
                logging.Information = "Kayıtlı kullanıcı";
                logging.Name = (filterContext.HttpContext.Session["LogMember"] as AppUser).UserName;

            }
            else
            {
                logging.Information = "Kayıt olmayan kullanıcı";
                logging.Name = "Anonim";
            }

           

            logging.ActionName = filterContext.RouteData.Values["Action"].ToString();
            logging.ControllerName = filterContext.RouteData.Values["Controller"].ToString();
            logging.Description = KeyWord.Entry;
            logging.Information += "- Veri,View calısmadan önce olusmustur";
            lrep.Add(logging);
        }

    }
}