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

namespace ESimSol.Controllers
{
    public class TradingSaleInvoiceController : Controller
    {
        #region Declaration
        TradingSaleInvoice _oTradingSaleInvoice = new TradingSaleInvoice();
        List<TradingSaleInvoice> _oTradingSaleInvoices = new List<TradingSaleInvoice>();
        #endregion

        #region Actions
        public ActionResult ViewTradingSaleInvoices(int buid, int menuid)
        {
            ViewBag.BUID = buid;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TradingSaleInvoice).ToString() + "," + ((int)EnumModuleName.TradingDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oTradingSaleInvoices = new List<TradingSaleInvoice>();
            _oTradingSaleInvoices = TradingSaleInvoice.GetsInitialInvoices(buid, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.ApprovedBy FROM TradingSaleInvoice AS MM WHERE ISNULL(MM.ApprovedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<BusinessObjects.User>();
            BusinessObjects.User oApprovalUser = new BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));

            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.SalesTypeObjs = SalesTypeObj.Gets();
            return View(_oTradingSaleInvoices);
        }

        public ActionResult ViewTradingSaleInvoice(int id, int sOID, double ts)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            TradingSaleOrder oTradingSaleOrder = new TradingSaleOrder();
            if (id == 0 && sOID > 0)
            {
                oTradingSaleOrder = oTradingSaleOrder.Get(sOID, (int)Session[SessionInfo.currentUserID]);
                oTradingSaleOrder.TradingSaleOrderDetails = TradingSaleOrderDetail.GetsForInvoice(sOID, (int)Session[SessionInfo.currentUserID]);
                Contractor oContractor = new Contractor();
                oTradingSaleOrder.Contractor = oContractor.Get(oTradingSaleOrder.ContractorID, (int)Session[SessionInfo.currentUserID]);
                //CommissionSetup oCommissionSetup = new CommissionSetup();
                //oTradingSaleOrder.CommissionSetup = oCommissionSetup.GetByContractor(oTradingSaleOrder.ContractorID, (int)Session[SessionInfo.currentUserID]);

                //double commissionInPercent = oTradingSaleOrder.CommissionSetup.CommissionInPercent;

                _oTradingSaleInvoice.TradingSaleInvoiceID = 0;
                _oTradingSaleInvoice.BUID = oTradingSaleOrder.BUID;
                _oTradingSaleInvoice.SalesType = EnumSalesType.None;
                _oTradingSaleInvoice.SalesTypeInt = 0;
                _oTradingSaleInvoice.InvoiceNo = "";
                _oTradingSaleInvoice.InvoiceDate = oTradingSaleOrder.OrderCreateDate; //DateTime.Now
                _oTradingSaleInvoice.BuyerID = oTradingSaleOrder.ContractorID;
                _oTradingSaleInvoice.ContactPersonID = oTradingSaleOrder.ContractorPersonalID;
                _oTradingSaleInvoice.CurrencyID = 0;
                _oTradingSaleInvoice.Note = "";
                _oTradingSaleInvoice.ApprovedBy = 0;
                _oTradingSaleInvoice.GrossAmount = oTradingSaleOrder.Amount;
                _oTradingSaleInvoice.DiscountAmount = 0;
                _oTradingSaleInvoice.VatAmount = 0;
                _oTradingSaleInvoice.ServiceCharge = 0;
                //_oTradingSaleInvoice.CommissionAmount = (commissionInPercent > 0) ? (oTradingSaleOrder.Amount * commissionInPercent / 100) : 0;
                _oTradingSaleInvoice.NetAmount = oTradingSaleOrder.Amount - _oTradingSaleInvoice.CommissionAmount;
                _oTradingSaleInvoice.BUName = "";
                _oTradingSaleInvoice.BuyerName = oTradingSaleOrder.ContractorName;
                _oTradingSaleInvoice.BuyerAddress = "";
                _oTradingSaleInvoice.BuyerPhone = "";
                _oTradingSaleInvoice.ContactPersonName = "";
                _oTradingSaleInvoice.ContactPersonPhone = oTradingSaleOrder.ContractPersonName;
                _oTradingSaleInvoice.CurrencyName = "";
                _oTradingSaleInvoice.CurrencySymbol = "";
                _oTradingSaleInvoice.ApprovedByName = "";
                _oTradingSaleInvoice.YetToChallanQty = 0;
                _oTradingSaleInvoice.DueAmount = 0;
                _oTradingSaleInvoice.CardNo = "";
                _oTradingSaleInvoice.CardPaid = 0;
                _oTradingSaleInvoice.CashPaid = 0;
                _oTradingSaleInvoice.ATMCardID = 0;
                _oTradingSaleInvoice.ErrorMessage = "";

                List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
                foreach (TradingSaleOrderDetail oItem in oTradingSaleOrder.TradingSaleOrderDetails)
                {
                    TradingSaleInvoiceDetail oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
                    oTradingSaleInvoiceDetail.TradingSaleInvoiceDetailID = 0;
                    oTradingSaleInvoiceDetail.TradingSaleInvoiceID = 0;
                    oTradingSaleInvoiceDetail.ProductID = oItem.ProductID;
                    oTradingSaleInvoiceDetail.ItemDescription = "";
                    oTradingSaleInvoiceDetail.MeasurementUnitID = oItem.MeasurementUnitID;
                    oTradingSaleInvoiceDetail.InvoiceQty = oItem.OrderQty;
                    oTradingSaleInvoiceDetail.UnitPrice = oItem.UnitPrice;
                    oTradingSaleInvoiceDetail.Amount = oItem.UnitPrice * oItem.OrderQty;
                    oTradingSaleInvoiceDetail.Discount = 0;
                    oTradingSaleInvoiceDetail.NetAmount = oItem.UnitPrice * oItem.OrderQty;
                    oTradingSaleInvoiceDetail.ProductCode = oItem.ProductCode;
                    oTradingSaleInvoiceDetail.ProductName = oItem.ProductName;
                    oTradingSaleInvoiceDetail.UnitName = oItem.MeasurementUnitName;
                    oTradingSaleInvoiceDetail.Symbol = "";
                    oTradingSaleInvoiceDetail.ProductCategoryName = "";
                    oTradingSaleInvoiceDetail.ErrorMessage = "";
                    oTradingSaleInvoiceDetail.YetToChallanQty = 0;
                    oTradingSaleInvoiceDetail.PurchaseAmount = 0;
                    oTradingSaleInvoiceDetail.ReceivedQty = 0;
                    oTradingSaleInvoiceDetail.CurrentStock = 0;

                    oTradingSaleInvoiceDetail.LotNo = "";

                    oTradingSaleInvoiceDetails.Add(oTradingSaleInvoiceDetail);
                }
                _oTradingSaleInvoice.TradingSaleInvoiceDetails = oTradingSaleInvoiceDetails;
            }
            else
            {
                _oTradingSaleInvoice = _oTradingSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oTradingSaleInvoice.SalesTypeObjs = SalesTypeObj.Gets();
            _oTradingSaleInvoice.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oTradingSaleInvoice);

            //_oTradingSaleInvoice = new TradingSaleInvoice();
            //if (id > 0)
            //{
            //    _oTradingSaleInvoice = _oTradingSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            //    _oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            //}
            //_oTradingSaleInvoice.SalesTypeObjs = SalesTypeObj.Gets();
            //_oTradingSaleInvoice.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);                        
            //return View(_oTradingSaleInvoice);
        }


