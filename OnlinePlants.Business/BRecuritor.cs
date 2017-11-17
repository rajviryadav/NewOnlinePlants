using OnlinePlants.Data;
using OnlinePlants.Model.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePlants.Business.BusinessLogicModel;

namespace OnlinePlants.Business
{

    public class BRecuritor
    {
        OnlinePlantsContext db = new OnlinePlantsContext();

        public int PageSize = 4;

        #region Account
        public ResponseMessage UpdateUserProfile(string ProfileURl, string Name, string Email, string Phone, bool IsProfilePrivate, bool IsNotificatioOn, bool IsEmailForwardOn, bool IsMessageOn, int RegId)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Registration registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegId);

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


        public ResponseMessage UpdatePackage(int RegId, int PackageID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Registration registration = db.tblRegistration.SingleOrDefault(a => a.RegID == RegId);
                registration.PackageID = PackageID;
                db.SaveChanges();

                User user = db.tblUser.SingleOrDefault(a => a.RegID == RegId && a.UserTypeID == 4);
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

        #endregion 

        #region dashboard

        public ResponseMessage2<JobsMaster> GetJobList(int RegId, int PageNo, int IsJobLive, string JobSearchTerm)
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
                    JM = JM.Where(a => a.RegID == RegId && a.IsJobLive == IsJobLive).Take(PageSize);
                }

                if (PageNo > 1)
                {
                    JM = JM.Where(a => a.RegID == RegId && a.IsJobLive == IsJobLive).OrderBy(a => a.JobId).Skip(PageSize * (PageNo - 1)).Take(PageSize);

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
                    });
                });


                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

                responseMessage.objectList = vjobsMaster;
                responseMessage.TotalListRecords = db.tblJob.Where(a => a.RegID == RegId && a.IsJobLive == IsJobLive).Count();
                responseMessage.TotalPages = Convert.ToInt32(Math.Ceiling((double)responseMessage.TotalListRecords / PageSize));

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
                                       CVDocumentURL = reg.CVDocumentURL,
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

        public ResponseMessage AddUpdateNewJob(int JobID, int RegID, string title, double Salary, int JobLocationID, int JobTypeId, int CompID, string Description, int IsJobLive)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (JobID > 0)
                {

                    Job job = db.tblJob.SingleOrDefault(a => a.JobID == JobID);
                    job.Title = title;
                    job.Description = Description;
                    job.RegID = RegID;
                    job.Salary = Salary;
                    job.JobLocationID = JobLocationID;
                    job.JobTypeID = JobTypeId;
                    job.JobLocationID = JobLocationID;
                    job.CompID = CompID;
                    job.IsJobLive = IsJobLive;

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
                    job.JobLocationID = JobLocationID;
                    job.JobTypeID = JobTypeId;
                    job.CompID = CompID;
                    job.CreatedBy = RegID;
                    job.IsJobLive = IsJobLive;
                    job.CreatedDate = DateTime.Now;
                    job.UpdatedBy = RegID;
                    job.UpdatedDate = DateTime.Now;
                    db.tblJob.Add(job);
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

        public ResponseMessage GetLocationList(out List<LocationsMaster> vLocationsList)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            List<LocationsMaster> Location = new List<LocationsMaster>();
            vLocationsList = null;
            try
            {
                db.tblLocations.ToList().ForEach(rec =>
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
                                     RegID = j.RegID

                                 }).SingleOrDefault(a => a.JobId == JobId && a.RegID == RegId);


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
                             join sumary in db.tblSummary on reg.RegID equals sumary.RegID
                             join s in db.tblSkills on reg.RegID equals s.RegID

                             select new
                             {
                                 reg.RegID,
                                 reg.Email,
                                 reg.Name,
                                 reg.ProfileURL,
                                 reg.CVDocumentURL,
                                 sumary.SummaryDetails

                             }).Distinct();

                query.ToList().ForEach(rec =>
                {
                    vjobSeekerMaster.Add(new JobSeekerMaster()
                    {
                        RegID = rec.RegID,
                        Name = rec.Name == null ? rec.Email.Split('@')[0].ToString() : rec.Name,
                        ProfileURL = rec.ProfileURL == null ? "avatar-placeholder.png" : rec.ProfileURL,
                        CVDocumentURL = rec.CVDocumentURL,
                        Summary = rec.SummaryDetails,

                    });
                });


                if (CategoryID != 0)
                {
                    vjobSeekerMaster = vjobSeekerMaster.Where(a => a.CategoryID == CategoryID).ToList();
                }
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
                        CreatedOn = rec.CreatedDate.ToString("dd/MM/yyyy"),

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
                    responseMessage.RMessage = Messages.RequestSuccess;

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
                    responseMessage.RMessage = Messages.RequestSuccess;


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
    }
}
