using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePlants.Business
{
    public static class Messages
    {
        public static string RequestSuccess = "You request has been processed successfully.";
        public static string RequestFaild = "You are not authorized to do this operation";
        public static string RequestException = "System unable to process your request. Please come back later";
        public static string UserInvalid = "Invalid username and password";
        public static string RequestSuccessAndWait = "Your request has been successfully processed. Please wait...";
        public static string RequestMessageSent = "Your message has been successfully sent.";

    }

    public static class ColorCodes
    {
        public static string ThemeColor = "#58BA2B";
        public static string Green = "";
        public static string Blue = "";
        public static string Red = "#FF0000";
    }

    public static class ErrorCode
    {
        public static int Success = 1;
        public static int SuccessUpdate = 2;
        public static int Failure = 0;

    }

    public class SalaryType
    {
        public int SalaryCode { get; set; }
        public string SalaryName { get; set; }

        public SalaryType(int code, string name)
        {
            SalaryCode = code;
            SalaryName = name;
        }
    }



}
