using OnlinePlants.Business;
using OnlinePlants.Business.BusinessLogicModel;
using OnlinePlants.Model.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinePlants.UI.Controllers
{
    public class MessagesController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        //public JsonResult SendMessage(int ReceiverId, int JobId, string Description)
        //{
        //    ResponseMessage responseMessage = null;
        //    if (Session["user"] != null)
        //    {
        //        User user = (User)Session["user"];
        //        responseMessage = new BMessages().SendMessage(ReceiverId, JobId, Description, user.RegID);
        //    }

        //    return Json(responseMessage);
        //}

        //public JsonResult GetAllMessageList()
        //{
        //    List<UserMessagesMaster> vUserMessagesMaster = new List<UserMessagesMaster>();

        //    if (Session["user"] != null)
        //    {
        //        User user = (User)Session["user"];
        //        ResponseMessage responseMessage = new BMessages().GetMessageList(out vUserMessagesMaster, user.RegID);
        //    }


        //    return Json(vUserMessagesMaster);
        //}

        //public JsonResult GetAllMessageThread(int JobId, int ReceiverId)
        //{
        //    List<UserMessagesMaster> vUserMessagesMaster = new List<UserMessagesMaster>();

        //    if (Session["user"] != null)
        //    {
        //        User user = (User)Session["user"];
        //        ResponseMessage responseMessage = new BMessages().GetMessageThread(out vUserMessagesMaster, JobId, ReceiverId, user.RegID);
        //    }


        //    return Json(vUserMessagesMaster);
        //}

        //public JsonResult Changepassword(string Password, string NewPassword)
        //{
        //    ResponseMessage responseMessage = null;
        //    if (Session["user"] != null)
        //    {
        //        User user = (User)Session["user"];
        //        //responseMessage = new BMessages().Changepassword(Password, NewPassword, user.UserTypeID, user.RegID);
        //    }

        //    return Json(responseMessage);
        //}

    }
}