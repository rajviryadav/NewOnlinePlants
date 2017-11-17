using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlinePlants.Business;

namespace OnlinePlants.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //BUserType usertype = new BUserType();
            //var list = usertype.GetUserTypeList();
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Testimonial()
        {
            return View();
        }

        public ActionResult Contactus()
        {
            return View();
        }

        

        public ActionResult Aboutus()
        {
            return View();
        }

    }
}