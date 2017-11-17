using OnlinePlants.Data;
using OnlinePlants.Model.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePlants.Business.BusinessLogicModel;
using Stripe;

namespace OnlinePlants.Business
{

    public class BEmployer
    {
        OnlinePlantsContext db = new OnlinePlantsContext();



        #region Account
        public ResponseMessage UpdateUserProfile(string ProfileURl, string Name, string Email, string Phone, bool IsProfilePrivate, bool IsNotificatioOn, bool IsEmailForwardOn,
            bool IsMessageOn, int RegId, int UserTypeID, string Password, string NewPassword)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Registration registration = new Registration();
                if (Password != "")
                {
                    User user = new User();
                    if (db.tblUser.Where(a => a.UserTypeID == UserTypeID && a.RegID == RegId && a.UserPassword == Password).Count() == 1)
                    {
                        user = db.tblUser.Single(a => a.UserTypeID == UserTypeID && a.RegID == RegId && a.UserPassword == Password);
                        user.UserPassword = NewPassword;
                        db.SaveChanges();

                        registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegId);

                        registration.Name = Name;
                        registration.Phone = Phone;
                        registration.IsProfilePrivate = IsProfilePrivate;
                        registration.IsNotificationOn = IsNotificatioOn;
                        registration.IsMobileMessageOn = IsMessageOn;
                        registration.IsEmailForwardOn = IsEmailForwardOn;

                        if (ProfileURl != "")
                        {
                            registration.ProfileURL = ProfileURl;
                        }

                        db.SaveChanges();

                        responseMessage.RColorCode = ColorCodes.ThemeColor;
                        responseMessage.RCode = ErrorCode.Success;
                        responseMessage.RMessage = Messages.RequestSuccess;
                        responseMessage.classobject = registration;

                    }
                    else
                    {
                        responseMessage.RCode = ErrorCode.Failure;
                        responseMessage.RMessage = "Invalid old password entered";
                        responseMessage.RColorCode = ColorCodes.Red;
                    }
                }
                else
                {
                    registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegId);

                    registration.Name = Name;
                    registration.Phone = Phone;
                    registration.IsProfilePrivate = IsProfilePrivate;
                    registration.IsNotificationOn = IsNotificatioOn;
                    registration.IsMobileMessageOn = IsMessageOn;
                    registration.IsEmailForwardOn = IsEmailForwardOn;

                    if (ProfileURl != "")
                    {
                        registration.ProfileURL = ProfileURl;
                    }

                    db.SaveChanges();

                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.classobject = registration;
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

