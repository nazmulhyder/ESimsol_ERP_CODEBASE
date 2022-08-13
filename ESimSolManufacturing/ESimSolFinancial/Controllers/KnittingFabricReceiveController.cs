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
    public class KnittingFabricReceiveController : Controller
    {
        #region Declaration

        KnittingFabricReceive _oKnittingFabricReceive = new KnittingFabricReceive();
        List<KnittingFabricReceive> _oKnittingFabricReceives = new List<KnittingFabricReceive>();
        KnittingFabricReceiveDetail _oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
        List<KnittingFabricReceiveDetail> _oKnittingFabricReceiveDetails = new List<KnittingFabricReceiveDetail>();
        FabricReceiveRegister _oFabricReceiveRegister = new FabricReceiveRegister();
        List<FabricReceiveRegister> _oFabricReceiveRegisters = new List<FabricReceiveRegister>();
        string _sDateRange = "";
        #endregion

        #region Actions

        public ActionResult ViewKnittingFabricReceives(int nKntOrderID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.KnittingFabricReceive).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            KnittingOrder oKnittingOrder = new KnittingOrder();
            _oKnittingFabricReceives = new List<KnittingFabricReceive>();
            oKnittingOrder = oKnittingOrder.Get(nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            //oKnittingOrder.KnittingYarns = KnittingYarn.Gets("select * from View_KnittingYarn WHERE KnittingOrderID=" + nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            oKnittingOrder.KnittingOrderDetails = KnittingOrderDetail.Gets(nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            _oKnittingFabricReceives = KnittingFabricReceive.Gets("select * from View_KnittingFabricReceive WHERE KnittingOrderID=" + nKntOrderID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.KnittingOrder = oKnittingOrder;
            ViewBag.Units = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit ORDER BY UnitType ASC", (int)Session[SessionInfo.currentUserID]);
            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(oKnittingOrder.BUID, EnumModuleName.KnittingFabricReceive, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.Stores = oWorkingUnits;
            //ViewBag.Stores = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit ORDER BY WorkingUnitID", (int)Session[SessionInfo.currentUserID]);
            return View(_oKnittingFabricReceives);
        }
        [HttpPost]
        public JsonResult GetKnittingFabricReceive(KnittingFabricReceive oKnittingFabricReceive)
        {
            _oKnittingFabricReceive = new KnittingFabricReceive();
            _oKnittingFabricReceiveDetails = new List<KnittingFabricReceiveDetail>();

            try
            {
                _oKnittingFabricReceive = _oKnittingFabricReceive.Get(oKnittingFabricReceive.KnittingFabricReceiveID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSql = "SELECT * FROM View_KnittingFabricReceiveDetail AS HH WHERE HH.KnittingFabricReceiveID = " + _oKnittingFabricReceive.KnittingFabricReceiveID;
                _oKnittingFabricReceiveDetails = KnittingFabricReceiveDetail.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oKnittingFabricReceive.KnittingFabricReceiveDetails = _oKnittingFabricReceiveDetails;
            }
            catch (Exception ex)
            {
                _oKnittingFabricReceive = new KnittingFabricReceive();
                _oKnittingFabricReceive.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oKnittingFabricReceive);
            var jSonResult = Json(sjson, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpPost]
        public JsonResult GetLotByFabric(Lot oLot)
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

                //sTempSql += "  AND Balance >0";
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
        public JsonResult Save(KnittingFabricReceive oKnittingFabricReceive)
        {
            _oKnittingFabricReceive = new KnittingFabricReceive();
            try
            {
                oKnittingFabricReceive = oKnittingFabricReceive.Save((int)Session[SessionInfo.currentUserID]);
                oKnittingFabricReceive.KnittingFabricReceiveDetails = KnittingFabricReceiveDetail.Gets("SELECT * FROM View_KnittingFabricReceiveDetail WHERE KnittingFabricReceiveID= " + oKnittingFabricReceive.KnittingFabricReceiveID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oKnittingFabricReceive = new KnittingFabricReceive();
                _oKnittingFabricReceive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingFabricReceive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApproveChallan(KnittingFabricReceive oKnittingFabricReceive)
        {
            _oKnittingFabricReceive = new KnittingFabricReceive();
            try
            {
                oKnittingFabricReceive = oKnittingFabricReceive.Approve((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oKnittingFabricReceive = new KnittingFabricReceive();
                _oKnittingFabricReceive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oKnittingFabricReceive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(KnittingFabricReceive oKnittingFabricReceive)
        {
            string sFeedBackMessage = "";
            try
            {

                sFeedBackMessage = oKnittingFabricReceive.Delete(oKnittingFabricReceive.KnittingFabricReceiveID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult Delete(KnittingFabricReceive oJB)
        //{
        //    string sFeedBackMessage = "";
        //    try
        //    {
        //        KnittingFabricReceive oKnittingOrder = new KnittingFabricReceive();
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

        //public JsonResult GetYarnCategory(Product oProduct)
        //{
        //    List<Product> oProducts = new List<Product>();
        //    try
        //    {
        //        if (oProduct.ProductName != null && oProduct.ProductName != "")
        //        {
        //            oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.KnittingFabricReceive, EnumProductUsages.Yarn, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //        else
        //        {
        //            oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.KnittingFabricReceive, EnumProductUsages.Yarn, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        //public JsonResult GetOrderByOrderNo(knitiingor)
        //{
        //    List<Product> oProducts = new List<Product>();
        //    try
        //    {
        //        if (oProduct.ProductName != null && oProduct.ProductName != "")
        //        {
        //            oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.KnittingFabricReceive, EnumProductUsages.Fabric, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //        else
        //        {
        //            oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.KnittingFabricReceive, EnumProductUsages.Fabric, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        public ActionResult KnittingFabricReceivePrintPreview(int id)
        {
            BusinessUnit oBU = new BusinessUnit();
            _oKnittingFabricReceive = new KnittingFabricReceive();
            if (id > 0)
            {
                _oKnittingFabricReceive = _oKnittingFabricReceive.Get(id, (int)Session[SessionInfo.currentUserID]);
                string sSql = "SELECT * FROM View_KnittingFabricReceiveDetail WHERE KnittingFabricReceiveID = " + _oKnittingFabricReceive.KnittingFabricReceiveID;
                _oKnittingFabricReceive.KnittingFabricReceiveDetails = KnittingFabricReceiveDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }

            List<ApprovalHead> oApprovalHead = new List<ApprovalHead>();
            string ssql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.KnittingFabricReceive + " AND BUID = " + _oKnittingFabricReceive.BUID + "  Order By Sequence";
            oApprovalHead = ApprovalHead.Gets(ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<ApprovalHistory> oApprovalHistorys = new List<ApprovalHistory>();
            string Sql = "SELECT * FROM View_ApprovalHistory WHERE ModuleID = " + (int)EnumModuleName.KnittingFabricReceive + " AND BUID = " + _oKnittingFabricReceive.BUID + "  AND  ObjectRefID = " + id + " Order BY ApprovalSequence";
            oApprovalHistorys = ApprovalHistory.Gets(Sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oKnittingFabricReceive.BUID > 0)
            {
                oBU = oBU.Get(_oKnittingFabricReceive.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            byte[] abytes;
            rptKnittingFabricReceive oReport = new rptKnittingFabricReceive();
            abytes = oReport.PrepareReport(_oKnittingFabricReceive, oCompany, oBU, oApprovalHead, oApprovalHistorys);
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

        #region Fabric receive register
        public ActionResult ViewFabricReceiveRegister(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFabricReceiveRegister = new FabricReceiveRegister();
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
            return View(_oFabricReceiveRegister);
        }
        #endregion
        public ActionResult SetSessionSearchCriteria(FabricReceiveRegister oFabricReceiveRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricReceiveRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintFabricReceiveRegister(double ts)
        {
            FabricReceiveRegister oFabricReceiveRegister = new FabricReceiveRegister();
            string _sErrorMesage = "";
            try
            {
                _oFabricReceiveRegisters = new List<FabricReceiveRegister>();
                oFabricReceiveRegister = (FabricReceiveRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oFabricReceiveRegister);
                _oFabricReceiveRegisters = FabricReceiveRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFabricReceiveRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFabricReceiveRegisters = new List<FabricReceiveRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oFabricReceiveRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oFabricReceiveRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptFabricReceiveRegister oReport = new rptFabricReceiveRegister();
                byte[] abytes = oReport.PrepareReport(_oFabricReceiveRegisters, oCompany, oFabricReceiveRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
                
               
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        private string GetSQL(FabricReceiveRegister oFabricReceiveRegister)
        {
            //string _sDateRange = "";
            string sSearchingData = oFabricReceiveRegister.ErrorMessage;
            EnumCompareOperator eFabricReceiveDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dFabricReceiveStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dFabricReceiveEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sApprovalStatus = sSearchingData.Split('~')[3];

            #region make date range
            if (eFabricReceiveDate == EnumCompareOperator.EqualTo)
            {
                _sDateRange = "Receive Date: " + dFabricReceiveStartDate.ToString("dd MMM yyyy");
            }
            else if (eFabricReceiveDate == EnumCompareOperator.Between)
            {
                _sDateRange = "Receive Date: " + dFabricReceiveStartDate.ToString("dd MMM yyyy") + " - To - " + dFabricReceiveEndDate.ToString("dd MMM yyyy");
            }
            else if (eFabricReceiveDate == EnumCompareOperator.NotEqualTo)
            {
                _sDateRange = "Receive Date: Not Equal to " + dFabricReceiveStartDate.ToString("dd MMM yyyy");
            }
            else if (eFabricReceiveDate == EnumCompareOperator.GreaterThan)
            {
                _sDateRange = "Receive Date: Greater Than " + dFabricReceiveStartDate.ToString("dd MMM yyyy");
            }
            else if (eFabricReceiveDate == EnumCompareOperator.SmallerThan)
            {
                _sDateRange = "Receive Date: Smaller Than " + dFabricReceiveStartDate.ToString("dd MMM yyyy");
            }
            else if (eFabricReceiveDate == EnumCompareOperator.NotBetween)
            {
                _sDateRange = "Receive Date: Not Between " + dFabricReceiveStartDate.ToString("dd MMM yyyy") + " - To - " + dFabricReceiveEndDate.ToString("dd MMM yyyy");
            }
            #endregion
            
            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oFabricReceiveRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oFabricReceiveRegister.BUID.ToString();
            }
            #endregion

            #region FabricReceiveNo
            if (!string.IsNullOrEmpty(oFabricReceiveRegister.ReceiveNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ReceiveNo LIKE'%" + oFabricReceiveRegister.ReceiveNo + "%'";
            }
            #endregion

            #region BuyerID
            if (oFabricReceiveRegister.BuyerID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BuyerID =" + oFabricReceiveRegister.BuyerID;
            }
            #endregion

            #region StyleID
            if (oFabricReceiveRegister.StyleID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " StyleID =" + oFabricReceiveRegister.StyleID;
            }
            #endregion

            #region FactoryID
            if (oFabricReceiveRegister.FactoryID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " FactoryID =" + oFabricReceiveRegister.FactoryID;
            }
            #endregion

            #region Receive Date
            if (eFabricReceiveDate != EnumCompareOperator.None)
            {
                if (eFabricReceiveDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFabricReceiveStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eFabricReceiveDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFabricReceiveStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eFabricReceiveDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFabricReceiveStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eFabricReceiveDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFabricReceiveStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eFabricReceiveDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFabricReceiveStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFabricReceiveEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eFabricReceiveDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFabricReceiveStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dFabricReceiveEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            //#region Approval Status
            //if (sApprovalStatus != "Select Approval Status")
            //{
            //    if (sApprovalStatus == "Only Approved Challan")
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " KnittingYarnChallanID IN (SELECT HH.KnittingYarnChallanID FROM KnittingYarnChallan AS HH WHERE ISNULL(HH.ApprovedBy,0) != 0)";
            //    }
            //    else if (sApprovalStatus == "Only Un-Approved Challan")
            //    {
            //        Global.TagSQL(ref sWhereCluse);
            //        sWhereCluse = sWhereCluse + " KnittingYarnChallanID IN (SELECT HH.KnittingYarnChallanID FROM KnittingYarnChallan AS HH WHERE ISNULL(HH.ApprovedBy,0) = 0)";
            //    }
            //}
            //#endregion

            #region Report Layout
            if (oFabricReceiveRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_KnittingFabricReceiveRegister ";
                sOrderBy = " ORDER BY  ReceiveDate, KnittingFabricReceiveID, KnittingFabricReceiveDetailID ASC";
            }
            else if (oFabricReceiveRegister.ReportLayout == EnumReportLayout.Style_Wise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_KnittingFabricReceiveRegister ";
                sOrderBy = " ORDER BY  StyleID, KnittingFabricReceiveID, KnittingFabricReceiveDetailID ASC";
            }
            else if (oFabricReceiveRegister.ReportLayout == EnumReportLayout.Factorywise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_KnittingFabricReceiveRegister ";
                sOrderBy = " ORDER BY  FactoryID, KnittingFabricReceiveID, KnittingFabricReceiveDetailID ASC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }

        public void ExcelFabricReceiveRegister(double ts)
        {
            FabricReceiveRegister oFabricReceiveRegister = new FabricReceiveRegister();
            string _sErrorMesage = "";
            try
            {
                _oFabricReceiveRegisters = new List<FabricReceiveRegister>();
                oFabricReceiveRegister = (FabricReceiveRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oFabricReceiveRegister);
                _oFabricReceiveRegisters = FabricReceiveRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFabricReceiveRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oFabricReceiveRegisters = new List<FabricReceiveRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oFabricReceiveRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oFabricReceiveRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }
            double SubTotalQty = 0, SubtotalProcessLossQty = 0;
        double GrandTotalQty = 0, GrandTotalProcessLossQty = 0;
            if (oFabricReceiveRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using(var excelPackage=new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Status Wise Knitting Order");
                    sheet.Name = "Order Status Wise Fabric receive";
                    sheet.Column(2).Width = 8; //SL
                    sheet.Column(3).Width = 13; //Receive no
                    sheet.Column(4).Width = 14; //Knitting Order
                    sheet.Column(5).Width = 12; //Style

                    sheet.Column(6).Width = 12; //PAM
                    sheet.Column(7).Width = 14; //Finish Dia
                    sheet.Column(8).Width = 12; //buyer
                    sheet.Column(9).Width = 15; //factory
                    sheet.Column(10).Width = 12; //Product
                    sheet.Column(11).Width = 8; //GSM
                    sheet.Column(12).Width = 12; //MicDia
                    sheet.Column(13).Width = 10; //Lot no
                    sheet.Column(14).Width = 15; //Lot Balance
                    sheet.Column(15).Width = 12; //Store
                    sheet.Column(16).Width = 12; //m unit

                    sheet.Column(17).Width = 15; //ApprovedBy
                    sheet.Column(18).Width = 10; //qty
                    sheet.Column(19).Width = 14; //Bag Qty

                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = "Date Wise Fabric receive"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Receive No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "PAM"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Finish Dia"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Factory"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "GSM"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "MIC-DIA"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Lot Balance"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "M Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    cell = sheet.Cells[rowIndex, 17]; cell.Value = "Approved BY"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = "Process Lose Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rowIndex++;
                    if (_oFabricReceiveRegister != null && _oFabricReceiveRegisters.Count > 0)
                        {
                            int nFabricReceiveID = 0, nFabricReceiveDetailsID = 0;
                        int num=0; string sReceiveDate="";
                        SubTotalQty = 0; GrandTotalProcessLossQty = 0; SubtotalProcessLossQty = 0; GrandTotalQty = 0;
                            foreach (FabricReceiveRegister oItem in _oFabricReceiveRegisters)
                            {
                                #region body
                                if (oItem.KnittingFabricReceiveID != nFabricReceiveID)
                                {
                                    #region subtotal
                                    if (SubTotalQty > 0)
                                    {
                                        cell = sheet.Cells[rowIndex, 2, rowIndex,17]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        cell = sheet.Cells[rowIndex, 18]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                        

                                        cell = sheet.Cells[rowIndex, 19]; cell.Value = SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                       

                                        

                                    

                                        rowIndex += 1;
                                       SubTotalQty=0;
                                        SubtotalProcessLossQty=0;
                                    }
                                    #endregion
                                    if (sReceiveDate != oItem.ReceiveDateInString)
                                    {
                                        cell = sheet.Cells[rowIndex, 2, rowIndex , 19]; cell.Merge = true; cell.Value = "Receive Date : @ "+oItem.ReceiveDateInString; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        rowIndex++;
                                    }

                                    int rowCount = (_oFabricReceiveRegisters.Count(x => x.KnittingFabricReceiveID == oItem.KnittingFabricReceiveID)-1);
                                    rowCount = (rowCount == -1) ? 0 : rowCount;
                                    cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = ++num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                  

                                    cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value =  oItem.ReceiveNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    
                                }

                               
                                    

                                    cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.KnittingOrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.PAM.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    

                                    cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.FinishDia; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.FabricName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    

                                    cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.GSM; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.MICDia; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    

                                    cell = sheet.Cells[rowIndex, 13]; cell.Merge = true; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                   

                                    cell = sheet.Cells[rowIndex, 14]; cell.Merge = true; cell.Value = oItem.LotBalance.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    
                                

                                cell = sheet.Cells[rowIndex, 15]; cell.Merge = true; cell.Value = oItem.OperationUnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 16]; cell.Merge = true; cell.Value = oItem.MUnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 17]; cell.Merge = true; cell.Value = oItem.ApprovedByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                               

                                cell = sheet.Cells[rowIndex, 18]; cell.Merge = true; cell.Value = oItem.Qty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                

                                cell = sheet.Cells[rowIndex, 19]; cell.Merge = true; cell.Value = oItem.ProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                




                                SubTotalQty = SubTotalQty + oItem.Qty;
                                GrandTotalQty = GrandTotalQty + oItem.Qty;
                                SubtotalProcessLossQty = SubtotalProcessLossQty + oItem.OrderQty;
                                GrandTotalProcessLossQty = GrandTotalProcessLossQty + oItem.OrderQty;                               
                                sReceiveDate = oItem.ReceiveDateInString;
                                nFabricReceiveID = oItem.KnittingFabricReceiveID;
                                nFabricReceiveDetailsID = oItem.KnittingFabricReceiveDetailID;
                                rowIndex += 1;
                                #endregion
                            }

                            #region subtotal
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 18]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                            cell = sheet.Cells[rowIndex, 19]; cell.Value = SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            

                           

                           

                            rowIndex = rowIndex + 1;
                            #endregion

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = "Grand Total: "; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 18]; cell.Value = GrandTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                            cell = sheet.Cells[rowIndex, 19]; cell.Value = GrandTotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            

                           
                            #endregion
                        

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Date_Wise_Fabric_Receive.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }

                   
                    #endregion
                }
           #endregion
            }

            if (oFabricReceiveRegister.ReportLayout == EnumReportLayout.Style_Wise)
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("Style Wise Fabric Receive");
                    sheet.Name = "Style Wise Fabric receive";
                    sheet.Column(2).Width = 8; //SL
                    sheet.Column(3).Width = 13; //challan no
                    sheet.Column(4).Width = 14; //Knitting Order
                    sheet.Column(5).Width = 12; //Style

                    sheet.Column(6).Width = 12; //PAM
                    sheet.Column(7).Width = 14; //Finish Dia
                    sheet.Column(8).Width = 12; //buyer
                    sheet.Column(9).Width = 15; //factory
                    sheet.Column(10).Width = 12; //Product
                    sheet.Column(11).Width = 8; //GSM
                    sheet.Column(12).Width = 12; //MicDia
                    sheet.Column(13).Width = 10; //Lot no
                    sheet.Column(14).Width = 15; //Lot Balance
                    sheet.Column(15).Width = 12; //Store
                    sheet.Column(16).Width = 12; //m unit

                    sheet.Column(17).Width = 15; //ApprovedBy
                    sheet.Column(18).Width = 10; //qty
                    sheet.Column(19).Width = 14; //Bag Qty

                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = "Style Wise Fabric receive"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Receive No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "PAM"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Finish Dia"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Factory"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "GSM"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "MIC-DIA"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Lot Balance"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "M Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    cell = sheet.Cells[rowIndex, 17]; cell.Value = "Approved BY"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = "Process Lose Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rowIndex++;
                    if (_oFabricReceiveRegister != null && _oFabricReceiveRegisters.Count > 0)
                    {
                        int nFabricReceiveID = 0, nFabricReceiveDetailsID = 0;
                        int num = 0; string nStyleNo = "";
                        SubTotalQty = 0; GrandTotalProcessLossQty = 0; SubtotalProcessLossQty = 0; GrandTotalQty = 0;
                        foreach (FabricReceiveRegister oItem in _oFabricReceiveRegisters)
                        {
                            #region body
                            if (oItem.KnittingFabricReceiveID != nFabricReceiveID)
                            {
                                #region subtotal
                                if (SubTotalQty > 0)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 18]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                                    cell = sheet.Cells[rowIndex, 19]; cell.Value = SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;







                                    rowIndex += 1;
                                    SubTotalQty = 0;
                                    SubtotalProcessLossQty = 0;
                                }
                                #endregion
                                if (nStyleNo != oItem.StyleNo)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true; cell.Value = "Style NO : @ " + oItem.StyleNo; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rowIndex++;
                                }

                                int rowCount = (_oFabricReceiveRegisters.Count(x => x.KnittingFabricReceiveID == oItem.KnittingFabricReceiveID) - 1);
                                rowCount = (rowCount == -1) ? 0 : rowCount;
                                cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = ++num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                                cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.ReceiveNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            }




                            cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.KnittingOrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.PAM.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.FinishDia; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.FabricName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.GSM; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.MICDia; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 13]; cell.Merge = true; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 14]; cell.Merge = true; cell.Value = oItem.LotBalance.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                            cell = sheet.Cells[rowIndex, 15]; cell.Merge = true; cell.Value = oItem.OperationUnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 16]; cell.Merge = true; cell.Value = oItem.MUnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 17]; cell.Merge = true; cell.Value = oItem.ApprovedByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 18]; cell.Merge = true; cell.Value = oItem.Qty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 19]; cell.Merge = true; cell.Value = oItem.ProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;





                            SubTotalQty = SubTotalQty + oItem.Qty;
                            GrandTotalQty = GrandTotalQty + oItem.Qty;
                            SubtotalProcessLossQty = SubtotalProcessLossQty + oItem.OrderQty;
                            GrandTotalProcessLossQty = GrandTotalProcessLossQty + oItem.OrderQty;
                            nStyleNo = oItem.StyleNo;
                            nFabricReceiveID = oItem.KnittingFabricReceiveID;
                            nFabricReceiveDetailsID = oItem.KnittingFabricReceiveDetailID;
                            rowIndex += 1;
                            #endregion
                        }

                        #region subtotal
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        cell = sheet.Cells[rowIndex, 19]; cell.Value = SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;







                        rowIndex = rowIndex + 1;
                        #endregion

                        #region grand total
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = "Grand Total: "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = GrandTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        cell = sheet.Cells[rowIndex, 19]; cell.Value = GrandTotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;




                        #endregion


                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Style_Wise_Fabric_Receive.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }


                    #endregion
                }
                #endregion
            }
            if (oFabricReceiveRegister.ReportLayout == EnumReportLayout.Factorywise)
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("Factory Wise Fabric Receive");
                    sheet.Name = "Factory Wise Fabric receive";
                    sheet.Column(2).Width = 8; //SL
                    sheet.Column(3).Width = 13; //challan no
                    sheet.Column(4).Width = 14; //Knitting Order
                    sheet.Column(5).Width = 12; //Style

                    sheet.Column(6).Width = 12; //PAM
                    sheet.Column(7).Width = 14; //Finish Dia
                    sheet.Column(8).Width = 12; //buyer
                    sheet.Column(9).Width = 15; //factory
                    sheet.Column(10).Width = 12; //Product
                    sheet.Column(11).Width = 8; //GSM
                    sheet.Column(12).Width = 12; //MicDia
                    sheet.Column(13).Width = 10; //Lot no
                    sheet.Column(14).Width = 15; //Lot Balance
                    sheet.Column(15).Width = 12; //Store
                    sheet.Column(16).Width = 12; //m unit

                    sheet.Column(17).Width = 15; //ApprovedBy
                    sheet.Column(18).Width = 10; //qty
                    sheet.Column(19).Width = 14; //Bag Qty

                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = "Factory Wise Fabric receive"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Receive No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Order No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "PAM"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Finish Dia"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Factory"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "GSM"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "MIC-DIA"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Lot Balance"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Store"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "M Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    cell = sheet.Cells[rowIndex, 17]; cell.Value = "Approved BY"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = "Process Lose Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    rowIndex++;
                    if (_oFabricReceiveRegister != null && _oFabricReceiveRegisters.Count > 0)
                    {
                        int nFabricReceiveID = 0, nFabricReceiveDetailsID = 0;
                        int num = 0; string nFactoryName = "";
                        SubTotalQty = 0; GrandTotalProcessLossQty = 0; SubtotalProcessLossQty = 0; GrandTotalQty = 0;
                        foreach (FabricReceiveRegister oItem in _oFabricReceiveRegisters)
                        {
                            #region body
                            if (oItem.KnittingFabricReceiveID != nFabricReceiveID)
                            {
                                #region subtotal
                                if (SubTotalQty > 0)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 18]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                                    cell = sheet.Cells[rowIndex, 19]; cell.Value = SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;







                                    rowIndex += 1;
                                    SubTotalQty = 0;
                                    SubtotalProcessLossQty = 0;
                                }
                                #endregion
                                if (nFactoryName != oItem.FactoryName)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 19]; cell.Merge = true; cell.Value = "Factory Name : @ " + oItem.FactoryName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rowIndex++;
                                }

                                int rowCount = (_oFabricReceiveRegisters.Count(x => x.KnittingFabricReceiveID == oItem.KnittingFabricReceiveID) - 1);
                                rowCount = (rowCount == -1) ? 0 : rowCount;
                                cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = ++num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                                cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.ReceiveNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            }




                            cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.KnittingOrderNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.PAM.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.FinishDia; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.FabricName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.GSM; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.MICDia; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 13]; cell.Merge = true; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 14]; cell.Merge = true; cell.Value = oItem.LotBalance.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                            cell = sheet.Cells[rowIndex, 15]; cell.Merge = true; cell.Value = oItem.OperationUnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 16]; cell.Merge = true; cell.Value = oItem.MUnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 17]; cell.Merge = true; cell.Value = oItem.ApprovedByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 18]; cell.Merge = true; cell.Value = oItem.Qty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 19]; cell.Merge = true; cell.Value = oItem.ProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;





                            SubTotalQty = SubTotalQty + oItem.Qty;
                            GrandTotalQty = GrandTotalQty + oItem.Qty;
                            SubtotalProcessLossQty = SubtotalProcessLossQty + oItem.OrderQty;
                            GrandTotalProcessLossQty = GrandTotalProcessLossQty + oItem.OrderQty;
                            nFactoryName = oItem.FactoryName;
                            nFabricReceiveID = oItem.KnittingFabricReceiveID;
                            nFabricReceiveDetailsID = oItem.KnittingFabricReceiveDetailID;
                            rowIndex += 1;
                            #endregion
                        }

                        #region subtotal
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        cell = sheet.Cells[rowIndex, 19]; cell.Value = SubtotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;







                        rowIndex = rowIndex + 1;
                        #endregion

                        #region grand total
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 17]; cell.Merge = true; cell.Value = "Grand Total: "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 18]; cell.Value = GrandTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                        cell = sheet.Cells[rowIndex, 19]; cell.Value = GrandTotalProcessLossQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;




                        #endregion


                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Factory_Wise_Fabric_Receive.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }


                    #endregion
                }
                #endregion
            }
        }
            
        }
    }
}