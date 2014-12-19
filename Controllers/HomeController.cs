using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dotRDb.Web.Models;

namespace dotRDb.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int i;
            using (var db = new dotRDbEntities())
            {
                i = db.Resources.Count();
            }

            ViewBag.Message = string.Format("{0} resources in RDb", i);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
