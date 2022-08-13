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
using System.Net.Mail;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using CrystalDecisions.CrystalReports.Engine;

namespace ESimSolFinancial.Controllers
{
    public class LeaveApplicationController : Controller
    {

        #region Declaration
        LeaveApplication _oLeaveApplication;
        private List<LeaveApplication> _oLeaveApplications;
        User _oUser = new User();
        string _sErrorMessage = "";
        #endregion

        #region Application

        public ActionResult ViewLeaveApplications_V1(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLeaveApplications = new List<LeaveApplication>();
            List<LeaveApplication>  oLeaveApplications = new List<LeaveApplication>();
            TempData["Emp"] = ((User)(Session[SessionInfo.CurrentUser])).EmployeeID;

            string sSQL = "";
            oLeaveApplications = new List<LeaveApplication>();
            sSQL = "Select * from View_LeaveApplication WHere LeaveStatus=1 And (RecommendedBy=0)";//RequestForRecommendation=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + " And 
            if ((((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts) || (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.IndividualAccounts))
            {
                sSQL = sSQL + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }
            
            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLeaveApplications.AddRange(oLeaveApplications);

            oLeaveApplications = new List<LeaveApplication>();
            sSQL = "Select * from View_LeaveApplication WHere (RequestForAproval=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + " And ApproveBy=0)";
            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLeaveApplications.AddRange(oLeaveApplications);

            //On Behalf created leave
            oLeaveApplications = new List<LeaveApplication>();
            sSQL = "Select * from View_LeaveApplication WHere IsApprove=0 AND CancelledBy = 0 AND RequestForRecommendation=0 AND DBUserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID;
            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLeaveApplications.AddRange(oLeaveApplications);
            
            List<EmployeeReportingPerson> oERPs = new List<EmployeeReportingPerson>();
            sSQL = "Select * from View_EmployeeReportingPerson Where IsActive=1 And EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            oERPs = EmployeeReportingPerson.Gets((int)((User)(Session[SessionInfo.CurrentUser])).EmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ERPs = oERPs;

            sSQL = "Select * from LeaveHead Where IsActive=1";
            ViewBag.LeaveHeads = LeaveHead.Gets(sSQL,((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ApplicationTypes = Enum.GetValues(typeof(EnumLeaveApplication)).Cast<EnumLeaveApplication>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LeaveTypes = Enum.GetValues(typeof(EnumLeaveType)).Cast<EnumLeaveType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LeaveStatuss = Enum.GetValues(typeof(EnumLeaveStatus)).Cast<EnumLeaveStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            sSQL = "";
            sSQL = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSQL = sSQL + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            ViewBag.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.Approve_Leave_After_Save, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LeaveApplication).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.LeaveApplicationFormats = this.GetsPermittedLeaveApplication(oAuthorizationRoleMappings);
            return View(_oLeaveApplications);
        }

        #region Gets Permitted Leave Application
        private List<EnumObject> GetsPermittedLeaveApplication(List<AuthorizationRoleMapping> oAuthorizationRoleMappings)
        {
            EnumObject oLeaveApplication = new EnumObject();
            List<EnumObject> oLeaveApplications = new List<EnumObject>();


            foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
            {
                if (oItem.OperationType == EnumRoleOperationType.Leave_Application)
                {
                    oLeaveApplication = new EnumObject();
                    oLeaveApplication.id = (int)EnumLeaveApplicationFormat.Leave_Application;
                    oLeaveApplication.Value = EnumObject.jGet(EnumLeaveApplicationFormat.Leave_Application);
                    oLeaveApplications.Add(oLeaveApplication);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Leave_Application_XL)
                {
                    oLeaveApplication = new EnumObject();
                    oLeaveApplication.id = (int)EnumLeaveApplicationFormat.Leave_Application_XL;
                    oLeaveApplication.Value = EnumObject.jGet(EnumLeaveApplicationFormat.Leave_Application_XL);
                    oLeaveApplications.Add(oLeaveApplication);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Alternative_Duty)
                {
                    oLeaveApplication = new EnumObject();
                    oLeaveApplication.id = (int)EnumLeaveApplicationFormat.Alternative_Duty;
                    oLeaveApplication.Value = EnumObject.jGet(EnumLeaveApplicationFormat.Alternative_Duty);
                    oLeaveApplications.Add(oLeaveApplication);
                }
            }

            return oLeaveApplications;
        }
        #endregion
        public ActionResult ViewLeaveApplications_V1_Test(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLeaveApplications = new List<LeaveApplication>();
            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();
            TempData["Emp"] = ((User)(Session[SessionInfo.CurrentUser])).EmployeeID;

            string sSQL = "";
            oLeaveApplications = new List<LeaveApplication>();
            sSQL = "Select * from View_LeaveApplication WHere LeaveStatus=1 And (RequestForRecommendation=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + " And RecommendedBy=0)";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }
            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLeaveApplications.AddRange(oLeaveApplications);

            oLeaveApplications = new List<LeaveApplication>();
            sSQL = "Select * from View_LeaveApplication WHere (RequestForAproval=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + " And ApproveBy=0)";
            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLeaveApplications.AddRange(oLeaveApplications);

            //On Behalf created leave
            oLeaveApplications = new List<LeaveApplication>();
            sSQL = "Select * from View_LeaveApplication WHere IsApprove=0 AND RequestForRecommendation=0 AND DBUserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID;
            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLeaveApplications.AddRange(oLeaveApplications);

            List<EmployeeReportingPerson> oERPs = new List<EmployeeReportingPerson>();
            sSQL = "Select * from View_EmployeeReportingPerson Where IsActive=1 And EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            oERPs = EmployeeReportingPerson.Gets((int)((User)(Session[SessionInfo.CurrentUser])).EmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ERPs = oERPs;

            sSQL = "Select * from LeaveHead Where IsActive=1";
            ViewBag.LeaveHeads = LeaveHead.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ApplicationTypes = Enum.GetValues(typeof(EnumLeaveApplication)).Cast<EnumLeaveApplication>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LeaveTypes = Enum.GetValues(typeof(EnumLeaveType)).Cast<EnumLeaveType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LeaveStatuss = Enum.GetValues(typeof(EnumLeaveStatus)).Cast<EnumLeaveStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            sSQL = "";
            sSQL = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSQL = sSQL + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));

            return View(_oLeaveApplications);
        }

        #endregion

        #region Insert, Update, Delete

