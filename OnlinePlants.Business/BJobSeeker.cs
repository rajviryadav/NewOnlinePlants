using OnlinePlants.Business.BusinessLogicModel;
using OnlinePlants.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePlants.Model.BusinessModel;


namespace OnlinePlants.Business
{
    public class BJobSeeker
    {
        OnlinePlantsContext db = new OnlinePlantsContext();

        #region  Dashboard


        public ResponseMessage GetJobList(int RegId, out List<JobsMaster> jobsMaster, string JobTitle, int CategoryID, string JobPostedType)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            jobsMaster = null;

            List<JobsMaster> vjobsMaster = new List<JobsMaster>();
            var JobPostedArray = JobPostedType.Split(',');
            List<int> JobPostList = new List<int>();
            foreach (var item in JobPostedArray)
            {
                JobPostList.Add(Convert.ToInt32(item));
            }

            try
            {

                var JobsList = (from j in db.tblJob
                                join c in db.tblCompany on j.CompID equals c.CompID
                                join jt in db.tblJobType on j.JobTypeID equals jt.JobTypeID
                                join loc in db.tblLocations on j.JobLocationID equals loc.JobLocationID
                                select new
                                {
                                    j.JobID,
                                    j.Title,
                                    j.Description,
                                    j.Salary,
                                    j.CreatedDate,
                                    c.Name,
                                    LocationName = loc.Name,
                                    loc.JobLocationID,
                                    JobTypeName = jt.Name,
                                    j.IsJobLive,
                                    j.ExpiryDate


                                });

                if (CategoryID != 0)
                {
                    JobsList = JobsList.Where(a => a.JobLocationID == CategoryID);
                }

                if (JobTitle != "")
                {
                    JobsList = JobsList.Where(a => a.Title.Contains(JobTitle));
                }


                JobsList.Where(a => JobPostList.Contains(a.IsJobLive) && a.ExpiryDate >= DateTime.Now).ToList().ForEach(rec =>
                {
                    vjobsMaster.Add(new JobsMaster() 
                    {
                        JobId = rec.JobID,
                        Title = rec.Title,
                        Description = rec.Description,
                        //Description = rec.Description.Length > 100 ? rec.Description.Substring(0, 100) : rec.Description,
                        Salary = rec.Salary,
                        PostedDate = rec.CreatedDate.ToString("dd/MM/yyyy"),
                        CompanyName = rec.Name,
                        LocationName = rec.LocationName,

                        JobTypeName = rec.JobTypeName
                    });
                });




                jobsMaster = vjobsMaster;

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
        public ResponseMessage GetJobListForRecruiter(int RegId, out List<JobsMaster> jobsMaster, string JobTitle, int CategoryID, string JobPostedType, int UserTypeId)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            jobsMaster = null;

            List<JobsMaster> vjobsMaster = new List<JobsMaster>();
            var JobPostedArray = JobPostedType.Split(',');
            List<int> JobPostList = new List<int>();
            foreach (var item in JobPostedArray)
            {
                JobPostList.Add(Convert.ToInt32(item));
            }

            try
            {

                var JobsList = (from j in db.tblJob
                                join c in db.tblCompany on j.CompID equals c.CompID
                                join jt in db.tblJobType on j.JobTypeID equals jt.JobTypeID
                                join loc in db.tblLocations on j.JobLocationID equals loc.JobLocationID
                                where j.IsJobLive==4 || j.IsSplitedJob==1
                                select new
                                {
                                    j.JobID,
                                    j.Title,
                                    j.Description,
                                    j.Salary,
                                    j.CreatedDate,
                                    c.Name,
                                    LocationName = loc.Name,
                                    loc.JobLocationID,
                                    JobTypeName = jt.Name,
                                    j.IsJobLive,
                                    j.ExpiryDate


                                });

                if (CategoryID != 0)
                {
                    JobsList = JobsList.Where(a => a.JobLocationID == CategoryID);
                }

                if (JobTitle != "")
                {
                    JobsList = JobsList.Where(a => a.Title.Contains(JobTitle));
                }


                JobsList.Where(a => JobPostList.Contains(a.IsJobLive) && a.ExpiryDate >= DateTime.Now).ToList().ForEach(rec =>
                {
                    vjobsMaster.Add(new JobsMaster()
                    {
                        JobId = rec.JobID,
                        Title = rec.Title,
                        Description = rec.Description,
                        //Description = rec.Description.Length > 100 ? rec.Description.Substring(0, 100) : rec.Description,
                        Salary = rec.Salary,
                        PostedDate = rec.CreatedDate.ToString("dd/MM/yyyy"),
                        CompanyName = rec.Name,
                        LocationName = rec.LocationName,

                        JobTypeName = rec.JobTypeName
                    });
                });




                jobsMaster = vjobsMaster;

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

        public ResponseMessage GetJob(int JobId, int RegId)

        {
            ResponseMessage responseMessage = new ResponseMessage();

            try
            {
                JobsMaster jobMaster = (from j in db.tblJob
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

                                            LocationName = loc.Name,
                                            JobTypeID = jt.JobTypeID,
                                            JobTypeName = jt.Name,
                                            CompID = com.CompID,
                                            CompanyName = com.Name,
                                            RegID = j.RegID,
                                            IsJobLive=j.IsJobLive
                                           

                                        }).SingleOrDefault(a => a.JobId == JobId);

                var jobapplied = db.tblJobApplied.SingleOrDefault(a => a.JobID == JobId && a.RegID == RegId);

                if (jobapplied != null)
                {
                    jobMaster.Coverletter = jobapplied.CoverLetter;
                    jobMaster.JobAppliedID = jobapplied.JobAppliedID;
                    jobMaster.AppliedStatus = jobapplied.AppliedStatus;
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

        public ResponseMessage ApplyJob(int JobID, string Coverletter, int AppliedStatus, int CreatedBy,string HostName)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                JobApplied JA = db.tblJobApplied.SingleOrDefault(a => a.JobID == JobID && a.RegID == CreatedBy);
                if (JA == null)
                {
                    JobApplied jobApplied = new JobApplied();
                    jobApplied.JobID = JobID;
                    jobApplied.RegID = CreatedBy;
                    jobApplied.CoverLetter = Coverletter;
                    jobApplied.AppliedStatus = AppliedStatus;
                    jobApplied.CreatedBy = CreatedBy;
                    jobApplied.CreatedDate = DateTime.Now;
                    jobApplied.UpdatedBy = CreatedBy;
                    jobApplied.UpdatedDate = DateTime.Now;
                    db.tblJobApplied.Add(jobApplied);
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccessAndWait;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;

                    SendEmail(JobID,CreatedBy,HostName);
                    responseMessage.RURL = "/JobSeeker/Dashboard";


                }
                if (JA != null)
                {
                    //   JobApplied jobApplied = new JobApplied();
                    JA.JobID = JobID;
                    JA.RegID = CreatedBy;
                    JA.CoverLetter = Coverletter;
                    JA.AppliedStatus = AppliedStatus;
                    JA.CreatedBy = CreatedBy;
                    JA.CreatedDate = DateTime.Now;
                    JA.UpdatedBy = CreatedBy;
                    JA.UpdatedDate = DateTime.Now;                  
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccessAndWait;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;

                    SendEmail(JobID, CreatedBy, HostName);
                    responseMessage.RURL = "/JobSeeker/Dashboard";


                }
                else
                {
                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = "You already applied for this job.";
                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RURL = "/JobSeeker/Jobs/Index?Id="+ JobID;

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

        public ResponseMessage2<JobsMaster> GetJobAppliedList(int RegId, int PageNo, int AppliedStatus, string SearchTerm)
        {
            ResponseMessage2<JobsMaster> responseMessage = new ResponseMessage2<JobsMaster>();



            List<JobsMaster> vjobsMaster = new List<JobsMaster>();

            try
            {

                var JobsList = (from j in db.tblJob
                                join c in db.tblCompany on j.CompID equals c.CompID
                                join jt in db.tblJobType on j.JobTypeID equals jt.JobTypeID
                                join loc in db.tblLocations on j.JobLocationID equals loc.JobLocationID
                                join ja in db.tblJobApplied on j.JobID equals ja.JobID
                                select new
                                {
                                    UserAppliedJobId = ja.RegID,
                                    AppliedDate = ja.CreatedDate,
                                    j.JobID,
                                    j.Title,
                                    j.Description,
                                    j.Salary,
                                    j.CreatedDate,
                                    c.Name,
                                    LocationName = loc.Name,
                                    JobTypeName = jt.Name,
                                    ja.AppliedStatus,
                                    ja.JobAppliedID

                                });

                if (SearchTerm != "" && SearchTerm != null)
                {
                    JobsList = JobsList.Where(a => a.Title.Contains(SearchTerm));
                }


                if (PageNo == 1)
                {
                    JobsList = JobsList.Where(a => a.UserAppliedJobId == RegId && a.AppliedStatus == AppliedStatus).Take(CommonMethods.PageSize);
                }

                if (PageNo > 1)
                {
                    JobsList = JobsList.Where(a => a.UserAppliedJobId == RegId && a.AppliedStatus == AppliedStatus).OrderBy(a => a.JobAppliedID).Skip(CommonMethods.PageSize * (PageNo - 1)).Take(CommonMethods.PageSize);

                }


                JobsList.ToList().ForEach(rec =>
                {
                    vjobsMaster.Add(new JobsMaster()
                    {
                        JobId = rec.JobID,
                        Title = rec.Title,
                        Description = rec.Description.Length > 100 ? rec.Description.Substring(0, 100) : rec.Description,
                        Salary = rec.Salary,
                        PostedDate = rec.AppliedDate.ToString("dd/MM/yyyy"),
                        CompanyName = rec.Name,
                        LocationName = rec.LocationName,

                        JobTypeName = rec.JobTypeName,


                    });
                });





                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;

                responseMessage.objectList = vjobsMaster;
                responseMessage.TotalListRecords = db.tblJobApplied.Where(a => a.RegID == RegId && a.AppliedStatus == AppliedStatus).Count();
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

        public void SendEmail(int JobID, int CreatedBy, string HostName)
        {
            string applicantName= db.tblRegistration.SingleOrDefault(a => a.RegID == CreatedBy).Name;
            var jobCreatedBy = (from j in db.tblJob
                                join r in db.tblRegistration on j.RegID equals r.RegID
                                where j.JobID==JobID
                                select new JobCreator()
                                {
                                    Email = r.Email,
                                    RegID = r.RegID,
                                    Name=r.Name
                                }).SingleOrDefault();

            if (jobCreatedBy != null)
            {
                string Error = "";
                string body = "";
                string title = db.tblJob.SingleOrDefault(a => a.JobID == JobID && a.RegID == jobCreatedBy.RegID).Title;
                body += CommonMethods.GetJobAppliedMailBody(applicantName,jobCreatedBy.RegID,jobCreatedBy.Name,HostName,title);
                
                CommonMethods.SendEmail(jobCreatedBy.Email, "New applicant applied for your job: " + title, body,out Error);
            }

        }

        #endregion

        #region Profile Summary

        public ResponseMessage UpdateSummary(string SummaryDescription, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Summary summary = db.tblSummary.SingleOrDefault(a => a.CreatedBy == CreatedBy);
                if (summary == null)
                {
                    Summary Create = new Model.BusinessModel.Summary();
                    Create.SummaryDetails = SummaryDescription;
                    Create.RegID = CreatedBy;
                    Create.CreatedBy = CreatedBy;
                    Create.CreatedDate = DateTime.Now;
                    Create.UpdatedBy = CreatedBy;
                    Create.UpdatedDate = DateTime.Now;
                    db.tblSummary.Add(Create);
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccessAndWait;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                    responseMessage.RURL = "/JobSeeker/Dashboard";


                }
                else if (summary != null)
                {
                    summary.SummaryDetails = SummaryDescription;
                    summary.UpdatedBy = CreatedBy;
                    summary.UpdatedDate = DateTime.Now;
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;

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

        public ResponseMessage UpdateCVName(string Filename, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Registration registration = db.tblRegistration.SingleOrDefault(a => a.RegID == CreatedBy);

                if (registration != null)
                {
                    registration.CVDocumentURL = Filename;
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;
                }
                else
                {

                    responseMessage.RCode = ErrorCode.Failure;
                    responseMessage.RMessage = Messages.RequestFaild;
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

        public ResponseMessage GetSummaryDetails(int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();


            List<JobsMaster> vjobsMaster = new List<JobsMaster>();

            try
            {
                var jobSeekerMaster = (from reg in db.tblRegistration
                                       join sum in db.tblSummary on reg.RegID equals sum.RegID
                                       select new JobSeekerMaster
                                       {
                                           Name = reg.Name == null ? reg.Email : reg.Name,
                                           RegID = reg.RegID,
                                           Email = reg.Email,
                                           CVDocumentURL = reg.CVDocumentURL,
                                           ProfileURL = reg.ProfileURL == null ? "avatar-placeholder.png" : reg.ProfileURL,
                                           Summary = sum.SummaryDetails
                                       }).SingleOrDefault(a => a.RegID == CreatedBy);

                //Summary summary = db.tblSummary.SingleOrDefault(a => a.CreatedBy == CreatedBy);

                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.classobject = jobSeekerMaster;

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

        #region Profile Experience

        public ResponseMessage AddUpdateExperience(int ExpId, string FromDate, string ToDate, string Compnayname, string Designation, string Description, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                if (ExpId == 0)
                {
                    Experience Create = new Experience();
                    Create.RegID = CreatedBy;
                    Create.Companyname = Compnayname;
                    Create.DateFrom = Convert.ToDateTime(FromDate);
                    Create.DateTo = Convert.ToDateTime(ToDate);
                    Create.Designation = Designation;
                    Create.Description = Description;

                    Create.CreatedBy = CreatedBy;
                    Create.CreatedDate = DateTime.Now;
                    Create.UpdatedBy = CreatedBy;
                    Create.UpdatedDate = DateTime.Now;
                    db.tblExperience.Add(Create);
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccessAndWait;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;



                }
                else if (ExpId > 0)
                {
                    Experience experience = db.tblExperience.SingleOrDefault(a => a.ExpID == ExpId && a.CreatedBy == CreatedBy);
                    experience.RegID = CreatedBy;
                    experience.Companyname = Compnayname;
                    experience.DateFrom = Convert.ToDateTime(FromDate);
                    experience.DateTo = Convert.ToDateTime(ToDate);
                    experience.Designation = Designation;
                    experience.Description = Description;

                    experience.UpdatedBy = CreatedBy;
                    experience.UpdatedDate = DateTime.Now;
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;

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

        public ResponseMessage DeleteExperience(int ExpId, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Experience ex = db.tblExperience.SingleOrDefault(a => a.ExpID == ExpId && a.RegID == CreatedBy);
                db.tblExperience.Remove(ex);
                db.SaveChanges();
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
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

        public ResponseMessage GetExperienceList(int RegId, out List<ExperienceMaster> experienceMaster)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            experienceMaster = null;

            List<ExperienceMaster> vExperienceMaster = new List<ExperienceMaster>();

            try
            {
                db.tblExperience.Where(a => a.RegID == RegId).ToList().ForEach(rec =>
                {
                    vExperienceMaster.Add(new ExperienceMaster()
                    {
                        Companyname = rec.Companyname,
                        DateFrom = rec.DateFrom.ToString("MM/dd/yyyy"),
                        DateTo = rec.DateTo.ToString("MM/dd/yyyy"),
                        Description = rec.Description,
                        Designation = rec.Designation,
                        ExpID = rec.ExpID

                    });
                });



                experienceMaster = vExperienceMaster;

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

        public ResponseMessage FindExperience(int ExpId, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                ExperienceMaster exp = new ExperienceMaster();
                Experience experience = db.tblExperience.SingleOrDefault(a => a.ExpID == ExpId && a.CreatedBy == CreatedBy);

                exp.DateFrom = experience.DateFrom.ToString("dd/MM/yyyy");
                exp.DateTo = experience.DateTo.ToString("dd/MM/yyyy");
                exp.CompanyLogo = experience.Companyname;
                exp.Description = experience.Description;
                exp.Designation = experience.Designation;
                exp.ExpID = experience.ExpID;


                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.classobject = exp;
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

        #region Profile Education

        public ResponseMessage AddUpdateEducation(int EduID, string FromDate, string ToDate, string University, string Degree, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                if (EduID == 0)
                {
                    Education Create = new Education();
                    Create.RegID = CreatedBy;
                    Create.University = University;
                    Create.DateFrom = Convert.ToDateTime(FromDate);
                    Create.DateTo = Convert.ToDateTime(ToDate);
                    Create.Degree = Degree;


                    Create.CreatedBy = CreatedBy;
                    Create.CreatedDate = DateTime.Now;
                    Create.UpdatedBy = CreatedBy;
                    Create.UpdatedDate = DateTime.Now;
                    db.tblEducation.Add(Create);
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccessAndWait;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;



                }
                else if (EduID > 0)
                {
                    Education education = db.tblEducation.SingleOrDefault(a => a.EduID == EduID && a.CreatedBy == CreatedBy);
                    education.RegID = CreatedBy;
                    education.University = University;
                    education.DateFrom = Convert.ToDateTime(FromDate);
                    education.DateTo = Convert.ToDateTime(ToDate);
                    education.Degree = Degree;


                    education.UpdatedBy = CreatedBy;
                    education.UpdatedDate = DateTime.Now;
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;

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

        public ResponseMessage DeleteEducation(int EduID, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Education ex = db.tblEducation.SingleOrDefault(a => a.EduID == EduID && a.RegID == CreatedBy);
                db.tblEducation.Remove(ex);
                db.SaveChanges();
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
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

        public ResponseMessage GetEducationList(int RegId, out List<EducationMaster> experienceMaster)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            experienceMaster = null;

            List<EducationMaster> vExperienceMaster = new List<EducationMaster>();

            try
            {




                db.tblEducation.Where(a => a.RegID == RegId).ToList().ForEach(rec =>
                {
                    vExperienceMaster.Add(new EducationMaster()
                    {
                        University = rec.University,
                        DateFrom = rec.DateFrom.ToString("MM/dd/yyyy"),
                        DateTo = rec.DateTo.ToString("MM/dd/yyyy"),
                        Degree = rec.Degree,
                        EduID = rec.EduID

                    });
                });



                experienceMaster = vExperienceMaster;

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

        public ResponseMessage FindEducation(int EduId, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                EducationMaster exp = new EducationMaster();
                Education experience = db.tblEducation.SingleOrDefault(a => a.EduID == EduId && a.CreatedBy == CreatedBy);

                exp.DateFrom = experience.DateFrom.ToString("dd/MM/yyyy");
                exp.DateTo = experience.DateTo.ToString("dd/MM/yyyy");
                exp.University = experience.University;
                exp.Degree = experience.Degree;
                exp.EduID = experience.EduID;


                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.classobject = exp;
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

        #region Profile skills

        public ResponseMessage AddUpdateSkills(int SkillsID, string Name, float YearOfExperience, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {

                if (SkillsID == 0)
                {
                    Skills Create = new Skills();
                    Create.RegID = CreatedBy;
                    Create.Name = Name;
                    Create.YearOfExperience = YearOfExperience;
                    Create.CreatedBy = CreatedBy;
                    Create.CreatedDate = DateTime.Now;
                    Create.UpdatedBy = CreatedBy;
                    Create.UpdatedDate = DateTime.Now;
                    db.tblSkills.Add(Create);
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccessAndWait;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;



                }
                else if (SkillsID > 0)
                {
                    Skills education = db.tblSkills.SingleOrDefault(a => a.SkillsID == SkillsID && a.CreatedBy == CreatedBy);
                    education.RegID = CreatedBy;
                    education.Name = Name;
                    education.YearOfExperience = YearOfExperience;


                    education.UpdatedBy = CreatedBy;
                    education.UpdatedDate = DateTime.Now;
                    db.SaveChanges();

                    responseMessage.RCode = ErrorCode.Success;
                    responseMessage.RMessage = Messages.RequestSuccess;
                    responseMessage.RColorCode = ColorCodes.ThemeColor;

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

        public ResponseMessage DeleteSkills(int SkillsID, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                Skills ex = db.tblSkills.SingleOrDefault(a => a.SkillsID == SkillsID && a.RegID == CreatedBy);
                db.tblSkills.Remove(ex);
                db.SaveChanges();
                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
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

        public ResponseMessage GetSkillsList(int RegId, out List<SkillsMaster> experienceMaster)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            experienceMaster = null;

            List<SkillsMaster> vExperienceMaster = new List<SkillsMaster>();

            try
            {




                db.tblSkills.Where(a => a.RegID == RegId).ToList().ForEach(rec =>
                {
                    vExperienceMaster.Add(new SkillsMaster()
                    {
                        SkillsName = rec.Name,
                        YearOfExperience = rec.YearOfExperience,
                        SkillsID = rec.SkillsID

                    });
                });



                experienceMaster = vExperienceMaster;

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

        public ResponseMessage FindSkills(int SkillsID, int CreatedBy)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                SkillsMaster exp = new SkillsMaster();
                Skills experience = db.tblSkills.SingleOrDefault(a => a.SkillsID == SkillsID && a.CreatedBy == CreatedBy);


                exp.SkillsName = experience.Name;
                exp.YearOfExperience = experience.YearOfExperience;
                exp.SkillsID = experience.SkillsID;


                responseMessage.RCode = ErrorCode.Success;
                responseMessage.RMessage = Messages.RequestSuccess;
                responseMessage.RColorCode = ColorCodes.ThemeColor;
                responseMessage.classobject = exp;
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

    }
}
