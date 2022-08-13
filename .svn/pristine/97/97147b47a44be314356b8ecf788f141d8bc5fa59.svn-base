using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class RosterPlanController : Controller
    {
        #region Declaration
        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        RosterPlan _oRosterPlan = new RosterPlan();
        private List<RosterPlan> _oRosterPlans = new List<RosterPlan>();
        RosterPlanDetail _oRosterPlanDetail = new RosterPlanDetail();
        List<RosterPlanDetail> _oRosterPlanDetails = new List<RosterPlanDetail>();
        List<TDepartment> _oTDepartments = new List<TDepartment>();
        RosterPlanEmployee _oRosterPlanEmployee = new RosterPlanEmployee();
        static List<RosterPlanEmployee> oRosterPlanEmployeeForErrors = new List<RosterPlanEmployee>();
        #endregion

        #region Functions
        private List<TDepartment> GetRoots(int nParentID)
        {
            TDepartment oTDepartment = new TDepartment();
            List<TDepartment> oTDepartments = new List<TDepartment>();
            foreach (TDepartment oItem in _oTDepartments)
            {
                if (oItem.parentid == nParentID)
                {
                    oTDepartments.Add(oItem);
                }
            }
            return oTDepartments;
        }
        private void AddTreeNodes(ref TDepartment oTDepartment)
        {
            List<TDepartment> oChildNodes = new List<TDepartment>();
            oChildNodes = GetChild(oTDepartment.id);
            if (oChildNodes.Count > 0)
            {
                oTDepartment.state = "closed";
            }
            oTDepartment.children = oChildNodes;
            foreach (TDepartment oItem in oChildNodes)
            {
                TDepartment oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private List<TDepartment> GetChild(int nParentCategoryID)
        {
            List<TDepartment> oTDepartments = new List<TDepartment>();
            foreach (TDepartment oItem in _oTDepartments)
            {
                if (oItem.parentid == nParentCategoryID)
                {
                    oTDepartments.Add(oItem);
                }
            }
            return oTDepartments;
        }

        private List<EmployeeRoster> GetsEmployeeRoster(RosterPlanEmployee oRosterPlanEmployee, List<EmployeeOfficial> oEmployeeOfficials, List<RosterPlanEmployee> oRosterPlanEmployees)
        {
            string sCellCaption = "";
            int nCount = 0; string sPropertyName = ""; DateTime dStartDate;            
            EmployeeRoster oEmployeeRoster = new EmployeeRoster(); 
            List<EmployeeRoster> oEmployeeRosters = new List<EmployeeRoster>();
            RosterPlanEmployee oTempRosterPlanEmployee = new RosterPlanEmployee();

            foreach (EmployeeOfficial oItem in oEmployeeOfficials)
            {
                nCount = 0;
                oEmployeeRoster = new EmployeeRoster();
                oEmployeeRoster.EmployeeID = oItem.EmployeeID;
                oEmployeeRoster.EmployeeCode = oItem.Code;
                oEmployeeRoster.EmployeeName = oItem.EmployeeName;
                dStartDate = oRosterPlanEmployee.StartDate;
                while (dStartDate <= oRosterPlanEmployee.EndDate)
                {
                    nCount++;
                    oTempRosterPlanEmployee = oRosterPlanEmployees.Find(x => x.EmployeeID == oItem.EmployeeID && x.AttendanceDate == dStartDate);
                    if (oTempRosterPlanEmployee != null)
                    {
                        if (oTempRosterPlanEmployee.RPEID > 0)
                        {
                            #region Set RPObj
                            sPropertyName = "RPObj" + nCount.ToString("00");
                            PropertyInfo prop = oEmployeeRoster.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                            if (null != prop && prop.CanWrite)
                            {

                                prop.SetValue(oEmployeeRoster, oTempRosterPlanEmployee, null);
                            }
                            #endregion

                            #region Set Cell Caption
                            sPropertyName = "Day" + nCount.ToString("00");
                            PropertyInfo propobj = oEmployeeRoster.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                            if (null != propobj && propobj.CanWrite)
                            {
                                sCellCaption = oTempRosterPlanEmployee.ShiftWithDuration;
                                propobj.SetValue(oEmployeeRoster, sCellCaption, null);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Set RPObj
                            sPropertyName = "RPObj" + nCount.ToString("00");
                            PropertyInfo prop = oEmployeeRoster.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                            if (null != prop && prop.CanWrite)
                            {
                                oTempRosterPlanEmployee = new RosterPlanEmployee();
                                oTempRosterPlanEmployee.EmployeeID = oItem.EmployeeID;
                                oTempRosterPlanEmployee.AttendanceDate = dStartDate;
                                prop.SetValue(oEmployeeRoster, oTempRosterPlanEmployee, null);
                            }
                            #endregion

                            #region Set Cell Caption
                            sPropertyName = "Day" + nCount.ToString("00");
                            PropertyInfo propobj = oEmployeeRoster.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                            if (null != propobj && propobj.CanWrite)
                            {
                                sCellCaption = "Roster N/A";
                                propobj.SetValue(oEmployeeRoster, sCellCaption, null);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region Set RPObj
                        sPropertyName = "RPObj" + nCount.ToString("00");
                        PropertyInfo prop = oEmployeeRoster.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            oTempRosterPlanEmployee = new RosterPlanEmployee();
                            oTempRosterPlanEmployee.EmployeeID = oItem.EmployeeID;
                            oTempRosterPlanEmployee.AttendanceDate = dStartDate;
                            prop.SetValue(oEmployeeRoster, oTempRosterPlanEmployee, null);
                        }
                        #endregion

                        #region Set Cell Caption
                        sPropertyName = "Day" + nCount.ToString("00");
                        PropertyInfo propobj = oEmployeeRoster.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            sCellCaption = "Roster N/A";
                            propobj.SetValue(oEmployeeRoster, sCellCaption, null);
                        }
                        #endregion
                    }
                    dStartDate = dStartDate.AddDays(1);
                }
                oEmployeeRosters.Add(oEmployeeRoster);
            }
            return oEmployeeRosters;
        }
        #endregion

        public ActionResult ViewRosterPlans(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "";
            _oRosterPlans = new List<RosterPlan>();
            sSQL = "select * from View_RosterPlan";
            _oRosterPlans = RosterPlan.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oRosterPlans);
        }

        public ActionResult ViewRosterPlan(string sid, string sMsg)
        {
            int nRPID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oRosterPlan = new RosterPlan();
            HRMShift oHrmShift = new HRMShift();
            if (nRPID > 0)
            {
                _oRosterPlan = _oRosterPlan.Get(nRPID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oRosterPlanDetails = RosterPlanDetail.Gets("SELECT * FROM View_RosterPlanDetail WHERE RosterPlanID=" + nRPID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oRosterPlan.RosterPlanDetails = _oRosterPlanDetails;
            }
            oHrmShift.Name = "--Select Shift--";
            List<HRMShift> oHrmShifts = new List<HRMShift>();
            oHrmShifts.Add(oHrmShift);
            oHrmShifts.AddRange(HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID));
            _oRosterPlan.Shifts = oHrmShifts;
            if (sMsg != "N/A")
            {
                _oRosterPlan.ErrorMessage = sMsg;
            }
            return View(_oRosterPlan);
        }

        [HttpPost]
        public JsonResult GetRosterPlanDetail(int nID)
        {
            List<RosterPlanDetail> oRosterPlanDetails = new List<RosterPlanDetail>();
            if (nID > 0)
            {
                oRosterPlanDetails = RosterPlanDetail.Gets("SELECT * FROM View_RosterPlanDetail WHERE RosterPlanID=" + nID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else
            {
                oRosterPlanDetails = RosterPlanDetail.Gets("SELECT * FROM View_RosterPlanDetail", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRosterPlanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RosterPlan_Insert(RosterPlan oRosterPlan)// CR Insert
        {
            _oRosterPlan = new RosterPlan();
            try
            {
                _oRosterPlan = oRosterPlan;
                _oRosterPlan = _oRosterPlan.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oRosterPlan = new RosterPlan();
                _oRosterPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRosterPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RosterPlan_Update(RosterPlan oRosterPlan)// CR Insert
        {
            _oRosterPlan = new RosterPlan();
            try
            {
                _oRosterPlan = oRosterPlan;
                _oRosterPlan = _oRosterPlan.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oRosterPlan = new RosterPlan();
                _oRosterPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRosterPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RosterPlanDelete(int id)//Roster Plan Delete
        {
            _oRosterPlan = new RosterPlan();
            try
            {

                _oRosterPlan.RosterPlanID = id;
                _oRosterPlan = _oRosterPlan.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oRosterPlan = new RosterPlan();
                _oRosterPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRosterPlan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RosterPlanDetailDelete(int id)//Roster Plan Delete
        {
            _oRosterPlanDetail = new RosterPlanDetail();
            try
            {

                _oRosterPlanDetail.RosterPlanDetailID = id;
                _oRosterPlanDetail = _oRosterPlanDetail.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oRosterPlan = new RosterPlan();
                _oRosterPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRosterPlanDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeActiveStatus(RosterPlan oRosterPlan)
        {
            _oRosterPlan = new RosterPlan();
            string sMsg;

            sMsg = _oRosterPlan.ChangeActiveStatus(oRosterPlan, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oRosterPlan = _oRosterPlan.Get(oRosterPlan.RosterPlanID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRosterPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Rostering (Transfer)
        public ActionResult View_RosterTransfers(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            ViewBag.HRMShifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);

            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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


            return View(oRosterPlanEmployee);
        }
        public ActionResult View_RosterTransfer()
        {
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            string sSql_R = "SELECT * FROM RosterPlan WHERE IsActive=1 AND RosterCycle>0  AND RosterPlanID IN(SELECT RosterPlanID  FROM RosterPlanDetail WHERE NextShiftID>0)";
            ViewBag.RosterPlans = RosterPlan.Gets(sSql_R, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sSql_RD = "SELECT * FROM View_RosterPlanDetail WHERE  NextShiftID>0 AND RosterPlanID IN (SELECT RosterPlanID FROM RosterPlan WHERE IsActive=1 AND RosterCycle>0)";
            ViewBag.RosterPlanDetails = RosterPlanDetail.Gets(sSql_RD, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
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

            return View(oRosterPlanEmployee);
        }
        public ActionResult View_RosterTransferDept()
        {
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            string sSql_R = "SELECT * FROM RosterPlan WHERE IsActive=1 AND RosterCycle>0  AND RosterPlanID IN(SELECT RosterPlanID  FROM RosterPlanDetail WHERE NextShiftID>0)";
            ViewBag.RosterPlans = RosterPlan.Gets(sSql_R, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql_RD = "SELECT * FROM View_RosterPlanDetail WHERE  NextShiftID>0 AND RosterPlanID IN (SELECT RosterPlanID FROM RosterPlan WHERE IsActive=1 AND RosterCycle>0)";
            ViewBag.RosterPlanDetails = RosterPlanDetail.Gets(sSql_RD, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(oRosterPlanEmployee);
        }
        [HttpPost]
        public JsonResult GetsShiftRostering(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, double ts)
        {
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();
            try
            {
                oRosterPlanEmployees = RosterPlanEmployee.Gets(EmployeeIDs, ShiftID, StartDate, EndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
                oRosterPlanEmployee.ErrorMessage = ex.Message;
                oRosterPlanEmployees = new List<RosterPlanEmployee>();
                oRosterPlanEmployees.Add(oRosterPlanEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRosterPlanEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RosterTransfer_Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, DateTime MaxOTDateTime, bool IsDayOff)
        {
            int nOT_In_Minute = ((MaxOTDateTime.Hour * 60) + MaxOTDateTime.Minute);

            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();
            try
            {
                oRosterPlanEmployees = oRosterPlanEmployee.Transfer(EmployeeIDs, ShiftID, StartDate, EndDate, nOT_In_Minute, IsDayOff, (int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oRosterPlanEmployee = new RosterPlanEmployee();
                oRosterPlanEmployees = new List<RosterPlanEmployee>();
                oRosterPlanEmployee.ErrorMessage = ex.Message;
                oRosterPlanEmployees.Add(oRosterPlanEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRosterPlanEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RosterTransfer_TransferDept(RosterPlanEmployee oRPE)
        {
            DateTime StartDate = Convert.ToDateTime(oRPE.ErrorMessage.Split('~')[0]);
            DateTime EndDate= Convert.ToDateTime(oRPE.ErrorMessage.Split('~')[1]);
            int BUID= Convert.ToInt32(oRPE.ErrorMessage.Split('~')[2]);
            string LocationIDs= oRPE.ErrorMessage.Split('~')[3];
            string DepartmentIDs= oRPE.ErrorMessage.Split('~')[4];
            int ShiftID= Convert.ToInt32(oRPE.ErrorMessage.Split('~')[5]);
            DateTime InTime= Convert.ToDateTime(oRPE.ErrorMessage.Split('~')[6]);
            DateTime OutTime= Convert.ToDateTime(oRPE.ErrorMessage.Split('~')[7]);
            bool IsGWD= Convert.ToBoolean(oRPE.ErrorMessage.Split('~')[8]);
            bool IsDayOff= Convert.ToBoolean(oRPE.ErrorMessage.Split('~')[9]);
            string Remarks= oRPE.ErrorMessage.Split('~')[10];
            DateTime MaxOTDateTime= Convert.ToDateTime(oRPE.ErrorMessage.Split('~')[11]);
            string EmployeeIDs= oRPE.ErrorMessage.Split('~')[12];
            string DesignationIDs= oRPE.ErrorMessage.Split('~')[13];
            bool isOfficial= Convert.ToBoolean(oRPE.ErrorMessage.Split('~')[14]);
            DateTime RosterDate= Convert.ToDateTime(oRPE.ErrorMessage.Split('~')[15]);
            string GroupIDs= oRPE.ErrorMessage.Split('~')[16];
            string BlockIDs= oRPE.ErrorMessage.Split('~')[17];
            int TrsShiftID = Convert.ToInt32(oRPE.ErrorMessage.Split('~')[18]);

            int nOT_In_Minute = ((MaxOTDateTime.Hour * 60) + MaxOTDateTime.Minute);
            
            bool bFeedBack = true;

            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();
            try
            {
                oRosterPlanEmployees = oRosterPlanEmployee.TransferDept(StartDate, EndDate, BUID, LocationIDs, DepartmentIDs, ShiftID, InTime, OutTime, IsGWD, IsDayOff, Remarks, nOT_In_Minute, EmployeeIDs, DesignationIDs, isOfficial, RosterDate, GroupIDs, BlockIDs, TrsShiftID, (int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
                oRosterPlanEmployeeForErrors = oRosterPlanEmployees;

                if (oRosterPlanEmployees.Count > 0)
                {
                    bFeedBack = false;
                }


            }
            catch (Exception ex){}
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(new { FeedBackResponse = bFeedBack});
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void PrintErrorList(double ts)
        {
            if (oRosterPlanEmployeeForErrors.Count > 0)
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

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex += 1;

                    foreach (RosterPlanEmployee oItem in oRosterPlanEmployeeForErrors)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateInString; cell.Style.Font.Bold = false;
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
            oRosterPlanEmployeeForErrors = new List<RosterPlanEmployee>();
        }

        [HttpPost]
        public JsonResult RosterTransfer_Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate)
        {
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();
            try
            {
                oRosterPlanEmployees = oRosterPlanEmployee.Swap(RosterPlanID, StartDate, EndDate, (int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oRosterPlanEmployee = new RosterPlanEmployee();
                oRosterPlanEmployees = new List<RosterPlanEmployee>();
                oRosterPlanEmployee.ErrorMessage = ex.Message;
                oRosterPlanEmployees.Add(oRosterPlanEmployee);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRosterPlanEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchWithPagination(string sIDs, string dtDateFrom, string dtDateTo, double ts)
        {
            List<RosterPlanEmployee> RosterTransfers = new List<RosterPlanEmployee>();
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM View_RosterTransfer WHERE EmployeeID IN(" + sIDs + ") AND [DATE] BETWEEN '" + dtDateFrom + "' AND '" + dtDateTo + "'";

                RosterTransfers = RosterPlanEmployee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (RosterTransfers.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                RosterTransfers = new List<RosterPlanEmployee>();
                RosterPlanEmployee RosterTransfer = new RosterPlanEmployee();
                RosterTransfer.ErrorMessage = ex.Message;
                RosterTransfers.Add(RosterTransfer);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(RosterTransfers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }                
        [HttpPost]
        public JsonResult GetsDepartments(Department oDepartment)
        {
            List<Department> oDepartments = new List<Department>();
            oDepartments = Department.Gets((int)(Session[SessionInfo.currentUserID]));

            _oTDepartments = new List<TDepartment>();
            TDepartment oTDepartment = new TDepartment();

            foreach (Department oItem in oDepartments)
            {
                oTDepartment = new TDepartment();
                oTDepartment.id = oItem.DepartmentID;
                oTDepartment.parentid = oItem.ParentID;
                oTDepartment.text = oItem.Name;
                oTDepartment.attributes = "";
                oTDepartment.code = oItem.Code;
                oTDepartment.sequence = oItem.Sequence;
                oTDepartment.requiredPerson = oItem.RequiredPerson;
                oTDepartment.Description = oItem.Description;
                _oTDepartments.Add(oTDepartment);
            }
            List<TDepartment> oTDepartments = new List<TDepartment>();
            oTDepartments = this.GetRoots(1);
            foreach (TDepartment oItem in oTDepartments)
            {
                TDepartment oTempTDepartment = oItem;
                this.AddTreeNodes(ref oTempTDepartment);
            }

            #region Combo Box Caption
            oTDepartment = new TDepartment();
            oTDepartment.id = 0;
            oTDepartment.parentid = 0;
            oTDepartment.text = "--Select Department--";
            oTDepartment.attributes = "";
            oTDepartment.code = "";
            oTDepartment.sequence = 0;
            oTDepartment.requiredPerson = 0;
            oTDepartment.Description = "";
            oTDepartments.Add(oTDepartment);
            #endregion

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oTDepartments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchRosterPlan(RosterPlanEmployee oRosterPlanEmployee)
        {
            _oRosterPlanEmployee = new RosterPlanEmployee();
            List<EmployeeRoster> oEmployeeRosters = new List<EmployeeRoster>();
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();
            List<EmployeeOfficial> oEmployeeOfficials = new List<EmployeeOfficial>();
            if (oRosterPlanEmployee.EmployeeName == null) oRosterPlanEmployee.EmployeeName = "";
            try
            {
                #region Gets Employees
                string sQuerySQL = "SELECT ROW_NUMBER() OVER (ORDER BY Code) AS RowNum, * FROM View_EmployeeOfficialALL AS HH";
                string sRosterSQL = "SELECT ROW_NUMBER() OVER (ORDER BY Code) AS RowNum, * FROM View_EmployeeOfficialALL AS HH";
                string sTotalCountSQl = "SELECT COUNT(*) AS TotalRecordCount FROM View_EmployeeOfficialALL AS HH";
                string sReturn = "";

                #region Department
                if (oRosterPlanEmployee.DepartmentID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.DepartmentID IN (SELECT DD.DepartmentID FROM [dbo].[Fn_GetsDepartmnetByParent](" + oRosterPlanEmployee.DepartmentID.ToString() + ") AS DD)";
                }
                #endregion

                #region Employee Name
                if (oRosterPlanEmployee.EmployeeName != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (HH.Name LIKE '%" + oRosterPlanEmployee.EmployeeName + "%' OR  HH.Code LIKE '%" + oRosterPlanEmployee.EmployeeName + "%')";
                }
                #endregion

                if (oRosterPlanEmployee.IsRostered == true)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "(HH.EmployeeID IN (SELECT TT.EmployeeID FROM RosterPlanEmployee WITH(NOLOCK) AS TT WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),TT.AttendanceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRosterPlanEmployee.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRosterPlanEmployee.EndDate.ToString("dd MMM yyyy") + "',106))))";
                }

                #region Deafult Parameter
                if (oRosterPlanEmployee.ShiftID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "(HH.EmployeeID IN (SELECT TT.EmployeeID FROM RosterPlanEmployee WITH (NOLOCK) AS TT WHERE TT.ShiftID=" + oRosterPlanEmployee.ShiftID.ToString() + " AND CONVERT(DATE,CONVERT(VARCHAR(12),TT.AttendanceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRosterPlanEmployee.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRosterPlanEmployee.EndDate.ToString("dd MMM yyyy") + "',106))))";
                }
                else if (oRosterPlanEmployee.IsRostered != true)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + "(HH.IsActive=1 OR HH.EmployeeID IN (SELECT TT.EmployeeID FROM RosterPlanEmployee AS TT WHERE CONVERT(DATE,CONVERT(VARCHAR(12),TT.AttendanceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRosterPlanEmployee.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRosterPlanEmployee.EndDate.ToString("dd MMM yyyy") + "',106))))";
                    sReturn = sReturn + "(HH.IsActive=1)";
                }
                #endregion

                sTotalCountSQl = sTotalCountSQl + sReturn;
                sQuerySQL = sQuerySQL + sReturn;
                sRosterSQL = sRosterSQL + sReturn;

                #region Rows Number
                if (oRosterPlanEmployee.EmployeeName == "")
                {
                    sQuerySQL = "SELECT * FROM (" + sQuerySQL + ") AS TT WHERE TT.RowNum Between " + (oRosterPlanEmployee.LastRowNum + 1).ToString() + " AND " + (oRosterPlanEmployee.LastRowNum + oRosterPlanEmployee.RecordCount).ToString();
                    sRosterSQL = "SELECT TT.EmployeeID FROM (" + sRosterSQL + ") AS TT WHERE TT.RowNum Between " + (oRosterPlanEmployee.LastRowNum + 1).ToString() + " AND " + (oRosterPlanEmployee.LastRowNum + oRosterPlanEmployee.RecordCount).ToString();
                }
                else
                {
                    sQuerySQL = "SELECT * FROM (" + sQuerySQL + ") AS TT";
                    sRosterSQL = "SELECT TT.EmployeeID FROM (" + sRosterSQL + ") AS TT";
                }
                #endregion

                oEmployeeOfficials = EmployeeOfficial.Gets(sQuerySQL, (int)Session[SessionInfo.currentUserID]);
                #endregion

                if (oEmployeeOfficials.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }

                #region Gets Roster Plans
                string sSQL = "SELECT * FROM View_RosterPlanEmployee AS KK";
                sReturn = "";

                #region Employee
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " KK.EmployeeID IN(" + sRosterSQL + ")";
                #endregion

                #region Date Range
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),KK.AttendanceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRosterPlanEmployee.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oRosterPlanEmployee.EndDate.ToString("dd MMM yyyy") + "',106))";
                #endregion

                sSQL = sSQL + sReturn;
                oRosterPlanEmployees = RosterPlanEmployee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                #endregion

                int nTotalRecords = EmployeeOfficial.TotalRecordCount(sTotalCountSQl, (int)Session[SessionInfo.currentUserID]);
                _oRosterPlanEmployee.EmployeeRosters = this.GetsEmployeeRoster(oRosterPlanEmployee, oEmployeeOfficials, oRosterPlanEmployees);
                _oRosterPlanEmployee.StartDate = oRosterPlanEmployee.StartDate;
                _oRosterPlanEmployee.EndDate = oRosterPlanEmployee.EndDate;
                _oRosterPlanEmployee.RecordCount = oRosterPlanEmployee.RecordCount;
                _oRosterPlanEmployee.LastRowNum = oEmployeeOfficials[oEmployeeOfficials.Count - 1].RowNum;
                _oRosterPlanEmployee.TotalRecord = "Item Count : " + (oRosterPlanEmployee.LastRowNum + oEmployeeOfficials.Count) + " of " + nTotalRecords.ToString();
            }
            catch (Exception ex)
            {
                _oRosterPlanEmployee = new RosterPlanEmployee();
                _oRosterPlanEmployee.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRosterPlanEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsRosterPlan(RosterPlanEmployee oRosterPlan)
        {
            _oRosterPlanEmployee = new RosterPlanEmployee();
            if (oRosterPlan.RPEID > 0)
            {
                _oRosterPlanEmployee = RosterPlanEmployee.Get(oRosterPlan.RPEID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                HRMShift oHRMShift = new HRMShift();
                EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
                oEmployeeOfficial = oEmployeeOfficial.GetByEmployee(oRosterPlan.EmployeeID, (int)Session[SessionInfo.currentUserID]);
                oHRMShift = oHRMShift.Get(oEmployeeOfficial.CurrentShiftID, (int)Session[SessionInfo.currentUserID]);

                _oRosterPlanEmployee.RPEID = 0;
                _oRosterPlanEmployee.EmployeeID = oEmployeeOfficial.EmployeeID;
                _oRosterPlanEmployee.ShiftID = oEmployeeOfficial.CurrentShiftID;

                _oRosterPlanEmployee.IsDayOff = false;
                _oRosterPlanEmployee.IsHoliday = false;
                _oRosterPlanEmployee.InTime = oHRMShift.StartTime;
                _oRosterPlanEmployee.OutTime = oHRMShift.EndTime;
                _oRosterPlanEmployee.AttendanceDate = oRosterPlan.AttendanceDate;
                _oRosterPlanEmployee.ShiftStartTime = oHRMShift.StartTime;
                _oRosterPlanEmployee.ShiftEndTime = oHRMShift.EndTime;
                _oRosterPlanEmployee.EmployeeCode = oEmployeeOfficial.Code;
                _oRosterPlanEmployee.EmployeeName = oEmployeeOfficial.EmployeeName;
                _oRosterPlanEmployee.ShiftName = oEmployeeOfficial.CurrentShiftName;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oRosterPlanEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRosterPlanEmployee(RosterPlanEmployee oRosterPlanEmployee)
        {
            _oRosterPlanEmployee = new RosterPlanEmployee();
            try
            {
                _oRosterPlanEmployee = oRosterPlanEmployee;
                if (_oRosterPlanEmployee.RPEID > 0)
                {
                    _oRosterPlanEmployee = _oRosterPlanEmployee.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oRosterPlanEmployee = _oRosterPlanEmployee.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oRosterPlanEmployee = new RosterPlanEmployee();
                _oRosterPlanEmployee.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oRosterPlanEmployee);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion




        [HttpPost]
        public JsonResult SearchByNameOrCode(Employee oEmp)
        {
            _oEmployees = new List<Employee>();
            string Ssql = "SELECT * FROM View_Employee WHERE (Name LIKE '%" + oEmp.Name + "%'" + " OR Code LIKE '%" + oEmp.Name + "%')" + " AND IsActive = 1";
            try
            {
                _oEmployees = Employee.Gets(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployees.Count <= 0)
                {
                    throw new Exception("Employee Is Not Active!");
                }
            }
            catch (Exception ex)
            {
                _oEmployee = new Employee();
                _oEmployees.Add(_oEmployee);
                _oEmployees[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region Upload Excel


        private List<RosterPlanEmployee> GetRosterFromExcel(HttpPostedFileBase PostedFile, DateTime dtStartDate, DateTime dtEndDate)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    PostedFile.SaveAs(fileDirectory);
                    string excelConnectionString = string.Empty;
                    //connection String for xls file format.
                    //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }
                    excelConnection.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                    oRows = ds.Tables[0].Rows;

                    string sRosterPlanEmployee = "";
                    DateTime dTempStartDate;
                    //DateTime dStartDate = Convert.ToDateTime("26 Dec 2015");  //Convert.ToDateTime(oRows[0][2] == DBNull.Value ? DateTime.Today : oRows[0][2]);
                    //DateTime dEndDate = Convert.ToDateTime("25 Jan 2016");  //Convert.ToDateTime(oRows[0][3] == DBNull.Value ? DateTime.Today : oRows[0][3]);

                    //DateTime dStartDate = Convert.ToDateTime(oRows[oRows.Count-1][2] == DBNull.Value ? DateTime.Today : oRows[oRows.Count-1][2]);
                    //DateTime dEndDate = dtEndDate == DBNull.Value ? DateTime.Today : dtEndDate;

                    for (int i = 0; i < oRows.Count; i++)
                    {
                        dTempStartDate = dtStartDate;
                        oRosterPlanEmployee = new RosterPlanEmployee();
                        oRosterPlanEmployee.EmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        int j = 1;
                        sRosterPlanEmployee = "";
                        while (dTempStartDate <= dtEndDate)
                        {
                            sRosterPlanEmployee = sRosterPlanEmployee + Convert.ToString(oRows[i][j] == DBNull.Value ? "BLANK" : oRows[i][j]) + ",";
                            dTempStartDate = dTempStartDate.AddDays(1);
                            j = j + 1;
                        }

                        if (sRosterPlanEmployee.Length > 0)
                        {
                            sRosterPlanEmployee = sRosterPlanEmployee.Remove(sRosterPlanEmployee.Length - 1, 1);
                        }
                        oRosterPlanEmployee.ErrorMessage = sRosterPlanEmployee;
                        oRosterPlanEmployee.AttendanceDate = dtStartDate;
                        oRosterPlanEmployees.Add(oRosterPlanEmployee);
                    }
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }
            return oRosterPlanEmployees;
        }


        [HttpPost]
        public ActionResult View_RosterTransfers(HttpPostedFileBase fileRosters, DateTime dtStartDateLoader, DateTime dtEndDateLoader)
        {
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();
            List<RosterPlanEmployee> oRPEs = new List<RosterPlanEmployee>();
            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            try
            {
                if (fileRosters == null) { throw new Exception("File not Found"); }
                oRosterPlanEmployees = this.GetRosterFromExcel(fileRosters, dtStartDateLoader, dtEndDateLoader);
                oRPEs = RosterPlanEmployee.UploadRosterEmpXL(oRosterPlanEmployees, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oRPEs.Count <= 0)
                {
                    throw new Exception("Nothing to upload. Please check the file formation.");
                }
                if (oRPEs.Count > 0 && oRPEs[0].ErrorMessage != "")
                {
                    throw new Exception(oRPEs[0].ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                ViewBag.FeedBackText = "Unsuccessful";
                oRPEs = new List<RosterPlanEmployee>();
                ViewBag.HRMShifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
                if(oRPEs.Count > 0)
                    return View(oRPEs[0]);
                else
                {

                    RosterPlanEmployee oRPE = new RosterPlanEmployee();
                    return View(oRPE);
                }
            }
            ViewBag.HRMShifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FeedBackText = "successful";
            return View(oRPEs[0]);
            //return RedirectToAction("ImportAttendanceFromExcel", "AttendanceUpload_XL", new { menuid = (int)Session[SessionInfo.MenuID] });
        }

        #endregion

        #region Download Format
        public void SampleFormatDownload()
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Format Downlaod");
                sheet.Name = "Format Downlaod";

                int n = 1;
                sheet.Column(n++).Width = 15;//code
                sheet.Column(n++).Width = 15;//Date
                sheet.Column(n++).Width = 15;//Date
                sheet.Column(n++).Width = 15;//Date
                sheet.Column(n++).Width = 15;//Date
                sheet.Column(n++).Width = 15;//Date
                sheet.Column(n++).Width = 200;//Date


                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "20-Feb-2018"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "21-Feb-2018"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "22-Feb-2018"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "23-Feb-2018"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "24-Feb-2018"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "10001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-A"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-B"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OFF"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-C"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-D"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "IF only OFF will give then shift will be taken from official"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "10002"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-C"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-A"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-D"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OFF"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-E"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "IF only OFF will give then shift will be taken from official"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "10003"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-C"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OFF/Shift-A"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-D"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OFF"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Shift-E"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "IF OFF/Shift then for this date dayoff will be counted for this shift"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Format.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion


        #region RosterPlanEmployeeExcel
        public void ExcelRosterPlanEmployee(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds)
        {
            DateTime StartDate = DateFrom;
            DateTime EndDate = DateTo;
            DateTime StartDateN = DateFrom;
            DateTime EndDateN = DateTo;

            RosterPlanEmployee oRosterPlanEmployee = new RosterPlanEmployee();
            List<RosterPlanEmployee> oRosterPlanEmployees = new List<RosterPlanEmployee>();

            oRosterPlanEmployees = RosterPlanEmployee.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, "", sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, "", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 1;

            int nDays = (int)(DateTo - DateFrom).TotalDays + 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Roster Plan");
                sheet.Name = "Employee Roster Plan";

                int n = 1;
                sheet.Column(n++).Width = 15;//code
                for (int i = 1; i <= nDays; i++)
                {
                    sheet.Column(n++).Width = 15;//date
                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                DateTo = DateTo.AddDays(1);
                while (DateFrom.ToString("dd MMM yyyy") != DateTo.ToString("dd MMM yyyy"))
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = DateFrom.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    DateFrom = DateFrom.AddDays(1);
                }
                rowIndex += 1;

                bool flag = false;
                int EmpID = 0;
                foreach (RosterPlanEmployee oItem in oRosterPlanEmployees)
                {
                    if (EmpID == oItem.EmployeeID)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                    EmpID = oItem.EmployeeID;
                    if (!flag)
                    {

                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        EndDate = EndDate.AddDays(1);
                        while (StartDate.ToString("dd MMM yyyy") != EndDate.ToString("dd MMM yyyy"))
                        {
                            oRosterPlanEmployee = oRosterPlanEmployees.Where(x => x.EmployeeID == oItem.EmployeeID && x.AttendanceDate.ToString("dd MMM yyyy") == StartDate.ToString("dd MMM yyyy")).FirstOrDefault();

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oRosterPlanEmployee == null) ? "" : oRosterPlanEmployee.ShiftName; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            StartDate = StartDate.AddDays(1);
                        }
                        rowIndex += 1;

                        StartDate = StartDateN;
                        EndDate = EndDateN;
                    }
                }



                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeRosterPlan.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        

        #endregion

    }
}
