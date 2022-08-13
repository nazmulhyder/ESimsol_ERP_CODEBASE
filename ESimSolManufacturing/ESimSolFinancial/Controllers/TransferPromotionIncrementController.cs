using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Drawing.Imaging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;

namespace ESimSolFinancial.Controllers
{
    public class TransferPromotionIncrementController : Controller
    {
        #region Declaration
        TransferPromotionIncrement _oTransferPromotionIncrement;
        private List<TransferPromotionIncrement> _oTransferPromotionIncrements;

        private static List<TransferPromotionIncrement> _oErrorListTPI = new List<TransferPromotionIncrement>();

        private List<SalaryScheme> _oSalarySchemes;
        #endregion

        #region Views
        public ActionResult View_TPIs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            List<AuthorizationUserOEDO> oAUOEDOs = new List<AuthorizationUserOEDO>();
            //oAUOEDOs = AuthorizationUserOEDO.GetsByUser(((User)(Session[SessionInfo.CurrentUser])).UserID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //bool bAdd = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Add, "TransferPromotionIncrement", oAUOEDOs);
            //bool bEdit = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Edit, "TransferPromotionIncrement", oAUOEDOs);
            //bool bDelete = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Delete, "TransferPromotionIncrement", oAUOEDOs);
            //bool bRecommend = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Accept, "TransferPromotionIncrement", oAUOEDOs);
            //bool bApprove = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Approve, "TransferPromotionIncrement", oAUOEDOs);
            //bool bEffect = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._Lock, "TransferPromotionIncrement", oAUOEDOs);
            //bool bPreview = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._View, "TransferPromotionIncrement", oAUOEDOs);
            //bool bAdvSearch = ((User)(Session[SessionInfo.CurrentUser])).HasFunctionalityWeb(EnumOperationFunctionality._View, "TransferPromotionIncrement", oAUOEDOs);

