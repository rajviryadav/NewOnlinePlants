using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace OnlinePlants.UI.Models
{
    public static class CommonDefault
    {
       

        public static bool CreateDefaultDirectory()
        {
            bool IsCreated = true;
            try
            {
                string CVPath = HttpContext.Current.Server.MapPath("~/Uploads/CVs");
                string ProfilePicturePath = HttpContext.Current.Server.MapPath("~/Images/PP");

                string SourceFileLoc = HttpContext.Current.Server.MapPath("~/Images/avatar-placeholder.png");
                string DefaultImagePath = HttpContext.Current.Server.MapPath("~/Images/PP/avatar-placeholder.png");






                if (!System.IO.Directory.Exists(CVPath))
                {
                    System.IO.Directory.CreateDirectory(CVPath);
                }

                if (!System.IO.Directory.Exists(ProfilePicturePath))
                {
                    System.IO.Directory.CreateDirectory(ProfilePicturePath);
                }

                if (!System.IO.File.Exists(DefaultImagePath))
                {
                    System.IO.File.Copy(SourceFileLoc, DefaultImagePath);
                }




            }
            catch (Exception ex)
            {
                IsCreated = false;
            }

            return IsCreated;

        }
    }
}