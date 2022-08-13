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
	public class FARegisterController : Controller
	{
		#region Declaration
        FARegister _oFARegister = new FARegister();
        List<FARegister> _oFARegisters = new List<FARegister>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewFARegisters(int buid, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			_oFARegisters = new List<FARegister>(); 
			_oFARegisters = FARegister.Gets((int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            //ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FAMethods = EnumObject.jGets(typeof(EnumFAMethod));
            ViewBag.DEPCalculateOns = EnumObject.jGets(typeof(EnumDEPCalculateOn));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oFARegisters);
		}
		public ActionResult ViewFARegister(int id)
		{
			_oFARegister = new FARegister();
            if (id > 0)
            {
                _oFARegister = _oFARegister.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else {
                _oFARegister.CurrencyID = (int)Session[SessionInfo.BaseCurrencyID];
                //_oFARegister.CRate = 1;
            }

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));

            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FAMethods = EnumObject.jGets(typeof(EnumFAMethod));
            ViewBag.DEPCalculateOns = EnumObject.jGets(typeof(EnumDEPCalculateOn));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oFARegister);
		}
        public ActionResult ViewFARegisterLog(int id)
        {
            _oFARegister = new FARegister();
            if (id > 0)
            {
                _oFARegister = _oFARegister.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oFARegister.CurrencyID = (int)Session[SessionInfo.BaseCurrencyID];
            }

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));

            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FAMethods = EnumObject.jGets(typeof(EnumFAMethod));
            ViewBag.DEPCalculateOns = EnumObject.jGets(typeof(EnumDEPCalculateOn));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.MeasurementUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFARegister);
        }
      
        [HttpPost]
		public JsonResult Save(FARegister oFARegister)
		{
			_oFARegister = new FARegister();
			try
			{
				_oFARegister = oFARegister;
				_oFARegister = _oFARegister.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFARegister = new FARegister();
				_oFARegister.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFARegister);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult RequestForRevise(FARegister oFARegister)
        {
            _oFARegister = new FARegister();
            try
            {
                _oFARegister = oFARegister;
                if (_oFARegister.FAStatus == EnumFAStatus.Approved)
                {
                    _oFARegister = _oFARegister.RequestForRevise((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oFARegister = new FARegister();
                }
            }
            catch (Exception ex)
            {
                _oFARegister = new FARegister();
                _oFARegister.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFARegister);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Revise(FARegister oFARegister)
        {
            _oFARegister = new FARegister();
            try
            {
                _oFARegister = oFARegister;
                if (_oFARegister.FAStatus == EnumFAStatus.ReqForRevice)
                {
                    _oFARegister = _oFARegister.Revise((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oFARegister.ErrorMessage = "This is not in Request For Revise Status";
                }
            }
            catch (Exception ex)
            {
                _oFARegister = new FARegister();
                _oFARegister.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFARegister);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
        [HttpPost]
        public JsonResult RequestForApprove(FARegister oFARegister)
        {
            _oFARegister = new FARegister();
            try
            {
                _oFARegister = oFARegister;
                if (_oFARegister.FAStatus == EnumFAStatus.None || _oFARegister.FAStatus == EnumFAStatus.Initialized || _oFARegister.FAStatus == EnumFAStatus.ReqForRevice)
                {
                    _oFARegister = _oFARegister.RequestForApprove((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oFARegister = new FARegister();
                }
            }
            catch (Exception ex)
            {
                _oFARegister = new FARegister();
                _oFARegister.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFARegister);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
		[HttpPost]
		public JsonResult Delete(FARegister oFARegister)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oFARegister.Delete(oFARegister.FARegisterID, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult GetFACode(FARegister oFARegister)
        {
            _oFARegister = new FARegister();
            
            try
            {
                if (oFARegister.ProductID > 0 && oFARegister.BUID > 0 && oFARegister.LocationID > 0 && oFARegister.PurchaseDateInString != null)
                {
                    _oFARegister.FACodeFull = _oFARegister.GetFACode(oFARegister, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                
            }
            catch (Exception ex)
            {
                _oFARegister.ErrorMessage = ex.Message;
            }
            var jsonResult = Json(_oFARegister, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsProductByType(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                string sSQL = "SELECT * FROM View_Product WHERE ProductType=2 ";

                if (!string.IsNullOrEmpty(oProduct.ProductName))
                    sSQL += " AND ProductName Like '%" + oProduct.ProductName + "%'";

                oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsLog(FARegister oFARegister)
        {
            List<FARegister> oFARegisters = new List<FARegister>();
            try
            {
                string sSQL = "SELECT * FROM View_FARegisterLog WHERE FARegisterID = " + oFARegister.FARegisterID;
                oFARegisters = FARegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFARegister = new FARegister();
                oFARegister.ErrorMessage = ex.Message;
                oFARegisters.Add(oFARegister);
            }
            var jsonResult = Json(oFARegisters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsDepartment(Department oDepartment)
        {
            List<Department> oDepartments = new List<Department>();
            try
            {
                string sSQL = "SELECT * FROM Department WHERE ParentID!=0 ";

                if (!string.IsNullOrEmpty(oDepartment.Name))
                    sSQL += " AND Name Like '%" + oDepartment.Name + "%'";

                oDepartments = Department.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDepartment = new Department();
                oDepartment.ErrorMessage = ex.Message;
                oDepartments.Add(oDepartment);
            }
            var jsonResult = Json(oDepartments, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult FA_Process(FARegister oFARegister)
        {
            _oFARegisters = new List<FARegister>();
            try
            {
                _oFARegisters = FARegister.FA_Process(oFARegister,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFARegister = new FARegister();
                _oFARegister.ErrorMessage = ex.Message;
                _oFARegisters.Add(_oFARegister);
            }
            var jsonResult = Json(_oFARegisters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
       
        [HttpPost]
        public JsonResult FARProcess(FARegister oFARegister)
		{
			_oFARegister = new FARegister();
			try
			{
				_oFARegister = oFARegister;
                _oFARegister = _oFARegister.FARProcess((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFARegister = new FARegister();
				_oFARegister.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFARegister);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        #endregion

        #region FASchedule & Report
        public ActionResult ViewFASchedules(int FARID)
        {
            List<FASchedule> oFARules = new List<FASchedule>();
            try
            {
                if (FARID > 0)
                {
                    _oFARegister = _oFARegister.Get(FARID, (int)Session[SessionInfo.currentUserID]);
                    oFARules = FASchedule.Gets(_oFARegister.FARegisterID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex) { }

            ViewBag.FARegister = _oFARegister;
            return View(oFARules);
        }
        public ActionResult ViewReviseHistorys(int FARID)
        {
            List<FASchedule> oFARules = new List<FASchedule>();
            try
            {
                if (FARID > 0)
                {
                    _oFARegister = _oFARegister.Get(FARID, (int)Session[SessionInfo.currentUserID]);
                    oFARules = FASchedule.Gets(_oFARegister.FARegisterID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex) { }

            ViewBag.FARegister = _oFARegister;
            return View(oFARules);
        }
        [HttpPost]
        public JsonResult SaveFASchedules(FARegister oFARegister)
        {
            List<FASchedule> oFASchedules = new List<FASchedule>();
            try
            {
                if (oFARegister.FARegisterID > 0)
                {
                    oFASchedules = FASchedule.SaveFASchedules(oFARegister.FARegisterID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
			{
                FASchedule oFASchedule = new FASchedule();
                oFASchedule.ErrorMessage = ex.Message;
                oFASchedules.Add(oFASchedule);
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFASchedules);
			return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Adv Search
        [HttpPost]
        public JsonResult AdvSearch(FARegister oFARegister)
        {
            List<FARegister> oFARegisters = new List<FARegister>();
            FARegister _oFARegister = new FARegister();
            string sSQL = MakeSQL(oFARegister);
            if (sSQL == "Error")
            {
                _oFARegister = new FARegister();
                _oFARegister.ErrorMessage = "Please select a searching critaria.";
                oFARegisters = new List<FARegister>();
            }
            else
            {
                oFARegisters = new List<FARegister>();
                oFARegisters = FARegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFARegisters.Count == 0)
                {
                    oFARegisters = new List<FARegister>();
                }
            }
            var jsonResult = Json(oFARegisters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FARegister oFARegister)
        {
            string sParams = oFARegister.Params;

            int nDateCriteria_Issue = 0;

            string sSLNo = "",
                   sCodeNo = "",
                   sProductIDs = "";

            DateTime dStart_Issue = DateTime.Today,
                     dEnd_Issue = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sSLNo = sParams.Split('~')[nCount++];
                sCodeNo = sParams.Split('~')[nCount++];
                sProductIDs = sParams.Split('~')[nCount++];
                nDateCriteria_Issue = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT * FROM View_FARegister AS EB";
            string sReturn = "";

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.PurchaseDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            #endregion

            #region sSLNo
            if (!string.IsNullOrEmpty(sSLNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.SLNo LIKE '%" + sSLNo + "%'";
            }
            #endregion

            #region sCodeNo
            if (!string.IsNullOrEmpty(sCodeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.FACodeFull LIKE '%" + sCodeNo + "%'";
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ProductID IN (" + sProductIDs + ") ";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region GRNDetail
        [HttpPost]
        public JsonResult GetGRNDetails(string StartDate, string EndDate, int BUID, int ProductID)
        {
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            string sSQL = MakeSQL(StartDate, EndDate, BUID, ProductID);

            try
            {
                if (sSQL != "")
                {
                    oGRNDetails = GRNDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                GRNDetail oGRNDetail = new GRNDetail();
                oGRNDetail.ErrorMessage = ex.Message;
                oGRNDetails.Add(oGRNDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGRNDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(string StartDate, string EndDate, int BUID, int ProductID)
        {
            string sReturn1 = "SELECT * FROM View_GRNDetail AS HH WHERE HH.ProductType = 2";
            string sReturn = "";


            #region ReceivedDateTime
            sReturn = " AND HH.GRNDetailID NOT IN (SELECT ISNULL(HH.GRNDetailID,0) FROM FARegister AS HH) AND	HH.GRNID IN (SELECT GRN.GRNID FROM GRN WHERE CONVERT(DATE,CONVERT(VARCHAR(12),GRN.ReceivedDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + StartDate + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + EndDate + "',106)) AND ISNULL(GRN.BUID,0) = " + BUID + " AND ISNULL(GRN.ReceivedBy,0) !=0)";
            #endregion
            if (ProductID!=0)
            {
                sReturn += " AND HH.ProductID = " + ProductID;
            }
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        public JsonResult FAGRNProcess(List<GRNDetail> oGRNDetails)
        {
            List<FARegister> oFARegisters = new List<FARegister>();
            try
            {
                oFARegisters = FARegister.FAGRNProcess(oGRNDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FARegister oFARegister = new FARegister();
                oFARegister.ErrorMessage = ex.Message;
                oFARegisters.Add(oFARegister);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFARegisters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult Print(int id)
        {
            FARegister oFARegister = new FARegister();
            oFARegister = oFARegister.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oFARegister.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptFARegister oReport = new rptFARegister();
            //byte[] abytes = oReport.PrepareReport(oFARegister, oCompany, oBusinessUnit);
            byte[] abytes = oReport.PrepareReportNew(oFARegister, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintLogSchedule(int id)
        {
            FARegister oFARegister = new FARegister();
            oFARegister = oFARegister.GetLogByLogID(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oFARegister.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<FASchedule> oFASchedules = new List<FASchedule>();
            oFASchedules = FASchedule.Gets(oFARegister.FARegisterID, (int)Session[SessionInfo.currentUserID]);
            
            rptFARegisterSchedule oReport = new rptFARegisterSchedule();
            byte[] abytes = oReport.PrepareReport(oFASchedules, oFARegister, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintSchedules(int id)
        {
            FARegister oFARegister = new FARegister();
            oFARegister = oFARegister.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oFARegister.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<FASchedule> oFASchedules = new List<FASchedule>();
            oFASchedules = FASchedule.Gets(oFARegister.FARegisterID, (int)Session[SessionInfo.currentUserID]);

            rptFARegisterSchedule oReport = new rptFARegisterSchedule();
            byte[] abytes = oReport.PrepareReport(oFASchedules, oFARegister, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        #endregion
    }
}
