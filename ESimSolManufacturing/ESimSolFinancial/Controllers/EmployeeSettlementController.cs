using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Net.Mail;
using CrystalDecisions.CrystalReports.Engine;

namespace ESimSolFinancial.Controllers
{
    public class EmployeeSettlementController : Controller
    {
        #region Declaration
        EmployeeSettlement _oEmployeeSettlement;
        List<EmployeeSettlement> _oEmployeeSettlements;
        static List<EmployeeSettlement> oESForErrors = new List<EmployeeSettlement>();
        static List<EmployeeSalary> oEmpSalaryForErrors = new List<EmployeeSalary>();
        List<EmployeeSettlementSalaryDetail> _oTEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
        double nExceptSalaryHead = 0.0;
        #endregion

        #region Views

        public ActionResult View_EmployeeSettlements(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSettlement).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oEmployeeSettlements = new List<EmployeeSettlement>();
            string sSql = "SELECT *  FROM View_EmployeeSettlement WHERE IsResigned=0";
            _oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EnumSettleMentTypes = Enum.GetValues(typeof(EnumSettleMentType)).Cast<EnumSettleMentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.ClearanceStatus = Enum.GetValues(typeof(EnumESCrearance)).Cast<EnumESCrearance>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            //List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            //oAUOEDOs = AuthorizationUserOEDO.GetsByUser(((User)(Session[SessionInfo.CurrentUser])).UserID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //bool bView = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._View, "EmployeeSettlement", oAUOEDOs);

            //bool bView = true;
            //TempData["View"] = bView;

            return View(_oEmployeeSettlements);
        }

