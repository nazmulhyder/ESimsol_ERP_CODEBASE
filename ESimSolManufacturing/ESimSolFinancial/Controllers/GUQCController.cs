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
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class GUQCController : Controller
    {
        #region Declaration

        GUQC _oGUQC = new GUQC();
        List<GUQC> _oGUQCs = new List<GUQC>();
        GUQCDetail _oGUQCDetail = new GUQCDetail();
        List<GUQCDetail> _oGUQCDetails = new List<GUQCDetail>();
        OrderRecap _oOrderRecap = new OrderRecap();
        List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
        List<User> _oUsers = new List<User>();
        User _oUser = new User();
        List<Employee> _oEmployees = new List<Employee>();
        List<GUQCRegister> _oGUQCRegisters = new List<GUQCRegister>();
        GUQCRegister _oGUQCRegister = new GUQCRegister();
        string _sDateRange = "";
        #endregion

        #region Actions

        public ActionResult ViewGUQCs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GUQC).ToString() + "," + ((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oGUQCs = new List<GUQC>();
            string sSQL = "SELECT * FROM View_GUQC AS HH WHERE HH.BUID = "+buid.ToString()+" AND ISNULL(HH.ApproveBy,0) =0 ORDER BY HH.GUQCID ASC";
            _oGUQCs = GUQC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.GUQC, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.Stores = oWorkingUnits;
            _oEmployees = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.QC_Person + " AND IsActive = 1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oUsers = ESimSol.BusinessObjects.User.GetsBySql("SELECT * FROM View_User", (int)Session[SessionInfo.currentUserID]);
            ViewBag.Employees = _oEmployees;
            ViewBag.Users = _oUsers;
            ViewBag.CompareOperator = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oGUQCs);
        }

        public ActionResult ViewGUQC(int id, int buid)
        {
            _oGUQC = new GUQC();
            if (id > 0)
            {
                _oGUQC = _oGUQC.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oGUQC.GUQCDetails = GUQCDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oGUQC.BUID = buid;

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.GUQC, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.Stores = oWorkingUnits;
            List<Employee> _oEmployees = new List<Employee>();
            _oEmployees = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.QC_Person + " AND IsActive = 1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Employees = _oEmployees;
            return View(_oGUQC);
        }

        public ActionResult ViewGUQCRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oGUQC = new GUQC();
            _oGUQC.BUID = buid;

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.GUQC, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.Stores = oWorkingUnits;
            List<Employee> _oEmployees = new List<Employee>();
            _oEmployees = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeDesignationType = " + (int)EnumEmployeeDesignationType.QC_Person + " AND IsActive = 1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Employees = _oEmployees;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.Order_Wise_Details || (EnumReportLayout)oItem.id == EnumReportLayout.Style_Wise_Details)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            return View(_oGUQC);
        }
        #endregion

        #region Functions
        [HttpPost]
        public JsonResult Save(GUQC oGUQC)
        {
            _oGUQC = new GUQC();
            try
            {
                _oGUQC = oGUQC;
                _oGUQC = oGUQC.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUQC = new GUQC();
                _oGUQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(GUQC oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                GUQC oGUQC = new GUQC();
                sFeedBackMessage = oGUQC.Delete(oJB.GUQCID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approve(GUQC oGUQC)
        {
            _oGUQC = new GUQC();
            try
            {
                _oGUQC = oGUQC;
                _oGUQC = oGUQC.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUQC = new GUQC();
                _oGUQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult GetsDetailsByID(GUQCDetail oGUQCDetail)//Id=ContractorID
        {
            try
            {
                string Ssql = "SELECT*FROM View_GUQCDetail WHERE GUQCID=" + oGUQCDetail.GUQCID + " ";
                _oGUQCDetails = new List<GUQCDetail>();
                _oGUQCDetail.GUQCDetails = GUQCDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oGUQCDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUQCDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOrderRecaps(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            try
            {
                string sSQL = "SELECT * FROM View_OrderRecap WHERE DBServerDateTime >= '01 Jan 2018' AND BuyerID = " + oOrderRecap.BuyerID + " AND ISNULL(ApproveBy,0) != 0 AND ISNULL(ISNULL(OrderRecapNo,'')+ISNULL(StyleNo,''),'') LIKE '%" + oOrderRecap.OrderRecapNo + "%' ";
                //if (!string.IsNullOrEmpty(oOrderRecap.OrderRecapNo))
                //{
                //    sSQL += " AND OrderRecapNo LIKE '%" + oOrderRecap.OrderRecapNo + "%'";
                //}
                sSQL += " ORDER BY OrderRecapID";

                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                OrderRecap oTempOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = "ex";
                _oOrderRecaps.Add(oTempOrderRecap);
            }

            var jsonResult = Json(_oOrderRecaps, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region AdvanceSearch
        [HttpPost]
        public JsonResult Search(GUQC oGUQC)
        {
            _oGUQCs = new List<GUQC>();
            try
            {
                string sSQL = GetSQLAdvSrc(oGUQC.ErrorMessage);
                _oGUQCs = GUQC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUQC = new GUQC();
                _oGUQC.ErrorMessage = ex.Message;
                _oGUQCs.Add(_oGUQC);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oGUQCs);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jSonResult = Json(_oGUQCs, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        private string GetSQLAdvSrc(string sTemp)
        {

            string sQCNo = sTemp.Split('~')[0];

            int nQCDateMenu = Convert.ToInt32(sTemp.Split('~')[1]);
            DateTime dQCDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            DateTime dQCDateTo = Convert.ToDateTime(sTemp.Split('~')[3]);

            string sBuyerIDs = sTemp.Split('~')[4];
            int nStoreID = Convert.ToInt32(sTemp.Split('~')[5]);
            int nQCByID = Convert.ToInt32(sTemp.Split('~')[6]);

            int nApproveByID = Convert.ToInt32(sTemp.Split('~')[7]);

            int nApproveDateMenu = Convert.ToInt32(sTemp.Split('~')[8]);
            DateTime dApproveDate = Convert.ToDateTime(sTemp.Split('~')[9]);
            DateTime dApproveDateTo = Convert.ToDateTime(sTemp.Split('~')[10]);


            string sReturn1 = "SELECT * FROM View_GUQC";
            string sReturn = "";

            if (sQCNo != "undefined" && sQCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " QCNo LIKE '%" + sQCNo + "%' ";
            }

            #region qc Date Wise

            if (nQCDateMenu > 0)
            {
                if (nQCDateMenu == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " QCDate = '" + dQCDate.ToString("dd MMM yyyy") + "'";
                }
                if (nQCDateMenu == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " QCDate != '" + dQCDate.ToString("dd MMM yyyy") + "'";
                }
                if (nQCDateMenu == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " QCDate > '" + dQCDate.ToString("dd MMM yyyy") + "'";
                }
                if (nQCDateMenu == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " QCDate < '" + dQCDate.ToString("dd MMM yyyy") + "'";
                }
                if (nQCDateMenu == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " QCDate>= '" + dQCDate.ToString("dd MMM yyyy") + "' AND QCDate < '" + dQCDateTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nQCDateMenu == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (QCDate< '" + dQCDate.ToString("dd MMM yyyy") + "' OR QCDate > '" + dQCDateTo.AddDays(1).ToString("dd MMM yyyy") + "') ";
                }
            }
            #endregion

            if (sBuyerIDs != "undefined" && sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ") ";
            }

            if (nStoreID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StoreID = " + nStoreID + " ";
            }

            if (nQCByID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " QCBy = " + nQCByID + " ";
            }

            if (nApproveByID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApproveBy = " + nApproveByID + " ";
            }

            #region approve Date Wise

            if (nApproveDateMenu > 0)
            {
                if (nApproveDateMenu == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate = '" + dApproveDate.ToString("dd MMM yyyy") + "'";
                }
                if (nApproveDateMenu == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate != '" + dApproveDate.ToString("dd MMM yyyy") + "'";
                }
                if (nApproveDateMenu == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate > '" + dApproveDate.ToString("dd MMM yyyy") + "'";
                }
                if (nApproveDateMenu == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate < '" + dApproveDate.ToString("dd MMM yyyy") + "'";
                }
                if (nApproveDateMenu == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate>= '" + dApproveDate.ToString("dd MMM yyyy") + "' AND ApproveDate < '" + dApproveDateTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nApproveDateMenu == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ApproveDate< '" + dApproveDate.ToString("dd MMM yyyy") + "' OR ApproveDate > '" + dApproveDateTo.AddDays(1).ToString("dd MMM yyyy") + "') ";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region print
        [HttpPost]
        public ActionResult SetGUQCListData(GUQC oGUQC)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oGUQC);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintGUQCs()
        {
            _oGUQC = new GUQC();
            try
            {
                _oGUQC = (GUQC)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_GUQC WHERE GUQCID IN (" + _oGUQC.ErrorMessage + ") Order By GUQCID";
                _oGUQCs = GUQC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGUQC = new GUQC();
                _oGUQCs = new List<GUQC>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oGUQC.Company = oCompany;
            if (_oGUQCs.Count > 0)
            {
                rptGUQCs oReport = new rptGUQCs();
                byte[] abytes = oReport.PrepareReport(_oGUQCs, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult GUQCPrintPreview(int id)
        {
            _oGUQC = new GUQC();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oGUQC = _oGUQC.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oGUQC.GUQCDetails = GUQCDetail.Gets(_oGUQC.GUQCID, (int)Session[SessionInfo.currentUserID]);
                //_oGUQC.BusinessUnit = oBusinessUnit.Get(_oGUQC.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                rptErrorMessage oReportErr = new rptErrorMessage();
                byte[] errorBytes = oReportErr.PrepareReport("No Data Found!!");
                return File(errorBytes, "application/pdf");
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oGUQC.Company = oCompany;
            byte[] abytes;
            rptGUQC oReport = new rptGUQC();
            abytes = oReport.PrepareReport(_oGUQC, oCompany);
            return File(abytes, "application/pdf");
        }
        public Image GetCompanyLogo(Company oCompany)
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

        #region GUQC register

        public ActionResult SetSessionSearchCriteria(GUQCRegister oGUQCRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oGUQCRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintGUQCRegister(double ts)
        {
            GUQCRegister oGUQCRegister = new GUQCRegister();
            string _sErrorMesage = "";
            try
            {
                _oGUQCRegisters = new List<GUQCRegister>();
                oGUQCRegister = (GUQCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oGUQCRegister);
                _oGUQCRegisters = GUQCRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oGUQCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oGUQCRegisters = new List<GUQCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oGUQCRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oGUQCRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptGUQCRegisters oReport = new rptGUQCRegisters();
                byte[] abytes = oReport.PrepareReport(_oGUQCRegisters, oCompany, oGUQCRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
                //return null;
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        private string GetSQL(GUQCRegister oGUQCRegister)
        {
            //string _sDateRange = "";
            string sShipmentDateRange = "";
            string sSearchingData = oGUQCRegister.ErrorMessage;
            EnumCompareOperator eGUQCDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dGUQCStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dGUQCEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            EnumCompareOperator eShipmentDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[9]);
            DateTime dShipmentStartDate = Convert.ToDateTime(sSearchingData.Split('~')[10]);
            DateTime dShipmentEndDate = Convert.ToDateTime(sSearchingData.Split('~')[11]);
            #region make date range
            if (eShipmentDate == EnumCompareOperator.EqualTo)
            {
                sShipmentDateRange = "Shipment Date: " + dShipmentStartDate.ToString("dd MMM yyyy");
            }
            else if (eShipmentDate == EnumCompareOperator.Between)
            {
                sShipmentDateRange = "Shipment Date: " + dShipmentStartDate.ToString("dd MMM yyyy") + " - To - " + dShipmentEndDate.ToString("dd MMM yyyy");
            }
            else if (eShipmentDate == EnumCompareOperator.NotEqualTo)
            {
                sShipmentDateRange = "Shipment Date: Not Equal to " + dShipmentStartDate.ToString("dd MMM yyyy");
            }
            else if (eShipmentDate == EnumCompareOperator.GreaterThan)
            {
                sShipmentDateRange = "Shipment Date: Greater Than " + dShipmentStartDate.ToString("dd MMM yyyy");
            }
            else if (eShipmentDate == EnumCompareOperator.SmallerThan)
            {
                sShipmentDateRange = "Shipment Date: Smaller Than " + dShipmentStartDate.ToString("dd MMM yyyy");
            }
            else if (eShipmentDate == EnumCompareOperator.NotBetween)
            {
                sShipmentDateRange = "Shipment Date: Not Between " + dShipmentStartDate.ToString("dd MMM yyyy") + " - To - " + dShipmentEndDate.ToString("dd MMM yyyy");
            }
            #endregion
            #region make date range
            if (eGUQCDate == EnumCompareOperator.EqualTo)
            {
                _sDateRange = "QC Date: " + dGUQCStartDate.ToString("dd MMM yyyy");
            }
            else if (eGUQCDate == EnumCompareOperator.Between)
            {
                _sDateRange = "QC Date: " + dGUQCStartDate.ToString("dd MMM yyyy") + " - To - " + dGUQCEndDate.ToString("dd MMM yyyy");
            }
            else if (eGUQCDate == EnumCompareOperator.NotEqualTo)
            {
                _sDateRange = "QC Date: Not Equal to " + dGUQCStartDate.ToString("dd MMM yyyy");
            }
            else if (eGUQCDate == EnumCompareOperator.GreaterThan)
            {
                _sDateRange = "QC Date: Greater Than " + dGUQCStartDate.ToString("dd MMM yyyy");
            }
            else if (eGUQCDate == EnumCompareOperator.SmallerThan)
            {
                _sDateRange = "QC Date: Smaller Than " + dGUQCStartDate.ToString("dd MMM yyyy");
            }
            else if (eGUQCDate == EnumCompareOperator.NotBetween)
            {
                _sDateRange = "QC Date: Not Between " + dGUQCStartDate.ToString("dd MMM yyyy") + " - To - " + dGUQCEndDate.ToString("dd MMM yyyy");
            }
            #endregion

            EnumCompareOperator eQCQty = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            double nQCQtyStsrt = Convert.ToDouble(sSearchingData.Split('~')[4]);
            double nQCQtyEnd = Convert.ToDouble(sSearchingData.Split('~')[5]);

            EnumCompareOperator eRejectQty = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            double nRejectQtyStsrt = Convert.ToDouble(sSearchingData.Split('~')[7]);
            double nRejectQtyEnd = Convert.ToDouble(sSearchingData.Split('~')[8]);

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oGUQCRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oGUQCRegister.BUID.ToString();
            }
            #endregion

            #region GUQCNo
            if (oGUQCRegister.QCNo != null && oGUQCRegister.QCNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " QCNo LIKE'%" + oGUQCRegister.QCNo + "%'";
            }
            #endregion

            #region QCBy
            if (oGUQCRegister.QCBy != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " QCBy =" + oGUQCRegister.QCBy.ToString();
            }
            #endregion

            #region StoreID
            if (oGUQCRegister.StoreID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " StoreID =" + oGUQCRegister.StoreID.ToString();
            }
            #endregion

            #region BuyerID
            if (oGUQCRegister.BuyerID != 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BuyerID =" + oGUQCRegister.BuyerID.ToString();
            }
            #endregion

            #region StyleNo
            if (oGUQCRegister.StyleNo != null && oGUQCRegister.StyleNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " StyleNo LIKE'%" + oGUQCRegister.StyleNo + "%'";
            }
            #endregion

            #region OrderRecapNo
            if (oGUQCRegister.OrderRecapNo != null && oGUQCRegister.OrderRecapNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " OrderRecapNo LIKE'%" + oGUQCRegister.OrderRecapNo + "%'";
            }
            #endregion

            #region Remarks
            if (oGUQCRegister.Remarks != null && oGUQCRegister.Remarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " Remarks LIKE'%" + oGUQCRegister.Remarks + "%'";
            }
            #endregion

            #region POWiseRemarks
            if (oGUQCRegister.POWiseRemarks != null && oGUQCRegister.POWiseRemarks != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " POWiseRemarks LIKE'%" + oGUQCRegister.POWiseRemarks + "%'";
            }
            #endregion

            #region Shipment Date
            if (eShipmentDate != EnumCompareOperator.None)
            {
                if (eShipmentDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dShipmentStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eShipmentDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dShipmentStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eShipmentDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dShipmentStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eShipmentDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dShipmentStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eShipmentDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dShipmentStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dShipmentEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eShipmentDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dShipmentStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dShipmentEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion


            #region QC Date
            if (eGUQCDate != EnumCompareOperator.None)
            {
                if (eGUQCDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGUQCStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGUQCDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGUQCStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGUQCDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGUQCStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGUQCDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGUQCStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGUQCDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGUQCStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGUQCEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eGUQCDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGUQCStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dGUQCEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region QC Qty
            if (eQCQty != EnumCompareOperator.None)
            {
                if (eQCQty == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " QCQty = " + nQCQtyStsrt.ToString("0.00");
                }
                else if (eQCQty == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " QCQty != " + nQCQtyStsrt.ToString("0.00"); ;
                }
                else if (eQCQty == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " QCQty > " + nQCQtyStsrt.ToString("0.00"); ;
                }
                else if (eQCQty == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " QCQty < " + nQCQtyStsrt.ToString("0.00"); ;
                }
                else if (eQCQty == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " QCQty BETWEEN " + nQCQtyStsrt.ToString("0.00") + " AND " + nQCQtyEnd.ToString("0.00");
                }
                else if (eQCQty == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " QCQty NOT BETWEEN " + nQCQtyStsrt.ToString("0.00") + " AND " + nQCQtyEnd.ToString("0.00");
                }
            }
            #endregion

            #region Reject Qty
            if (eRejectQty != EnumCompareOperator.None)
            {
                if (eRejectQty == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " RejectQty = " + nRejectQtyStsrt.ToString("0.00");
                }
                else if (eRejectQty == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " RejectQty != " + nRejectQtyStsrt.ToString("0.00"); ;
                }
                else if (eRejectQty == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " RejectQty > " + nRejectQtyStsrt.ToString("0.00"); ;
                }
                else if (eRejectQty == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " RejectQty < " + nRejectQtyStsrt.ToString("0.00"); ;
                }
                else if (eRejectQty == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " RejectQty BETWEEN " + nRejectQtyStsrt.ToString("0.00") + " AND " + nRejectQtyEnd.ToString("0.00");
                }
                else if (eRejectQty == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " RejectQty NOT BETWEEN " + nRejectQtyStsrt.ToString("0.00") + " AND " + nRejectQtyEnd.ToString("0.00");
                }
            }
            #endregion

            #region Report Layout
            if (oGUQCRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_GUQCRegister ";
                sOrderBy = " ORDER BY  QCDate, GUQCID, GUQCDetailID ASC";
            }

            else if (oGUQCRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_GUQCRegister ";
                sOrderBy = " ORDER BY  BuyerName, GUQCID, GUQCDetailID ASC";
            }
            else if (oGUQCRegister.ReportLayout == EnumReportLayout.Order_Wise_Details)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_GUQCRegister ";
                sOrderBy = " ORDER BY  OrderRecapNo, GUQCID, GUQCDetailID ASC";
            }
            else if (oGUQCRegister.ReportLayout == EnumReportLayout.Style_Wise_Details)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_GUQCRegister ";
                sOrderBy = " ORDER BY  StyleNo, GUQCID, GUQCDetailID ASC";
            }
            else
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_GUQCRegister ";
                sOrderBy = " ORDER BY QCDate, GUQCID, GUQCDetailID ASC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }

        public void ExportToExcelGUQC()
        {
            GUQCRegister oGUQCRegister = new GUQCRegister();
            string _sErrorMesage = "";
            try
            {
                _oGUQCRegisters = new List<GUQCRegister>();
                oGUQCRegister = (GUQCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oGUQCRegister);
                _oGUQCRegisters = GUQCRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oGUQCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oGUQCRegisters = new List<GUQCRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCompany.CompanyLogo = GetCompanyLogo(_oCompany);
                if (oGUQCRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oGUQCRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                double GrandTotalOrderQty = 0, GrandTotalQCPassQty = 0, GrandTotalRejectQty = 0;
                int count = 0, num = 0;
                double SubTotalOrderQty = 0, SubTotalQCPassQty = 0, SubTotalRejectQty = 0;
                double TotalOrderQty = 0, TotalQCPassQty = 0, TotalRejectQty = 0;
                string sQCNo = "";

                if (oGUQCRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Date Wise GU QC Register(Details)");
                        sheet.Name = "Date Wise QC Register(Details)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 10; //qc no
                        sheet.Column(4).Width = 20; //qc by
                        sheet.Column(5).Width = 25; //Store
                        sheet.Column(6).Width = 20; //buyer
                        sheet.Column(7).Width = 20; //Style
                        sheet.Column(8).Width = 15; //PO
                        sheet.Column(9).Width = 13; //Order qty
                        sheet.Column(10).Width = 13; //QC qty
                        sheet.Column(11).Width = 13; //Reject qty

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Wise QC Register(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "QC No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "QC By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "PO/Recap No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oGUQCRegisters.Count > 0)
                        {
                            var data = _oGUQCRegisters.GroupBy(x => new { x.QCDateInString }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                            {
                                QCDate = key.QCDateInString,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "QC Date : @ " + oData.QCDate; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    if (sQCNo != "")
                                    {
                                        if (sQCNo != oItem.QCNo && count > 1)
                                        {
                                            cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            rowIndex = rowIndex + 1;
                                            SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                        }
                                    }
                                    #endregion

                                    if (sQCNo != oItem.QCNo)
                                    {
                                        num++;
                                        int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo)-1);
                                        cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    }
                                    cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalOrderQty += oItem.TotalQuantity;
                                    TotalOrderQty += oItem.TotalQuantity;
                                    GrandTotalOrderQty += oItem.TotalQuantity;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalQCPassQty += oItem.QCPassQty;
                                    TotalQCPassQty += oItem.QCPassQty;
                                    GrandTotalQCPassQty += oItem.QCPassQty;

                                    cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalRejectQty += oItem.RejectQty;
                                    TotalRejectQty += oItem.RejectQty;
                                    GrandTotalRejectQty += oItem.RejectQty;

                                    rowIndex++;
                                    sQCNo = oItem.QCNo;
                                }
                                #region subtotal
                                if (sQCNo != "")
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex = rowIndex + 1;
                                    SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                }
                                #endregion

                                #region total
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Date Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 9]; cell.Value = TotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 10]; cell.Value = TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 11]; cell.Value = TotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                rowIndex = rowIndex + 1;
                                #endregion

                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Date_Wise_GU_QC_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                else if (oGUQCRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Party Wise GU QC Register(Details)");
                        sheet.Name = "Party Wise GU QC Register(Details)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 10; //qc no
                        sheet.Column(4).Width = 20; //qc by
                        sheet.Column(5).Width = 25; //Store
                        sheet.Column(6).Width = 12; //qc date
                        sheet.Column(7).Width = 20; //Style
                        sheet.Column(8).Width = 15; //PO
                        sheet.Column(9).Width = 13; //Order qty
                        sheet.Column(10).Width = 13; //QC qty
                        sheet.Column(11).Width = 13; //Reject qty

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Party Wise QC Register(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "QC No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "QC By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "QC Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "PO/Recap No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oGUQCRegisters.Count > 0)
                        {
                            var data = _oGUQCRegisters.GroupBy(x => new { x.BuyerName }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                            {
                                buyerName = key.BuyerName,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Party : @ " + oData.buyerName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    if (sQCNo != "")
                                    {
                                        if (sQCNo != oItem.QCNo && count > 1)
                                        {
                                            cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                            rowIndex = rowIndex + 1;
                                            SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                        }
                                    }
                                    #endregion

                                    if (sQCNo != oItem.QCNo)
                                    {
                                        num++;
                                        int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo) - 1);
                                        cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.QCDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    }
                                    cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalOrderQty += oItem.TotalQuantity;
                                    TotalOrderQty += oItem.TotalQuantity;
                                    GrandTotalOrderQty += oItem.TotalQuantity;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalQCPassQty += oItem.QCPassQty;
                                    TotalQCPassQty += oItem.QCPassQty;
                                    GrandTotalQCPassQty += oItem.QCPassQty;

                                    cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalRejectQty += oItem.RejectQty;
                                    TotalRejectQty += oItem.RejectQty;
                                    GrandTotalRejectQty += oItem.RejectQty;

                                    rowIndex++;
                                    sQCNo = oItem.QCNo;
                                }
                                #region subtotal
                                if (sQCNo != "")
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex = rowIndex + 1;
                                    SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                }
                                #endregion

                                #region total
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Party Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 9]; cell.Value = TotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 10]; cell.Value = TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 11]; cell.Value = TotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                rowIndex = rowIndex + 1;
                                #endregion

                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Party_Wise_GU_QC_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                else if (oGUQCRegister.ReportLayout == EnumReportLayout.Order_Wise_Details)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Order Wise GU QC Register(Details)");
                        sheet.Name = "Order Wise GU QC Register(Details)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 10; //qc no
                        sheet.Column(4).Width = 20; //qc by
                        sheet.Column(5).Width = 12; //qc date
                        sheet.Column(6).Width = 25; //Store
                        sheet.Column(7).Width = 20; //buyer
                        sheet.Column(8).Width = 20; //Style
                        sheet.Column(9).Width = 13; //Order qty
                        sheet.Column(10).Width = 13;//shipment date
                        sheet.Column(11).Width = 13; //QC qty
                        sheet.Column(12).Width = 13; //Reject qty
                        sheet.Column(13).Width = 13;

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 13].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 13].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Order Wise QC Register(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 13].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "QC No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "QC By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "QC Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Style"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "Remaining Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oGUQCRegisters.Count > 0)
                        {
                            var data = _oGUQCRegisters.GroupBy(x => new { x.OrderRecapNo }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                            {
                                PO = key.OrderRecapNo,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0; int nRowSpan = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = "PO/Order No : @ " + oData.PO; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;

                                    cell = sheet.Cells[rowIndex, 2]; cell.Merge = true; cell.Value = count; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.QCDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    if (count == 1)
                                    {
                                        nRowSpan = oData.Results.Count() - 1;
                                        if (nRowSpan < 0) { nRowSpan = 0; }
                                        cell = sheet.Cells[rowIndex, 6,rowIndex+nRowSpan,6]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 7, rowIndex + nRowSpan, 7]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 8,rowIndex + nRowSpan,8]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 9,rowIndex + nRowSpan,9]; cell.Merge = true; cell.Value = oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        SubTotalOrderQty += oItem.TotalQuantity;
                                        TotalOrderQty = oItem.TotalQuantity;
                                        GrandTotalOrderQty += oItem.TotalQuantity;

                                        cell = sheet.Cells[rowIndex, 10, rowIndex + nRowSpan, 10]; cell.Merge = true; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    }
                                    cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalQCPassQty += oItem.QCPassQty;
                                    TotalQCPassQty += oItem.QCPassQty;
                                    GrandTotalQCPassQty += oItem.QCPassQty;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalRejectQty += oItem.RejectQty;
                                    TotalRejectQty += oItem.RejectQty;
                                    GrandTotalRejectQty += oItem.RejectQty;
                                    if (count == 1)
                                    {
                                        cell = sheet.Cells[rowIndex, 13, rowIndex + nRowSpan, 13]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    }
                                    rowIndex++;
                                    sQCNo = oItem.QCNo;
                                }
                                
                                #region total
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Order Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                

                                cell = sheet.Cells[rowIndex, 11]; cell.Value = TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 12]; cell.Value = TotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 13]; cell.Value = (TotalOrderQty - TotalQCPassQty).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                                #endregion

                                cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = (GrandTotalOrderQty - GrandTotalQCPassQty).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Order_Wise_GU_QC_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                else if (oGUQCRegister.ReportLayout == EnumReportLayout.Style_Wise_Details)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Style Wise GU QC Register(Details)");
                        sheet.Name = "Style Wise GU QC Register(Details)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 10; //qc no
                        sheet.Column(4).Width = 20; //qc by
                        sheet.Column(5).Width = 12; //qc date
                        sheet.Column(6).Width = 25; //Store
                        sheet.Column(7).Width = 20; //buyer
                        sheet.Column(8).Width = 20; //PO
                        sheet.Column(9).Width = 13; //Order qty
                        sheet.Column(10).Width = 13;//shipment Date
                        sheet.Column(11).Width = 13; //QC qty
                        sheet.Column(12).Width = 13; //Reject qty
                        sheet.Column(13).Width = 16;

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 13].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 13].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Style Wise QC Register(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 13].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;

                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "QC No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "QC By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "QC Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "PO/Recap No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "Remaining Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oGUQCRegisters.Count > 0)
                        {
                            var data = _oGUQCRegisters.GroupBy(x => new { x.StyleNo }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                            {
                                StyleNo = key.StyleNo,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0; int nRowspan = 0; int nOrderId = 0; double remainingQty = 0;
                            int nQcId = 0;int QcRowSpan = 0;
                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = "Style No : @ " + oData.StyleNo; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0; remainingQty = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;

                                    cell = sheet.Cells[rowIndex, 2]; cell.Merge = true; cell.Value = count; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    if (nQcId != oItem.GUQCID)
                                    {
                                        QcRowSpan = oData.Results.Count(a => a.GUQCID == oItem.GUQCID)-1;
                                        if (QcRowSpan < 0) { remainingQty = 0; }
                                        cell = sheet.Cells[rowIndex, 3,rowIndex+QcRowSpan,3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    }
                                    cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.QCDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    if (nOrderId != oItem.OrderRecapID)
                                    {

                                        nRowspan = oData.Results.Count(a => a.OrderRecapID == oItem.OrderRecapID)-1;
                                        if (nRowspan < 0) { nRowspan = 0; }
                                        cell = sheet.Cells[rowIndex, 6,rowIndex+nRowspan,6]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 7,rowIndex+nRowspan,7]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 8,rowIndex+nRowspan,8]; cell.Merge = true; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 9,rowIndex+nRowspan,9]; cell.Merge = true; cell.Value = oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        SubTotalOrderQty += oItem.TotalQuantity;
                                        TotalOrderQty = oItem.TotalQuantity;
                                        GrandTotalOrderQty += oItem.TotalQuantity;

                                        cell = sheet.Cells[rowIndex, 10, rowIndex + nRowspan, 10]; cell.Merge = true; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    }
                                    cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalQCPassQty += oItem.QCPassQty;
                                    TotalQCPassQty += oItem.QCPassQty;
                                    GrandTotalQCPassQty += oItem.QCPassQty;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalRejectQty += oItem.RejectQty;
                                    TotalRejectQty += oItem.RejectQty;
                                    GrandTotalRejectQty += oItem.RejectQty;
                                    if(count==1)
                                    {
                                        nRowspan=oData.Results.Count()-1;
                                        if (nRowspan < 0) { nRowspan = 0; }
                                        cell = sheet.Cells[rowIndex, 13,rowIndex+nRowspan,13]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    }

                                    rowIndex++;
                                    sQCNo = oItem.QCNo;
                                    nOrderId = oItem.OrderRecapID;
                                    nQcId = oItem.GUQCID;
                                }

                                #region total
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Style Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                

                                cell = sheet.Cells[rowIndex, 11]; cell.Value = TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 12]; cell.Value = TotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                remainingQty=TotalOrderQty - TotalQCPassQty;
                                if (remainingQty < 0) { remainingQty = 0; }
                                cell = sheet.Cells[rowIndex, 13]; cell.Value = (remainingQty).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                rowIndex = rowIndex + 1;
                                #endregion

                                cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                               
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = (GrandTotalOrderQty - GrandTotalQCPassQty).ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Style_Wise_GU_QC_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
            }
        }

        #endregion
    }
}