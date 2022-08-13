using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;
using ESimSol.Reports;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class CareerController : Controller
    {
        #region Declaration
        Candidate _oCandidate = new Candidate();
        List<Candidate> _oCandidates = new List<Candidate>();
        #endregion

        public ActionResult View_Candidate(int nCID)
        {
            _oCandidate = new Candidate();
            if (nCID > 0)
            {
                _oCandidate = Candidate.Get(nCID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oCandidate.CandidateEducations = CandidateEducation.Gets(nCID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oCandidate.CandidateTrainings = CandidateTraining.Gets(nCID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oCandidate.CandidateExperiences = CandidateExperience.Gets(nCID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oCandidate.CandidateReferences = CandidateReference.Gets(nCID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return PartialView(_oCandidate);
        }
        public ActionResult View_NewCirculars(string sMessage,int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
           List<Circular>  oCirculars = new List<Circular>();
           string sSql = "SELECT * FROM View_Circular WHERE EndDate>GETDATE() AND IsActive = 1 AND ApproveBy>0";
           this.Session[SessionInfo.wcfSessionID] = new Guid();
           oCirculars = Circular.GetNewCirculars(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
           TempData["sMsg"] = sMessage;
           return View(oCirculars);
        }
        public ActionResult View_CircularDetail(int nCircularID)
        {
            Circular oCircular = new Circular();
            List<Circular> oCirculars = new List<Circular>();
            string sSql = "SELECT * FROM View_Circular WHERE CircularID=" + nCircularID;
            oCirculars = Circular.GetNewCirculars(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCircular = oCirculars[0];
            return PartialView(oCircular);
        }
        #region Save/ Delete

        [HttpPost]
        public ActionResult Save(HttpPostedFileBase file1, Candidate oCandidate)
        {
            string sPassword = "";
            try
            {
                
                //if (_oMailController.IsValidMail(oCandidate.Email) == false)
                //{
                //    oCandidate.ErrorMessage = "In Valid Email address. Please Type Valid Mail Address.";
                //    return RedirectToAction("ViewApplication", "Application", new { nEmpID = 0, sMessage = oCandidate.ErrorMessage });
                //}

                #region Photo
                if (file1 != null && file1.ContentLength > 0)
                {
                    Image oPhotoImage = Image.FromStream(file1.InputStream, true, true);
                    //oImage.Save(@"F:\images\" + file.FileName + ".jpg");

                    //Orginal Image to byte array
                    byte[] aPhotoImageInByteArray = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        oPhotoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        aPhotoImageInByteArray = ms.ToArray();
                    }

                    #region Image Size Validation
                    double nMaxLength = 500 * 1024;
                    if (aPhotoImageInByteArray.Length > nMaxLength)
                    {
                        oCandidate.ErrorMessage = "Your profile picture size is " + aPhotoImageInByteArray.Length + "KB! You can selecte maximum 100KB image";
                        return PartialView(oCandidate);
                    }
                    else
                    {
                        oCandidate.Photo = aPhotoImageInByteArray;
                    }
                    #endregion
                }
                #endregion
                
                if (oCandidate.CandidateID <= 0)
                {
                    //sPassword = oCandidate.Password;
                    oCandidate = oCandidate.InsertNewCandidate((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    
                }
                else
                {
                    oCandidate = oCandidate.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }

            }
            catch (Exception ex)
            {
                oCandidate = new Candidate();
                oCandidate.ErrorMessage = ex.Message;
            }
            if (oCandidate.ErrorMessage != "" && oCandidate.CandidateID <= 0)
            {
                return RedirectToAction("View_NewCirculars", "Career", new {  sMessage = oCandidate.ErrorMessage });
            }
            //else if (((User)(Session[SessionInfo.CurrentUser])).UserID != Guid.Empty)
            //{
            //    return RedirectToAction("View_NewCirculars", "Career", new {  sMessage = oCandidate.ErrorMessage });
            //}
            else if (oCandidate.CandidateID > 0)
            {
                //string sBodyInformation = "<h2>Welcome,   " + oCandidate.FullName + ".......</h2><br> User Name: " + oCandidate.Code + " <br>Password: " + sPassword + " <br><div style='float:right; Font-size:11px;'> Created at " + DateTime.Now.ToString("dd MMM yyyy hh:mm") + "</div>";
                //_oMailController.MailNewAccount("New Account", sBodyInformation, oCandidate.Email);

                return RedirectToAction("View_NewCirculars", "Career", new { sMessage = "Account Created Successfully. Please Login by your username and password and enter other information." });
            }
            else
            {
                return RedirectToAction("View_NewCirculars", "Career", new { sMessage = "SignUp is not possible please try again !" });
            }
        }

        public System.Drawing.Image GetPhoto(int nid)//EmployeeID
        {
            Candidate oCandidate = new Candidate();
            oCandidate = Candidate.Get(nid, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oCandidate.Photo != null)
            {
                MemoryStream m = new MemoryStream(oCandidate.Photo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Candidate Login
        [HttpPost]
        public ActionResult LogIn(string txtUsername, string txtPassword)
        {
            CandidateUser oCandidateUser = new ESimSol.BusinessObjects.CandidateUser();
            try
            {
                if ((CandidateUser)Session[SessionInfo.CurrentUser] == null || ((CandidateUser)Session[SessionInfo.CurrentUser]).UserID == 0)
                {
                    //oCandidateUser = ESimSol.BusinessObjects.CandidateUser.CandidateUserLogin(((User)(Session[SessionInfo.CurrentUser])).UserID, txtUsername, txtPassword);
                    if (oCandidateUser.UserID > 0)
                    {
                        this.Session.Add(SessionInfo.wcfSessionID, oCandidateUser.UserID);
                        this.Session.Add(SessionInfo.CurrentUser, oCandidateUser);

                        //this.Session.Add(SessionInfo.BaseAddress, "/HCM");
                        this.Session.Add(SessionInfo.BaseAddress, "");
                    }
                }
                else
                {
                    oCandidateUser = (CandidateUser)Session[SessionInfo.CurrentUser];
                }
                if (oCandidateUser != null && oCandidateUser.CandidateID > 0)
                {
                    //TempData["message"] = oCandidateUser.ErrorMessage;
                    return RedirectToAction("View_Candidate", "Career", new { nCID = oCandidateUser.CandidateID }); 
                }
            }
            catch (Exception exp)
            {
                
                TempData["message"] = exp.Message;
                oCandidateUser.ErrorMessage = exp.Message;
                
            }
             return RedirectToAction("View_NewCirculars", "Career", new { sMessage = oCandidateUser.ErrorMessage }); }
        #endregion Candidate Login

        #region Candidate Education

        [HttpPost]
        public JsonResult CandidateEducationIU(CandidateEducation oCE)
        {
            CandidateEducation oCEducation = new CandidateEducation();
            try
            {
                oCEducation = oCE;
                if (oCEducation.CEID > 0)
                {
                    oCEducation = oCEducation.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else { oCEducation = oCEducation.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            }
            catch (Exception ex)
            {
                oCEducation = new CandidateEducation();
                oCEducation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCEducation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CandidateEducationDelete(CandidateEducation oCE)
        {
            CandidateEducation oCEducation = new CandidateEducation();
            try
            {
                oCEducation = oCE;
                oCEducation = oCEducation.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oCEducation = new CandidateEducation();
                oCEducation.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCEducation);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion Candidate Education

        #region Candidate Training
        [HttpPost]
        public JsonResult CandidateTrainingIU(CandidateTraining oCT)
        {
            CandidateTraining oCTr = new CandidateTraining();
            try
            {
                oCTr = oCT;
                if (oCTr.CTID > 0)
                {
                    oCTr = oCTr.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else { oCTr = oCTr.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            }
            catch (Exception ex)
            {
                oCTr = new CandidateTraining();
                oCTr.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCTr);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CandidateTrainingDelete(CandidateTraining oCT)
        {
            CandidateTraining oCTr = new CandidateTraining();
            try
            {
                oCTr = oCT;
                oCTr = oCTr.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oCTr = new CandidateTraining();
                oCTr.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCTr);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Candidate Training

        #region Candidate Experience
        [HttpPost]
        public JsonResult CandidateExperienceIU(CandidateExperience oCExperience)
        {
            CandidateExperience oCExp = new CandidateExperience();
            try
            {
                oCExp = oCExperience;
                if (oCExp.CExpID > 0)
                {
                    oCExp = oCExp.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else { oCExp = oCExp.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            }
            catch (Exception ex)
            {
                oCExp = new CandidateExperience();
                oCExp.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCExp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CandidateExperienceDelete(CandidateExperience oCExperience)
        {
            CandidateExperience oCExp = new CandidateExperience();
            try
            {
                oCExp = oCExperience;
                oCExp = oCExp.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oCExp = new CandidateExperience();
                oCExp.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCExp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Candidate Experience

        #region Candidate Reference
        [HttpPost]
        public JsonResult CandidateReferenceIU(CandidateReference oCReference)
        {
            CandidateReference oCRef = new CandidateReference();
            try
            {
                oCRef = oCReference;
                if (oCRef.CRefID > 0)
                {
                    oCRef = oCRef.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else { oCRef = oCRef.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            }
            catch (Exception ex)
            {
                oCRef = new CandidateReference();
                oCRef.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCRef);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CandidateReferenceDelete(CandidateReference oCReference)
        {
            CandidateReference oCRef = new CandidateReference();
            try
            {
                oCRef = oCReference;
                oCRef = oCRef.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oCRef = new CandidateReference();
                oCRef.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCRef);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Candidate Reference

        #region Online Application
        [HttpPost]
        public JsonResult OnlineApplication(int nCircularID, double nExpextedSalary, string sUsernmae, string sPassword)
        {
            string sMsg = "";
            CandidateUser oCandidateUser = new CandidateUser();
            CandidateApplication oCandidateApplication = new CandidateApplication();
            try
            {
                if ((CandidateUser)Session[SessionInfo.CurrentUser] == null || ((CandidateUser)Session[SessionInfo.CurrentUser]).UserID == 0)
                {
                    this.Session[SessionInfo.wcfSessionID] = new Guid();
                    //oCandidateUser = ESimSol.BusinessObjects.CandidateUser.CandidateUserLogin(((User)(Session[SessionInfo.CurrentUser])).UserID, sUsernmae, sPassword);
                    if (oCandidateUser.UserID > 0)
                    {
                        this.Session.Add(SessionInfo.wcfSessionID, oCandidateUser.UserID);
                        this.Session.Add(SessionInfo.CurrentUser, oCandidateUser);

                        //this.Session.Add(SessionInfo.BaseAddress, "/HCM");
                        this.Session.Add(SessionInfo.BaseAddress, "");
                    }
                }
                else
                {
                    oCandidateUser = (CandidateUser)Session[SessionInfo.CurrentUser];
                }
                if (oCandidateUser.CandidateID > 0 && oCandidateUser.ErrorMessage == "")
                {
                    oCandidateApplication.CandidateID = oCandidateUser.CandidateID;
                    oCandidateApplication.CircularID = nCircularID;
                    oCandidateApplication.ExpectedSalary = nExpextedSalary;
                    oCandidateApplication = oCandidateApplication.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    if (oCandidateApplication.CAppID > 0 && oCandidateApplication.ErrorMessage == "")
                    {
                        sMsg = "Applied successfully !";
                        LogOut();
                    }
                    else
                    {
                        sMsg = oCandidateApplication.ErrorMessage;
                        LogOut();

                    }
                }  
                else
                {
                    sMsg = oCandidateUser.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                sMsg = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMsg);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void LogOut()
        {
            this.Session.Remove(SessionInfo.wcfSessionID);
            this.Session.Remove(SessionInfo.CurrentUser);
            Global.CurrentSession = Guid.Empty;
            
        }

        #endregion Online Application

        #region Application Print
        public ActionResult PrintCandidateAppplication(int nCandidateID, double ts)
        {
            Candidate oCandidate = new Candidate();

            string sSql = "SELECT * FROM Candidate WHERE CandidateID = " + nCandidateID;
            oCandidate = Candidate.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCandidate.CandidateEducations = CandidateEducation.Gets(nCandidateID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCandidate.CandidateTrainings = CandidateTraining.Gets(nCandidateID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCandidate.CandidateExperiences = CandidateExperience.Gets(nCandidateID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCandidate.CandidateReferences = CandidateReference.Gets(nCandidateID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oCandidate.CandidatePhoto = GetCompanyLogo(oCandidate.Photo);

            rptCandidateAppplication oReport = new rptCandidateAppplication();
            byte[] abytes = oReport.PrepareReport(oCandidate);
            return File(abytes, "application/pdf");
        }
        public Image GetCompanyLogo(byte[] Photo)
        {
            if (Photo != null)
            {
                MemoryStream m = new MemoryStream(Photo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #endregion Application Print

    }
        
    }






