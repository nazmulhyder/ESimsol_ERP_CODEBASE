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

namespace ESimSolFinancial.Controllers
{
    public class KnittingYarnChallanController:Controller
    {
        #region Declaration
     
        KnittingYarnChallan _oKnittingYarnChallan = new KnittingYarnChallan();
        List<KnittingYarnChallan> _oKnittingYarnChallans = new List<KnittingYarnChallan>();
        KnittingYarnChallanDetail _oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
        List<KnittingYarnChallanDetail> _oKnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();
        YarnChallanRegister _oYarnChallanRegister = new YarnChallanRegister();
        List<YarnChallanRegister> _oYarnChallanRegisters = new List<YarnChallanRegister>();
        string _sDateRange = "";
        #endregion

        #region Actions

        public ActionResult ViewKnittingYarnChallans(int nKntOrderID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.KnittingYarnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            KnittingOrder oKnittingOrder = new KnittingOrder();
            _oKnittingYarnChallans = new List<KnittingYarnChallan>();
            oKnittingOrder = oKnittingOrder.Get(nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            _oKnittingYarnChallans = KnittingYarnChallan.Gets("select * from View_KnittingYarnChallan WHERE KnittingOrderID=" + nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.KnittingOrder = oKnittingOrder;
            ViewBag.Units = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit ORDER BY UnitType ASC", (int)Session[SessionInfo.currentUserID]);

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(oKnittingOrder.BUID, EnumModuleName.KnittingYarnChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.Stores = oWorkingUnits;
            //ViewBag.Stores = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit ORDER BY WorkingUnitID", (int)Session[SessionInfo.currentUserID]);
            List<KnittingOrderDetail> oKnittingOrderDetails = new List<KnittingOrderDetail>();
            oKnittingOrderDetails = KnittingOrderDetail.Gets(nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.KnittingOrderDetails = oKnittingOrderDetails;
            //ViewBag.KnittingCompostions = KnittingComposition.Gets("SELECT * FROM View_KnittingComposition WHERE KnittingOrderDetailID IN (SELECT KnittingOrderDetailID FROM KnittingOrderDetail WHERE KnittingOrderID = " + nKntOrderID + ")", (int)Session[SessionInfo.currentUserID]);
            return View(_oKnittingYarnChallans);
        }
        [HttpPost]
        public JsonResult GetKnittingYarnChallan(KnittingYarnChallan oKnittingYarnChallan)
        {
            _oKnittingYarnChallan = new KnittingYarnChallan();
            _oKnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();

            try
            {
                _oKnittingYarnChallan = _oKnittingYarnChallan.Get(oKnittingYarnChallan.KnittingYarnChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSql = "SELECT * FROM View_KnittingYarnChallanDetail AS HH WHERE HH.KnittingYarnChallanID = " + _oKnittingYarnChallan.KnittingYarnChallanID;
                _oKnittingYarnChallanDetails = KnittingYarnChallanDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oKnittingYarnChallan.KnittingYarnChallanDetails = _oKnittingYarnChallanDetails;
            }
            catch (Exception ex)
            {
                _oKnittingYarnChallan = new KnittingYarnChallan();
                _oKnittingYarnChallan.ErrorMessage = ex.Message;
              
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingYarnChallan);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpPost]
        public JsonResult GetLotByYarn(Lot oLot)
        {
            string sSQL = "", sTempSql = "";
            List<Lot> oLots = new List<Lot>();
         
            try
            {
                sSQL = "SELECT * FROM View_Lot ";
                if (oLot.WorkingUnitID != 0)
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += " WorkingUnitID =" + oLot.WorkingUnitID;
                }
                if (!string.IsNullOrEmpty(oLot.LotNo))
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += "  LotNo LIKE '%" + oLot.LotNo + "%'";
                }
                if (oLot.ProductID != 0)
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += " ProductID =" + oLot.ProductID;
                }
                if (oLot.BUID != 0)
                {
                    Global.TagSQL(ref sTempSql);
                    sTempSql += " BUID =" + oLot.BUID;
                }
              
                sTempSql += "  AND Balance >0";
                sSQL += sTempSql;
                oLots = Lot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots.Add(oLot);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult Save(KnittingYarnChallan oKnittingYarnChallan)
        {
            _oKnittingYarnChallan = new KnittingYarnChallan();
            try
            {
                oKnittingYarnChallan = oKnittingYarnChallan.Save((int)Session[SessionInfo.currentUserID]);
                oKnittingYarnChallan.KnittingYarnChallanDetails = KnittingYarnChallanDetail.Gets("SELECT * FROM View_KnittingYarnChallanDetail WHERE KnittingYarnChallanID= " + oKnittingYarnChallan.KnittingYarnChallanID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oKnittingYarnChallan = new KnittingYarnChallan();
                _oKnittingYarnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingYarnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApproveChallan(KnittingYarnChallan oKnittingYarnChallan)
        {
            _oKnittingYarnChallan = new KnittingYarnChallan();
            try
            {
                oKnittingYarnChallan = oKnittingYarnChallan.Approve((int)Session[SessionInfo.currentUserID]);
               
            }
            catch (Exception ex)
            {
                _oKnittingYarnChallan = new KnittingYarnChallan();
                _oKnittingYarnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingYarnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(KnittingYarnChallan oKnittingYarnChallan)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oKnittingYarnChallan.Delete(oKnittingYarnChallan.KnittingYarnChallanID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KnittingYarnChallanPrintPreview(int id)
        {
            BusinessUnit oBU = new BusinessUnit();
            _oKnittingYarnChallan = new KnittingYarnChallan();
            if (id > 0)
            {
                _oKnittingYarnChallan = _oKnittingYarnChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                string sSql = "SELECT * FROM View_KnittingYarnChallanDetail WHERE KnittingYarnChallanID = " + _oKnittingYarnChallan.KnittingYarnChallanID;
                _oKnittingYarnChallan.KnittingYarnChallanDetails = KnittingYarnChallanDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }

            //List<ApprovalHead> oApprovalHead = new List<ApprovalHead>();
            //string ssql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.KnittingYarnChallan + " AND BUID = " + _oKnittingYarnChallan.BUID + "  Order By Sequence";
            //oApprovalHead = ApprovalHead.Gets(ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //List<ApprovalHistory> oApprovalHistorys = new List<ApprovalHistory>();
            //string Sql = "SELECT * FROM View_ApprovalHistory WHERE ModuleID = " + (int)EnumModuleName.KnittingYarnChallan + " AND BUID = " + _oKnittingYarnChallan.BUID + "  AND  ObjectRefID = " + id + " Order BY ApprovalSequence";
            //oApprovalHistorys = ApprovalHistory.Gets(Sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.KnittingYarnChallanPreview, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oKnittingYarnChallan.BUID > 0)
            {
                oBU = oBU.Get(_oKnittingYarnChallan.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            byte[] abytes;
            rptKnittingYarnChallan oReport = new rptKnittingYarnChallan();
            abytes = oReport.PrepareReport(_oKnittingYarnChallan, oCompany, oBU, oSignatureSetups);
            return File(abytes, "application/pdf");
            //return null;
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

        //[HttpPost]
        //public JsonResult Delete(KnittingYarnChallan oJB)
        //{
        //    string sFeedBackMessage = "";
        //    try
        //    {
        //        KnittingYarnChallan oKnittingOrder = new KnittingYarnChallan();
        //        sFeedBackMessage = oKnittingOrder.Delete(oJB.KnittingOrderID, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        sFeedBackMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sFeedBackMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}


        #endregion

        #region Functions
        #endregion

        #region Get

        public JsonResult GetKnittingCompostionsByKOID(int KOID)
        {
            List<KnittingComposition> oKnittingCompositions = new List<KnittingComposition>();
            try
            {
                oKnittingCompositions = KnittingComposition.Gets("SELECT * FROM View_KnittingComposition WHERE KnittingOrderDetailID IN (SELECT KnittingOrderDetailID FROM KnittingOrderDetail WHERE KnittingOrderID = " + KOID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oKnittingCompositions = new List<KnittingComposition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingCompositions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetYarnCategory(Product oProduct)
        //{
        //    List<Product> oProducts = new List<Product>();
        //    try
        //    {
        //        if (oProduct.ProductName != null && oProduct.ProductName != "")
        //        {
        //            oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.KnittingYarnChallan, EnumProductUsages.Yarn, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //        else
        //        {
        //            oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.KnittingYarnChallan, EnumProductUsages.Yarn, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oProduct = new Product();
        //        oProduct.ErrorMessage = ex.Message;
        //        oProducts.Add(oProduct);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oProducts);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetFabricCategory(Product oProduct)
        //{
        //    List<Product> oProducts = new List<Product>();
        //    try
        //    {
        //        if (oProduct.ProductName != null && oProduct.ProductName != "")
        //        {
        //            oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.KnittingYarnChallan, EnumProductUsages.Fabric, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //        else
        //        {
        //            oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.KnittingYarnChallan, EnumProductUsages.Fabric, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oProduct = new Product();
        //        oProduct.ErrorMessage = ex.Message;
        //        oProducts.Add(oProduct);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oProducts);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        

        #endregion

        #region print
        
        #endregion

        #region register
        public ActionResult ViewYarnChallanRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oYarnChallanRegister = new YarnChallanRegister();

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.Style_Wise || (EnumReportLayout)oItem.id == EnumReportLayout.Factorywise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            return View(_oYarnChallanRegister);
        }

        public ActionResult SetSessionSearchCriteria(YarnChallanRegister oYarnChallanRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oYarnChallanRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintYarnChallanRegister(double ts)
       {
            YarnChallanRegister oYarnChallanRegister = new YarnChallanRegister();
            string _sErrorMesage = "";
            try
            {
                _oYarnChallanRegisters = new List<YarnChallanRegister>();
                oYarnChallanRegister = (YarnChallanRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oYarnChallanRegister);
                _oYarnChallanRegisters = YarnChallanRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oYarnChallanRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oYarnChallanRegisters = new List<YarnChallanRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oYarnChallanRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oYarnChallanRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptYarnChallanRegisters oReport = new rptYarnChallanRegisters();
                byte[] abytes = oReport.PrepareReport(_oYarnChallanRegisters, oCompany, oYarnChallanRegister.ReportLayout, _sDateRange);
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

        private string GetSQL(YarnChallanRegister oYarnChallanRegister)
        {
            //string _sDateRange = "";
            string sSearchingData = oYarnChallanRegister.ErrorMessage;
            EnumCompareOperator eYarnChallanDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dYarnChallanStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dYarnChallanEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sApprovalStatus = sSearchingData.Split('~')[3];

            #region make date range
            if (eYarnChallanDate == EnumCompareOperator.EqualTo)
            {
                _sDateRange = "Challan Date: " + dYarnChallanStartDate.ToString("dd MMM yyyy");
            }
            else if (eYarnChallanDate == EnumCompareOperator.Between)
            {
                _sDateRange = "Challan Date: " + dYarnChallanStartDate.ToString("dd MMM yyyy") + " - To - " + dYarnChallanEndDate.ToString("dd MMM yyyy");
            }
            else if (eYarnChallanDate == EnumCompareOperator.NotEqualTo)
            {
                _sDateRange = "Challan Date: Not Equal to " + dYarnChallanStartDate.ToString("dd MMM yyyy");
            }
            else if (eYarnChallanDate == EnumCompareOperator.GreaterThan)
            {
                _sDateRange = "Challan Date: Greater Than " + dYarnChallanStartDate.ToString("dd MMM yyyy");
            }
            else if (eYarnChallanDate == EnumCompareOperator.SmallerThan)
            {
                _sDateRange = "Challan Date: Smaller Than " + dYarnChallanStartDate.ToString("dd MMM yyyy");
            }
            else if (eYarnChallanDate == EnumCompareOperator.NotBetween)
            {
                _sDateRange = "Challan Date: Not Between " + dYarnChallanStartDate.ToString("dd MMM yyyy") + " - To - " + dYarnChallanEndDate.ToString("dd MMM yyyy");
            }
            #endregion

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oYarnChallanRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oYarnChallanRegister.BUID.ToString();
            }
            #endregion

            #region YarnChallanNo
            if (!string.IsNullOrEmpty(oYarnChallanRegister.ChallanNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ChallanNo LIKE'%" + oYarnChallanRegister.ChallanNo + "%'";
            }
            #endregion

            #region BuyerID
            if (oYarnChallanRegister.BuyerID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BuyerID =" + oYarnChallanRegister.BuyerID;
            }
            #endregion

            #region BuyerID
            if (oYarnChallanRegister.StyleID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " StyleID =" + oYarnChallanRegister.StyleID;
            }
            #endregion

            #region BuyerID
            if (oYarnChallanRegister.FactoryID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FactoryID =" + oYarnChallanRegister.FactoryID;
            }
            #endregion

            #region Challan Date
            if (eYarnChallanDate != EnumCompareOperator.None)
            {
                if (eYarnChallanDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dYarnChallanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eYarnChallanDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dYarnChallanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eYarnChallanDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dYarnChallanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eYarnChallanDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dYarnChallanStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eYarnChallanDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dYarnChallanStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dYarnChallanEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eYarnChallanDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dYarnChallanStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dYarnChallanEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Approval Status
            if (sApprovalStatus != "Select Approval Status")
            {                
                if (sApprovalStatus == "Only Approved Challan")
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " KnittingYarnChallanID IN (SELECT HH.KnittingYarnChallanID FROM KnittingYarnChallan AS HH WHERE ISNULL(HH.ApprovedBy,0) != 0)";
                }
                else if (sApprovalStatus == "Only Un-Approved Challan")
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " KnittingYarnChallanID IN (SELECT HH.KnittingYarnChallanID FROM KnittingYarnChallan AS HH WHERE ISNULL(HH.ApprovedBy,0) = 0)";
                }
            }
            #endregion

            #region Report Layout
            if (oYarnChallanRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_YarnChallanRegister ";
                sOrderBy = " ORDER BY  ChallanDate, KnittingYarnChallanID, KnittingYarnChallanDetailID ASC";
            }
            else if (oYarnChallanRegister.ReportLayout == EnumReportLayout.Style_Wise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_YarnChallanRegister ";
                sOrderBy = " ORDER BY  StyleID, KnittingYarnChallanID, KnittingYarnChallanDetailID ASC";
            }
            else if (oYarnChallanRegister.ReportLayout == EnumReportLayout.Factorywise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_YarnChallanRegister ";
                sOrderBy = " ORDER BY  FactoryID, KnittingYarnChallanID, KnittingYarnChallanDetailID ASC";
            }
            
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }

        //public void ExportToExcelYarnChallan()
        //{
        //    YarnChallanRegister oYarnChallanRegister = new YarnChallanRegister();
        //    string _sErrorMesage = "";
        //    try
        //    {
        //        _oYarnChallanRegisters = new List<YarnChallanRegister>();
        //        oYarnChallanRegister = (YarnChallanRegister)Session[SessionInfo.ParamObj];
        //        string sSQL = this.GetSQL(oYarnChallanRegister);
        //        _oYarnChallanRegisters = YarnChallanRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //        if (_oYarnChallanRegisters.Count <= 0)
        //        {
        //            _sErrorMesage = "There is no data for print!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _oYarnChallanRegisters = new List<YarnChallanRegister>();
        //        _sErrorMesage = ex.Message;
        //    }

        //    if (_sErrorMesage == "")
        //    {
        //        Company _oCompany = new Company();
        //        _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        _oCompany.CompanyLogo = GetCompanyLogo(_oCompany);
        //        if (oYarnChallanRegister.BUID > 0)
        //        {
        //            BusinessUnit oBU = new BusinessUnit();
        //            oBU = oBU.Get(oYarnChallanRegister.BUID, (int)Session[SessionInfo.currentUserID]);
        //            _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
        //        }

        //        double GrandTotalOrderQty = 0, GrandTotalQCPassQty = 0, GrandTotalRejectQty = 0;
        //        int count = 0, num = 0;
        //        double SubTotalOrderQty = 0, SubTotalQCPassQty = 0, SubTotalRejectQty = 0;
        //        double TotalOrderQty = 0, TotalQCPassQty = 0, TotalRejectQty = 0;
        //        string sQCNo = "";

        //        if (oYarnChallanRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
        //        {
        //            #region full excel
        //            int rowIndex = 2;
        //            ExcelRange cell;
        //            ExcelFill fill;
        //            OfficeOpenXml.Style.Border border;

        //            using (var excelPackage = new ExcelPackage())
        //            {
        //                excelPackage.Workbook.Properties.Author = "ESimSol";
        //                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //                var sheet = excelPackage.Workbook.Worksheets.Add("Date Wise GU QC Register(Details)");
        //                sheet.Name = "Date Wise QC Register(Details)";
        //                sheet.Column(2).Width = 5; //SL
        //                sheet.Column(3).Width = 10; //qc no
        //                sheet.Column(4).Width = 20; //qc by
        //                sheet.Column(5).Width = 25; //Store
        //                sheet.Column(6).Width = 20; //buyer
        //                sheet.Column(7).Width = 20; //Style
        //                sheet.Column(8).Width = 15; //PO
        //                sheet.Column(9).Width = 13; //Order qty
        //                sheet.Column(10).Width = 13; //QC qty
        //                sheet.Column(11).Width = 13; //Reject qty

        //                #region Report Header
        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 1;

        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Wise QC Register(Details)"; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 1;

        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 2;
        //                #endregion

        //                #region Column Header

        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 3]; cell.Value = "QC No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 4]; cell.Value = "QC By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 5]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 6]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 8]; cell.Value = "PO/Recap No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 10]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 11]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                rowIndex = rowIndex + 1;
        //                #endregion

        //                #region group by
        //                if (_oYarnChallanRegisters.Count > 0)
        //                {
        //                    var data = _oYarnChallanRegisters.GroupBy(x => new { x.QCDateInString }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
        //                    {
        //                        QCDate = key.QCDateInString,
        //                        Results = grp.ToList() //All data
        //                    });
        //                #endregion

        //                    #region Report Data
        //                    GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0;

        //                    foreach (var oData in data)
        //                    {
        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "QC Date : @ " + oData.QCDate; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        rowIndex = rowIndex + 1;

        //                        count = 0; num = 0;
        //                        SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                        TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0;

        //                        foreach (var oItem in oData.Results)
        //                        {
        //                            count++;
        //                            #region subtotal
        //                            if (sQCNo != "")
        //                            {
        //                                if (sQCNo != oItem.QCNo && count > 1)
        //                                {
        //                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                    cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                    cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                    rowIndex = rowIndex + 1;
        //                                    SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                                }
        //                            }
        //                            #endregion

        //                            if (sQCNo != oItem.QCNo)
        //                            {
        //                                num++;
        //                                int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo) - 1);
        //                                cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            }
        //                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalOrderQty += oItem.TotalQuantity;
        //                            TotalOrderQty += oItem.TotalQuantity;
        //                            GrandTotalOrderQty += oItem.TotalQuantity;

        //                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalQCPassQty += oItem.QCPassQty;
        //                            TotalQCPassQty += oItem.QCPassQty;
        //                            GrandTotalQCPassQty += oItem.QCPassQty;

        //                            cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalRejectQty += oItem.RejectQty;
        //                            TotalRejectQty += oItem.RejectQty;
        //                            GrandTotalRejectQty += oItem.RejectQty;

        //                            rowIndex++;
        //                            sQCNo = oItem.QCNo;
        //                        }
        //                        #region subtotal
        //                        if (sQCNo != "")
        //                        {
        //                            cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            rowIndex = rowIndex + 1;
        //                            SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                        }
        //                        #endregion

        //                        #region total
        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Date Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 9]; cell.Value = TotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 10]; cell.Value = TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 11]; cell.Value = TotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        rowIndex = rowIndex + 1;
        //                        #endregion

        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        rowIndex = rowIndex + 1;
        //                    }

        //                    #region grand total
        //                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 10]; cell.Value = GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    rowIndex = rowIndex + 1;
        //                    #endregion

        //                    #endregion

        //                    Response.ClearContent();
        //                    Response.BinaryWrite(excelPackage.GetAsByteArray());
        //                    Response.AddHeader("content-disposition", "attachment; filename=Date_Wise_GU_QC_Register.xlsx");
        //                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                    Response.Flush();
        //                    Response.End();
        //                }

        //            }
        //            #endregion
        //        }
        //        else if (oYarnChallanRegister.ReportLayout == EnumReportLayout.PartyWiseDetails)
        //        {
        //            #region full excel
        //            int rowIndex = 2;
        //            ExcelRange cell;
        //            ExcelFill fill;
        //            OfficeOpenXml.Style.Border border;

        //            using (var excelPackage = new ExcelPackage())
        //            {
        //                excelPackage.Workbook.Properties.Author = "ESimSol";
        //                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //                var sheet = excelPackage.Workbook.Worksheets.Add("Party Wise GU QC Register(Details)");
        //                sheet.Name = "Party Wise GU QC Register(Details)";
        //                sheet.Column(2).Width = 5; //SL
        //                sheet.Column(3).Width = 10; //qc no
        //                sheet.Column(4).Width = 20; //qc by
        //                sheet.Column(5).Width = 25; //Store
        //                sheet.Column(6).Width = 12; //qc date
        //                sheet.Column(7).Width = 20; //Style
        //                sheet.Column(8).Width = 15; //PO
        //                sheet.Column(9).Width = 13; //Order qty
        //                sheet.Column(10).Width = 13; //QC qty
        //                sheet.Column(11).Width = 13; //Reject qty

        //                #region Report Header
        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 1;

        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Party Wise QC Register(Details)"; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 1;

        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 2;
        //                #endregion

        //                #region Column Header

        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 3]; cell.Value = "QC No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 4]; cell.Value = "QC By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 5]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 6]; cell.Value = "QC Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 8]; cell.Value = "PO/Recap No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 10]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 11]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                rowIndex = rowIndex + 1;
        //                #endregion

        //                #region group by
        //                if (_oYarnChallanRegisters.Count > 0)
        //                {
        //                    var data = _oYarnChallanRegisters.GroupBy(x => new { x.BuyerName }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
        //                    {
        //                        buyerName = key.BuyerName,
        //                        Results = grp.ToList() //All data
        //                    });
        //                #endregion

        //                    #region Report Data
        //                    GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0;

        //                    foreach (var oData in data)
        //                    {
        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Party : @ " + oData.buyerName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        rowIndex = rowIndex + 1;

        //                        count = 0; num = 0;
        //                        SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                        TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0;

        //                        foreach (var oItem in oData.Results)
        //                        {
        //                            count++;
        //                            #region subtotal
        //                            if (sQCNo != "")
        //                            {
        //                                if (sQCNo != oItem.QCNo && count > 1)
        //                                {
        //                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                    cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                    cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                    rowIndex = rowIndex + 1;
        //                                    SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                                }
        //                            }
        //                            #endregion

        //                            if (sQCNo != oItem.QCNo)
        //                            {
        //                                num++;
        //                                int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo) - 1);
        //                                cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                                cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.QCDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            }
        //                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalOrderQty += oItem.TotalQuantity;
        //                            TotalOrderQty += oItem.TotalQuantity;
        //                            GrandTotalOrderQty += oItem.TotalQuantity;

        //                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalQCPassQty += oItem.QCPassQty;
        //                            TotalQCPassQty += oItem.QCPassQty;
        //                            GrandTotalQCPassQty += oItem.QCPassQty;

        //                            cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalRejectQty += oItem.RejectQty;
        //                            TotalRejectQty += oItem.RejectQty;
        //                            GrandTotalRejectQty += oItem.RejectQty;

        //                            rowIndex++;
        //                            sQCNo = oItem.QCNo;
        //                        }
        //                        #region subtotal
        //                        if (sQCNo != "")
        //                        {
        //                            cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            rowIndex = rowIndex + 1;
        //                            SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                        }
        //                        #endregion

        //                        #region total
        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Party Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 9]; cell.Value = TotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 10]; cell.Value = TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 11]; cell.Value = TotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        rowIndex = rowIndex + 1;
        //                        #endregion

        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        rowIndex = rowIndex + 1;
        //                    }

        //                    #region grand total
        //                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 10]; cell.Value = GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    rowIndex = rowIndex + 1;
        //                    #endregion

        //                    #endregion

        //                    Response.ClearContent();
        //                    Response.BinaryWrite(excelPackage.GetAsByteArray());
        //                    Response.AddHeader("content-disposition", "attachment; filename=Party_Wise_GU_QC_Register.xlsx");
        //                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                    Response.Flush();
        //                    Response.End();
        //                }

        //            }
        //            #endregion
        //        }
        //        else if (oYarnChallanRegister.ReportLayout == EnumReportLayout.Order_Wise_Details)
        //        {
        //            #region full excel
        //            int rowIndex = 2;
        //            ExcelRange cell;
        //            ExcelFill fill;
        //            OfficeOpenXml.Style.Border border;

        //            using (var excelPackage = new ExcelPackage())
        //            {
        //                excelPackage.Workbook.Properties.Author = "ESimSol";
        //                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //                var sheet = excelPackage.Workbook.Worksheets.Add("Order Wise GU QC Register(Details)");
        //                sheet.Name = "Order Wise GU QC Register(Details)";
        //                sheet.Column(2).Width = 5; //SL
        //                sheet.Column(3).Width = 10; //qc no
        //                sheet.Column(4).Width = 20; //qc by
        //                sheet.Column(5).Width = 12; //qc date
        //                sheet.Column(6).Width = 25; //Store
        //                sheet.Column(7).Width = 20; //buyer
        //                sheet.Column(8).Width = 20; //Style
        //                sheet.Column(9).Width = 13; //Order qty
        //                sheet.Column(10).Width = 13; //QC qty
        //                sheet.Column(11).Width = 13; //Reject qty

        //                #region Report Header
        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 1;

        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Order Wise QC Register(Details)"; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 1;

        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 2;
        //                #endregion

        //                #region Column Header

        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 3]; cell.Value = "QC No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 4]; cell.Value = "QC By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 5]; cell.Value = "QC Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 6]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 8]; cell.Value = "Style"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 10]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 11]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                rowIndex = rowIndex + 1;
        //                #endregion

        //                #region group by
        //                if (_oYarnChallanRegisters.Count > 0)
        //                {
        //                    var data = _oYarnChallanRegisters.GroupBy(x => new { x.OrderRecapNo }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
        //                    {
        //                        PO = key.OrderRecapNo,
        //                        Results = grp.ToList() //All data
        //                    });
        //                #endregion

        //                    #region Report Data
        //                    GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0;

        //                    foreach (var oData in data)
        //                    {
        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "PO/Order No : @ " + oData.PO; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        rowIndex = rowIndex + 1;

        //                        count = 0; num = 0;
        //                        SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                        TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0;

        //                        foreach (var oItem in oData.Results)
        //                        {
        //                            count++;

        //                            cell = sheet.Cells[rowIndex, 2]; cell.Merge = true; cell.Value = count; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.QCDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalOrderQty += oItem.TotalQuantity;
        //                            TotalOrderQty += oItem.TotalQuantity;
        //                            GrandTotalOrderQty += oItem.TotalQuantity;

        //                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalQCPassQty += oItem.QCPassQty;
        //                            TotalQCPassQty += oItem.QCPassQty;
        //                            GrandTotalQCPassQty += oItem.QCPassQty;

        //                            cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalRejectQty += oItem.RejectQty;
        //                            TotalRejectQty += oItem.RejectQty;
        //                            GrandTotalRejectQty += oItem.RejectQty;

        //                            rowIndex++;
        //                            sQCNo = oItem.QCNo;
        //                        }

        //                        #region total
        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Order Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 9]; cell.Value = TotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 10]; cell.Value = TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 11]; cell.Value = TotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        rowIndex = rowIndex + 1;
        //                        #endregion

        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        rowIndex = rowIndex + 1;
        //                    }

        //                    #region grand total
        //                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 10]; cell.Value = GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    rowIndex = rowIndex + 1;
        //                    #endregion

        //                    #endregion

        //                    Response.ClearContent();
        //                    Response.BinaryWrite(excelPackage.GetAsByteArray());
        //                    Response.AddHeader("content-disposition", "attachment; filename=Order_Wise_GU_QC_Register.xlsx");
        //                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                    Response.Flush();
        //                    Response.End();
        //                }

        //            }
        //            #endregion
        //        }
        //        else if (oYarnChallanRegister.ReportLayout == EnumReportLayout.Style_Wise_Details)
        //        {
        //            #region full excel
        //            int rowIndex = 2;
        //            ExcelRange cell;
        //            ExcelFill fill;
        //            OfficeOpenXml.Style.Border border;

        //            using (var excelPackage = new ExcelPackage())
        //            {
        //                excelPackage.Workbook.Properties.Author = "ESimSol";
        //                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //                var sheet = excelPackage.Workbook.Worksheets.Add("Style Wise GU QC Register(Details)");
        //                sheet.Name = "Style Wise GU QC Register(Details)";
        //                sheet.Column(2).Width = 5; //SL
        //                sheet.Column(3).Width = 10; //qc no
        //                sheet.Column(4).Width = 20; //qc by
        //                sheet.Column(5).Width = 12; //qc date
        //                sheet.Column(6).Width = 25; //Store
        //                sheet.Column(7).Width = 20; //buyer
        //                sheet.Column(8).Width = 20; //PO
        //                sheet.Column(9).Width = 13; //Order qty
        //                sheet.Column(10).Width = 13; //QC qty
        //                sheet.Column(11).Width = 13; //Reject qty

        //                #region Report Header
        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 1;

        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Style Wise QC Register(Details)"; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 1;

        //                sheet.Cells[rowIndex, 2, rowIndex, 11].Merge = true;
        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
        //                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                rowIndex = rowIndex + 2;

        //                #endregion

        //                #region Column Header

        //                cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 3]; cell.Value = "QC No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 4]; cell.Value = "QC By"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 5]; cell.Value = "QC Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 6]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 7]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 8]; cell.Value = "PO/Recap No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 9]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 10]; cell.Value = "QC Pass Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                cell = sheet.Cells[rowIndex, 11]; cell.Value = "Reject Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                rowIndex = rowIndex + 1;
        //                #endregion

        //                #region group by
        //                if (_oYarnChallanRegisters.Count > 0)
        //                {
        //                    var data = _oYarnChallanRegisters.GroupBy(x => new { x.StyleNo }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
        //                    {
        //                        StyleNo = key.StyleNo,
        //                        Results = grp.ToList() //All data
        //                    });
        //                #endregion

        //                    #region Report Data
        //                    GrandTotalOrderQty = 0; GrandTotalQCPassQty = 0; GrandTotalRejectQty = 0;

        //                    foreach (var oData in data)
        //                    {
        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Style No : @ " + oData.StyleNo; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        rowIndex = rowIndex + 1;

        //                        count = 0; num = 0;
        //                        SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
        //                        TotalOrderQty = 0; TotalQCPassQty = 0; TotalRejectQty = 0;

        //                        foreach (var oItem in oData.Results)
        //                        {
        //                            count++;

        //                            cell = sheet.Cells[rowIndex, 2]; cell.Merge = true; cell.Value = count; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.QCDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.TotalQuantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalOrderQty += oItem.TotalQuantity;
        //                            TotalOrderQty += oItem.TotalQuantity;
        //                            GrandTotalOrderQty += oItem.TotalQuantity;

        //                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.QCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalQCPassQty += oItem.QCPassQty;
        //                            TotalQCPassQty += oItem.QCPassQty;
        //                            GrandTotalQCPassQty += oItem.QCPassQty;

        //                            cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.RejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
        //                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                            SubTotalRejectQty += oItem.RejectQty;
        //                            TotalRejectQty += oItem.RejectQty;
        //                            GrandTotalRejectQty += oItem.RejectQty;

        //                            rowIndex++;
        //                            sQCNo = oItem.QCNo;
        //                        }

        //                        #region total
        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Style Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 9]; cell.Value = TotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 10]; cell.Value = TotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        cell = sheet.Cells[rowIndex, 11]; cell.Value = TotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                        rowIndex = rowIndex + 1;
        //                        #endregion

        //                        cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //                        rowIndex = rowIndex + 1;
        //                    }

        //                    #region grand total
        //                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 10]; cell.Value = GrandTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    cell = sheet.Cells[rowIndex, 11]; cell.Value = GrandTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
        //                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //                    rowIndex = rowIndex + 1;
        //                    #endregion

        //                    #endregion

        //                    Response.ClearContent();
        //                    Response.BinaryWrite(excelPackage.GetAsByteArray());
        //                    Response.AddHeader("content-disposition", "attachment; filename=Style_Wise_GU_QC_Register.xlsx");
        //                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                    Response.Flush();
        //                    Response.End();
        //                }

        //            }
        //            #endregion
        //        }
        //    }
        //}

        #endregion
    }
}