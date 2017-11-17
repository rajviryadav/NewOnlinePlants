using OnlinePlants.Business;
using OnlinePlants.Business.BusinessLogicModel;
using OnlinePlants.Model.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinePlants.UI.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            #region Page Initial

            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if (Session["user"] != null)
            {
                Registration registration = (Registration)Session["Registration"];
                User user = (User)Session["user"];
                string RedirecURL = "";
                string name = "";
                string ImageURL = "";
                bool IsValidUser = false;
                CommonMethods.GetUserValue(registration, user, 1, out name, out ImageURL, out IsValidUser, out RedirecURL);
                if (IsValidUser == false)
                {
                    var url = RedirecURL.Split('/');
                    return RedirectToAction(url[2], url[1], new { area = url[0] });
                }

                ViewBag.Name = name;
                ViewBag.ImageURL = ImageURL;

            }

            #endregion
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        public JsonResult GetUserList(int UserTypeId, int PageNo, string JobSearchTerm)
        {
            User user = (User)Session["user"];
            ResponseMessage2<RegistrationMaster> responseMessage = null;
            if (Session["user"] != null)
            {
                responseMessage = new BAdmin().GetUsersList(UserTypeId, PageNo, JobSearchTerm);
            }

            return Json(responseMessage);

        }

        public JsonResult UpdateUserStatus(int UserTypeId, int UserId)
        {
            ResponseMessage responseMessage = null;
            if (Session["user"] != null)
            {
                User user = (User)Session["user"];
                responseMessage = new BAdmin().DeactiveUser(UserTypeId, UserId, user.RegID);
            }
            return Json(responseMessage);
        }
        public ActionResult UserProfile()
        {
            #region Page Initial

            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if (Session["user"] != null)
            {
                Registration registration = (Registration)Session["Registration"];
                User user = (User)Session["user"];
                string RedirecURL = "";
                string name = "";
                string ImageURL = "";
                bool IsValidUser = false;
                CommonMethods.GetUserValue(registration, user, 1, out name, out ImageURL, out IsValidUser, out RedirecURL);
                if (IsValidUser == false)
                {
                    var url = RedirecURL.Split('/');
                    return RedirectToAction(url[2], url[1], new { area = url[0] });
                }

                ViewBag.Name = name;
                ViewBag.ImageURL = ImageURL;

            }

            #endregion
            return View();
        }
    }
}