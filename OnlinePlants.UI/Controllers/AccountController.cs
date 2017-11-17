using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OnlinePlants.Business;
using OnlinePlants.Model.BusinessModel;

namespace OnlinePlants.UI.Controllers
{
    public class AccountController : AsyncController
    {
        // GET: Account
        public ActionResult Register()
        {
            #region Page Initial



            if (Session["user"] != null)
            {
                Registration registration = (Registration)Session["Registration"];
                User user = (User)Session["user"];
                string RedirecURL = "";
                string name = "";
                string ImageURL = "";
                bool IsValidUser = false;
                CommonMethods.GetUserValue(registration, user, user.UserTypeID, out name, out ImageURL, out IsValidUser, out RedirecURL);
                if (IsValidUser == true)
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

        public ActionResult Login()
        {

            #region Page Initial



            if (Session["user"] != null)
            {
                Registration registration = (Registration)Session["Registration"];
                User user = (User)Session["user"];
                string RedirecURL = "";
                string name = "";
                string ImageURL = "";
                bool IsValidUser = false;
                CommonMethods.GetUserValue(registration, user, user.UserTypeID, out name, out ImageURL, out IsValidUser, out RedirecURL);
                if (IsValidUser == true)
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

        [HttpPost]
        public JsonResult UserRegistration(string Email, string Password, string UserType, string Name)
        {
            User user = null;
            string body = "";
            ResponseMessage responseMessage = new BRegistration().UserRegistration(Email, Password, UserType, Name, out user);
            if (responseMessage.RCode == 1)
            {
                Registration reg = (Registration)responseMessage.classobject;

                Session["User"] = user;
                Session["UserRegistration"] = responseMessage.classobject;

                string Error = "";
                string hostURL = string.Empty;
                if (System.Web.HttpContext.Current.Request.Url.Host.ToLower() == "localhost")
                    hostURL = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port;
                else
                    hostURL = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host;
                body = CommonMethods.GetSubscribtionMailBody(user.Username, hostURL, out Error);

                CommonMethods.SendEmail(user.Username, "OnlinePlants - Registration", body, out Error);
                if (Error != "")
                {
                    responseMessage.RException = "System unable to send Notification - " + Error;
                }
                responseMessage.RURL = "/Home/Index";
            }
            return Json(responseMessage);
        }
        public ActionResult UserLogin(string Username, string Password)
        {
            User user;
            ResponseMessage responseMessage = new BRegistration().UserLogin(Username, Password, out user);
            if (responseMessage.RCode == 1)
            {
                Registration reg = (Registration)responseMessage.classobject;
                Session["User"] = user;
                Session["UserRegistration"] = responseMessage.classobject;
                if (user.UserTypeID == 1)
                    responseMessage.RURL = "/Admin/Dashboard/";
                else
                    responseMessage.RURL = "/Home/Index";
            }
            return Json(responseMessage);
        }

        public ActionResult ForgetPassword(string Username)
        {
            ResponseMessage responseMessage = new BRegistration().ForgetPassword(Username);
            return Json(responseMessage);
        }
    }
}