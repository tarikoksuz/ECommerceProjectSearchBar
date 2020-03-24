
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
    public class ActFilter : FilterAttribute, IActionFilter
    {

       LogRepository lrep;
        public ActFilter()
        {
            lrep = new LogRepository();
        }



        public void OnActionExecuted(ActionExecutedContext filterContext)
        {


           
            Log logger = new Log();
            if (filterContext.HttpContext.Session["LogMember"] == null)

            {
                logger.Information = "Anonim kullanıcı";
                logger.Name = "Kullanıcı yok";
                logger.LogOutTime = DateTime.Now;

            }

            else
            {
                logger.Information =$"{ (filterContext.HttpContext.Session["LogMember"] as AppUser).UserName} ({(filterContext.HttpContext.Session["LogMember"] as AppUser).Role})";
                logger.Name = (filterContext.HttpContext.Session["LogMember"] as AppUser).UserName;
                logger.LogOutTime = DateTime.Now;
            }



            logger.ActionName = filterContext.ActionDescriptor.ActionName;

            logger.ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            logger.Description = KeyWord.Exit;
            lrep.Add(logger);

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            Log logger = new Log();
            if (filterContext.HttpContext.Session["LogMember"] == null)

            {
                logger.Information = "Anonim kullanıcı";
                logger.Name = "Kullanıcı yok";
                logger.LoginTime = DateTime.Now;

            }

            else
            {
                logger.Information = $"{ (filterContext.HttpContext.Session["LogMember"] as AppUser).UserName} ({(filterContext.HttpContext.Session["LogMember"] as AppUser).Role})";
                logger.Name = (filterContext.HttpContext.Session["LogMember"] as AppUser).UserName;
                logger.LoginTime = DateTime.Now;
            }



            logger.ActionName = filterContext.ActionDescriptor.ActionName;

            logger.ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            logger.Description = KeyWord.Entry;
            lrep.Add(logger);


        }
    }
}
    
    