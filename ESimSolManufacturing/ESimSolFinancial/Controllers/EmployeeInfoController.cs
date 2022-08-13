using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class EmployeeInfoController : Controller
    {
        #region Declaration
        Employee _oEmployee = new Employee();
        #endregion

        #region Actions
        public ActionResult ViewEmployeeInfo(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oEmployee = new Employee();
            return View(_oEmployee);
        }


        [HttpPost]
        public JsonResult SearchProfile(Employee oEmployee)
        {
            _oEmployee = new Employee();
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();

            _oEmployee = _oEmployee.Get(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeTINInformations = EmployeeTINInformation.Get(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeOfficial = oEmployeeOfficial.Get(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeReportingPersons = EmployeeReportingPerson.Gets(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeSalaryStructures = EmployeeSalaryStructure.Gets("SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID=" + oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.LeaveLedgerReports = LeaveLedgerReport.Gets(""+oEmployee.EmployeeID, "", "", 0, 0, 0, 0, false, DateTime.Now, DateTime.Now, false, false, false, "", "", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployee.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets("SELECT * FROM View_EmployeeSalaryStructureDetail WHERE SalaryHeadType=1 AND ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID = " + oEmployee.EmployeeID + ")", (int)(Session[SessionInfo.currentUserID]));
            
            _oEmployee.EmployeeNominees = EmployeeNominee.Gets(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeEducations = EmployeeEducation.Gets(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeTrainings = EmployeeTraining.Gets(oEmployee.EmployeeID, (int)(Session[SessionInfo.currentUserID]));

            string sSql = "SELECT * FROM VIEW_TransferPromotionIncrement WHERE ISNULL(IsNoHistory, 0)=0 AND ApproveBy >0 AND EmployeeID=" + oEmployee.EmployeeID + " ORDER BY EffectedDate DESC";
            _oEmployee.TPIs = TransferPromotionIncrement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _oEmployee.IncrementHistorys = _oEmployee.TPIs.Where(s => s.IsIncrement == true).ToList();
            _oEmployee.TransferHistorys = _oEmployee.TPIs.Where(s => s.IsTransfer == true).ToList();
            _oEmployee.PromotionHistorys = _oEmployee.TPIs.Where(s => s.IsPromotion == true).ToList();


            string sSqlForEAN = "SELECT * FROM View_EmployeeActivityNote AS HH WHERE HH.EmployeeID="+oEmployee.EmployeeID+" AND ApproveBy > 0";
            _oEmployee.AcitivityNotes = EmployeeActivityNote.Gets(sSqlForEAN, (int)(Session[SessionInfo.currentUserID]));

            string sSqlForELL = "SELECT HH.[Session], HH.LeaveName, HH.TotalDay, ISNULL((SELECT SUM(DATEDIFF(DAY,MM.StartDateTime, MM.EndDateTime)+1) FROM LeaveApplication AS MM WHERE ISNULL(MM.CancelledBy,0)<=0 AND MM.EmpLeaveLedgerID=HH.EmpLeaveLedgerID),0) AS EnjoyLeave FROM View_EmployeeLeaveLedger AS HH WHERE HH.EmployeeID=" + oEmployee.EmployeeID + " AND LeaveName IN ('Casual Leave','Sick Leave') ORDER BY [Session] DESC";
            _oEmployee.EmployeeLeaveLedgers = EmployeeLeaveLedger.Gets(sSqlForELL, (int)(Session[SessionInfo.currentUserID]));

            var queryResult = from ELL in _oEmployee.EmployeeLeaveLedgers group ELL by ELL.Session;

            List<EmployeeLeaveLedger> oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            foreach (var ELLGroup in queryResult)
            {
                EmployeeLeaveLedger oEmployeeLeaveLedger = new EmployeeLeaveLedger();
                oEmployeeLeaveLedger.Session = ELLGroup.Key;
                foreach (var ELL in ELLGroup)
                {
                    if(ELL.LeaveName == "Casual Leave")
                    {
                        oEmployeeLeaveLedger.CLDetailsInStr = "T-"+ ELL.TotalDay + ", E-"+ ELL.EnjoyLeave +", B-"+ (ELL.TotalDay - ELL.EnjoyLeave) +"";
                    }
                    else
                    {
                        oEmployeeLeaveLedger.SLDetailsInStr = "T-" + ELL.TotalDay + ", E-" + ELL.EnjoyLeave + ", B-" + (ELL.TotalDay - ELL.EnjoyLeave) + "";
                    }
                }
                oEmployeeLeaveLedgers.Add(oEmployeeLeaveLedger);
            }
            _oEmployee.EmployeeLeaveLedgers = oEmployeeLeaveLedgers;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //Attendance History Search
        [HttpPost]
        public JsonResult AttendanceHistorySearch(int nEmpID, string sStartDate, string sEndDate)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sStartDate);
            DateTime dtDateTo = Convert.ToDateTime(sEndDate);
            List<MonthlyAttendanceReport> MonthlyAttendanceReports = new List<MonthlyAttendanceReport>();
            try
            {
                MonthlyAttendanceReports = MonthlyAttendanceReport.Gets("" + nEmpID, "", "", "", "", "", dtDateFrom, dtDateTo, "", "", "", 0, 0, "", "", (int)(Session[SessionInfo.currentUserID]));
                if (MonthlyAttendanceReports.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                MonthlyAttendanceReport oMonthlyAttendanceReport = new MonthlyAttendanceReport();
                MonthlyAttendanceReports = new List<MonthlyAttendanceReport>();
                oMonthlyAttendanceReport.ErrorMessage = ex.Message;
                MonthlyAttendanceReports.Add(oMonthlyAttendanceReport);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(MonthlyAttendanceReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //Leave Application History Search
        [HttpPost]
        public JsonResult LAHSearch(int nEmpID, string sStartDate, string sEndDate)
        {
            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();
            string sSql = "SELECT * FROM View_LeaveApplication AS HH WHERE HH.EmployeeID=" + nEmpID + " AND HH.StartDateTime >= CONVERT(DATE,CONVERT(VARCHAR(12),'" + sStartDate + "',106)) AND HH.EndDateTime <= CONVERT(DATE,CONVERT(VARCHAR(12),'" + sEndDate + "',106)) ORDER BY StartDateTime DESC";
            try
            {
                oLeaveApplications = LeaveApplication.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
                if (oLeaveApplications.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                LeaveApplication oLeaveApplication = new LeaveApplication();
                oLeaveApplications = new List<LeaveApplication>();
                oLeaveApplication.ErrorMessage = ex.Message;
                oLeaveApplications.Add(oLeaveApplication);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLeaveApplications);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Print_PDF(string sParams)
        {
            List<MonthlyAttendanceReport> MonthlyAttendanceReports = new List<MonthlyAttendanceReport>();
            List<LeaveApplication> oLeaveApplications = new List<LeaveApplication>();

            int nEmployeeID = Convert.ToInt32(sParams.Split('~')[0]);
            string sAttHistory = sParams.Split('~')[1];
            string sLAHistory = sParams.Split('~')[2];

            if (sAttHistory.Split(',').Count() > 0)
            {
                if (!string.IsNullOrEmpty(sAttHistory.Split(',')[0]))
                {
                    DateTime dtDateFrom = Convert.ToDateTime(sAttHistory.Split(',')[0]);
                    DateTime dtDateTo = Convert.ToDateTime(sAttHistory.Split(',')[1]);

                    MonthlyAttendanceReports = MonthlyAttendanceReport.Gets("" + nEmployeeID, "", "", "", "", "", dtDateFrom, dtDateTo, "", "", "", 0, 0, "", "", (int)(Session[SessionInfo.currentUserID]));
                }
            }
            if (sLAHistory.Split(',').Count() > 0)
            {
                if (!string.IsNullOrEmpty(sLAHistory.Split(',')[0]))
                {
                    string sStartDate = sLAHistory.Split(',')[0];
                    string sEndDate = sLAHistory.Split(',')[1];
                    string sSqlL = "SELECT * FROM View_LeaveApplication AS HH WHERE HH.EmployeeID=" + nEmployeeID + " AND HH.StartDateTime >= CONVERT(DATE,CONVERT(VARCHAR(12),'" + sStartDate + "',106)) AND HH.EndDateTime <= CONVERT(DATE,CONVERT(VARCHAR(12),'" + sEndDate + "',106)) ORDER BY StartDateTime DESC";

                    oLeaveApplications = LeaveApplication.Gets(sSqlL, (int)(Session[SessionInfo.currentUserID]));
                }
            }

            _oEmployee = new Employee();
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();

            _oEmployee = _oEmployee.Get(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeBankAccounts = EmployeeBankAccount.Gets(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeTINInformations = EmployeeTINInformation.Get(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeOfficial = oEmployeeOfficial.Get(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeReportingPersons = EmployeeReportingPerson.Gets(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeSalaryStructures = EmployeeSalaryStructure.Gets("SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID=" + nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.LeaveLedgerReports = LeaveLedgerReport.Gets("" + nEmployeeID, "", "", 0, 0, 0, 0, false, DateTime.Now, DateTime.Now, false, false, false, "", "", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployee.EmployeeSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets("SELECT * FROM View_EmployeeSalaryStructureDetail WHERE SalaryHeadType=1 AND ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID = " + nEmployeeID + ")", (int)(Session[SessionInfo.currentUserID]));

            //_oEmployee.DisciplinaryActions = DisciplinaryAction.Gets("SELECT * FROM View_DisciplinaryAction WHERE EmployeeID=" + nEmployeeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _oEmployee.EmployeeNominees = EmployeeNominee.Gets(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeExperiences = EmployeeExperience.Gets(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeEducations = EmployeeEducation.Gets(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));
            _oEmployee.EmployeeTrainings = EmployeeTraining.Gets(nEmployeeID, (int)(Session[SessionInfo.currentUserID]));

            string sSql = "SELECT * FROM VIEW_TransferPromotionIncrement WHERE ISNULL(IsNoHistory, 0)=0 AND ApproveBy >0 AND EmployeeID=" + nEmployeeID + " ORDER BY EffectedDate DESC";
            _oEmployee.TPIs = TransferPromotionIncrement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            _oEmployee.IncrementHistorys = _oEmployee.TPIs.Where(s => s.IsIncrement == true).ToList();
            _oEmployee.TransferHistorys = _oEmployee.TPIs.Where(s => s.IsTransfer == true).ToList();
            _oEmployee.PromotionHistorys = _oEmployee.TPIs.Where(s => s.IsPromotion == true).ToList();


            string sSqlForEAN = "SELECT * FROM View_EmployeeActivityNote AS HH WHERE HH.EmployeeID=" + nEmployeeID + " AND ApproveBy > 0";
            _oEmployee.AcitivityNotes = EmployeeActivityNote.Gets(sSqlForEAN, (int)(Session[SessionInfo.currentUserID]));

            string sSqlForELL = "SELECT HH.[Session], HH.LeaveName, HH.TotalDay, ISNULL((SELECT SUM(DATEDIFF(DAY,MM.StartDateTime, MM.EndDateTime)+1) FROM LeaveApplication AS MM WHERE ISNULL(MM.CancelledBy,0)<=0 AND MM.EmpLeaveLedgerID=HH.EmpLeaveLedgerID),0) AS EnjoyLeave FROM View_EmployeeLeaveLedger AS HH WHERE HH.EmployeeID=" + nEmployeeID + " AND LeaveName IN ('Casual Leave','Sick Leave') ORDER BY [Session] DESC";
            _oEmployee.EmployeeLeaveLedgers = EmployeeLeaveLedger.Gets(sSqlForELL, (int)(Session[SessionInfo.currentUserID]));

            var queryResult = from ELL in _oEmployee.EmployeeLeaveLedgers group ELL by ELL.Session;

            List<EmployeeLeaveLedger> oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            foreach (var ELLGroup in queryResult)
            {
                EmployeeLeaveLedger oEmployeeLeaveLedger = new EmployeeLeaveLedger();
                oEmployeeLeaveLedger.Session = ELLGroup.Key;
                foreach (var ELL in ELLGroup)
                {
                    if (ELL.LeaveName == "Casual Leave")
                    {
                        oEmployeeLeaveLedger.CLDetailsInStr = "T-" + ELL.TotalDay + ", E-" + ELL.EnjoyLeave + ", B-" + (ELL.TotalDay - ELL.EnjoyLeave) + "";
                    }
                    else
                    {
                        oEmployeeLeaveLedger.SLDetailsInStr = "T-" + ELL.TotalDay + ", E-" + ELL.EnjoyLeave + ", B-" + (ELL.TotalDay - ELL.EnjoyLeave) + "";
                    }
                }
                oEmployeeLeaveLedgers.Add(oEmployeeLeaveLedger);
            }
            _oEmployee.EmployeeLeaveLedgers = oEmployeeLeaveLedgers;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptEmployeeInformation oReport = new rptEmployeeInformation();
            byte[] abytes = oReport.PrepareReport(_oEmployee, MonthlyAttendanceReports, oLeaveApplications, oCompany);
            return File(abytes, "application/pdf");
        }


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

        #endregion
    }
}