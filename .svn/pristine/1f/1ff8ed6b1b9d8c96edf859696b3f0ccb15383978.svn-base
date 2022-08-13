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
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSolFinancial.Controllers
{
    public class PurchaseInvoiceController : Controller
    {
        # region Declaration
        Product _oProduct = new Product();
        List<Product> _oProducts = new List<Product>();
        PurchaseInvoice _oPurchaseInvoice = new PurchaseInvoice();
        List<PurchaseInvoice> _oPurchaseInvoices = new List<PurchaseInvoice>();
        PurchaseInvoiceDetail _oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
        List<PurchaseInvoiceDetail> _oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        #endregion

        #region PurchaseInvoice Actions
        public ActionResult ViewPurchaseInvoices(int pit, int buid, int menuid)
        {
            //pit = purchase invoice type  pit 1 = normal invoice & 2 = landing cost
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PurchaseInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oPurchaseInvoices = new List<PurchaseInvoice>();

            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "";
            if (pit == 1)
                sSQL = "SELECT * FROM View_PurchaseInvoice AS HH WHERE HH.RefType IN (1,2)  AND ISNULL(HH.ApprovedBy,0)=0 ";
            else
                sSQL = "SELECT * FROM View_PurchaseInvoice AS HH WHERE HH.RefType IN (3,4) AND ISNULL(HH.ApprovedBy,0)=0";
            if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                sSQL += " AND BUID =" + buid;
            sSQL += " ORDER BY HH.PurchaseInvoiceID ASC";
            _oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            #region Approval Users
            sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM PurchaseInvoice AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion
            
            #region Ref Type           
            List<EnumObject> oRefTyps = new List<EnumObject>();
            List<EnumObject> oTempRefTyps = new List<EnumObject>();
            oTempRefTyps = EnumObject.jGets(typeof(EnumInvoiceReferenceType));
            foreach (EnumObject oItem in oTempRefTyps)
            {
                if (pit == 2)
                {
                    if ((EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.Import || (EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.Local)
                    {
                        oRefTyps.Add(oItem);
                    }
                }
                else
                {
                    if ((EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.Open || (EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.PO)
                    {
                        oRefTyps.Add(oItem);
                    }
                }
            }
            #endregion

            ViewBag.RefTyps = oRefTyps;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.InvoicePaymentModes = EnumObject.jGets(typeof(EnumInvoicePaymentMode));
            ViewBag.PIT = pit;
            return View(_oPurchaseInvoices);
        }
        public ActionResult ViewPurchaseInvoice(int nid, decimal ts)
        {
            _oClientOperationSetting = new ClientOperationSetting();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();
            if (nid > 0)
            {
                oPurchaseInvoice = oPurchaseInvoice.Get(nid, (int)Session[SessionInfo.currentUserID]);
                oPurchaseInvoice.PurchaseInvoiceDetails = PurchaseInvoiceDetail.GetsByPurchaseInvoiceID(nid, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oPurchaseInvoice.CurrencyID = oCompany.BaseCurrencyID;
                oPurchaseInvoice.ConvertionRate = 1;
            }

            #region InvoiceType & Reference Type
            List<EnumObject> oInvoiceReferenceTypeObjs = new List<EnumObject>();
            List<EnumObject> oTempEnumObjects = EnumObject.jGets(typeof(EnumInvoiceReferenceType));
            foreach (EnumObject oItem in oTempEnumObjects)
            {
                if ((EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.Open || (EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.PO || (EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.WO)
                {
                    oInvoiceReferenceTypeObjs.Add(oItem);
                }
            }
            #endregion

            ViewBag.ServiceCharge = ServiceCharge.Gets("SELECT * FROM ServiceCharge", (int)Session[SessionInfo.currentUserID]);
            ViewBag.InvoicePaymentModes = EnumObject.jGets(typeof(EnumInvoicePaymentMode));
            ViewBag.PInvoiceTypeObjs = EnumObject.jGets(typeof(EnumPInvoiceType));
            ViewBag.InvoiceReferenceTypeObj = oInvoiceReferenceTypeObjs;
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany;
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oPurchaseInvoice);
        }
        public ActionResult ViewLandingCost(int nid, int nbuid, decimal ts)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            PurchaseInvoice oPurchaseInvoice = new PurchaseInvoice();
            if (nid > 0)
            {
                oPurchaseInvoice = oPurchaseInvoice.Get(nid, (int)Session[SessionInfo.currentUserID]);
                oPurchaseInvoice.PurchaseInvoiceDetails = PurchaseInvoiceDetail.GetsByPurchaseInvoiceID(nid, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oPurchaseInvoice.CurrencyID = oCompany.BaseCurrencyID;
                oPurchaseInvoice.ConvertionRate = 1;
                oPurchaseInvoice.InvoiceTypeInt = (int)EnumPInvoiceType.Standard;                
            }

            #region InvoiceType & Reference Type
            List<EnumObject> oInvoiceReferenceTypeObjs = new List<EnumObject>();
            List<EnumObject> oTempEnumObjects = EnumObject.jGets(typeof(EnumInvoiceReferenceType));
            foreach (EnumObject oItem in oTempEnumObjects)
            {
                if ((EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.Local || (EnumInvoiceReferenceType)oItem.id == EnumInvoiceReferenceType.Import)
                {
                    oInvoiceReferenceTypeObjs.Add(oItem);
                }
            }

            List<EnumObject> oInvoiceTypeObjs = new List<EnumObject>();
            List<EnumObject> oTempInvoiceTypeObjs = EnumObject.jGets(typeof(EnumPInvoiceType));
            foreach (EnumObject oItem in oTempInvoiceTypeObjs)
            {
                if ((EnumPInvoiceType)oItem.id == EnumPInvoiceType.Standard || (EnumPInvoiceType)oItem.id == EnumPInvoiceType.Advance || (EnumPInvoiceType)oItem.id == EnumPInvoiceType.Bonded)
                {
                    oInvoiceTypeObjs.Add(oItem);
                }
            }
            #endregion

            ViewBag.PInvoiceTypeObjs = oInvoiceTypeObjs;
            ViewBag.InvoiceReferenceTypeObj = oInvoiceReferenceTypeObjs;
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany;
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsLandingCostHeadLedger, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PaymentMethod = EnumObject.jGets(typeof(EnumPaymentMethod));
            ViewBag.BankAccounts = BankAccount.Gets("SELECT * FROM View_BankAccount WHERE BusinessUnitID = " + nbuid.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oPurchaseInvoice);
        }
        [HttpPost]
        public JsonResult Save(PurchaseInvoice oPurchaseInvoice)
        {
            try
            {
                oPurchaseInvoice.Note = oPurchaseInvoice.Note == null ? "" : oPurchaseInvoice.Note;
                oPurchaseInvoice = oPurchaseInvoice.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseInvoice = new PurchaseInvoice();
                _oPurchaseInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveApprovalHistory(ApprovalHistory oApprovalHistory)
        {
            ApprovalHistory _oApprovalHistory = new ApprovalHistory();
            PurchaseInvoice _oPurchaseInvoice = new PurchaseInvoice();

            try
            {
                if (oApprovalHistory.ObjectRefID <= 0)
                    throw new Exception("Invalid Object ID!");

                _oApprovalHistory = oApprovalHistory.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oPurchaseInvoice = _oPurchaseInvoice.Get(oApprovalHistory.ObjectRefID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseInvoice = new PurchaseInvoice();
                _oPurchaseInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(PurchaseInvoice oPurchaseInvoice)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oPurchaseInvoice.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approved(PurchaseInvoice oPurchaseInvoice)
        {
            try
            {
                oPurchaseInvoice.Note = oPurchaseInvoice.Note == null ? "" : oPurchaseInvoice.Note;                
                oPurchaseInvoice = oPurchaseInvoice.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseInvoice = new PurchaseInvoice();

                _oPurchaseInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UndoApproved(PurchaseInvoice oPurchaseInvoice)
        {
            try
            {
                oPurchaseInvoice.Note = oPurchaseInvoice.Note == null ? "" : oPurchaseInvoice.Note;
                oPurchaseInvoice = oPurchaseInvoice.UndoApproved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseInvoice = new PurchaseInvoice();

                _oPurchaseInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByInvoiceNo(PurchaseInvoice oPurchaseInvoice)
        {
            _oPurchaseInvoices = new List<PurchaseInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseInvoice WHERE BUID = " + oPurchaseInvoice.BUID + " AND  PurchaseInvoiceNo LIKE '%" + oPurchaseInvoice.PurchaseInvoiceNo + "%' ";
                _oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseInvoice = new PurchaseInvoice();
                _oPurchaseInvoices = new List<PurchaseInvoice>();
                _oPurchaseInvoice.ErrorMessage = ex.Message;
                _oPurchaseInvoices.Add(_oPurchaseInvoice);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Supporting Data Gets
        [HttpPost]
        public JsonResult GetsPOForInvoice(PurchaseOrder oPurchaseOrder)
        {
            List<PurchaseOrder> oPurchaseOrders = new List<PurchaseOrder>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseOrder AS HH";
                string sReturn = "";

                #region Deafult Only Approved PO
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(HH.ApproveBy,0) !=0";
                #endregion

                #region Deafult Not In Invoice
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.YetToInvocieQty>0";
                #endregion

                #region BusinessUnit
                if (oPurchaseOrder.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.BUID=" + oPurchaseOrder.BUID.ToString();
                }
                #endregion

                #region PO No
                if (!String.IsNullOrEmpty(oPurchaseOrder.PONo))
                {
                    oPurchaseOrder.PONo = oPurchaseOrder.PONo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.PONo LIKE'%" + oPurchaseOrder.PONo + "%'";
                }
                #endregion
               
                #region ContractorID
                if (oPurchaseOrder.ContractorID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " HH.ContractorID=" + oPurchaseOrder.ContractorID.ToString();
                }
                #endregion

                sSQL = sSQL + " " + sReturn + " ORDER BY HH.POID DESC";
                oPurchaseOrders = PurchaseOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oPurchaseOrders = new List<PurchaseOrder>();
                oPurchaseOrder = new PurchaseOrder();
                oPurchaseOrder.ErrorMessage = ex.Message;
                oPurchaseOrders.Add(oPurchaseOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.PurchaseInvoice, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.PurchaseInvoice, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetProductsByPO(PurchaseInvoice oPurchaseInvoice)
        {
            PurchaseInvoiceDetail oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
            List<PurchaseOrderDetail> oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
            List<PurchaseInvoiceDetail> oPurchaseInvoiceDetails = new List<PurchaseInvoiceDetail>();
            try
            {
                if (oPurchaseInvoice.RefID > 0)
                {
                    string sSQL = "";
                    if ((EnumPInvoiceType)oPurchaseInvoice.InvoiceTypeInt == EnumPInvoiceType.Advance)
                    {
                        sSQL = "SELECT * FROM View_PurchaseOrderDetail AS HH WHERE HH.POID=" + oPurchaseInvoice.RefID.ToString() + " ORDER BY HH.PODetailID ASC";
                    }
                    else
                    {
                        sSQL = "SELECT * FROM View_PurchaseOrderDetail AS HH WHERE HH.POID=" + oPurchaseInvoice.RefID.ToString() + " AND HH.YetToInvoiceQty>0 ORDER BY HH.PODetailID ASC";
                    }
                    oPurchaseOrderDetails = PurchaseOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (PurchaseOrderDetail oItem in oPurchaseOrderDetails)
                    {
                        oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
                        oPurchaseInvoiceDetail.PurchaseInvoiceDetailID = 0;
                        oPurchaseInvoiceDetail.PurchaseInvoiceID = 0;
                        oPurchaseInvoiceDetail.OrderRecapID = oItem.OrderRecapID;
                        oPurchaseInvoiceDetail.OrderRecapNo = oItem.OrderRecapNo;
                        oPurchaseInvoiceDetail.StyleNo = oItem.StyleNo;
                        oPurchaseInvoiceDetail.BuyerName = oItem.BuyerName;
                        oPurchaseInvoiceDetail.ProductID = oItem.ProductID;
                        oPurchaseInvoiceDetail.UnitPrice = oItem.UnitPrice;
                        oPurchaseInvoiceDetail.Qty = oItem.YetToInvoiceQty;
                        oPurchaseInvoiceDetail.ReceiveQty = 0;
                        oPurchaseInvoiceDetail.MUnitID = oItem.MUnitID;
                        oPurchaseInvoiceDetail.RefDetailID = oItem.PODetailID;
                        oPurchaseInvoiceDetail.GRNID = 0;
                        oPurchaseInvoiceDetail.Amount = (oItem.YetToInvoiceQty * oItem.UnitPrice);
                        oPurchaseInvoiceDetail.AdvanceSettle = 0;
                        oPurchaseInvoiceDetail.LCID = 0;
                        oPurchaseInvoiceDetail.InvoiceID = 0;
                        oPurchaseInvoiceDetail.CostHeadID = 0;
                        oPurchaseInvoiceDetail.Remarks = "";
                        oPurchaseInvoiceDetail.ProductCode = oItem.ProductCode;
                        oPurchaseInvoiceDetail.ProductName = oItem.ProductName;
                        oPurchaseInvoiceDetail.ProductSpec = oItem.ProductSpec;
                        oPurchaseInvoiceDetail.AccountHeadID = 0;
                        oPurchaseInvoiceDetail.MUName = oItem.UnitName;
                        oPurchaseInvoiceDetail.MUSymbol = oItem.UnitSymbol;
                        oPurchaseInvoiceDetail.PODQty = oItem.Qty;
                        oPurchaseInvoiceDetail.PODRate = oItem.UnitPrice;
                        oPurchaseInvoiceDetail.PODAmount = (oItem.Qty * oItem.UnitPrice);
                        oPurchaseInvoiceDetail.LCNo = "";
                        oPurchaseInvoiceDetail.ShipmentDate = DateTime.Today;
                        oPurchaseInvoiceDetail.InvoiceNo = "";
                        oPurchaseInvoiceDetail.InvoiceDate = DateTime.Today;
                        oPurchaseInvoiceDetail.ConvertionRate = 0;
                        oPurchaseInvoiceDetail.CurrencyName = "";
                        oPurchaseInvoiceDetail.CurrencySymbol = "";
                        oPurchaseInvoiceDetail.YetToInvoiceQty = oItem.YetToInvoiceQty;
                        oPurchaseInvoiceDetail.YetToGRNQty = 0;
                        oPurchaseInvoiceDetail.PrevoiusInvoice = oItem.InvoiceValue;
                        oPurchaseInvoiceDetail.AdvanceSettle = 0;
                        oPurchaseInvoiceDetail.AdvInvoice = oItem.AdvInvoice;
                        oPurchaseInvoiceDetails.Add(oPurchaseInvoiceDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                _oPurchaseInvoiceDetail = new PurchaseInvoiceDetail();
                _oPurchaseInvoiceDetail.ErrorMessage = ex.Message;
                oPurchaseInvoiceDetails.Add(_oPurchaseInvoiceDetail);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oPurchaseInvoiceDetails);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jSonResult = Json(oPurchaseInvoiceDetails, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetPaymentTermsForInvoice(PaymentTerm oPaymentTerm)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM PaymentTerm ";
            string sTempReturn = "";
            if (oPaymentTerm.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sTempReturn);
                sTempReturn += " BUID =" + oPaymentTerm.BUID;
            }
            if (oPaymentTerm.TermText != "" && oPaymentTerm.TermText != null)
            {
                Global.TagSQL(ref sTempReturn);
                sTempReturn += " TermText LIKE '%" + oPaymentTerm.TermText + "%'";
            }
            sSQL += sTempReturn;
            List<PaymentTerm> oPaymentTerms = new List<PaymentTerm>();
            oPaymentTerms = PaymentTerm.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPaymentTerms);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsImportLC(ImportLC oImportLC)//for landing Cost entry
        {
            List<ImportLC> oImportLCs = new List<ImportLC>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                if(oImportLC.ImportLCNo==null){ oImportLC.ImportLCNo=""; }
                string sSQL = "SELECT * FROM View_ImportLC AS HH WHERE HH.ImportLCNo LIKE '%" + oImportLC.ImportLCNo.Trim() + "%' AND ISNULL(HH.ReceivedBy,0)!=0 ";
                if (oImportLC.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oImportLC.BUID;
                }
                sSQL += "ORDER BY HH.ImportLCID ASC";
                oImportLCs = ImportLC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportLC = new ImportLC();
                oImportLCs = new List<ImportLC>();
                oImportLC.ErrorMessage = ex.Message;
                oImportLCs.Add(oImportLC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsImportInvoice(ImportInvoice oImportInvoice)//for landing Cost entry
        {
            List<ImportInvoice> oImportInvoices = new List<ImportInvoice>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_ImportInvoice AS HH WHERE ISNULL(HH.InvoiceStatus,0)!=0";
                if (oImportInvoice.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oImportInvoice.BUID;
                }
                if (oImportInvoice.ImportInvoiceNo != null && oImportInvoice.ImportInvoiceNo != "")
                {
                    sSQL = sSQL + " AND HH.ImportInvoiceNo LIKE '%"+oImportInvoice.ImportInvoiceNo.Trim()+"%'";
                }
                if (oImportInvoice.ImportLCID > 0)
                {
                    sSQL = sSQL + " AND HH.ImportLCID = " + oImportInvoice.ImportLCID.ToString();
                }
                sSQL = sSQL + " ORDER BY HH.ImportInvoiceID ASC";                
                oImportInvoices = ImportInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportInvoice = new ImportInvoice();
                oImportInvoices = new List<ImportInvoice>();
                oImportInvoice.ErrorMessage = ex.Message;
                oImportInvoices.Add(oImportInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsImportInvoiceDetails(ImportInvoiceDetail oImportInvoiceDetail)//for landing Cost entry
        {
            List<ImportInvoiceDetail> oImportInvoiceDetails = new List<ImportInvoiceDetail>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_ImportInvoiceDetail AS HH WHERE ISNULL(HH.ImportInvoiceID,0)="+oImportInvoiceDetail.ImportInvoiceID;
                if(!string.IsNullOrEmpty(oImportInvoiceDetail.ProductName))
                {

                    sSQL += " AND HH.ProductName LIKE '%"+oImportInvoiceDetail.ProductName+"%'";
                }
                sSQL = sSQL + " ORDER BY HH.ImportInvoiceDetailID ASC";
                oImportInvoiceDetails = ImportInvoiceDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportInvoiceDetail = new ImportInvoiceDetail();
                oImportInvoiceDetails = new List<ImportInvoiceDetail>();
                oImportInvoiceDetail.ErrorMessage = ex.Message;
                oImportInvoiceDetails.Add(oImportInvoiceDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsPurchaseInvoice(PurchaseInvoice oPurchaseInvoice)//for landing Cost entry
        {
            List<PurchaseInvoice> oPurchaseInvoices = new List<PurchaseInvoice>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                if (oPurchaseInvoice.BillNo == null) { oPurchaseInvoice.BillNo = ""; }
                string sSQL = "SELECT * FROM View_PurchaseInvoice AS HH WHERE HH.InvoiceType!=2 AND HH.RefType NOT IN(3,4) AND ISNULL(HH.ApprovedBy,0)!=0 AND HH.BillNo LIKE '%" + oPurchaseInvoice.BillNo.Trim() + "%' ";
                if (oPurchaseInvoice.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oPurchaseInvoice.BUID;
                }
                sSQL += " ORDER BY HH.PurchaseInvoiceID ASC";
                oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoices = new List<PurchaseInvoice>();
                oPurchaseInvoice.ErrorMessage = ex.Message;
                oPurchaseInvoices.Add(oPurchaseInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCostHeads(ChartsOfAccount oChartsOfAccount)//for landing Cost entry
        {
            List<ChartsOfAccount> _oChartsOfAccounts = new List<ChartsOfAccount>();
            try
            {
                string sSQL = "SELECT * FROM View_ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Ledger+ " AND ComponentID IN(2,6)";
                if (!string.IsNullOrEmpty(oChartsOfAccount.AccountHeadName))
                {
                    sSQL += " AND AccountHeadName LIKE'%" + oChartsOfAccount.AccountHeadName + "%'";
                }
                _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount.ErrorMessage = ex.Message;
                _oChartsOfAccounts.Add(oChartsOfAccount);
            }

            var jsonResult = Json(_oChartsOfAccounts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsACCostCenters(ACCostCenter oACCostCenter)//for landing Cost entry
        {
            List<ACCostCenter> _oACCostCenters = new List<ACCostCenter>();
            try
            {
                string sSQL = "SELECT * FROM View_ACCostCenter  AS HH WHERE HH.ParentID NOT IN (0,1)  AND HH.ACCostCenterID IN (SELECT SubLedgerID FROM BUWiseSubLedger WHERE BusinessUnitID = "+oACCostCenter.BUID+")";
                if (!string.IsNullOrEmpty(oACCostCenter.Name))
                {
                    sSQL += " AND HH.Name LIKE'%" + oACCostCenter.Name + "%'";
                }
                _oACCostCenters = ACCostCenter.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oACCostCenter = new ACCostCenter();
                oACCostCenter.ErrorMessage = ex.Message;
                _oACCostCenters.Add(oACCostCenter);
            }
  
            var jsonResult = Json(_oACCostCenters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region UpdatePaymentMode
        
        [HttpPost]
        public JsonResult UpdatePaymentMode(PurchaseInvoice oPurchaseInvoice)
        {
            try
            {
                oPurchaseInvoice = oPurchaseInvoice.UpdatePaymentMode((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateVoucherEffect

        [HttpPost]
        public JsonResult UpdateVoucherEffect(PurchaseInvoice oPurchaseInvoice)
        {
            try
            {
                oPurchaseInvoice = oPurchaseInvoice.UpdateVoucherEffect((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oPurchaseInvoice = new PurchaseInvoice();
                oPurchaseInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Search
        [HttpPost]
        public JsonResult WaitForApproval(PurchaseInvoice oPurchaseInvoice)
        {
            _oPurchaseInvoices = new List<PurchaseInvoice>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                //pit = purchase invoice type  pit 1 = normal invoice & 2 = landing cost
                string sSQL = "";
                if (oPurchaseInvoice.PIT == 1)
                {
                    sSQL = "SELECT * FROM View_PurchaseInvoice AS HH WHERE HH.RefType IN (1,2)  AND ISNULL(HH.ApprovedBy,0)=0 ";
                }
                else
                {
                    sSQL = "SELECT * FROM View_PurchaseInvoice AS HH WHERE HH.RefType IN (3,4) AND ISNULL(HH.ApprovedBy,0)=0";
                }
                if (oPurchaseInvoice.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oPurchaseInvoice.BUID;
                }
                sSQL += " ORDER BY HH.PurchaseInvoiceID ASC";
                _oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseInvoice = new PurchaseInvoice();
                _oPurchaseInvoices = new List<PurchaseInvoice>();
                _oPurchaseInvoice.ErrorMessage = ex.Message;
                _oPurchaseInvoices.Add(_oPurchaseInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(PurchaseInvoice oPurchaseInvoice)
        {
            _oPurchaseInvoices = new List<PurchaseInvoice>();
            try
            {
                string sSQL = this.GetSQL(oPurchaseInvoice.Note, oPurchaseInvoice.BUID);
                _oPurchaseInvoices = PurchaseInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseInvoice = new PurchaseInvoice();
                _oPurchaseInvoices = new List<PurchaseInvoice>();
                _oPurchaseInvoice.ErrorMessage = ex.Message;
                _oPurchaseInvoices.Add(_oPurchaseInvoice);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sSearchingData, int nBUID)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sInvoiceNo = Convert.ToString(sSearchingData.Split('~')[0]);
            string sBillNo = Convert.ToString(sSearchingData.Split('~')[1]);
            EnumPInvoiceType ePInvoiceType = (EnumPInvoiceType)Convert.ToInt32(sSearchingData.Split('~')[2]);
            EnumInvoiceReferenceType eInvoiceReferenceType = (EnumInvoiceReferenceType)Convert.ToInt32(sSearchingData.Split('~')[3]);
            string sPONo = Convert.ToString(sSearchingData.Split('~')[4]);            
            EnumCompareOperator eInvoiceDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[5]);
            DateTime dInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[6]);
            DateTime dInvoiceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[7]);
            EnumCompareOperator eBillDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[8]);
            DateTime dBillStartDate = Convert.ToDateTime(sSearchingData.Split('~')[9]);
            DateTime dBillEndDate = Convert.ToDateTime(sSearchingData.Split('~')[10]);
            EnumCompareOperator eInvoiceAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[11]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[12]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[13]);
            int nApprovedBy = Convert.ToInt32(sSearchingData.Split('~')[14]);
            string sSupplierIDs = Convert.ToString(sSearchingData.Split('~')[15]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[16]);
            int nPIT = Convert.ToInt32(sSearchingData.Split('~')[17]);
            int nInvoicePaymentMode = Convert.ToInt32(sSearchingData.Split('~')[18]);
            string sImportLCNo = Convert.ToString(sSearchingData.Split('~')[19]);
            string sImportInvoiceNo = Convert.ToString(sSearchingData.Split('~')[20]);

            string sReturn1 = "SELECT * FROM View_PurchaseInvoice";
            string sReturn = "";

            #region PIT
            if (nPIT == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefType IN (" + (int)EnumInvoiceReferenceType.Open + "," + (int)EnumInvoiceReferenceType.PO + "," + (int)EnumInvoiceReferenceType.WO+ ")";
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefType IN (" + (int)EnumInvoiceReferenceType.Import + "," + (int)EnumInvoiceReferenceType.Local+")";
            }
            #endregion

            #region BUID
            if (nBUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID.ToString();
            }
            #endregion

            #region InvoiceNo
            if (sInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseInvoiceNo LIKE '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region BillNo
            if (sBillNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BillNo LIKE '%" + sBillNo + "%'";
            }
            #endregion

            #region InvoiceType
            if (ePInvoiceType != EnumPInvoiceType.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " InvoiceType = " + ((int)ePInvoiceType).ToString();
            }
            #endregion

            #region InvoiceReferenceType
            if (eInvoiceReferenceType != EnumInvoiceReferenceType.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefType = " + ((int)eInvoiceReferenceType).ToString();
            }
            #endregion
           
            #region PONo
            if (sPONo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PONo LIKE '%" + sPONo + "%'";
            }
            #endregion
            
            #region Invoice Date
            if (eInvoiceDate != EnumCompareOperator.None)
            {
                if (eInvoiceDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dInvoiceEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofInvoice,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dInvoiceStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dInvoiceEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Bill Date
            if (eBillDate != EnumCompareOperator.None)
            {
                if (eBillDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofBill,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dBillStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eBillDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofBill,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dBillStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eBillDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofBill,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dBillStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eBillDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofBill,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dBillStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eBillDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofBill,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dBillStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dBillEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eBillDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DateofBill,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dBillStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dBillEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region Invoice Amount
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

            #region ApprovedBy
            if (nApprovedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApprovedBy,0) = " + nApprovedBy.ToString();
            }
            #endregion

            #region InvoicePaymentMode
            if (nInvoicePaymentMode != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(InvoicePaymentMode,0) = " + nInvoicePaymentMode.ToString();
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
                sReturn = sReturn + " PurchaseInvoiceID IN (SELECT NN.PurchaseInvoiceID FROM PurchaseInvoiceDetail AS NN WHERE NN.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            #region ImportLCNo
            if (sImportLCNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseInvoiceID IN (SELECT NN.PurchaseInvoiceID FROM View_PurchaseInvoiceDetail AS NN WHERE NN.LCNo LIKE '%" + sImportLCNo + "%')";
            }
            #endregion

            #region ImportInvoiceNo
            if (sImportInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PurchaseInvoiceID IN (SELECT NN.PurchaseInvoiceID FROM View_PurchaseInvoiceDetail AS NN WHERE NN.InvoiceNo LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion       

        #region Printing
        public ActionResult PrintPurchaseInvoice(int id)
        {
            _oPurchaseInvoice = new PurchaseInvoice();
            _oClientOperationSetting = new ClientOperationSetting();
            _oPurchaseInvoice = _oPurchaseInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseInvoice.PurchaseInvoiceDetails = PurchaseInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            string sql = "select * from ApprovalHead where ModuleID = " + (int)EnumModuleName.PurchaseInvoice + "  AND BUID = " + _oPurchaseInvoice.BUID + " Order By Sequence";
            oApprovalHeads = ApprovalHead.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<ApprovalHistory> oApprovalHistorys = new List<ApprovalHistory>();
            sql = "SELECT * FROM View_ApprovalHistory WHERE ModuleID = " + (int)EnumModuleName.PurchaseInvoice + " AND BUID = " + _oPurchaseInvoice.BUID + " AND  ObjectRefID = " + id + " ORder BY ApprovalSequence";
            oApprovalHistorys = ApprovalHistory.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<PISpec> oPISpecs = new List<PISpec>();
            if (_oPurchaseInvoice.PurchaseInvoiceDetails.Any())
            {
                string sSql = "SELECT * FROM View_PISpec Where PIDetailID IN ( " + (string.Join(",", _oPurchaseInvoice.PurchaseInvoiceDetails.Select(x => x.PurchaseInvoiceDetailID))) +")";
                oPISpecs = PISpec.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            Contractor oContractor = new Contractor();
            oContractor = oContractor.Get(_oPurchaseInvoice.ContractorID, (int)Session[SessionInfo.currentUserID]);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPurchaseInvoice.BUID, (int)Session[SessionInfo.currentUserID]);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.PurchaseInvoicePreview, (int)Session[SessionInfo.currentUserID]);


            _oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptPurchaseInvoice oReport = new rptPurchaseInvoice();
            byte[] abytes=null;
            if (_oPurchaseInvoice.RefType == EnumInvoiceReferenceType.Open || _oPurchaseInvoice.RefType == EnumInvoiceReferenceType.PO) 
            {
                abytes = oReport.PrepareReport(_oPurchaseInvoice, oCompany, oContractor, oBusinessUnit, _oClientOperationSetting, oApprovalHeads, oApprovalHistorys, oPISpecs);
            }else
                abytes = oReport.PrepareReportLCost(_oPurchaseInvoice, oCompany, oContractor, oBusinessUnit, _oClientOperationSetting, oApprovalHeads, oApprovalHistorys, oPISpecs);

            return File(abytes, "application/pdf");
        }

        public ActionResult PrintPurchaseInvoices(string ids, double ts)
        {
            _oPurchaseInvoices = new List<PurchaseInvoice>();
            string sSql = "SELECT * FROM View_PurchaseInvoice WHERE PurchaseInvoiceID IN (" + ids + ") ORDER BY PurchaseInvoiceID ASC";
            _oPurchaseInvoices = PurchaseInvoice.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptPurchaseInvoices oReport = new rptPurchaseInvoices();
            byte[] abytes = oReport.PrepareReport(_oPurchaseInvoices, oCompany);
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

        #region SpecificationHead
        [HttpPost]
        public JsonResult ProductSpecHeadFORPOByProduct(PISpec oPISpec)
        {
            ProductSpecHead _oProductSpecHead = new ProductSpecHead();
            List<ProductSpecHead> _oSProductSpecHeads = new List<ProductSpecHead>();
            PISpec _oPISpec = new PISpec();
            List<PISpec> oPISpecs = new List<PISpec>();
            string sSQL = string.Empty;
            try
            {
                sSQL = "Select * from View_PISpec Where PIDetailID =" + oPISpec.PIDetailID;
                oPISpecs = PISpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from View_ProductSpecHead Where ProductID =" + oPISpec.ProductID + "Order By Sequence";
                _oSProductSpecHeads = ProductSpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oSProductSpecHeads.Any())
                {
                    if (oPISpecs.Any())
                    {
                        _oSProductSpecHeads.RemoveAll(x => oPISpecs.Select(p => p.SpecHeadID).Contains(x.SpecHeadID));

                    }
                    if (_oSProductSpecHeads.Any())
                    {
                        foreach (var oitem in _oSProductSpecHeads)
                        {
                            _oPISpec = new PISpec();
                            _oPISpec.SpecName = oitem.SpecName;
                            _oPISpec.PISpecID = 0;
                            _oPISpec.SpecHeadID = oitem.SpecHeadID;
                            _oPISpec.PIDescription = string.Empty;
                            _oPISpec.PIDetailID = oPISpec.PIDetailID;
                            oPISpecs.Add(_oPISpec);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _oProductSpecHead = new ProductSpecHead();
                _oProductSpecHead.ErrorMessage = ex.Message;
                _oSProductSpecHeads.Add(_oProductSpecHead);
            }

            var jsonResult = Json(oPISpecs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult IUDPISpec(PISpec oPISpec)
        {
            try
            {
                oPISpec = oPISpec.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPISpec = new PISpec();
                oPISpec.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPISpec);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
