namespace OnlinePlants.Business.BusinessLogicModel
{
    public class RegistrationMaster
    {
        public int RegID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsProfilePrivate { get; set; }
        public bool IsNotificationOn { get; set; }
        public bool IsEmailForwardOn { get; set; }
        public bool IsMobileMessageOn { get; set; }
        public string ProfileURL { get; set; }
        public int UserTypeID { get; set; }
        public string RegistrationDate { get; set; }
        public int PackageID { get; set; }
        public string CVDocumentURL { get; set; }
        public bool IsAgreement { get; set; }
        public int DefaultCompanyId { get; set; }
        public string UserStatus { get; set; }
    }
}