            //TempData["Add"] = bAdd;
            //TempData["Edit"] = bEdit;
            //TempData["Delete"] = bDelete;
            //TempData["Recommend"] = bRecommend;
            //TempData["Approve"] = bApprove;
            //TempData["Effect"] = bEffect;
            //TempData["Preview"] = bPreview;
            //TempData["AdvSearch"] = bAdvSearch;

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            sSql = "";
            sSql = "SELECT*FROM View_TransferPromotionIncrement WHERE ISNULL(RecommendedBy,0)<=0 AND ISNULL(ApproveBy,0)<=0 ";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }
            //_oTransferPromotionIncrements = TransferPromotionIncrement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oTransferPromotionIncrements);
        }

        public ActionResult View_TPI(string sid, string sMsg)//nId=TPIID
        {
            int nTPIID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            if (nTPIID > 0)
            {
                _oTransferPromotionIncrement = TransferPromotionIncrement.Get(nTPIID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            if (sMsg != "N/A")
            {
                _oTransferPromotionIncrement.ErrorMessage = sMsg;
            }
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
            return View(_oTransferPromotionIncrement);
        }
        public ActionResult View_IncrementByPercents(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            ViewBag.SalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType=1 AND IsActive=1", (int)(Session[SessionInfo.currentUserID]));

            List<int> oYears = new List<int>();
            int nCurrentYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            int nStartRange = nCurrentYear - 30;
            int nEndRange = nCurrentYear + 5;
            for (int i = nStartRange; i <= nEndRange; i++)
            {
                oYears.Add(i);
            }
            ViewBag.Years = oYears;

            return View(_oTransferPromotionIncrements);
        }

        public ActionResult View_SalaryStructure(string sTemp, double ts)
        {

            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            oEmployeeSalaryStructure.EmployeeID = Convert.ToInt32(sTemp.Split('~')[0]);
            oEmployeeSalaryStructure.EmployeeName = sTemp.Split('~')[1];
            oEmployeeSalaryStructure.EmployeeTypeName = sTemp.Split('~')[2];
            oEmployeeSalaryStructure.SalarySchemeID = Convert.ToInt32(sTemp.Split('~')[3]);
            oEmployeeSalaryStructure.SalarySchemeName = sTemp.Split('~')[4];
            oEmployeeSalaryStructure.ErrorMessage = sTemp.Split('~')[5];
            oEmployeeSalaryStructure.GrossAmount = Convert.ToDouble(sTemp.Split('~')[6]);
            oEmployeeSalaryStructure.CurrencyID = Convert.ToInt32(sTemp.Split('~')[7]);

            return PartialView(oEmployeeSalaryStructure);
        }
        #endregion

        #region IUD
        [HttpPost]
        public JsonResult TransferPromotionIncrement_IU(TransferPromotionIncrement oTransferPromotionIncrement)
        {

            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            try
            {
                _oTransferPromotionIncrement = oTransferPromotionIncrement;
                if (_oTransferPromotionIncrement.TPIID > 0)
                {
                    _oTransferPromotionIncrement = _oTransferPromotionIncrement.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oTransferPromotionIncrement = _oTransferPromotionIncrement.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TransferAttScheme(TransferPromotionIncrement oTransferPromotionIncrement)
        {

            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            try
            {
                _oTransferPromotionIncrement = oTransferPromotionIncrement;
                if (_oTransferPromotionIncrement.TPIID > 0)
                {
                    _oTransferPromotionIncrement = _oTransferPromotionIncrement.AttScheme((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oTransferPromotionIncrement = _oTransferPromotionIncrement.AttScheme((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TPI_IUQuick(TransferPromotionIncrement oTransferPromotionIncrement)
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            List<Employee> oEmployees = new List<Employee>();
            Employee oEmp = new Employee();
            string sSQL = "SELECT * FROM View_Employee WHERE EmployeeID IN (" + oTransferPromotionIncrement.EmployeeIDs + ")";
            try
            {
                _oTransferPromotionIncrement = oTransferPromotionIncrement.IUDQuick((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oTransferPromotionIncrement.ErrorMessage == "")
                {
                    oEmployees = Employee.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oEmployees = new List<Employee>();
                    oEmp.ErrorMessage = _oTransferPromotionIncrement.ErrorMessage;
                    oEmployees.Add(oEmp);
                }

            }
            catch (Exception ex)
            {
                oEmployees = new List<Employee>();
                oEmp.ErrorMessage = ex.Message;
                oEmployees.Add(oEmp);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult TransferPromotionIncrement_Delete(int nId, double ts)//nId=TPIID
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            try
            {
                _oTransferPromotionIncrement.TPIID = nId;
                _oTransferPromotionIncrement = _oTransferPromotionIncrement.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrement.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GetDRP
        [HttpPost]
        public JsonResult GetDRP(DepartmentRequirementPolicy oDPI)
        {
            int DRPId = oDPI.DepartmentRequirementPolicyID;
            DepartmentRequirementPolicy oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
            try
            {
                oDepartmentRequirementPolicy = oDepartmentRequirementPolicy.Get(DRPId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                oDepartmentRequirementPolicy.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDepartmentRequirementPolicy);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Search
        [HttpPost]
        public JsonResult SearchByName(TransferPromotionIncrement oTPI)
        {
            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            string Ssql = "SELECT * FROM View_TransferPromotionIncrement WHERE EmployeeName LIKE '%" + oTPI.EmployeeName + "%'" + " OR EmployeeCode LIKE '%" + oTPI.EmployeeName + "%'";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                Ssql = Ssql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }
            try
            {
                _oTransferPromotionIncrements = TransferPromotionIncrement.Gets(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oTransferPromotionIncrements.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrements.Add(_oTransferPromotionIncrement);
                _oTransferPromotionIncrements[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByEmployeeName(TransferPromotionIncrement oTPI)
        {
            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            string Ssql = "SELECT * FROM View_TransferPromotionIncrement WHERE EmployeeID IN (" + oTPI.IDs + ")";
            try
            {
                _oTransferPromotionIncrements = TransferPromotionIncrement.Gets(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oTransferPromotionIncrements.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrements.Add(_oTransferPromotionIncrement);
                _oTransferPromotionIncrements[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Recommend,approve
        [HttpPost]
        public JsonResult TransferPromotionIncrement_Recommend(TransferPromotionIncrement oTPI)
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            try
            {
                _oTransferPromotionIncrement = TransferPromotionIncrement.Recommend(oTPI.TPIID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TransferPromotionIncrement_Approve(TransferPromotionIncrement oTPI)
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            try
            {
                _oTransferPromotionIncrement = TransferPromotionIncrement.Approve(oTPI.TPIID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Undo_Approve(TransferPromotionIncrement oTransferPromotionIncrement)
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            try
            {
                _oTransferPromotionIncrement = oTransferPromotionIncrement.IUD((int)EnumDBOperation.UnApproval, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Undo_Recommend(TransferPromotionIncrement oTransferPromotionIncrement)
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            try
            {
                _oTransferPromotionIncrement = oTransferPromotionIncrement.IUD((int)EnumDBOperation.Undo, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region DA Approve Unapprove
        [HttpPost]
        public JsonResult TPI_MultipleApprove(TransferPromotionIncrement oTransferPromotionIncrement)
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            try
            {
                _oTransferPromotionIncrements = TransferPromotionIncrement.MultipleApprove(oTransferPromotionIncrement, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oTransferPromotionIncrements.Count > 0 && _oTransferPromotionIncrements[0].ErrorMessage != "")
                {
                    throw new Exception(_oTransferPromotionIncrements[0].ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
                _oTransferPromotionIncrements.Add(_oTransferPromotionIncrement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion DA Approve Unapprove

        #endregion

        #region GetAttendanceScheme
        [HttpPost]
        public JsonResult GetAttScheme(int nID)
        {
            AttendanceScheme oAttendanceScheme = new AttendanceScheme();
            try
            {
                oAttendanceScheme = oAttendanceScheme.Get(nID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oAttendanceScheme = new AttendanceScheme();
                oAttendanceScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Effect
        public ActionResult View_Effect(int nId, double ts)//nId=TPIID
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            _oTransferPromotionIncrement.TPIID = nId;
            return PartialView(_oTransferPromotionIncrement);
        }

        [HttpPost]
        public JsonResult TransferPromotionIncrement_Effect(TransferPromotionIncrement oTPI)
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            try
            {
                _oTransferPromotionIncrement = oTPI;
                _oTransferPromotionIncrement = _oTransferPromotionIncrement.Effect(((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion Effect

        #region Multiple Increment
        public ActionResult View_MultipleIncrement(double ts)
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            return PartialView(oEmployeeSalaryStructure);
        }

        [HttpPost]
        public JsonResult MultipleIncrement(EmployeeSalaryStructure oEmployeeSalaryStructure)
        {
            List<EmployeeSalaryStructure> oEmpSalaryStructures = new List<EmployeeSalaryStructure>();
            try
            {
                oEmpSalaryStructures = oEmployeeSalaryStructure.MultipleIncrement(oEmployeeSalaryStructure.ErrorMessage, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                EmployeeSalaryStructure oEmpSalaryStructure = new EmployeeSalaryStructure();
                oEmpSalaryStructures = new List<EmployeeSalaryStructure>();
                oEmpSalaryStructure.ErrorMessage = ex.Message;
                oEmpSalaryStructures.Add(oEmpSalaryStructure);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmpSalaryStructures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion  Multiple Increment

        #region Report
        public ActionResult PrintPromotionLetter_MAMIYA(int nEmpID, int nTPIID, double ts)
        {
            Employee oEmployee = new Employee();
            EmployeeOfficial oEmployeeOfficial = new EmployeeOfficial();
            string sSql = "SELECT * FROM View_EmployeeOfficialALL	WHERE EmployeeID=" + nEmpID;
            oEmployee.EmployeeOfficial = EmployeeOfficial.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sSql_TPI = "SELECT * FROM View_TransferPromotionIncrement WHERE TPIID=" + nTPIID;
            oEmployee.TPIs = TransferPromotionIncrement.Gets(sSql_TPI, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployee.Company = oCompanys.First();
            rptPromotionLetter_MAMIYA oReport = new rptPromotionLetter_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oEmployee);
            return File(abytes, "application/pdf");
        }

        #endregion Report

        #region Import & Export
        private List<TransferPromotionIncrement> GetTPFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<TransferPromotionIncrement> oTPIXLs = new List<TransferPromotionIncrement>();
            TransferPromotionIncrement oTPIXL = new TransferPromotionIncrement();
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
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

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

                    for (int i = 0; i < oRows.Count; i++)
                    {
                        oTPIXL = new TransferPromotionIncrement();
                        oTPIXL.EmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        //if (oTPIXL.EmployeeCode != "" && oTPIXL.EmployeeCode != null)
                        //{
                            oTPIXL.BUCode = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                            oTPIXL.LocCode = Convert.ToString(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);
                            oTPIXL.DeptCode = Convert.ToString(oRows[i][3] == DBNull.Value ? "" : oRows[i][3]);
                            oTPIXL.AttSchemeName = Convert.ToString(oRows[i][4] == DBNull.Value ? "" : oRows[i][4]);
                            oTPIXL.ShiftCode = Convert.ToString(oRows[i][5] == DBNull.Value ? "" : oRows[i][5]);
                            oTPIXL.DesgCode = Convert.ToString(oRows[i][6] == DBNull.Value ? "" : oRows[i][6]);
                            oTPIXL.EmpTypeName = Convert.ToString(oRows[i][7] == DBNull.Value ? "" : oRows[i][7]);
                            double BankAmount = Convert.ToDouble(oRows[i][8] == DBNull.Value ? 0 : oRows[i][8]);
                            double CashAmount = Convert.ToDouble(oRows[i][9] == DBNull.Value ? 0 : oRows[i][9]);
                            if (BankAmount > 0)
                            {
                                oTPIXL.IsCashFixed = false;
                                oTPIXL.CashAmount = BankAmount;
                            }
                            else if (CashAmount > 0)
                            {
                                oTPIXL.IsCashFixed = true;
                                oTPIXL.CashAmount = CashAmount;
                            }
                            
                            oTPIXLs.Add(oTPIXL);
                        //}

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
            return oTPIXLs;
        }

        private List<TransferPromotionIncrement> GetIncrementFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<TransferPromotionIncrement> oTPIXLs = new List<TransferPromotionIncrement>();
            TransferPromotionIncrement oTPIXL = new TransferPromotionIncrement();
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
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

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

                    int nCol = 0;
                    string SalarySchemeName = "";
                    bool IsNoteCount = false;
                    bool IsNote = false;
                    int nInitCol = 0;
                    bool bIsNoHistory = false;

                    nCol = Convert.ToInt16((oRows[0][3] == DBNull.Value || oRows[0][3] == "") ? 0 : oRows[0][3]);

                    nInitCol = nCol;

                    if (nCol <= 0) { throw new Exception("Please enter total no of salary head!"); }
                    SalarySchemeName = Convert.ToString(oRows[0][1] == DBNull.Value ? "" : oRows[0][1]);

                    nCol = nCol + 3;
                    if(Convert.ToString(oRows[1][3] == DBNull.Value ? "" : oRows[1][3])=="Note")
                    {
                        nCol = nCol+1;
                        IsNoteCount = true;
                    }
                    int nCompSH = Convert.ToInt16(oRows[0][4] == DBNull.Value ? "" : oRows[0][4]);
                    if (Convert.ToString(oRows[0][5] == DBNull.Value ? "" : oRows[0][5]) == "1")
                    {
                        bIsNoHistory = true;
                    }
                    //bIsNoHistory = Convert.ToBoolean(oRows[0][5] == DBNull.Value ? "" : oRows[0][5]);
                    //if (nCompSH == 1)
                    //{
                    //    oTPIXL.CompGrossSalary = Convert.ToDouble(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);
                    //}
                    
                    List<string> SHead = new List<string>();
                    List<string> CompSHead = new List<string>();

                    for(int j = 3; j < nCol; j++)
                    {
                        SHead.Add(Convert.ToString(oRows[1][j] == DBNull.Value ? "" : oRows[1][j]));
                    }
                    int nEndCol = nCol + nInitCol;
                    if(nCompSH == 1) 
                    {
                        oTPIXL.nCompSHField = 1;
                        for (int j = nCol + 1; j <= nEndCol; j++)
                        {
                            CompSHead.Add(Convert.ToString(oRows[1][j] == DBNull.Value ? "" : oRows[1][j]));
                        }
                    }

                    DateTime ExecutedOn = DateTime.Now;
                    for (int i = 2; i < oRows.Count; i++)
                    {
                        int nStartCompHead = nCol;
                        if (IsNoteCount) { IsNote = true; }
                        string SalaryHeadNames = "";
                        oTPIXL = new TransferPromotionIncrement();
                        oTPIXL.EmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                        if (oTPIXL.EmployeeCode!="" && oTPIXL.EmployeeCode!=null)
                        {
                            oTPIXL.EffectedDate = Convert.ToDateTime(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                            oTPIXL.GrossSalary = Convert.ToDouble(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);
                            oTPIXL.CompGrossSalary = Convert.ToDouble(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);
                            double BankAmount = Convert.ToDouble(oRows[i][3] == DBNull.Value ? 0 : oRows[i][3]);
                            double CashAmount = Convert.ToDouble(oRows[i][4] == DBNull.Value ? 0 : oRows[i][4]);
                            if (BankAmount > 0)
                            {
                                oTPIXL.IsCashFixed = false;
                                oTPIXL.CashAmount = BankAmount;
                            }
                            if (CashAmount > 0)
                            {
                                oTPIXL.IsCashFixed = false;
                                oTPIXL.CashAmount = CashAmount;
                            }

                            oTPIXL.SalarySchemeName = SalarySchemeName;

                            int n = 5;
                            foreach (string str in SHead)
                            {
                                if (IsNote)
                                { oTPIXL.Note = Convert.ToString(oRows[i][n] == DBNull.Value ? "" : oRows[i][n]); IsNote = false; }
                                else
                                {

                                    if (nCompSH == 1)
                                    {
                                        SalaryHeadNames += str + "," + Convert.ToString(oRows[i][n] == DBNull.Value ? "" : oRows[i][n]) + "," + Convert.ToString(oRows[i][nStartCompHead] == DBNull.Value ? "" : oRows[i][nStartCompHead]) + "~";
                                    }
                                    else
                                    {
                                        SalaryHeadNames += str + "," + Convert.ToString(oRows[i][n] == DBNull.Value ? "" : oRows[i][n]) + "," + Convert.ToString(oRows[i][n] == DBNull.Value ? "" : oRows[i][n]) + "~";
                                    }
                                }
                                nStartCompHead++;
                                n++;
                            }

                            if (nCompSH == 1)
                            {
                                oTPIXL.CompGrossSalary = Convert.ToDouble(oRows[i][n] == DBNull.Value ? "" : oRows[i][n]);
                                //foreach (string str in CompSHead)
                                //{
                                //    n++;
                                //    CompSalaryHeadNames += str + "," + Convert.ToString(oRows[i][n] == DBNull.Value ? "" : oRows[i][n]) + "~";
                                //}
                            }


                            SalaryHeadNames = SalaryHeadNames.Remove(SalaryHeadNames.Length - 1);
                            //CompSalaryHeadNames = CompSalaryHeadNames.Remove(CompSalaryHeadNames.Length - 1);
                            oTPIXL.SalaryHeadNames = SalaryHeadNames;
                            oTPIXL.IsNoHistory = bIsNoHistory;
                            //oTPIXL.CompSalaryHeadNames = CompSalaryHeadNames;
                            oTPIXLs.Add(oTPIXL); 
                        }

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
            return oTPIXLs;
        }

        private void GetSalaryScheme() {
            _oSalarySchemes = SalaryScheme.Gets("SELECT * FROM SalaryScheme", (int)(Session[SessionInfo.currentUserID]));
        }

        private int GetSalarySchemeID(string sSchemeName)
        {
            int nSchemeID = 0;
            foreach (SalaryScheme item in _oSalarySchemes)
            {
                if (item.Name == sSchemeName)
                {
                    nSchemeID = item.SalarySchemeID;
                    break;
                }
            }
            return nSchemeID;
        }

        private List<TransferPromotionIncrement> GetIncrementAsPerSchemeFromExcel(HttpPostedFileBase PostedFile)
        {
            DataSet ds = new DataSet();
            DataRowCollection oRows = null;
            string fileExtension = "";
            string fileDirectory = "";
            List<TransferPromotionIncrement> oTPIXLs = new List<TransferPromotionIncrement>();
            TransferPromotionIncrement oTPIXL = new TransferPromotionIncrement();
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
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

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

                    Employee oEmployee = new Employee();

                    DateTime ExecutedOn = DateTime.Now;
                    for (int i = 0; i < oRows.Count; i++)
                    {
                        oTPIXL = new TransferPromotionIncrement();
                        oTPIXL.EmployeeCode = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);

                        //oEmployee = Employee.Get("SELECT * FROM Employee WHERE Code='" + oTPIXL.EmployeeCode+"'", (int)(Session[SessionInfo.currentUserID]));

                        string sTempDate = "";
                        string pdt = "";
                        DateTime dDate = DateTime.Now;

                        if (oTPIXL.EmployeeCode != "")
                        {
                            //oTPIXL.EmployeeID = oEmployee.EmployeeID;
                            oTPIXL.SalarySchemeName = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                            //oTPIXL.TPISalarySchemeID = GetSalarySchemeID(oTPIXL.SalarySchemeName);
                            string sEffectedDate = Convert.ToString(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);

                            if (sEffectedDate != "")
                            {
                                sTempDate = "";
                                pdt = "";
                                DateTime dtD = DateTime.MinValue;
                                //sTempDate = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                                //string sDay = sEffectedDate.Split('/')[0];
                                //string sMonth = sEffectedDate.Split('/')[1];
                                //string sYear = sEffectedDate.Split('/')[2];

                                //string sHour = "00";
                                //string sMin = "00";
                                //string sSecond = "00";

                                //dDate = DateTime.ParseExact(Convert.ToInt32(sMonth).ToString("00") + "/" + Convert.ToInt32(sDay).ToString("00") + "/" + sYear + " " + Convert.ToInt32(sHour).ToString("00") + ":" + Convert.ToInt32(sMin).ToString("00") + ":" + sSecond, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                                //pdt = dDate.ToString("dd MMM yyyy");
                                DateTime.TryParse(sEffectedDate, out dtD);
                                oTPIXL.ActualEffectedDate = Convert.ToDateTime(dtD);
                            }

                            oTPIXL.TPIGrossSalary = Convert.ToDouble(oRows[i][3] == DBNull.Value ? 0 : oRows[i][3]);
                            oTPIXL.CompTPIGrossSalary = Convert.ToDouble(oRows[i][4] == DBNull.Value ? 0 : oRows[i][4]);

                            double BankAmount = Convert.ToDouble(oRows[i][5] == DBNull.Value ? 0 : oRows[i][5]);
                            double CashAmount = Convert.ToDouble(oRows[i][6] == DBNull.Value ? 0 : oRows[i][6]);

                            if (BankAmount > 0)
                            {
                                oTPIXL.IsCashFixed = false;
                                oTPIXL.CashAmount = BankAmount;
                            }
                            else if (CashAmount > 0)
                            {
                                oTPIXL.IsCashFixed = true;
                                oTPIXL.CashAmount = CashAmount;
                            }

                            oTPIXL.IsIncrement = true;
                            oTPIXLs.Add(oTPIXL); 
                        }

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
            return oTPIXLs;
        }
        [HttpPost]
        public ActionResult View_TPIs(HttpPostedFileBase fileIncrements, string isTPI)
        {
            List<TransferPromotionIncrement> oTPIXLs = new List<TransferPromotionIncrement>();
            TransferPromotionIncrement oTPIXL = new TransferPromotionIncrement();

            if (isTPI == "Inc")
            {
                try
                {
                    if (fileIncrements == null) { throw new Exception("File not Found"); }

                    oTPIXLs = new List<TransferPromotionIncrement>();
                    oTPIXLs = this.GetIncrementFromExcel(fileIncrements);
                    oTPIXLs = TransferPromotionIncrement.UploadXL(oTPIXLs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oTPIXLs.Count > 0)
                    {
                        oTPIXLs[0].ErrorMessage = "Uploaded Successfully!";
                    }
                    else
                    {
                        oTPIXLs = new List<TransferPromotionIncrement>();
                        TransferPromotionIncrement oTPI = new TransferPromotionIncrement();
                        oTPI.ErrorMessage = "nothing to upload or alraedy uploaded!";
                        oTPIXLs.Add(oTPI);
                    }

                    ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                    ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                    ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
                    ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                }
                catch (Exception ex)
                {
                    ViewBag.FeedBack = ex.Message;
                    return View(oTPIXLs);
                }
            }
            else if (isTPI == "IncAsPerScheme")
            {
                try
                {
                    if (fileIncrements == null) { throw new Exception("File not Found"); }

                    oTPIXLs = new List<TransferPromotionIncrement>();
                    GetSalaryScheme();
                    oTPIXLs = this.GetIncrementAsPerSchemeFromExcel(fileIncrements);
                    oTPIXLs = TransferPromotionIncrement.UploadXLAsPerScheme(oTPIXLs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    
                    if (oTPIXLs.Count > 0)
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

                            foreach (TransferPromotionIncrement oItem in oTPIXLs)
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
                    else
                    {

                        TransferPromotionIncrement oTPI = new TransferPromotionIncrement();
                        oTPI.ErrorMessage = "Uploaded Successfully!";
                        oTPIXLs.Add(oTPI);
                    }
                    
                    
                    //if (oTPIXLs.Count > 0)
                    //{
                    //    oTPIXLs[0].ErrorMessage = "Uploaded Successfully!";
                    //}
                    //else
                    //{
                    //    oTPIXLs = new List<TransferPromotionIncrement>();
                    //    TransferPromotionIncrement oTPI = new TransferPromotionIncrement();
                    //    oTPI.ErrorMessage = "nothing to upload or alraedy uploaded!";
                    //    oTPIXLs.Add(oTPI);
                    //}

                    ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                    ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                    ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
                    ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                }
                catch (Exception ex)
                {
                    ViewBag.FeedBack = ex.Message;
                    return View(oTPIXLs);
                }
            }
            else if (isTPI == "TP")
            {
                try
                {
                    if (fileIncrements == null) { throw new Exception("File not Found"); }

                    oTPIXLs = new List<TransferPromotionIncrement>();
                    List<TransferPromotionIncrement> oTPIs = new List<TransferPromotionIncrement>();
                    oTPIs = this.GetTPFromExcel(fileIncrements);
                    oTPIXLs = TransferPromotionIncrement.UploadXLTP(oTPIs, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    
                    
                    if (oTPIXLs.Count > 0)
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

                            foreach (TransferPromotionIncrement oItem in oTPIXLs)
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
                    else
                    {

                        TransferPromotionIncrement oTPI = new TransferPromotionIncrement();
                        oTPI.ErrorMessage = "Uploaded Successfully!";
                        oTPIXLs.Add(oTPI);
                    }
                    ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
                    ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                    ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                    ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
                }
                catch (Exception ex)
                {
                    ViewBag.FeedBack = ex.Message;
                    return View(oTPIXLs);
                }
            }
            //ViewBag.TPIs = oTPIXLs;
            //int  menuid=0;
            //menuid = (int)Session[SessionInfo.MenuID];
            //return RedirectToAction("View_TPIs", "TransferPromotionIncrement", new { menuid});
            return View(oTPIXLs);
        }
        #endregion Import & Export

        #region TPI Search
        [HttpPost]
        public JsonResult TPI_Search(string sParams)
        {
            DateTime dtStartDate= Convert.ToDateTime(sParams.Split('~')[0]);
            DateTime dtEndDate=Convert.ToDateTime(sParams.Split('~')[1]);
            bool IsTransfer=Convert.ToBoolean(sParams.Split('~')[2]);
            bool IsPromotion=Convert.ToBoolean(sParams.Split('~')[3]);
            bool IsIncrement = Convert.ToBoolean(sParams.Split('~')[4]);

            string sBusinessUnitIds = Convert.ToString(sParams.Split('~')[5]);
            string sLocationID = Convert.ToString(sParams.Split('~')[6]);
            string sDepartmentIds = Convert.ToString(sParams.Split('~')[7]);
            string sDesignationIds = Convert.ToString(sParams.Split('~')[8]);

            bool IsRecommend = Convert.ToBoolean(sParams.Split('~')[9]);
            bool IsApprove = Convert.ToBoolean(sParams.Split('~')[10]);

            string sSql = "SELECT * FROM View_TransferPromotionIncrement WHERE EffectedDate BETWEEN '"+dtStartDate.ToString("dd MMM yyyy")+"' AND '"+dtEndDate.ToString("dd MMM yyyy")+"' ";
            if (IsTransfer) { sSql = sSql + " AND IsTransfer = 1"; }
            if (IsPromotion) { sSql = sSql + " AND IsPromotion = 1"; }
            if (IsIncrement) { sSql = sSql + " AND IsIncrement = 1"; }
            if (IsRecommend) { sSql = sSql + " AND RecommendedBy>0"; }
            if (IsApprove) { sSql = sSql + " AND ApproveBy>0"; }

            if (sBusinessUnitIds != "") { sSql = sSql + " AND EmployeeID IN(SELECT  EmployeeID FROM View_Employee WHERE BusinessUnitID IN(" + sBusinessUnitIds + "))"; }
            if (sLocationID!="") { sSql = sSql + " AND EmployeeID IN(SELECT  EmployeeID FROM View_Employee WHERE LocationID IN("+sLocationID+"))"; }
            if (sDepartmentIds != "") { sSql = sSql + " AND EmployeeID IN(SELECT  EmployeeID FROM View_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))"; }
            if (sDesignationIds != "") { sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")"; }

            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }
            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            try
            {
                _oTransferPromotionIncrements = TransferPromotionIncrement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oTransferPromotionIncrements.Count <=0)
                {
                    throw new Exception("Data not found.");
                }
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
                _oTransferPromotionIncrements.Add(_oTransferPromotionIncrement);
            }

            var jsonResult = Json(_oTransferPromotionIncrements, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion TPI Search


        public object[] AutoTPI()
        {
            object[] objArr = new object[1];
            string html = "Successful";
            var oTPIs = TransferPromotionIncrement.GetsAutoTPI(0);
            
            objArr[0] = html;
            return objArr;
        }


        public ActionResult PrintIncrementHistory(string sEmployeeID)
        {
            _oTransferPromotionIncrement = new TransferPromotionIncrement();
            string sSql = "SELECT * FROM VIEW_TransferPromotionIncrement WHERE IsIncrement=1 AND  ISNULL(IsNoHistory, 0)=0 AND ApproveBy >0 AND EmployeeID=" + sEmployeeID + " ORDER BY EffectedDate DESC";
            _oTransferPromotionIncrement.TransferPromotionIncrements = TransferPromotionIncrement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oTransferPromotionIncrement.Company = oCompanys.First();
            _oTransferPromotionIncrement.Company.CompanyLogo = GetImage(_oTransferPromotionIncrement.Company.OrganizationLogo);

            rptIncrementHistory oReport = new rptIncrementHistory();
            byte[] abytes = oReport.PrepareReport(_oTransferPromotionIncrement);
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


        public ActionResult PrintIncrementApprisal(string sParam)
        {

            DateTime UpToDate = Convert.ToDateTime(sParam.Split('~')[0]);
            string BusinessUnitIds = sParam.Split('~')[1];
            string LocationIDs = sParam.Split('~')[2];
            string DepartmentIDs = sParam.Split('~')[3];
            string DesignationIDs = sParam.Split('~')[4];
            string EmployeeIDs = sParam.Split('~')[5];
            DateTime JoiningDate = Convert.ToDateTime(sParam.Split('~')[6]);
            bool IsMultipleMonth = Convert.ToBoolean(sParam.Split('~')[7]);
            string sMonths = sParam.Split('~')[8];
            string sYears = sParam.Split('~')[9];
            bool IsJoinDate = Convert.ToBoolean(sParam.Split('~')[10]);
            double nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[11]);
            double nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[12]);
            string BlockIDs = sParam.Split('~')[13];
            string GroupIDs = sParam.Split('~')[14];
            //int MonthFrom = Convert.ToInt32(sParam.Split('~')[8]);
            //int YearFrom = Convert.ToInt32(sParam.Split('~')[9]);
            //int MonthTo = Convert.ToInt32(sParam.Split('~')[10]);
            //int YearTo = Convert.ToInt32(sParam.Split('~')[11]);
            //bool IsJoinDate = Convert.ToBoolean(sParam.Split('~')[12]);

            IncrementApprisal _oIncrementApprisal = new IncrementApprisal();
            _oIncrementApprisal.IncrementApprisals = IncrementApprisal.Search(UpToDate, EmployeeIDs, BusinessUnitIds, LocationIDs, DepartmentIDs, DesignationIDs, JoiningDate, IsMultipleMonth, sMonths, sYears, IsJoinDate, nStartSalaryRange, nEndSalaryRange, BlockIDs, GroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oIncrementApprisal.Company = oCompanys.First();

            string BUIDs = string.Join(",", _oIncrementApprisal.IncrementApprisals.Where(x => x.EmployeeID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { _oIncrementApprisal.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }



            rptIncrementApprisal oReport = new rptIncrementApprisal();
            byte[] abytes = oReport.PrepareReport(_oIncrementApprisal, UpToDate);
            return File(abytes, "application/pdf");
        }
        public void ExcelIncrementApprisal(string sParam)
        {

            DateTime UpToDate = Convert.ToDateTime(sParam.Split('~')[0]);
            string BusinessUnitIds = sParam.Split('~')[1];
            string LocationIDs = sParam.Split('~')[2];
            string DepartmentIDs = sParam.Split('~')[3];
            string DesignationIDs = sParam.Split('~')[4];
            string EmployeeIDs = sParam.Split('~')[5];
            DateTime JoiningDate = Convert.ToDateTime(sParam.Split('~')[6]);
            bool IsMultipleMonth = Convert.ToBoolean(sParam.Split('~')[7]);
            string sMonths = sParam.Split('~')[8];
            string sYears = sParam.Split('~')[9];
            bool IsJoinDate = Convert.ToBoolean(sParam.Split('~')[10]);
            double nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[11]);
            double nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[12]);
            string BlockIDs = sParam.Split('~')[13];
            string GroupIDs = sParam.Split('~')[14];
            //int MonthFrom = Convert.ToInt32(sParam.Split('~')[8]);
            //int YearFrom = Convert.ToInt32(sParam.Split('~')[9]);
            //int MonthTo = Convert.ToInt32(sParam.Split('~')[10]);
            //int YearTo = Convert.ToInt32(sParam.Split('~')[11]);
            //bool IsJoinDate = Convert.ToBoolean(sParam.Split('~')[12]);

            IncrementApprisal _oIncrementApprisal = new IncrementApprisal();
            _oIncrementApprisal.IncrementApprisals = IncrementApprisal.Search(UpToDate, EmployeeIDs, BusinessUnitIds, LocationIDs, DepartmentIDs, DesignationIDs,JoiningDate, IsMultipleMonth, sMonths, sYears, IsJoinDate, nStartSalaryRange, nEndSalaryRange, BlockIDs, GroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oIncrementApprisal.Company = oCompanys.First();

            string BUIDs = string.Join(",", _oIncrementApprisal.IncrementApprisals.Where(x => x.EmployeeID > 0).Select(p => p.BusinessUnitID).Distinct().ToList());
            if (BUIDs != "") { _oIncrementApprisal.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }
            
            
            int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Increment Apprisal");
                sheet.Name = "Increment Apprisal";

                int n = 1;
                sheet.Column(n++).Width = 8;
                sheet.Column(n++).Width = 13;
                sheet.Column(n++).Width = 15;
                sheet.Column(n++).Width = 15;
                sheet.Column(n++).Width = 15;

                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;

                sheet.Column(n++).Width = 13;

                sheet.Column(n++).Width = 11;

                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 8;
                sheet.Column(n++).Width = 7;
                sheet.Column(n++).Width = 7;
                sheet.Column(n++).Width = 5;
                sheet.Column(n++).Width = 7;
                sheet.Column(n++).Width = 7;
                sheet.Column(n++).Width = 8;

                nEndCol = n;

                //#region Report Header
                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rowIndex = rowIndex + 1;

                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "TPI Excel"; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //rowIndex = rowIndex + 2;
                //#endregion

                string[] nMonthIDs = new string[sMonths.Split(',').Count()];
                string strMonthName="";
                string monthNames = "";

                if (IsJoinDate)
                {
                    monthNames += "(";
                    monthNames += JoiningDate.ToString("MMM");
                    monthNames += ")";
                }
                else if (IsMultipleMonth)
                {
                    nMonthIDs = sMonths.Split(',');
                    monthNames += "(";
                    foreach (var oitem in nMonthIDs)
                    {
                        DateTimeFormatInfo mfi = new DateTimeFormatInfo();
                        strMonthName = mfi.GetMonthName(Convert.ToInt32(oitem)).ToString();
                        monthNames = monthNames + strMonthName.Substring(0, 3) + ",";
                    }
                    monthNames = monthNames.Substring(0, monthNames.Length-1);
                    monthNames += ")";
                }

                string sBUCompany = "";
                string sBUAddress = "";
                if (_oIncrementApprisal.BusinessUnits.Count > 1)
                {
                    sBUCompany = _oIncrementApprisal.Company.Name;
                    sBUAddress = _oIncrementApprisal.Company.Address;
                }
                else
                {
                    sBUCompany = _oIncrementApprisal.BusinessUnits[0].Name;
                    sBUAddress = _oIncrementApprisal.BusinessUnits[0].Address;
                }
                cell = sheet.Cells[rowIndex, 1, rowIndex, nEndCol]; cell.Value = sBUCompany; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, 1, rowIndex, nEndCol]; cell.Value = sBUAddress; cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, 1, rowIndex, nEndCol]; cell.Value = "Yearly Increment Sheet " + /*UpToDate.ToString("MMMM")*/monthNames + " - " + UpToDate.ToString("yyyy"); cell.Style.Font.Bold = true; cell.Merge = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                rowIndex = rowIndex + 1;
                colIndex = 1;

                List<IncrementApprisal> oIncrementApprisals = new List<IncrementApprisal>();
                _oIncrementApprisal.IncrementApprisals.ForEach(x => oIncrementApprisals.Add(x));
                oIncrementApprisals = oIncrementApprisals.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                ColumnSetup(ref sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nEndCol, oIncrementApprisals[0]);
                rowIndex -= 1;
                while (oIncrementApprisals.Count > 0)
                {
                    var oResults = oIncrementApprisals.Where(x => x.LocationID == oIncrementApprisals[0].LocationID && x.DepartmentID == oIncrementApprisals[0].DepartmentID).ToList();
                    List<BusinessUnit> oTempBusinessUnits = new List<BusinessUnit>();
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    oTempBusinessUnits = _oIncrementApprisal.BusinessUnits.Where(x => x.BusinessUnitID == oIncrementApprisals[0].BusinessUnitID).ToList();
                    oBusinessUnit = oTempBusinessUnits.Count > 0 ? oTempBusinessUnits[0] : new BusinessUnit();

                    cell = sheet.Cells[rowIndex+1, 1]; cell.Value = "Location: " + oResults.First().LocationName+" ,Department : " + oResults.FirstOrDefault().DepartmentName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    rowIndex = rowIndex + 2;
                    colIndex = 1;

                    //ColumnSetup(ref sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nEndCol, oIncrementApprisals[0]);
                    
                    BodySetup(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nEndCol, oResults, oResults[0].DepartmentID, UpToDate);

                    oIncrementApprisals.RemoveAll(x => x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                }

                

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=IncrementApprisal.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }
        public void BodySetup(ref ExcelWorksheet sheet, ref ExcelRange cell, ref ExcelFill fill, ref OfficeOpenXml.Style.Border border, ref int rowIndex, ref int colIndex, int nMaxColumn, List<IncrementApprisal> oIncrementApprisals, int nDepartmentID, DateTime CurrentDate)
        {
            int nSL = 0;

            foreach (IncrementApprisal oItem in oIncrementApprisals)
            {
                nSL++;
                sheet.Row(rowIndex).Height = 40;

                cell = sheet.Cells[rowIndex, 1, rowIndex, 1]; cell.Value = nSL; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 1, rowIndex, 1].Merge = true;
                cell = sheet.Cells[rowIndex, 2, rowIndex , 2]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 2, rowIndex, 2].Merge = true;
                cell = sheet.Cells[rowIndex, 3, rowIndex , 3]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 3, rowIndex, 3].Merge = true;
                cell = sheet.Cells[rowIndex, 4, rowIndex , 4]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 4, rowIndex , 4].Merge = true;
                cell = sheet.Cells[rowIndex, 5, rowIndex , 5]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 5, rowIndex , 5].Merge = true;
                cell = sheet.Cells[rowIndex, 6, rowIndex , 6]; cell.Value = oItem.BeforeIncrement; cell.Style.Font.Bold = false; cell.Style.TextRotation = 90; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 6, rowIndex , 6].Merge = true;
                cell = sheet.Cells[rowIndex, 7, rowIndex, 7]; cell.Value = oItem.BeforeEffectDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 7, rowIndex , 7].Merge = true;
                cell = sheet.Cells[rowIndex, 8, rowIndex , 8]; cell.Value = oItem.RecentIncrement; cell.Style.Font.Bold = false; cell.Style.TextRotation = 90; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 8, rowIndex , 8].Merge = true;
                cell = sheet.Cells[rowIndex, 9, rowIndex , 9]; cell.Value = oItem.RecentEffectDateInString; cell.Style.Font.Bold = false; cell.Style.TextRotation = 90; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 9, rowIndex , 9].Merge = true;
                cell = sheet.Cells[rowIndex, 10, rowIndex , 10]; cell.Value = oItem.PresentSalary; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 10, rowIndex , 10].Merge = true;
                cell = sheet.Cells[rowIndex, 11, rowIndex, 11]; cell.Value = oItem.Education; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 11, rowIndex, 11].Merge = true;
                cell = sheet.Cells[rowIndex, 12, rowIndex , 12]; cell.Value = oItem.TotalLate; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 12, rowIndex, 12].Merge = true;
                cell = sheet.Cells[rowIndex, 13, rowIndex, 13]; cell.Value = oItem.TotalLeave; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 13, rowIndex, 13].Merge = true;
                cell = sheet.Cells[rowIndex, 14, rowIndex, 14]; cell.Value = oItem.TotalAbsent; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 14, rowIndex, 14].Merge = true;
                cell = sheet.Cells[rowIndex, 15, rowIndex , 15]; cell.Value = oItem.AttendancePercent; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 15, rowIndex, 15].Merge = true;
                cell = sheet.Cells[rowIndex, 16, rowIndex, 16]; cell.Value = oItem.Warning; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                //sheet.Cells[rowIndex, 16, rowIndex, 16].Merge = true;
                cell = sheet.Cells[rowIndex, 17, rowIndex , 17]; cell.Value = (oItem.LastPromotionDateInString != "-") ? oItem.LastPromotionDateInString : "-"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 17, rowIndex, 17].Merge = true;
                cell = sheet.Cells[rowIndex, 18, rowIndex , 18]; cell.Value = CurrentDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                DateTime tempDate = oItem.LastPromotionDate.AddDays(1);
                DateDifference ServiceYear = new DateDifference(tempDate, CurrentDate);

                //sheet.Cells[rowIndex, 18, rowIndex, 18].Merge = true;
                cell = sheet.Cells[rowIndex, 19, rowIndex, 19]; cell.Value = (oItem.LastPromotionDate.ToString("dd MMM yyyy") == "01 Jan 1900") ? "-" : ServiceYear.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                //sheet.Cells[rowIndex, 19, rowIndex, 19].Merge = true;
                cell = sheet.Cells[rowIndex, 20, rowIndex, 20]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex,20, rowIndex, 20].Merge = true;
                cell = sheet.Cells[rowIndex, 21, rowIndex, 21]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 21, rowIndex, 21].Merge = true;
                cell = sheet.Cells[rowIndex, 22, rowIndex, 22]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 22, rowIndex, 22].Merge = true;
                cell = sheet.Cells[rowIndex, 23, rowIndex, 23]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 23, rowIndex, 23].Merge = true;
                cell = sheet.Cells[rowIndex, 24, rowIndex, 24]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 24, rowIndex, 24].Merge = true;
                cell = sheet.Cells[rowIndex, 25, rowIndex , 25]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 25, rowIndex, 25].Merge = true;
                cell = sheet.Cells[rowIndex, 26, rowIndex, 26]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 26, rowIndex, 26].Merge = true;
                cell = sheet.Cells[rowIndex, 27, rowIndex, 27]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 27, rowIndex , 27].Merge = true;
                cell = sheet.Cells[rowIndex, 28, rowIndex, 28]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 28, rowIndex , 28].Merge = true;
                cell = sheet.Cells[rowIndex, 29, rowIndex, 29]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 29, rowIndex , 29].Merge = true;
                cell = sheet.Cells[rowIndex, 30, rowIndex, 30]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 30, rowIndex , 30].Merge = true;
                cell = sheet.Cells[rowIndex, 31, rowIndex , 31]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 31, rowIndex , 31].Merge = true;
                cell = sheet.Cells[rowIndex, 32, rowIndex, 32]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 32, rowIndex , 32].Merge = true;
                cell = sheet.Cells[rowIndex, 33, rowIndex, 33]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 33, rowIndex , 33].Merge = true;
                cell = sheet.Cells[rowIndex, 34, rowIndex, 34]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 34, rowIndex , 34].Merge = true;
                cell = sheet.Cells[rowIndex, 35, rowIndex, 35]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 35, rowIndex , 35].Merge = true;
                cell = sheet.Cells[rowIndex, 36, rowIndex, 36]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                
                //sheet.Cells[rowIndex, 36, rowIndex, 36].Merge = true;
                cell = sheet.Cells[rowIndex, 37, rowIndex, 37]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 37, rowIndex, 37].Merge = true;
                cell = sheet.Cells[rowIndex, 38, rowIndex , 38]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //sheet.Cells[rowIndex, 38, rowIndex, 38].Merge = true;
                cell = sheet.Cells[rowIndex, 39, rowIndex, 39]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                cell.Style.TextRotation = 90;
                cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                
                rowIndex+=1;
            }
        }
        public void ColumnSetup(ref ExcelWorksheet sheet, ref ExcelRange cell, ref ExcelFill fill, ref OfficeOpenXml.Style.Border border, ref int rowIndex, ref int colIndex, int nMaxColumn, IncrementApprisal oIncrementApprisal)
        {
            sheet.Row(rowIndex).Height = 30;
            sheet.Row(rowIndex+1).Height = 60;


            sheet.Cells[rowIndex, 1, rowIndex + 1, 1].Merge = true;
            cell = sheet.Cells[rowIndex, 1, rowIndex + 1, 1]; cell.Value = "SL"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 2, rowIndex + 1, 2].Merge = true;
            cell = sheet.Cells[rowIndex, 2, rowIndex + 1, 2]; cell.Value = "EmployeeID"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 3, rowIndex + 1, 3].Merge = true;
            cell = sheet.Cells[rowIndex, 3, rowIndex + 1, 3]; cell.Value = "Name"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 4, rowIndex + 1, 4].Merge = true;
            cell = sheet.Cells[rowIndex, 4, rowIndex + 1, 4]; cell.Value = "Designation"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 5, rowIndex + 1, 5].Merge = true;
            cell = sheet.Cells[rowIndex, 5, rowIndex + 1, 5]; cell.Value = "JoiningDate"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 6, rowIndex + 1, 6].Merge = true;
            cell = sheet.Cells[rowIndex, 6, rowIndex + 1, 6]; cell.Value = "1st Increment"; cell.Style.Font.Bold = false; cell.Style.TextRotation = 90; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 7, rowIndex + 1, 7].Merge = true;
            cell = sheet.Cells[rowIndex, 7, rowIndex + 1, 7]; cell.Value = "1st Increment Month"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.TextRotation = 90; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 8, rowIndex + 1, 8].Merge = true;
            cell = sheet.Cells[rowIndex, 8, rowIndex + 1, 8]; cell.Value = "2nd Increment"; cell.Style.Font.Bold = false; cell.Style.TextRotation = 90; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 9, rowIndex + 1, 9].Merge = true;
            cell = sheet.Cells[rowIndex, 9, rowIndex + 1, 9]; cell.Value = "2nd Increment Month"; cell.Style.Font.Bold = false; cell.Style.TextRotation = 90; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 10, rowIndex + 1, 10].Merge = true;
            cell = sheet.Cells[rowIndex, 10, rowIndex + 1, 10]; cell.Value = "Present Salary"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 11, rowIndex + 1, 11].Merge = true;
            cell = sheet.Cells[rowIndex, 11, rowIndex + 1, 11]; cell.Value = "Education"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 12, rowIndex, 16].Merge = true;
            cell = sheet.Cells[rowIndex, 12, rowIndex, 16]; cell.Value = "Last 1 Year"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 17, rowIndex, 19].Merge = true;
            cell = sheet.Cells[rowIndex, 17, rowIndex, 19]; cell.Value = "Promotion"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 20, rowIndex, 27].Merge = true;
            cell = sheet.Cells[rowIndex, 20, rowIndex, 27]; cell.Value = "Performance Numbering System (High=80-100, Medium=60-79, Normal=41-59, Low=up to 40)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 28, rowIndex + 1, 28].Merge = true;
            cell = sheet.Cells[rowIndex, 28, rowIndex + 1, 28]; cell.Value = "Average Marks"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 29, rowIndex + 1, 29].Merge = true;
            cell = sheet.Cells[rowIndex, 29, rowIndex + 1, 29]; cell.Value = "Efficency(If Any)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 30, rowIndex + 1, 30].Merge = true;
            cell = sheet.Cells[rowIndex, 30, rowIndex + 1, 30]; cell.Value = "Performance Grade"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 31, rowIndex + 1, 31].Merge = true;
            cell = sheet.Cells[rowIndex, 31, rowIndex + 1, 31]; cell.Value = "Proposed Increment Amount"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 32, rowIndex + 1, 32].Merge = true;
            cell = sheet.Cells[rowIndex, 32, rowIndex + 1, 32]; cell.Value = "Board Approved Increment Amount"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 33, rowIndex + 1, 33].Merge = true;
            cell = sheet.Cells[rowIndex, 33, rowIndex + 1, 33]; cell.Value = "Deduction(Seam Loss, Sea/Hot/Shot,Shipment, D.A)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 34, rowIndex + 1, 34].Merge = true;
            cell = sheet.Cells[rowIndex, 34, rowIndex + 1, 34]; cell.Value = "Departmental Approval"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 35, rowIndex + 1, 35].Merge = true;
            cell = sheet.Cells[rowIndex, 35, rowIndex + 1, 35]; cell.Value = "%"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 36, rowIndex + 1, 36].Merge = true;
            cell = sheet.Cells[rowIndex, 36, rowIndex + 1, 36]; cell.Value = "Final Approval"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 37, rowIndex + 1, 37].Merge = true;
            cell = sheet.Cells[rowIndex, 37, rowIndex + 1, 37]; cell.Value = "%"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 38, rowIndex + 1, 38].Merge = true;
            cell = sheet.Cells[rowIndex, 38, rowIndex + 1, 38]; cell.Value = "Salary After Increment"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 39, rowIndex + 1, 39].Merge = true;
            cell = sheet.Cells[rowIndex, 39, rowIndex + 1, 39]; cell.Value = "Promotion(If Any)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rowIndex++;

            colIndex = 11;
            sheet.Cells[rowIndex, 12, rowIndex, 12].Merge = true;
            cell = sheet.Cells[rowIndex, 12, rowIndex, 12]; cell.Value = "Late"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 13, rowIndex, 13].Merge = true;
            cell = sheet.Cells[rowIndex, 13, rowIndex, 13]; cell.Value = "Leave"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 14, rowIndex, 14].Merge = true;
            cell = sheet.Cells[rowIndex, 14, rowIndex, 14]; cell.Value = "Absent"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 15, rowIndex, 15].Merge = true;
            cell = sheet.Cells[rowIndex, 15, rowIndex, 15]; cell.Value = "Attendance %"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 16, rowIndex, 16].Merge = true;
            cell = sheet.Cells[rowIndex, 16, rowIndex, 16]; cell.Value = "Warning"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 17, rowIndex, 17].Merge = true;
            cell = sheet.Cells[rowIndex, 17, rowIndex, 17]; cell.Value = "Last Promotion Date"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 18, rowIndex, 18].Merge = true;
            cell = sheet.Cells[rowIndex, 18, rowIndex, 18]; cell.Value = "Current Date"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 19, rowIndex, 19].Merge = true;
            cell = sheet.Cells[rowIndex, 19, rowIndex, 19]; cell.Value = "Promotion Duration"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            sheet.Cells[rowIndex, 20, rowIndex, 20].Merge = true;
            cell = sheet.Cells[rowIndex, 20, rowIndex, 20]; cell.Value = "Working Efficency Status"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 21, rowIndex, 21].Merge = true;
            cell = sheet.Cells[rowIndex, 21, rowIndex, 21]; cell.Value = "Willing to the works"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 22, rowIndex, 22].Merge = true;
            cell = sheet.Cells[rowIndex, 22, rowIndex, 22]; cell.Value = "Cooperation"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 23, rowIndex, 23].Merge = true;
            cell = sheet.Cells[rowIndex, 23, rowIndex, 23]; cell.Value = "Intelligence and Strategy"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 24, rowIndex, 24].Merge = true;
            cell = sheet.Cells[rowIndex, 24, rowIndex, 24]; cell.Value = "Reliability"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 25, rowIndex, 25].Merge = true;
            cell = sheet.Cells[rowIndex, 25, rowIndex, 25]; cell.Value = "Behavior"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 26, rowIndex, 26].Merge = true;
            cell = sheet.Cells[rowIndex, 26, rowIndex, 26]; cell.Value = "Discipline and Obeys"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells[rowIndex, 27, rowIndex, 27].Merge = true;
            cell = sheet.Cells[rowIndex, 27, rowIndex, 27]; cell.Value = "Congrolling Ability Subordinates"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
            cell.Style.TextRotation = 90;
            cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            rowIndex += 1;
            colIndex = 1;
        }

        public void DownloadFormat()
        {
            int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
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
                sheet.Column(n++).Width = 13;//Empcode
                sheet.Column(n++).Width = 13;//BUCode
                sheet.Column(n++).Width = 13;//LocCode
                sheet.Column(n++).Width = 13;//DeptCode
                sheet.Column(n++).Width = 13;//AttSchemeName
                sheet.Column(n++).Width = 13;//ShiftCode
                sheet.Column(n++).Width = 13;//DesgCode
                sheet.Column(n++).Width = 13;//EmpTypeName
                sheet.Column(n++).Width = 13;//BankAmount
                sheet.Column(n++).Width = 13;//CashAmount
                sheet.Column(n++).Width = 13;//blank
                sheet.Column(n++).Width = 20;//T/P


                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmployeeCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BUCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LocCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DeptCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AttSchemeName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ShiftCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DesgCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmpTypeName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Cash Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "102"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Scheme1"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "202"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Type1"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Transfer Promotion"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1002"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "202"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Type1"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Promotion"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1003"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "102"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Transfer"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1004"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "102"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Scheme2"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1002"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Transfer"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1005"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "102"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Scheme1"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Transfer"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1006"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Scheme3"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1004"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Edit Only"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1007"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Type3"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Edit Only"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1008"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "101"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "102"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Scheme4"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Transfer"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1009"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "203"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Promotion"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex+=4;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 2]; cell.Value = "Condition:"; cell.Style.Font.Bold = false; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 9]; cell.Value = "01. For Transfer BUCode,LocCode,DeptCode mandatory."; cell.Style.Font.Bold = false; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 9]; cell.Value = "02. For Promotion DesgCode Mandatory."; cell.Style.Font.Bold = false; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + 9]; cell.Value = "03. Without BUCode,LocCode,DeptCode,DesgCode, Others will be only edit without no history."; cell.Style.Font.Bold = false; cell.Merge = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Format.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void DownloadFormatIncrement()
        {
            int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
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
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 100;//


                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Increment"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SalaryScheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Scheme-1"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "No of SH"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "*(0/1)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "*(0/1/BLANK)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "(*) 1/0(If IsComp=1 Then You need to set compGross and others, else you do not need to set other option)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex += 1;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EmployeeCode"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Effect Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Cash Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Note"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "(*) 0/1/BLANK (If 1 Then there will be no history otherwise not."; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1001"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "01-Jan-18"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "10000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "7000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Note"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "6000"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                colIndex = 1;


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Format.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void SampleFormatDownloadIncAsPerScheme()
        {
            int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
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
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 13;//
                sheet.Column(n++).Width = 100;//


                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary Scheme"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Effect Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Comp Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Cash"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex += 1;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "1001"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "300"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "9/8/2018"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "30000"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "25000"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "20000"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "0"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "# Fixed amount will declare in bank/cash field as per policy. Date Format (mm-dd-yyyy)"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Format.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void TPIExcel(string sParams, double nts)
        {

            DateTime dtStartDate = Convert.ToDateTime(sParams.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(sParams.Split('~')[1]);
            bool IsTransfer = Convert.ToBoolean(sParams.Split('~')[2]);
            bool IsPromotion = Convert.ToBoolean(sParams.Split('~')[3]);
            bool IsIncrement = Convert.ToBoolean(sParams.Split('~')[4]);

            string sBusinessUnitIds = Convert.ToString(sParams.Split('~')[5]);
            string sLocationID = Convert.ToString(sParams.Split('~')[6]);
            string sDepartmentIds = Convert.ToString(sParams.Split('~')[7]);
            string sDesignationIds = Convert.ToString(sParams.Split('~')[8]);

            bool IsRecommend = Convert.ToBoolean(sParams.Split('~')[9]);
            bool IsApprove = Convert.ToBoolean(sParams.Split('~')[10]);

            string sSql = "SELECT * FROM View_TransferPromotionIncrement WHERE EffectedDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' ";
            if (IsTransfer) { sSql = sSql + " AND IsTransfer = 1"; }
            if (IsPromotion) { sSql = sSql + " AND IsPromotion = 1"; }
            if (IsIncrement) { sSql = sSql + " AND IsIncrement = 1"; }
            if (IsRecommend) { sSql = sSql + " AND RecommendedBy>0"; }
            if (IsApprove) { sSql = sSql + " AND ApproveBy>0"; }

            if (sBusinessUnitIds != "") { sSql = sSql + " AND EmployeeID IN(SELECT  EmployeeID FROM View_Employee WHERE BusinessUnitID IN(" + sBusinessUnitIds + "))"; }
            if (sLocationID != "") { sSql = sSql + " AND EmployeeID IN(SELECT  EmployeeID FROM View_Employee WHERE LocationID IN(" + sLocationID + "))"; }
            if (sDepartmentIds != "") { sSql = sSql + " AND EmployeeID IN(SELECT  EmployeeID FROM View_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))"; }
            if (sDesignationIds != "") { sSql = sSql + " AND DesignationID IN(" + sDesignationIds + ")"; }

            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + ")";
            }
            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            _oTransferPromotionIncrements = TransferPromotionIncrement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);


            string EmpIDs = "";
            List<EmployeeSalaryStructure> oEmpSalaryStructures = new List<EmployeeSalaryStructure>();
            List<EmployeeSalaryStructureDetail> oEmpSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            if (_oTransferPromotionIncrements.Count > 0)
            {
                string TempEmpIDs = "";
                int nCount = 0;
                oEmpSalaryStructures = new List<EmployeeSalaryStructure>();
                foreach (TransferPromotionIncrement oItem in _oTransferPromotionIncrements)
                {
                    TempEmpIDs += oItem.EmployeeID + ",";
                    EmpIDs += oItem.EmployeeID + ",";
                    nCount++;

                    if (nCount % 100 == 0 || nCount == _oTransferPromotionIncrements.Count)
                    {
                        TempEmpIDs = TempEmpIDs.Remove(TempEmpIDs.Length - 1, 1);
                        sSql = "";
                        
                        List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
                        oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets("SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID IN("+TempEmpIDs+")", ((User)(Session[SessionInfo.CurrentUser])).UserID);

                        oEmpSalaryStructures.AddRange(oEmployeeSalaryStructures);
                        TempEmpIDs = "";
                    }
                }
                string sESSID = "";
                foreach(EmployeeSalaryStructure oItem in oEmpSalaryStructures) {
                    sESSID += oItem.ESSID + ",";
                }
                sESSID = sESSID.Remove(sESSID.Length - 1, 1);

                if (sESSID.Length > 0)
                {
                    oEmpSalaryStructureDetails = EmployeeSalaryStructureDetail.Gets("SELECT * FROM View_EmployeeSalaryStructureDetail WHERE ESSID IN(" + sESSID + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }

            List<string> ColBasics = new List<string>();
            ColBasics = oEmpSalaryStructureDetails.Where(x => ((int)x.SalaryHeadType == (int)EnumSalaryHeadType.Basic)).Select(x => x.SalaryHeadName).ToList();
            ColBasics = ColBasics.Distinct().ToList();

            List<string> ColAdditions = new List<string>();
            ColAdditions = oEmpSalaryStructureDetails.Where(x => ((int)x.SalaryHeadType == (int)EnumSalaryHeadType.Addition)).Select(x => x.SalaryHeadName).ToList();
            ColAdditions = ColAdditions.Distinct().ToList();

            List<string> ColDeductions = new List<string>();
            ColDeductions = oEmpSalaryStructureDetails.Where(x => (int)x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction).Select(x => x.SalaryHeadName).ToList();
            ColDeductions = ColDeductions.Distinct().ToList();


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
                var sheet = excelPackage.Workbook.Worksheets.Add("TPIExcel");
                sheet.Name = "TPIExcel";

                int nCount = 2;
                sheet.Column(nCount++).Width = 8; //SL
                sheet.Column(nCount++).Width = 15; //CODE
                sheet.Column(nCount++).Width = 22; //Name
                if (IsIncrement)
                {
                    sheet.Column(nCount++).Width = 15; //Previous Gross
                    sheet.Column(nCount++).Width = 15; //Increment Amount
                }
                sheet.Column(nCount++).Width = 15; //Gross

                if (ColBasics.Count > 0)
                {
                    for (int i = 0; i < ColBasics.Count; i++)
                    {
                        sheet.Column(nCount++).Width = 15;
                    }
                }
                if (ColAdditions.Count > 0)
                {
                    for (int i = 0; i < ColAdditions.Count; i++)
                    {
                        sheet.Column(nCount++).Width = 15;
                    }
                }
                if (ColDeductions.Count > 0)
                {
                    for (int i = 0; i < ColDeductions.Count; i++)
                    {
                        sheet.Column(nCount++).Width = 15;
                    }
                }
                oEmpSalaryStructures = oEmpSalaryStructures.OrderBy(x => x.EmployeeCode).ToList();

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
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "TPI Excel"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

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

                if (IsIncrement)
                {

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Previous Gross"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Increment Amount"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Present Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                foreach (string sItem in ColBasics)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = sItem; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                foreach (string sItem in ColAdditions)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = sItem; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                foreach (string sItem in ColDeductions)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = sItem; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex++;

                int nSL = 0;
                foreach (EmployeeSalaryStructure oItem in oEmpSalaryStructures)
                {
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

                    double nPreviousGross = Convert.ToDouble(_oTransferPromotionIncrements.Where(x => x.EmployeeID == oItem.EmployeeID).Select(x => x.GrossSalary).FirstOrDefault());
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nPreviousGross; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    double nIncrementAmount = oItem.GrossAmount - nPreviousGross;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nIncrementAmount; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossAmount; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    double nAmount;
                    foreach (string sItem in ColBasics)
                    {
                        var oESDs = oEmpSalaryStructureDetails.Where(x => x.SalaryHeadName == sItem && x.ESSID == oItem.ESSID).ToList();
                        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;

                        nAmount = Math.Round(nAmount);


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    foreach (string sItem in ColAdditions)
                    {
                        var oESDs = oEmpSalaryStructureDetails.Where(x => x.SalaryHeadName == sItem && x.ESSID == oItem.ESSID).ToList();
                        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;

                        nAmount = Math.Round(nAmount);


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    foreach (string sItem in ColDeductions)
                    {
                        var oESDs = oEmpSalaryStructureDetails.Where(x => x.SalaryHeadName == sItem && x.ESSID == oItem.ESSID).ToList();
                        nAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;

                        nAmount = Math.Round(nAmount);


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAmount; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    rowIndex++;
                }


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=TPIExcel.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }



        [HttpPost]
        public JsonResult GetsIncrementByPercent(TransferPromotionIncrement oTransferPromotionIncrement)
        {
            string sEmployeeIDs = oTransferPromotionIncrement.Note.Split('~')[0];
            int nSalaryHeadID = Convert.ToInt32(oTransferPromotionIncrement.Note.Split('~')[1]);
            int nPercent= Convert.ToInt32(oTransferPromotionIncrement.Note.Split('~')[2]);
            string sMonthIDs = oTransferPromotionIncrement.Note.Split('~')[3];
            string sYearIDs = oTransferPromotionIncrement.Note.Split('~')[4];
            string sBUIDs = oTransferPromotionIncrement.Note.Split('~')[5];
            string sLocationIDs = oTransferPromotionIncrement.Note.Split('~')[6];


            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            try
            {
                _oTransferPromotionIncrements = TransferPromotionIncrement.GetsIncrementByPercent(sEmployeeIDs, nSalaryHeadID, nPercent, sMonthIDs, sYearIDs, sBUIDs, sLocationIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oTransferPromotionIncrements.Count <= 0)
                {
                    throw new Exception("Data not found.");
                }
            }
            catch (Exception ex)
            {
                _oTransferPromotionIncrement = new TransferPromotionIncrement();
                _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
                _oTransferPromotionIncrement.ErrorMessage = ex.Message;
                _oTransferPromotionIncrements.Add(_oTransferPromotionIncrement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTransferPromotionIncrements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TransferPromotionIncrement_IU_Multiple(TransferPromotionIncrement oTransferPromotionIncrement)
        {

            string sEmployeeIDs = oTransferPromotionIncrement.Note.Split('~')[0];
            int nSalaryHeadID = Convert.ToInt32(oTransferPromotionIncrement.Note.Split('~')[1]);
            int nPercent = Convert.ToInt32(oTransferPromotionIncrement.Note.Split('~')[2]);
            string sMonthIDs = oTransferPromotionIncrement.Note.Split('~')[3];
            string sYearIDs = oTransferPromotionIncrement.Note.Split('~')[4];
            string sBUIDs = oTransferPromotionIncrement.Note.Split('~')[5];
            string sLocationIDs = oTransferPromotionIncrement.Note.Split('~')[6];


            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            TransferPromotionIncrement oTPI = new TransferPromotionIncrement();
            _oTransferPromotionIncrements = TransferPromotionIncrement.IUD_Multiple(sEmployeeIDs, nSalaryHeadID, nPercent, sMonthIDs, sYearIDs, sBUIDs, sLocationIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            if (_oTransferPromotionIncrements.Count > 0)
            {
                _oErrorListTPI = _oTransferPromotionIncrements;
            }
            else
            {

                oTPI = new TransferPromotionIncrement();
                oTPI.ErrorMessage = "Process Successful.";
            }

            //_oTransferPromotionIncrement = new TransferPromotionIncrement();
            //try
            //{
            //    _oTransferPromotionIncrement = oTransferPromotionIncrement;
            //    if (_oTransferPromotionIncrement.TPIID > 0)
            //    {
            //        _oTransferPromotionIncrement = _oTransferPromotionIncrement.IUD_Multiple((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //    }
            //    else
            //    {
            //        _oTransferPromotionIncrement = _oTransferPromotionIncrement.IUD_Multiple((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _oTransferPromotionIncrement = new TransferPromotionIncrement();
            //    _oTransferPromotionIncrement.ErrorMessage = ex.Message;
            //}
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTPI);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ErrorExcelIncrementByPercent()
        {
            if (_oErrorListTPI.Count > 0)
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

                    foreach (TransferPromotionIncrement oItem in _oErrorListTPI)
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
        }


        public void ExcelIncrementByPercent(string sParam)
        {
            string sEmployeeIDs = sParam.Split('~')[0];
            int nSalaryHeadID = Convert.ToInt32(sParam.Split('~')[1]);
            int nPercent = Convert.ToInt32(sParam.Split('~')[2]);
            string sMonthIDs = sParam.Split('~')[3];
            string sYearIDs = sParam.Split('~')[4];
            string sBUIDs = sParam.Split('~')[5];
            string sLocationIDs = sParam.Split('~')[6];


            _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
            _oTransferPromotionIncrements = TransferPromotionIncrement.GetsIncrementByPercent(sEmployeeIDs, nSalaryHeadID, nPercent, sMonthIDs, sYearIDs, sBUIDs, sLocationIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            int nRowIndex = 2, nStartCol = 2, nEndCol = 2;
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            ExcelFill fill;
            int colIndex = 1;
            int rowIndex = 1;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Proposed Increment");
                sheet.Name = "Proposed Increment";

                int n = 1;
                sheet.Column(n++).Width = 8;//sl
                sheet.Column(n++).Width = 20;//code
                sheet.Column(n++).Width = 30;//name
                sheet.Column(n++).Width = 30;//bu
                sheet.Column(n++).Width = 30;//location
                sheet.Column(n++).Width = 20;//effect date
                sheet.Column(n++).Width = 20;//previous gross
                sheet.Column(n++).Width = 20;//previous basic
                sheet.Column(n++).Width = 20;//incremented gross
                sheet.Column(n++).Width = 20;//incremented basic

                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BU Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Effect Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Previous Gross"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Previous Basic"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                if (nPercent > 0)
                {

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Incremented Gross"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Incremented Basic"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }
                rowIndex++;

                colIndex = 1;
                if (_oTransferPromotionIncrements.Count() > 0)
                {
                    int nSl = 0;
                    foreach (TransferPromotionIncrement oItem in _oTransferPromotionIncrements)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ++nSl; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BUName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ActualEffectedDateInString; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.GrossSalary; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BasicAmount; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if (nPercent > 0)
                        {

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TPIGrossSalary; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TPIBasicAmount; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        }
                        rowIndex++;
                    }
                }
                rowIndex++;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Proposed_Increment.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }


        }

    }
}
