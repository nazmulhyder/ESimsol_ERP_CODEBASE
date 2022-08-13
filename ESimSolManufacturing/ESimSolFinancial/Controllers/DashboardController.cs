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
using System.Net.Mail;
using System.Dynamic;
using System.Drawing.Imaging;


namespace ESimSolFinancial.Controllers
{
    public class DashboardController : Controller
    {
        #region Declaration
        AttendanceDaily _oAttendanceDaily;
        EmployeeRequestOnAttendance _oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
        private List<AttendanceDaily> _oAttendanceDailys;
        EmployeeDocumentAcceptance _oEmployeeDocumentAcceptance = new EmployeeDocumentAcceptance();
        
        #endregion

        #region Views
        public ActionResult View_AttendanceSummeryDashboards(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAttendanceDaily = new AttendanceDaily();
            _oAttendanceDaily.AttendanceSummery_Chart = AttendanceSummery_Chart.Get(DateTime.Now,DateTime.Now,((User)(Session[SessionInfo.CurrentUser])).UserID);
            string sSql_ACS = "SELECT * FROM AttendanceCalendarSession WHERE IsActive=1 ORDER BY [Session] DESC";
            _oAttendanceDaily.AttendanceCalendarSessions = AttendanceCalendarSession.Gets(sSql_ACS,((User)(Session[SessionInfo.CurrentUser])).UserID);

            //string sParams = "0" + "~" + "" + "~" + _oAttendanceDaily.AttendanceCalendarSessions[0].StartDate + "~" + _oAttendanceDaily.AttendanceCalendarSessions[0].EndDate
            //    + "~" + false + "~" + DateTime.Now + "~" + DateTime.Now + "~" + false + "~" + DateTime.Now + "~" + DateTime.Now+"~"+"50"+"~"+"0";
            //_oAttendanceDaily.AttendanceSummery_Lists = AttendanceSummery_List.Gets(sParams, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _oAttendanceDaily.AttendancePerformanceChart = AttendancePerformanceChart.Get(((User)(Session[SessionInfo.CurrentUser])).UserID);

            //string sSQL = "Select * from View_Notice Where IsActive=1 And [ExpireDate]>='" + DateTime.Now.AddDays(-7).ToString("dd MMM yyyy") + "' Order by IssueDate DESC";
            //ViewBag.Notices = Notice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.TotalEmployees = TotalEmployee.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.TotalEmployeeDetails = TotalEmployeeDetail.Gets(DateTime.Now, DateTime.Now,((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oAttendanceDaily);
        }

