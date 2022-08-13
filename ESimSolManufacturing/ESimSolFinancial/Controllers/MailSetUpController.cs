using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Net.Mail;


namespace ESimSolFinancial.Controllers
{
    public class MailSetUpController : Controller
    {
        #region Declaration
        private MailSetUp _oMS = new MailSetUp();
        private List<MailSetUp> _oMSs = new List<MailSetUp>();

        #endregion

        #region Action Result View
        public ActionResult ViewMailReportings(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<MailReporting> oMailReportings = new List<MailReporting>();
            string sSQL = "Select * from MailReporting Where ReportID<>0";
            oMailReportings = MailReporting.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oMailReportings);
        }

        public ActionResult ViewMailSetUps(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oMSs = new List<MailSetUp>();

            string sSQL = "Select * from View_MailSetUp Where MSID<>0";
            _oMSs = MailSetUp.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<MailReporting> oMailReportings = new List<MailReporting>();
            sSQL = "Select * from MailReporting Where ReportID<>0 And IsActive=1";
            oMailReportings = MailReporting.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.MailReportings = oMailReportings;
            ViewBag.ModuleTypes = EnumObject.jGets(typeof(EnumModuleName)).OrderBy(x=>x.Value);

            ViewBag.MailTypes = Enum.GetValues(typeof(MailReportingType)).Cast<MailReportingType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oMSs);
        }
        #endregion

        #region Mail Reporting
        [HttpPost]
        public JsonResult SaveMR(MailReporting oMR)
        {
            try
            {
                if (oMR.ReportID <= 0)
                {
                    oMR = oMR.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oMR = oMR.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oMR = new MailReporting();
                oMR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMR);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMR(MailReporting oMR)
        {
            try
            {
                if (oMR.ReportID <= 0) { throw new Exception("Please select an valid item."); }

                oMR = oMR.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMR = new MailReporting();
                oMR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMR.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveMR(MailReporting oMR)
        {
            try
            {
                if (oMR.ReportID <= 0) { throw new Exception("Please select an valid item."); }
                oMR = oMR.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMR = new MailReporting();
                oMR.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMR);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetMR(MailReporting oMR)
        {
            try
            {
                if (oMR.ReportID <= 0) { throw new Exception("Please select an valid item."); }
                oMR = MailReporting.Get(oMR.ReportID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMR = new MailReporting();
                oMR.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMR);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mail SetUp
        [HttpPost]
        public JsonResult SaveMS(MailSetUp oMS)
        {
            try
            {
                if (oMS.MSID <= 0)
                {
                    oMS = oMS.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oMS = oMS.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oMS = new MailSetUp();
                oMS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMS(MailSetUp oMS)
        {
            try
            {
                if (oMS.MSID <= 0) { throw new Exception("Please select an valid item."); }

                oMS = oMS.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMS = new MailSetUp();
                oMS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMS.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveMS(MailSetUp oMS)
        {
            try
            {
                if (oMS.MSID <= 0) { throw new Exception("Please select an valid item."); }
                oMS = oMS.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMS = new MailSetUp();
                oMS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mail Assigend Person

        [HttpPost]
        public JsonResult SaveMAP(MailAssignedPerson oMAP)
        {
            try
            {
                if (!Global.IsValidMail(oMAP.MailTo)) { throw new Exception("Please enter valid mail adderss."); }
                if (oMAP.MAPID <= 0)
                {
                    oMAP = oMAP.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oMAP = oMAP.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oMAP = new MailAssignedPerson();
                oMAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMAP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMAP(MailAssignedPerson oMAP)
        {
            try
            {
                if (oMAP.MAPID <= 0) { throw new Exception("Please select an valid item."); }

                oMAP = oMAP.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oMAP = new MailAssignedPerson();
                oMAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMAP.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Search
        [HttpPost]
        public ActionResult GetMS(MailSetUp oMS)
        {
            try
            {
                if (oMS.MSID <= 0) { throw new Exception("Please select an valid item."); }
                oMS = MailSetUp.Get(oMS.MSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oMS.MSID > 0)
                {
                    string sSQL = "Select * from MailAssignedPerson Where MSID=" + oMS.MSID + "";
                    List<MailAssignedPerson> oMAPs = new List<MailAssignedPerson>();
                    oMAPs = MailAssignedPerson.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oMAPs.Count() > 0)
                    {
                        oMS.ToMail = oMAPs.Where(x => x.IsCCMail == false).ElementAtOrDefault(0);
                        oMS.CCMails = oMAPs.Where(x => x.IsCCMail == true).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                oMS = new MailSetUp();
                oMS.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Mail Send Dinamically

        public static List<MailSetUp> GetMailSetUp(DateTime mailTimeFrom)
        {

            List<MailSetUp> oMailSetUps = new List<MailSetUp>();
            //DateTime mailTimeTo = mailTimeFrom.AddHours(1);
            //string sSQL = "Select * from View_MailSetUp Where IsActive=1 And NextTimeToMail>= '" + mailTimeFrom.ToString("dd MMM yyyy HH:mm") + "' And NextTimeToMail< '" + mailTimeTo.ToString("dd MMM yyyy HH:mm") + "'";
            string sSQL = "Select * from View_MailSetUp Where IsActive=1 And Convert(date,NextTimeToMail) <= CONVERT(date, '" + mailTimeFrom.ToString("dd MMM yyyy") + "') And (LastMailTime IS NULL OR  LastMailTime <= CONVERT(datetime,'" + mailTimeFrom.ToString("dd MMM yyyy hh:mm") + "'))";
            oMailSetUps = MailSetUp.Gets(sSQL, -9);
            return oMailSetUps;
        }

        public static void MailReport(List<MailSetUp> oMailSetUps)
        {
            //if (oMailSetUps.Count() > 0)
            //{
            //    oMailSetUps.ForEach(x =>
            //    {
            //        x.LastMailTime = new DateTime(x.LastMailTime.Year, x.LastMailTime.Month, x.LastMailTime.Day, x.MailTime.Hour, x.MailTime.Minute, 0);
            //    });
            //    string sSQL = "";
            //    foreach (MailSetUp oItem in oMailSetUps)
            //    {
                 
            //        System.Reflection.Assembly asm = System.Reflection.Assembly.Load("ESimSolFinancial");
            //        object Item = asm.CreateInstance("ESimSolFinancial.Controllers." + oItem.ControllerName.Trim() + "Controller", false, System.Reflection.BindingFlags.CreateInstance, null, null, null, null);
            //        if (Item != null)
            //        {
            //            MethodInfo oMInfo = Item.GetType().GetMethod(oItem.FunctionName);
            //            if (oMInfo != null)
            //            {
            //                object[] Params = new object[oMInfo.GetParameters().Count()];
   
            //                object[] objArr = (object[])oMInfo.Invoke(Item, Params);

            //                if (oItem.IsMail)
            //                {
            //                    List<MailAssignedPerson> oMAPs = new List<MailAssignedPerson>();
            //                    sSQL = "Select * FROM  MailAssignedPerson WHERE MSID= " + oItem.MSID + "";
            //                    oMAPs = MailAssignedPerson.Gets(sSQL, -9);


            //                    List<Attachment> oAttachments = new List<Attachment>();
            //                    string sBodyInfo = "";

            //                    if (objArr.Count() > 0)
            //                    {
            //                        for (int i = 0; i < objArr.Count(); i++)
            //                        {
            //                            if (objArr[i].GetType() == typeof(System.String))
            //                            {
            //                                sBodyInfo += (string)objArr[i];
            //                            }
            //                            else if (objArr[i].GetType() == typeof(Attachment))
            //                            {
            //                                oAttachments.Add((Attachment)objArr[i]);
            //                            }
            //                            else if (objArr[i].GetType() == typeof(List<Attachment>))
            //                            {
            //                                oAttachments = (List<Attachment>)objArr[i];
            //                            }
            //                        }
            //                    }

            //                    List<string> sMailTos = oMAPs.Where(x => x.IsCCMail == false).Select(x => x.MailTo).ToList();
            //                    List<string> sMailCCs = oMAPs.Where(x => x.IsCCMail == true).Select(x => x.MailTo).ToList();

            //                    #region Email Credential
            //                    EmailConfig oEmailConfig = new EmailConfig();
            //                    oEmailConfig = oEmailConfig.GetByBU(1, -9);
            //                    #endregion

            //                    Global.MailSend(oItem.Subject, sBodyInfo, sMailTos, sMailCCs, oAttachments, oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
            //                }
         
            //            }

            //        }
            //        oItem.IsMailSend = true;
            //        oItem.IUD((int)EnumDBOperation.Update, -9);
            //    }
            //}
        }


        #endregion

        #region Force Mail Send By User 


        [HttpPost]
        public JsonResult ForceMailSend(MailSetUp oMS)
        {
            try
            {
                //List<string> sMailTos = new List<string>();
                //List<string> sMailCCs = new List<string>();
                //List<Attachment> oAttachments = new List<Attachment>();
                //sMailTos.Add("csefaruk@gmail.com");

                //EmailConfig oEmailConfig = new EmailConfig();
                //oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);


                //Global.MailSend("Test Mail", "Hi Faruk", sMailTos, sMailCCs, oAttachments, oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
                ////Global.MailSend(oItem.Subject, sBodyInfo, sMailTos, sMailCCs, oAttachments, oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
                //oMS = new MailSetUp();
                //oMS.ErrorMessage = "Mail Send Successfully.";

                List<MailSetUp> oMailSetUps = new List<MailSetUp>();
                string sSQL = "Select * from View_MailSetUp Where IsActive=1 And Convert(date,NextTimeToMail)= '" + DateTime.Now.ToString("dd MMM yy") + "'";
                if (oMS.Params != null && oMS.Params != "")
                {
                    sSQL = sSQL + " And MSID In (" + oMS.Params + ")";
                }
                oMailSetUps = MailSetUp.Gets(sSQL, -9);
                if (oMailSetUps.Count() > 0)
                {
                    MailReport(oMailSetUps);
                    oMS = new MailSetUp();
                    oMS.ErrorMessage = "Mail Send Successfully.";
                }
                else
                {
                    throw new Exception("For today, there is no mail found to send.");
                }

            }
            catch (Exception ex)
            {
                oMS = new MailSetUp();
                oMS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

}