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
    public class BMessages
    {
        OnlinePlantsContext db = new OnlinePlantsContext();

        public ResponseMessage SendMessage(int ReceiverId, int JobId, string Description, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                UserMessages createmessage = new UserMessages();
                createmessage.ReceiverId = ReceiverId;
                createmessage.SenderId = CreatedBy;
                createmessage.JobId = JobId;
                createmessage.MessageDetails = Description;

                createmessage.CreatedBy = CreatedBy;
                createmessage.CreatedDate = DateTime.Now;
                createmessage.UpdatedBy = CreatedBy;
                createmessage.UpdatedDate = DateTime.Now;

                db.tblMessages.Add(createmessage);
                db.SaveChanges();



                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestMessageSent;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RURL = "/JobSeeker/Dashboard";




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

        public ResponseMessage GetMessageList(out List<UserMessagesMaster> userMessagesMaster, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            userMessagesMaster = null;

            List<UserMessagesMaster> vUserMessagesMaster = new List<UserMessagesMaster>();

            try
            {
                var message = (from m in db.tblMessages
                               join r in db.tblRegistration on m.ReceiverId equals r.RegID
                               join j in db.tblJob on m.JobId equals j.JobID into grouped
                               from msg in grouped.DefaultIfEmpty()
                               group msg by new { m.SenderId, m.ReceiverId, r.Name, r.Email, msg.Title, msg.JobID } into all
                               select new
                               {

                                   all.Key.JobID,
                                   all.Key.ReceiverId,
                                   all.Key.SenderId,
                                   Name = all.Key.Name == null ? all.Key.Email : all.Key.Name,
                                   all.Key.Title

                               });

                message.Where(a => a.SenderId == CreatedBy).ToList().ForEach(rec =>
                {
                    vUserMessagesMaster.Add(new UserMessagesMaster()
                    {
                        ReceiverId = rec.ReceiverId,
                        JobId = rec.JobID,
                        SenderId = rec.SenderId,
                        Name = rec.Name,
                        Subject = rec.Title,

                    });
                });


                userMessagesMaster = vUserMessagesMaster;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

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

        public ResponseMessage GetMessageThread(out List<UserMessagesMaster> userMessagesMaster, int JobId, int ReceiverId, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            userMessagesMaster = null;

            List<int> senderIdList = new List<int>();
            senderIdList.Add(CreatedBy);
            senderIdList.Add(ReceiverId);



            List<UserMessagesMaster> vUserMessagesMaster = new List<UserMessagesMaster>();

            try
            {
                var message = (from m in db.tblMessages
                               join r in db.tblRegistration on m.SenderId equals r.RegID

                               select new
                               {
                                   r.ProfileURL,
                                   m.JobId,
                                   m.ReceiverId,
                                   m.MessageId,
                                   m.CreatedDate,
                                   m.SenderId,
                                   Name = r.Name == null ? r.Email : r.Name,
                                   m.MessageDetails
                               });

                message.Where(a => senderIdList.Contains(a.SenderId) && a.JobId == JobId).ToList().ForEach(rec =>
                {
                    vUserMessagesMaster.Add(new UserMessagesMaster()
                    {
                        MessageId = rec.MessageId,
                        PostedOn = rec.CreatedDate.ToString("dd/MM/yyyy"),
                        SenderId = rec.SenderId,
                        Name = rec.Name,
                        ProfileURL = rec.ProfileURL,
                        MessageDetails = rec.MessageDetails

                    });
                });


                userMessagesMaster = vUserMessagesMaster;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

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

        public ResponseMessage Changepassword(string Password, string Newpassword, int UserTypeID, int RegID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                User user = new User();

                if (db.tblUser.Where(a => a.UserTypeID == UserTypeID && a.RegID == RegID && a.UserPassword == Password).Count() == 1)
                {
                    user = db.tblUser.Single(a => a.UserTypeID == UserTypeID && a.RegID == RegID && a.UserPassword == Password);
                    user.UserPassword = Newpassword;
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;

                }
                else
                {
                    responseMessage.RCode = ErrorCode.Failure;
                    responseMessage.RMessage = "Invalid old password entered";
                    responseMessage.RColorCode = ColorCodes.Red;
                }

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

    }
}