        public ActionResult View_EmployeeSelfServices(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            Employee oEmployee = new Employee();
            List<Employee> oEmployees = new List<Employee>();

            List<ITaxLedger>  _oITaxLedgers = new List<ITaxLedger>();
            ITaxLedger _oITaxLedger = new ITaxLedger();
            _oITaxLedgers.Add(_oITaxLedger);

            List<EmployeeRequestOnAttendance> oEmployeeRequestOnAttendances = new List<EmployeeRequestOnAttendance>();

            string sAssesmentYearSql = "SELECT * FROM ITaxAssessmentYear WHERE IsActive =1";
            ViewBag.ITaxAssessmentYear = ITaxAssessmentYear.Get(sAssesmentYearSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _oITaxLedgers[0].ITaxAssessmentYears = ITaxAssessmentYear.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.TaxAssYear = _oITaxLedgers[0].ITaxAssessmentYears;

            string sSql = "SELECT * FROM View_EmployeeRequestOnAttendance where (ApproveBy IS NULL OR ApproveBy = 0) AND EmployeeID=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID;
            oEmployeeRequestOnAttendances = EmployeeRequestOnAttendance.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EROA = oEmployeeRequestOnAttendances;

            List<AttendanceScheme> oAttendanceSchemes = new List<AttendanceScheme>();

            string sSql_Emp = "SELECT * FROM View_Employee WHERE EmployeeID=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID;
            oEmployees = Employee.Gets(sSql_Emp, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (oEmployees.Count > 0)
            {
                oEmployee = oEmployees[0];
            }

            string sSql_AS = "SELECT * FROM View_AttendanceScheme where AttendanceSchemeID IN(SELECT AttendanceSchemeID FROM View_EmployeeOfficial WHERE EmployeeID=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID+")";
            oAttendanceSchemes = AttendanceScheme.Gets(sSql_AS, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployee.AttendanceScheme = (oAttendanceSchemes.Count() > 0 && oAttendanceSchemes.FirstOrDefault().AttendanceSchemeID > 0) ? oAttendanceSchemes.FirstOrDefault() : new AttendanceScheme();

            List<EmployeeReportingPerson> oERPs = new List<EmployeeReportingPerson>();
            string sSQL = "Select * from View_EmployeeReportingPerson Where IsActive=1 And EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ") ORDER BY ERPID ASC";
            oERPs = EmployeeReportingPerson.Gets((int)((User)(Session[SessionInfo.CurrentUser])).EmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ERPs = oERPs;

            sSQL = "Select * from View_Notice Where IsActive=1 And [ExpireDate]>='" + DateTime.Now.AddDays(-7).ToString("dd MMM yyyy") + "' Order by IssueDate DESC";
            ViewBag.Notices = Notice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //Policy==>DocAttatchment
            //string sSQL_Policy = "SELECT * FROM CompanyDocument WHERE CDID <>0";
            string sSQL_Policy = "SELECT CDID,CompanyID,[Description],[FileName],NULL AS [DocFile],FileType,DBUserID,DBServerDateTime FROM CompanyDocument WHERE CDID <>0";
            List<CompanyDocument> oCompanyDocuments = new List<CompanyDocument>();
            oCompanyDocuments = CompanyDocument.Gets(sSQL_Policy, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oCompanyDocuments.ForEach(
                x =>
                {
                    x.DocFile = null;
                }
                );

            ViewBag.CompanyDocuments = oCompanyDocuments;
            string sSQL_Doc = "SELECT * FROM EmployeeDocument WHERE EDID <>0 AND EmployeeID=" + +((User)(Session[SessionInfo.CurrentUser])).EmployeeID;
            List<EmployeeDocument> oEmployeeDocuments = new List<EmployeeDocument>();
            oEmployeeDocuments = EmployeeDocument.Gets(sSQL_Doc, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEmployeeDocuments.ForEach(
                x =>
                {
                    x.DocFile = null;
                }
                );

            ViewBag.EmployeeDocuments = oEmployeeDocuments;
            #region 

            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            sSQL = "Select * from View_EmployeeSalaryStructure Where EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "Select * from View_EmployeeSalaryStructureDetail Where ESSID IN( SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            oEmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

           int nId = oEmployeeSalaryStructures.Any()? oEmployeeSalaryStructures[0].SalarySchemeID:0;

           
           


            sSQL = "SELECT * FROM  View_SalarySchemeDetail WHERE SalarySchemeID=" + nId + " ORDER BY SalarySchemeDetailID ";
            List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();
            oSalarySchemeDetails = SalarySchemeDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL="SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + nId + ") ORDER BY SalarySchemeDetailID ";
            List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
            oSalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



            oSalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalarySchemeDetails, oSalarySchemeDetailCalculations);

            foreach (SalarySchemeDetail oSSDItem in oSalarySchemeDetails)
            {

                foreach (EmployeeSalaryStructureDetail oESSDItem in oEmployeeSalaryStructureDetails)
                {

                    if (oSSDItem.SalaryHeadID == oESSDItem.SalaryHeadID)
                    {
                        oESSDItem.Calculation = oSSDItem.Calculation;
                    }

                }
                
            }

            ViewBag.ESS = oEmployeeSalaryStructures;
            ViewBag.ESSD = oEmployeeSalaryStructureDetails;

            ViewBag.SSDCalculation = oSalarySchemeDetailCalculations;


            
            //foreach (EmployeeSalaryStructureDetail oItem in oEmployeeSalaryStructureDetails)
            //{
            //    oItem.Calculation = SalarySchemeDetail.GetEquation(oSalarySchemeDetailCalculations);
            //}
            #endregion

            ViewBag.SSchemeDetail = oEmployeeSalaryStructureDetails;

            sSQL = "select * from SalaryScheme where SalarySchemeID IN(select SalarySchemeID from EmployeeSalaryStructure where EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
            ViewBag.CycleS = SalaryScheme.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);



            sSQL = "Select * from View_TrainingDevelopment Where IsActive=1 And ApproveBy>0 And EmployeeID =" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + " Order By StartDate DESC";
            ViewBag.TrainingDevelopments = TrainingDevelopment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Leave Application
            //sSQL = "Select * from LeaveHead Where IsActive=1";
            //List<LeaveHead> oLeaveHeads = new List<LeaveHead>();

            //oLeaveHeads = LeaveHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //if (oEmployee.Gender == "Male"){ oLeaveHeads.RemoveAll(x => x.RequiredFor == EnumLeaveRequiredFor.Female || x.RequiredFor == EnumLeaveRequiredFor.Others);}
            //ViewBag.LeaveHeads = oLeaveHeads;

            List<EmployeeLeaveLedger> oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            oEmployeeLeaveLedgers = EmployeeLeaveLedger.GetsActiveLeaveLedger(oEmployee.EmployeeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EmployeeLeaveLedgers = oEmployeeLeaveLedgers;

            sSQL = "Select * from View_AttendanceCalendarSession Where IsActive=1 And AttendanceCalendarID In(Select AttendanceCalendarID from " +
                  "AttendanceCalendar Where IsActive=1 And AttendanceCalendarID In ( Select AttendanceCalenderID from AttendanceScheme "+
                  "Where IsActive=1 And AttendanceSchemeID In (Select AttendanceSchemeID from EmployeeOfficial Where IsActive=1 And EmployeeID=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + ")))";
            List<AttendanceCalendarSession> oACSs = new List<AttendanceCalendarSession>();
            oACSs = AttendanceCalendarSession.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oACSs.Count() > 0)
            {
                int nEmpID=Convert.ToInt32(((User)(Session[SessionInfo.CurrentUser])).EmployeeID);
                List<LeaveLedger> oLeaveLedgers = new List<LeaveLedger>();
                oLeaveLedgers = LeaveLedger.Gets(nEmpID, oACSs[0].ACSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLeaveLedgers.Count() > 0 && oLeaveLedgers[0].ErrorMessage != "") { oLeaveLedgers = new List<LeaveLedger>(); }
                ViewBag.LeaveLedgers = oLeaveLedgers;
            }
            else
            {
                List<LeaveLedger> oLeaveLedgers=new List<LeaveLedger>();
                ViewBag.LeaveLedgers = oLeaveLedgers;
            }

