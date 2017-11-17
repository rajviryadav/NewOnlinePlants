using OnlinePlants.Business.BusinessLogicModel;
using OnlinePlants.Data;
using OnlinePlants.Model.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePlants.Business
{
    public class BAdmin
    {
        OnlinePlantsContext db = new OnlinePlantsContext();

        public ResponseMessage2<RegistrationMaster> GetUsersList(int UserTypeId, int PageNo, string JobSearchTerm)
        {
            ResponseMessage2<RegistrationMaster> responseMessage = new ResponseMessage2<RegistrationMaster>();

            List<RegistrationMaster> vjobsMaster = new List<RegistrationMaster>();

            try
            {
                var JM = (from r in db.tblRegistration
                          join u in db.tblUser on r.RegID equals u.RegID
                          select new
                          {
                              r.Name,
                              r.Email,
                              r.Phone,
                              r.CreatedDate,
                              r.RegID,
                              r.UserTypeID,
                              u.IsActive,
                              r.ProfileURL
                          }).Where(a => a.UserTypeID == UserTypeId);

                if (JobSearchTerm != "" && JobSearchTerm != null)
                {
                    JM = JM.Where(a => a.Name.Contains(JobSearchTerm));
                }


                // before paging calcul the total records
                responseMessage.TotalListRecords = JM.Count();

                if (PageNo == 1)
                {
                    JM = JM.Take(CommonMethods.PageSize);
                }

                if (PageNo > 1)
                {
                    JM = JM.OrderBy(a => a.RegID).Skip(CommonMethods.PageSize * (PageNo - 1)).Take(CommonMethods.PageSize);

                }


                JM.ToList().ForEach(rec =>
                {
                    vjobsMaster.Add(new RegistrationMaster()
                    {
                        RegID = rec.RegID,
                        Name = rec.Name == null ? rec.Email.Split('@')[0].ToString() : rec.Name,
                        Phone = rec.Phone,
                        Email = rec.Email,
                        RegistrationDate = rec.CreatedDate.ToString("MM/dd/yyyy"),
                        UserStatus = rec.IsActive == true ? "Deactive" : "Active",
                        UserTypeID = rec.UserTypeID,
                        ProfileURL = rec.ProfileURL != null ? rec.ProfileURL : "avatar-placeholder.png"

                    });
                });


                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

                responseMessage.objectList = vjobsMaster;

                responseMessage.TotalPages = Convert.ToInt32(Math.Ceiling((double)responseMessage.TotalListRecords / CommonMethods.PageSize));

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;
        }

        public ResponseMessage DeactiveUser(int UserTypeId, int UserId, int RegId)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {

                var User = db.tblUser.SingleOrDefault(a => a.UserTypeID == UserTypeId && a.RegID == UserId);

                if (User.IsActive == true)
                {
                    User.IsActive = false;
                }
                else
                {
                    User.IsActive = true;
                }
                User.UpdatedDate = DateTime.Now;
                db.SaveChanges();


                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RColorCode = ColorCodes.Red;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }
    }
}