        [HttpPost]
        public JsonResult LeaveApplication_Insert(LeaveApplication oLeaveApplication)
        {
            _oLeaveApplication = new LeaveApplication();
            try
            {
                _oLeaveApplication = oLeaveApplication;
                _oLeaveApplication.ApplicationNature = (EnumLeaveApplication)oLeaveApplication.ApplicationNatureInt;
                _oLeaveApplication.LeaveType = (EnumLeaveType)oLeaveApplication.LeaveTypeInt;
                _oLeaveApplication.LeaveStatus = (EnumLeaveStatus)oLeaveApplication.LeaveStatusInt;
                _oLeaveApplication = _oLeaveApplication.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if ((_oLeaveApplication.LeaveApplicationID > 0 && _oLeaveApplication.RequestForRecommendation != 0))
                {
                    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.RequestForRecommendation, "NotApprove");
                    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.ResponsiblePersonID, "NotApprove");
                }
                if ((_oLeaveApplication.RequestForAproval > 0 && _oLeaveApplication.RequestForAproval != 0))
                {
                    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.RequestForAproval, "Approve");
                    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.ResponsiblePersonID, "NotApprove");
                }
            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplication);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LeaveApplication_Update(LeaveApplication oLeaveApplication)
        {
            _oLeaveApplication = new LeaveApplication();
            try
            {
                _oLeaveApplication = oLeaveApplication;
                _oLeaveApplication.ApplicationNature = (EnumLeaveApplication)oLeaveApplication.ApplicationNatureInt;
                _oLeaveApplication.LeaveType = (EnumLeaveType)oLeaveApplication.LeaveTypeInt;
                _oLeaveApplication.LeaveStatus = (EnumLeaveStatus)oLeaveApplication.LeaveStatusInt;
                _oLeaveApplication = _oLeaveApplication.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if ((_oLeaveApplication.LeaveApplicationID > 0 && _oLeaveApplication.RequestForRecommendation != 0))
                {
                    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.RequestForRecommendation, "NotApprove");
                    if (_oLeaveApplication.RequestForRecommendation != _oLeaveApplication.ResponsiblePersonID)
                    {
                        SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.ResponsiblePersonID, "NotApprove");
                    }
                }
                if ((_oLeaveApplication.LeaveApplicationID > 0 && _oLeaveApplication.RequestForAproval != 0))
                {
                    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.RequestForAproval, "Approve");
                    if (_oLeaveApplication.RequestForAproval != _oLeaveApplication.ResponsiblePersonID)
                    {
                        SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.ResponsiblePersonID, "NotApprove");
                    }
                }
            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplication);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLeaveApplication(LeaveApplication oLeaveApplication)
        {
            _oLeaveApplication = new LeaveApplication();
            try
            {
                if (oLeaveApplication.LeaveApplicationID <= 0)
                {
                    throw new Exception("Please select a valid leave application from list.");
                }
                if (oLeaveApplication.ApproveBy > 0)
                {
                    throw new Exception("Unable to delete. Already approved.");
                }

                _oLeaveApplication.LeaveApplicationID = oLeaveApplication.LeaveApplicationID;
                _oLeaveApplication = _oLeaveApplication.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplication.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        #endregion

        #region Recommend, Approve, Cancel

        [HttpPost]
        public JsonResult LeaveApplication_Recommend(LeaveApplication oLeaveApplication)
        {
            _oLeaveApplication = new LeaveApplication();
            try
            {
                _oLeaveApplication = oLeaveApplication;
                _oLeaveApplication.ApplicationNature = (EnumLeaveApplication)oLeaveApplication.ApplicationNatureInt;
                _oLeaveApplication.LeaveType = (EnumLeaveType)oLeaveApplication.LeaveTypeInt;
                _oLeaveApplication.LeaveStatus = EnumLeaveStatus.Recommended;
                _oLeaveApplication.RecommendedBy = Convert.ToInt32(((User)(Session[SessionInfo.CurrentUser])).UserID);

                _oLeaveApplication = _oLeaveApplication.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if ((_oLeaveApplication.LeaveApplicationID > 0 && _oLeaveApplication.RequestForAproval != 0))
                {
                    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.RequestForAproval, "NotApprove");
                    SendMailForLeaveRecommendation(_oLeaveApplication, (int)_oLeaveApplication.EmployeeID, "NotApprove");

                    if (_oLeaveApplication.RequestForAproval != _oLeaveApplication.ResponsiblePersonID)
                    {
                        SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.ResponsiblePersonID, "NotApprove");
                    }
                }
            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplication);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LeaveApplication_Approved(LeaveApplication oLeaveApplication)
        {
            _oLeaveApplication = new LeaveApplication();
            try
            {
                _oLeaveApplication = oLeaveApplication;
                _oLeaveApplication.LeaveStatus = EnumLeaveStatus.Approved;
                _oLeaveApplication = _oLeaveApplication.Approved(((User)(Session[SessionInfo.CurrentUser])).UserID); 
                if ((_oLeaveApplication.LeaveApplicationID > 0 && _oLeaveApplication.ApproveBy != 0))
                {
                    SendMailForLeaveRecommendation(_oLeaveApplication, (int)_oLeaveApplication.EmployeeID, "Approve");
                    //SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.RequestForRecommendation, "Approve");
                    //if (_oLeaveApplication.RequestForAproval != _oLeaveApplication.RequestForRecommendation)
                    //{
                    //    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.RequestForAproval, "Approve");
                    //}
                    //if ((_oLeaveApplication.RequestForAproval != _oLeaveApplication.ResponsiblePersonID) && (_oLeaveApplication.ResponsiblePersonID != _oLeaveApplication.RequestForRecommendation))
                    //{
                    //    SendMailForLeaveRecommendation(_oLeaveApplication, _oLeaveApplication.ResponsiblePersonID, "Approve");
                    //}
                }
            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplication);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LeaveApplication_Cancel(LeaveApplication oLeaveApplication)
        {
            _oLeaveApplication = new LeaveApplication();
            try
            {
                _oLeaveApplication = oLeaveApplication;
                _oLeaveApplication.LeaveStatus = EnumLeaveStatus.Canceled;
                _oLeaveApplication = _oLeaveApplication.Cancel(((User)(Session[SessionInfo.CurrentUser])).UserID);

                SendMailForLeaveCancel(_oLeaveApplication, (int)_oLeaveApplication.EmployeeID);
                if ((_oLeaveApplication.LeaveApplicationID > 0 && _oLeaveApplication.CancelledBy != 0 && _oLeaveApplication.ApproveBy <= 0 && _oLeaveApplication.RecommendedBy <= 1))
                {
                    SendMailForLeaveCancel(_oLeaveApplication, _oLeaveApplication.RequestForRecommendation);
                    if (_oLeaveApplication.ResponsiblePersonID != _oLeaveApplication.RequestForRecommendation)
                    {
                        SendMailForLeaveCancel(_oLeaveApplication, _oLeaveApplication.ResponsiblePersonID);
                    }
                }
            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplication);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Searching

        [HttpPost]
        public JsonResult GetLeaveApp(LeaveApplication oLeaveApplication)
        {
            _oLeaveApplication = new LeaveApplication();
            try
            {
                if (oLeaveApplication.LeaveApplicationID <= 0)
                {
                    throw new Exception("Please select a valid leave application from list.");
                }
                _oLeaveApplication = _oLeaveApplication.Get(oLeaveApplication.LeaveApplicationID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (_oLeaveApplication.LeaveApplicationID > 0)
                {
                    string sSQL = "Select * from View_EmployeeLeaveLedger Where  EmployeeID=" + _oLeaveApplication.EmployeeID + "  And ACSID =(Select ACSID from EmployeeLeaveLedger Where EmpLeaveLedgerID=" + _oLeaveApplication.EmpLeaveLedgerID + ")";
                    _oLeaveApplication.EmployeeLeaveLedgers = EmployeeLeaveLedger.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplication);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsLeaveApp(LeaveApplication oLeaveApplication)
        {
            _oLeaveApplications = new List<LeaveApplication>();
            try
            {
                bool IsDateSearch = Convert.ToBoolean(oLeaveApplication.Params.Split('~')[0]);
                DateTime dtDateFrom = Convert.ToDateTime(oLeaveApplication.Params.Split('~')[1]);
                DateTime dtDateTo = Convert.ToDateTime(oLeaveApplication.Params.Split('~')[2]);
                int nEmployeeID = Convert.ToInt32(oLeaveApplication.Params.Split('~')[3]);

                string sSQL = "SELECT * FROM View_LeaveApplication WHERE LeaveApplicationID<>0 ";
                if (nEmployeeID > 0) { sSQL += " And EmployeeID=" + nEmployeeID + ""; }
                if (!IsDateSearch) { sSQL += " And LeaveStatus NOT IN (1,3)"; }
                else { sSQL += " And convert(date,StartDateTime)>='" + dtDateFrom.ToString("dd-MMM-yyyy") + "' And convert(date,EndDateTime)<='" + dtDateTo.ToString("dd-MMM-yyyy") + "'"; }

                _oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
                _oLeaveApplications.Add(_oLeaveApplication);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplications);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchLeaveApplication(LeaveApplication oLeaveApplication)
        {
            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();
            List<LeaveApplication> oLAs = new List<LeaveApplication>();

            string sBUIDs = oLeaveApplication.Params.Split('~')[0];
            string sLocIDs = oLeaveApplication.Params.Split('~')[1];
            string sDeptIDs = oLeaveApplication.Params.Split('~')[2];
            string sDesgIDs = oLeaveApplication.Params.Split('~')[3];
            string sEmployeeIDs = oLeaveApplication.Params.Split('~')[4];
            DateTime sStartDate = Convert.ToDateTime(oLeaveApplication.Params.Split('~')[5]);
            DateTime sEndDate = Convert.ToDateTime(oLeaveApplication.Params.Split('~')[6]);
            int nApplicationNature = Convert.ToInt32(oLeaveApplication.Params.Split('~')[7]);
            int nLeaveHeadId = Convert.ToInt32(oLeaveApplication.Params.Split('~')[8]);
            int nLeaveType = Convert.ToInt32(oLeaveApplication.Params.Split('~')[9]);
            int nLeaveStatus = Convert.ToInt32(oLeaveApplication.Params.Split('~')[10]);
            int nIsPaid = Convert.ToInt16(oLeaveApplication.Params.Split('~')[11]);
            int nIsUnPaid = Convert.ToInt16(oLeaveApplication.Params.Split('~')[12]);
            //bool IsDateSearch = Convert.ToBoolean(oLeaveApplication.Params.Split('~')[13]);
            int nRowLength = Convert.ToInt32(oLeaveApplication.Params.Split('~')[13]);
            int nLoadRecord = Convert.ToInt32(oLeaveApplication.Params.Split('~')[14]);

            //string sSQL = "select * from View_LeaveApplication WHERE LeaveApplicationID<>0";
            string sSQL = "SELECT top(" + nLoadRecord + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeCode) Row,* FROM View_LeaveApplication WHERE LeaveApplicationID <>0 AND ((StartDateTime BETWEEN '" + sStartDate + "' AND '" + sEndDate.AddDays(1) + "') OR (EndDateTime BETWEEN '" + sStartDate + "' AND '" + sEndDate.AddDays(1) + "' ))";

            if (!string.IsNullOrEmpty(sBUIDs))
            {
                sSQL += " AND BusinessUnitID IN(" + sBUIDs + ")";
            }
            if (!string.IsNullOrEmpty(sLocIDs))
            {
                sSQL += " AND LocationID IN(" + sLocIDs + ")";
            }
            if (!string.IsNullOrEmpty(sDeptIDs))
            {
                sSQL += " AND DepartmentID IN(" + sDeptIDs + ")";
            }
            if (!string.IsNullOrEmpty(sDesgIDs))
            {
                sSQL += " AND DesignationID IN(" + sDesgIDs + ")";
            }
            if (!string.IsNullOrEmpty(sEmployeeIDs))
            {
                sSQL += " AND EmployeeID IN(" + sEmployeeIDs + ")";
            }
            //if (nEmployeeID != 0)
            //{
            //    sSQL = sSQL + " AND EmployeeID=" + nEmployeeID;
            //}

            //if (nDepartmentID != 0)
            //{
            //    sSQL = sSQL + " AND EmployeeID IN (SELECT EmployeeID FROM View_EmployeeOfficialAll WHERE DepartmentID=" + nDepartmentID + ")";
            //}

            //if (IsDateSearch)
            //{
            //    //sSQL = sSQL + " AND CONVERT(date,StartDateTime)>='" + sStartDate.ToString("dd MMM yyyy") + "' And CONVERT(date,EndDateTime)<='" + sEndDate.ToString("dd MMM yyyy") + "'";
            //    sSQL = sSQL + " AND ((StartDateTime BETWEEN '" + sStartDate + "' AND '" + sEndDate.AddDays(1) + "') OR (EndDateTime BETWEEN '" + sStartDate + "' AND '" + sEndDate.AddDays(1) + "' ))";
            //}

            if (nApplicationNature != 0)
            {
                sSQL = sSQL + " AND ApplicationNature=" + nApplicationNature;
            }
            if (nLeaveHeadId != 0)
            {
                if (!string.IsNullOrEmpty(sEmployeeIDs))
                {
                    sSQL = sSQL + " AND EmpLeaveLedgerID=" + nLeaveHeadId;
                }
                else
                {
                    sSQL = sSQL + " AND EmpLeaveLedgerID In (Select EmpLeaveLedgerID from EmployeeLeaveLedger Where LeaveID=" + nLeaveHeadId + ")";
                }
            }
            if (nLeaveType != 0)
            {
                sSQL = sSQL + " AND LeaveType=" + nLeaveType;
            }
            if (nLeaveStatus != 0)
            {
                sSQL = sSQL + " AND LeaveStatus=" + nLeaveStatus;
            }

            if (nIsPaid == 1 && nIsUnPaid == 1)
            {
                sSQL = sSQL + " AND IsUnPaid In (0,1)";
            }
            else if (nIsPaid == 1 && nIsUnPaid == 0)
            {
                sSQL = sSQL + " AND IsUnPaid=" + 0;
            }
            else if (nIsPaid == 0 && nIsUnPaid == 1)
            {
                sSQL = sSQL + " AND IsUnPaid=" + nIsUnPaid;
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }

            sSQL = sSQL + ") aa WHERE Row >" + nRowLength + " ORDER BY EmployeeCode, StartDateTime, EndDateTime";
            //sSQL = sSQL + "ORDER BY EmployeeCode, StartDateTime, EndDateTime";
            try
            {
                oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oLeaveApplications = oLeaveApplications.OrderBy(x => x.EmployeeCode).ThenByDescending(x => x.StartDateTime).ThenByDescending(x => x.EndDateTime).ToList();
                //oLeaveApplications = oLeaveApplications.OrderByDescending(x => x.EndDateTime).ThenByDescending(x => x.StartDateTime).ThenByDescending(x => x.EmployeeCode).ToList();
            }

            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
                oLeaveApplications = new List<LeaveApplication>();
                oLeaveApplications.Add(_oLeaveApplication);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLeaveApplications);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExcelLeaveWithEnjoyedBalance(string sTempString)
        {
            List<LeaveLedgerReport> oEmployeeLeaveLedgers = new List<LeaveLedgerReport>();
            List<LeaveLedgerReport> oLeaveLedgerReportDists = new List<LeaveLedgerReport>();


            string sBUIDs = sTempString.Split('~')[0];
            string sLocIDs = sTempString.Split('~')[1];
            string sDeptIDs = sTempString.Split('~')[2];
            string sDesgIDs = sTempString.Split('~')[3];
            string sEmployeeIDs = sTempString.Split('~')[4];
            DateTime sStartDate = Convert.ToDateTime(sTempString.Split('~')[5]);
            DateTime sEndDate = Convert.ToDateTime(sTempString.Split('~')[6]);
            int nApplicationNature = Convert.ToInt32(sTempString.Split('~')[7]);
            int nLeaveHeadId = Convert.ToInt32(sTempString.Split('~')[8]);
            int nLeaveType = Convert.ToInt32(sTempString.Split('~')[9]);
            int nLeaveStatus = Convert.ToInt32(sTempString.Split('~')[10]);
            int nIsPaid = Convert.ToInt16(sTempString.Split('~')[11]);
            int nIsUnPaid = Convert.ToInt16(sTempString.Split('~')[12]);

            oEmployeeLeaveLedgers = LeaveLedgerReport.GetLeaveWithEnjoyBalance(sBUIDs, sLocIDs, sDeptIDs, sDesgIDs, sEmployeeIDs, sStartDate, sEndDate,
                nApplicationNature, nLeaveHeadId, nLeaveType, nLeaveStatus, nIsPaid, nIsUnPaid, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEmployeeLeaveLedgers = oEmployeeLeaveLedgers.Where(x => x.LeaveDuration > 0).OrderBy(x=>x.EmployeeID).ToList();
            oLeaveLedgerReportDists = oEmployeeLeaveLedgers.GroupBy(x => x.LeaveShortName)
                   .Select(grp => grp.First())
                   .ToList();

            int nMaxColumn = 0;
            int nStartCol = 1, nEndCol = 13;
            int colIndex = 1;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;


            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Leave Rerport");
                sheet.Name = "LeaveRerport";

                int n = 1;
                sheet.Column(n++).Width = 6; //SL
                sheet.Column(n++).Width = 15; //CODE
                sheet.Column(n++).Width = 25; //NAME
                sheet.Column(n++).Width = 20; //DSIGNATION
                sheet.Column(n++).Width = 20; //DOJ
                sheet.Column(n++).Width = 20; //Dept
                sheet.Column(n++).Width = 20; //Loc
                sheet.Column(n++).Width = 20; //LeaveType
                sheet.Column(n++).Width = 20; //Application Date
                sheet.Column(n++).Width = 20; //Start Date
                sheet.Column(n++).Width = 20; //End Date
                sheet.Column(n++).Width = 20; //Leave Days
                for (int i = 0; i < oLeaveLedgerReportDists.Count; i++)
                {
                    sheet.Column(n++).Width = 10; //Leave Days
                }
                nMaxColumn = n;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string BUIDs = string.Join(",", oEmployeeLeaveLedgers.Select(p => p.BusinessUnitID).Distinct().ToList());
                if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


                cell = sheet.Cells[rowIndex, 3]; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Font.Bold = true; cell.Value = (oBusinessUnits.Count > 1 || oBusinessUnits.Count <= 0) ? oCompany.Name : oBusinessUnits[0].Name; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 20;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 3];
                cell.Style.Font.Bold = true; cell.Value = "Leave Report"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 20;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Code"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Employee Name"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Designation"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Department"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Location"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "LeaveType"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Application Date"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Start Date"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "End Date"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Value = "Leave Days"; cell.Style.Font.Bold = true; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex += 1;

                int nleaveCount = oLeaveLedgerReportDists.Count;
                if (nleaveCount > 0)
                {
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + nleaveCount - 1]; cell.Value = "Leave Taken"; cell.Style.Font.Bold = true; cell.Merge = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += nleaveCount;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + nleaveCount - 1]; cell.Value = "Leave Balance"; cell.Style.Font.Bold = true; cell.Merge = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    colIndex += nleaveCount;
                }

                rowIndex += 1;

                colIndex = 13;
                foreach (var item in oLeaveLedgerReportDists)
                {
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex]; cell.Value = item.LeaveShortName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;
                }
                foreach (var item in oLeaveLedgerReportDists)
                {
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex]; cell.Value = item.LeaveShortName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;
                }
                rowIndex += 1;
                var deptWise = oEmployeeLeaveLedgers.GroupBy(x => new {x.LocationName, x.DepartmentName }, (key, grp) => new
                {
                    LocationName = key.LocationName,
                    DepartmentName = key.DepartmentName,
                    Result = grp
                }).OrderBy(x=>x.LocationName).ThenBy(x=>x.DepartmentName).ToList();

                int nSL = 0;
                foreach (var data in deptWise)
                {
                    nSL = 0;
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, 1];
                    cell.Style.Font.Bold = true; cell.Value = "Location : " + data.LocationName +", Department : " + data.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex+=1;

                    foreach (var oItem in data.Result)
                    {
                        nSL++;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LeaveTypeSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ApplicationDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.StartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LeaveDuration; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        foreach (var oLLR in oLeaveLedgerReportDists)
                        {
                            double enjoyed = (oLLR.LeaveHeadID == oItem.LeaveHeadID) ? oItem.Full_Enjoyed : 0;
                           // double enjoyed = (oEmployeeLeaveLedgers.Where(x => x.EmployeeID == oItem.EmployeeID && x.LeaveHeadID == oLLR.LeaveHeadID && x.EmpLeaveLedgerID == oLLR.EmpLeaveLedgerID).Any()) ? oEmployeeLeaveLedgers.Where(x => x.EmployeeID == oItem.EmployeeID && x.LeaveHeadID == oLLR.LeaveHeadID && x.EmpLeaveLedgerID == oLLR.EmpLeaveLedgerID).FirstOrDefault().Full_Enjoyed : 0;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = enjoyed; cell.Style.Font.Bold = true; cell.Merge = true; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        foreach (var oLLR in oLeaveLedgerReportDists)
                        {
                            double balance = (oLLR.LeaveHeadID == oItem.LeaveHeadID) ? oItem.Full_Balance : 0;
                            //double balance = (oEmployeeLeaveLedgers.Where(x => x.EmployeeID == oItem.EmployeeID && x.LeaveHeadID == oLLR.LeaveHeadID && x.EmpLeaveLedgerID == oLLR.EmpLeaveLedgerID).Any()) ? oEmployeeLeaveLedgers.Where(x => x.EmployeeID == oItem.EmployeeID && x.LeaveHeadID == oLLR.LeaveHeadID && x.EmpLeaveLedgerID == oLLR.EmpLeaveLedgerID).FirstOrDefault().Full_Balance : 0;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = balance; cell.Style.Font.Bold = true; cell.Merge = true; cell.Style.WrapText = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        rowIndex += 1;
                    }

                }


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LeaveReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        [HttpPost]
        public JsonResult SearchLeaveApplicationByEmpAndDate(LeaveApplication oLeaveApplication)
        {
            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();
            List<LeaveApplication> oLAs = new List<LeaveApplication>();

            int nEmployeeID = Convert.ToInt32( oLeaveApplication.Params.Split('~')[0]);
            string  sStartDate = oLeaveApplication.Params.Split('~')[1];
            string sEndDate = oLeaveApplication.Params.Split('~')[2];
            int nPerson = Convert.ToInt32(oLeaveApplication.Params.Split('~')[3]);
            
            string sSQL = "select * from View_LeaveApplication WHERE LeaveApplicationID<>0";
            if (nEmployeeID > 0 && nPerson<=0)
            {
                sSQL = sSQL + " AND EmployeeID ="+ nEmployeeID;
            }

            if (nPerson > 0)
            {
                if (nPerson == 1)
                {
                    sSQL = sSQL + " AND EmployeeID IN(SELECT EmployeeID  FROM EmployeeReportingPerson WHERE RPID = " + nEmployeeID + ")";
                }

                else if (nPerson == 2)
                {
                    sSQL = sSQL + " AND RequestForRecommendation  = " + nEmployeeID;
                }

                else if (nPerson == 3)
                {
                    sSQL = sSQL + " AND RequestForAproval  = " + nEmployeeID;
                }
            }

            if (sStartDate != "" && sEndDate!="")
            {
                sSQL = sSQL + " AND ((StartDateTime BETWEEN '" + sStartDate + "' AND '" + Convert.ToDateTime(sEndDate).AddDays(1).ToString("dd MMM yyyy") + "') OR (EndDateTime BETWEEN '" + sStartDate + "' AND '" + Convert.ToDateTime(sEndDate).AddDays(1).ToString("dd MMM yyyy") +"' ))";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            }
            sSQL = sSQL + " ORDER BY EmployeeCode, StartDateTime, EndDateTime";
            try
            {
                oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oLeaveApplications = oLeaveApplications.OrderBy(x => x.EmployeeCode).ThenByDescending(x => x.StartDateTime).ThenByDescending(x => x.EndDateTime).ToList();
                //oLeaveApplications = oLeaveApplications.OrderByDescending(x => x.EndDateTime).ThenByDescending(x => x.StartDateTime).ThenByDescending(x => x.EmployeeCode).ToList();
            }

            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
                oLeaveApplications = new List<LeaveApplication>();
                oLeaveApplications.Add(_oLeaveApplication);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLeaveApplications);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region LeaveLedger

        public ActionResult ViewLeaveLedger(int nEmpID, double nts)
        {
            Employee oEmployee = new Employee();
            List<LeaveLedger> oLLs = new List<LeaveLedger>();
            List<AttendanceCalendarSession> oACSs = new List<AttendanceCalendarSession>();

            oEmployee = oEmployee.Get(nEmpID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sSQL = "Select * from View_AttendanceCalendarSession";
            oACSs = AttendanceCalendarSession.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oACSs.Count() > 0)
            {
                int nACSID = (oACSs.Where(x => x.IsActive == true).OrderByDescending(x => x.ACSID).ToList().Count() > 0) ? oACSs.Where(x => x.IsActive == true).OrderByDescending(x => x.ACSID).ToList().ElementAtOrDefault(0).ACSID : 0;
                if (nACSID > 0)
                {
                    oLLs = LeaveLedger.Gets(nEmpID, nACSID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            ViewBag.ACSs = oACSs;
            ViewBag.Employee = oEmployee;
            return View(oLLs);
        }

        [HttpPost]
        public JsonResult GetLeaveLedger(LeaveLedger oLeaveLedger)
        {

            List<LeaveLedger> oLLs = new List<LeaveLedger>();
            try
            {
                int nEmpID = Convert.ToInt32(oLeaveLedger.Params.Split('~')[0]);
                int nACSID = Convert.ToInt32(oLeaveLedger.Params.Split('~')[1]);
                oLLs = LeaveLedger.Gets(nEmpID, nACSID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oLLs.Count() < 0) { throw new Exception("No Leave found for this session."); }
            }
            catch (Exception ex)
            {
                oLLs = new List<LeaveLedger>();
                oLeaveLedger = new LeaveLedger();
                oLeaveLedger.ErrorMessage = ex.Message;
                oLLs.Add(oLeaveLedger);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLLs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Multiple Approve
        [HttpPost]
        public JsonResult MultipleApprove(List<LeaveApplication> oLeaveApplications, double nts)
        {
            _oLeaveApplications = new List<LeaveApplication>();
            try
            {

                _oLeaveApplications = LeaveApplication.MultipleApprove(oLeaveApplications, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach (var oItem in _oLeaveApplications)
                {
                    oItem.Duration = oItem.StartDateTimeInString + "-" + oItem.EndDateTimeInString;
                    if (oItem.IsUnPaid == false)
                    {
                        oItem.LeaveDetails = oItem.ApplicationNatureInString + "-" + oItem.LeaveTypeInString + "-Paid";
                    }
                    else
                    {
                        oItem.LeaveDetails = oItem.ApplicationNatureInString + "-" + oItem.LeaveTypeInString + "-Unpaid";
                    }
                    if (oItem.RecommendedByName == "NULL" || oItem.RecommendedByName == "")
                    {
                        oItem.RecommendedByName = " -- ";
                    }
                    if (oItem.ApproveByName == "NULL" || oItem.ApproveByName == "")
                    {
                        oItem.ApproveByName = " -- ";
                    }
                }
            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplications = new List<LeaveApplication>();
                _oLeaveApplication.ErrorMessage = ex.Message;
                _oLeaveApplications.Add(_oLeaveApplication);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplications);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Multiple Approve

        #region Report
        public ActionResult View_LeaveLedgerReports(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LeaveLedgerReport> oLeaveLedgerReports = new List<LeaveLedgerReport>();
            ViewBag.oACSs = AttendanceCalendarSession.Gets("SELECT * FROM AttendanceCalendarSession ORDER BY Session DESC", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.oLeaveHeads = LeaveHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            return View(oLeaveLedgerReports);
        }
        public ActionResult PrintLeaveLedger(string sParams, double ts)
        {
            string sEmployeeIDs= sParams.Split('~')[0];
            string sDepartmentIds = sParams.Split('~')[1];
            string sDesignationIds = sParams.Split('~')[2];
            int ACSID = Convert.ToInt32(sParams.Split('~')[3]);
            int nLeaveHeadID = Convert.ToInt32(sParams.Split('~')[4]);
            double nBalanceAmount = Convert.ToDouble(sParams.Split('~')[5]);
            int nBalanceType = Convert.ToInt16(sParams.Split('~')[6]);
            string sSessions = sParams.Split('~')[7];
            bool bReportingPerson = Convert.ToBoolean(sParams.Split('~')[8]);
            DateTime dtFrom = Convert.ToDateTime(sParams.Split('~')[9]);
            DateTime dtTo = Convert.ToDateTime(sParams.Split('~')[10]);
            bool bDate = Convert.ToBoolean(sParams.Split('~')[11]);
            bool IsActive = Convert.ToBoolean(sParams.Split('~')[12]);
            bool IsInActive = Convert.ToBoolean(sParams.Split('~')[13]);
            string sBUnit = sParams.Split('~')[14];
            string sLocationID = sParams.Split('~')[15];

            
            LeaveLedgerReport oLeaveLedgerReport = new LeaveLedgerReport();
            oLeaveLedgerReport.LeaveLedgerReports = LeaveLedgerReport.Gets(sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID,((User)(Session[SessionInfo.CurrentUser])).UserID);
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            oLeaveLedgerReport.LeaveHeads = LeaveHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oLeaveLedgerReport.Company = oCompanys.First();
            oLeaveLedgerReport.Company.CompanyLogo = GetCompanyLogo(oLeaveLedgerReport.Company);

            string BUIDs = string.Join(",", oLeaveLedgerReport.LeaveLedgerReports.Where(x => x.EmployeeID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oLeaveLedgerReport.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


            //if (sBUnit != "")
            //{
            //    oLeaveLedgerReport.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnit + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //}

            if (ACSID<=0)
            {
                string sSql = "SELECT TOP(1)* FROM AttendanceCalendarSession WHERE IsActive=1 ORDER BY Session DESC";
                List<AttendanceCalendarSession> oACSs = new List<AttendanceCalendarSession>();
                oACSs = AttendanceCalendarSession.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if(oACSs.Count>0)
                {
                    sSessions = oACSs[0].Session;
                }
            }
            oLeaveLedgerReport.ErrorMessage = nLeaveHeadID + "~" + sSessions + "~" + dtFrom.ToString("dd MMM yyyy") + "~" + dtTo.ToString("dd MMM yyyy") + "~" + bDate;

            rptLeaveLedger oReport = new rptLeaveLedger();
            byte[] abytes = oReport.PrepareReport(oLeaveLedgerReport);
            return File(abytes, "application/pdf");

        }
        public ActionResult LeaveReportPDF(string sParams, double ts)
        {
            string sEmployeeIDs = sParams.Split('~')[0];
            string sDepartmentIds = sParams.Split('~')[1];
            string sDesignationIds = sParams.Split('~')[2];
            int ACSID = Convert.ToInt32(sParams.Split('~')[3]);
            int nLeaveHeadID = Convert.ToInt32(sParams.Split('~')[4]);
            double nBalanceAmount = Convert.ToDouble(sParams.Split('~')[5]);
            int nBalanceType = Convert.ToInt16(sParams.Split('~')[6]);
            string sSessions = sParams.Split('~')[7];
            bool bReportingPerson = Convert.ToBoolean(sParams.Split('~')[8]);
            DateTime dtFrom = Convert.ToDateTime(sParams.Split('~')[9]);
            DateTime dtTo = Convert.ToDateTime(sParams.Split('~')[10]);
            bool bDate = Convert.ToBoolean(sParams.Split('~')[11]);
            bool IsActive = Convert.ToBoolean(sParams.Split('~')[12]);
            bool IsInActive = Convert.ToBoolean(sParams.Split('~')[13]);
            string sBUnit = sParams.Split('~')[14];
            string sLocationID = sParams.Split('~')[15];

            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();
            string sSQL = GetSQL(sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, sSessions, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID);
            oLeaveApplications = LeaveApplication.GetsEmployeeLeaveLedger(sSQL, ACSID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "LeaveLedgerRegister.rpt"));
            rd.Database.Tables["LeaveApplication"].SetDataSource(oLeaveApplications);
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

        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #region Print Leave application form
        public ActionResult PrintLeaveApplicationForm_MAMIYA(int nEmpID, int nLeaveApplicationID, double ts)
        {
            Employee oEmployee = new Employee();
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            oEmployee.EmployeeOfficial = oEmployeeOfficial.Get(nEmpID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sSql_LeaveHead = "SELECT * FROM LeaveHead ";
            oEmployee.LeaveHeads = LeaveHead.Gets(sSql_LeaveHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sSql_LeaveApplication = "SELECT * FROM View_LeaveApplication WHERE LeaveApplicationID=" + nLeaveApplicationID;
            oEmployee.LeaveApplication = LeaveApplication.Get(sSql_LeaveApplication, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployee.Company = oCompanys.First();
            rptLeaveApplicationForm_MAMIYA oReport = new rptLeaveApplicationForm_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oEmployee);
            return File(abytes, "application/pdf");
        }
        #endregion Print Leave application form

        #endregion Report

        #region Adjustment
        [HttpPost]
        public JsonResult LeaveAdjustment(LeaveApplication oLeaveApplication)
        {
            int nLeaveApplicationID = Convert.ToInt32(oLeaveApplication.Params.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(oLeaveApplication.Params.Split('~')[1]);

            _oLeaveApplication = new LeaveApplication();
            try
            {
                _oLeaveApplication = _oLeaveApplication.LeaveAdjustment(nLeaveApplicationID,dtEndDate,((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oLeaveApplication = new LeaveApplication();
                _oLeaveApplication.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveApplication);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Adjustment

        #region Leave Application Mail

        private void SendMailForLeaveRecommendation(LeaveApplication oLeaveApplication,int nEmployeeID, string sApproveOrNot)
        {
            Employee oEmployee = new Employee();
            oEmployee = oEmployee.Get(nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (Global.IsValidMail(oEmployee.Email))
            {
                List<string> emialTos = new List<string>();
                emialTos.Add(oEmployee.Email);

                string subject = "Leave Application Sent from " + oLeaveApplication.EmployeeName;
                string message = "";
                if (sApproveOrNot == "Approve")
                {
                    message = "Your Leave is approved which is " + oLeaveApplication.LeaveTypeInString + " leave from " + oLeaveApplication.StartDateTimeInString + " to " + oLeaveApplication.EndDateTimeInString + "."
                                 + " Which is a " + oLeaveApplication.LeaveHeadName + ".";
                }
                else
                {
                    message = "I am applying for a " + oLeaveApplication.LeaveTypeInString + " leave from " + oLeaveApplication.StartDateTimeInString + " to " + oLeaveApplication.EndDateTimeInString + "."
                                 + " Which should be a " + oLeaveApplication.LeaveHeadName + ".";
                }

                string bodyInfo = "";

                List<User> oUsers=new List<User>();
                string sSQL = "Select * from View_User Where EmployeeID=" + oEmployee.EmployeeID;
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oUsers.Any() && oUsers.FirstOrDefault().UserID > 0)
                {
                   
                    var oUser= oUsers.FirstOrDefault();
                    if (sApproveOrNot == "Approve")
                    {
                        bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                                 "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                                 oEmployee.Name, message, oLeaveApplication.ApproveByName, oLeaveApplication.ApprovedByDesignation, oLeaveApplication.ApprovedByDepartment,
                            //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                                 Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                    }
                    else
                    {
                        bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                                 "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                                 oEmployee.Name, message, oLeaveApplication.EmployeeNameCode, oLeaveApplication.DesignationName, oLeaveApplication.DepartmentName,
                                                 //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                                 Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                    }
                }
                else
                {
                    if (sApproveOrNot == "Approve")
                    {
                        bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                                 "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                                 oEmployee.Name, message, oLeaveApplication.ApproveByName, oLeaveApplication.ApprovedByDesignation, oLeaveApplication.ApprovedByDepartment,
                            //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                                 Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                    }
                    else
                    {
                        bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                              "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:20px;'>Mail sent at time {5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                              oEmployee.Name, message, oLeaveApplication.EmployeeNameCode, oLeaveApplication.DesignationName, oLeaveApplication.DepartmentName
                                              , Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                    }
                }

                #region Email Credential
                EmailConfig oEmailConfig = new EmailConfig();
                oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                #endregion

                Global.MailSend(subject, bodyInfo, emialTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
            }
        }


        private void SendMailForLeaveCancel(LeaveApplication oLeaveApplication, int nEmployeeID)
        {
            Employee oEmployee = new Employee();
            Employee oEmployeeUser = new Employee();

            User oUserEmployee = new User();
            oUserEmployee = ((User)(Session[SessionInfo.CurrentUser]));

            oEmployee = oEmployee.Get(nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeUser = oEmployee.Get((int)oUserEmployee.EmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            
            if (Global.IsValidMail(oEmployee.Email))
            {
                List<string> emialTos = new List<string>();
                emialTos.Add(oEmployee.Email);

                string subject = "Leave Application Sent from " + oEmployeeUser.Name;
                string message = "Your leave application (" + oLeaveApplication.LeaveHeadName + ") from " + oLeaveApplication.StartDateTimeInString + " to " + oLeaveApplication.EndDateTimeInString + " has been canceled";
                
                string bodyInfo = "";

                List<User> oUsers = new List<User>();
                string sSQL = "Select * from View_User Where EmployeeID=" + oEmployee.EmployeeID;
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oUsers.Any() && oUsers.FirstOrDefault().UserID > 0)
                {

                    var oUser = oUsers.FirstOrDefault();
                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                             "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                             oEmployee.Name, message, oEmployeeUser.Name + "[" + oEmployeeUser.Code + "]", oEmployeeUser.DesignationName, oEmployeeUser.DepartmentName,
                        //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                             Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                }
                else
                {
                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                              "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:20px;'>Mail sent at time {5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                              oEmployee.Name, message, oEmployeeUser.Name + "[" + oEmployeeUser.Code + "]", oEmployeeUser.DesignationName, oEmployeeUser.DepartmentName
                                              , Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                }

                #region Email Credential
                EmailConfig oEmailConfig = new EmailConfig();
                oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                #endregion

                Global.MailSend(subject, bodyInfo, emialTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
            }
        }


        
        #endregion

        #region Employee LeaveReport

        public void PrintLeaveReport(string sParams, double ts)
        {
            string sEmployeeIDs = sParams.Split('~')[0];
            string sDepartmentIds = sParams.Split('~')[1];
            string sDesignationIds = sParams.Split('~')[2];
            int ACSID = Convert.ToInt32(sParams.Split('~')[3]);
            int nLeaveHeadID = Convert.ToInt32(sParams.Split('~')[4]);
            double nBalanceAmount = Convert.ToDouble(sParams.Split('~')[5]);
            int nBalanceType = Convert.ToInt16(sParams.Split('~')[6]);
            string sSessions = sParams.Split('~')[7];
            bool bReportingPerson = Convert.ToBoolean(sParams.Split('~')[8]);
            DateTime dtFrom = Convert.ToDateTime(sParams.Split('~')[9]);
            DateTime dtTo = Convert.ToDateTime(sParams.Split('~')[10]);
            bool bDate = Convert.ToBoolean(sParams.Split('~')[11]);
            bool IsActive = Convert.ToBoolean(sParams.Split('~')[12]);
            bool IsInActive = Convert.ToBoolean(sParams.Split('~')[13]);
            string sBUnit = sParams.Split('~')[14];
            string sLocationID = sParams.Split('~')[15];

            List<LeaveLedgerReport> oLeaveLedgers = new List<LeaveLedgerReport>();
            List<LeaveLedgerReport> oLeaveHeads = new List<LeaveLedgerReport>();

            LeaveLedgerReport oLeaveLed = new LeaveLedgerReport();

            oLeaveLedgers = LeaveLedgerReport.Gets(sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oLeaveHeads = oLeaveLedgers.GroupBy(x => x.LeaveHeadID).Select(x => x.FirstOrDefault()).ToList();

            List<AttendanceCalendarSession> oAttendanceCalendarSessions = new List<AttendanceCalendarSession>();
            string sSql = "";
            if (ACSID > 0)
            { sSql = "SELECT * FROM AttendanceCalendarSession WHERE ACSID=" + ACSID; }
            else { sSql = "SELECT TOP(1)* FROM AttendanceCalendarSession WHERE IsActive=1 ORDER BY ACSID DESC";}
            oAttendanceCalendarSessions = AttendanceCalendarSession.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string BUIDs = string.Join(",", oLeaveLedgers.Where(x => x.EmployeeID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { oLeaveLed.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


            int nStartCol = 2, nEndCol = 2;
            int rowIndex = 3;
            int nMaxCol = 0;

            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Leave report");
                sheet.Name = "Leave Report";

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 15; //Code 
                sheet.Column(4).Width = 35; //Employee Name
                //sheet.Column(5).Width = 25; //Dept
                sheet.Column(5).Width = 25; //Designation
                sheet.Column(6).Width = 25; //Designation

                int i = 0;
                for (i = 7; i <= (2 * oLeaveHeads.Count()) + 4; i++)
                {
                    sheet.Column(i).Width = 10; //Leave Short Name
                }
                nEndCol = i-1;
                nMaxCol = i - 1;
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Merge = true; cell.Value = oLeaveLed.BusinessUnits.Count == 1 ? oLeaveLed.BusinessUnits[0].Name : oCompany.Name; cell.Merge = true; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Merge = true; cell.Value = "Leave Report " + (bDate ? ("@ " + dtFrom.ToString("dd MMM yyyy") + " - " + dtTo.ToString("dd MMM yyyy")) : "") + (oAttendanceCalendarSessions.Count > 0 ? (" @ Session-" + oAttendanceCalendarSessions[0].Session) : ""); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02

                oLeaveLedgers = oLeaveLedgers.OrderBy(x => x.DepartmentName).ToList();
                List<LeaveLedgerReport> oTempLLs = new List<LeaveLedgerReport>();
                oLeaveLedgers.ForEach(x => oTempLLs.Add(x));

                while (oTempLLs.Count > 0)
                {
                    List<LeaveLedgerReport> oTempLeaveLedgers = new List<LeaveLedgerReport>();
                    List<LeaveLedgerReport> oLeaveLedgerDistinctEmps = new List<LeaveLedgerReport>();
                    oTempLeaveLedgers = oTempLLs.Where(x => x.DepartmentName == oTempLLs[0].DepartmentName).ToList();
                    oLeaveLedgerDistinctEmps = oTempLeaveLedgers.GroupBy(x => x.EmployeeID).Select(x => x.FirstOrDefault()).ToList();

                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nEndCol]; cell.Merge = true; cell.Value = oTempLLs[0].DepartmentName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex = rowIndex + 1;

                    nEndCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol++, rowIndex + 1, nEndCol++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nStartCol++, rowIndex + 1, nEndCol++]; cell.Merge = true; cell.Value = "Code"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nStartCol++, rowIndex + 1, nEndCol++]; cell.Merge = true; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[rowIndex, nStartCol++, rowIndex + 1, nEndCol++]; cell.Merge = true; cell.Value = "Department"; cell.Style.Font.Bold = true;
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nStartCol++, rowIndex + 1, nEndCol++]; cell.Merge = true; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nStartCol++, rowIndex + 1, nEndCol++]; cell.Merge = true; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nEndCol = nEndCol + oLeaveHeads.Count() - 1;
                    cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = "Leave Taken"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nStartCol = nEndCol;
                    nEndCol = nEndCol + oLeaveHeads.Count() - 1;
                    cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = "Current Balance"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;

                    nStartCol = 7;
                    nEndCol = 7;
                    foreach (LeaveLedgerReport oItem in oLeaveHeads)
                    {
                        cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = oItem.LeaveShortName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    foreach (LeaveLedgerReport oItem in oLeaveHeads)
                    {
                        cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = oItem.LeaveShortName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    #region Table Body
                    int nSL = 0;
                    rowIndex++;

                    foreach (LeaveLedgerReport oLeaveLedger in oLeaveLedgerDistinctEmps)
                    {
                        nStartCol = 2;
                        nEndCol = 2;
                        nSL++;

                        cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = nSL; cell.Style.Numberformat.Format = "0"; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                       //Code
                        int Code = 0;
                        if (int.TryParse(oLeaveLedger.EmployeeCode, out Code))
                        {
                            cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = Code; 
                            cell.Style.Numberformat.Format = "0";
                        }
                        else
                        {
                            cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = oLeaveLedger.EmployeeCode; 

                        }
                        
                        cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //EmployeeName
                        cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = oLeaveLedger.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = oLeaveLedger.DepartmentName; cell.Style.Font.Bold = false;
                        //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = oLeaveLedger.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = oLeaveLedger.JoiningDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        foreach (LeaveLedgerReport oItem in oLeaveHeads)
                        {
                            double nLeavDays = 0;
                            if (oLeaveLedgers.Where(x => x.EmployeeID == oLeaveLedger.EmployeeID && x.LeaveHeadID == oItem.LeaveHeadID).Any())
                            {
                                nLeavDays = Convert.ToDouble(oLeaveLedgers.Where(x => x.EmployeeID == oLeaveLedger.EmployeeID && x.LeaveHeadID == oItem.LeaveHeadID).FirstOrDefault().Enjoyed);
                            }
                            cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = nLeavDays; cell.Style.Numberformat.Format = "#0.00"; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        foreach (LeaveLedgerReport oItem in oLeaveHeads)
                        {
                            double nEnjoyedDays = 0;
                            if (oLeaveLedgers.Where(x => x.EmployeeID == oLeaveLedger.EmployeeID && x.LeaveHeadID == oItem.LeaveHeadID).Any())
                            {
                                nEnjoyedDays = Convert.ToDouble(oLeaveLedgers.Where(x => x.EmployeeID == oLeaveLedger.EmployeeID && x.LeaveHeadID == oItem.LeaveHeadID).FirstOrDefault().Balance);
                            }

                            cell = sheet.Cells[rowIndex, nStartCol++, rowIndex, nEndCol++]; cell.Merge = true; cell.Value = nEnjoyedDays; cell.Style.Numberformat.Format = "#0.00"; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        rowIndex++;
                    }
                    oTempLLs.RemoveAll(x => x.DepartmentName == oTempLeaveLedgers[0].DepartmentName);
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LeaveReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        private string GetSQL(string sEmployeeIDs,string sDepartmentIds,string sDesignationIds, int ACSID ,int nLeaveHeadID,  double nBalanceAmount, int nBalanceType , string sSessions,bool bReportingPerson,  DateTime dtFrom , DateTime dtTo, bool bDate ,bool IsActive, bool IsInActive ,string sBUnit,string sLocationID)
        {
            //string sTemp = "SELECT * FROM View_LeaveApplication ";
            string sTemp = "";
            string sTemp1 = "";

            #region EmployeeID
            if (!string.IsNullOrEmpty(sEmployeeIDs))
            {
                Global.TagSQL(ref sTemp1);
                sTemp1 = sTemp1 + " EmployeeID IN (" + sEmployeeIDs + ") ";
            }
            #endregion

            #region LocationID
            if (!string.IsNullOrEmpty(sLocationID))
            {
                Global.TagSQL(ref sTemp1);
                sTemp1 = sTemp1 + " LocationID IN (" + sLocationID + ") ";
            }
            #endregion

            #region BusinessUnitID
            if (!string.IsNullOrEmpty(sBUnit))
            {
                Global.TagSQL(ref sTemp1);
                sTemp1 = sTemp1 + " BusinessUnitID IN (" + sBUnit + ") ";
            }
            #endregion

            #region DepartmentID
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                Global.TagSQL(ref sTemp1);
                sTemp1 = sTemp1 + " DepartmentID IN (" + sDepartmentIds + ") ";
            }
            #endregion

            #region DesignationID
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                Global.TagSQL(ref sTemp1);
                sTemp1 = sTemp1 + " DesignationID IN (" + sDesignationIds + ") ";
            }
            #endregion

            #region Leave Head
            if (nLeaveHeadID > 0)
            {
                Global.TagSQL(ref sTemp1);
                sTemp1 = sTemp1 + " LeaveHeadID = " + nLeaveHeadID;
            }
            #endregion

            #region Date
            if (bDate)
            {
                DateObject.CompareDateQuery(ref sTemp1, "StartDateTime", 5, dtFrom, dtTo);
                DateObject.CompareDateQuery(ref sTemp1, "EndDateTime", 5, dtFrom, dtTo);
            }
            #endregion

            return sTemp + sTemp1 + "AND IsActive = 1 AND IsApprove = 1  AND CancelledBy IS NULL";
        }
        public void PrintLeaveReportNew(string sParams, double ts)
        {
            string sEmployeeIDs = sParams.Split('~')[0];
            string sDepartmentIds = sParams.Split('~')[1];
            string sDesignationIds = sParams.Split('~')[2];
            int ACSID = Convert.ToInt32(sParams.Split('~')[3]);
            int nLeaveHeadID = Convert.ToInt32(sParams.Split('~')[4]);
            double nBalanceAmount = Convert.ToDouble(sParams.Split('~')[5]);
            int nBalanceType = Convert.ToInt16(sParams.Split('~')[6]);
            string sSessions = sParams.Split('~')[7];
            bool bReportingPerson = Convert.ToBoolean(sParams.Split('~')[8]);
            DateTime dtFrom = Convert.ToDateTime(sParams.Split('~')[9]);
            DateTime dtTo = Convert.ToDateTime(sParams.Split('~')[10]);
            bool bDate = Convert.ToBoolean(sParams.Split('~')[11]);
            bool IsActive = Convert.ToBoolean(sParams.Split('~')[12]);
            bool IsInActive = Convert.ToBoolean(sParams.Split('~')[13]);
            string sBUnit = sParams.Split('~')[14];
            string sLocationID = sParams.Split('~')[15];

            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();
            string sSQL = GetSQL(sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, sSessions, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID);

            oLeaveApplications = LeaveApplication.GetsEmployeeLeaveLedger(sSQL, ACSID,((User)(Session[SessionInfo.CurrentUser])).UserID);


            var oCompany = new Company();
            var results = Company.Gets("Select * from View_Company", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (results.Any())
                oCompany = results.First();


            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Leave Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Excel Sheet");
                sheet.Name = "Salary Sheet.";
                int nColNumber = 2;

                #region Col
                sheet.Column(nColNumber++).Width = 6;   // SL
                sheet.Column(nColNumber++).Width = 20;  //Code
                sheet.Column(nColNumber++).Width = 35;  //name
                sheet.Column(nColNumber++).Width = 30;  //desi
                sheet.Column(nColNumber++).Width = 10;  //gen
                sheet.Column(nColNumber++).Width = 20;  //doj
                sheet.Column(nColNumber++).Width = 18;  //leave type
                sheet.Column(nColNumber++).Width = 15;  //app date
                sheet.Column(nColNumber++).Width = 15;  //s d
                sheet.Column(nColNumber++).Width = 15;  //e d
                sheet.Column(nColNumber++).Width = 12;  //ls
                sheet.Column(nColNumber++).Width = 12;  //ld d
                sheet.Column(nColNumber++).Width = 12;  //balance
                sheet.Column(nColNumber++).Width = 15;  //loc
                sheet.Column(nColNumber++).Width = 40;  //bu
                #endregion

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 16]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 16; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, 16]; cell.Merge = true; cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, 16]; cell.Merge = true; cell.Value = oCompany.Phone; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, 16]; cell.Merge = true; cell.Value = "Leave Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex++;

                cell = sheet.Cells[rowIndex, 2, rowIndex, 16]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex++;
                #endregion

                #region Header
                nColNumber = 2;
                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "#SL"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Employee Code"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Employee Name"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Gender"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Leave Type"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "App Date"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Leave Start"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Leave End"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Leave Days"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "LDays"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = "Business Unit"; cell.Style.Font.Bold = true;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                rowIndex++;
                #endregion

                #region data
                int nCount = 1;
                foreach (LeaveApplication oItem in oLeaveApplications)
                {
                    nColNumber = 2;
                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = nCount++.ToString(); cell.Style.Font.Bold = false; 
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;    

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.EmployeeCode; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.EmployeeName; cell.Style.WrapText = true; cell.Style.Font.Bold = false;   
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.DesignationName; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.Gender; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.JoiningDate.ToString("dd/MM/yyyy"); cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin; 
                    
                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.LeaveHeadName; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.DBServerDateTime.ToString("dd/MM/yyyy"); cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.StartDateTime.ToString("dd/MM/yyyy"); cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.EndDateTime.ToString("dd/MM/yyyy"); cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.LeaveDuration; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.LDays; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.LDays - oItem.LeaveDuration; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin; 

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.LocationName; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, nColNumber++]; cell.Value = oItem.BusinessUnitName; cell.Style.WrapText = true; cell.Style.Font.Bold = false;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                }
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Monthly Leave Ledger Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }




        #endregion

        [HttpPost]
        public JsonResult GetsLeaveLedger(int nEmployeeID, int nACSID)
        {
            string sEmployeeIDs = nEmployeeID.ToString();
            List<LeaveLedgerReport> oLeaveLedgerReports = new List<LeaveLedgerReport>();
            try
            {
                oLeaveLedgerReports = LeaveLedgerReport.Gets(sEmployeeIDs, "", "", nACSID, 0, 0, 0, false, DateTime.Now, DateTime.Now, false, false, false,"","" ,((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oLeaveLedgerReports.Count>0)
                {
                    List<AttendanceCalendarSession> oAttendanceCalendarSessions = new List<AttendanceCalendarSession>();
                    string sSql = "SELECT TOP(1)* FROM AttendanceCalendarSession WHERE IsActive=1 ORDER BY ACSID DESC";
                    oAttendanceCalendarSessions = AttendanceCalendarSession.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    if(oAttendanceCalendarSessions.Count>0)
                    {
                        oLeaveLedgerReports[0].AttendanceCalendarSession = oAttendanceCalendarSessions[0];
                    }
                    else
                    {
                        oLeaveLedgerReports[0].AttendanceCalendarSession = new AttendanceCalendarSession();
                    }
                }
                else
                {
                    throw new Exception("Data not found!");
                }
             }
            catch (Exception ex)
            {
                LeaveLedgerReport oLeaveLedgerReport = new LeaveLedgerReport();
                oLeaveLedgerReports = new List<LeaveLedgerReport>();
                oLeaveLedgerReports.Add(oLeaveLedgerReport);
                oLeaveLedgerReport.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLeaveLedgerReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region HR Approve
        public ActionResult View_LeaveHRApproves(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLeaveApplications = new List<LeaveApplication>();
            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();

            //List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            //oAUOEDOs = AuthorizationUserOEDO.GetsByUser(((User)(Session[SessionInfo.CurrentUser])).UserID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //bool bRecommendation = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Recommendation, "LeaveApplication", oAUOEDOs);
            //TempData["Recommendation"] = bRecommendation;

            //bool bApprove = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Approve, "LeaveApplication", oAUOEDOs);
            //TempData["Approve"] = bApprove;

            //bool bCancel = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Cancel, "LeaveApplication", oAUOEDOs);
            //TempData["Cancel"] = bCancel;
            ////sSQL = "Select * from View_LeaveApplication WHere LeaveStatus=1 And (RequestForRecommendation=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + " And RecommendedBy=0) oR  RequestForAproval = " + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + " And ApproveBy=0";

            //TempData["Emp"] = ((User)(Session[SessionInfo.CurrentUser])).EmployeeID;

            string sSQL = "";
            //Self Applied
            oLeaveApplications = new List<LeaveApplication>();
            sSQL = "SELECT * FROM View_LeaveApplication WHERE IsApprove=0 AND ApproveBy>0" 
                    +" AND (CancelledBy IS NULL OR CancelledBy=0)  AND LeaveHeadID IN ("
                    +" SELECT LeaveHeadID FROM LeaveHead WHERE IsHRApproval=1)"
                    +" Order By LeaveApplicationID DESC";
            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLeaveApplications.AddRange(oLeaveApplications);



            List<EmployeeReportingPerson> oERPs = new List<EmployeeReportingPerson>();
            sSQL = "Select * from View_EmployeeReportingPerson Where IsActive=1 And EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            oERPs = EmployeeReportingPerson.Gets((int)((User)(Session[SessionInfo.CurrentUser])).EmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ERPs = oERPs;

            sSQL = "Select * from LeaveHead Where IsActive=1";
            ViewBag.LeaveHeads = LeaveHead.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ApplicationTypes = Enum.GetValues(typeof(EnumLeaveApplication)).Cast<EnumLeaveApplication>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LeaveTypes = Enum.GetValues(typeof(EnumLeaveType)).Cast<EnumLeaveType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.LeaveStatuss = Enum.GetValues(typeof(EnumLeaveStatus)).Cast<EnumLeaveStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(_oLeaveApplications);
        }
        #endregion HR Approve

        #region XL
        public void PrintLeaveApplicationXL(string sTempString, double ts)
        {
            LeaveApplication oLeaveApplication = new LeaveApplication();
            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();

            int nEmployeeID = Convert.ToInt32(sTempString.Split('~')[0]);
            string sStartDate = sTempString.Split('~')[1];
            string sEndDate = sTempString.Split('~')[2];
            int nPerson = Convert.ToInt32(sTempString.Split('~')[3]);

            string sSQL = "select * from View_LeaveApplication WHERE LeaveApplicationID<>0";
            if (nEmployeeID > 0 && nPerson <= 0)
            {
                sSQL = sSQL + " AND EmployeeID =" + nEmployeeID;
            }

            if (nPerson > 0)
            {
                if (nPerson == 1)
                {
                    sSQL = sSQL + " AND EmployeeID IN(SELECT EmployeeID  FROM EmployeeReportingPerson WHERE RPID = " + nEmployeeID + ")";
                }

                else if (nPerson == 2)
                {
                    sSQL = sSQL + " AND RequestForRecommendation  = " + nEmployeeID;
                }

                else if (nPerson == 3)
                {
                    sSQL = sSQL + " AND RequestForAproval  = " + nEmployeeID;
                }
            }

            if (sStartDate != "" && sEndDate != "")
            {
                sSQL = sSQL + " AND ((StartDateTime BETWEEN '" + sStartDate + "' AND '" + Convert.ToDateTime(sEndDate).AddDays(1).ToString("dd MMM yyyy") + "') OR (EndDateTime BETWEEN '" + sStartDate + "' AND '" + Convert.ToDateTime(sEndDate).AddDays(1).ToString("dd MMM yyyy") + "' ))";
            }
            sSQL = sSQL + "ORDER BY EmployeeCode, StartDateTime, EndDateTime";

            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oLeaveApplications = oLeaveApplications.OrderBy(x => x.EmployeeCode).ThenByDescending(x => x.StartDateTime).ThenByDescending(x => x.EndDateTime).ToList();

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
                var sheet = excelPackage.Workbook.Worksheets.Add("LEAVE APPLICATION");
                sheet.Name = "LEAVE APPLICATION";

                nMaxColumn = 25;

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
                sheet.Column(20).Width = 15;
                sheet.Column(21).Width = 15;
                sheet.Column(22).Width = 15;
                sheet.Column(23).Width = 15;
                sheet.Column(24).Width = 15;
                sheet.Column(25).Width = 15;
                sheet.Column(26).Width = 15;


                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = this.GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "LEAVE APPLICATION"; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "W. Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Leave Head"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Application Nature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Reason"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LeaveType"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Start Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "End Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Leave Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Paid/UnPaid"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Responsible Person"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Applied By"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Applied Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Recommended By"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Recommendation Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Recommendation Note"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Approve By Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Approve By Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Approval Note"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "HRApprove BY"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                if (oLeaveApplications.Count > 0)
                {
                    foreach (LeaveApplication oitem in oLeaveApplications)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.DepartmentName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.EmployeeWorkingStatus; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.JoiningDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.LeaveHeadName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ApplicationNatureInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.Location; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.Reason; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.LeaveTypeInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.StartDateTimeInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.EndDateTimeInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.LeaveStatusInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.IsUnPaidInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ResponsiblePersonName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.DBUserName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.SubmissionDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.RecommendedByName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.RecommendedByDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.RecommendationNote; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ApproveByName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ApproveByDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ApprovalNote; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                       
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.HRApproveBYName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                    }
                }
                else
                {
                    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Nothing to print!!"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 50; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LEAVEAPPLICATION.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion XL


        #region AlternativeDuty XL
        public void AlternativeDutyXL(string sTempString, double ts)
        {
            LeaveApplication oLeaveApplication = new LeaveApplication();
            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();

            string sStartDate = sTempString;

            string sSQL = "SELECT * from View_LeaveApplication AS LA WHERE CONVERT(DATETIME,CONVERT(VARCHAR(12),'"+sTempString+"',106)) BETWEEN CONVERT(DATETIME,CONVERT(VARCHAR(12), LA.StartDateTime,106)) AND CONVERT(DATETIME,CONVERT(VARCHAR(12),LA.EndDateTime,106))";
            sSQL = sSQL + "ORDER BY LA.EmployeeCode, LA.StartDateTime, LA.EndDateTime";

            oLeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            int nMaxColumn = 0;
            int colIndex = 1;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LEAVE APPLICATION");
                sheet.Name = "LEAVE APPLICATION";
                sheet.View.FreezePanes(6, 11);

                sheet.Column(colIndex++).Width = 5;//SL
                sheet.Column(colIndex++).Width = 12;//Name
                sheet.Column(colIndex++).Width = 15;//EmployeeCode
                sheet.Column(colIndex++).Width = 15;//Designation
                sheet.Column(colIndex++).Width = 15;//Department
                sheet.Column(colIndex++).Width = 8;//Type Of Leave
                sheet.Column(colIndex++).Width = 12;//Period Of Leave
                sheet.Column(colIndex++).Width = 12;//Period Of Leave
                sheet.Column(colIndex++).Width = 10;//Reliver
                sheet.Column(colIndex++).Width = 11;//Date Of Resume Duty
                nMaxColumn = colIndex;
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = this.GetImage(oCompany.OrganizationLogo);

                #region Report Header
                if(oLeaveApplications.Count>0)
                {
                    cell = sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oLeaveApplications[0].BusinessUnitName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;
                }
               

                cell=sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn];cell.Merge = true;
                cell = sheet.Cells[rowIndex, 1]; cell.Value = "All The Following Employees will be in leave on "+sTempString+ " and Onward"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion


                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation";  cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Type Of Leave"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                cell = sheet.Cells[rowIndex, colIndex++, rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Period Of Leave"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Reliever"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Resume Duty"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                rowIndex++;
                int count = 0;
                if (oLeaveApplications.Count > 0)
                {
                    foreach (LeaveApplication oitem in oLeaveApplications)
                    {
                        colIndex = 1;
                        count++;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = count.ToString(); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.DepartmentName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.LeaveHeadShortName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.StartDateTimeInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.EndDateTimeInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.ResumeDate; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                    }
                    rowIndex+=4;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 5]; cell.Merge = true;
                    cell.Value ="Signature Of HR Officer (In-Charge Of Leave)"; 
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Top.Style =  ExcelBorderStyle.Thin;

                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 5]; cell.Merge = true; cell.Value = "Date: " + DateTime.Now.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                }
                else
                {
                    cell=sheet.Cells[rowIndex, 1, rowIndex, nMaxColumn];cell.Merge = true;
                    cell.Value = "Nothing to print!!"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 50; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                cell = sheet.Cells[1, 1, rowIndex+13, nMaxColumn+1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LEAVEAPPLICATION.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion AlternativeDuty XL

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
    }
}