            ViewBag.LeaveTypes = Enum.GetValues(typeof(EnumLeaveType)).Cast<EnumLeaveType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            #endregion

            #region Loan Application

            ViewBag.LoanStatus = Enum.GetValues(typeof(EnumEmployeeLoanStatus)).Cast<EnumEmployeeLoanStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            sSQL = "SELECT * FROM VIEW_EmployeeSalaryStructure WHERE EmployeeID=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID;
            ViewBag.EmployeeSalaryStructure = EmployeeSalaryStructure.Get(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSQL = "SELECT * FROM VIEW_EmployeeSalaryStructureDetail where ESSID = (SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID + ") AND SalaryHeadID =1";
            ViewBag.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);


             //sSQL = "SELECT * FROM View_LeaveApplication WHERE LeaveApplicationID<>0 "
             //+ " AND RequestForRecommendation =" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID
             //+ " AND RequestForAproval =" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID
             //+ " AND LeaveStatus NOT IN (3,4)";

            sSQL = "SELECT * FROM View_LeaveApplication WHERE LeaveApplicationID<>0 AND EmployeeID=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID
                    + "AND EmpLeaveLedgerID IN(SELECT EmpLeaveLedgerID  FROM View_EmployeeLeaveLedger WHERE ACSID IN"
                    + "(SELECT ACSID FROM AttendanceCalendarSession WHERE IsActive = 1)) Order By StartDAteTime DESC";

             ViewBag.LeaveApplications = LeaveApplication.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //ViewBag.LastEmployeeLoan=null;

            #endregion

