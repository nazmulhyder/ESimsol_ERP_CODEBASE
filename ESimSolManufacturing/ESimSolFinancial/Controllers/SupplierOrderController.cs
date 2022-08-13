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
using ICS.Core.Utility;
using System.Linq;


namespace ESimSolFinancial.Controllers
{
    public class SupplierOrderController : Controller
    {
        #region Declaration
        Product _oProduct = new Product();
        List<Product> _oProducts = new List<Product>();
        PurchaseOrder _oPurchaseOrder = new PurchaseOrder();
        List<PurchaseOrder> _oPurchaseOrders = new List<PurchaseOrder>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        #endregion

        #region Supplier Order Actions
        public ActionResult ViewSupplierOrders(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PurchaseOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oPurchaseOrders = new List<PurchaseOrder>();

            #region Approval Users
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApproveBy FROM PurchaseOrder AS MM WHERE ISNULL(MM.ApproveBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oPurchaseOrders);
        }
        public ActionResult ViewSupplierOrder(int buid, int id)
        {
            PaymentTerm oPaymentTerm = new PaymentTerm();
            _oPurchaseOrder = new PurchaseOrder();
            Company oCompany = new Company();
            _oPurchaseOrder.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
            if (id > 0)
            {
                _oPurchaseOrder = _oPurchaseOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseOrder.PurchaseOrderDetails = PurchaseOrderDetail.Gets(_oPurchaseOrder.POID, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseOrder.POTandCClauses = POTandCClause.Gets(_oPurchaseOrder.POID, (int)Session[SessionInfo.currentUserID]);
                //Dont delete it;  just map for Payment term.
                oPaymentTerm = oPaymentTerm.Get(_oPurchaseOrder.PaymentTermID, (int)Session[SessionInfo.currentUserID]);
                _oPurchaseOrder.PaymentTermText = oPaymentTerm.FullTerm;
            }
            ViewBag.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.MarketingAccounts = MarketingAccount.GetsByBU(buid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Company = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            return View(_oPurchaseOrder);
        }
        [HttpPost]
        public JsonResult Save(PurchaseOrder oPurchaseOrder)
        {
            _oPurchaseOrder = new PurchaseOrder();            
            oPurchaseOrder.Note = oPurchaseOrder.Note == null ? "" : oPurchaseOrder.Note;
            try
            {
                _oPurchaseOrder = oPurchaseOrder;
                _oPurchaseOrder = _oPurchaseOrder.Save((int)Session[SessionInfo.currentUserID]);                
            }
            catch (Exception ex)
            {
                _oPurchaseOrder = new PurchaseOrder();
                _oPurchaseOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(PurchaseOrder oPurchaseOrder)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oPurchaseOrder.Delete(oPurchaseOrder.POID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approved(PurchaseOrder oPurchaseOrder)
        {
            _oPurchaseOrder = new PurchaseOrder();            
            oPurchaseOrder.Note = oPurchaseOrder.Note == null ? "" : oPurchaseOrder.Note;
            try
            {
                _oPurchaseOrder = oPurchaseOrder;
                _oPurchaseOrder = _oPurchaseOrder.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseOrder = new PurchaseOrder();
                _oPurchaseOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Get Product BU, User and Name wise ( write by Mahabub)
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.PurchaseOrder, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.PurchaseOrder, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsProductsFromPR(PurchaseOrder oPurchaseOrder)
        {
            List<NOADetail> oNOADetails = new List<NOADetail>();
            PurchaseOrderDetail oPurchaseOrderDetail = new PurchaseOrderDetail();
            List<PurchaseOrderDetail> oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
            List<PurchaseRequisitionDetail> oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
            try
            {
                    oPurchaseRequisitionDetails = PurchaseRequisitionDetail.Gets(oPurchaseOrder.RefID, (int)Session[SessionInfo.currentUserID]);
                    foreach (PurchaseRequisitionDetail oItem in oPurchaseRequisitionDetails)
                    {
                        oPurchaseOrderDetail = new PurchaseOrderDetail();
                        oPurchaseOrderDetail.OrderRecapID = oItem.OrderRecapID;
                        oPurchaseOrderDetail.OrderRecapNo = oItem.OrderRecapNo;
                        oPurchaseOrderDetail.StyleNo = oItem.StyleNo;
                        oPurchaseOrderDetail.BuyerName = oItem.BuyerName;
                        oPurchaseOrderDetail.ProductID = oItem.ProductID;
                        oPurchaseOrderDetail.ProductName = oItem.ProductName;
                        oPurchaseOrderDetail.ProductCode = oItem.ProductCode;
                        oPurchaseOrderDetail.Qty = oItem.Qty;
                        oPurchaseOrderDetail.MUnitID = oItem.MUnitID;
                        oPurchaseOrderDetail.UnitName = oItem.UnitName;
                        oPurchaseOrderDetail.UnitSymbol = oItem.UnitSymbol;
                        oPurchaseOrderDetail.VehicleModelID = oItem.VehicleModelID;
                        oPurchaseOrderDetail.ModelShortName = oItem.ModelShortName;
                        oPurchaseOrderDetails.Add(oPurchaseOrderDetail);
                    }
     
            }
            catch (Exception ex)
            {
                oPurchaseOrderDetail = new PurchaseOrderDetail();
                oPurchaseOrderDetail.ErrorMessage = ex.Message;
                oPurchaseOrderDetails.Add(oPurchaseOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPurchaseOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsVendors(PurchaseOrder oPurchaseOrder)
        {
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                if (oPurchaseOrder.RefTypeInt == (int)EnumPOReferenceType.NOA)
                {
                    oContractors = Contractor.Gets("Select * from Contractor where ContractorID in (Select SupplierID from View_NOADetail where NOAID = " + oPurchaseOrder.RefID + " AND ISNULL(ApprovedRate,0)>0 )", (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                Contractor oContractor = new Contractor();
                oContractor.ErrorMessage = ex.Message;
                oContractors.Add(oContractor);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Search
        [HttpPost]
        public JsonResult WaitForApproval(PurchaseOrder oPurchaseOrder)
        {
            _oPurchaseOrders = new List<PurchaseOrder>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseOrder WHERE  ISNULL(ApproveBy,0)=0 ";

                if (oPurchaseOrder.BUID> 0&& Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID =" + oPurchaseOrder.BUID;
                }
                sSQL += " ORDER BY POID ASC";
                _oPurchaseOrders = PurchaseOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseOrder = new PurchaseOrder();
                _oPurchaseOrders = new List<PurchaseOrder>();
                _oPurchaseOrder.ErrorMessage = ex.Message;
                _oPurchaseOrders.Add(_oPurchaseOrder);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult AdvanceSearch(PurchaseOrder oPurchaseOrder)
        {
            _oPurchaseOrders = new List<PurchaseOrder>();
            try
            {
                string sSQL = this.GetSQL(oPurchaseOrder.Note, oPurchaseOrder.BUID);
                _oPurchaseOrders = PurchaseOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseOrder = new PurchaseOrder();
                _oPurchaseOrders = new List<PurchaseOrder>();
                _oPurchaseOrder.ErrorMessage = ex.Message;
                _oPurchaseOrders.Add(_oPurchaseOrder);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sSearchingData, int nBUID)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sPONo = Convert.ToString(sSearchingData.Split('~')[0]);
            string sRefNo = Convert.ToString(sSearchingData.Split('~')[1]);
            EnumPOReferenceType ePOReferenceType = (EnumPOReferenceType)Convert.ToInt32(sSearchingData.Split('~')[2]);
            EnumCompareOperator ePODate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[3]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[5]);
            EnumCompareOperator ePOAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[6]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[7]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[8]);
            int nApprovedBy = Convert.ToInt32(sSearchingData.Split('~')[9]);
            string sSupplierIDs = Convert.ToString(sSearchingData.Split('~')[10]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[11]);

            string sReturn1 = "SELECT * FROM View_PurchaseOrder";
            string sReturn = "";

            #region BUID
            if (nBUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value))==true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID.ToString();
            }
            #endregion

            #region PONo
            if (sPONo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PONo LIKE '%" + sPONo + "%'";
            }
            #endregion

            #region Ref Type
            if (ePOReferenceType !=  EnumPOReferenceType.None)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefType = " + ((int)ePOReferenceType).ToString();
            }
            #endregion

            #region RefNo
            if (sRefNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE '%" + sRefNo + "%'";
            }
            #endregion

            #region PO Date
            if (ePODate != EnumCompareOperator.None)
            {
                if (ePODate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePODate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePODate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePODate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePODate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (ePODate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),PODate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region PO Amount
            if (ePOAmount != EnumCompareOperator.None)
            {
                if (ePOAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount = " + nStartAmount.ToString("0.00");
                }
                else if (ePOAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount != " + nStartAmount.ToString("0.00");
                }
                else if (ePOAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount < " + nStartAmount.ToString("0.00");
                }
                else if (ePOAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount > " + nStartAmount.ToString("0.00");
                }
                else if (ePOAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " Amount BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (ePOAmount == EnumCompareOperator.NotBetween)
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
                sReturn = sReturn + " ISNULL(ApproveBy,0) = " + nApprovedBy.ToString();
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
                sReturn = sReturn + " POID IN (SELECT TT.POID FROM PurchaseOrderDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Printing
        public ActionResult PrintoPurchaseOrder(int id)
        {
            _oPurchaseOrder = new PurchaseOrder();
            PaymentTerm oPaymentTerm = new PaymentTerm();
            _oClientOperationSetting = new ClientOperationSetting();
            _oPurchaseOrder = _oPurchaseOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseOrder.PurchaseOrderDetails = PurchaseOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseOrder.POTandCClauses = POTandCClause.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oPaymentTerm = oPaymentTerm.Get(_oPurchaseOrder.PaymentTermID, (int)Session[SessionInfo.currentUserID]);
            _oPurchaseOrder.PaymentTermText = oPaymentTerm.FullTerm;
            List<POSpec> oPOSpecs = new List<POSpec>();
            if (_oPurchaseOrder.PurchaseOrderDetails.Any())
            {
                string sSql = "SELECT * FROM View_POSpec Where PODetailID IN ( " +(string.Join(",",_oPurchaseOrder.PurchaseOrderDetails.Select(x=>x.PODetailID)))+")";
                oPOSpecs = POSpec.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            Contractor oContractor = new Contractor();
            Contractor oDeliveryTo = new Contractor();
            oContractor = oContractor.Get(_oPurchaseOrder.ContractorID, (int)Session[SessionInfo.currentUserID]);
            oDeliveryTo = oDeliveryTo.Get(_oPurchaseOrder.DeliveryTo, (int)Session[SessionInfo.currentUserID]);
            
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPurchaseOrder.BUID, (int)Session[SessionInfo.currentUserID]);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.PurchaseOrderPreview, (int)Session[SessionInfo.currentUserID]);
            
            rptSupplierOrder oReport = new rptSupplierOrder();
            byte[] abytes = oReport.PrepareReport(_oPurchaseOrder, oCompany, oContractor, oBusinessUnit, oSignatureSetups);
            return File(abytes, "application/pdf");
          
        }
        public ActionResult PrintPurchaseOrders(string ids, double ts)
        {
            _oPurchaseOrders = new List<PurchaseOrder>();
            string sSql = "SELECT * FROM View_PurchaseOrder WHERE POID IN (" + ids + ") ORDER BY POID";
            _oPurchaseOrders = PurchaseOrder.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptSupplierOrders oReport = new rptSupplierOrders();
            byte[] abytes = oReport.PrepareReport(_oPurchaseOrders, oCompany);
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
        public JsonResult ProductSpecHeadFORPOByProduct(POSpec oPOSpec)
        {
            ProductSpecHead _oProductSpecHead = new ProductSpecHead();
            List<ProductSpecHead> _oSProductSpecHeads = new List<ProductSpecHead>();
            POSpec _oPQSpec = new POSpec();
            List<POSpec> oPQSpecs = new List<POSpec>();
            string sSQL = string.Empty;
            try
            {
                sSQL = "Select * from View_POSpec Where PODetailID =" + oPOSpec.PODetailID;
                oPQSpecs = POSpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from View_ProductSpecHead Where ProductID =" + oPOSpec.ProductID + "Order By Sequence";
                _oSProductSpecHeads = ProductSpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oSProductSpecHeads.Any())
                {
                    if (oPQSpecs.Any())
                    {
                        _oSProductSpecHeads.RemoveAll(x => oPQSpecs.Select(p => p.SpecHeadID).Contains(x.SpecHeadID));

                    }
                    if (_oSProductSpecHeads.Any())
                    {
                        foreach (var oitem in _oSProductSpecHeads)
                        {
                            _oPQSpec = new POSpec();
                            _oPQSpec.SpecName = oitem.SpecName;
                            _oPQSpec.POSpecID = 0;
                            _oPQSpec.SpecHeadID = oitem.SpecHeadID;
                            _oPQSpec.PODescription = string.Empty;
                            _oPQSpec.PODetailID = oPOSpec.PODetailID;
                            oPQSpecs.Add(_oPQSpec);

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

            var jsonResult = Json(oPQSpecs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult IUDPOSpec(POSpec oPOSpec)
        {
            try
            {
                oPOSpec = oPOSpec.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPOSpec = new POSpec();
                oPOSpec.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPOSpec);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