        public ActionResult ViewTradingSaleInvoiceRevise(int id, int sOID, double ts)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            TradingSaleOrder oTradingSaleOrder = new TradingSaleOrder();
            if (id == 0 && sOID > 0)
            {
                oTradingSaleOrder = oTradingSaleOrder.Get(sOID, (int)Session[SessionInfo.currentUserID]);
                oTradingSaleOrder.TradingSaleOrderDetails = TradingSaleOrderDetail.GetsForInvoice(sOID, (int)Session[SessionInfo.currentUserID]);
                Contractor oContractor = new Contractor();
                oTradingSaleOrder.Contractor = oContractor.Get(oTradingSaleOrder.ContractorID, (int)Session[SessionInfo.currentUserID]);
                //CommissionSetup oCommissionSetup = new CommissionSetup();
                //oTradingSaleOrder.CommissionSetup = oCommissionSetup.GetByContractor(oTradingSaleOrder.ContractorID, (int)Session[SessionInfo.currentUserID]);

                //double commissionInPercent = oTradingSaleOrder.CommissionSetup.CommissionInPercent;

                _oTradingSaleInvoice.TradingSaleInvoiceID = 0;
                _oTradingSaleInvoice.BUID = oTradingSaleOrder.BUID;
                _oTradingSaleInvoice.SalesType = EnumSalesType.None;
                _oTradingSaleInvoice.SalesTypeInt = 0;
                _oTradingSaleInvoice.InvoiceNo = "";
                _oTradingSaleInvoice.InvoiceDate = oTradingSaleOrder.OrderCreateDate; //DateTime.Now
                _oTradingSaleInvoice.BuyerID = oTradingSaleOrder.ContractorID;
                _oTradingSaleInvoice.ContactPersonID = oTradingSaleOrder.ContractorPersonalID;
                _oTradingSaleInvoice.CurrencyID = 0;
                _oTradingSaleInvoice.Note = "";
                _oTradingSaleInvoice.ApprovedBy = 0;
                _oTradingSaleInvoice.GrossAmount = oTradingSaleOrder.Amount;
                _oTradingSaleInvoice.DiscountAmount = 0;
                _oTradingSaleInvoice.VatAmount = 0;
                _oTradingSaleInvoice.ServiceCharge = 0;
                //_oTradingSaleInvoice.CommissionAmount = (commissionInPercent > 0) ? (oTradingSaleOrder.Amount * commissionInPercent / 100) : 0;
                _oTradingSaleInvoice.NetAmount = oTradingSaleOrder.Amount - _oTradingSaleInvoice.CommissionAmount;
                _oTradingSaleInvoice.BUName = "";
                _oTradingSaleInvoice.BuyerName = oTradingSaleOrder.ContractorName;
                _oTradingSaleInvoice.BuyerAddress = "";
                _oTradingSaleInvoice.BuyerPhone = "";
                _oTradingSaleInvoice.ContactPersonName = "";
                _oTradingSaleInvoice.ContactPersonPhone = oTradingSaleOrder.ContractPersonName;
                _oTradingSaleInvoice.CurrencyName = "";
                _oTradingSaleInvoice.CurrencySymbol = "";
                _oTradingSaleInvoice.ApprovedByName = "";
                _oTradingSaleInvoice.YetToChallanQty = 0;
                _oTradingSaleInvoice.DueAmount = 0;
                _oTradingSaleInvoice.CardNo = "";
                _oTradingSaleInvoice.CardPaid = 0;
                _oTradingSaleInvoice.CashPaid = 0;
                _oTradingSaleInvoice.ATMCardID = 0;
                _oTradingSaleInvoice.ErrorMessage = "";

                List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
                foreach (TradingSaleOrderDetail oItem in oTradingSaleOrder.TradingSaleOrderDetails)
                {
                    TradingSaleInvoiceDetail oTradingSaleInvoiceDetail = new TradingSaleInvoiceDetail();
                    oTradingSaleInvoiceDetail.TradingSaleInvoiceDetailID = 0;
                    oTradingSaleInvoiceDetail.TradingSaleInvoiceID = 0;
                    oTradingSaleInvoiceDetail.ProductID = oItem.ProductID;
                    oTradingSaleInvoiceDetail.ItemDescription = "";
                    oTradingSaleInvoiceDetail.MeasurementUnitID = oItem.MeasurementUnitID;
                    oTradingSaleInvoiceDetail.InvoiceQty = oItem.OrderQty;
                    oTradingSaleInvoiceDetail.UnitPrice = oItem.UnitPrice;
                    oTradingSaleInvoiceDetail.Amount = oItem.UnitPrice * oItem.OrderQty;
                    oTradingSaleInvoiceDetail.Discount = 0;
                    oTradingSaleInvoiceDetail.NetAmount = oItem.UnitPrice * oItem.OrderQty;
                    oTradingSaleInvoiceDetail.ProductCode = oItem.ProductCode;
                    oTradingSaleInvoiceDetail.ProductName = oItem.ProductName;
                    oTradingSaleInvoiceDetail.UnitName = oItem.MeasurementUnitName;
                    oTradingSaleInvoiceDetail.Symbol = "";
                    oTradingSaleInvoiceDetail.ProductCategoryName = "";
                    oTradingSaleInvoiceDetail.ErrorMessage = "";
                    oTradingSaleInvoiceDetail.YetToChallanQty = 0;
                    oTradingSaleInvoiceDetail.PurchaseAmount = 0;
                    oTradingSaleInvoiceDetail.ReceivedQty = 0;
                    oTradingSaleInvoiceDetail.CurrentStock = 0;

                    oTradingSaleInvoiceDetail.LotNo = "";

                    oTradingSaleInvoiceDetails.Add(oTradingSaleInvoiceDetail);
                }
                _oTradingSaleInvoice.TradingSaleInvoiceDetails = oTradingSaleInvoiceDetails;
            }
            else
            {
                _oTradingSaleInvoice = _oTradingSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oTradingSaleInvoice.SalesTypeObjs = SalesTypeObj.Gets();
            _oTradingSaleInvoice.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oTradingSaleInvoice);