            ViewBag.ClearanceStatus = Enum.GetValues(typeof(EnumESCrearance)).Cast<EnumESCrearance>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SelfServiceClearances = SelfServiceClearance.Gets(Convert.ToInt32(((User)Session[SessionInfo.CurrentUser]).EmployeeID), ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.oACSs = AttendanceCalendarSession.Gets("SELECT * FROM AttendanceCalendarSession", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            return View(oEmployee);
        }


        public ActionResult View_PayrollDashBoard(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
   
            List<EmployeeOfficial> oEmployeeOfficials = new List<EmployeeOfficial>();
            string sSQL = "Select * from View_EmployeeOfficial Where IsActive=1";
            //oEmployeeOfficials = EmployeeOfficial.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Department> oDepartments = new List<Department>();
            sSQL = "Select * from Department Where IsActive=1 And DepartmentID In(Select Distinct(DepartmentID) from DepartmentRequirementPolicy Where DepartmentRequirementPolicyID In (Select distinct(DRPID) from EmployeeOfficial Where IsActive=1))";
            oDepartments = Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Departments = oDepartments;

            sSQL = "Select * from Location Where IsActive=1";
            ViewBag.Locations = Location.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<SalarySummary> oSalarySummarys = new List<SalarySummary>();
            List<PayrollProcessManagement> oPPMs = new List<PayrollProcessManagement>();
            sSQL = "Select top(1)* from View_PayrollProcessManagement Order By PPMID DESC";
            oPPMs = PayrollProcessManagement.Gets(sSQL,((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oPPMs.Count() > 0 && oPPMs[0].PPMID > 0)
            { 
                ViewBag.SalaryDate = "Salary Summary Date: " + oPPMs[0].SalaryFromInString + " to " + oPPMs[0].SalaryToInString;
                ViewBag.SalaryBreakDowns = EmployeeSalary.GetsPayRollBreakDown(oPPMs[0].SalaryFrom, oPPMs[0].SalaryTo, false, 0, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oSalarySummarys = SalarySummary.Gets(oPPMs[0].SalaryFrom, oPPMs[0].SalaryTo, false, 0, ((User)(Session[SessionInfo.CurrentUser])).UserID);
               if(oSalarySummarys.Count()>0 && oSalarySummarys[0].DepartmentID>0){
                ViewBag.SalarySummarys = GenerateDeptWiseSalarySummary(oSalarySummarys); 
               }
               else
               {
                   ViewBag.SalarySummarys = oSalarySummarys;
               }
            }
            else
            { 
                ViewBag.SalaryDate="";
                oSalarySummarys = new List<SalarySummary>();
                ViewBag.SalaryBreakDowns = oSalarySummarys;
                ViewBag.SalarySummarys =  oSalarySummarys;
            }
            
            return View(oEmployeeOfficials);
        }

        #endregion

        #region Search

        [HttpPost]
        public JsonResult AttendanceSummerySearch_Chart(DateTime DateFrom, DateTime DateTo)
        {
            AttendanceSummery_Chart oAttendanceSummery = new AttendanceSummery_Chart();
            try
            {
                oAttendanceSummery = AttendanceSummery_Chart.Get(DateFrom, DateTo,((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            }
            catch (Exception ex)
            {
                oAttendanceSummery = new AttendanceSummery_Chart();
                oAttendanceSummery.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceSummery);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTotalEmployeeDetail(DateTime DateFrom, DateTime DateTo)
        {
            List<TotalEmployeeDetail> oTotalEmployeeDetails = new List<TotalEmployeeDetail>();
            try
            {
                oTotalEmployeeDetails = TotalEmployeeDetail.Gets(DateFrom, DateTo, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                TotalEmployeeDetail oTotalEmployeeDetail = new TotalEmployeeDetail();
                oTotalEmployeeDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTotalEmployeeDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttendanceSummerySearch_List(string sParams)
        {
            List<AttendanceSummery_List> oAttendanceSummerys = new List<AttendanceSummery_List>();
            List<AttendanceCalendarSession> oACSs = new List<AttendanceCalendarSession>();
            try
            {
                string sSql_ACS = "SELECT * FROM AttendanceCalendarSession WHERE IsActive=1 ORDER BY [Session] DESC";
                oACSs = AttendanceCalendarSession.Gets(sSql_ACS, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sParams.Split('~')[2] = oACSs[0].StartDate.ToString();
                sParams.Split('~')[3] = oACSs[0].EndDate.ToString();
                oAttendanceSummerys = AttendanceSummery_List.Gets(sParams, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                AttendanceSummery_List oAttendanceSummery = new AttendanceSummery_List();
                oAttendanceSummery.ErrorMessage = ex.Message;
                oAttendanceSummerys = new List<AttendanceSummery_List>();
                oAttendanceSummerys.Add(oAttendanceSummery);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceSummerys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult SaveAcceptance(EmployeeDocumentAcceptance oEmployeeDocumentAcceptance)
        {
            _oEmployeeDocumentAcceptance = new EmployeeDocumentAcceptance();
            try
            {
                _oEmployeeDocumentAcceptance = oEmployeeDocumentAcceptance;
                if (_oEmployeeDocumentAcceptance.EDAID > 0)
                {
                    _oEmployeeDocumentAcceptance = _oEmployeeDocumentAcceptance.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oEmployeeDocumentAcceptance = _oEmployeeDocumentAcceptance.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                if (_oEmployeeDocumentAcceptance.EDAID > 0)
                {
                    SendMailForDocumentAcceptance(_oEmployeeDocumentAcceptance, _oEmployeeDocumentAcceptance.EmployeeID);
                }
            }
            catch (Exception ex)
            {
                _oEmployeeDocumentAcceptance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeDocumentAcceptance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsForData(List<EmployeeOfficial> oEmployeeOfficials)
        {
            List<EmployeeOfficial> objEmployeeOfficials = new List<EmployeeOfficial>();
            string sSQL = "Select * from View_EmployeeOfficial Where IsActive=1";
            objEmployeeOfficials = EmployeeOfficial.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            var jSonResult = Json(objEmployeeOfficials, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        private void SendMailForDocumentAcceptance(EmployeeDocumentAcceptance _oEmployeeDocumentAcceptance, int nEmployeeID)
        {
            List<EmployeeReportingPerson> oERPs = new List<EmployeeReportingPerson>();
            string sSQL1 = "Select * from View_EmployeeReportingPerson Where IsActive=1 And EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            oERPs = EmployeeReportingPerson.Gets("SELECT TOP(1) * FROM View_EmployeeReportingPerson WHERE EmployeeID="+nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            
            Employee oEmployee = new Employee();
            oEmployee = oEmployee.Get(nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            Employee oEmp = new Employee();
            if (oERPs.Count > 0)
            {
                oEmp = oEmp.Get(oERPs[0].EmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            if (Global.IsValidMail(oEmployee.Email))
            {
                List<string> emialTos = new List<string>();
                emialTos.Add(oEmp.Email);

                string subject = "Policy Acceptance from " + _oEmployeeDocumentAcceptance.EmployeeName;
                string message = "I am " + _oEmployeeDocumentAcceptance.EmployeeName + " and read the policy of the company and accepted it.";
                string bodyInfo = "";

                List<User> oUsers = new List<User>();
                string sSQL = "Select * from View_User Where EmployeeID=" + oEmployee.EmployeeID;
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oUsers.Any() && oUsers.FirstOrDefault().UserID > 0)
                {

                    var oUser = oUsers.FirstOrDefault();
                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                             "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                             oEmployee.Name, message, _oEmployeeDocumentAcceptance.EmployeeName,
                        //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                             Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                }
                else
                {
                    bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div>" +
                                             "<div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{3}</div> <div style='padding-top:20px;'>Mail sent at time {4}</div>",
                                             oEmployee.Name, message, _oEmployeeDocumentAcceptance.EmployeeName,
                                             Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                }


                #region Email Credential
                EmailConfig oEmailConfig = new EmailConfig();
                oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                #endregion

                Global.MailSend(subject, bodyInfo, emialTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
            }
        }


        #region Mail Send
        [HttpPost]
        public JsonResult MailSendFromAttendanceDashBoard(string MailSubject, string MailBody, string sMailTo)
        {
            _oAttendanceDaily = new AttendanceDaily();
            try
            {
                List<string> sMailTos = new List<string>();
                sMailTos.Add(sMailTo);

                #region Email Credential
                EmailConfig oEmailConfig = new EmailConfig();
                oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                #endregion

                if (Global.MailSend(MailSubject, MailBody, sMailTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired))
                {
                    _oAttendanceDaily.ErrorMessage = "Mail Send!";
                }
                else
                {
                    _oAttendanceDaily.ErrorMessage = "Mail Not Send!";
                }

            }
            catch (Exception ex)
            {
                _oAttendanceDaily = new AttendanceDaily();
                _oAttendanceDaily.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceDaily);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Mail Send

        #region employee request
        [HttpPost]
        public JsonResult SaveRequest(EmployeeRequestOnAttendance oEmployeeRequestOnAttendance)
        {
            _oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();

            try
            {
                _oEmployeeRequestOnAttendance = oEmployeeRequestOnAttendance;
                if (_oEmployeeRequestOnAttendance.EROAID > 0)
                {
                    _oEmployeeRequestOnAttendance = _oEmployeeRequestOnAttendance.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oEmployeeRequestOnAttendance = _oEmployeeRequestOnAttendance.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                if (_oEmployeeRequestOnAttendance.EROAID > 0)
                {
                    SendMailForLeaveRecommendation(_oEmployeeRequestOnAttendance, oEmployeeRequestOnAttendance.RecommendPersonID);
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
        private void SendMailForLeaveRecommendation(EmployeeRequestOnAttendance oEmployeeRequestOnAttendance, int nEmployeeID)
        {
            List<EmployeeReportingPerson> oERPs = new List<EmployeeReportingPerson>();
            string sSQL1 = "Select * from View_EmployeeReportingPerson Where IsActive=1 And EmployeeID=(Select EmployeeID from Users Where UserID=" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            oERPs = EmployeeReportingPerson.Gets(nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            
            Employee oEmployee = new Employee();
            oEmployee = oEmployee.Get(nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (Global.IsValidMail(oEmployee.Email))
            {
                List<string> emialTos = new List<string>();
                emialTos.Add(oEmployee.Email);

                string subject = "OSD Request from " + oEmployeeRequestOnAttendance.EmployeeName;
                string message = "I am applying for an OSD on " + oEmployeeRequestOnAttendance.AttendanceDateInString + ".";
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


        [HttpPost]
        public JsonResult DeleteRequest(int nEROAID, double nts)
        {
            _oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
            try
            {
                _oEmployeeRequestOnAttendance.EROAID = nEROAID;
                _oEmployeeRequestOnAttendance = _oEmployeeRequestOnAttendance.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeRequestOnAttendance.ErrorMessage == "")
                {
                    _oEmployeeRequestOnAttendance.ErrorMessage = "Delete Successfully.";
                }
            }
            catch (Exception ex)
            {
                _oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
                _oEmployeeRequestOnAttendance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeRequestOnAttendance.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchRequest(DateTime sDateFrom, DateTime sDateTo)
        {

            List<EmployeeRequestOnAttendance> oEmployeeRequestOnAttendances = new List<EmployeeRequestOnAttendance>();
            string sSql = "SELECT * FROM View_EmployeeRequestOnAttendance WHERE AttendanceDate BETWEEN '" + sDateFrom.ToString("dd MMM yyyy") + "' AND '" + sDateTo.ToString("dd MMM yyyy") + "' AND EmployeeID=" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID;
            oEmployeeRequestOnAttendances = EmployeeRequestOnAttendance.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (oEmployeeRequestOnAttendances.Count <= 0)
            {
                EmployeeRequestOnAttendance oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
                oEmployeeRequestOnAttendance.ErrorMessage = "No Requests between " + sDateFrom.ToString("dd MMM yyyy") + " and " + sDateTo.ToString("dd MMM yyyy");
                oEmployeeRequestOnAttendances.Add(oEmployeeRequestOnAttendance);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeRequestOnAttendances);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion


        public ActionResult PrintTimeCard_F2(string dtForm, string dtTo)
        {

            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();

            string sEmployeeIDs = (((User)(Session[SessionInfo.CurrentUser])).EmployeeID).ToString();
            DateTime Startdate = Convert.ToDateTime(dtForm);
            DateTime EndDate = Convert.ToDateTime(dtTo);
            string sLocationID = "";
            string sDepartmentIds = "";
            string sBUnitIDs = "";
            string sType = "";
            double nStartSalaryRange = 0.0;
            double nEndSalaryRange = 0.0;


            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange,"", "", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.Company = oCompanys.First();
            oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
            oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            rptTimeCard_F2 oReport = new rptTimeCard_F2();
            byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
            return File(abytes, "application/pdf");
        }

        public System.Drawing.Image GetImage(byte[] Image)
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

        public List<dynamic> GenerateDeptWiseSalarySummary(List<SalarySummary> oSalarySummarys)
        {
            List<dynamic> oDynamicList = new List<dynamic>();

            List<int> oDeptIDs = new List<int>();
            List<SalarySummary> oTSSs = new List<SalarySummary>();
            List<SalarySummary> oTSSummarys = new List<SalarySummary>();
            if (oSalarySummarys.Count() > 0)
            {
                oDeptIDs = oSalarySummarys.Select(x => x.DepartmentID).Distinct().ToList();

                foreach (int DeptID in oDeptIDs)
                {
                    oTSSs = oSalarySummarys.Where(x => x.DepartmentID == DeptID).ToList();
                    dynamic obj = new ExpandoObject();
                    var oExpObj = obj as IDictionary<string, Object>;
                    oExpObj.Add("DepartmentName", oTSSs[0].DepartmentName);
                    oExpObj.Add("GrossSalary", oTSSs.Where(x => x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Basic).Sum(x => x.Amount));
                    oExpObj.Add("EarningsSpan", oTSSs.Where(x => (x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Basic)).Count()+2);

                    foreach (SalarySummary oItem in oTSSs.Where(x => (x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Basic)).ToList())
                    {
                        oExpObj.Add(oItem.SalaryHeadName, oItem.Amount);
                    }

                    oExpObj.Add("OTAllowance", oTSSs.Where(x => x.SalaryHeadName == "OT Allowance").Sum(x => x.Amount));
                    oExpObj.Add("Others", oTSSs.Where(x => (x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Addition && x.SalaryHeadName != "OT Allowance")).Sum(x => x.Amount));
                    oExpObj.Add("GrossEarning", oTSSs.Where(x => (x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Basic || x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Addition)).Sum(x => x.Amount));

                    foreach (SalarySummary oItem in oTSSs.Where(x => (x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Deduction)).ToList())
                    {
                        oExpObj.Add(oItem.SalaryHeadName, oItem.Amount);
                    }
                    oExpObj.Add("DeductionSpan", oTSSs.Where(x => (x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Deduction)).Count());
                    oExpObj.Add("NetSalary", oTSSs.Where(x => (x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Basic || x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Addition)).Sum(x => x.Amount) - oTSSs.Where(x => (x.SalaryHeadTypeInt == (int)EnumSalaryHeadType.Deduction)).Sum(x => x.Amount));
                   
                    oDynamicList.Add(oExpObj);
                }

            }
            return oDynamicList;
        }

        [HttpPost]
        public JsonResult GetPayrollSummary(PayrollSummary oPayrollSummary)
        {
            DateTime dStartDate = Convert.ToDateTime(oPayrollSummary.Params.Split('~')[0]);
            DateTime dEndDate = Convert.ToDateTime(oPayrollSummary.Params.Split('~')[1]);
            int nLocationID = Convert.ToInt32(oPayrollSummary.Params.Split('~')[2]);
            try
            {
                oPayrollSummary = new PayrollSummary();

                oPayrollSummary.SalaryBreakDowns = EmployeeSalary.GetsPayRollBreakDown(dStartDate, dEndDate, true, nLocationID,((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oPayrollSummary.SalaryBreakDowns.Count() > 0 && oPayrollSummary.SalaryBreakDowns[0].DepartmentID>0)
                {
                    oPayrollSummary.SalaryDate = "Salary Summary Date: " + dStartDate.ToString() + " to " + dEndDate.ToString();
                    List<SalarySummary> oSalarySummarys = new List<SalarySummary>();
                    oSalarySummarys = SalarySummary.Gets(dStartDate, dEndDate, true, nLocationID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    if (oSalarySummarys.Count() > 0 && oSalarySummarys[0].DepartmentID > 0)
                    {
                        oPayrollSummary.SalarySummarys = GenerateDeptWiseSalarySummary(oSalarySummarys);
                    }
                    else
                    {
                        oPayrollSummary.SalarySummarys = new List<dynamic>();
                    }
                }
                else
                {
                    throw new Exception(oPayrollSummary.SalaryBreakDowns[0].ErrorMessage);
                }
                
            }
            catch (Exception ex)
            {
                oPayrollSummary = new PayrollSummary();
                oPayrollSummary.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayrollSummary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsSettlementHistory(SelfServiceClearance oSelfServiceClearance)
        {
            List<SelfServiceClearance> oSelfServiceClearances = new List<SelfServiceClearance>();
            try
            {
                oSelfServiceClearances = SelfServiceClearance.Gets(Convert.ToInt32(((User)Session[SessionInfo.CurrentUser]).EmployeeID), ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSelfServiceClearance = new SelfServiceClearance();
                oSelfServiceClearance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSelfServiceClearances);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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
        public ActionResult PDFTaxAssessment(int nEmployeeID, int AssesmentYearID, int Status)
        {
            ITaxLedger _oITaxLedger = new ITaxLedger();
            string sSql = "SELECT * FROM View_ITaxLedger WHERE ITaxLedgerID<>0";
            if (nEmployeeID > 0)
            {
                sSql += " AND EmployeeID =" + nEmployeeID;
            }
            if (AssesmentYearID > 0)
            {
                sSql += " AND ITaxRateSchemeID IN(SELECT ITaxRateSchemeID  FROM ITaxRateScheme WHERE ITaxAssessmentYearID=" + AssesmentYearID + ")";
            }
            if (Status == 1)
            {
                sSql += " AND IsActive = 1";
            }
            if (Status == 0)
            {
                sSql += " AND IsActive = 0";
            }

            _oITaxLedger = ITaxLedger.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            

            sSql = "SELECT * FROM Employee WHERE EmployeeID=" + nEmployeeID;
            _oITaxLedger.Employee = Employee.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT * FROM View_ITaxLedgerSalaryHead WHERE ITaxLedgerID=" + _oITaxLedger.ITaxLedgerID;
            _oITaxLedger.ITaxLedgerSalaryHeads = ITaxLedgerSalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "";
            sSql = "SELECT TOP(1)* FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID IN(SELECT ITaxAssessmentYearID FROM ITaxRateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxLedger WHERE ITaxLedgerID=" + _oITaxLedger.ITaxLedgerID + "))";
            _oITaxLedger.ITaxAssessmentYear = ITaxAssessmentYear.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM ITaxRateScheme WHERE ITaxRateSchemeID=" + _oITaxLedger.ITaxRateSchemeID;
            _oITaxLedger.ITaxRateScheme = ITaxRateScheme.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM EmployeeTINInformation WHERE EmployeeID=" + nEmployeeID;
            _oITaxLedger.EmployeeTINInformation = EmployeeTINInformation.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "select * from View_EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID + ")";
            _oITaxLedger.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM View_ITaxHeadConfiguration ";
            _oITaxLedger.ITaxHeadConfigurations = ITaxHeadConfiguration.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM ITaxRateSlab";
            _oITaxLedger.ITaxRateSlabs = ITaxRateSlab.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM ITaxRebateItem";
            _oITaxLedger.ITaxRebateItems = ITaxRebateItem.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM ITaxRebatePayment WHERE EmployeeID=" + nEmployeeID;
            _oITaxLedger.ITaxRebatePayments = ITaxRebatePayment.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);


            //select * from View_EmployeeSalaryDetail where Employeesalaryid in(select EmployeeSalaryID from EmployeeSalary where EmployeeID = 3 and EndDate between '01 Jul 2016' and '30 Jun 2017')

            sSql = "select * from EmployeeSalary where EmployeeID=" + nEmployeeID + " AND (SalaryDate BETWEEN '" + _oITaxLedger.ITaxAssessmentYear.StartDateInString + "' AND '" + _oITaxLedger.ITaxAssessmentYear.EndDateInString + "')";
            _oITaxLedger.EmployeeSalarys = EmployeeSalary.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN(select EmployeeSalaryID from EmployeeSalary WHERE EmployeeID = "+nEmployeeID+" AND (EndDate BETWEEN '" + _oITaxLedger.ITaxAssessmentYear.StartDateInString + "' AND '" + _oITaxLedger.ITaxAssessmentYear.EndDateInString + "'))";
            _oITaxLedger.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            sSql = "SELECT * FROM SalaryHead";
            _oITaxLedger.SalaryHeads = SalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oITaxLedger.Company = oCompanys.First();
            _oITaxLedger.Company.CompanyLogo = GetCompanyLogo(_oITaxLedger.Company);

            rptTaxAssessment oReport = new rptTaxAssessment();
            byte[] abytes = oReport.PrepareReport(_oITaxLedger);
            return File(abytes, "application/pdf");
        }

    }
    public class PayrollSummary
    {

        public PayrollSummary()
        {
            SalaryBreakDowns = new List<EmployeeSalary>();
            SalaryDate = "";
            Params = "";
            ErrorMessage = "";
        }

        public List<dynamic> SalarySummarys { get; set; }
        public List<EmployeeSalary> SalaryBreakDowns { get; set; }
        public string SalaryDate { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }

    }




}
