using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePlants.Data;
using OnlinePlants.Model.BusinessModel;

namespace OnlinePlants.Business
{
    public class BRegistration
    {
        OnlinePlantsContext db = new OnlinePlantsContext();
        public ResponseMessage UserRegistration(string Email, string Password, string UserType, string Name, out User vuser)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            vuser = null;
            try
            {
                if (db.tblRegistration.Where(a => a.Email == Email).Count() > 0 || db.tblUser.Where(u => u.Username == Email).Count() > 0)
                {
                    responseMessage.RCode = 2;
                    responseMessage.RMessage = "Email address already exists";
                }
                else
                {
                    Registration registration = new Registration();
                    registration.Email = Email;
                    registration.Name = Name;
                    registration.CreatedDate = DateTime.Now;
                    registration.UpdatedDate = DateTime.Now;
                    registration.UserTypeID = Convert.ToInt32(UserType);
                    db.tblRegistration.Add(registration);
                    db.SaveChanges();

                    User user = new User();
                    user.Username = Email;
                    user.UserPassword = Password;
                    user.UserTypeID = Convert.ToInt32(UserType);
                    user.IsActive = true;
                    user.RegID = registration.RegID;
                    user.CreatedDate = DateTime.Now;
                    user.UpdatedDate = DateTime.Now;
                    db.tblUser.Add(user);
                    db.SaveChanges();
                    vuser = user;
                    responseMessage.RCode = 1;
                    responseMessage.classobject = registration;
                    responseMessage.RMessage = "Your request has been successfully submitted. Please wait...";
                }

            }
            catch (Exception ex)
            {
                responseMessage.RCode = 0;
                responseMessage.RMessage = "System unable to process your request. Please come back later";
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage UserLogin(string Username, string Password, out User objuser)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            objuser = null;
            try
            {
                if (db.tblUser.Where(a => a.Username == Username && a.UserPassword == Password && a.IsActive == true).Count() == 0)
                {
                    responseMessage.RCode = 2;
                    responseMessage.RMessage = "Invalid Username/Email Address or Password";
                }
                else if (db.tblUser.Where(a => a.Username == Username && a.UserPassword == Password && a.IsActive == true).Count() > 0)
                {
                    objuser = db.tblUser.SingleOrDefault(a => a.Username == Username && a.UserPassword == Password);
                    int RegID = objuser.RegID;
                    Registration registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegID);
                    responseMessage.RCode = 1;
                    responseMessage.RMessage = "You are successfully logged in. Please wait...";
                    responseMessage.classobject = registration;

                }

            }
            catch (Exception ex)
            {
                responseMessage.RCode = 0;
                responseMessage.RMessage = "System unable to process your reques. Please come back later";
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage ForgetPassword(string Username)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                if (db.tblUser.Where(a => a.Username == Username && a.IsActive == true).Count() == 0)
                {
                    responseMessage.RCode = 2;
                    responseMessage.RMessage = "Incorrect Username or Email Address. Please re-enter";
                }
                else if (db.tblUser.Where(a => a.Username == Username && a.IsActive == true).Count() == 1)
                {
                    User user = db.tblUser.SingleOrDefault(a => a.Username == Username && a.IsActive == true);
                    string body = "Hi " + user.Username;
                    body += "<br /> Please find below your credentials";
                    body += "<br />";
                    body += "<br />Username: " + user.Username;
                    body += "<br />Password: " + user.UserPassword;

                    string Error = "";

                    CommonMethods.SendEmail(user.Username, "OnlinePlants - Reset Password", body, out Error);
                    if (Error == "")
                    {
                        responseMessage.RCode = 1;
                        responseMessage.RMessage = "Your password has been sent. Please check your email";
                        responseMessage.RColorCode = ColorCodes.ThemeColor;
                        responseMessage.classobject = null;
                    }
                    if (Error != "")
                    {
                        responseMessage.RCode = 2;
                        responseMessage.RColorCode = ColorCodes.Red;
                        responseMessage.RMessage = "System unable to process your reques. Please come back later";
                        responseMessage.classobject = null;


                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RCode = 0;
                responseMessage.RMessage = "System unable to process your reques. Please come back later";
                responseMessage.RException = ex.Message;
                responseMessage.RColorCode = ColorCodes.Red;
            }


            return responseMessage;

        }
    }
}
