using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Linq;
using System.Drawing.Imaging;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class GRNController : Controller
    {
        #region Declaration
        GRN _oGRN = new GRN();
        List<GRN> _oGRNs = new List<GRN>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        Company _oCompany = new Company();
        #endregion
        #region Actions
        public ActionResult ViewGRNs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID])); 
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            _oGRNs = new List<GRN>();
            string sSQL = "SELECT * FROM View_GRN AS HH WHERE ISNULL(HH.ApproveBy,0)=0 ";
            if (buid>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND BUID = " + buid ;
            }
            _oGRNs = GRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oGRNs = _oGRNs.OrderByDescending(x => x.GRNDate).ToList();
            #region Get User
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ReceivedBy FROM GRN AS MM WHERE MM.BUID =" + buid.ToString() + " AND ISNULL(MM.ReceivedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oReceivedUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oReceivedUser = new ESimSol.BusinessObjects.User();
            oReceivedUser.UserID = 0; oReceivedUser.UserName = "--Select Received User--";
            oReceivedUsers.Add(oReceivedUser);
            oReceivedUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.GRN, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.Stores = oWorkingUnits;
            ViewBag.ReceivedUsers = oReceivedUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            ViewBag.GRNTypes = EnumObject.jGets(typeof(EnumGRNType));
            ViewBag.GRNStatusList = EnumObject.jGets(typeof(EnumGRNStatus));
            return View(_oGRNs);
        }
        public ActionResult ViewGRN(int id, int buid)
        {
            _oGRN = new GRN();
            Company oCompany = new Company();

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.GRN, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            if (id > 0)
            {
                _oGRN = _oGRN.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oGRN.GRNDetails = GRNDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                if (!this.ContainStore(_oGRN.StoreID, oWorkingUnits))
                {
                    oWorkingUnit = new WorkingUnit();
                    oWorkingUnit = oWorkingUnit.Get(_oGRN.StoreID, (int)Session[SessionInfo.currentUserID]);
                    oWorkingUnits.Add(oWorkingUnit);
                }
            }
            
            ViewBag.Stores = oWorkingUnits;
            ViewBag.GRNTypes = EnumObject.jGets(typeof(EnumGRNType));
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oGRN);
        }

        #region Product Report
        public ActionResult ViewGRNProductReports(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            
            ViewBag.BUID = buid;

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.GRN, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.WorkingUnits = oWorkingUnits;
            //ViewBag.ProductCategorys = ProductCategory.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(oGRNDetails);
        }
        [HttpPost]
        public JsonResult GetsProductReport(int nBUID, int nWorkingUnitID, int nDateYear, int nLayout)
        {
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            try
            {
                oGRNDetails = GRNDetail.GetsRpt_Product(nBUID, nWorkingUnitID, nDateYear, nLayout, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oGRNDetails = new List<GRNDetail>();
                GRNDetail oGRNDetail = new GRNDetail();
                oGRNDetail.ErrorMessage = ex.Message;
                oGRNDetails.Add(oGRNDetail); ;
            }
            var jsonResult = Json(oGRNDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Print Product Report
        public ActionResult PrintGRNDetailProduct(String sTemp)
        {
            List<GRNDetail> oGRNDetailProductList = new List<GRNDetail>();

            int nBUID = 0;
            int nWorkingUnitID = 0;
            int nDateYear = 0;
            int nLayout = 0;

            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                nBUID = Convert.ToInt32(sTemp.Split('~')[0]);
                nWorkingUnitID = Convert.ToInt32(sTemp.Split('~')[1]);
                nDateYear = Convert.ToInt32(sTemp.Split('~')[2]);
                nLayout = Convert.ToInt32(sTemp.Split('~')[3]);
                oGRNDetailProductList = GRNDetail.GetsRpt_Product(nBUID, nWorkingUnitID, nDateYear, nLayout, (int)Session[SessionInfo.currentUserID]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sHeader = "Year Wise LC Receive Report";
            if (nLayout == 1)
                sHeader = "Month Wise LC Receive Report";

            rptGRNDetailProduct oReport = new rptGRNDetailProduct();
            byte[] abytes = oReport.PrepareReport(oGRNDetailProductList, new int[] {nDateYear, nLayout}, oCompany, oBusinessUnit, sHeader);
            return File(abytes, "application/pdf");
        }
        #endregion

        private bool ContainStore(int nStoreID, List<WorkingUnit> oWorkingUnits)
        {
            foreach (WorkingUnit oItem in oWorkingUnits)
            {
                if (oItem.WorkingUnitID == nStoreID)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public JsonResult Save(GRN oGRN)
        {
            _oGRN = new GRN();
            oGRN.Remarks = oGRN.Remarks == null ? "" : oGRN.Remarks;
            try
            {
                _oGRN = oGRN;
                _oGRN = _oGRN.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approve(GRN oGRN)
        {
            _oGRN = new GRN();
            oGRN.Remarks = oGRN.Remarks == null ? "" : oGRN.Remarks;
            oGRN.ApproveBy = (int)Session[SessionInfo.currentUserID];
            oGRN.ApproveDate = DateTime.Now;
            oGRN.GRNStatusInt = (int)EnumGRNStatus.Approved;
            try
            {
                _oGRN = oGRN;
                _oGRN = _oGRN.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UndoApprove(GRN oGRN)
        {
            _oGRN = new GRN();
            try
            {
                _oGRN = oGRN;
                _oGRN = _oGRN.UndoApprove((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMRIRInfo(GRN oGRN)
        {
            _oGRN = new GRN();
            
            try
            {
                _oGRN = oGRN;
                _oGRN = _oGRN.UpdateMRIRInfo((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Receive(GRN oGRN)
        {
            _oGRN = new GRN();
            try
            {
                _oGRN = oGRN;
                _oGRN = _oGRN.Receive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(GRN oGRN)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oGRN.Delete(oGRN.GRNID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsPurchaseInvocie(GRN oGRN)
        {
            List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseInvoice AS TT WHERE  ISNULL(TT.ApprovedBy,0)!=0 AND TT.BUID= " + oGRN.BUID.ToString() + " AND TT.YetToGRNQty>0  AND TT.ContractorID=" + oGRN.ContractorID.ToString() + " OR TT.PurchaseInvoiceID=" + oGRN.RefObjectID.ToString();
                oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseInvoices = new List<PurchaseInvoice>();
                PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = ex.Message;
                oPurchaseInvoices.Add(oPurchaseInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsPurchaseOrders(GRN oGRN)
        {
            List<PurchaseOrder> oPurchaseOrders = new List<PurchaseOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseOrder AS TT WHERE  ISNULL(TT.ApproveBy,0)!=0 AND TT.BUID= " + oGRN.BUID.ToString() + " AND TT.YetToGRNQty>0  AND TT.ContractorID=" + oGRN.ContractorID.ToString();
                oPurchaseOrders = PurchaseOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseOrders = new List<PurchaseOrder>();
                PurchaseOrder oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.ErrorMessage = ex.Message;
                oPurchaseOrders.Add(oPurchaseOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsImportInvocie(GRN oGRN)
        {
            String sImportPIType = "";

            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            ImportInvoice oImportInvoice = new ImportInvoice();
            try
            {
                string sSQL = "SELECT * FROM View_ImportInvoice";
                string sReturn = " WHERE BUID=" + oGRN.BUID.ToString();
                if (!String.IsNullOrEmpty(oGRN.RefObjectNo))
                {
                    oGRN.RefObjectNo = oGRN.RefObjectNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ImportInvoiceNo Like'%" + oGRN.RefObjectNo + "%'";
                }
                if (oGRN.ContractorID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID  in (" + oGRN.ContractorID + " )";


                    #region Deafult Yet To Received Import Invoice
                    sReturn = sReturn + " AND ImportInvoiceID IN (SELECT HH.ImportInvoiceID FROM View_ImportInvoiceDetail AS HH WHERE HH.YetToGRNQty>0 AND HH.ImportInvoiceID IN (SELECT ImpInv.ImportInvoiceID FROM  ImportInvoice AS ImpInv WHERE ImpInv.ContractorID = " + oGRN.ContractorID + "))";
                    #endregion
                }
                if (oGRN.GRNTypeInt > 0)
                {
                    if(oGRN.GRNTypeInt==(int)EnumGRNType.LocalInvoice)
                    {
                        sImportPIType = ((int)EnumImportPIType.NonLC).ToString();
                    }
                    else if (oGRN.GRNTypeInt == (int)EnumGRNType.ImportInvoice)
                    {
                        sImportPIType = ((int)EnumImportPIType.Foreign).ToString() + ","+((int)EnumImportPIType.Local).ToString() + "," + ((int)EnumImportPIType.TTForeign).ToString();
                    }
                   
                    else if (oGRN.GRNTypeInt == (int)EnumGRNType.FancyYarn)
                    {
                        sImportPIType = ((int)EnumImportPIType.FancyYarn).ToString();

                    }
                    else
                    {
                        sImportPIType = "";
                    }
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "InvoiceType in (" + sImportPIType+")";
                }

                sSQL = sSQL + "" + sReturn;
                oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (ImportInvoice oItem in oImportInvoices)
                {
                    _oGRN = new  GRN();
                    _oGRN.RefObjectNo = oItem.ImportInvoiceNo;                    
                    _oGRN.RefObjectID = oItem.ImportInvoiceID;
                    _oGRN.RefObjectDate = oItem.DateofInvoice;
                    _oGRN.ContractorID = oItem.ContractorID;
                    _oGRN.ContractorName = oItem.ContractorName;
                    _oGRN.CurrencyID = oItem.CurrencyID;
                    _oGRN.ImportLCNo = oItem.ImportLCNo;
                    if (oItem.CCRate<=0)
                    {
                        oItem.CCRate = 1;
                    }
                    _oGRN.ConversionRate = oItem.CRate_Acceptance;
                    //if (oItem.InvoiceType == EnumImportPIType.Foreign)
                    //{
                    //    if (oItem.InvoiceStatus == EnumInvoiceEvent.Goods_In_Transit ||  oItem.InvoiceStatus == EnumInvoiceEvent.DO_Received_From_Shippingline || oItem.InvoiceStatus == EnumInvoiceEvent.Stock_In_Partial || oItem.InvoiceStatus == EnumInvoiceEvent.Stock_In)
                    //    {
                    //        _oGRNs.Add(_oGRN);
                    //    }
                    //}
                    //else
                    //{
                        _oGRNs.Add(_oGRN);
                    //}
                }
            }
            catch (Exception ex)
            {
                _oGRNs = new List<GRN>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsWorkOrder(GRN oGRN)
        {
            List<WorkOrder> oWorkOrders = new List<WorkOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_WorkOrder AS TT WHERE ISNULL(TT.ApproveBy,0)!=0 AND  TT.BUID = " + oGRN.BUID.ToString() + " AND  TT.YetToGRNQty>0  AND TT.SupplierID=" + oGRN.ContractorID.ToString() + " OR TT.WorkOrderID=" + oGRN.RefObjectID.ToString();
                oWorkOrders = WorkOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWorkOrders = new List<WorkOrder>();
                WorkOrder oWorkOrder = new WorkOrder();
                oWorkOrder.ErrorMessage = ex.Message;
                oWorkOrders.Add(oWorkOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWorkOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsImportPIs(GRN oGRN)
        {
            List<ImportPI> oImportPIs = new List<ImportPI>();
            try
            {
                string sSQL = "SELECT * FROM View_ImportPI AS TT WHERE ISNULL(TT.ApprovedBy,0)!=0 AND  TT.BUID = " + oGRN.BUID.ToString() + " AND  TT.YetToGRNQty>0  AND TT.SupplierID=" + oGRN.ContractorID.ToString() + " OR TT.ImportPIID =" + oGRN.RefObjectID.ToString();
                oImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportPIs = new List<ImportPI>();
                ImportPI oImportPI = new ImportPI();
                oImportPI.ErrorMessage = ex.Message;
                oImportPIs.Add(oImportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsConsumptions(GRN oGRN)
        {
            List<ConsumptionRequisition> oConsumptionRequisitions = new List<ConsumptionRequisition>();
            try
            {
                if (oGRN.RefObjectNo == null) { oGRN.RefObjectNo = ""; }
                string sSQL = "SELECT * FROM View_ConsumptionRequisition AS TT WHERE TT.CRType=2 AND ISNULL(TT.ApprovedBy,0)!=0 AND TT.RequisitionForName LIKE '%" + oGRN.RefObjectNo + "%' AND TT.BUID = " + oGRN.BUID.ToString() + " AND ISNULL(TT.DeliveryBy,0) !=0 OR TT.ConsumptionRequisitionID =" + oGRN.RefObjectID.ToString();
                oConsumptionRequisitions = ConsumptionRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oConsumptionRequisitions = new List<ConsumptionRequisition>();
                ConsumptionRequisition oConsumptionRequisition = new ConsumptionRequisition();
                oConsumptionRequisition.ErrorMessage = ex.Message;
                oConsumptionRequisitions.Add(oConsumptionRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oConsumptionRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }                
        [HttpPost]
        public JsonResult GetsRefItems(GRN oGRN)
        {
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();            
            try
            {
                if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.LocalInvoice)
                {
                    oGRNDetails = MapGRNDetailFromInvoice(oGRN.RefObjectID);
                }else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.PurchaseOrder)
                {
                    oGRNDetails = MapGRNDetailFromPO(oGRN.RefObjectID);
                }
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.ImportInvoice)
                {
                    oGRNDetails = MapGRNDetailFromImportInvoice(oGRN.RefObjectID);
                }
                //else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.Service)
                //{
                //    oGRNDetails = MapGRNDetailFromImportInvoice(oGRN.RefObjectID);
                //}
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.FancyYarn)
                {
                    oGRNDetails = MapGRNDetailFromImportInvoice(oGRN.RefObjectID);
                }
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.WorkOrder)
                {
                    oGRNDetails = MapGRNDetailFromWorkOrder(oGRN.RefObjectID);
                }
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.ImportPI)
                {
                    oGRNDetails = MapGRNDetailFromImportPI(oGRN.RefObjectID);
                }
                else if ((EnumGRNType)oGRN.GRNTypeInt == EnumGRNType.FloorReturn)
                {
                    oGRNDetails = MapGRNDetailFromConsumtion(oGRN.RefObjectID);
                }
            }
            catch (Exception ex)
            {
                oGRNDetails = new List<GRNDetail>();
                GRNDetail oGRNDetail = new GRNDetail();
                oGRNDetail.ErrorMessage = ex.Message;
                oGRNDetails.Add(oGRNDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGRNDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private List<GRNDetail> MapGRNDetailFromInvoice(int nInvoiceID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            _oCompany = new Company();
            PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();
            _oCompany = _oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oPurchaseInvoice = oPurchaseInvoice.Get(nInvoiceID, (int)Session[SessionInfo.currentUserID]);
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();
            string sSQL = "SELECT * FROM View_PurchaseInvoiceDetail AS TT WHERE TT.PurchaseInvoiceID=" + nInvoiceID.ToString() + " AND TT.YetToGRNQty>0";
            oPurchaseInvoiceDetails = PurchaseInvoiceDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (PurchaseInvoiceDetail oItem in oPurchaseInvoiceDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = oItem.ProductSpec;
                oGRNDetail.MUnitID = oItem.MUnitID;
                //oGRNDetail.RefQty = oItem.YetToGRNQty;   
                oGRNDetail.RefQty = oItem.Qty; 
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                if (_oCompany.BaseCurrencyID != oPurchaseInvoice.CurrencyID)
                {
                    oGRNDetail.UnitPrice = ((oItem.UnitPrice/oPurchaseInvoice.RateOn) *oPurchaseInvoice.ConvertionRate);
                }
                else
                {
                    oGRNDetail.UnitPrice = oItem.UnitPrice / oPurchaseInvoice.RateOn;
                }
                oGRNDetail.Amount = (oItem.YetToGRNQty * oGRNDetail.UnitPrice);               
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.LocalInvoice;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.LocalInvoice;
                oGRNDetail.RefObjectID = oItem.PurchaseInvoiceDetailID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.MUName;
                oGRNDetail.MUSymbol = oItem.MUSymbol;                
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotNo = "";
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromPO(int nPOID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            List<PurchaseOrderDetail> oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
            _oCompany = new Company();
            PurchaseOrder oPurchaseOrder = new PurchaseOrder();
            _oCompany = _oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oPurchaseOrder = oPurchaseOrder.Get(nPOID, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_PurchaseOrderDetail AS TT WHERE TT.POID=" + nPOID.ToString() + " AND TT.YetToGRNQty>0";
            oPurchaseOrderDetails = PurchaseOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (PurchaseOrderDetail oItem in oPurchaseOrderDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = oItem.ProductSpec;
                oGRNDetail.MUnitID = oItem.MUnitID;
                //oGRNDetail.RefQty = oItem.YetToGRNQty;
                oGRNDetail.RefQty = oItem.Qty;
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                if (_oCompany.BaseCurrencyID != oPurchaseOrder.CurrencyID)
                {
                    oGRNDetail.UnitPrice = (oItem.UnitPrice * oPurchaseOrder.CRate);
                }
                else
                {
                    oGRNDetail.UnitPrice = oItem.UnitPrice;
                }

                oGRNDetail.Amount = (oItem.YetToGRNQty * oGRNDetail.UnitPrice);               
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.PurchaseOrder;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.PurchaseOrder;
                oGRNDetail.RefObjectID = oItem.PODetailID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.UnitName;
                oGRNDetail.MUSymbol = oItem.UnitSymbol;                
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotNo = "";
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromImportInvoice(int nInvoiceID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();            
            List<ImportPackDetail> oImportPackDetails = new List<ImportPackDetail>();
            string sSQL = "SELECT * FROM View_ImportPackDetail AS TT WHERE TT.ImportInvoiceID=" + nInvoiceID.ToString() + " AND ISNULL(TT.YetToGRNQty,0)>0 order by ProductID, LotNo";            
            oImportPackDetails = ImportPackDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);           
            foreach (ImportPackDetail oItem in oImportPackDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = oItem.ProductSpec;
                oGRNDetail.MUnitID = oItem.MUnitID;
                //oGRNDetail.RefQty = oItem.YetToGRNQty; 
                oGRNDetail.RefQty = oItem.Qty; //requiremnt by Sabuj
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                oGRNDetail.UnitPrice = (oItem.UnitPriceBC + oItem.InvoiceLandingCost + oItem.LCLandingCost);
                oGRNDetail.Amount = (oItem.YetToGRNQty * (oItem.UnitPriceBC + oItem.InvoiceLandingCost + oItem.LCLandingCost));
                oGRNDetail.InvoiceLandingCost = oItem.InvoiceLandingCost;
                oGRNDetail.LCLandingCost = oItem.LCLandingCost;
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.ImportInvoice;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.ImportInvoice;
                oGRNDetail.RefObjectID = oItem.ImportPackDetailID;                
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.MUName;
                oGRNDetail.MUSymbol = oItem.MUName;
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotNo = oItem.LotNo;
                oGRNDetail.NumberOfPack = oItem.NumberOfPack;
                oGRNDetail.QtyPerPack = oItem.QtyPerPack;
                oGRNDetail.Brand = oItem.Brand;
                oGRNDetail.Origin = oItem.Origin;
                oGRNDetail.StyleID = oItem.TechnicalSheetID;
                oGRNDetail.StyleNo = oItem.StyleNo;
                oGRNDetail.Shade = oItem.Shade;
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromWorkOrder(int nWorkOrderID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
            _oCompany = new Company();
            WorkOrder oWorkOrder = new WorkOrder();
            _oCompany = _oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oWorkOrder = oWorkOrder.Get(nWorkOrderID, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_WorkOrderDetail AS TT WHERE TT.WorkOrderID=" + nWorkOrderID.ToString() + " AND TT.YetToGRNQty>0";
            oWorkOrderDetails = WorkOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (WorkOrderDetail oItem in oWorkOrderDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;                
                oGRNDetail.TechnicalSpecification = oItem.ProductDescription;
                oGRNDetail.MUnitID = oItem.UnitID;
                //oGRNDetail.RefQty = oItem.YetToGRNQty;
                oGRNDetail.RefQty = oItem.Qty;
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                if (_oCompany.BaseCurrencyID != oWorkOrder.CurrencyID)
                {
                    oGRNDetail.UnitPrice = ((oItem.UnitPrice / oWorkOrder.RateUnit) * oWorkOrder.CRate);
                }
                else
                {
                    oGRNDetail.UnitPrice = (oItem.UnitPrice / oItem.RateUnit);
                }
                oGRNDetail.Amount = (oItem.YetToGRNQty * oGRNDetail.UnitPrice);
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.WorkOrder;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.WorkOrder;
                oGRNDetail.RefObjectID = oItem.WorkOrderDetailID;
                oGRNDetail.StyleID = oItem.StyleID;
                oGRNDetail.ColorID = oItem.ColorID;
                oGRNDetail.SizeID = oItem.SizeID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.UnitName;
                oGRNDetail.MUSymbol = oItem.UnitSymbol;
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.StyleNo = oItem.StyleNo;
                oGRNDetail.BuyerName = oItem.BuyerName;
                oGRNDetail.ColorName = oItem.ColorName;
                oGRNDetail.SizeName = oItem.SizeName;
                oGRNDetail.Remarks = oItem.Remarks;
                oGRNDetail.MCDia = oItem.MCDia;
                oGRNDetail.FinishDia = oItem.FinishDia;
                oGRNDetail.GSM = oItem.GSM;
                oGRNDetail.Stretch_Length= oItem.Stretch_Length;
                oGRNDetail.LotNo = "";
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromImportPI(int nImportPIID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
            List<ImportPIDetail> oImportPIDetails = new List<ImportPIDetail>();

            string sSQL = "SELECT * FROM View_ImportPIDetail AS TT WHERE TT.ImportPIID =" + nImportPIID.ToString() + " AND ISNULL(TT.YetToGRNQty,0)>0";
            oImportPIDetails = ImportPIDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (ImportPIDetail oItem in oImportPIDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = "";
                oGRNDetail.MUnitID = oItem.MUnitID;
                //oGRNDetail.RefQty = oItem.YetToGRNQty;
                oGRNDetail.RefQty = oItem.Qty;
                oGRNDetail.ReceivedQty = oItem.YetToGRNQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToGRNQty;
                oGRNDetail.UnitPrice = (oItem.UnitPrice * oItem.CRate);
                oGRNDetail.Amount = (oItem.YetToGRNQty * (oItem.UnitPrice * oItem.CRate));
                oGRNDetail.InvoiceLandingCost = 0.00;
                oGRNDetail.LCLandingCost = 0.00;
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.ImportPI;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.ImportPI;
                oGRNDetail.RefObjectID = oItem.ImportPIDetailID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.MUName;
                oGRNDetail.MUSymbol = oItem.MUName;
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotNo = "";
                oGRNDetail.NumberOfPack = 0.00;
                oGRNDetail.QtyPerPack = 0.00;
                oGRNDetail.Brand = "";
                oGRNDetail.Origin = "";
                oGRNDetail.StyleID = oItem.TechnicalSheetID;
                oGRNDetail.StyleNo = oItem.StyleNo;
                oGRNDetail.BuyerName = oItem.BuyerName;
                oGRNDetail.Shade = oItem.Shade;
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        private List<GRNDetail> MapGRNDetailFromConsumtion(int nConsumtionID)
        {
            GRNDetail oGRNDetail = new GRNDetail();
            List<GRNDetail> oGRNDetails = new List<GRNDetail>();
          
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            string sSQL = "SELECT * FROM View_ConsumptionRequisitionDetail AS TT WHERE TT.YetToReturnQty>0 AND TT.ConsumptionRequisitionID =" + nConsumtionID.ToString();
            oConsumptionRequisitionDetails = ConsumptionRequisitionDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (ConsumptionRequisitionDetail oItem in oConsumptionRequisitionDetails)
            {
                oGRNDetail = new GRNDetail();
                oGRNDetail.GRNDetailID = 0;
                oGRNDetail.GRNID = 0;
                oGRNDetail.ProductID = oItem.ProductID;
                oGRNDetail.TechnicalSpecification = "";
                oGRNDetail.MUnitID = oItem.UnitID;
                //oGRNDetail.RefQty = oItem.YetToReturnQty;
                oGRNDetail.RefQty = oItem.Quantity;
                oGRNDetail.ReceivedQty = oItem.YetToReturnQty;
                oGRNDetail.YetToReceiveQty = oItem.YetToReturnQty;
                oGRNDetail.UnitPrice = oItem.UnitPrice;
                oGRNDetail.Amount = (oItem.YetToReturnQty * oItem.UnitPrice);
                oGRNDetail.InvoiceLandingCost = 0.00;
                oGRNDetail.LCLandingCost = 0.00;
                oGRNDetail.LotID = 0;
                oGRNDetail.RefType = EnumGRNType.FloorReturn;
                oGRNDetail.RefTypeInt = (int)EnumGRNType.FloorReturn;
                oGRNDetail.RefObjectID = oItem.ConsumptionRequisitionDetailID;
                oGRNDetail.GRNNo = "";
                oGRNDetail.MUName = oItem.UnitName;
                oGRNDetail.MUSymbol = oItem.Symbol;
                oGRNDetail.ProductName = oItem.ProductName;
                oGRNDetail.ProductCode = oItem.ProductCode;
                oGRNDetail.LotID = oItem.LotID;
                oGRNDetail.LotNo = oItem.LotNo;
                oGRNDetail.StyleID = oItem.StyleID;
                oGRNDetail.ColorID = oItem.ColorID;
                oGRNDetail.SizeID = oItem.SizeID;
                oGRNDetail.NumberOfPack = 0.00;
                oGRNDetail.QtyPerPack = 0.00;
                oGRNDetail.Brand = "";
                oGRNDetail.Origin = "";
                oGRNDetails.Add(oGRNDetail);
            }
            return oGRNDetails;
        }
        public ActionResult PreviewGRN(int bIsA4, int id)
        {
            _oGRN = new GRN();
            _oClientOperationSetting = new ClientOperationSetting();
            _oGRN = _oGRN.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oGRN.GRNDetails = GRNDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oGRN.GRNDetails = _oGRN.GRNDetails.OrderBy(x => x.StyleNo).ToList();
            _oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oGRN.BusinessUnit = oBusinessUnit.Get(_oGRN.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.GRN + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.GRNPreview, (int)Session[SessionInfo.currentUserID]);

            rptGRN oReport = new rptGRN();
            byte[] abytes = oReport.PrepareReport(_oGRN, _oClientOperationSetting, oCompany, oApprovalHeads, Convert.ToBoolean(bIsA4), oSignatureSetups);
            return File(abytes, "application/pdf");
        }

        public ActionResult PreviewGRNWithRate(int bIsA4, int id)
        {
            _oGRN = new GRN();
            _oGRN = _oGRN.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oGRN.GRNDetails = GRNDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oGRN.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);

            //List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            //oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.GRN + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.GRNBillPreview, (int)Session[SessionInfo.currentUserID]);
          
            rptGRNWithRate oReport = new rptGRNWithRate();
            byte[] abytes = oReport.PrepareReport(_oGRN, oCompany, Convert.ToBoolean(bIsA4), oSignatureSetups);
            return File(abytes, "application/pdf");
        }

        public ActionResult MRIRPrint(int id)
        {
            _oGRN = new GRN();
            _oGRN = _oGRN.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oGRN.GRNDetails = GRNDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oGRN.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptMRIRPrint oReport = new rptMRIRPrint();
            byte[] abytes = oReport.PrepareReport(_oGRN, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintGRNs(string Param)
        {
            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping=new List<AuthorizationRoleMapping>(); 
           oAuthorizationRoleMapping= AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

           oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
           if (oAuthorizationRoleMapping.Count>0)
           {
               bIsRateView = true;
           }


            _oGRNs = new List<GRN>();
            string sSQLQuery = "SELECT *  FROM View_GRN WHERE GRNID IN ("+Param+")";
            _oGRNs = GRN.Gets(sSQLQuery, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            string Messge = "GRN List";
            rptGRNs oReport = new rptGRNs();
            byte[] abytes = oReport.PrepareReport(_oGRNs, oCompany, Messge, bIsRateView);
            return File(abytes, "application/pdf");

        }

        public Image GetDBCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.Get(id, (int)Session[SessionInfo.currentUserID]);
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
            #endregion
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
        #region UpdateVoucherEffect

        [HttpPost]
        public JsonResult UpdateVoucherEffect(GRN oGRN)
        {
            try
            {
                oGRN = oGRN.UpdateVoucherEffect((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oGRN = new GRN();
                oGRN.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGRN);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Advance Search
        [HttpPost]
        public JsonResult GetsByGRNNo(GRN oGRN)
        {
            List<GRN> oGRNs = new List<GRN>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_GRN AS HH WHERE  (HH.GRNNo+HH.VendorName+HH.RefObjectNo) LIKE '%" + oGRN.GRNNo + "%' ";
                if (oGRN.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oGRN.BUID ;
                }
                sSQL += " ORDER BY GRNID ASC";
                oGRNs = GRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oGRNs = new List<GRN>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGRNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitForReceived(GRN oGRN)
        {
            _oGRNs = new List<GRN>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_GRN WHERE ISNULL(ReceivedBy,0)=0 ";
                if (oGRN.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oGRN.BUID;
                }
                sSQL += " ORDER BY GRNID ASC";
                _oGRNs = GRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRNs = new List<GRN>();
                _oGRN.ErrorMessage = ex.Message;
                _oGRNs.Add(_oGRN);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(GRN oGRN)
        {
            _oGRNs = new List<GRN>();
            try
            {
                string sSQL = this.GetSQL(oGRN);
                _oGRNs = GRN.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGRN = new GRN();
                _oGRNs = new List<GRN>();
                _oGRN.ErrorMessage = ex.Message;
                _oGRNs.Add(_oGRN);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGRNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(GRN oGRN )
        {
            
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sGRNNo = Convert.ToString(oGRN.Remarks.Split('~')[0]);
            string sInvoiceNo = Convert.ToString(oGRN.Remarks.Split('~')[1]);
            EnumCompareOperator eGRNDate = (EnumCompareOperator)Convert.ToInt32(oGRN.Remarks.Split('~')[2]);
            DateTime dStartDate = Convert.ToDateTime(oGRN.Remarks.Split('~')[3]);
            DateTime dEndDate = Convert.ToDateTime(oGRN.Remarks.Split('~')[4]);
            EnumCompareOperator eInvoiceAmount = (EnumCompareOperator)Convert.ToInt32(oGRN.Remarks.Split('~')[5]);
            double nStartAmount = Convert.ToDouble(oGRN.Remarks.Split('~')[6]);
            double nEndAmount = Convert.ToDouble(oGRN.Remarks.Split('~')[7]);
            int nReceivedBy = Convert.ToInt32(oGRN.Remarks.Split('~')[8]);
            string sSupplierIDs = Convert.ToString(oGRN.Remarks.Split('~')[9]);
            string sProductIDs = Convert.ToString(oGRN.Remarks.Split('~')[10]);
            string sStyleIDs = Convert.ToString(oGRN.Remarks.Split('~')[11]);

            string sReturn1 = "SELECT * FROM View_GRN";
            string sReturn = "";

            #region BUID
            if (oGRN.BUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oGRN.BUID.ToString();
            }
            #endregion

            #region InvoiceSLNo
            if (sGRNNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNNo LIKE '%" + sGRNNo + "%'";
            }
            #endregion

            #region InvoiceNo
            if (sInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefObjectNo LIKE '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region GRN Date
            if (eGRNDate != EnumCompareOperator.None)
            {
                if (eGRNDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eGRNDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),GRNDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region GRNAmount
            if (eInvoiceAmount != EnumCompareOperator.None)
            {
                if (eInvoiceAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount NOT BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
            }
            #endregion

            #region Received By
            if (nReceivedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ReceivedBy,0) = " + nReceivedBy.ToString();
            }
            #endregion

            #region SupplierIDs
            if (sSupplierIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sSupplierIDs + ")";
            }
            #endregion

            #region Products
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNID IN (SELECT TT.GRNID FROM GRNDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            #region Styles
            if (!string.IsNullOrEmpty(sStyleIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNID IN (SELECT TT.GRNID FROM GRNDetail AS TT WHERE TT.StyleID IN(" + sStyleIDs + "))";
            }
            #endregion

            #region GRN Type
            if (oGRN.GRNTypeInt != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNType = " + oGRN.GRNTypeInt;
            }
            #endregion

            #region GRNStatus
            if (oGRN.GRNStatusInt>-1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " GRNStatus = " + oGRN.GRNStatusInt;
            }
            #endregion

            #region GRN Type
            if (oGRN.StoreID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StoreID = " + oGRN.StoreID;
            }
            #endregion
            
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion
    }
}