            //_oTradingSaleInvoice = new TradingSaleInvoice();
            //if (id > 0)
            //{
            //    _oTradingSaleInvoice = _oTradingSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            //    _oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            //}
            //_oTradingSaleInvoice.SalesTypeObjs = SalesTypeObj.Gets();
            //_oTradingSaleInvoice.Currencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);                        
            //return View(_oTradingSaleInvoice);
        }

        public ActionResult ViewPosSale(int buid, int menuid)
        {
            ViewBag.BUID = buid;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TradingSaleInvoice).ToString() + "," + ((int)EnumModuleName.GRN).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oTradingSaleInvoice = new TradingSaleInvoice();


            Company oCompany = new Company();
            oCompany = (oCompany.Get(1, (int)Session[SessionInfo.currentUserID]));

            _oTradingSaleInvoice.CurrencyID = oCompany.BaseCurrencyID;
            _oTradingSaleInvoice.CurrencySymbol = oCompany.CurrencySymbol;

            BusinessUnit oBU = new BusinessUnit();
            oBU = oBU.Get(buid, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "Select * from View_SalesmanBusinessUnit Where SalesmanID In (Select SalesmanID from SalesMan Where UserID=" + (int)Session[SessionInfo.currentUserID] + ") And BUID=" + oBU.BusinessUnitID + "";
            //var oSalesmanBusinessUnits = SalesmanBusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //double nSalesAmount = 0;
            //int nSalesmanID = 0;
            //if (oSalesmanBusinessUnits.Any() && oSalesmanBusinessUnits.FirstOrDefault().SalesmanBUID > 0)
            //{
            //    nSalesmanID = oSalesmanBusinessUnits.FirstOrDefault().SalesmanID;
            //    _oTradingSaleInvoice.SalesmanName = oSalesmanBusinessUnits.FirstOrDefault().SalesmanName;
            //    nSalesAmount = oSalesmanBusinessUnits.FirstOrDefault().Balance;
            //}
            sSQL = "Select * from View_ATMCard";
            //ViewBag.ATMCards = ATMCard.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //ViewBag.SalesmanID = nSalesmanID;
            //ViewBag.SalesAmount = nSalesAmount;
            ViewBag.BusinessUnit = oBU;
            //ViewBag.DateCompareOperatorObjs = CompareOperatorObj.Gets();

            return View(_oTradingSaleInvoice);
        }
        #region Revise Request
        public JsonResult RequestRevise(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            _oTradingSaleInvoice = oTradingSaleInvoice;
            try
            {

                _oTradingSaleInvoice.ReviseRequest.RequestBy = ((int)Session[SessionInfo.currentUserID]);
                _oTradingSaleInvoice.ReviseRequest.OperationType = EnumReviseRequestOperationType.OrderSheet;

                _oTradingSaleInvoice.TradingSaleInvoiceStatus = EnumTradingSaleInvoiceStatus.Request_For_Revise;
                _oTradingSaleInvoice = _oTradingSaleInvoice.RequestRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public JsonResult AcceptRevise(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            try
            {
                _oTradingSaleInvoice = oTradingSaleInvoice;
                _oTradingSaleInvoice = _oTradingSaleInvoice.AcceptRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetReviseHistory(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoices = new List<TradingSaleInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_TradingSaleInvoiceLog WHERE TradingSaleInvoiceID = " + oTradingSaleInvoice.TradingSaleInvoiceID;
                _oTradingSaleInvoices = TradingSaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
                _oTradingSaleInvoices.Add(_oTradingSaleInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region HTTP UndoApproved
        [HttpPost]
        public JsonResult UndoApproved(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            try
            {

                _oTradingSaleInvoice = oTradingSaleInvoice;
                _oTradingSaleInvoice.SalesType = (EnumSalesType)oTradingSaleInvoice.SalesTypeInt;


                _oTradingSaleInvoice = _oTradingSaleInvoice.UndoApproved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public JsonResult Save(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            try
            {
                _oTradingSaleInvoice = oTradingSaleInvoice;
                _oTradingSaleInvoice.SalesTypeInt = (int)EnumSalesType.CreditSale;
                _oTradingSaleInvoice = _oTradingSaleInvoice.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(TradingSaleInvoice oTradingSaleInvoice)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oTradingSaleInvoice.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approved(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            try
            {
                _oTradingSaleInvoice = oTradingSaleInvoice;
                _oTradingSaleInvoice.SalesType = (EnumSalesType)oTradingSaleInvoice.SalesTypeInt;
                _oTradingSaleInvoice = _oTradingSaleInvoice.Approved((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SavePosSale(TradingSaleInvoice oTradingSaleInvoice)
        {
            Contractor oContractor = new Contractor();
            _oTradingSaleInvoice = new TradingSaleInvoice();
            try
            {
                oContractor.ContractorID = oTradingSaleInvoice.BuyerID;
                oContractor.Name = oTradingSaleInvoice.BuyerName;
                oContractor.Address = oTradingSaleInvoice.BuyerAddress;
                oContractor.Phone = oTradingSaleInvoice.BuyerPhone;
                if (oContractor.ContractorID > 0)
                {
                    //oContractor = oContractor.UpdateContactInfo((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    BusinessUnit oBusinessUnit = new BusinessUnit();
                    ContractorType oContractorType = new ContractorType();
                    List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                    List<ContractorType> oContractorTypes = new List<ContractorType>();

                    oBusinessUnit = new BusinessUnit();
                    oBusinessUnits = new List<BusinessUnit>();
                    oBusinessUnit.BusinessUnitID = oTradingSaleInvoice.BUID;
                    oBusinessUnits.Add(oBusinessUnit);

                    oContractorType = new ContractorType();
                    oContractorTypes = new List<ContractorType>();
                    oContractorType.ContractorTypeID = (int)EnumContractorType.Buyer;
                    oContractorType.ContractorID = oTradingSaleInvoice.BuyerID;
                    oContractorTypes.Add(oContractorType);


                    oContractor.BusinessUnits = oBusinessUnits;
                    oContractor.ContractorTypes = oContractorTypes;
                    oContractor = oContractor.Save((int)Session[SessionInfo.currentUserID]);
                }

                if (oContractor.ContractorID > 0)
                {
                    _oTradingSaleInvoice = oTradingSaleInvoice;
                    _oTradingSaleInvoice.SalesTypeInt = (int)EnumSalesType.CashSale;
                    _oTradingSaleInvoice.BuyerID = oContractor.ContractorID;
                    _oTradingSaleInvoice = _oTradingSaleInvoice.Save((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    if (oContractor.ErrorMessage != "")
                    {
                        _oTradingSaleInvoice = new TradingSaleInvoice();
                        _oTradingSaleInvoice.ErrorMessage = oContractor.ErrorMessage;
                    }
                    else
                    {
                        _oTradingSaleInvoice = new TradingSaleInvoice();
                        _oTradingSaleInvoice.ErrorMessage = "Invalid Customer!";
                    }
                }
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult PrintTradingSaleInvoice(int id, double ts)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            _oTradingSaleInvoice = _oTradingSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<TradingSaleInvoice> oTradingSaleInvoices = new List<TradingSaleInvoice>();
            string sSQL = "SELECT * FROM View_TradingSaleInvoice AS HH WHERE HH.BUID=" + _oTradingSaleInvoice.BUID.ToString() + " AND HH.BuyerID=" + _oTradingSaleInvoice.BuyerID.ToString() + " AND HH.DueAmount>0 AND HH.TradingSaleInvoiceID<(SELECT MM.TradingSaleInvoiceID FROM TradingSaleInvoice AS MM WHERE MM.TradingSaleInvoiceID=" + _oTradingSaleInvoice.TradingSaleInvoiceID.ToString() + ") ORDER BY HH.TradingSaleInvoiceID ASC";
            oTradingSaleInvoices = TradingSaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            rptTradingSaleInvoice oReport = new rptTradingSaleInvoice();
            byte[] abytes = oReport.PrepareReport(_oTradingSaleInvoice, oCompany, oTradingSaleInvoices);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintTradingSaleInvoiceLog(int id, double ts)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            _oTradingSaleInvoice = _oTradingSaleInvoice.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.GetsLog(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<TradingSaleInvoice> oTradingSaleInvoices = new List<TradingSaleInvoice>();

            rptTradingSaleInvoice oReport = new rptTradingSaleInvoice();
            byte[] abytes = oReport.PrepareReport(_oTradingSaleInvoice, oCompany, oTradingSaleInvoices);
            return File(abytes, "application/pdf");

        }


        public ActionResult POSPrint(int id, double ts)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            _oTradingSaleInvoice = _oTradingSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptPrintPOS oReport = new rptPrintPOS();
            byte[] abytes = oReport.PrepareReport(_oTradingSaleInvoice, oCompany);
            return File(abytes, "application/pdf");

        }

        public ActionResult ProfitLoss(int id, double ts)
        {
            _oTradingSaleInvoice = new TradingSaleInvoice();
            _oTradingSaleInvoice = _oTradingSaleInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptTradingInvoiceWiseProfitLoss oReport = new rptTradingInvoiceWiseProfitLoss();
            byte[] abytes = oReport.PrepareReport(_oTradingSaleInvoice, oCompany);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintTradingSaleInvoices(string ids, double ts)
        {
            _oTradingSaleInvoices = new List<TradingSaleInvoice>();
            string sSql = "SELECT * FROM View_TradingSaleInvoice WHERE TradingSaleInvoiceID IN (" + ids + ") ORDER BY TradingSaleInvoiceID";
            _oTradingSaleInvoices = TradingSaleInvoice.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptTradingSaleInvoices oReport = new rptTradingSaleInvoices();
            byte[] abytes = oReport.PrepareReport(_oTradingSaleInvoices, oCompany);
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

        #region Advance Search
        [HttpPost]
        public JsonResult GetsByInvoiceNo(TradingSaleInvoice oTradingSaleInvoice)
        {
            List<TradingSaleInvoice> oTradingSaleInvoices = new List<TradingSaleInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_TradingSaleInvoice AS HH WHERE HH.BUID=" + oTradingSaleInvoice.BUID.ToString() + " AND (HH.InvoiceNo+HH.BuyerName) LIKE '%" + oTradingSaleInvoice.InvoiceNo + "%' ORDER BY TradingSaleInvoiceID ASC";
                oTradingSaleInvoices = TradingSaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTradingSaleInvoices = new List<TradingSaleInvoice>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTradingSaleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult WaitForApproval(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoices = new List<TradingSaleInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_TradingSaleInvoice WHERE BUID = " + oTradingSaleInvoice.BUID.ToString() + " AND ISNULL(ApprovedBy,0)=0 ORDER BY TradingSaleInvoiceID ASC";
                _oTradingSaleInvoices = TradingSaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingSaleInvoices = new List<TradingSaleInvoice>();
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
                _oTradingSaleInvoices.Add(_oTradingSaleInvoice);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult WaitForChallan(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoices = new List<TradingSaleInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_TradingSaleInvoice WHERE BUID = " + oTradingSaleInvoice.BUID.ToString() + " AND ISNULL(ApprovedBy,0)!=0 AND YetToChallanQty>0 ORDER BY TradingSaleInvoiceID ASC";
                _oTradingSaleInvoices = TradingSaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingSaleInvoices = new List<TradingSaleInvoice>();
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
                _oTradingSaleInvoices.Add(_oTradingSaleInvoice);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoices = new List<TradingSaleInvoice>();
            try
            {
                string sSQL = this.GetSQL(oTradingSaleInvoice.Note, oTradingSaleInvoice.BUID);
                _oTradingSaleInvoices = TradingSaleInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingSaleInvoices = new List<TradingSaleInvoice>();
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
                _oTradingSaleInvoices.Add(_oTradingSaleInvoice);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sSearchingData, int nBUID)
        {
            int nSalesType = Convert.ToInt32(sSearchingData.Split('~')[0]);
            string sInvoiceNo = Convert.ToString(sSearchingData.Split('~')[1]);
            EnumCompareOperator eInvoiceDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[2]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[3]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            EnumCompareOperator eInvoiceAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[5]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[6]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[7]);
            int nApprovedBy = Convert.ToInt32(sSearchingData.Split('~')[8]);
            string sSupplierIDs = Convert.ToString(sSearchingData.Split('~')[9]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_TradingSaleInvoice";
            string sReturn = "";

            #region BUID
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID.ToString();
            }
            #endregion

            #region SalesType
            if (nSalesType != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SalesType =" + nSalesType.ToString();
            }
            #endregion

            #region InvoiceNo
            if (sInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " InvoiceNo LIKE '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region Invoice Date
            if (eInvoiceDate != EnumCompareOperator.None)
            {
                if (eInvoiceDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region InvoiceAmount
            if (eInvoiceAmount != EnumCompareOperator.None)
            {
                if (eInvoiceAmount == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetAmount = " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetAmount != " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetAmount < " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetAmount > " + nStartAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetAmount BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
                }
                else if (eInvoiceAmount == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NetAmount NOT BETWEEN " + nStartAmount.ToString("0.00") + " AND " + nEndAmount.ToString("0.00");
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

            #region SupplierIDs
            if (sSupplierIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sSupplierIDs + ")";
            }
            #endregion

            #region Products
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TradingSaleInvoiceID IN (SELECT TT.TradingSaleInvoiceID FROM TradingSaleInvoiceDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }


        [HttpPost]
        public JsonResult GetsCashTradingSaleInvoice(TradingSaleInvoice oTradingSaleInvoice)
        {
            _oTradingSaleInvoices = new List<TradingSaleInvoice>();
            try
            {
                string sInvoiceNo = Convert.ToString(oTradingSaleInvoice.Note.Split('~')[0]);
                EnumCompareOperator eInvoiceDate = (EnumCompareOperator)Convert.ToInt32(oTradingSaleInvoice.Note.Split('~')[1]);
                DateTime dStartDate = Convert.ToDateTime(oTradingSaleInvoice.Note.Split('~')[2]);
                DateTime dEndDate = Convert.ToDateTime(oTradingSaleInvoice.Note.Split('~')[3]);
                string sBuyerIDs = Convert.ToString(oTradingSaleInvoice.Note.Split('~')[4]);

                string sReturn1 = "SELECT * FROM View_TradingSaleInvoice ";
                string sReturn = " Where ApprovedBy>0 And SalesType=" + (int)EnumSalesType.CashSale;

                #region BUID
                if (oTradingSaleInvoice.BUID != 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BUID = " + oTradingSaleInvoice.BUID.ToString();
                }
                #endregion

                #region InvoiceNo
                if (sInvoiceNo != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " InvoiceNo LIKE '%" + sInvoiceNo + "%'";
                }
                #endregion

                #region Invoice Date
                if (eInvoiceDate != EnumCompareOperator.None)
                {
                    if (eInvoiceDate == EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                    }
                    else if (eInvoiceDate == EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                    }
                    else if (eInvoiceDate == EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                    }
                    else if (eInvoiceDate == EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                    }
                    else if (eInvoiceDate == EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                    }
                    else if (eInvoiceDate == EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                    }
                }
                #endregion

                #region BuyerIDs
                if (sBuyerIDs != "")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
                }
                #endregion

                sReturn = sReturn1 + sReturn;
                _oTradingSaleInvoices = TradingSaleInvoice.Gets(sReturn, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingSaleInvoices = new List<TradingSaleInvoice>();
                _oTradingSaleInvoice.ErrorMessage = ex.Message;
                _oTradingSaleInvoices.Add(_oTradingSaleInvoice);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingSaleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetTradingSaleInvoice(TradingSaleInvoice oTradingSaleInvoice)
        {
            try
            {
                oTradingSaleInvoice = oTradingSaleInvoice.Get(oTradingSaleInvoice.TradingSaleInvoiceID, (int)Session[SessionInfo.currentUserID]);
                if (oTradingSaleInvoice.TradingSaleInvoiceID > 0)
                {
                    oTradingSaleInvoice.TradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(oTradingSaleInvoice.TradingSaleInvoiceID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oTradingSaleInvoice = new TradingSaleInvoice();
                oTradingSaleInvoice.TradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
                oTradingSaleInvoice.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTradingSaleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PrintExcel
        public ActionResult ViewTradingSaleInvoiceRegister(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            TradingSaleInvoiceRegister oTradingSaleInvoiceRegister = new TradingSaleInvoiceRegister();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWise_PaymentBased || (EnumReportLayout)oItem.id == EnumReportLayout.DateWise_ProductBased || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise_PaymentBased || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise_ProductBased || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise_ProductBased)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            return View(oTradingSaleInvoiceRegister);
        }

        public ActionResult SetSessionSearchCriteria(TradingSaleInvoiceRegister oTradingSaleInvoiceRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oTradingSaleInvoiceRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public void ExcelTradingSaleInvoiceRegister(double ts)
        {
            TradingSaleInvoiceRegister oTradingSaleInvoiceRegister = new TradingSaleInvoiceRegister();
            List<TradingSaleInvoiceRegister> _oTradingSaleInvoiceRegisters = new List<TradingSaleInvoiceRegister>();

            try
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oTradingSaleInvoiceRegister = (TradingSaleInvoiceRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetTradingSaleInvoiceSQL(oTradingSaleInvoiceRegister);
                _oTradingSaleInvoiceRegisters = TradingSaleInvoiceRegister.Gets(sSQL, (int)oTradingSaleInvoiceRegister.ReportLayout);
                if ((EnumReportLayout)oTradingSaleInvoiceRegister.ReportLayout == EnumReportLayout.DateWise_PaymentBased)
                {
                    DateWisePayemntBasedXL(_oTradingSaleInvoiceRegisters, oCompany);
                }
                else if ((EnumReportLayout)oTradingSaleInvoiceRegister.ReportLayout == EnumReportLayout.PartyWise_PaymentBased)
                {
                    PartyWisePayemntBasedXL(_oTradingSaleInvoiceRegisters, oCompany);
                }
                else if ((EnumReportLayout)oTradingSaleInvoiceRegister.ReportLayout == EnumReportLayout.DateWise_ProductBased)
                {
                    DateWiseProductBasedXL(_oTradingSaleInvoiceRegisters, oCompany);
                }
                else if ((EnumReportLayout)oTradingSaleInvoiceRegister.ReportLayout == EnumReportLayout.PartyWise_ProductBased)
                {
                    PartyWiseProductBasedXL(_oTradingSaleInvoiceRegisters, oCompany);
                }
                else if ((EnumReportLayout)oTradingSaleInvoiceRegister.ReportLayout == EnumReportLayout.ProductWise_ProductBased)
                {
                    ProductWiseProductBasedXL(_oTradingSaleInvoiceRegisters, oCompany);
                }

            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sales & Marketing");
                    sheet.Name = "Sales & Marketing";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Buyer-MonthWiseSalesReport.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void DateWiseProductBasedXL(List<TradingSaleInvoiceRegister> oTradingSaleInvoiceRegisters, Company oCompany)
        {
            #region Report Body
            int rowIndex = 2;
            int nMaxColumn;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            float nFontSize = 10f;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Trading Sale Invoice Register");
                sheet.Name = "Trading Sale Invoice Register";
                sheet.View.FreezePanes(6, 6);
                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                #region Coloums
                sheet.Column(colIndex++).Width = 10;//SL
                sheet.Column(colIndex++).Width = 20;//Invoice No
                sheet.Column(colIndex++).Width = 15;//Invoice Date
                sheet.Column(colIndex++).Width = 15;//Sales Type
                sheet.Column(colIndex++).Width = 15;//Sales By
                sheet.Column(colIndex++).Width = 18;//Product name
                sheet.Column(colIndex++).Width = 18;//Product Code
                sheet.Column(colIndex++).Width = 15;//MUnit
                sheet.Column(colIndex++).Width = 15;//UnitPrice
                sheet.Column(colIndex++).Width = 15;//Quantity
                sheet.Column(colIndex++).Width = 15;//Amount
                sheet.Column(colIndex++).Width = 15;//ItemWise Discount
                sheet.Column(colIndex++).Width = 15;//Gross Amount
                sheet.Column(colIndex++).Width = 15;//Discount Amount
                sheet.Column(colIndex++).Width = 15;//Vat Amount
                sheet.Column(colIndex++).Width = 15;//Service Charge
                sheet.Column(colIndex++).Width = 15;//Net Amount
                sheet.Column(colIndex++).Width = 15;//Received Amount
                sheet.Column(colIndex++).Width = 15;//Due Amount
                sheet.Column(colIndex++).Width = 20;//Purchase Price
                sheet.Column(colIndex++).Width = 20;//Profit/Loss

                nMaxColumn = colIndex - 1;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Cash Sales and Received Register Date Wise(Product based)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion


                #region Column Header
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "S.L No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Invoice No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "CustomerName"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Sales Type"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Salse By"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                int nSubColIndex = colIndex;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, (colIndex + 7)]; cell.Merge = true; cell.Value = "Item Description"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex = (colIndex + 7) + 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Gross Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Discount Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Vat Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Service Charge"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Net Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Received Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Due Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Purchase Price"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Profit/Loss"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                colIndex = nSubColIndex;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Product name"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "M.Unit"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Quantity"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Discount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                #endregion
                int nCount = 0;
                string sPreviousInvoiceDate = ""; int nPrevInvoiceID = 0;
                string sStartCell = "", sEndCell = "";
                int nStartCol = 0, nEndCol = 0, nStartRow = 0, nEndRow = 0;
                int nNumbeOfRowsSpan = 0;
                #region Report Body
                foreach (TradingSaleInvoiceRegister oItem in oTradingSaleInvoiceRegisters)
                {
                    colIndex = 2;

                    if (oItem.InvoiceDateInString != sPreviousInvoiceDate)
                    {
                        if (sPreviousInvoiceDate != "")
                        {
                            #region SubTotal
                            colIndex = 2;
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                            cell = sheet.Cells[rowIndex, colIndex, rowIndex, 9]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Unit Price
                            colIndex = 10;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Qty
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Item Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Item Discount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Item NetAmount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Gross Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Discount Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Vat Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Service Chareg
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Net Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Rcv Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Due Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Purchase Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Profit Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            colIndex = 2;
                            rowIndex++;
                            #endregion
                        }

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn+1]; cell.Merge = true; cell.Value = "Invoice Date: " + oItem.InvoiceDateInString; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex = rowIndex + 1;
                        sPreviousInvoiceDate = oItem.InvoiceDateInString;
                        nCount = 0;
                        nStartRow = rowIndex;
                    }
                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceID)
                    {
                        nCount++;
                        nNumbeOfRowsSpan = GetRowSpanCount(oItem.TradingSaleInvoiceID, oTradingSaleInvoiceRegisters);
                        nNumbeOfRowsSpan = nNumbeOfRowsSpan > 0 ? nNumbeOfRowsSpan - 1 : 0;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0;(#,##0)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.CustomerName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.SalesTypeInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    else
                    {
                        colIndex = 7;
                    }
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Symbol; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InvoiceQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ItemDiscount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ItemNetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceID)
                    {

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.GrossAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.DiscountAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.VatAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.ServiceCharge; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.TotalReceivedAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.DueAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.PurchasePrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.ProfitAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }
                    else
                    {
                        //For Mange Merge Cell Value
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }
                    sPreviousInvoiceDate = oItem.InvoiceDateInString;
                    nPrevInvoiceID = oItem.TradingSaleInvoiceID;
                    rowIndex++;
                }

                #region SubTotal
                colIndex = 2;
                aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 9]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Unit Price
                colIndex = 10;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Qty
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Item Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Item Discount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Item NetAmount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Gross Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Discount Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Vat Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Service Chareg
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Net Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Rcv Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Due Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Purchase Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Profit Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                colIndex = 2;
                rowIndex++;
                #endregion



                #region Grand Total
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 10; nStartCol = 2;
                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalIndex]; cell.Merge = true;
                cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                nEndRow = rowIndex - 1;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                #endregion
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DateWise_Trading_Sale_Invoice(Product Based).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        private void PartyWiseProductBasedXL(List<TradingSaleInvoiceRegister> oTradingSaleInvoiceRegisters, Company oCompany)
        {
            #region Report Body
            int rowIndex = 2;
            int nMaxColumn;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            float nFontSize = 10f;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Trading Sale Invoice Register");
                sheet.Name = "Trading Sale Invoice Register";
                sheet.View.FreezePanes(6, 6);
                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                #region Coloums
                sheet.Column(colIndex++).Width = 10;//SL
                sheet.Column(colIndex++).Width = 20;//Invoice No
                sheet.Column(colIndex++).Width = 15;//Invoice Date
                sheet.Column(colIndex++).Width = 15;//Sales Type
                sheet.Column(colIndex++).Width = 15;//Sales By
                sheet.Column(colIndex++).Width = 18;//Product name
                sheet.Column(colIndex++).Width = 18;//Product Code
                sheet.Column(colIndex++).Width = 15;//MUnit
                sheet.Column(colIndex++).Width = 15;//UnitPrice
                sheet.Column(colIndex++).Width = 15;//Quantity
                sheet.Column(colIndex++).Width = 15;//Amount
                sheet.Column(colIndex++).Width = 15;//ItemWise Discount
                sheet.Column(colIndex++).Width = 15;//Gross Amount
                sheet.Column(colIndex++).Width = 15;//Discount Amount
                sheet.Column(colIndex++).Width = 15;//Vat Amount
                sheet.Column(colIndex++).Width = 15;//Service Charge
                sheet.Column(colIndex++).Width = 15;//Net Amount
                sheet.Column(colIndex++).Width = 15;//Received Amount
                sheet.Column(colIndex++).Width = 15;//Due Amount
                sheet.Column(colIndex++).Width = 20;//Purchase Price
                sheet.Column(colIndex++).Width = 20;//Profit/Loss

                nMaxColumn = colIndex - 1;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Cash Sales and Received Register Party Wise(Product based)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion


                #region Column Header
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "S.L No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Invoice No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Sales Type"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Salse By"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                int nSubColIndex = colIndex;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, (colIndex + 7)]; cell.Merge = true; cell.Value = "Item Description"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex = (colIndex + 7) + 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Gross Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Discount Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Vat Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Service Charge"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Net Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Received Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Due Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Purchase Price"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Profit/Loss"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                colIndex = nSubColIndex;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Product Code"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Product name"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "M.Unit"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Quantity"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Discount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                #endregion
                int nCount = 0;
                int nPrevInvoiceID = 0;
                int nPrevCustomerID = 0;
                string sStartCell = "", sEndCell = "";
                int nStartCol = 0, nEndCol = 0, nStartRow = 0, nEndRow = 0;
                int nNumbeOfRowsSpan = 0;
                #region Report Body
                foreach (TradingSaleInvoiceRegister oItem in oTradingSaleInvoiceRegisters)
                {
                    colIndex = 2;

                    if (oItem.BuyerID != nPrevCustomerID)
                    {
                        if (nPrevCustomerID != 0)
                        {
                            #region SubTotal
                            colIndex = 2;
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                            cell = sheet.Cells[rowIndex, colIndex, rowIndex, 9]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Unit Price
                            colIndex = 10;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Qty
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Item Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Item Discount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Item NetAmount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Gross Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Discount Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Vat Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Service Chareg
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Net Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Rcv Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Due Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Purchase Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Profit Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            colIndex = 2;
                            rowIndex++;
                            #endregion
                        }

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn+1]; cell.Merge = true; cell.Value = "Customer Name: " + oItem.CustomerName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex = rowIndex + 1;
                        nPrevCustomerID = oItem.BuyerID;
                        nCount = 0;
                        nStartRow = rowIndex;
                    }
                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceID)
                    {
                        nCount++;
                        nNumbeOfRowsSpan = GetRowSpanCount(oItem.TradingSaleInvoiceID, oTradingSaleInvoiceRegisters);
                        nNumbeOfRowsSpan = nNumbeOfRowsSpan > 0 ? nNumbeOfRowsSpan - 1 : 0;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0;(#,##0)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.InvoiceDateInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.SalesTypeInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    else
                    {
                        colIndex = 7;
                    }
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Symbol; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InvoiceQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ItemDiscount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ItemNetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceID)
                    {

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.GrossAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.DiscountAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.VatAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.ServiceCharge; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.TotalReceivedAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.DueAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.PurchasePrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.ProfitAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }
                    else
                    {
                        //For Mange Merge Cell Value
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }
                    nPrevCustomerID = oItem.BuyerID;
                    nPrevInvoiceID = oItem.TradingSaleInvoiceID;
                    rowIndex++;
                }

                #region SubTotal
                colIndex = 2;
                aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 9]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Unit Price
                colIndex = 10;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Qty
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Item Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Item Discount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Item NetAmount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Gross Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Discount Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Vat Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Service Chareg
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Net Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Rcv Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Due Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Purchase Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Profit Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                colIndex = 2;
                rowIndex++;
                #endregion



                #region Grand Total
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 10; nStartCol = 2;
                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalIndex]; cell.Merge = true;
                cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nEndRow = rowIndex - 1;
                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                #endregion
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BuyerWise_Trading_Sale_Invoice(Product Based).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        private void ProductWiseProductBasedXL(List<TradingSaleInvoiceRegister> oTradingSaleInvoiceRegisters, Company oCompany)
        {
            #region Report Body
            int rowIndex = 2;
            int nMaxColumn;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            float nFontSize = 10f;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Trading Sale Invoice Register");
                sheet.Name = "Trading Sale Invoice Register";
                sheet.View.FreezePanes(6, 6);
                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                #region Coloums
                sheet.Column(colIndex++).Width = 10;//SL
                sheet.Column(colIndex++).Width = 20;//Invoice No
                sheet.Column(colIndex++).Width = 15;//Invoice Date
                sheet.Column(colIndex++).Width = 15;//Sales Type
                sheet.Column(colIndex++).Width = 15;//Sales By
                sheet.Column(colIndex++).Width = 18;//CustomerName
                sheet.Column(colIndex++).Width = 15;//MUnit
                sheet.Column(colIndex++).Width = 15;//UnitPrice
                sheet.Column(colIndex++).Width = 15;//Quantity
                sheet.Column(colIndex++).Width = 15;//Amount
                sheet.Column(colIndex++).Width = 15;//ItemWise Discount
                sheet.Column(colIndex++).Width = 20;//Purchase Price
                sheet.Column(colIndex++).Width = 20;//Profit/Loss

                nMaxColumn = colIndex - 1;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Cash Sales and Received Register Product Wise(Product based)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion


                #region Column Header
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "S.L No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Invoice No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Sales Type"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Salse By"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "CustomerName"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                int nSubColIndex = colIndex;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, (colIndex + 5)]; cell.Merge = true; cell.Value = "Item Description"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                colIndex = (colIndex + 5) + 1;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Purchase Price"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Profit/Loss"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                colIndex = nSubColIndex;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "M.Unit"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Quantity"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Discount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex++;

                #endregion
                int nCount = 0;
                int nPrevProductID = 0;
                string sStartCell = "", sEndCell = "";
                int nStartCol = 0, nEndCol = 0, nStartRow = 0, nEndRow = 0;
                #region Report Body
                foreach (TradingSaleInvoiceRegister oItem in oTradingSaleInvoiceRegisters)
                {
                    colIndex = 2;

                    if (oItem.ProductID != nPrevProductID)
                    {
                        if (nPrevProductID != 0)
                        {
                            #region SubTotal
                            colIndex = 2;
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                            cell = sheet.Cells[rowIndex, colIndex, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Unit Price
                            colIndex = 9;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Qty
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Item Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Item Discount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            //Item NetAmount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                            //Purchase Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            //Profit Amount
                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            colIndex = 2;
                            rowIndex++;
                            #endregion
                        }

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn+1]; cell.Merge = true; cell.Value = "Product Code: " + oItem.ProductCode + " , Product Name: " + oItem.ProductName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex = rowIndex + 1;
                        nPrevProductID = oItem.ProductID;
                        nCount = 0;
                        nStartRow = rowIndex;
                    }


                    nCount++;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0;(#,##0)";

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InvoiceDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.SalesTypeInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.CustomerName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Symbol; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InvoiceQty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Amount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ItemDiscount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ItemNetAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PurchasePrice; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ProfitAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nPrevProductID = oItem.ProductID;
                    rowIndex++;
                }

                #region SubTotal
                colIndex = 2;
                aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Unit Price
                colIndex = 9;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Qty
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Item Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Item Discount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Item NetAmount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                //Purchase Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                //Profit Amount
                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                colIndex = 2;
                rowIndex++;
                #endregion



                #region Grand Total
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 9; nStartCol = 2;
                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalIndex]; cell.Merge = true;
                cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nEndRow = rowIndex - 1;
                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                #endregion
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ProductWise_Trading_Sale_Invoice(Product Based).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        private void DateWisePayemntBasedXL(List<TradingSaleInvoiceRegister> oTradingSaleInvoiceRegisters, Company oCompany)
        {
            #region Report Body
            int rowIndex = 2;
            int nMaxColumn;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            float nFontSize = 10f;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Trading Sale Invoice Register");
                sheet.Name = "Trading Sale Invoice Register";
                sheet.View.FreezePanes(5, 5);
                Dictionary<string, string> aSubTotals = new Dictionary<string, string>();
                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 9;
                #region Coloums
                sheet.Column(colIndex++).Width = 10;//SL
                sheet.Column(colIndex++).Width = 20;//Invoice No
                sheet.Column(colIndex++).Width = 15;//Customer Name
                sheet.Column(colIndex++).Width = 15;//Sales Type
                sheet.Column(colIndex++).Width = 15;//Sales By
                sheet.Column(colIndex++).Width = 15;//Amounts
                sheet.Column(colIndex++).Width = 15;//Discount amount
                sheet.Column(colIndex++).Width = 15;//Vat Amount
                sheet.Column(colIndex++).Width = 15;//Service Charge
                sheet.Column(colIndex++).Width = 15;//Net Amount
                sheet.Column(colIndex++).Width = 15;//Receive No
                sheet.Column(colIndex++).Width = 15;//Rcv Date
                sheet.Column(colIndex++).Width = 15;//Rcv Amount
                sheet.Column(colIndex++).Width = 15;//Due Amount
                sheet.Column(colIndex++).Width = 15;//Net Amount
                sheet.Column(colIndex++).Width = 20;//Purchase Price
                sheet.Column(colIndex++).Width = 20;//Profit/Loss

                nMaxColumn = colIndex - 1;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Cash Sales and Received Register Date Wise(Payment based)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion


                #region Column Header
                colIndex = 2;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "S.L No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Invoice No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Customer Name"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sales Type"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salse By"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Discount Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Vat Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Charge"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Received No"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Received Date"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Received Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Due Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Purchase Price"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Profit/Loss"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion
                int nCount = 0;
                string sPreviousDate = "";
                int nPrevInvoiceId = 0;
                string sStartCell = "", sEndCell = "";
                int nStartCol = 0, nEndCol = 0, nStartRow = 0, nEndRow = 0, nStartRowRcv = 0,nSubEndRow=0;
                int nNumbeOfRowsSpan = 0;
                #region Report Body
                foreach (TradingSaleInvoiceRegister oItem in oTradingSaleInvoiceRegisters)
                {
                    colIndex = 2;
                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceId)
                    {
                        if (nPrevInvoiceId != 0)
                        {
                            #region InvoiceWise Subtotal

                            aSubTotals.Add((aSubTotals.Count + 1).ToString(), (nStartRowRcv.ToString() + "," + (rowIndex - 1).ToString()));
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nStartCol = 14;
                            sStartCell = Global.GetExcelCellName(nStartRowRcv, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowIndex, 15, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                        }
                    }
                    if (oItem.InvoiceDateInString != sPreviousDate)
                    {
                        if (sPreviousDate != "")
                        {
                            #region SubTotal
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            colIndex = 7;


                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            sFormula = ""; nTempStartRow = 0; nTempEndRow = 0;
                            nTotalIndex = 9; nStartCol = 2;
                            #region Formula
                            sFormula = "";
                            if (aSubTotals.Count > 0)
                            {
                                nTotalIndex = nTotalIndex + 1;
                                sFormula = "SUM(";
                                for (int i = 1; i <= aSubTotals.Count; i++)
                                {
                                    nTempStartRow = Convert.ToInt32(Convert.ToString(aSubTotals[i.ToString()]).Split(',')[0]);
                                    nTempEndRow = Convert.ToInt32(Convert.ToString(aSubTotals[i.ToString()]).Split(',')[1]);
                                    sStartCell = Global.GetExcelCellName(nTempStartRow, 14);
                                    sEndCell = Global.GetExcelCellName(nTempEndRow, 14);
                                    sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                                }
                                if (sFormula.Length > 0)
                                {
                                    sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                                }
                                sFormula = sFormula + ")";
                            }
                            else
                            {
                                sStartCell = Global.GetExcelCellName(nStartRow, 14);
                                sEndCell = Global.GetExcelCellName(rowIndex-1, 14);
                                sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                            }
                            #endregion
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            aSubTotals = new Dictionary<string, string>();

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            colIndex = 2;
                            rowIndex++;
                            #endregion
                        }
                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 1]; cell.Merge = true; cell.Value = "Invoice Date: " + oItem.InvoiceDateInString; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex = rowIndex + 1;
                        sPreviousDate = oItem.InvoiceDateInString;
                        nCount = 0;
                        nStartRow = rowIndex;
                    }
                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceId)
                    {

                        nCount++;
                        nNumbeOfRowsSpan = GetRowSpanCount(oItem.TradingSaleInvoiceID, oTradingSaleInvoiceRegisters);

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0;(#,##0)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.CustomerName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.SalesTypeInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.DiscountAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.VatAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.ServiceCharge; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        nStartRowRcv = rowIndex;
                    }
                    else
                    {

                        colIndex = 7;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";


                        //colIndex = 12;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceivedNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.ReceivedDate == DateTime.MinValue) ? "" : oItem.ReceivedDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceivedAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceId)
                    {

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.DueAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.PurchasePrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.ProfitAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }
                    sPreviousDate = oItem.InvoiceDateInString;
                    nPrevInvoiceId = oItem.TradingSaleInvoiceID;
                    rowIndex++;
                }
                #region SubTotal
                aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                colIndex = 7;


                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                sFormula = ""; nTempStartRow = 0; nTempEndRow = 0;
                nTotalIndex = 9; nStartCol = 2;
                #region Formula
                sFormula = "";
                if (aSubTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aSubTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aSubTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aSubTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, 14);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, 14);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, 14);
                    sEndCell = Global.GetExcelCellName(rowIndex-1, 14);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                aSubTotals = new Dictionary<string, string>();

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                colIndex = 2;
                rowIndex++;
                #endregion
                #region Grand Total
                sFormula = ""; nTempStartRow = 0; nTempEndRow = 0;
                nTotalIndex = 6; nStartCol = 2;
                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalIndex]; cell.Merge = true;
                cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex++]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, nTotalIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;




                #endregion
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=DateWise_Trading_Sale_Invoice(Payment based).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        private void PartyWisePayemntBasedXL(List<TradingSaleInvoiceRegister> oTradingSaleInvoiceRegisters, Company oCompany)
        {
            #region Report Body
            int rowIndex = 2;
            int nMaxColumn;
            int colIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            float nFontSize = 10f;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Trading Sale Invoice Register");
                sheet.Name = "Trading Sale Invoice Register";
                sheet.View.FreezePanes(5, 5);
                Dictionary<string, string> aSubTotals = new Dictionary<string, string>();
                Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                string sFormula = ""; int nTempStartRow = 0, nTempEndRow = 0;
                int nTotalIndex = 9;
                #region Coloums
                sheet.Column(colIndex++).Width = 10;//SL
                sheet.Column(colIndex++).Width = 20;//Invoice No
                sheet.Column(colIndex++).Width = 15;//Invoice Date
                sheet.Column(colIndex++).Width = 15;//Sales Type
                sheet.Column(colIndex++).Width = 15;//Sales By
                sheet.Column(colIndex++).Width = 15;//Amounts
                sheet.Column(colIndex++).Width = 15;//Discount amount
                sheet.Column(colIndex++).Width = 15;//Vat Amount
                sheet.Column(colIndex++).Width = 15;//Service Charge
                sheet.Column(colIndex++).Width = 15;//Net Amount
                sheet.Column(colIndex++).Width = 15;//Receive No
                sheet.Column(colIndex++).Width = 15;//Rcv Date
                sheet.Column(colIndex++).Width = 15;//Rcv Amount
                sheet.Column(colIndex++).Width = 15;//Due Amount
                sheet.Column(colIndex++).Width = 15;//Net Amount
                sheet.Column(colIndex++).Width = 20;//Purchase Price
                sheet.Column(colIndex++).Width = 20;//Profit/Loss

                nMaxColumn = colIndex - 1;
                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Cash Sales and Received Register Party Wise(Payment based)"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion


                #region Column Header
                colIndex = 2;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "S.L No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Invoice No."; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sales Type"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salse By"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Discount Amounts"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Vat Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Service Charge"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Received No"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Received Date"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Received Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Due Amount"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Purchase Price"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Profit/Loss"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion
                int nCount = 0;
                int nPrevBuyerID = 0;
                int nPrevInvoiceId = 0;
                string sStartCell = "", sEndCell = "";
                int nStartCol = 0, nEndCol = 0, nStartRow = 0, nEndRow = 0, nStartRowRcv = 0;
                int nNumbeOfRowsSpan = 0;
                #region Report Body
                foreach (TradingSaleInvoiceRegister oItem in oTradingSaleInvoiceRegisters)
                {
                    colIndex = 2;
                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceId)
                    {
                        if (nPrevInvoiceId != 0)
                        {
                            #region InvoiceWise Subtotal

                            aSubTotals.Add((aSubTotals.Count + 1).ToString(), (nStartRowRcv.ToString() + "," + (rowIndex - 1).ToString()));
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 13]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nStartCol = 14;
                            sStartCell = Global.GetExcelCellName(nStartRowRcv, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);
                            cell = sheet.Cells[rowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[rowIndex, 15, rowIndex, 17]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                            #endregion
                        }
                    }
                    if (oItem.BuyerID != nPrevBuyerID)
                    {
                        if (nPrevBuyerID != 0)
                        {
                            #region SubTotal
                            aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            colIndex = 7;


                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            sFormula = ""; nTempStartRow = 0; nTempEndRow = 0;
                            nTotalIndex = 9; nStartCol = 2;
                            #region Formula
                            sFormula = "";
                            if (aSubTotals.Count > 0)
                            {
                                nTotalIndex = nTotalIndex + 1;
                                sFormula = "SUM(";
                                for (int i = 1; i <= aSubTotals.Count; i++)
                                {
                                    nTempStartRow = Convert.ToInt32(Convert.ToString(aSubTotals[i.ToString()]).Split(',')[0]);
                                    nTempEndRow = Convert.ToInt32(Convert.ToString(aSubTotals[i.ToString()]).Split(',')[1]);
                                    sStartCell = Global.GetExcelCellName(nTempStartRow, 14);
                                    sEndCell = Global.GetExcelCellName(nTempEndRow, 14);
                                    sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                                }
                                if (sFormula.Length > 0)
                                {
                                    sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                                }
                                sFormula = sFormula + ")";
                            }
                            else
                            {
                                sStartCell = Global.GetExcelCellName(nStartRow, 14);
                                sEndCell = Global.GetExcelCellName(rowIndex - 1, 14);
                                sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                            }
                            #endregion
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            aSubTotals = new Dictionary<string, string>();

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            nStartCol = colIndex;
                            sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                            sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                            cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            colIndex = 2;
                            rowIndex++;
                            #endregion
                        }
                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn - 1]; cell.Merge = true; cell.Value = "Buyer Name: " + oItem.CustomerName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex = rowIndex + 1;
                        nPrevBuyerID = oItem.BuyerID;
                        nCount = 0;
                        nStartRow = rowIndex;
                    }
                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceId)
                    {

                        nCount++;
                        nNumbeOfRowsSpan = GetRowSpanCount(oItem.TradingSaleInvoiceID, oTradingSaleInvoiceRegisters);

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0;(#,##0)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.InvoiceDateInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.SalesTypeInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.Amount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.DiscountAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.VatAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.ServiceCharge; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.NetAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        nStartRowRcv = rowIndex;
                    }
                    else
                    {

                        colIndex = 7;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";


                        //colIndex = 12;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceivedNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.ReceivedDate == DateTime.MinValue) ? "" : oItem.ReceivedDateInString; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ReceivedAmount; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    if (oItem.TradingSaleInvoiceID != nPrevInvoiceId)
                    {

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.DueAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.PurchasePrice; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex, rowIndex + nNumbeOfRowsSpan - 1, colIndex++]; cell.Merge = true; cell.Value = oItem.ProfitAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    }
                    nPrevBuyerID = oItem.BuyerID;
                    nPrevInvoiceId = oItem.TradingSaleInvoiceID;
                    rowIndex++;
                }
                #region SubTotal
                aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + (rowIndex - 1).ToString()));
                cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12f; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                colIndex = 7;


                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                sFormula = ""; nTempStartRow = 0; nTempEndRow = 0;
                nTotalIndex = 9; nStartCol = 2;
                #region Formula
                sFormula = "";
                if (aSubTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aSubTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aSubTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aSubTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, 14);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, 14);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, 14);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, 14);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                aSubTotals = new Dictionary<string, string>();

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                nStartCol = colIndex;
                sStartCell = Global.GetExcelCellName(nStartRow, nStartCol);
                sEndCell = Global.GetExcelCellName(rowIndex - 1, nStartCol);

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = nFontSize; fill = cell.Style.Fill; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                colIndex = 2;
                rowIndex++;
                #endregion
                #region Grand Total
                sFormula = ""; nTempStartRow = 0; nTempEndRow = 0;
                nTotalIndex = 6; nStartCol = 2;
                cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalIndex]; cell.Merge = true;
                cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex++]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, nTotalIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;



                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                #region Formula
                sFormula = "";
                if (aGrandTotals.Count > 0)
                {
                    nTotalIndex = nTotalIndex + 1;
                    sFormula = "SUM(";
                    for (int i = 1; i <= aGrandTotals.Count; i++)
                    {
                        nTempStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                        nTempEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                        sStartCell = Global.GetExcelCellName(nTempStartRow, nTotalIndex);
                        sEndCell = Global.GetExcelCellName(nTempEndRow, nTotalIndex);
                        sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                    }
                    if (sFormula.Length > 0)
                    {
                        sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                    }
                    sFormula = sFormula + ")";
                }
                else
                {
                    sStartCell = Global.GetExcelCellName(nStartRow, ++nTotalIndex);
                    sEndCell = Global.GetExcelCellName(rowIndex - 1, nTotalIndex);
                    sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                }
                #endregion
                cell = sheet.Cells[rowIndex, nTotalIndex]; cell.Formula = sFormula; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;




                #endregion
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PartyWise_Trading_Sale_Invoice(Payment based).xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        private string GetTradingSaleInvoiceSQL(TradingSaleInvoiceRegister oTradingSaleInvoiceRegister)
        {
            string sSearchingData = oTradingSaleInvoiceRegister.ErrorMessage;
            EnumCompareOperator eInvoiceDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dInvoiceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[3]);
            string sContractorIDs = Convert.ToString(sSearchingData.Split('~')[4]);


            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oTradingSaleInvoiceRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oTradingSaleInvoiceRegister.BUID.ToString();
            }
            #endregion

            #region FabricRequestNo
            if (!string.IsNullOrEmpty(oTradingSaleInvoiceRegister.InvoiceNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " InvoiceNo LIKE'%" + oTradingSaleInvoiceRegister.InvoiceNo + "%'";
            }
            #endregion

            #region ContractorID
            if (sContractorIDs != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BuyerID IN (" + sContractorIDs + ")";
            }
            #endregion

            #region ProductID
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + "  TradingSaleInvoiceID IN (SELECT TradingSaleInvoiceID FROM TradingSaleInvoiceDetail WHERE ProductID IN(" + sProductIDs + "))";
            }
            #endregion



            #region Receive Date
            if (eInvoiceDate != EnumCompareOperator.None)
            {
                if (eInvoiceDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eInvoiceDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),InvoiceDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }

        private int GetRowSpanCount(int nInvoiceId, List<TradingSaleInvoiceRegister> oTradingSaleInvoiceRegisters)
        {
            int nRowSpanCount = 0;

            foreach (TradingSaleInvoiceRegister oItem in oTradingSaleInvoiceRegisters)
            {
                if (oItem.TradingSaleInvoiceID == nInvoiceId)
                {
                    nRowSpanCount = nRowSpanCount + 1;
                }
            }
            return nRowSpanCount;
        }


        #endregion
    }
}