        public ActionResult View_EmployeeSettlement(string sid, string sMsg)
        {
            _oEmployeeSettlement = new EmployeeSettlement();
            int nERID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            if (nERID > 0)
            {
                _oEmployeeSettlement = EmployeeSettlement.Get(nERID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EnumSettleMentTypes = Enum.GetValues(typeof(EnumSettleMentType)).Cast<EnumSettleMentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            return View(_oEmployeeSettlement);
        }

        #endregion

        #region EmployeeSettlement_IUD
        [HttpPost]
        public JsonResult EmployeeSettlement_IU(EmployeeSettlement oEmployeeSettlement)
        {
            _oEmployeeSettlement = new EmployeeSettlement();
            try
            {
                _oEmployeeSettlement = oEmployeeSettlement;
                if (_oEmployeeSettlement.EmployeeSettlementID > 0)
                {
                    _oEmployeeSettlement = _oEmployeeSettlement.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oEmployeeSettlement = _oEmployeeSettlement.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oEmployeeSettlement = new EmployeeSettlement();
                _oEmployeeSettlement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSettlement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ER_Delete(EmployeeSettlement oEmployeeSettlement)
        {
            string sErrorMease = "";
            try
            {
                oEmployeeSettlement = oEmployeeSettlement.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sErrorMease = oEmployeeSettlement.ErrorMessage;
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion EmployeeSettlement_IUD

        #region EmployeeSettlement Approve

        [HttpPost]
        public JsonResult EmployeeSettlement_Approve(EmployeeSettlement oEmployeeSettlement)
        {
            List<EmployeeSettlement> oESs = new List<EmployeeSettlement>();
            List<EmployeeSettlement> oESXLs = new List<EmployeeSettlement>();
            List<EmployeeReportingPerson> oEmployeeReportingPersons = new List<EmployeeReportingPerson>();
            bool bFeedBack = true;
            string sEmployeeIDs = "";
            try
            {
                oESXLs = oEmployeeSettlement.Approve_Multiple((int)EnumDBOperation.Approval, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                
                oESForErrors = oESXLs;
                
                if (oESXLs.Count > 0)
                {
                    bFeedBack = false;
                }
                int[] sEmpSSIDs = new int[10000];
                int[] errorIDs = new int[10000];
                string sFindIDs = string.Join(",", oESXLs.Select(x => x.EmployeeSettlementID));
                sEmpSSIDs = oEmployeeSettlement.ErrorMessage.Split(',').Select(x => Convert.ToInt32(x)).ToArray();

                if (!String.IsNullOrEmpty(sFindIDs))
                {
                    errorIDs = sFindIDs.Split(',').Select(m => Convert.ToInt32(m)).ToArray();
                }
                int[] set3 = sEmpSSIDs.Except(errorIDs).Select(x => Convert.ToInt32(x)).ToArray();
                string sSuccessIDs = string.Join(",", set3.Select(x => x));


                if (!String.IsNullOrEmpty(sSuccessIDs))
                {
                    oESs = EmployeeSettlement.Gets("SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID IN("+sSuccessIDs+")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oESs = new List<EmployeeSettlement>();
                }


                #region mail
                if (oESXLs.Count <= 0)
                {
                    foreach (string sid in oEmployeeSettlement.EmpIDs.Split(','))
                    {
                        sEmployeeIDs = sid;
                        oEmployeeReportingPersons = EmployeeReportingPerson.GetHierarchy(sEmployeeIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                        foreach (EmployeeReportingPerson oitem in oEmployeeReportingPersons)
                        {
                            Employee oEmployeeSett = new Employee();
                            oEmployeeSett = oEmployeeSett.Get(Convert.ToInt32(sid), ((User)(Session[SessionInfo.CurrentUser])).UserID);

                            Employee oEmployee = new Employee();
                            oEmployee = oEmployee.Get(oitem.RPID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                            if (Global.IsValidMail(oEmployee.Email))
                            {
                                List<string> emialTos = new List<string>();
                                emialTos.Add(oEmployee.Email);

                                string subject = "Final settlement for " + oEmployeeSett.Name;
                                string message = "Final settlement for " + oEmployeeSett.Name + " is approved.";
                                string bodyInfo = "";

                                List<User> oUsers = new List<User>();
                                string sSQL = "Select * from View_User Where EmployeeID=" + oEmployee.EmployeeID;
                                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                                if (oUsers.Any() && oUsers.FirstOrDefault().UserID > 0)
                                {

                                    var oUser = oUsers.FirstOrDefault();
                                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                                             "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                                             oEmployee.Name, message, oEmployeeSett.Name,
                                        //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                                             Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                }
                                else
                                {
                                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                                             "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                                             oEmployee.Name, message, oEmployeeSett.Name,
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

                else
                {
                    foreach (string sid in oEmployeeSettlement.EmpIDs.Split(','))
                    {
                        foreach (EmployeeSettlement oes in oESXLs)
                        {

                            if (Convert.ToInt32(sid) != oes.EmployeeID)
                            {
                                sEmployeeIDs = sid;
                                oEmployeeReportingPersons = EmployeeReportingPerson.GetHierarchy(sEmployeeIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                                foreach (EmployeeReportingPerson oitem in oEmployeeReportingPersons)
                                {

                                    Employee oEmployeeSett = new Employee();
                                    oEmployeeSett = oEmployeeSett.Get(Convert.ToInt32(sid), ((User)(Session[SessionInfo.CurrentUser])).UserID);


                                    Employee oEmployee = new Employee();
                                    oEmployee = oEmployee.Get(oitem.RPID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                                    if (Global.IsValidMail(oEmployee.Email))
                                    {
                                        List<string> emialTos = new List<string>();
                                        emialTos.Add(oEmployee.Email);

                                        string subject = "Final settlement for " + oEmployeeSett.Name;
                                        string message = "Final settlement for " + oEmployeeSett.Name + " is approved.";
                                        string bodyInfo = "";

                                        List<User> oUsers = new List<User>();
                                        string sSQL = "Select * from View_User Where EmployeeID=" + oEmployee.EmployeeID;
                                        oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                                        if (oUsers.Any() && oUsers.FirstOrDefault().UserID > 0)
                                        {

                                            var oUser = oUsers.FirstOrDefault();
                                            bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                                                        "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                                                        oEmployee.Name, message, oEmployeeSett.Name,
                                                //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                                                        Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                        }
                                        else
                                        {
                                            bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                                                        "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                                                        oEmployee.Name, message, oEmployeeSett.Name,
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
                            //int nid = Convert.ToInt32(sid);

                        }
                    }

                }
                #endregion
            }
            catch (Exception ex){}
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(new { FeedBackResponse = bFeedBack, EmployeeSettlement = oESs });//, ESSErrorXL = oESXLs 
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void PrintErrorList(double ts)
        {
            if (oESForErrors.Count > 0)
            {
                ExcelRange cell;
                OfficeOpenXml.Style.Border border;
                ExcelFill fill;
                int colIndex = 1;
                int rowIndex = 2;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Error List");
                    sheet.Name = "Error List";

                    int n = 1;
                    sheet.Column(n++).Width = 13;
                    sheet.Column(n++).Width = 50;

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmployeeCode"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex += 1;

                    foreach (EmployeeSettlement oItem in oESForErrors)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ErrorList.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
            oESForErrors = new List<EmployeeSettlement>();
        }

        //[HttpPost]
        //public JsonResult EmployeeSettlement_Approve(EmployeeSettlement oEmployeeSettlement)
        //{
        //    _oEmployeeSettlement = new EmployeeSettlement();
        //    try
        //    {
        //        _oEmployeeSettlement = oEmployeeSettlement.IUD((int)EnumDBOperation.Approval,((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oEmployeeSettlement = new EmployeeSettlement();
        //        _oEmployeeSettlement.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oEmployeeSettlement);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult EmployeeSettlement_UndoApprove(EmployeeSettlement oEmployeeSettlement)
        {
            _oEmployeeSettlement = new EmployeeSettlement();
            try
            {
                _oEmployeeSettlement = oEmployeeSettlement.IUD((int)EnumDBOperation.UnApproval, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oEmployeeSettlement = new EmployeeSettlement();
                _oEmployeeSettlement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSettlement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Settlement Salary Process
        [HttpPost]
        public JsonResult SettlementSalaryProcess(EmployeeSettlement oEmployeeSettlement)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            try
            {
                oEmployeeSalary = PayrollProcessManagement.SettlementSalaryProcess(oEmployeeSettlement.EmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeSalary = new EmployeeSalary();
                oEmployeeSalary.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSalary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SettlementSalaryProcess_Corporate(EmployeeSettlement oEmployeeSettlement)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            try
            {
                oEmployeeSalary = PayrollProcessManagement.SettlementSalaryProcess_Corporate(oEmployeeSettlement.EmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeSalary = new EmployeeSalary();
                oEmployeeSalary.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSalary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SettlementSalaryProcess_Corporate_Multiple(EmployeeSettlement oEmployeeSettlement)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            List<EmployeeSalary> oESs = new List<EmployeeSalary>();
            List<EmployeeSalary> oESXLs = new List<EmployeeSalary>();
            bool bFeedBack = true;

            try
            {
                oESXLs = PayrollProcessManagement.SettlementSalaryProcess_Corporate_Multiple(oEmployeeSettlement, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                oEmpSalaryForErrors = oESXLs;

                if (oESXLs.Count > 0)
                {
                    bFeedBack = false;
                }

            }
            catch (Exception ex) { }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(new { FeedBackResponse = bFeedBack});//, ESSErrorXL = oESXLs 
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void PrintSalaryErrorList(double ts)
        {
            if (oEmpSalaryForErrors.Count > 0)
            {
                ExcelRange cell;
                OfficeOpenXml.Style.Border border;
                ExcelFill fill;
                int colIndex = 1;
                int rowIndex = 2;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Error List");
                    sheet.Name = "Error List";

                    int n = 1;
                    sheet.Column(n++).Width = 13;
                    sheet.Column(n++).Width = 50;

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmployeeCode"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Error"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex += 1;

                    foreach (EmployeeSalary oItem in oEmpSalaryForErrors)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ErrorMessage; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex++;
                    }

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ErrorList.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
            }
            oEmpSalaryForErrors = new List<EmployeeSalary>();
        }
        #endregion

        #region Payment Done
        [HttpPost]
        public JsonResult Payment_Done(EmployeeSettlement oEmployeeSettlement)
        {
            _oEmployeeSettlement = new EmployeeSettlement();
            try
            {
                _oEmployeeSettlement = oEmployeeSettlement;
                _oEmployeeSettlement = EmployeeSettlement.PaymentDone(_oEmployeeSettlement.EmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oEmployeeSettlement = new EmployeeSettlement();
                _oEmployeeSettlement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSettlement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region EmployeeSettlement Approve
        //[HttpPost]
        //public JsonResult EmployeeSettlement_PaymentDone(EmployeeSettlement oEmployeeSettlement)
        //{
        //    _oEmployeeSettlement = new EmployeeSettlement();
        //    try
        //    {
        //        _oEmployeeSettlement = oEmployeeSettlement;
        //        _oEmployeeSettlement = EmployeeSettlement.Approve(_oEmployeeSettlement.EmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

        //    }
        //    catch (Exception ex)
        //    {
        //        _oEmployeeSettlement = new EmployeeSettlement();
        //        _oEmployeeSettlement.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oEmployeeSettlement);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region Resignation Search
        [HttpPost]
        public JsonResult SearchResignation(string sEmployeeIDs)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeID IN(" + sEmployeeIDs + ")";
                _oEmployeeSettlements = new List<EmployeeSettlement>();
                _oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeSettlements.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployeeSettlements = new List<EmployeeSettlement>();
                _oEmployeeSettlement = new EmployeeSettlement();
                _oEmployeeSettlement.ErrorMessage = ex.Message;
                _oEmployeeSettlements.Add(_oEmployeeSettlement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSettlements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchWithCriteria(string dtDateFrom, string dtDateTo, int nSettlementType, string sBusinessUnitIds, string sLocationID,  string sDepartmentIds, string sDesignationIDs,int nClearanceStatus,int nApproveStatus, double ts)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM View_EmployeeSettlement WHERE  EffectDate BETWEEN '" + dtDateFrom + "' AND '" + dtDateTo+"' ";

                if (!string.IsNullOrEmpty(sBusinessUnitIds))
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE BusinessUnitID IN(" + sBusinessUnitIds + "))";
                }
                if (!string.IsNullOrEmpty(sLocationID))
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE LocationID IN(" + sLocationID + "))";
                }
                if ( !string.IsNullOrEmpty(sDepartmentIds))
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))";
                }
                if (!string.IsNullOrEmpty(sDesignationIDs))
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DesignationID IN(" + sDesignationIDs + "))";
                }

                if (nSettlementType > 0)
                {
                    sSql = sSql + " AND SettlementType=" + nSettlementType;
                }

                if (nApproveStatus ==1)
                {
                    sSql = sSql + " AND ApproveBy>0";
                }
                if (nApproveStatus == 2)
                {
                    sSql = sSql + " AND ApproveBy<=0";
                }
                if (nClearanceStatus >0)
                {
                    sSql = sSql + " AND EmployeeSettlementID IN(SELECT EmployeeSettlementID  FROM EmployeeSettlementClearance WHERE CurrentStatus=" + nClearanceStatus + ")";
                }
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE DRPID IN( "
                                + " SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
                }
                sSql = sSql +" ORDER BY EmployeeCode";
                _oEmployeeSettlements = new List<EmployeeSettlement>();
                _oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeSettlements.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployeeSettlements = new List<EmployeeSettlement>();
                _oEmployeeSettlement = new EmployeeSettlement();
                _oEmployeeSettlement.ErrorMessage = ex.Message;
                _oEmployeeSettlements.Add(_oEmployeeSettlement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSettlements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Report
        public ActionResult PrintFinalSettlementOfResig_MAMIYA(int nEmpID, double ts)
        {
            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_FinalSettlementOfResig(nEmpID,((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

            rptFinalSettlementOfResig_MAMIYA oReport = new rptFinalSettlementOfResig_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintFinalSettlementOfResig_Accounts_MAMIYA(int nEmployeeSettlementID, int nEmployeeID, double ts)
        {
            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_FinalSettlementOfResig(nEmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (oEmployeeSalary.EmployeeSalary_MAMIYAs.Count > 0 && oEmployeeSalary.EmployeeSalary_MAMIYAs[0].ErrorMessage=="")
            {
                oEmployeeSalary.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets("SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSalaryStructures = EmployeeSalaryStructure.Gets("SELECT *  FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSettlements = EmployeeSettlement.Gets("SELECT *  FROM VIEW_EmployeeSettlement WHERE EmployeeSettlementID=" + nEmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

            rptFinalSettlementOfResig_Accounts_MAMIYA oReport = new rptFinalSettlementOfResig_Accounts_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintFinalSettlementForm_MAMIYA(int nEmpID, double ts)
        {
            FinalSettlementForm oFinalSettlementForm = new FinalSettlementForm();
            oFinalSettlementForm.FinalSettlementForms = FinalSettlementForm.Gets(nEmpID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oFinalSettlementForm.Company = oCompanys.First();

            rptFinalSettlement_MAMIYA oReport = new rptFinalSettlement_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oFinalSettlementForm);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintFinalSettlement_HR_MAMIYA(int nEmpID, double ts)
        {
            FinalSettlementForm oFinalSettlementForm = new FinalSettlementForm();
            oFinalSettlementForm.FinalSettlementForms = FinalSettlementForm.Gets(nEmpID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //List<Company> oCompanys = new List<Company>();
            //oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            //oFinalSettlementForm.Company = oCompanys.First();
            Company oCompany = new Company();
            oFinalSettlementForm.Company = new Company();
            oCompany = oCompany.GetCompanyLogo(1,((User)(Session[SessionInfo.CurrentUser])).UserID);
            oFinalSettlementForm.Company = oCompany;
            oFinalSettlementForm.Company.CompanyLogo = GetImage(oCompany.OrganizationLogo);

            rptFinalSettlement_HR_MAMIYA oReport = new rptFinalSettlement_HR_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oFinalSettlementForm);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintReleaseLetter(int nEmployeeSettlementID, double ts)
        {
            EmployeeSettlement oEmployeeSettlement = new EmployeeSettlement();
            oEmployeeSettlement.EmployeeSettlements = EmployeeSettlement.Gets("SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID="+nEmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSettlement.Company = oCompanys.First();
            oEmployeeSettlement.Company.CompanyLogo = GetImage(oEmployeeSettlement.Company.OrganizationLogo);

            rptReleaseLetter oReport = new rptReleaseLetter();
            byte[] abytes = oReport.PrepareReport(oEmployeeSettlement);
            return File(abytes, "application/pdf");
        }

        public ActionResult BanglaSettlementForm(int SettlementID, double ts)
        {
            FinalSettlement oFinalSettlement = new FinalSettlement();
            oFinalSettlement = FinalSettlement.Get(SettlementID, (int)Session[SessionInfo.currentUserID]);
            List<FinalSettlement> oFinalSettlements = new List<FinalSettlement>();
            oFinalSettlements.Add(oFinalSettlement);
            Company oCompany = new Company();
            List<Company> oCompanys = new List<Company>();
            oCompany = oCompany.GetCompanyLogo(1, (int)(Session[SessionInfo.currentUserID]));
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //oCompany.AuthSig = GetImage(oCompany.AuthorizedSignature, "AuthSig.jpg");
            oCompanys.Add(oCompany);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "EmployeeSettlementBangla.rpt"));
            rd.Database.Tables["EmployeeSettlement"].SetDataSource(oFinalSettlements);
            rd.Database.Tables["Company"].SetDataSource(oCompanys);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {                
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);               
                return File(stream, "application/pdf");
            }
            catch { throw; }
        }
        public Image GetImage(byte[] Image, string sImageName = "Image.jpg")
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Images/" + sImageName);
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(Image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;

            }
            else
            {
                return null;
            }
        }
        #endregion Report

        [HttpPost]
        public JsonResult SearchByName(EmployeeSettlement ES)
        {
            _oEmployeeSettlements = new List<EmployeeSettlement>();
            string Ssql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeName LIKE '%" + ES.EmployeeName + "%'" + " OR EmployeeCode LIKE '%" + ES.EmployeeName + "%'";
            try
            {
                _oEmployeeSettlements = EmployeeSettlement.Gets(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeSettlements.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployeeSettlement = new EmployeeSettlement();
                _oEmployeeSettlements.Add(_oEmployeeSettlement);
                _oEmployeeSettlements[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSettlements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetImage(byte[] Image)
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Image.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(Image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetsByEmpCode(Employee oEmployee)
        {
            string sCode = oEmployee.Params.Split('~')[0];
            int nDepartmentID = Convert.ToInt32(oEmployee.Params.Split('~')[1]);
            List<Employee>  oEmployees = new List<Employee>();
            try
            {
                string sSql = "SELECT * FROM VIEW_Employee WHERE Code LIKE '%" + sCode + "%' OR Name LIKE '%" + sCode + "%'";
                if (nDepartmentID != 0)
                {
                    sSql += " AND DepartmentID =" + nDepartmentID;
                }
                oEmployees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEmployees.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oEmployees = new List<Employee>();
                oEmployee = new Employee();
                oEmployee.ErrorMessage = ex.Message;
                oEmployees.Add(oEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region XL
        public void PrintFinalSettlementOfResig_Accounts_MAMIYA_XL(int nEmployeeSettlementID, int nEmployeeID, double ts)
        {
            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
           
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_FinalSettlementOfResig(nEmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets("SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets("SELECT *  FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSettlements = EmployeeSettlement.Gets("SELECT *  FROM VIEW_EmployeeSettlement WHERE EmployeeSettlementID=" + nEmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (oEmployeeSalary.EmployeeSalary_MAMIYAs.Count > 0)
            {
                oEmployeeSalary = oEmployeeSalary.EmployeeSalary_MAMIYAs[0];
            }

            int nMaxColumn = 0;
            int colIndex = 1;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nFontSize = 0;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("FINAL SETTLEMENT OF " + oEmployeeSalary.SettlementTypeInString);
                sheet.Name = "FINAL SETTLEMENT OF " + oEmployeeSalary.SettlementTypeInString;

                nMaxColumn = 5;

                sheet.Column(1).Width = 25;
                sheet.Column(2).Width = 15;

                sheet.Column(3).Width = 1;

                sheet.Column(4).Width = 24;
                sheet.Column(5).Width = 20;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = this.GetImage(oCompany.OrganizationLogo);

                #region Report Header
                nFontSize = 20;
                sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                nFontSize = 15;
                sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "FINAL SETTLEMENT OF " + oEmployeeSalary.SettlementTypeInString; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                nFontSize = 10;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last Working Day(শেষ কর্মদিবস)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DateOfResigInString; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary Cycle(বেতন মাস)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.SalaryStartDateInString + "-" + oEmployeeSalary.SalaryEndDateInString; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code(কোড নং)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.EmployeeCode; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name(নাম)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.EmployeeName; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department(বিভাগ)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DepartmentName; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation(পদবী)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DesignationName; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining(যোগদানের তারিখ)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DateOfJoinInString; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Confirm.(স্থায়ীকরনের তারিখ)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DateOfConfirmationInString; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date of Birth(জন্ম তারিখ)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DateOfBirthInString; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Year(চাকুরীর বয়স)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                DateTime TempDateOfJoin = oEmployeeSalary.DateOfJoin;
                TempDateOfJoin = TempDateOfJoin.AddDays(1);

                DateDifference dateDifference = new DateDifference(oEmployeeSalary.DateOfResigEffect, TempDateOfJoin);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dateDifference.ToString(); cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic Days(বেসিক দিন)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                DateTime TempDateOfResigEffect = oEmployeeSalary.DateOfResigEffect;
                TempDateOfResigEffect = TempDateOfResigEffect.AddDays(-(oEmployeeSalary.FriDay-1));

                if (oEmployeeSalary.DateOfJoin > oEmployeeSalary.SalaryStartDate) { dateDifference = new DateDifference(oEmployeeSalary.DateOfJoin, TempDateOfResigEffect); }
                else { dateDifference = new DateDifference(oEmployeeSalary.SalaryStartDate, TempDateOfResigEffect); }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (dateDifference.Days) > 0 ? (dateDifference.Days).ToString() : "-"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Font.Size = nFontSize;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LWP Days/Hrs(অনুপস্থিত ছুটি)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.TotalUpLeave > 0 ? oEmployeeSalary.TotalUpLeave.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Allow. Days(অন্যান্য ভাতার দিন)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                TempDateOfResigEffect = oEmployeeSalary.DateOfResigEffect;
                TempDateOfResigEffect = TempDateOfResigEffect.AddDays(1);// for all days friday will not be deducted

                if (oEmployeeSalary.DateOfJoin > oEmployeeSalary.SalaryStartDate) { dateDifference = new DateDifference(oEmployeeSalary.DateOfJoin, TempDateOfResigEffect); }
                else { dateDifference = new DateDifference(oEmployeeSalary.SalaryStartDate, TempDateOfResigEffect); }
                int nAllDays = 0;
                if (dateDifference.Days <= 0 && dateDifference.Months==1)// when months is 1 then days returns zero . 
                {
                    if (oEmployeeSalary.DateOfJoin > oEmployeeSalary.SalaryStartDate) { dateDifference = new DateDifference(oEmployeeSalary.DateOfJoin, TempDateOfResigEffect.AddDays(-1)); }
                    else { dateDifference = new DateDifference(oEmployeeSalary.SalaryStartDate, TempDateOfResigEffect.AddDays(-1)); }
                    nAllDays = dateDifference.Days + 1;
                }
                else
                {
                    nAllDays = dateDifference.Days;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAllDays > 0 ? nAllDays.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Absent Days/Hrs(অনুপস্থিত দিন)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.TotalAbsent > 0 ? oEmployeeSalary.TotalAbsent.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift Allow. Days(শিফটিং দিন)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.ShiftAllDay > 0 ? oEmployeeSalary.ShiftAllDay.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No Work Days(নো ওয়ার্ক দিন)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.TotalNoWork > 0 ? oEmployeeSalary.TotalNoWork.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL Days/Hours(অসুস্থতা ছুটি)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.AbsentHr_Sick > 0 ? oEmployeeSalary.AbsentHr_Sick.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EL Balance(প্রাপ্য ই. এল. দিন)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.ELBalance > 0 ? oEmployeeSalary.ELBalance.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT (Normal)(নরমাল ওটি)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.OT_NHR_HR > 0 ? oEmployeeSalary.OT_NHR_HR.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT (Holiday)(হলিডে ওটি)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.OT_HHR_HR > 0 ? oEmployeeSalary.OT_HHR_HR.ToString() : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex++;
                colIndex = 1;

                Double nAmount = 0;
                nAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == 1).ToList()[0].Amount;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic(প্রদেয় মূল)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount > 0 ? GetAmountInStr(nAmount, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nAmount = 0;
                nAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == 2).ToList()[0].Amount;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "House Rent(বাড়ি ভাড়া)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount > 0 ? GetAmountInStr(nAmount, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                nAmount = 0;
                nAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == 3).ToList()[0].Amount;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Medical(চিকিৎসা ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount > 0 ? GetAmountInStr(nAmount, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary(মূল বেতন)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalaryStructures.Count > 0 ? GetAmountInStr(oEmployeeSalaryStructures[0].GrossAmount, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex++;
                colIndex = 1;

                sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "SETTLEMENT ACCOUNTS (নিষ্পত্তি হিসাব) "; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex++;
                colIndex = 1;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex, ++colIndex]; cell.Merge = true;
                cell.Value = "EARNING (উপার্জন)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //sheet.Cells[rowIndex,++colIndex , rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = "DEDUCTION (কর্তন)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic Pay(প্রদেয় বেতন)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.Basic > 0 ? GetAmountInStr(oEmployeeSalary.Basic, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sick(অসুস্থতা ছুটি)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.SLDeduction > 0 ? GetAmountInStr(oEmployeeSalary.SLDeduction, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "House Allowance(বাড়ি ভাড়া)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.HRent > 0 ? GetAmountInStr(oEmployeeSalary.HRent, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Absent/LWP(অনুপস্থিত)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.TotalAbsentAmount > 0 ? GetAmountInStr(oEmployeeSalary.TotalAbsentAmount, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Medical Allow.(চিকিৎসা ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.Med > 0 ? GetAmountInStr(oEmployeeSalary.Med, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Transport(যাতায়ত)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.TRNS > 0 ? GetAmountInStr(oEmployeeSalary.TRNS, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Conv. Allowance(যাতায়ত ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.Conveyance > 0 ? GetAmountInStr(oEmployeeSalary.Conveyance, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Others(অন্যান্য)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Other Allowance(অন্যান্য ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.OtherAll > 0 ? GetAmountInStr(oEmployeeSalary.OtherAll, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Advance(অগ্রিম)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.ADV > 0 ? GetAmountInStr(oEmployeeSalary.ADV, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift Allowance(শিফটিং ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.ShiftAmount > 0 ? GetAmountInStr(oEmployeeSalary.ShiftAmount, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Income Tax(আয়কর)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.IncomeTax > 0 ? GetAmountInStr(oEmployeeSalary.IncomeTax, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No Work Allow.(নো ওয়ার্ক ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.TotalNoWorkDayAllowance > 0 ? GetAmountInStr(oEmployeeSalary.TotalNoWorkDayAllowance, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Adj. Dr.(ডেবিট সমন্বয়)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.ADJDR > 0 ? GetAmountInStr(oEmployeeSalary.ADJDR, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Att. Bonus(উপস্থিতি বোনাস)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.AttendanceBonus > 0 ? GetAmountInStr(oEmployeeSalary.AttendanceBonus, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Notice Pay(নোটিশ পে)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //nAmount = 0;
                //nAmount = oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == 1).ToList()[0].Amount;//basic

                //double DeductionNotice = 0;
                //DeductionNotice = oEmployeeSettlements.Count > 0 ? ((oEmployeeSettlements[0].IsNoticePayDeduction) ? nAmount : 0) : 0;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.NoticePayDeduction > 0 ? GetAmountInStr(oEmployeeSalary.NoticePayDeduction, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Festival Bonus(উৎসব বোনাস)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.FB > 0 ? GetAmountInStr(oEmployeeSalary.FB, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Revenue Stamp(ষ্ট্যাম্প)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.RS > 0 ? GetAmountInStr(oEmployeeSalary.RS, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Adj. Cr.(ক্রেডিট সমন্বয়)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.ADJCR > 0 ? GetAmountInStr(oEmployeeSalary.ADJCR, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT (Normal)(নরমাল ওটি ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.OT_NHR_AMT > 0 ? Global.MillionFormat(oEmployeeSalary.OT_NHR_AMT) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT (Holiday)(হলিডে ওটি ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.OT_HHR_AMT > 0 ? (Global.MillionFormat(oEmployeeSalary.OT_HHR_AMT)) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Holiday Allow.(হলিডে ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.HolidayAll > 0 ? GetAmountInStr(oEmployeeSalary.HolidayAll, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Notice Pay(নোটিশ পে)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //double EarningNotice = 0;
                //EarningNotice = oEmployeeSettlements.Count > 0 ? ((oEmployeeSalary.SettlementType == EnumSettleMentType.Termination) ? nAmount * 4 : 0) : 0;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.NoticePayAddition > 0 ? GetAmountInStr(oEmployeeSalary.NoticePayAddition, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gratuity(গ্র্যাচুয়িটি)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.Gratuity > 0 ? GetAmountInStr(oEmployeeSalary.Gratuity, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EL Amount(ই. এল. ভাতা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.ELAmount > 0 ? GetAmountInStr(oEmployeeSalary.ELAmount, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Fraction Rate(ভগ্নাংশ টাকা)"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.Fracretained > 0 ? GetAmountInStr(oEmployeeSalary.Fracretained, true, false) : "-"; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                //double nEarningsTotal = 0;
                //double nDeductionTotal = 0;

                //nEarningsTotal = oEmployeeSalary.EarningsTotal + EarningNotice;
                //nDeductionTotal = oEmployeeSalary.DeductionTotal - DeductionNotice;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Earning (মোট উপার্জন) "; cell.Style.Font.Bold = true; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.EarningsTotal > 0 ? GetAmountInStr(oEmployeeSalary.EarningsTotal, true, false) : "-"; cell.Style.Font.Bold = true; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Deduction (মোট কর্তন)"; cell.Style.Font.Bold = true; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DeductionTotal > 0 ? GetAmountInStr(oEmployeeSalary.DeductionTotal, true, false) : "-"; cell.Style.Font.Bold = true; cell.Style.Font.Size = nFontSize;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, ++colIndex]; cell.Merge = true;
                cell.Value = "Net Earning (চূড়ান্ত উপার্জন) - " + (((oEmployeeSalary.EarningsTotal - oEmployeeSalary.DeductionTotal) < 0) ? ("(" + GetAmountInStr((oEmployeeSalary.EarningsTotal - oEmployeeSalary.DeductionTotal) * (-1), true, false) + ")") : (GetAmountInStr((oEmployeeSalary.EarningsTotal - oEmployeeSalary.DeductionTotal), true, false))); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = "In word:" + Global.TakaWords(Math.Round(oEmployeeSalary.EarningsTotal - oEmployeeSalary.DeductionTotal)) + "."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                colIndex = 1;

                sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex++;
                colIndex = 1;

                sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Acknowledged By"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 5, colIndex]; cell.Merge = true;
                cell.Value = " Receiver's Signature"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 5, ++colIndex]; cell.Merge = true;
                cell.Value = "Prepared By"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 5, colIndex]; cell.Merge = true;
                //cell.Value = ""; cell.Style.Font.Bold = false;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 5, colIndex]; cell.Merge = true;
                cell.Value = "Admin, HR & Compliance"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 5, nMaxColumn]; cell.Merge = true;
                cell.Value = "Accounts"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;





                //rowIndex = rowIndex + 5;
                //colIndex = 1;

                //sheet.Cells[rowIndex, 2, rowIndex, 4].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "_________________\r\n Receiver's Signature "; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment= ExcelVerticalAlignment.Bottom;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //sheet.Cells[rowIndex, 6, rowIndex, 7].Merge = true;
                //cell = sheet.Cells[rowIndex, 6]; cell.Value = "_________________\r\n Prepared By"; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //sheet.Cells[rowIndex, 8, rowIndex, 8].Merge = true;
                //cell = sheet.Cells[rowIndex, 8]; cell.Value = "_________________\r\n Approved By"; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FINALSETTLEMENT.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void PrintFinalSettlement_XL(int nEmployeeSettlementID, double ts)
        {
            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            EmployeeSettlement oEmployeeSettlement = new EmployeeSettlement();
            FinalSettlementForm oFinalSettlementForm = new FinalSettlementForm();
            List<FinalSettlementForm> oFinalSettlementForms = new List<FinalSettlementForm>();

            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_FinalSettlementOfResig(nEmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSettlement = EmployeeSettlement.Get("SELECT *  FROM VIEW_EmployeeSettlement WHERE EmployeeSettlementID=" + nEmployeeSettlementID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (oEmployeeSalary.EmployeeSalary_MAMIYAs.Count > 0)
            {
                oEmployeeSalary = oEmployeeSalary.EmployeeSalary_MAMIYAs[0];
                oEmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets("SELECT * FROM VIEW_EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + oEmployeeSalary.EmployeeID + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets("SELECT *  FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID=" + oEmployeeSalary.EmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                oFinalSettlementForms = FinalSettlementForm.Gets(oEmployeeSalary.EmployeeID,((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oFinalSettlementForms.Count > 0) { oFinalSettlementForm = oFinalSettlementForms[0]; }
            }

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("FINAL SETTLEMENT OF " + oEmployeeSalary.SettlementTypeInString);
                sheet.Name = "FINAL SETTLEMENT OF " + oEmployeeSalary.SettlementTypeInString;

                nMaxColumn = 20;

                sheet.Column(2).Width = 15;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 15;
                sheet.Column(5).Width = 15;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 15;
                sheet.Column(10).Width = 15;
                sheet.Column(11).Width = 15;
                sheet.Column(12).Width = 15;
                sheet.Column(13).Width = 15;
                sheet.Column(14).Width = 15;
                sheet.Column(15).Width = 15;
                sheet.Column(16).Width = 15;
                sheet.Column(17).Width = 15;
                sheet.Column(18).Width = 15;
                sheet.Column(19).Width = 15;
              

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = this.GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "FINAL SETTLEMENT OF " + oEmployeeSalary.SettlementTypeInString; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

             
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LWD"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Notice Period"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Job Tenure"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Due"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Deduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PF(Accumulation)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PF(Company)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PF(Emp+Com)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gratuity"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EL Encashment"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Notice Period Deduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PF Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Other Dues"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                if (oEmployeeSalary.EmployeeID > 0)
                {
                    rowIndex++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.DateOfResigEffect; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateTime TempSubmissionDate = oEmployeeSettlement.SubmissionDate;
                    TempSubmissionDate = TempSubmissionDate.AddDays(1);

                    DateDifference dateDifference;
                    dateDifference = new DateDifference(oEmployeeSettlement.EffectDate, TempSubmissionDate);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dateDifference.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateTime TempDateOfJoin = oEmployeeSalary.DateOfJoin;
                    TempDateOfJoin = TempDateOfJoin.AddDays(1);

                    //dateDifference = new DateDifference(oEmployeeSalary.DateOfResigEffect, oEmployeeSalary.DateOfJoin);
                    dateDifference = new DateDifference(oEmployeeSalary.DateOfResigEffect, TempDateOfJoin);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = dateDifference.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEmployeeSalary.ELBalance; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GetAmountInStr(oEmployeeSalary.GrossPay, true, false); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GetAmountInStr(oEmployeeSalary.DeductionTotal, true, false); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GetAmountInStr(0, true, false); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GetAmountInStr(oEmployeeSalary.PF, true, false); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GetAmountInStr(oEmployeeSalary.Gratuity, true, false); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oFinalSettlementForms[0].TotalEL - oFinalSettlementForm.EnjoyedEl); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GetAmountInStr(oEmployeeSalary.NoticePayDeduction, true, false); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GetAmountInStr(oEmployeeSalary.PF, true, false); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = GetAmountInStr(oEmployeeSalary.EarningsTotal - oEmployeeSalary.DeductionTotal, true, false); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                else
                {
                    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Nothing to print!!"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 50; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FINALSETTLEMENT.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void PrintSettlementDetail_XL(string sParam)
        {
            List<EmployeeSalary_MAMIYA> oEmployeeSettlements = new List<EmployeeSalary_MAMIYA>();
            oEmployeeSettlements = EmployeeSalary_MAMIYA.GetsSettlementDetailList(sParam, ((User)(Session[SessionInfo.CurrentUser])).UserID, (int)((User)(Session[SessionInfo.CurrentUser])).FinancialUserType);

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("EMPLOYEE");
                sheet.Name = "EMPLOYEE";

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 15; //CODE
                sheet.Column(4).Width = 18; //Name
                sheet.Column(5).Width = 18; //Department 
                sheet.Column(6).Width = 18; //Designation
                sheet.Column(7).Width = 18; //Joining date
                sheet.Column(8).Width = 18; //Confirmation Date
                sheet.Column(9).Width = 18; //Last working date
                sheet.Column(10).Width = 18; //Service year
                sheet.Column(11).Width = 18; //Basic Days
                sheet.Column(12).Width = 18; //Allowance Days
                sheet.Column(13).Width = 18; //Shift Allowance Days
                sheet.Column(14).Width = 18; //SL Days/Hours
                sheet.Column(15).Width = 18; //LWP Days/Hours
                sheet.Column(16).Width = 18; //Absent Days/Hours
                sheet.Column(17).Width = 18; //No Work Days
                sheet.Column(18).Width = 18; //EL Balance
                sheet.Column(19).Width = 18; //OT (Normal)
                sheet.Column(20).Width = 18; //OT (Holiday)
                sheet.Column(21).Width = 18; //Basic
                sheet.Column(22).Width = 18; //House Rent
                sheet.Column(23).Width = 18; //Medical

                sheet.Column(24).Width = 18; //Gross Salary
                sheet.Column(25).Width = 18; //Basic Pay
                sheet.Column(26).Width = 18; //House Allowance

                sheet.Column(27).Width = 18; //Medical Allowance
                sheet.Column(28).Width = 18; //Conv. Allowance
                sheet.Column(29).Width = 18; //Other Allowance

                sheet.Column(30).Width = 18; //Shift Allowance
                sheet.Column(31).Width = 18; //No Work Allow.
                sheet.Column(32).Width = 18; //Att. Bonus

                sheet.Column(33).Width = 18; //Festival Bonus
                sheet.Column(34).Width = 18; //Adj. Cr. 
                sheet.Column(35).Width = 18; //OT (Normal)

                sheet.Column(36).Width = 18; //OT (Holiday)
                sheet.Column(37).Width = 18; //Holiday Allow.
                sheet.Column(38).Width = 18; //Notice Pay

                sheet.Column(39).Width = 18; //Gratuity
                sheet.Column(40).Width = 18; //EL Amount
                sheet.Column(41).Width = 18; //Fraction Rate

                sheet.Column(42).Width = 18; //Total Earning
                sheet.Column(43).Width = 18; //Sick
                sheet.Column(44).Width = 18; //Absent/LWP

                sheet.Column(45).Width = 18; //Transport
                sheet.Column(46).Width = 18; //Others
                sheet.Column(47).Width = 18; //Advance

                sheet.Column(48).Width = 18; //Income Tax
                sheet.Column(49).Width = 18; //Adj. Dr.
                sheet.Column(50).Width = 18; //Notice Pay

                sheet.Column(51).Width = 18; //Revenue Stamp
                sheet.Column(52).Width = 18; //Total Deduction
                sheet.Column(53).Width = 18; //Net Earning 

                nMaxColumn = 53;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SETTLEMENT LIST"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = " Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Confirmation Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Last working date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Service year"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Basic Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Allowance Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Shift Allowance Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "SL Days/Hours"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "LWP Days/Hours"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Absent Days/Hours"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "No Work Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

              
                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "EL Balance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "OT (Normal)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "OT (Holiday)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Basic"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "House Rent"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Medical"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++, rowIndex, colIndex=colIndex+16]; cell.Merge = true; cell.Value = "Earnings"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn-1]; cell.Merge = true; cell.Value = "Deduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nMaxColumn, rowIndex + 1, nMaxColumn]; cell.Merge = true; cell.Value = "Net Earning"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex = rowIndex + 1;
                colIndex = 25;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic Pay"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "House Allowance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Medical Allowance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Conv. Allowance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Other Allowance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift Allowance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No Work Allow."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Att. Bonus"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Festival Bonus"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Adj. Cr."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT (Normal)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT (Holiday)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Holiday Allow."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Notice Pay"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gratuity"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EL Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Fraction Rate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Earning"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sick"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Absent/LWP"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Transport"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Others"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Advance"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Income Tax"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Adj. Dr."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Notice Pay"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Revenue Stamp"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Deduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                #endregion

                #region Table Body

                int nSL = 0;
                foreach (EmployeeSalary_MAMIYA oItem in oEmployeeSettlements)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 
                    int Code = 0;
                    if (int.TryParse(oItem.EmployeeCode, out Code))
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Code; cell.Style.Font.Bold = false;
                        cell.Style.Numberformat.Format = "0";
                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    }
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfConfirmationInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfResigInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateTime TempDateOfJoin = oItem.DateOfJoin;
                    TempDateOfJoin = TempDateOfJoin.AddDays(1);

                    DateDifference ServiceYear = new DateDifference(oItem.DateOfResigEffect, TempDateOfJoin);
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ServiceYear.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateDifference dateDifference;
                    DateTime TempDateOfResigEffect = oItem.DateOfResigEffect;
                    TempDateOfResigEffect = TempDateOfResigEffect.AddDays(-(oItem.FriDay - 1));

                    if (oItem.DateOfJoin > oItem.SalaryStartDate) { dateDifference = new DateDifference(oItem.DateOfJoin, TempDateOfResigEffect); }
                    else { dateDifference = new DateDifference(oItem.SalaryStartDate, TempDateOfResigEffect); }

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (dateDifference.Days) > 0 ? (dateDifference.Days).ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (dateDifference.Days) > 0 ? (dateDifference.Days).ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ShiftAllDay > 0 ? oItem.ShiftAllDay.ToString() : "-";  cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AbsentHr_Sick > 0 ? oItem.AbsentHr_Sick.ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalUpLeave > 0 ? oItem.TotalUpLeave.ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalAbsent > 0 ? oItem.TotalAbsent.ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalNoWork > 0 ? oItem.TotalNoWork.ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ELBalance > 0 ? oItem.ELBalance.ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OT_NHR_HR > 0 ? oItem.OT_NHR_HR.ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OT_HHR_HR > 0 ? oItem.OT_HHR_HR.ToString() : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StructureBasic > 0 ? GetAmountInStr(oItem.StructureBasic, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StructureHR > 0 ? GetAmountInStr(oItem.StructureHR, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StructureMedical > 0 ? GetAmountInStr(oItem.StructureMedical, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StructureGross > 0 ? GetAmountInStr(oItem.StructureGross, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Basic > 0 ? GetAmountInStr(oItem.Basic, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.HRent > 0 ? GetAmountInStr(oItem.HRent, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Med > 0 ? GetAmountInStr(oItem.Med, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Conveyance > 0 ? GetAmountInStr(oItem.Conveyance, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OtherAll > 0 ? GetAmountInStr(oItem.OtherAll, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ShiftAmount > 0 ? GetAmountInStr(oItem.ShiftAmount, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalNoWorkDayAllowance > 0 ? GetAmountInStr(oItem.TotalNoWorkDayAllowance, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AttendanceBonus > 0 ? GetAmountInStr(oItem.AttendanceBonus, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.FB > 0 ? GetAmountInStr(oItem.FB, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ADJCR > 0 ? GetAmountInStr(oItem.ADJCR, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OT_NHR_AMT > 0 ? Global.MillionFormat(oItem.OT_NHR_AMT) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OT_HHR_AMT > 0 ? (Global.MillionFormat(oItem.OT_HHR_AMT)) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.HolidayAll > 0 ? GetAmountInStr(oItem.HolidayAll, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NoticePayAddition > 0 ? GetAmountInStr(oItem.NoticePayAddition, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Gratuity > 0 ? GetAmountInStr(oItem.Gratuity, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ELAmount > 0 ? GetAmountInStr(oItem.ELAmount, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Fracretained > 0 ? GetAmountInStr(oItem.Fracretained, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EarningsTotal > 0 ? GetAmountInStr(oItem.EarningsTotal, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SLDeduction > 0 ? GetAmountInStr(oItem.SLDeduction, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalAbsentAmount > 0 ? GetAmountInStr(oItem.TotalAbsentAmount, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TRNS > 0 ? GetAmountInStr(oItem.TRNS, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ADV > 0 ? GetAmountInStr(oItem.ADV, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.IncomeTax > 0 ? GetAmountInStr(oItem.IncomeTax, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ADJDR > 0 ? GetAmountInStr(oItem.ADJDR, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NoticePayDeduction > 0 ? GetAmountInStr(oItem.NoticePayDeduction, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =oItem.RS > 0 ? GetAmountInStr(oItem.RS, true, false) : "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value =oItem.DeductionTotal > 0 ? GetAmountInStr(oItem.DeductionTotal, true, false) : "-";cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (((oItem.EarningsTotal - oItem.DeductionTotal) < 0) ? ("(" + GetAmountInStr((oItem.EarningsTotal - oItem.DeductionTotal) * (-1), true, false) + ")") : (GetAmountInStr((oItem.EarningsTotal - oItem.DeductionTotal), true, false))); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    
                    rowIndex++;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SETTLEMENTDETAIL.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion XL

        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
        }

        #region Settlement Clearance
        public ActionResult View_EmployeeSettlementClearanceSections(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<EmployeeSettlementClearanceSection>  oEmployeeSettlementClearanceSections = new List<EmployeeSettlementClearanceSection>();
            oEmployeeSettlementClearanceSections = EmployeeSettlementClearanceSection.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(oEmployeeSettlementClearanceSections);
        }

        [HttpPost]
        public JsonResult EmployeeSettlementClearanceSection_IU(EmployeeSettlementClearanceSection oESCS)
        {
            try
            {
                if (oESCS.ESCSID > 0)
                {
                    oESCS = oESCS.IUD((int)EnumDBOperation.Update,((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oESCS = oESCS.IUD((int)EnumDBOperation.Insert,((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oESCS = new EmployeeSettlementClearanceSection();
                oESCS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oESCS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EmployeeSettlementClearanceSection_Delete(int nESCSID)
        {
            string sErrorMease = "";
            try
            {
                EmployeeSettlementClearanceSection oEmployeeSettlementClearanceSection = new EmployeeSettlementClearanceSection();
                oEmployeeSettlementClearanceSection.ESCSID = nESCSID;
                oEmployeeSettlementClearanceSection = oEmployeeSettlementClearanceSection.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sErrorMease = oEmployeeSettlementClearanceSection.ErrorMessage;
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeSettlementClearanceSetup_IU(EmployeeSettlementClearanceSetup oESCS)
        {
            try
            {
                if (oESCS.ESCSetupID > 0)
                {
                    oESCS = oESCS.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oESCS = oESCS.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oESCS = new EmployeeSettlementClearanceSetup();
                oESCS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oESCS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EmployeeSettlementClearanceSetup_Delete(int nESCSetupID)
        {
            string sErrorMease = "";
            try
            {
                EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup = new EmployeeSettlementClearanceSetup();
                oEmployeeSettlementClearanceSetup.ESCSetupID = nESCSetupID;
                oEmployeeSettlementClearanceSetup = oEmployeeSettlementClearanceSetup.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sErrorMease = oEmployeeSettlementClearanceSetup.ErrorMessage;
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Gets_EmployeeSettlementClearanceSetup(int nESCSID)
        {
            List<EmployeeSettlementClearanceSetup> oEmployeeSettlementClearanceSetups = new List<EmployeeSettlementClearanceSetup>();
            try
            {
                string sSql = "";
                if (nESCSID > 0) { sSql = "SELECT * FROM View_EmployeeSettlementClearanceSetup WHERE ESCSID=" + nESCSID; }
                else { sSql = "SELECT * FROM View_EmployeeSettlementClearanceSetup"; }
                oEmployeeSettlementClearanceSetups = EmployeeSettlementClearanceSetup.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup = new EmployeeSettlementClearanceSetup();
                oEmployeeSettlementClearanceSetups = new List<EmployeeSettlementClearanceSetup>();
                oEmployeeSettlementClearanceSetup.ErrorMessage = ex.Message;
                oEmployeeSettlementClearanceSetups.Add(oEmployeeSettlementClearanceSetup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSettlementClearanceSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeSettlementClearanceSetup_Inactive(EmployeeSettlementClearanceSetup oESCS)
        {
            try
            {
                if (oESCS.ESCSetupID > 0)
                {
                    oESCS = oESCS.IUD((int)EnumDBOperation.InActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    throw new Exception("Please select a valid item !");
                }
            }
            catch (Exception ex)
            {
                oESCS = new EmployeeSettlementClearanceSetup();
                oESCS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oESCS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View_EmployeeSettlementClearances(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSettlement).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            List<EmployeeSettlementClearance> oEmployeeSettlementClearances = new List<EmployeeSettlementClearance>();
            List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
            string sSql = "SELECT *  FROM View_EmployeeSettlement WHERE IsResigned=0 ";
            //oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeSettlements = oEmployeeSettlements;
            ViewBag.EnumSettleMentTypes = Enum.GetValues(typeof(EnumSettleMentType)).Cast<EnumSettleMentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            ViewBag.ClearanceStatus = Enum.GetValues(typeof(EnumESCrearance)).Cast<EnumESCrearance>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            //List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            //oAUOEDOs = AuthorizationUserOEDO.GetsByUser(((User)(Session[SessionInfo.CurrentUser])).UserID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //bool bView = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._View, "EmployeeSettlement", oAUOEDOs);
            //TempData["View"] = bView;

            return View(oEmployeeSettlementClearances);
        }

        #region EmployeeSettlementClearance_IUD
        [HttpPost]
        public JsonResult EmployeeSettlementClearance_IUD(EmployeeSettlementClearance oEmployeeSettlementClearance)
        {
            try
            {
                List<EmployeeSettlementClearance> oEmpSCs = new List<EmployeeSettlementClearance>();
                oEmpSCs = oEmployeeSettlementClearance.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEmpSCs.Count > 0)
                {
                    foreach (EmployeeSettlementClearance oitem in oEmpSCs)
                    {
                        string sSql = "";
                        List<Employee> oEmployees = new List<Employee>();
                        sSql = "SELECT * FROM view_Employee WHERE EmployeeID IN(SELECT EmployeeID FROM View_EmployeeSettlementClearanceSetup "
                                    + "WHERE InActiveDate IS NULL AND ESCSetupID IN(SELECT ESCSetupID FROM EmployeeSettlementClearance WHERE ESCID =" + oitem.ESCID + "))";
                        oEmployees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                        List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
                        sSql = "";
                        sSql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID IN(SELECT EmployeeSettlementID FROM EmployeeSettlementClearance WHERE ESCID =" + oitem.ESCID + ")";
                        oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                        if (oEmployees.Count > 0 && (Global.IsValidMail(oEmployees[0].Email)))
                        {
                            List<User> oUsers = new List<User>();
                            string sSQL = "Select * from View_User Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID;
                            oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                            List<string> emialTos = new List<string>();
                            emialTos.Add(oEmployees[0].Email);

                            string subject = "Settlement clearance ";
                            string message = "Please give a clearance for Mr./Mrs " + (oEmployeeSettlements.Count > 0 ? oEmployeeSettlements[0].EmployeeName : "") + " Clearance Issue.";
                            string bodyInfo = "";

                            bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2}"+
                                                  " <div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                                  oEmployees[0].Name, message, (oUsers.Count > 0 ? oUsers[0].EmployeeNameCode : ""),
                                                  Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));

                            #region Email Credential
                            EmailConfig oEmailConfig = new EmailConfig();
                            oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                            #endregion

                            Global.MailSend(subject, bodyInfo, emialTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oEmployeeSettlementClearance = new EmployeeSettlementClearance();
                oEmployeeSettlementClearance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSettlementClearance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion  EmployeeSettlementClearance_IUD

        [HttpPost]
        public JsonResult GetsEmployeeSettlementClearance_By_SettlementID(int nEmployeeSettlementID)
        {
            EmployeeSettlementClearance oEmployeeSettlementClearance = new EmployeeSettlementClearance();
            try
            {
                string sSql = "";
                if (nEmployeeSettlementID > 0) { sSql = "SELECT * FROM View_EmployeeSettlementClearance WHERE EmployeeSettlementID=" + nEmployeeSettlementID; }
                else { sSql = "SELECT * FROM View_EmployeeSettlementClearance"; }
                oEmployeeSettlementClearance.EmployeeSettlementClearances = EmployeeSettlementClearance.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "";
                if (nEmployeeSettlementID > 0) { sSql = "SELECT * FROM EmployeeSettlementClearanceHistory WHERE ESCID IN(SELECT ESCID FROM EmployeeSettlementClearance WHERE EmployeeSettlementID =" + nEmployeeSettlementID+")"; }
                else { sSql = "SELECT * FROM EmployeeSettlementClearanceHistory"; }
                oEmployeeSettlementClearance.EmployeeSettlementClearanceHistorys = EmployeeSettlementClearanceHistory.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                 oEmployeeSettlementClearance = new EmployeeSettlementClearance();
                oEmployeeSettlementClearance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSettlementClearance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region EmployeeSettlementClearanceHistory_IUD
        [HttpPost]
        public JsonResult EmployeeSettlementClearanceHistory_IUD(EmployeeSettlementClearanceHistory oEmployeeSettlementClearanceHistory)
        {
            try
            {
                oEmployeeSettlementClearanceHistory = oEmployeeSettlementClearanceHistory.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeSettlementClearanceHistory = new EmployeeSettlementClearanceHistory();
                oEmployeeSettlementClearanceHistory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSettlementClearanceHistory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion  EmployeeSettlementClearance_IUD

        [HttpPost]
        public JsonResult Gets_EmployeeSettlementClearanceSetup_ForSendTO(int nEmployeeSettlementID)
        {
            List<EmployeeSettlementClearanceSetup> oEmployeeSettlementClearanceSetups = new List<EmployeeSettlementClearanceSetup>();
            try
            {
                string sSql = "";
                if (nEmployeeSettlementID > 0) { sSql = "SELECT * FROM View_EmployeeSettlementClearanceSetup WHERE InActiveDate IS NULL" 
                                                        +" AND ESCSetupID NOT IN(SELECT ESCSetupID FROM EmployeeSettlementClearance "
                                                        +"WHERE EmployeeSettlementID=" + nEmployeeSettlementID + ")"; }
                else { sSql = "SELECT * FROM View_EmployeeSettlementClearanceSetup AND InActiveDate IS NULL"; }
                oEmployeeSettlementClearanceSetups = EmployeeSettlementClearanceSetup.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEmployeeSettlementClearanceSetups.Count <= 0)
                {
                    throw new Exception("Sent to everyone!");
                }
            }
            catch (Exception ex)
            {
                EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup = new EmployeeSettlementClearanceSetup();
                oEmployeeSettlementClearanceSetups = new List<EmployeeSettlementClearanceSetup>();
                oEmployeeSettlementClearanceSetup.ErrorMessage = ex.Message;
                oEmployeeSettlementClearanceSetups.Add(oEmployeeSettlementClearanceSetup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSettlementClearanceSetups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion Settlement Clearance


        #region Settlement Pay Slip
        public void PrintSettlementPaySlip_XL(int nEmployeeSettlementID)
        {
            EmployeeSalary oEmployeeSalary = GetEmployeeSettleemtSalary(nEmployeeSettlementID);

            var oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oEmployeeSalary.Company = oCompany;

            this.PrintSettlementPaySlipXL(oEmployeeSalary,1);
        }

        public EmployeeSalary GetEmployeeSettleemtSalary(int nEmployeeSettlementID)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            string sSql = "";
            sSql = "SELECT EmployeeSalaryID,EmployeeID,LocationID,DepartmentID,DesignationID,GrossAmount,NetAmount,CurrencyID,SalaryDate"
                    +",SalaryReceiveDate,EmployeeSettlementID,StartDate,EndDate,ProductionAmount,ProductionBonus,OTHour,OTRatePerHour,TotalWorkingDay"
                    +",TotalAbsent,TotalLate,TotalEarlyLeaving,TotalDayOff,TotalUpLeave,TotalPLeave,RevenueStemp,TotalNoWorkDay,TotalNoWorkDayAllowance"
                    +",AddShortFall,TotalHoliday,EmployeeName,EmployeeNameInBangla,EmployeeCode,JoiningDate,EmployeeTypeName,DateOfConfirmation"
                    +",LocationName,DepartmentName,DesignationName,IsProductionBase,IsAllowOverTime,IsAllowBankAccount,CashAmount"
                    +" FROM View_EmployeeSettlementSalary WHERE EmployeeSettlementID=" + nEmployeeSettlementID;

            oEmployeeSalary.EmployeeSalarys = EmployeeSalary.GetsEmployeeSettleemtSalary(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oEmployeeSalary.EmployeeSalarys.Count > 0)
            {
                sSql = "SELECT * FROM View_EmployeeSettlementSalaryDetail WHERE EmployeeSalaryID =" + oEmployeeSalary.EmployeeSalarys[0].EmployeeSalaryID;
                oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM VIEW_EmployeeBankAccount WHERE EmployeeID IN(" + oEmployeeSalary.EmployeeSalarys[0].EmployeeID + ") AND IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            sSql = "SELECT * FROM SalaryHead WHERE IsActive=1 ORDER BY SalaryHeadType";
            oEmployeeSalary.SalaryHeads = SalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return oEmployeeSalary;
        }

        private void PrintSettlementPaySlipXL(EmployeeSalary oEmployeeSalary, int nPrintCount)
        {
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            int nRowIndex = 2, nEndCol = 17;
            double nRowHeight = 12;

            Company oCompany = new Company();
            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
            List<EmployeeSalary> oEmployeeSalarys = new List<EmployeeSalary>();
            List<EmployeeSalaryDetail> oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();

            oCompany = oEmployeeSalary.Company;
            oSalaryHeads = oEmployeeSalary.SalaryHeads;
            oEmployeeSalarys = oEmployeeSalary.EmployeeSalarys;
            oEmployeeSalaryDetails = oEmployeeSalary.EmployeeSalaryDetails;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.2M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.2M;
                sheet.PrinterSettings.Orientation = eOrientation.Portrait;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;
                sheet.Name = "Pay Slip";

                #region Column Declare
                sheet.Column(1).Width = 3;//Extra
                sheet.Column(2).Width = 2; //Blank
                sheet.Column(3).Width = 10;//Caption
                sheet.Column(4).Width = 2; //:
                sheet.Column(5).Width = 10;//Basic Salary
                sheet.Column(6).Width = 10;//Other Salary
                sheet.Column(7).Width = 10;//Net Salary
                sheet.Column(8).Width = 2;//Blank

                sheet.Column(9).Width = 4; //Middle Blank

                sheet.Column(10).Width = 2; //Blank
                sheet.Column(11).Width = 10;//Caption
                sheet.Column(12).Width = 2; //:
                sheet.Column(13).Width = 10;//Basic Salary
                sheet.Column(14).Width = 10;//Other Salary
                sheet.Column(15).Width = 10;//Net Salary
                sheet.Column(16).Width = 2;//Blank
                sheet.Column(17).Width = 3;//Extra
                #endregion

                if (oEmployeeSalarys != null)
                {
                    int nLeftSideRowIndex = 0, nRightSideRowIndex = 0, nTempPrintCount = 0;
                    for (int i = 0; i < oEmployeeSalarys.Count; i = i + 2)
                    {
                        nTempPrintCount = nTempPrintCount + 2;
                        if (i <= (oEmployeeSalarys.Count - 1))
                        {
                            nLeftSideRowIndex = this.FillSettlementSalaraySlip(sheet, nRowIndex, oEmployeeSalarys[i], oEmployeeSalaryDetails, oSalaryHeads, oCompany, 2, nRowHeight);
                        }

                        if ((i + 1) <= (oEmployeeSalarys.Count - 1))
                        {
                            nRightSideRowIndex = this.FillSettlementSalaraySlip(sheet, nRowIndex, oEmployeeSalarys[i + 1], oEmployeeSalaryDetails, oSalaryHeads, oCompany, 10, nRowHeight);
                        }

                        nRowIndex = nLeftSideRowIndex;
                        if (nRightSideRowIndex > nLeftSideRowIndex)
                        {
                            nRowIndex = nRightSideRowIndex;
                        }

                        #region Blank
                        sheet.Row(nRowIndex).Height = 20;
                        sheet.Cells[nRowIndex, 2, nRowIndex, 16].Merge = true;
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "";
                        cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;
                        if (nTempPrintCount >= nPrintCount)
                        {
                            sheet.Row(nRowIndex).PageBreak = true;
                            nTempPrintCount = 0;
                        }
                        nRowIndex = nRowIndex + 1;
                        #endregion
                    }
                }
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Contractor.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private int FillSettlementSalaraySlip(OfficeOpenXml.ExcelWorksheet sheet, int nRowIndex, EmployeeSalary oEmployeeSalary, List<EmployeeSalaryDetail> oEmployeeSalaryDetails, List<SalaryHead> oSalaryHeads, Company oCompany, int nColumnIndex, double nRowHeight)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            #region Blank with Top/Left/Right Border
            sheet.Row(nRowIndex).Height = 5;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Company Name
            sheet.Row(nRowIndex).Height = nRowHeight;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = (oCompany.NameInBangla != "") ? oCompany.NameInBangla : oCompany.Name;
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region বেতন প্রদান রশিদ
            sheet.Row(nRowIndex).Height = nRowHeight;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "বেতন প্রদান রশিদ";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Salary Date
            sheet.Row(nRowIndex).Height = nRowHeight;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = oEmployeeSalary.StartDateInString + "-" + oEmployeeSalary.EndDateInString;
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region ইউনিট
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "ইউনিট";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.LocationName;
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region কার্ড নং
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "কার্ড নং";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.EmployeeCode;
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region নাম
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "নাম";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.EmployeeName;
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region পদবী
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "পদবী";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.DesignationName;
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region সেকশন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "সেকশন";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = oEmployeeSalary.DepartmentName + "      গ্রেড:" + oEmployeeSalary.EmployeeTypeName;
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region কর্মদিবস
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "কর্মদিবস";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = this.NumberFormat(oEmployeeSalary.TotalWorkingDay.ToString());
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            TimeSpan diff = oEmployeeSalary.EndDate - oEmployeeSalary.StartDate;
            int nDays = diff.Days + 1;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 6)]; cell.Merge = true;
            cell.Value = "মোট দিন : " + this.NumberFormat(nDays.ToString());
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region উপস্থিত দিন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "উপস্থিত দিন";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)];
            cell.Value = this.NumberFormat(oEmployeeSalary.TotalPresent.ToString());
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 6)]; cell.Merge = true;
            cell.Value = "মোট বন্ধের দিন : " + this.NumberFormat(oEmployeeSalary.TotalDayOff.ToString());
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region ছুটি
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "ছুটি";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)];
            cell.Value = this.NumberFormat((oEmployeeSalary.TotalUpLeave + oEmployeeSalary.TotalPLeave).ToString());
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 6)]; cell.Merge = true;
            cell.Value = "মোট ছুটির দিন : " + this.NumberFormat(oEmployeeSalary.TotalHoliday.ToString());
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region অনুপস্থিত দিন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "অনুপস্থিত দিন";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = this.NumberFormat(oEmployeeSalary.TotalAbsent.ToString());
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region মোট বেতন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "মোট বেতন";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = this.NumberFormat(oEmployeeSalary.GrossAmount > 0 ? this.GetAmountInStr(oEmployeeSalary.GrossAmount, true, false) : "-");
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            double nAmount = 0; string sHeadName = "";
            List<EmployeeSalaryDetail> oTempEmployeeSalaryDetails = oEmployeeSalaryDetails.Where(x => x.EmployeeSalaryID == oEmployeeSalary.EmployeeSalaryID).ToList();

            #region Salary Head
            foreach (SalaryHead oItem in oSalaryHeads)
            {
                nAmount = (oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).Count() > 0) ? oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).FirstOrDefault().Amount : 0;
                if (!string.Equals(@oItem.Name.ToLower(), "n/a") && !string.Equals(@oItem.Name.Trim(), ""))
                {
                    sHeadName = ((!string.Equals(@oItem.NameInBangla.ToLower(), "n/a") && !string.Equals(@oItem.NameInBangla.Trim(), "")) ? @oItem.NameInBangla : @oItem.Name);
                }

                sheet.Row(nRowIndex).Height = nRowHeight;
                cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
                cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = sHeadName;
                cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
                cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                if (oItem.SalaryHeadType == EnumSalaryHeadType.Basic || oItem.SalaryHeadType == EnumSalaryHeadType.Basic)
                {
                    sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = this.NumberFormat(ICS.Core.Utility.Global.MillionFormat(nAmount).Split('.')[0]);
                    cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                }
                else
                {
                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 3)]; cell.Value = "";
                    cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 5)].Merge = true;
                    cell = sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 5)]; cell.Value = this.NumberFormat(ICS.Core.Utility.Global.MillionFormat(nAmount).Split('.')[0]);
                    cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                }

                cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
                cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
                nRowIndex = nRowIndex + 1;
            }
            #endregion

            #region সর্বমোট বেতন
            nAmount = oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadType == (int)ESimSol.BusinessObjects.EnumSalaryHeadType.Basic).Sum(x => x.Amount) +
                           oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadType == (int)ESimSol.BusinessObjects.EnumSalaryHeadType.Addition).Sum(x => x.Amount) -
                           oTempEmployeeSalaryDetails.Where(x => x.SalaryHeadType == (int)ESimSol.BusinessObjects.EnumSalaryHeadType.Deduction).Sum(x => x.Amount);

            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "সর্বমোট বেতন";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = this.NumberFormat(ICS.Core.Utility.Global.MillionFormat(nAmount).Split('.')[0]);
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region ওভারটাইম হার
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "ওভারটাইম হার";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = this.NumberFormat(oEmployeeSalary.OTRatePerHour.ToString());
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region ওভারটাইম ঘন্টা
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "ওভারটাইম ঘন্টা";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


            Double OTmin = oEmployeeSalary.OTHour * 60;
            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = Global.MinInHourMin(Convert.ToInt32(OTmin));
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region ওভারটাইম চার্জ
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "ওভারটাইম চার্জ";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = this.NumberFormat(ICS.Core.Utility.Global.MillionFormat(oEmployeeSalary.OverTimeAmount).Split('.')[0]);
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region প্রদেয় বেতন
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "প্রদেয় বেতন";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 2)]; cell.Value = ":";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 3), nRowIndex, (nColumnIndex + 5)]; cell.Value = this.NumberFormat(ICS.Core.Utility.Global.MillionFormat(oEmployeeSalary.OverTimeAmount + nAmount).Split('.')[0]);
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 20;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Under Line
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex, (nColumnIndex + 3)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "_________________";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


            sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = "_________________";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region স্বাক্ষর
            sheet.Row(nRowIndex).Height = nRowHeight;
            cell = sheet.Cells[nRowIndex, nColumnIndex]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            sheet.Cells[nRowIndex, (nColumnIndex + 1), nRowIndex, (nColumnIndex + 3)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 1)]; cell.Value = "শ্রমিকের স্বাক্ষর";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;


            sheet.Cells[nRowIndex, (nColumnIndex + 4), nRowIndex, (nColumnIndex + 5)].Merge = true;
            cell = sheet.Cells[nRowIndex, (nColumnIndex + 4)]; cell.Value = "কর্তৃপক্ষের স্বাক্ষর";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

            cell = sheet.Cells[nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Left Right Border
            sheet.Row(nRowIndex).Height = 5;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            #region Blank with Bottom/Left/Right Border
            sheet.Row(nRowIndex).Height = 2;
            sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)].Merge = true;
            cell = sheet.Cells[nRowIndex, nColumnIndex, nRowIndex, (nColumnIndex + 6)]; cell.Value = "";
            cell.Style.Font.Size = 8; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
            nRowIndex = nRowIndex + 1;
            #endregion

            return nRowIndex;
        }
        public string NumberFormat(string sNum)
        {
            char[] NumbersInBangla = { '০', '১', '২', '৩', '৪', '৫', '৬', '৭', '৮', '৯' };
            char[] NumbersInEnglish = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            char[] arr = sNum.ToCharArray();

            foreach (char ch in arr)
            {
                int i = 0;
                while (i != 10)
                {
                    if (ch == NumbersInEnglish[i])
                    {
                        sNum = sNum.Replace(ch, NumbersInBangla[i]);
                        break;
                    }
                    i++;
                }
            }
            return sNum;
        }

        #endregion Settlement Pay Slip




        #region SetlementSummary
        public void EmpSettlementSummary_XL(string sParam)
        {
            DateTime dtStartDate = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(sParam.Split('~')[1]);
            Int16 nSettlementType = Convert.ToInt16(sParam.Split('~')[2]);
            string sDepartmentIds = sParam.Split('~')[3];
            string sDesignationIds =sParam.Split('~')[4];
            Int16 nClearanceStatus =  Convert.ToInt16(sParam.Split('~')[5]);
            Int16 nApproveStatus = Convert.ToInt16(sParam.Split('~')[6]);

            string sBUIDs = sParam.Split('~')[7];
            string sLocationIds = sParam.Split('~')[8];
            string sEmpIDs = sParam.Split('~')[9];

            List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
            List<EmployeeSettlementSalary> oEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
            List<EmployeeSettlementSalaryDetail> oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            string sSql = "";

            sSql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID<>0 AND EffectDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (!string.IsNullOrEmpty(sBUIDs))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE BusinessUnitID IN(" + sBUIDs + "))";
            }
            if (!string.IsNullOrEmpty(sLocationIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE LocationID IN(" + sLocationIds + "))";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DesignationID IN(" + sDesignationIds + "))";
            }

            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN("+sEmpIDs+")";
            }


            if (nSettlementType > 0)
            {
                sSql = sSql + " AND SettlementType=" + nSettlementType;
            }

            if (nApproveStatus == 1)
            {
                sSql = sSql + " AND ApproveBy>0";
            }
            if (nApproveStatus == 2)
            {
                sSql = sSql + " AND ApproveBy<=0";
            }
            if (nClearanceStatus > 0)
            {
                sSql = sSql + " AND EmployeeSettlementID IN(SELECT EmployeeSettlementID  FROM EmployeeSettlementClearance WHERE CurrentStatus=" + nClearanceStatus + ")";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }

            oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //End EmployeeSettlement


            string tEmpIDs = "";
            if (oEmployeeSettlements.Count > 0)
            {
                foreach (EmployeeSettlement oItem in oEmployeeSettlements)
                {
                    tEmpIDs += oItem.EmployeeID + ",";
                }
                tEmpIDs = tEmpIDs.Remove(tEmpIDs.Length - 1, 1);
            }


            //EmployeeSettlementSalary
            string sSQL = "";
            if (!string.IsNullOrEmpty(tEmpIDs))
            {
                sSQL += "SELECT * FROM View_EmployeeSettlementSalary WHERE EmployeeID IN(" + tEmpIDs + ")";
                oEmployeeSettlementSalarys = EmployeeSettlementSalary.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            //EmployeeSettlementSalaryDetail
            string EmpIDs = "";
            if (oEmployeeSettlementSalarys.Count > 0)
            {
                string TempEmpIDs = "";
                int nCount = 0;
                oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
                foreach (EmployeeSettlementSalary oItem in oEmployeeSettlementSalarys)
                {
                    TempEmpIDs += oItem.EmployeeID + ",";
                    EmpIDs += oItem.EmployeeID + ",";
                    nCount++;

                    if (nCount % 100 == 0 || nCount == oEmployeeSettlementSalarys.Count)
                    {
                        TempEmpIDs = TempEmpIDs.Remove(TempEmpIDs.Length - 1, 1);
                        sSql = "";
                        sSql = "SELECT * FROM View_EmployeeSettlementSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSettlementSalary WHERE EmployeeID IN (" + TempEmpIDs + ")) ORDER BY SalaryHeadID";
                        List<EmployeeSettlementSalaryDetail> oTempEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
                        oTempEmployeeSettlementSalaryDetails = EmployeeSettlementSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        oEmployeeSettlementSalaryDetails.AddRange(oTempEmployeeSettlementSalaryDetails);
                        TempEmpIDs = "";
                    }
                }
                EmpIDs = EmpIDs.Remove(EmpIDs.Length - 1, 1);
            }
            else
            {
                oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            }

            List<string> ColEarnings = new List<string>();
            ColEarnings = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition)).Select(x => x.SalaryHeadName).ToList();
            ColEarnings = ColEarnings.Distinct().ToList();

            List<string> ColDeductions = new List<string>();
            ColDeductions = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction).Select(x => x.SalaryHeadName).ToList();
            ColDeductions = ColDeductions.Distinct().ToList();
           
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            oLeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead WHERE IsActive <> 0", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            sSql = "SELECT * FROM View_AttendanceDaily WHERE AttendanceDate BETWEEN '" +dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (!string.IsNullOrEmpty(EmpIDs))
            {
                sSql += " AND EmployeeID IN(" + EmpIDs + ")";
            }
            oAttendanceDailys = AttendanceDaily.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //var attList = oAttendanceDailys.GroupBy(x => x.LeaveHeadID).Select(y => y.First()).Distinct();
            var attList = oAttendanceDailys.Where(x=>x.LeaveHeadID>0).GroupBy(x => new { x.LeaveHeadID}, (key, grp) => new
            {
                LeaveHeadID = key.LeaveHeadID,
                result = grp,

            }).OrderBy(x => x.LeaveHeadID).ToList();
            List<string> ColLeaveHeads = new List<string>();
            foreach (var col in attList)
            {
                foreach (LeaveHead lh in oLeaveHeads)
                {
                    if (col.LeaveHeadID == lh.LeaveHeadID)
                    {
                        ColLeaveHeads.Add(lh.ShortName);
                    }
                }
            }



            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SettlementSummary");
                sheet.Name = "SettlementSummary";

                int nCount = 2;
                sheet.Column(nCount++).Width = 8; //SL
                sheet.Column(nCount++).Width = 15; //CODE
                sheet.Column(nCount++).Width = 18; //Name
                sheet.Column(nCount++).Width = 18; //Department 
                sheet.Column(nCount++).Width = 18; //Designation
                sheet.Column(nCount++).Width = 18; //Joining date
                sheet.Column(nCount++).Width = 18; //Total Day
                sheet.Column(nCount++).Width = 18; //Present Day
                sheet.Column(nCount++).Width = 18; //DayOff
                sheet.Column(nCount++).Width = 18; //Absent

                //if (ColLeaveHeads.Count > 0)
                //{
                //    foreach (string col in ColLeaveHeads)
                //    {
                //        sheet.Column(nCount++).Width = 12;
                //    }
                //}

                sheet.Column(nCount++).Width = 18; //Leave Days
                sheet.Column(nCount++).Width = 18; //EWD
                sheet.Column(nCount++).Width = 18; //Early Out Days
                sheet.Column(nCount++).Width = 18; //Early Out Mins
                sheet.Column(nCount++).Width = 18; //Late Days
                sheet.Column(nCount++).Width = 18; //Late Mins
                sheet.Column(nCount++).Width = 18; //OT Hr
                sheet.Column(nCount++).Width = 18; //OT Rate
                sheet.Column(nCount++).Width = 18; //Present Salary

                if (ColEarnings.Count > 0)
                {
                    for (int i = 0; i < ColEarnings.Count; i++)
                    {
                        sheet.Column(nCount++).Width = 18;
                    }
                }
                sheet.Column(nCount++).Width = 18; //OT Amount
                sheet.Column(nCount++).Width = 18; //Gross Earnings


                if (ColDeductions.Count > 0)
                {
                    for (int i = 0; i < ColDeductions.Count; i++)
                    {
                        sheet.Column(nCount++).Width = 18;
                    }
                }

                sheet.Column(nCount++).Width = 18; //Gross Deductions
                sheet.Column(nCount++).Width = 18; //Net Amount
                sheet.Column(nCount++).Width = 18; //Signature

                nMaxColumn = nCount;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SETTLEMENT SUMMARY"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Month Cycle Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Present Day"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Day Off Holidays"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Absent Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //foreach (string sColumn in ColAttDetail)
                //{
                //    if (ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any())
                //    {
                //        foreach (LeaveHead oItem in oLeaveHeads)
                //        {
                //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.ShortName; cell.Style.Font.Bold = true;
                //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        }
                //    }
                //}

                //if (ColLeaveHeads.Count > 0)
                //{
                //    foreach (string col in ColLeaveHeads)
                //    {
                //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = col; cell.Style.Font.Bold = true;
                //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    }
                //}

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "CL"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "ML"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "EL"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "LWP"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Leave Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "EWD"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Early Out Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Early Out Mins"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Late Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Late Mins"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "OT Hr"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "OT Rate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];  cell.Merge = true; cell.Value = "Present Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Addition fields
                //if (ColEarnings.Count > 0)
                //{
                //    for (int i = 0; i < ColEarnings.Count; i++)
                //    {

                //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = ColEarnings[i]; cell.Style.Font.Bold = true;
                //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //    }
                //}
                foreach (string sItem in ColEarnings)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = sItem; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "OT Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Gross Earnings"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Deduction fields
                //if (ColDeductions.Count > 0)
                //{
                //    for (int i = 0; i < ColDeductions.Count; i++)
                //    {

                //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = ColDeductions[i]; cell.Style.Font.Bold = true;
                //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //    }
                //}
                foreach (string sItem in ColDeductions)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = sItem; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }



                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Deductions"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                #region Table Body

                
                int nSL = 0;
                foreach (EmployeeSettlementSalary oItem in oEmployeeSettlementSalarys)
                {
                    //var oEarnings = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Addition && x.EmployeeSalaryID == oItem.EmployeeSalaryID)).ToList();
                    //oEarnings = oEarnings.Distinct().ToList();
                    //var oDeductions = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                    //oDeductions = oDeductions.Distinct().ToList();
                    var oEarnings = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition && x.EmployeeSalaryID == oItem.EmployeeSalaryID)).ToList();
                    var oDeductions = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();

                    
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //joining date
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Month cycle day
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.EndDate - oItem.StartDate).TotalDays + 1; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Present
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Present; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //TotalDayOff
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalDayOff + oItem.TotalHoliday; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //TotalAbsent
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalAbsent; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //
                    int leaveCount = 0;
                    int totalLeave = 0;
                    foreach (var list in attList)
                    {
                        leaveCount = oAttendanceDailys.Where(x=>(x.EmployeeID == oItem.EmployeeID && x.LeaveHeadID == list.LeaveHeadID)).Count();
                        totalLeave += leaveCount;
                        //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = leaveCount; cell.Style.Font.Bold = false;
                        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    //totalLeave
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = totalLeave; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //EWD = TotalPresent + TotalHoliday + TotalDayOff + TotalPLeave + TotalUpLeave
                    int EWD = 0;
                    EWD = oItem.Present + oItem.TotalHoliday + oItem.TotalDayOff + oItem.TotalPLeave + oItem.TotalUpLeave;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EWD; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    var currentEmpAtt = oAttendanceDailys.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();

                    int earlyOutDays = currentEmpAtt.Where(x => x.EarlyDepartureMinute > 0).Count();
                    double earlyOutMins = currentEmpAtt.Sum(x => x.EarlyDepartureMinute);

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = earlyOutDays; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = earlyOutMins; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    int LateDays = currentEmpAtt.Where(x => x.LateArrivalMinute > 0).Count();
                    double lateMins = currentEmpAtt.Sum(x => x.LateArrivalMinute);

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = LateDays; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = lateMins; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OTHour; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OTRatePerHour; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //GrossAmount
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossAmount; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double grossEarnings = 0.0;
                    double nAmount;
                    //if (oEarnings.Count > 0)
                    //{
                    //    foreach (EmployeeSettlementSalaryDetail oEarningsItem in oEarnings.OrderBy(x => x.SalaryHeadType))
                    //    {
                    //        var oESDs = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadID == oEarningsItem.SalaryHeadID && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                    //        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;
                    //        nAmount = Math.Round(nAmount);

                    //        grossEarnings += nAmount;
                    //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                    //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //    }

                    //}
                    foreach (string sItem in ColEarnings)
                    {
                        var oESDs = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadName == sItem && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;

                        //nAmount = (oSalaryHead.SalaryHeadType == EnumSalaryHeadType.Basic) ? nAmount : Math.Round(nAmount);
                        nAmount = Math.Round(nAmount);

                        grossEarnings +=  nAmount;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oItem.OTHour * oItem.OTRatePerHour); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //oItem.GrossAmount + grossEarnings + OTAmount == Gross Earnings
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossAmount + grossEarnings + Math.Round(oItem.OTHour * oItem.OTRatePerHour); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double grossDeductions = 0.0;
                    //if (oDeductions.Count > 0)
                    //{
                    //    foreach (EmployeeSettlementSalaryDetail oDeductionItem in oDeductions)
                    //    {
                    //        var oESDs = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadID == oDeductionItem.SalaryHeadID && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                    //        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;
                    //        nAmount = Math.Round(nAmount);

                    //        grossDeductions += nAmount;
                    //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                    //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //    }

                    //}
                    foreach (string sItem in ColDeductions)
                    {
                        var oESDs = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadName == sItem && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;
                        nAmount = Math.Round(nAmount);

                        grossDeductions += nAmount;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = grossDeductions; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //((oItem.GrossAmount + grossEarnings) - grossDeductions) == Net Amount
                    //((oItem.GrossAmount + grossEarnings + Math.Floor(oItem.OTHour * oItem.OTRatePerHour)) - grossDeductions)
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oItem.NetAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex++;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SETTLEMENT_SUMMARY.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        public void EmpSettlementSummary_XL_AMG(string sParam)
        {
            DateTime dtStartDate = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(sParam.Split('~')[1]);
            Int16 nSettlementType = Convert.ToInt16(sParam.Split('~')[2]);
            string sDepartmentIds = sParam.Split('~')[3];
            string sDesignationIds = sParam.Split('~')[4];
            Int16 nClearanceStatus = Convert.ToInt16(sParam.Split('~')[5]);
            Int16 nApproveStatus = Convert.ToInt16(sParam.Split('~')[6]);

            string sBUIDs = sParam.Split('~')[7];
            string sLocationIds = sParam.Split('~')[8];
            string sEmpIDs = sParam.Split('~')[9];

            List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
            List<EmployeeSettlementSalary> oEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
            List<EmployeeSettlementSalary> oSummaryEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
            List<EmployeeSettlementSalaryDetail> oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            List<EmployeeSettlementSalaryDetail> oTEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            string sSql = "";

            sSql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID<>0 AND EffectDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (!string.IsNullOrEmpty(sBUIDs))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE BusinessUnitID IN(" + sBUIDs + "))";
            }
            if (!string.IsNullOrEmpty(sLocationIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE LocationID IN(" + sLocationIds + "))";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DesignationID IN(" + sDesignationIds + "))";
            }

            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN(" + sEmpIDs + ")";
            }


            if (nSettlementType > 0)
            {
                sSql = sSql + " AND SettlementType=" + nSettlementType;
            }

            if (nApproveStatus == 1)
            {
                sSql = sSql + " AND ApproveBy>0";
            }
            if (nApproveStatus == 2)
            {
                sSql = sSql + " AND ApproveBy<=0";
            }
            if (nClearanceStatus > 0)
            {
                sSql = sSql + " AND EmployeeSettlementID IN(SELECT EmployeeSettlementID  FROM EmployeeSettlementClearance WHERE CurrentStatus=" + nClearanceStatus + ")";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }

            oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //End EmployeeSettlement


            string tEmpIDs = "";
            if (oEmployeeSettlements.Count > 0)
            {
                foreach (EmployeeSettlement oItem in oEmployeeSettlements)
                {
                    tEmpIDs += oItem.EmployeeID + ",";
                }
                tEmpIDs = tEmpIDs.Remove(tEmpIDs.Length - 1, 1);
            }


            //EmployeeSettlementSalary
            string sSQL = "";
            if (!string.IsNullOrEmpty(tEmpIDs))
            {
                sSQL += "SELECT * FROM View_EmployeeSettlementSalary WHERE EmployeeID IN(" + tEmpIDs + ")";
                oEmployeeSettlementSalarys = EmployeeSettlementSalary.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            //EmployeeSettlementSalaryDetail
            string EmpIDs = "";
            if (oEmployeeSettlementSalarys.Count > 0)
            {
                string TempEmpIDs = "";
                int nCount = 0;
                oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
                foreach (EmployeeSettlementSalary oItem in oEmployeeSettlementSalarys)
                {
                    TempEmpIDs += oItem.EmployeeID + ",";
                    EmpIDs += oItem.EmployeeID + ",";
                    nCount++;

                    if (nCount % 100 == 0 || nCount == oEmployeeSettlementSalarys.Count)
                    {
                        TempEmpIDs = TempEmpIDs.Remove(TempEmpIDs.Length - 1, 1);
                        sSql = "";
                        sSql = "SELECT * FROM View_EmployeeSettlementSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSettlementSalary WHERE EmployeeID IN (" + TempEmpIDs + ")) ORDER BY SalaryHeadID";
                        List<EmployeeSettlementSalaryDetail> oTempEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
                        oTempEmployeeSettlementSalaryDetails = EmployeeSettlementSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        oEmployeeSettlementSalaryDetails.AddRange(oTempEmployeeSettlementSalaryDetails);
                        _oTEmployeeSettlementSalaryDetails.AddRange(oTempEmployeeSettlementSalaryDetails);
                        TempEmpIDs = "";
                    }
                }
                EmpIDs = EmpIDs.Remove(EmpIDs.Length - 1, 1);
            }
            else
            {
                oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            }

            List<string> ColEarnings = new List<string>();
            ColEarnings = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition)).Select(x => x.SalaryHeadName).ToList();
            ColEarnings = ColEarnings.Distinct().ToList();

            List<string> ColDeductions = new List<string>();
            //ColDeductions = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction && (x.SalaryHeadID == 8 || x.SalaryHeadID == 20 || x.SalaryHeadID == 25 || x.SalaryHeadID == 26))).Select(x => x.SalaryHeadName).ToList();

            ColDeductions = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction && (x.SalaryHeadID == 8 || x.SalaryHeadID == 20 || x.SalaryHeadID == 25 || x.SalaryHeadID == 26))).Select(x => x.SalaryHeadName).ToList();
            List<string> allCols = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction)).Select(x => x.SalaryHeadName).ToList();

            ColDeductions = allCols.Except(oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction && (x.SalaryHeadID == 8 || x.SalaryHeadID == 20 || x.SalaryHeadID == 25 || x.SalaryHeadID == 26))).Select(x => x.SalaryHeadName).ToList()).ToList();
            

            ColDeductions = ColDeductions.Distinct().ToList();

            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            oLeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead WHERE IsActive <> 0", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            sSql = "SELECT * FROM View_AttendanceDaily WHERE AttendanceDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (!string.IsNullOrEmpty(EmpIDs))
            {
                sSql += " AND EmployeeID IN(" + EmpIDs + ")";
            }
            oAttendanceDailys = AttendanceDaily.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //var attList = oAttendanceDailys.GroupBy(x => x.LeaveHeadID).Select(y => y.First()).Distinct();
            var attList = oAttendanceDailys.Where(x => x.LeaveHeadID > 0).GroupBy(x => new { x.LeaveHeadID }, (key, grp) => new
            {
                LeaveHeadID = key.LeaveHeadID,
                result = grp,

            }).OrderBy(x => x.LeaveHeadID).ToList();
            List<string> ColLeaveHeads = new List<string>();
            foreach (var col in attList)
            {
                foreach (LeaveHead lh in oLeaveHeads)
                {
                    if (col.LeaveHeadID == lh.LeaveHeadID)
                    {
                        ColLeaveHeads.Add(lh.ShortName);
                    }
                }
            }



            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("SettlementSummary");
                sheet.Name = "SettlementSummary";

                int nCount = 2;
                sheet.Column(nCount++).Width = 8; //SL
                sheet.Column(nCount++).Width = 15; //CODE
                sheet.Column(nCount++).Width = 18; //Name
                sheet.Column(nCount++).Width = 18; //Department 
                sheet.Column(nCount++).Width = 18; //Designation
                sheet.Column(nCount++).Width = 18; //Joining date
                sheet.Column(nCount++).Width = 18; //Total Day
                sheet.Column(nCount++).Width = 18; //Present Day
                sheet.Column(nCount++).Width = 18; //DayOff
                sheet.Column(nCount++).Width = 18; //Absent

                //if (ColLeaveHeads.Count > 0)
                //{
                //    foreach (string col in ColLeaveHeads)
                //    {
                //        sheet.Column(nCount++).Width = 12;
                //    }
                //}

                sheet.Column(nCount++).Width = 18; //Leave Days
                sheet.Column(nCount++).Width = 18; //EWD
                sheet.Column(nCount++).Width = 18; //Early Out Days
                sheet.Column(nCount++).Width = 18; //Early Out Mins
                sheet.Column(nCount++).Width = 18; //Late Days
                sheet.Column(nCount++).Width = 18; //Late Mins
                sheet.Column(nCount++).Width = 18; //OT Hr
                sheet.Column(nCount++).Width = 18; //OT Rate
                sheet.Column(nCount++).Width = 18; //Present Salary

                if (ColEarnings.Count > 0)
                {
                    for (int i = 0; i < ColEarnings.Count; i++)
                    {
                        sheet.Column(nCount++).Width = 18;
                    }
                }
                sheet.Column(nCount++).Width = 18; //OT Amount
                //sheet.Column(nCount++).Width = 18; //Gross Earnings


                if (ColDeductions.Count > 0)
                {
                    for (int i = 0; i < ColDeductions.Count; i++)
                    {
                        sheet.Column(nCount++).Width = 18;
                    }
                }

                sheet.Column(nCount++).Width = 18; //Gross Deductions
                sheet.Column(nCount++).Width = 18; //Net Amount
                sheet.Column(nCount++).Width = 18; //Signature

                nMaxColumn = nCount;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "SETTLEMENT SUMMARY"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Month Cycle Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Present Day"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Day Off Holidays"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Absent Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //foreach (string sColumn in ColAttDetail)
                //{
                //    if (ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any())
                //    {
                //        foreach (LeaveHead oItem in oLeaveHeads)
                //        {
                //            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = oItem.ShortName; cell.Style.Font.Bold = true;
                //            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //        }
                //    }
                //}

                //if (ColLeaveHeads.Count > 0)
                //{
                //    foreach (string col in ColLeaveHeads)
                //    {
                //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = col; cell.Style.Font.Bold = true;
                //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //    }
                //}

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "CL"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "ML"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "EL"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "LWP"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Leave Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "EWD"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Early Out Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Early Out Mins"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Late Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Late Mins"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "OT Hr"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "OT Rate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Present Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Addition fields
                //if (ColEarnings.Count > 0)
                //{
                //    for (int i = 0; i < ColEarnings.Count; i++)
                //    {

                //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = ColEarnings[i]; cell.Style.Font.Bold = true;
                //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //    }
                //}
                foreach (string sItem in ColEarnings)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = sItem; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "OT Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Gross Earnings"; cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Deduction fields
                //if (ColDeductions.Count > 0)
                //{
                //    for (int i = 0; i < ColDeductions.Count; i++)
                //    {

                //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = ColDeductions[i]; cell.Style.Font.Bold = true;
                //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //    }
                //}
                foreach (string sItem in ColDeductions)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = sItem; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }



                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Deductions"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Signature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                #region Table Body

                double nTotalGrossDeduction = 0.0;
                double nTotalGrossDeductionGrand = 0.0;
                double nTotalGrossEarning = 0.0;

                oEmployeeSettlementSalarys = oEmployeeSettlementSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                oEmployeeSettlementSalarys.ForEach(x => oSummaryEmployeeSettlementSalarys.Add(x));
                while (oEmployeeSettlementSalarys.Count > 0)
                {
                    List<EmployeeSettlementSalary> oTempEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
                    oTempEmployeeSettlementSalarys = oEmployeeSettlementSalarys.Where(x => x.LocationName == oEmployeeSettlementSalarys[0].LocationName).ToList();
                    string sLocationName = oTempEmployeeSettlementSalarys.Count > 0 ? oTempEmployeeSettlementSalarys[0].LocationName : "";
                    while (oTempEmployeeSettlementSalarys.Count > 0)
                    {
                        List<EmployeeSettlementSalary> oTempEmpSs = new List<EmployeeSettlementSalary>();
                        oTempEmpSs = oTempEmployeeSettlementSalarys.Where(x => x.DepartmentName == oTempEmployeeSettlementSalarys[0].DepartmentName).ToList();

                        //this.PrintHeader();
                        //PrintHaedRow(oTempEmpSs[0], _nGroupDept);

                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Unit:" + oTempEmpSs[0].LocationName + ", " + "Department:" + oTempEmpSs[0].DepartmentName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex += 1;

                        int nSL = 0;
                        foreach (EmployeeSettlementSalary oItem in oTempEmpSs)
                        {
                            //var oEarnings = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Addition && x.EmployeeSalaryID == oItem.EmployeeSalaryID)).ToList();
                            //oEarnings = oEarnings.Distinct().ToList();
                            //var oDeductions = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                            //oDeductions = oDeductions.Distinct().ToList();
                            var oEarnings = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition && x.EmployeeSalaryID == oItem.EmployeeSalaryID)).ToList();
                            var oDeductions = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();


                            nSL++;
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //joining date
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Month cycle day
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.EndDate - oItem.StartDate).TotalDays + 1; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Present
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Present; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //TotalDayOff
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalDayOff + oItem.TotalHoliday; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //TotalAbsent
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalAbsent; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //
                            int leaveCount = 0;
                            int totalLeave = 0;
                            foreach (var list in attList)
                            {
                                leaveCount = oAttendanceDailys.Where(x => (x.EmployeeID == oItem.EmployeeID && x.LeaveHeadID == list.LeaveHeadID)).Count();
                                totalLeave += leaveCount;
                                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = leaveCount; cell.Style.Font.Bold = false;
                                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }

                            //totalLeave
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = totalLeave; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //EWD = TotalPresent + TotalHoliday + TotalDayOff + TotalPLeave + TotalUpLeave
                            int EWD = 0;
                            EWD = oItem.Present + oItem.TotalHoliday + oItem.TotalDayOff + oItem.TotalPLeave + oItem.TotalUpLeave;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EWD; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            var currentEmpAtt = oAttendanceDailys.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();

                            int earlyOutDays = currentEmpAtt.Where(x => x.EarlyDepartureMinute > 0).Count();
                            double earlyOutMins = currentEmpAtt.Sum(x => x.EarlyDepartureMinute);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = earlyOutDays; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = earlyOutMins; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                            int LateDays = currentEmpAtt.Where(x => x.LateArrivalMinute > 0).Count();
                            double lateMins = currentEmpAtt.Sum(x => x.LateArrivalMinute);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = LateDays; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = lateMins; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OTHour; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OTRatePerHour; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //GrossAmount
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossAmount; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            double grossEarnings = 0.0;
                            double nAmount;
                            //if (oEarnings.Count > 0)
                            //{
                            //    foreach (EmployeeSettlementSalaryDetail oEarningsItem in oEarnings.OrderBy(x => x.SalaryHeadType))
                            //    {
                            //        var oESDs = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadID == oEarningsItem.SalaryHeadID && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                            //        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;
                            //        nAmount = Math.Round(nAmount);

                            //        grossEarnings += nAmount;
                            //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                            //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            //    }

                            //}
                            foreach (string sItem in ColEarnings)
                            {
                                var oESDs = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadName == sItem && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                                nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;

                                //nAmount = (oSalaryHead.SalaryHeadType == EnumSalaryHeadType.Basic) ? nAmount : Math.Round(nAmount);
                                nAmount = Math.Round(nAmount);

                                grossEarnings += nAmount;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            nTotalGrossEarning += grossEarnings;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oItem.OTHour * oItem.OTRatePerHour); cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //oItem.GrossAmount + grossEarnings + OTAmount == Gross Earnings
                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossAmount + grossEarnings + Math.Round(oItem.OTHour * oItem.OTRatePerHour); cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            double grossDeductions = 0.0;
                            //if (oDeductions.Count > 0)
                            //{
                            //    foreach (EmployeeSettlementSalaryDetail oDeductionItem in oDeductions)
                            //    {
                            //        var oESDs = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadID == oDeductionItem.SalaryHeadID && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                            //        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;
                            //        nAmount = Math.Round(nAmount);

                            //        grossDeductions += nAmount;
                            //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                            //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            //    }

                            //}
                            foreach (string sItem in ColDeductions)
                            {
                                var oESDs = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadName == sItem && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();


                                //if (oESDItem.SalaryHeadID == 8 || oESDItem.SalaryHeadID == 20 || oESDItem.SalaryHeadID == 25 || oESDItem.SalaryHeadID == 26)
                                //{
                                //    nExceptSalaryHead += oESDItem.Amount;
                                //}

                                nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;

                                //double ndedAmount = (oESDs.Count() > 0) ? oESDs.Where(x => (x.SalaryHeadID == 8 || x.SalaryHeadID == 20 || x.SalaryHeadID == 25 || x.SalaryHeadID == 26)).Sum(x => x.Amount) : 0;

                                nAmount = Math.Round(nAmount);// -Math.Round(ndedAmount);

                                grossDeductions += nAmount;
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            //foreach (string sItem in ColDeductions)
                            //{
                            //    double nESH = GetExceptDeductionSalary(oItem.EmployeeSalaryID);

                            //    grossDeductions = grossDeductions - nESH;
                            //    nExceptSalaryHead = 0;
                            //}
                            nTotalGrossDeduction += grossDeductions;
                            nTotalGrossDeductionGrand += grossDeductions;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = grossDeductions; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //((oItem.GrossAmount + grossEarnings) - grossDeductions) == Net Amount

                            //((oItem.GrossAmount + grossEarnings + Math.Floor(oItem.OTHour * oItem.OTRatePerHour)) - grossDeductions)
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            rowIndex++;
                        }
                        colIndex = 2;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Present Salary
                        double nGrossAmount = oTempEmpSs.Sum(x => x.GrossAmount);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGrossAmount); cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        foreach (string sItem in ColEarnings)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        }
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nTotalGrossEarning = 0;

                        foreach (string sItem in ColDeductions)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nTotalGrossDeduction); cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nTotalGrossDeduction = 0;

                        double nNetAmount = oTempEmpSs.Sum(x => x.NetAmount);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nNetAmount); cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex += 1;
                        //PrintSalarySheet(oTempEmpSs);
                        oTempEmployeeSettlementSalarys.RemoveAll(x => x.DepartmentName == oTempEmpSs[0].DepartmentName);
                    }
                    oEmployeeSettlementSalarys.RemoveAll(x => x.LocationName == sLocationName);
                }
                rowIndex += 1;


                colIndex = 2;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Present Salary
                double nGrossAmountG = oSummaryEmployeeSettlementSalarys.Sum(x => x.GrossAmount);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nGrossAmountG); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                foreach (string sItem in ColEarnings)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nTotalGrossEarning = 0;

                foreach (string sItem in ColDeductions)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nTotalGrossDeductionGrand); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nTotalGrossDeduction = 0;

                double nNetAmountG = oSummaryEmployeeSettlementSalarys.Sum(x => x.NetAmount);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(nNetAmountG); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex += 1;
                
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SETTLEMENT_SUMMARY.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private double GetExceptDeductionSalary(int EmployeeSalaryID)
        {
            foreach (EmployeeSettlementSalaryDetail oESDItem in _oTEmployeeSettlementSalaryDetails)
            {
                if (oESDItem.SalaryHeadType == 3 && oESDItem.EmployeeSalaryID == EmployeeSalaryID)
                {
                    if (oESDItem.SalaryHeadID == 8 || oESDItem.SalaryHeadID == 20 || oESDItem.SalaryHeadID == 25 || oESDItem.SalaryHeadID == 26)
                    {
                        nExceptSalaryHead += oESDItem.Amount;
                    }
                }
            }
            return nExceptSalaryHead;
        }
        #endregion



        #region FinalSettlementSalary PDF

        public ActionResult FinalSettlementSalaryPDF(string sParam)
        {
            DateTime dtStartDate = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(sParam.Split('~')[1]);
            Int16 nSettlementType = Convert.ToInt16(sParam.Split('~')[2]);
            string sDepartmentIds = sParam.Split('~')[3];
            string sDesignationIds =sParam.Split('~')[4];
            Int16 nClearanceStatus =  Convert.ToInt16(sParam.Split('~')[5]);
            Int16 nApproveStatus = Convert.ToInt16(sParam.Split('~')[6]);

            string sBUIDs = sParam.Split('~')[7];
            string sLocationIds = sParam.Split('~')[8];
            string sEmpIDs = sParam.Split('~')[9];
            int nGroupDept = Convert.ToInt16(sParam.Split('~')[10]);

            List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
            List<EmployeeSettlementSalary> oEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
            List<EmployeeSettlementSalaryDetail> oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            string sSql = "";

            sSql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID<>0 AND EffectDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (!string.IsNullOrEmpty(sBUIDs))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE BusinessUnitID IN(" + sBUIDs + "))";
            }
            if (!string.IsNullOrEmpty(sLocationIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE LocationID IN(" + sLocationIds + "))";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DesignationID IN(" + sDesignationIds + "))";
            }

            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN("+sEmpIDs+")";
            }


            if (nSettlementType > 0)
            {
                sSql = sSql + " AND SettlementType=" + nSettlementType;
            }

            if (nApproveStatus == 1)
            {
                sSql = sSql + " AND ApproveBy>0";
            }
            if (nApproveStatus == 2)
            {
                sSql = sSql + " AND ApproveBy<=0";
            }
            if (nClearanceStatus > 0)
            {
                sSql = sSql + " AND EmployeeSettlementID IN(SELECT EmployeeSettlementID  FROM EmployeeSettlementClearance WHERE CurrentStatus=" + nClearanceStatus + ")";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }

            oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oEmployeeSettlements.Count > 0)
            {
                oEmployeeSettlements[0].ErrorMessage = dtStartDate.ToString("dd MMM yyyy") + "," + dtEndDate.ToString("dd MMM yyyy");
            }
            //End EmployeeSettlement


            string tEmpIDs = "";
            if (oEmployeeSettlements.Count > 0)
            {
                foreach (EmployeeSettlement oItem in oEmployeeSettlements)
                {
                    tEmpIDs += oItem.EmployeeID + ",";
                }
                tEmpIDs = tEmpIDs.Remove(tEmpIDs.Length - 1, 1);
            }

            List<EmployeeLeaveOnAttendance> oELOnAttendances = new List<EmployeeLeaveOnAttendance>();
            oELOnAttendances = EmployeeLeaveOnAttendance.Gets(tEmpIDs, Convert.ToDateTime(dtStartDate), Convert.ToDateTime(dtEndDate), ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Employee> oEmployees = new List<Employee>();
            sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN ( " + tEmpIDs + ")";
            oEmployees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //EmployeeSettlementSalary
            string sSQL = "";
            if (!string.IsNullOrEmpty(tEmpIDs))
            {
                sSQL += "SELECT * FROM View_EmployeeSettlementSalary WHERE EmployeeID IN(" + tEmpIDs + ")";
                oEmployeeSettlementSalarys = EmployeeSettlementSalary.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            //EmployeeSettlementSalaryDetail
            string EmpIDs = "";
            if (oEmployeeSettlementSalarys.Count > 0)
            {
                string TempEmpIDs = "";
                int nCount = 0;
                oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
                foreach (EmployeeSettlementSalary oItem in oEmployeeSettlementSalarys)
                {
                    TempEmpIDs += oItem.EmployeeID + ",";
                    EmpIDs += oItem.EmployeeID + ",";
                    nCount++;

                    if (nCount % 100 == 0 || nCount == oEmployeeSettlementSalarys.Count)
                    {
                        TempEmpIDs = TempEmpIDs.Remove(TempEmpIDs.Length - 1, 1);
                        sSql = "";
                        sSql = "SELECT * FROM View_EmployeeSettlementSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSettlementSalary WHERE EmployeeID IN (" + TempEmpIDs + ")) ORDER BY SalaryHeadID";
                        List<EmployeeSettlementSalaryDetail> oTempEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
                        oTempEmployeeSettlementSalaryDetails = EmployeeSettlementSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        oEmployeeSettlementSalaryDetails.AddRange(oTempEmployeeSettlementSalaryDetails);
                        TempEmpIDs = "";
                    }
                }
                EmpIDs = EmpIDs.Remove(EmpIDs.Length - 1, 1);
            }
            else
            {
                oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            }

            //List<string> ColEarnings = new List<string>();
            //ColEarnings = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition)).Select(x => x.SalaryHeadName).ToList();
            //ColEarnings = ColEarnings.Distinct().ToList();

            //List<string> ColDeductions = new List<string>();
            //ColDeductions = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction).Select(x => x.SalaryHeadName).ToList();
            //ColDeductions = ColDeductions.Distinct().ToList();
           
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            oLeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead WHERE IsActive <> 0", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            sSql = "SELECT * FROM View_AttendanceDaily WHERE AttendanceDate BETWEEN '" +dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (!string.IsNullOrEmpty(EmpIDs))
            {
                sSql += " AND EmployeeID IN(" + EmpIDs + ")";
            }
            oAttendanceDailys = AttendanceDaily.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //var attList = oAttendanceDailys.GroupBy(x => x.LeaveHeadID).Select(y => y.First()).Distinct();
            //var attList = oAttendanceDailys.Where(x=>x.LeaveHeadID>0).GroupBy(x => new { x.LeaveHeadID}, (key, grp) => new
            //{
            //    LeaveHeadID = key.LeaveHeadID,
            //    result = grp,

            //}).OrderBy(x => x.LeaveHeadID).ToList();
            //List<string> ColLeaveHeads = new List<string>();
            //foreach (var col in attList)
            //{
            //    foreach (LeaveHead lh in oLeaveHeads)
            //    {
            //        if (col.LeaveHeadID == lh.LeaveHeadID)
            //        {
            //            ColLeaveHeads.Add(lh.ShortName);
            //        }
            //    }
            //}
            sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetPropertys = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptFinalSettlementSalary oReport = new rptFinalSettlementSalary();
            byte[] abytes = oReport.PrepareReport(oEmployeeSettlements, oEmployeeSettlementSalarys, oEmployeeSettlementSalaryDetails, oLeaveHeads, oAttendanceDailys, oCompany, oEmployees, oELOnAttendances, oSalarySheetPropertys, nGroupDept);
            return File(abytes, "application/pdf");
        }

        public ActionResult FinalSettlementSalaryPDFAMG(string sParam)
        {
            DateTime dtStartDate = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(sParam.Split('~')[1]);
            Int16 nSettlementType = Convert.ToInt16(sParam.Split('~')[2]);
            string sDepartmentIds = sParam.Split('~')[3];
            string sDesignationIds = sParam.Split('~')[4];
            Int16 nClearanceStatus = Convert.ToInt16(sParam.Split('~')[5]);
            Int16 nApproveStatus = Convert.ToInt16(sParam.Split('~')[6]);

            string sBUIDs = sParam.Split('~')[7];
            string sLocationIds = sParam.Split('~')[8];
            string sEmpIDs = sParam.Split('~')[9];
            int nGroupDept = Convert.ToInt16(sParam.Split('~')[10]);

            List<EmployeeSettlement> oEmployeeSettlements = new List<EmployeeSettlement>();
            List<EmployeeSettlementSalary> oEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
            List<EmployeeSettlementSalaryDetail> oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            string sSql = "";

            sSql = "SELECT * FROM View_EmployeeSettlement WHERE EmployeeSettlementID<>0 AND EffectDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (!string.IsNullOrEmpty(sBUIDs))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE BusinessUnitID IN(" + sBUIDs + "))";
            }
            if (!string.IsNullOrEmpty(sLocationIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE LocationID IN(" + sLocationIds + "))";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM VIEW_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeOfficial WHERE DesignationID IN(" + sDesignationIds + "))";
            }

            if (!string.IsNullOrEmpty(sEmpIDs))
            {
                sSql += " AND EmployeeID IN(" + sEmpIDs + ")";
            }


            if (nSettlementType > 0)
            {
                sSql = sSql + " AND SettlementType=" + nSettlementType;
            }

            if (nApproveStatus == 1)
            {
                sSql = sSql + " AND ApproveBy>0";
            }
            if (nApproveStatus == 2)
            {
                sSql = sSql + " AND ApproveBy<=0";
            }
            if (nClearanceStatus > 0)
            {
                sSql = sSql + " AND EmployeeSettlementID IN(SELECT EmployeeSettlementID  FROM EmployeeSettlementClearance WHERE CurrentStatus=" + nClearanceStatus + ")";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }

            oEmployeeSettlements = EmployeeSettlement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oEmployeeSettlements.Count > 0)
            {
                oEmployeeSettlements[0].ErrorMessage = dtStartDate.ToString("dd MMM yyyy") + "," + dtEndDate.ToString("dd MMM yyyy");
            }
            //End EmployeeSettlement


            string tEmpIDs = "";
            if (oEmployeeSettlements.Count > 0)
            {
                foreach (EmployeeSettlement oItem in oEmployeeSettlements)
                {
                    tEmpIDs += oItem.EmployeeID + ",";
                }
                tEmpIDs = tEmpIDs.Remove(tEmpIDs.Length - 1, 1);
            }

            List<EmployeeLeaveOnAttendance> oELOnAttendances = new List<EmployeeLeaveOnAttendance>();
            oELOnAttendances = EmployeeLeaveOnAttendance.Gets(tEmpIDs, Convert.ToDateTime(dtStartDate), Convert.ToDateTime(dtEndDate), ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Employee> oEmployees = new List<Employee>();
            sSql = "SELECT * FROM View_Employee WHERE EmployeeID IN ( " + tEmpIDs + ")";
            oEmployees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //EmployeeSettlementSalary
            string sSQL = "";
            if (!string.IsNullOrEmpty(tEmpIDs))
            {
                sSQL += "SELECT * FROM View_EmployeeSettlementSalary WHERE EmployeeID IN(" + tEmpIDs + ")";
                oEmployeeSettlementSalarys = EmployeeSettlementSalary.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            //EmployeeSettlementSalaryDetail
            string EmpIDs = "";
            if (oEmployeeSettlementSalarys.Count > 0)
            {
                string TempEmpIDs = "";
                int nCount = 0;
                oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
                foreach (EmployeeSettlementSalary oItem in oEmployeeSettlementSalarys)
                {
                    TempEmpIDs += oItem.EmployeeID + ",";
                    EmpIDs += oItem.EmployeeID + ",";
                    nCount++;

                    if (nCount % 100 == 0 || nCount == oEmployeeSettlementSalarys.Count)
                    {
                        TempEmpIDs = TempEmpIDs.Remove(TempEmpIDs.Length - 1, 1);
                        sSql = "";
                        sSql = "SELECT * FROM View_EmployeeSettlementSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSettlementSalary WHERE EmployeeID IN (" + TempEmpIDs + ")) ORDER BY SalaryHeadID";
                        List<EmployeeSettlementSalaryDetail> oTempEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
                        oTempEmployeeSettlementSalaryDetails = EmployeeSettlementSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        oEmployeeSettlementSalaryDetails.AddRange(oTempEmployeeSettlementSalaryDetails);
                        TempEmpIDs = "";
                    }
                }
                EmpIDs = EmpIDs.Remove(EmpIDs.Length - 1, 1);
            }
            else
            {
                oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
            }

            //List<string> ColEarnings = new List<string>();
            //ColEarnings = oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition)).Select(x => x.SalaryHeadName).ToList();
            //ColEarnings = ColEarnings.Distinct().ToList();

            //List<string> ColDeductions = new List<string>();
            //ColDeductions = oEmployeeSettlementSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction).Select(x => x.SalaryHeadName).ToList();
            //ColDeductions = ColDeductions.Distinct().ToList();

            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            oLeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead WHERE IsActive <> 0", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            sSql = "SELECT * FROM View_AttendanceDaily WHERE AttendanceDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (!string.IsNullOrEmpty(EmpIDs))
            {
                sSql += " AND EmployeeID IN(" + EmpIDs + ")";
            }
            oAttendanceDailys = AttendanceDaily.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //var attList = oAttendanceDailys.GroupBy(x => x.LeaveHeadID).Select(y => y.First()).Distinct();
            //var attList = oAttendanceDailys.Where(x=>x.LeaveHeadID>0).GroupBy(x => new { x.LeaveHeadID}, (key, grp) => new
            //{
            //    LeaveHeadID = key.LeaveHeadID,
            //    result = grp,

            //}).OrderBy(x => x.LeaveHeadID).ToList();
            //List<string> ColLeaveHeads = new List<string>();
            //foreach (var col in attList)
            //{
            //    foreach (LeaveHead lh in oLeaveHeads)
            //    {
            //        if (col.LeaveHeadID == lh.LeaveHeadID)
            //        {
            //            ColLeaveHeads.Add(lh.ShortName);
            //        }
            //    }
            //}
            sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetPropertys = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            //oEmployeeSettlementSalarys.ForEach(x =>
            //{
            //    x.NetAmount = (x.NetAmount - x.NetAmount);
            //});

            rptFinalSettlementSalaryAMG oReport = new rptFinalSettlementSalaryAMG();
            byte[] abytes = oReport.PrepareReport(oEmployeeSettlements, oEmployeeSettlementSalarys, oEmployeeSettlementSalaryDetails, oLeaveHeads, oAttendanceDailys, oCompany, oEmployees, oELOnAttendances, oSalarySheetPropertys, nGroupDept);
            return File(abytes, "application/pdf");
        }

        #endregion

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
    }
}
