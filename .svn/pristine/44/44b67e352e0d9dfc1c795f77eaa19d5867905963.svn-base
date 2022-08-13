using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using iTextSharp.text;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Mail;


namespace ESimSolFinancial.Controllers
{
    public class EmployeeRequestOnAttendanceController : PdfViewController
    {
        #region Declaration
        EmployeeRequestOnAttendance _oEmployeeRequestOnAttendance;
        List<EmployeeRequestOnAttendance> _oEmployeeRequestOnAttendances;
        
        #endregion

        #region Views
        public ActionResult View_EmployeeRequestOnAttendances(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "SELECT * FROM View_EmployeeRequestOnAttendance WHERE ApproveBy IS NULL OR ApproveBy = 0";
            _oEmployeeRequestOnAttendances = EmployeeRequestOnAttendance.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return View(_oEmployeeRequestOnAttendances);
        }
        #endregion

        public JsonResult SearchRequest(DateTime sDateFrom, DateTime sDateTo, string sEmployeeIDs, bool IsDateSearch)
        {

            List<EmployeeRequestOnAttendance> oEmployeeRequestOnAttendances = new List<EmployeeRequestOnAttendance>();
            string sSql = "SELECT * FROM View_EmployeeRequestOnAttendance WHERE (ApproveBy IS NULL OR ApproveBy = 0)";
            if (IsDateSearch)
            {
                sSql += " AND AttendanceDate BETWEEN '"+sDateFrom.ToString("dd MMM yyyy")+"' AND '"+sDateTo.ToString("dd MMM yyyy")+"'";
            }
            if (!string.IsNullOrEmpty(sEmployeeIDs))
            {
                sSql += " AND EmployeeID IN("+sEmployeeIDs+")";
            }
            oEmployeeRequestOnAttendances = EmployeeRequestOnAttendance.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (oEmployeeRequestOnAttendances.Count <= 0)
            {
                EmployeeRequestOnAttendance oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
                oEmployeeRequestOnAttendance.ErrorMessage = "No Data Found by these searching criteria.";
                oEmployeeRequestOnAttendances.Add(oEmployeeRequestOnAttendance);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeRequestOnAttendances);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ApproveRequest(EmployeeRequestOnAttendance oEmployeeRequestOnAttendance)
        {
            _oEmployeeRequestOnAttendances = new List<EmployeeRequestOnAttendance>();
            _oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
            try
            {
                _oEmployeeRequestOnAttendance = oEmployeeRequestOnAttendance;
                _oEmployeeRequestOnAttendance = _oEmployeeRequestOnAttendance.IUD((int)EnumDBOperation.Approval, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeRequestOnAttendance.ApproveBy > 0)
                {
                    SendMailForLeaveApprove(_oEmployeeRequestOnAttendance, _oEmployeeRequestOnAttendance.EmployeeID);
                }
            }
            catch (Exception ex)
            {
                _oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
                _oEmployeeRequestOnAttendance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeRequestOnAttendance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CancelRequest(EmployeeRequestOnAttendance oEmployeeRequestOnAttendance)
        {
            _oEmployeeRequestOnAttendances = new List<EmployeeRequestOnAttendance>();
            _oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
            try
            {
                _oEmployeeRequestOnAttendance = oEmployeeRequestOnAttendance;
                _oEmployeeRequestOnAttendance = _oEmployeeRequestOnAttendance.IUD((int)EnumDBOperation.Cancel, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (_oEmployeeRequestOnAttendance.CancelBy > 0)
                {
                    SendMailForLeaveCancel(_oEmployeeRequestOnAttendance, _oEmployeeRequestOnAttendance.EmployeeID);
                }
            }
            catch (Exception ex)
            {
                _oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
                _oEmployeeRequestOnAttendance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeRequestOnAttendance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private void SendMailForLeaveCancel(EmployeeRequestOnAttendance oEmployeeRequestOnAttendance, int nEmployeeID)
        {
            Employee oEmployee = new Employee();
            oEmployee = oEmployee.Get(nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (Global.IsValidMail(oEmployee.Email))
            {
                List<string> emialTos = new List<string>();
                emialTos.Add(oEmployee.Email);

                string subject = "OSD Request Canceled for " + oEmployeeRequestOnAttendance.EmployeeName;
                string message = "OSD Request Canceled for " + oEmployeeRequestOnAttendance.AttendanceDateInString + ".";
                string bodyInfo = "";

                List<User> oUsers = new List<User>();
                string sSQL = "Select * from View_User Where EmployeeID=" + oEmployee.EmployeeID;
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oUsers.Any() && oUsers.FirstOrDefault().UserID > 0)
                {

                    var oUser = oUsers.FirstOrDefault();
                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                             "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                             oEmployee.Name, message, oEmployeeRequestOnAttendance.EmployeeName,
                        //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                             Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                }
                else
                {
                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                             "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                             oEmployee.Name, message, oEmployeeRequestOnAttendance.EmployeeName,
                                             Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                }

                #region Email Credential
                EmailConfig oEmailConfig = new EmailConfig();
                oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                #endregion

                Global.MailSend(subject, bodyInfo, emialTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
            }
        }
        private void SendMailForLeaveApprove(EmployeeRequestOnAttendance oEmployeeRequestOnAttendance, int nEmployeeID)
        {
            Employee oEmployee = new Employee();
            oEmployee = oEmployee.Get(nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (Global.IsValidMail(oEmployee.Email))
            {
                List<string> emialTos = new List<string>();
                emialTos.Add(oEmployee.Email);

                string subject = "OSD Request Approved for " + oEmployeeRequestOnAttendance.EmployeeName;
                string message = "OSD Request Approved for " + oEmployeeRequestOnAttendance.AttendanceDateInString + ".";
                string bodyInfo = "";

                List<User> oUsers = new List<User>();
                string sSQL = "Select * from View_User Where EmployeeID=" + oEmployee.EmployeeID;
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oUsers.Any() && oUsers.FirstOrDefault().UserID > 0)
                {

                    var oUser = oUsers.FirstOrDefault();
                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                             "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                             oEmployee.Name, message, oEmployeeRequestOnAttendance.EmployeeName,
                        //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                             Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                }
                else
                {
                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                             "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                             oEmployee.Name, message, oEmployeeRequestOnAttendance.EmployeeName,
                                             Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                }

                #region Email Credential
                EmailConfig oEmailConfig = new EmailConfig();
                oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                #endregion

                Global.MailSend(subject, bodyInfo, emialTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
            }
        }
    }
}

