using OnlinePlants.Model.BusinessModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePlants.Business
{
    public static class CommonMethods
    {

        public static bool CheckValidPageUser(int Usertype, List<int> PageType, out string RedirectURL, string isSearchForRecruiter)
        {
            bool IsValid = true;
            RedirectURL = "";
            if (PageType.Contains(Usertype) == false)
            {
                IsValid = false;
            }

            if (Usertype == 1)
            {
                RedirectURL = "/Admin/Dashboard/";
            }
            if (Usertype == 2)
            {
                RedirectURL = "/JobSeeker/Dashboard/Index";
            }
            if (Usertype == 3)
            {
                RedirectURL = "/Employer/Dashboard/Index";
            }
            if (Usertype == 4)
            {
                RedirectURL = "/Employer/Dashboard/Index";
            }

            if (isSearchForRecruiter == "true")
            {
                RedirectURL = "/JobSeeker/Dashboard/Index";
            }


            return IsValid;

        }

        public static void GetUserValue(Registration registration, User user, int PageType, out string name, out string ImageURL,
                                        out bool IsValidPageUser, out string RedirectURL)
        {
            name = "";
            ImageURL = "";
            IsValidPageUser = false;
            RedirectURL = "";

            try
            {
                if (registration.Name == null || registration.Name == "")
                {
                    name = user.Username.Split('@')[0].ToString();
                }
                else
                {
                    name = registration.Name;
                }

                if (registration.ProfileURL == "" || registration.ProfileURL == null)
                {
                    ImageURL = "avatar-placeholder.png";
                }
                else
                {
                    ImageURL = registration.ProfileURL;
                }


                List<int> vPageType = new List<int>();
                vPageType.Add(PageType);
                if (user.UserTypeID == 3 && PageType == 4)
                {
                    vPageType.Add(3);
                }
                if (user.UserTypeID == 1 && PageType == 4)
                {
                    vPageType.Add(1);
                }


                IsValidPageUser = CheckValidPageUser(user.UserTypeID, vPageType, out RedirectURL, null);
            }
            catch (Exception ex)
            {

            }
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {

            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static int PageSize = 10;

        public static string EmailFrom = "notify@nomansadiq.pk";
        public static string EmailPassword = "Hellonomi1$";
        public static string EmailSMTP = "mail.nomansadiq.pk";
        public static int EmailPort = 25;

        public static string GetSubscribtionMailBody(string UserName, string hostname, out string Error)
        {
            Error = "";
            try
            {
                StringBuilder sbMailBody = new StringBuilder();
                sbMailBody.Append("<!DOCTYPE html>");
                sbMailBody.Append("<html>");
                sbMailBody.Append("<head>");
                sbMailBody.Append("<meta charset=\"utf-8\"/>");
                sbMailBody.Append("<meta content=\"IE=edge,chrome=1\" http-equiv=\"X-UA-Compatible\" />");
                sbMailBody.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
                sbMailBody.Append("<meta name=\"viewport\" content=\"width=device-width,user-scalable=no,initial-scale=1.0,maximum-scale=1.0\">");
                sbMailBody.Append("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css\">");
                sbMailBody.Append("<style type=\"text/css\">");
                sbMailBody.Append("html,body{ min-height:100%;background-color: #e6e7e8;}");
                sbMailBody.Append("@font-face {font-family: 'Ikaros Sans Light';src: url('Ikaros Light.otf');}");
                sbMailBody.Append("@font-face {font-family: 'Ikaros Sans Regular';src: url('Ikaros Regular.otf');}");
                sbMailBody.Append("a{font-weight:normal;}");
                sbMailBody.Append(".heading{padding-top:30px;}");
                sbMailBody.Append(".main{background-color: white;margin-top: 100px;margin-bottom: 100px;-webkit-box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.3);-moz-box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.3);box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.3);}");
                sbMailBody.Append(".content{padding: 0px 100px;}");
                sbMailBody.Append("img,button{display: block;margin: 0 auto;}");
                sbMailBody.Append("h1,h3,span{font-family: 'Ikaros Sans Regular', sans-serif;}");
                sbMailBody.Append("h1{color: #27a9e1;letter-spacing: 0.05em;font-size: 26px;}");
                sbMailBody.Append("h3{font-weight: bold;color: gray;font-size: 18px;}");
                sbMailBody.Append(".btn-primary{background-color: #27a9e1;border-radius: 0px;border-color: transparent;padding: 15px 60px;font-size: 21px;box-shadow: 0px 1px 0px #000;transition: all 0.5s ease 0s;text-align: center;}");
                sbMailBody.Append(".btn-primary:hover,.btn-primary:focus,.btn-primary:active,.btn-primary:visited,.btn-primary:active:hover{border-color: transparent;background-color: #1b9cc9;}");
                sbMailBody.Append(".buttn{padding:30px 0;}");
                sbMailBody.Append("hr{border: 3px solid lightgray;}");
                sbMailBody.Append(".divider hr{border: 1px solid lightgray;}");
                sbMailBody.Append(".main span {font-size: 17px;padding: 30px 0;}");
                sbMailBody.Append(".footer span {font-size: 16px;color:#lightgray}");
                sbMailBody.Append(".footer {padding: 30px 0;}");
                sbMailBody.Append(".links {padding: 30px 0;}");
                sbMailBody.Append(".links a {white-space: nowrap;display: inline-block;text-transform: uppercase;letter-spacing: 1px;font-family: \"Ikaros Sans Regular\", sans-serif;font-size: 1em;margin: 0 7px 0 5px;position: relative;min-height: 28px;line-height: 26px;}");
                sbMailBody.Append(".links a:hover {color: black;transition: all .5s;text-decoration: none;}");
                sbMailBody.Append("</style>");
                sbMailBody.Append("</head>");
                sbMailBody.Append("<body>");
                sbMailBody.Append("<div class=\"container\">");
                sbMailBody.Append("<div class=\"row\">");
                sbMailBody.Append("<div class=\"col-md-10 col-md-offset-1 main\">");
                sbMailBody.Append("<div class=\"row heading\">");
                sbMailBody.Append("<img class=\"img-responsive img-centered\" src=\"" + hostname + "/images/logo.jpg\" alt=''>");
                sbMailBody.Append("<hr>");
                sbMailBody.Append("</div>");
                sbMailBody.Append("<div class=\"row content\">");
                sbMailBody.Append("<div class=\"row divider\">");
                sbMailBody.Append("<div class=\"col-md-8 col-md-offset-2\">");
                sbMailBody.Append("</div>");
                sbMailBody.Append("</div></br>");
                sbMailBody.Append("<span>Hi " + UserName + ", ");
                sbMailBody.Append("</br></br>Welcome to OnlinePlants website, we dedicate time to our clients and candidates in effort to identify the most qualified candidates who best fits the employer's requirements for their open position.");
                sbMailBody.Append("</span>");
                sbMailBody.Append("<div class=\"row text-center buttn\"><button class=\"btn btn-primary btn-lg text-center\" onclick=\" return window.location.href='http://OnlinePlants.com'\">Get Started</button></div>");
                sbMailBody.Append("<h3>Have a great day !</h3>");
                sbMailBody.Append("<span>OnlinePlants Team</span></br>");
                sbMailBody.Append("<a href=\"http://OnlinePlants.com/\"><span>OnlinePlants.com<span></a></br></br>");
                sbMailBody.Append("<hr>");
                sbMailBody.Append("</div>");
                sbMailBody.Append("<div class=\"row text-center footer\">");
                sbMailBody.Append("<div class=\"col-md-10 col-md-offset-1\">");
                sbMailBody.Append("<h3>What&#8217;s OnlinePlants?</h3>");
                sbMailBody.Append("<span>OnlinePlants Recruitment Service LLC &#169; Our firm has been recognized Globally, for our Professional Career Recruiting & Consulting Services. We provide Career Seekers, Outstanding Opportunities Globally, and assist them to reach thier Highest Level of Potential.");
                sbMailBody.Append("</span> ");
                sbMailBody.Append("<div class=\"links\">");
                sbMailBody.Append("<a href=\"http://facebook.com/OnlinePlants\">facebook</a>");
                sbMailBody.Append("<a href=\"https://www.linkedin.com/in/OnlinePlants\">Linkedin</a>");
                sbMailBody.Append("<a href=\"http://twitter.com/OnlinePlants\">Twitter</a>");
                sbMailBody.Append("</div>");
                sbMailBody.Append("</div>");
                sbMailBody.Append("</div>");
                sbMailBody.Append("</div>");
                sbMailBody.Append("</div>");
                sbMailBody.Append("</div>");
                sbMailBody.Append("</body>");
                sbMailBody.Append("</html>");
                return sbMailBody.ToString();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return string.Empty;
        }

        public static void SendEmail(string To, string Subject, string Body, out string Error)
        {
            try
            {
                Error = "";

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = new System.Net.Mail.MailAddress(EmailFrom, "OnlinePlants");
                mail.To.Add(new System.Net.Mail.MailAddress(To));
                mail.Subject = Subject;
                mail.IsBodyHtml = true;
                mail.Body = Body;
                SmtpClient client = new SmtpClient();
                client.Port = EmailPort;
                client.Host = EmailSMTP;
                client.Credentials = new System.Net.NetworkCredential(EmailFrom, EmailPassword);
                client.EnableSsl = false;
                client.Send(mail);


            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

        }
    }
}