        public ResponseMessage UpdateUserProfilePicture(string ProfileURl, int RegId)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Registration registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegId);


                if (ProfileURl != "")
                {
                    registration.ProfileURL = ProfileURl;
                }

                db.SaveChanges();

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.classobject = registration;

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

        public ResponseMessage ChangePassword(string OldPassword, string NewPassword, int RegId, int Usertype)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (db.tblUser.SingleOrDefault(a => a.RegID == RegId && a.UserTypeID == Usertype && a.UserPassword == OldPassword) == null)
                {
                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.SuccessUpdate;
                    responseMessage.RMessage = Messages.UserInvalid;
                }
                if (db.tblUser.SingleOrDefault(a => a.RegID == RegId && a.UserTypeID == Usertype && a.UserPassword == OldPassword) != null)
                {
                    User user = db.tblUser.SingleOrDefault(a => a.RegID == RegId && a.UserTypeID == Usertype && a.UserPassword == OldPassword);
                    db.SaveChanges();
                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
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

        public ResponseMessage LoadUserProfile(int RegId, out Registration registration)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            registration = null;
            try
            {
                registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegId);

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

                responseMessage.classobject = registration;
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


        public ResponseMessage UpdatePackage(int RegId, int PackageID, string Token, string Amount, string username)
        {

            StripeConfiguration.SetApiKey("sk_test_TghHPnrj5SeWHFv39KJVg8M5");
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                int TotalCents = Convert.ToInt32(Amount) * 100;
                var myCharge = new StripeChargeCreateOptions();
                myCharge.Amount = TotalCents;
                myCharge.Currency = "usd";
                myCharge.Capture = true;
                //    myCharge.SourceTokenOrExistingSourceId = Token;

                var chargeService = new StripeChargeService();
                //This is the recuring payment monthly so need to create a customer with appropreate plan in Stripe
                var myCustomer = new StripeCustomerCreateOptions();
                myCustomer.SourceToken = Token;
                switch (PackageID)
                {
                    case 1://$19
                        myCustomer.Description = "Plan19";
                        myCustomer.PlanId = "19PLAN";
                        break;
                    case 2://$59
                        myCustomer.Description = "Plan59";
                        myCustomer.PlanId = "59PLAN";
                        break;
                    case 3://$99
                        myCustomer.Description = "Plan99";
                        myCustomer.PlanId = "99PLAN";
                        break;
                }

                myCustomer.Email = username;
                var customerService = new StripeCustomerService();
                Stripe.StripeCustomer customer = customerService.Create(myCustomer);
                myCharge.CustomerId = customer.Id;
                StripeCharge stripeCharge = chargeService.Create(myCharge);

                if (stripeCharge.Paid == true)
                {
                    Payments payment = new Payments();
                    payment.Amount = stripeCharge.Amount;
                    payment.RegID = RegId;
                    payment.Payment_Des = stripeCharge.Description;
                    payment.CreatedDate = DateTime.Now;
                    payment.Currency = stripeCharge.Currency;
                    payment.BalanceTransactionId = stripeCharge.BalanceTransactionId;
                    payment.StripeCreated = stripeCharge.Created;
                    payment.StripeId = stripeCharge.Id;
                    payment.Stripepaid = stripeCharge.Paid;
                    payment.StripeObjectJson = stripeCharge.StripeResponse.ObjectJson;
                    payment.StripeRequestDate = stripeCharge.StripeResponse.RequestDate;
                    payment.StripeResponseJson = stripeCharge.StripeResponse.ResponseJson;
                    db.tblPayments.Add(payment);
                    db.SaveChanges();

                    Registration registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegId);
                    registration.PackageID = PackageID;
                    registration.IsPaymentDone = true;
                    db.SaveChanges();

                    User user = db.tblUser.SingleOrDefault(a => a.RegID == RegId && a.UserTypeID == 4);
                    user.IsActive = true;
                    db.SaveChanges();

                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccessAndWait;
                    responseMessage.RURL = "/Employer/Dashboard/";
                }
                if (stripeCharge.Paid == false)
                {
                    responseMessage.RColorCode = ColorCodes.Red;
                    responseMessage.RCode = ErrorCode.Failure;
                    responseMessage.RMessage = "System unable to process your payment. Please check your provided details.";

                }
            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                //responseMessage.RMessage = ex.StackTrace;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RException = ex.Message;
            }
            return responseMessage;
        }

        public ResponseMessage UpdateAgreement(int RegId, bool IsAgreement)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Registration registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegId);
                registration.IsAgreement = IsAgreement;
                db.SaveChanges();

                User user = db.tblUser.SingleOrDefault(a => a.RegID == RegId && a.UserTypeID == 3);
                user.IsActive = true;
                db.SaveChanges();

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccessAndWait;
                responseMessage.RURL = "/Employer/Dashboard/";
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

        public Boolean CheckCurrentPassowrd(string UserName, string Password)
        {
            bool result = false;

            if (db.tblUser.Any(a => a.Username == UserName && a.UserPassword == Password && a.IsActive == true))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        #endregion 

        #region dashboard

        public ResponseMessage2<JobsMaster> GetJobList(int RegId, int PageNo, int IsJobLive, string JobSearchTerm)
        {
            if (IsJobLive == 0)
            {

                return GetAllJobs(RegId, PageNo, JobSearchTerm);
            }
            else
            {
                return GetJobsByFilterId(RegId, PageNo, IsJobLive, JobSearchTerm);

            }
        }



        public ResponseMessage GetApplicantsList(int JobId, int RegID, out List<JobSeekerMaster> jobSeekerMaster)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            jobSeekerMaster = null;

            List<JobSeekerMaster> vJobSeeker = new List<JobSeekerMaster>();

            try
            {
                var JM = (from reg in db.tblRegistration
                          join JA in db.tblJobApplied on reg.RegID equals JA.RegID
                          select new { reg.Name, JA.JobID, reg.RegID, reg.Email, reg.CVDocumentURL, reg.ProfileURL, JA.JobAppliedID });



                JM.Where(a => a.JobID == JobId).ToList().ForEach(rec =>
                {
                    vJobSeeker.Add(new JobSeekerMaster()
                    {
                        Name = rec.Name == null ? rec.Email.Split('@')[0].ToString() : rec.Name,
                        RegID = rec.RegID,
                        CVDocumentURL = rec.CVDocumentURL == null ? "" : rec.CVDocumentURL,
                        ProfileURL = rec.ProfileURL == null ? "avatar-placeholder.png" : rec.ProfileURL,
                        JobAppliedID = rec.JobAppliedID,
                        JobID = rec.JobID


                    });
                });

                jobSeekerMaster = vJobSeeker;


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

        public ResponseMessage GetCoverletter(int JobAppliedID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                JobApplied ja = db.tblJobApplied.SingleOrDefault(a => a.JobAppliedID == JobAppliedID);
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.SuccessUpdate;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.classobject = ja;
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

        public ResponseMessage GetApplicantProfile(int ApplicantID, out JobSeekerMaster jobSeekerMaster)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            jobSeekerMaster = null;

            List<ExperienceMaster> ExperienceList = new List<ExperienceMaster>();
            List<EducationMaster> EducationList = new List<EducationMaster>();
            List<SkillsMaster> SkillsList = new List<SkillsMaster>();



            try
            {
                jobSeekerMaster = (from reg in db.tblRegistration
                                   join sum in db.tblSummary on reg.RegID equals sum.RegID
                                   select new JobSeekerMaster
                                   {
                                       Name = reg.Name,
                                       RegID = reg.RegID,
                                       Email = reg.Email,
                                       CVDocumentURL = reg.CVDocumentURL == null ? "" : reg.CVDocumentURL,
                                       ProfileURL = reg.ProfileURL == null ? "avatar-placeholder.png" : reg.ProfileURL,
                                       Summary = sum.SummaryDetails
                                   }).SingleOrDefault(a => a.RegID == ApplicantID);




                db.tblExperience.Where(a => a.RegID == ApplicantID).ToList().ForEach(rec =>
                {
                    ExperienceList.Add(new ExperienceMaster()
                    {
                        Companyname = rec.Companyname,
                        Designation = rec.Designation,
                        CompanyLogo = rec.CompanyLogo,
                        DateFrom = rec.DateFrom.ToString("dd/MM/yyyy"),
                        DateTo = rec.DateTo.ToString("dd/MM/yyyy"),
                        ExpID = rec.ExpID
                    });
                });

                db.tblEducation.Where(a => a.RegID == ApplicantID).ToList().ForEach(rec =>
                {
                    EducationList.Add(new EducationMaster()
                    {
                        EduID = rec.EduID,
                        University = rec.University,
                        DateFrom = rec.DateFrom.ToString("dd/MM/yyyy"),
                        DateTo = rec.DateTo.ToString("dd/MM/yyyy"),
                        Degree = rec.Degree,
                    });
                });


                var skills = (
                              from s in db.tblSkills
                              select new { s.Name, s.RegID, s.YearOfExperience, s.SkillsID });

                skills.Where(a => a.RegID == ApplicantID).ToList().ForEach(rec =>
                {
                    SkillsList.Add(new SkillsMaster()
                    {
                        SkillsID = rec.SkillsID,
                        SkillsName = rec.Name,
                        YearOfExperience = rec.YearOfExperience,
                    });
                });


                jobSeekerMaster.ExperienceList = ExperienceList;
                jobSeekerMaster.EducationList = EducationList;
                jobSeekerMaster.SkillsList = SkillsList;


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


        #endregion

        #region Jobs

        public ResponseMessage AddUpdateNewJob(int JobID, int RegID, string title, double Salary, int JobLocationID, int JobTypeId, int CompID,
                                                string Description, int IsJobLive, int StateId, string CityName, int JDID, int STCode,bool isSavedJob, int isSplitedJob)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DateTime ExpireDate = DateTime.Now.AddDays(db.tblJobDays.SingleOrDefault(a => a.JDID == JDID).JDDays);
                if (JobID > 0)
                {




                    Job job = db.tblJob.SingleOrDefault(a => a.JobID == JobID);
                    job.Title = title;
                    job.Description = Description;
                    job.Salary = Salary;
                    job.JobLocationID = JobLocationID;
                    job.JobTypeID = JobTypeId;
                    job.JobLocationID = JobLocationID;
                    job.CompID = CompID;
                    job.IsJobLive = IsJobLive;
                    job.StateID = StateId;
                    job.CityName = CityName;
                    job.JDID = JDID;
                    job.ExpiryDate = ExpireDate;
                    job.SalaryType = STCode;
                    job.IsSplitedJob = isSplitedJob;
                    job.UpdatedBy = RegID;
                    job.UpdatedDate = DateTime.Now;
                    db.SaveChanges();

                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    if (isSavedJob == true)
                    {
                        responseMessage.RCode = ErrorCode.Success;
                    }
                    else
                    {
                        responseMessage.RCode = ErrorCode.SuccessUpdate;
                    }

                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.RURL = "/Employer/Jobs/Jobdetails?id=" + job.JobID;

                }
                if (JobID == 0)
                {

                    Job job = new Job();
                    job.Title = title;
                    job.JobLocationID = JobLocationID;
                    job.Description = Description;
                    job.RegID = RegID;
                    job.Salary = Salary;
                    job.SalaryType = STCode;
                    job.JobLocationID = JobLocationID;
                    job.JobTypeID = JobTypeId;
                    job.CompID = CompID;
                    job.CreatedBy = RegID;
                    job.IsJobLive = IsJobLive;
                    job.StateID = StateId;
                    job.CityName = CityName;
                    job.JDID = JDID;
                    job.ExpiryDate = ExpireDate;
                    job.CreatedDate = DateTime.Now;
                    job.UpdatedBy = RegID;
                    job.UpdatedDate = DateTime.Now;
                    job.IsSplitedJob = isSplitedJob;
                    db.tblJob.Add(job);
                    db.SaveChanges();

                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;

                    // Send Email

                    string Error = "";
                    string body = "";
                    body += "Hi , <br /><br /> You have posted a job successfully.";
                    Registration reg = db.tblRegistration.SingleOrDefault(a => a.RegID == RegID);
                    CommonMethods.SendEmail(reg.Email, "Post Job: " + title, body, out Error);


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



        public ResponseMessage GetCategoryList(out List<CategoryMaster> vCategoryList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<CategoryMaster> CategoryList = new List<CategoryMaster>();
            vCategoryList = null;
            try
            {
                db.tblCategory.ToList().ForEach(rec =>
                {
                    CategoryList.Add(new CategoryMaster()
                    {
                        CategoryId = rec.CategoryID,
                        Title = rec.Title
                    });
                });

                vCategoryList = CategoryList;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage GetStateList(out List<StateMaster> vCategoryList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<StateMaster> CategoryList = new List<StateMaster>();
            vCategoryList = null;
            try
            {
                db.tblState.ToList().ForEach(rec =>
                {
                    CategoryList.Add(new StateMaster()
                    {
                        StateID = rec.StateID,
                        StateName = rec.StateName
                    });
                });

                vCategoryList = CategoryList;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage GetJobdaysList(out List<JobDaysMaster> vCategoryList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<JobDaysMaster> CategoryList = new List<JobDaysMaster>();
            vCategoryList = null;
            try
            {
                db.tblJobDays.ToList().ForEach(rec =>
                {
                    CategoryList.Add(new JobDaysMaster()
                    {
                        JDID = rec.JDID,
                        JDDays = rec.JDDays
                    });
                });

                vCategoryList = CategoryList;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage GetLocationList(out List<LocationsMaster> vLocationsList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<LocationsMaster> Location = new List<LocationsMaster>();
            vLocationsList = null;
            try
            {
                db.tblLocations.OrderBy(a => a.JobLocationID).ToList().ForEach(rec =>
                {
                    Location.Add(new LocationsMaster()
                    {
                        JobLocationID = rec.JobLocationID,
                        Name = rec.Name,


                    });
                });

                vLocationsList = Location;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage GetJobTypeList(out List<JobTypeMaster> vJobtypeList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<JobTypeMaster> JobTypeList = new List<JobTypeMaster>();
            vJobtypeList = null;
            try
            {
                db.tblJobType.ToList().ForEach(rec =>
                {
                    JobTypeList.Add(new JobTypeMaster()
                    {
                        JobTypeID = rec.JobTypeID,
                        Name = rec.Name,


                    });
                });

                vJobtypeList = JobTypeList;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage GetCompanyList(int RegID, out List<CompanyMaster> vcompanyList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<CompanyMaster> companyList = new List<CompanyMaster>();
            vcompanyList = null;
            try
            {
                db.tblCompany.Where(a => a.RegID == RegID).ToList().ForEach(rec =>
                {
                    companyList.Add(new CompanyMaster()
                    {
                        CompID = rec.CompID,
                        Name = rec.Name
                    });
                });

                vcompanyList = companyList;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage GetJob(int JobId, int RegId)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                var jobMaster = (from j in db.tblJob
                                 join loc in db.tblLocations on j.JobLocationID equals loc.JobLocationID
                                 join jt in db.tblJobType on j.JobTypeID equals jt.JobTypeID
                                 join com in db.tblCompany on j.CompID equals com.CompID
                                 join js in db.tblJobDays on j.JDID equals js.JDID
                                 join sts in db.tblState on j.StateID equals sts.StateID into leftjointable
                                 from leftjoin in leftjointable.DefaultIfEmpty()
                                 select new JobsMaster
                                 {
                                     JobId = j.JobID,
                                     Title = j.Title,
                                     CategoryId = j.CategoryID,
                                     Description = j.Description,
                                     Salary = j.Salary,
                                     JobLocationID = loc.JobLocationID,
                                     LocationName = loc.Name,
                                     JobTypeID = jt.JobTypeID,
                                     JobTypeName = jt.Name,
                                     CompID = com.CompID,
                                     CompanyName = com.Name,
                                     RegID = j.RegID,
                                     CityName = j.CityName,
                                     StateID = j.StateID,
                                     StateName = leftjoin.StateName,
                                     JDID = js.JDID,
                                     JDDays = js.JDDays,
                                     SalaryCode = j.SalaryType,
                                     SalaryName = "",
                                     IsJobLive = j.IsJobLive




                                 }).SingleOrDefault(a => a.JobId == JobId && a.RegID == RegId);

                if (CommonMethods.salaryType().SingleOrDefault(a => a.SalaryCode == jobMaster.SalaryCode) != null)
                {
                    jobMaster.SalaryName = CommonMethods.salaryType().SingleOrDefault(a => a.SalaryCode == jobMaster.SalaryCode).SalaryName;
                }


                if (jobMaster != null)
                {
                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.classobject = jobMaster;
                }
                if (jobMaster == null)
                {
                    responseMessage.RCode = ErrorCode.SuccessUpdate;
                    responseMessage.RMessage = Messages.RequestFaild;
                }


            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage DeleteJob(int JobId, int RegId)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                db.tblJobApplied.RemoveRange(db.tblJobApplied.Where(a => a.JobID == JobId && a.RegID == RegId));
                db.SaveChanges();
                db.tblJob.RemoveRange(db.tblJob.Where(a => a.JobID == JobId && a.RegID == RegId));
                db.SaveChanges();
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RURL = "/Employer/Dashboard";
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

        public ResponseMessage HireTheApplicant(int JobAppliedID, int JobID, string Description)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                JobApplied jobApplied = db.tblJobApplied.SingleOrDefault(a => a.JobAppliedID == JobAppliedID);
                Job job = db.tblJob.SingleOrDefault(a => a.JobID == JobID);

                if (jobApplied != null)
                {
                    jobApplied.JobAcceptDes = Description;
                    db.SaveChanges();
                }
                if (job != null)
                {
                    if (job.IsJobLive == 5)
                    {
                        job.IsSplitedJob = 1;
                        job.IsJobLive = 6;
                        db.SaveChanges();





                        Job CreateJob = new Job();
                        CreateJob.RegID = jobApplied.RegID;
                        CreateJob.Title = job.Title;
                        CreateJob.Description = job.Description;
                        CreateJob.CreatedBy = jobApplied.RegID;
                        CreateJob.CreatedDate = DateTime.Now;
                        CreateJob.UpdatedBy = jobApplied.RegID;
                        CreateJob.UpdatedDate = DateTime.Now;
                        CreateJob.Salary = job.Salary;
                        CreateJob.JobTypeID = job.JobTypeID;
                        CreateJob.CompID = db.tblRegistration.SingleOrDefault(a => a.RegID == jobApplied.RegID).DefaultCompanyId;
                        CreateJob.IsJobLive = 1;
                        CreateJob.JobLocationID = job.JobLocationID;
                        CreateJob.SplitedJobId = job.JobID;

                        db.tblJob.Add(CreateJob);
                        db.SaveChanges();

                    }
                    else
                    {
                        job.IsJobLive = 6;
                        db.SaveChanges();
                    }



                }

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.SuccessUpdate;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RURL = "";




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

        public ResponseMessage GetSalaryType(out List<SalaryType> vCategoryList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            vCategoryList = CommonMethods.salaryType();

            responseMessage.RColorCode = ColorCodes.ThemeColor;
            responseMessage.RCode = ErrorCode.Success;
            responseMessage.RMessage = Messages.RequestSuccess;

            return responseMessage;

        }

        public ResponseMessage2<JobsMaster> GetAllJobs(int RegId, int PageNo, string JobSearchTerm)
        {
            ResponseMessage2<JobsMaster> responseMessage = new ResponseMessage2<JobsMaster>();

            List<JobsMaster> vjobsMaster = new List<JobsMaster>();

            try
            {
                var JM = (from J in db.tblJob
                          join c in db.tblCompany on J.CompID equals c.CompID
                          join JA in db.tblJobApplied on J.JobID equals JA.JobID into jobmaster
                          from jm in jobmaster.DefaultIfEmpty()
                          group jm by new { J.JobID, J.CategoryID, J.RegID, J.Title, J.CreatedDate, J.IsJobLive, c.Name } into grouped
                          select new
                          {
                              JobId = grouped.Key.JobID,
                              Title = grouped.Key.Title,
                              CategoryId = grouped.Key.CategoryID,
                              RegID = grouped.Key.RegID,
                              TotalJobs = grouped.Count(a => a.JobAppliedID != null),
                              PostedDate = grouped.Key.CreatedDate,
                              IsJobLive = grouped.Key.IsJobLive,
                              CompanyName = grouped.Key.Name
                          });

                if (JobSearchTerm != "" && JobSearchTerm != null)
                {
                    JM = JM.Where(a => a.Title.Contains(JobSearchTerm));
                }


                if (PageNo == 1)
                {
                    JM = JM.Where(a => a.RegID == RegId).Take(CommonMethods.PageSize);
                }

                if (PageNo > 1)
                {
                    JM = JM.Where(a => a.RegID == RegId).OrderBy(a => a.JobId).Skip(CommonMethods.PageSize * (PageNo - 1)).Take(CommonMethods.PageSize);

                }


                JM.ToList().ForEach(rec =>
                {
                    vjobsMaster.Add(new JobsMaster()
                    {
                        JobId = rec.JobId,
                        Title = rec.Title,
                        CategoryId = rec.CategoryId,
                        RegID = rec.RegID,
                        TotalJobs = rec.TotalJobs,
                        PostedDate = rec.PostedDate.ToString("dd/MM/yyyy"),
                        CompanyName = rec.CompanyName,
                        IsJobLive = rec.IsJobLive
                    });
                });


                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

                responseMessage.objectList = vjobsMaster;
                responseMessage.TotalListRecords = db.tblJob.Where(a => a.RegID == RegId).Count();
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
        public ResponseMessage2<JobsMaster> GetJobsByFilterId(int RegId, int PageNo, int IsJobLive, string JobSearchTerm)
        {
            if (IsJobLive == 5)
            {
                return GetSplitedJobs(RegId,PageNo,IsJobLive,JobSearchTerm);
            }
            else
            {
                return GetNonSplitedJobs(RegId, PageNo, IsJobLive, JobSearchTerm);

            }
            
          
        }

        public ResponseMessage SaveJob(int JobID, int RegID, string title, double Salary, int JobLocationID, int JobTypeId, int CompID,
                                               string Description, int IsJobLive, int StateId, string CityName, int JDID, int STCode)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                DateTime ExpireDate = DateTime.Now.AddDays(db.tblJobDays.SingleOrDefault(a => a.JDID == JDID).JDDays);
                if (JobID > 0)
                {




                    Job job = db.tblJob.SingleOrDefault(a => a.JobID == JobID);
                    job.Title = title;
                    job.Description = Description;
                    job.Salary = Salary;
                    job.JobLocationID = JobLocationID;
                    job.JobTypeID = JobTypeId;
                    job.JobLocationID = JobLocationID;
                    job.CompID = CompID;
                    job.IsJobLive = IsJobLive;
                    job.StateID = StateId;
                    job.CityName = CityName;
                    job.JDID = JDID;
                    job.ExpiryDate =ExpireDate ;
                    job.SalaryType = STCode;

                    job.UpdatedBy = RegID;
                    job.UpdatedDate = DateTime.Now;
                    db.SaveChanges();

                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.SuccessUpdate;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.RURL = "/Employer/Jobs/Jobdetails?id=" + job.JobID;

                }
                if (JobID == 0)
                {

                    Job job = new Job();
                    job.Title = title;
                    job.JobLocationID = JobLocationID;
                    job.Description = Description;
                    job.RegID = RegID;
                    job.Salary = Salary;
                    job.SalaryType = STCode;
                    job.JobLocationID = JobLocationID;
                    job.JobTypeID = JobTypeId;
                    job.CompID = CompID;
                    job.CreatedBy = RegID;
                    job.IsJobLive = IsJobLive;
                    job.StateID = StateId;
                    job.CityName = CityName;
                    job.JDID = JDID;
                    job.ExpiryDate = ExpireDate;
                    job.CreatedDate = DateTime.Now;
                    job.UpdatedBy = RegID;
                    job.UpdatedDate = DateTime.Now;
                    db.tblJob.Add(job);
                    db.SaveChanges();

                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;

                    responseMessage.RURL = "/Employer/Dashboard";


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

        public Boolean CloseJob(int JobID)
        {
            bool result = false;
            try
            {
                var job = db.tblJob.Where(x => x.JobID == JobID && x.IsJobLive!=6).FirstOrDefault();
                if (job != null)
                {
                    job.IsJobLive = 6;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;

        }
        public Boolean ArchiveJob(int JobID)
        {
            bool result = false;
            try
            {
                var job = db.tblJob.Where(x => x.JobID == JobID && x.IsJobLive != 3).FirstOrDefault();
                if (job != null)
                {
                    job.IsJobLive = 3;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {

            }
            return result;

        }
        public ResponseMessage2<JobsMaster> GetSplitedJobs(int RegId, int PageNo, int IsJobLive, string JobSearchTerm)
        {
            ResponseMessage2<JobsMaster> responseMessage = new ResponseMessage2<JobsMaster>();

            List<JobsMaster> vjobsMaster = new List<JobsMaster>();

            try
            {
                var JM = (from J in db.tblJob
                          join c in db.tblCompany on J.CompID equals c.CompID
                          join JA in db.tblJobApplied on J.JobID equals JA.JobID into jobmaster
                          from jm in jobmaster.DefaultIfEmpty()
                          where J.IsSplitedJob==1
                          group jm by new { J.JobID, J.CategoryID, J.RegID, J.Title, J.CreatedDate, J.IsJobLive, c.Name,J.IsSplitedJob } into grouped
                          select new
                          {
                              JobId = grouped.Key.JobID,
                              Title = grouped.Key.Title,
                              CategoryId = grouped.Key.CategoryID,
                              RegID = grouped.Key.RegID,
                              TotalJobs = grouped.Count(a => a.JobAppliedID != null),
                              PostedDate = grouped.Key.CreatedDate,
                              IsJobLive = grouped.Key.IsJobLive,
                              CompanyName = grouped.Key.Name,
                              IsSplitedJob=grouped.Key.IsSplitedJob
                             
                          });

                if (JobSearchTerm != "" && JobSearchTerm != null)
                {
                    JM = JM.Where(a => a.Title.Contains(JobSearchTerm));
                }


                if (PageNo == 1)
                {
                    JM = JM.Where(a => a.RegID == RegId && a.IsSplitedJob== 1).Take(CommonMethods.PageSize);
                }

                if (PageNo > 1)
                {
                    JM = JM.Where(a => a.RegID == RegId && a.IsSplitedJob == 1).OrderBy(a => a.JobId).Skip(CommonMethods.PageSize * (PageNo - 1)).Take(CommonMethods.PageSize);

                }


                JM.ToList().ForEach(rec =>
                {
                    vjobsMaster.Add(new JobsMaster()
                    {
                        JobId = rec.JobId,
                        Title = rec.Title,
                        CategoryId = rec.CategoryId,
                        RegID = rec.RegID,
                        TotalJobs = rec.TotalJobs,
                        PostedDate = rec.PostedDate.ToString("dd/MM/yyyy"),
                        CompanyName = rec.CompanyName,
                        IsJobLive = rec.IsJobLive,
                        IsSplitJob=rec.IsSplitedJob
                    });
                });


                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

                responseMessage.objectList = vjobsMaster;
                responseMessage.TotalListRecords = db.tblJob.Where(a => a.RegID == RegId && a.IsSplitedJob == 1).Count();
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
        public ResponseMessage2<JobsMaster> GetNonSplitedJobs(int RegId, int PageNo, int IsJobLive, string JobSearchTerm)
        {
            ResponseMessage2<JobsMaster> responseMessage = new ResponseMessage2<JobsMaster>();

            List<JobsMaster> vjobsMaster = new List<JobsMaster>();

            try
            {
                var JM = (from J in db.tblJob
                          join c in db.tblCompany on J.CompID equals c.CompID
                          join JA in db.tblJobApplied on J.JobID equals JA.JobID into jobmaster
                          from jm in jobmaster.DefaultIfEmpty()
                          where J.IsJobLive == IsJobLive
                          group jm by new { J.JobID, J.CategoryID, J.RegID, J.Title, J.CreatedDate, J.IsJobLive, c.Name } into grouped
                          select new
                          {
                              JobId = grouped.Key.JobID,
                              Title = grouped.Key.Title,
                              CategoryId = grouped.Key.CategoryID,
                              RegID = grouped.Key.RegID,
                              TotalJobs = grouped.Count(a => a.JobAppliedID != null),
                              PostedDate = grouped.Key.CreatedDate,
                              IsJobLive = grouped.Key.IsJobLive,
                              CompanyName = grouped.Key.Name
                          });

                if (JobSearchTerm != "" && JobSearchTerm != null)
                {
                    JM = JM.Where(a => a.Title.Contains(JobSearchTerm));
                }


                if (PageNo == 1)
                {
                    JM = JM.Where(a => a.RegID == RegId && a.IsJobLive == IsJobLive).Take(CommonMethods.PageSize);
                }

                if (PageNo > 1)
                {
                    JM = JM.Where(a => a.RegID == RegId && a.IsJobLive == IsJobLive).OrderBy(a => a.JobId).Skip(CommonMethods.PageSize * (PageNo - 1)).Take(CommonMethods.PageSize);

                }


                JM.ToList().ForEach(rec =>
                {
                    vjobsMaster.Add(new JobsMaster()
                    {
                        JobId = rec.JobId,
                        Title = rec.Title,
                        CategoryId = rec.CategoryId,
                        RegID = rec.RegID,
                        TotalJobs = rec.TotalJobs,
                        PostedDate = rec.PostedDate.ToString("dd/MM/yyyy"),
                        CompanyName = rec.CompanyName,
                        IsJobLive = rec.IsJobLive
                    });
                });


                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

                responseMessage.objectList = vjobsMaster;
                responseMessage.TotalListRecords = db.tblJob.Where(a => a.RegID == RegId && a.IsJobLive == IsJobLive).Count();
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
        #endregion

        #region Applicant

        public ResponseMessage GetAllApplicant(string ApplicantName, int CategoryID, out List<JobSeekerMaster> jobSeekerMaster)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            jobSeekerMaster = null;


            List<JobSeekerMaster> vjobSeekerMaster = new List<JobSeekerMaster>();

            try
            {
                var query = (from reg in db.tblRegistration
                             select new
                             {
                                 reg.RegID,
                                 reg.Email,
                                 reg.Name,
                                 reg.ProfileURL,
                                 reg.CVDocumentURL,
                                 reg.UserTypeID

                             }).Where(a => a.UserTypeID == 2).Distinct();

                query.ToList().ForEach(rec =>
                {
                    vjobSeekerMaster.Add(new JobSeekerMaster()
                    {
                        RegID = rec.RegID,
                        Name = rec.Name == null ? rec.Email.Split('@')[0].ToString() : rec.Name,
                        ProfileURL = rec.ProfileURL == null ? "avatar-placeholder.png" : rec.ProfileURL,
                        CVDocumentURL = rec.CVDocumentURL == null ? "" : rec.CVDocumentURL,

                    });
                });


                //if (CategoryID != 0)
                //{
                //    vjobSeekerMaster = vjobSeekerMaster.Where(a => a.CategoryID == CategoryID).ToList();
                //}
                if (ApplicantName != "")
                {
                    vjobSeekerMaster = vjobSeekerMaster.Where(a => a.Name.Contains(ApplicantName)).ToList();
                }

                jobSeekerMaster = vjobSeekerMaster;



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

        #endregion

        #region Company CRUD

        public ResponseMessage GetCompanyListWithDetails(int RegID, out List<CompanyMasterForFullDetails> vcompanyList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<CompanyMasterForFullDetails> companyList = new List<CompanyMasterForFullDetails>();
            vcompanyList = null;
            try
            {
                db.tblCompany.Where(a => a.RegID == RegID).ToList().ForEach(rec =>
                {
                    companyList.Add(new CompanyMasterForFullDetails()
                    {
                        CompID = rec.CompID,
                        Name = rec.Name,
                        Description = rec.Description,
                        CreatedOn = rec.CreatedDate.ToString("MM/dd/yyyy"),

                    });
                });

                vcompanyList = companyList;

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage FindCompany(int CompID, int RegId)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                Company company = db.tblCompany.SingleOrDefault(a => a.CompID == CompID && a.RegID == RegId);


                if (company != null)
                {
                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.classobject = company;
                }
                if (company == null)
                {
                    responseMessage.RCode = ErrorCode.SuccessUpdate;
                    responseMessage.RMessage = Messages.RequestFaild;
                }


            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        public ResponseMessage AddUpdateCompany(int CompID, int RegID, string Name, string Description)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {


                if (CompID > 0)
                {
                    Company company = db.tblCompany.SingleOrDefault(a => a.CompID == CompID);
                    company.Name = Name;
                    company.Description = Description;
                    company.UpdatedBy = RegID;
                    company.UpdatedDate = DateTime.Now;
                    db.SaveChanges();

                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.SuccessUpdate;
                    responseMessage.RMessage = "Name of the company and Description are updated successfully.";

                }
                if (CompID == 0)
                {
                    Company company = new Company();
                    company.Name = Name;
                    company.Description = Description;
                    company.RegID = RegID;

                    company.CreatedBy = RegID;
                    company.CreatedDate = DateTime.Now;
                    company.UpdatedBy = RegID;
                    company.UpdatedDate = DateTime.Now;
                    db.tblCompany.Add(company);
                    db.SaveChanges();

                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = "Company created successfully";


                }



            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.InnerException.InnerException.Message;
            }


            return responseMessage;

        }

        public ResponseMessage DeleteCompany(int CompID, int RegId)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {


                int[] JobIds = (from job in db.tblJob select (int)job.JobID).ToArray();



                db.tblJobApplied.RemoveRange(db.tblJobApplied.Where(a => JobIds.Contains(a.JobID) && a.RegID == RegId));
                db.SaveChanges();
                db.tblJob.RemoveRange(db.tblJob.Where(a => a.CompID == CompID && a.RegID == RegId));
                db.SaveChanges();
                db.tblCompany.Remove(db.tblCompany.SingleOrDefault(a => a.CompID == CompID));
                db.SaveChanges();


                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RURL = "/Employer/Dashboard";
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

        #endregion

        #region

        public ResponseMessage AddPaymentDetails()
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<CompanyMasterForFullDetails> companyList = new List<CompanyMasterForFullDetails>();

            try
            {



                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

            }
            catch (Exception ex)
            {
                responseMessage.RCode = ErrorCode.Failure;
                responseMessage.RMessage = Messages.RequestException;
                responseMessage.RException = ex.Message;
            }


            return responseMessage;

        }

        #endregion
    }
}
