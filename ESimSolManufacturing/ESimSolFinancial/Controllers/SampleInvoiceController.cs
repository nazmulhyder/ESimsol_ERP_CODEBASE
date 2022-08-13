using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using ReportManagement;
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class SampleInvoiceController : Controller
    {
        #region Declaration
        SampleInvoice _oSampleInvoice = new SampleInvoice();
        List<SampleInvoice> _oSampleInvoices = new List<SampleInvoice>();
        List<SampleInvoiceDetail> _oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
        #endregion
        public ActionResult ViewSampleInvoices(int buid, int ProductNature, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            _oSampleInvoices = new List<SampleInvoice>();
            _oSampleInvoices = SampleInvoice.Gets("SELECT top(500)* FROM View_SampleInvoice  where BUID in (0," + buid + ") and Isnull(ApproveBy,0)=0 and CurrentStatus in (0,1) and ISNULL(ProductNature,0)= " + ProductNature + " and InvoiceType in (0,1,11) order by SampleInvoiceID DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            List<EnumObject> oOrderTypeObjs = new List<EnumObject>();
            List<EnumObject> oOrderTypes = new List<EnumObject>();
            oOrderTypeObjs = EnumObject.jGets(typeof(EnumOrderType));

            foreach (EnumObject oItem in oOrderTypeObjs)
            {
                if (oItem.id == (int)EnumOrderType.SampleOrder || oItem.id == (int)EnumOrderType.SaleOrder)
                {
                    oOrderTypes.Add(oItem);
                }
            }
            oSampleInvoiceSetup = oSampleInvoiceSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.OrderTypes = oOrderTypes;
            ViewBag.ProductNature = ProductNature;
            ViewBag.OrderPaymentTypeObjs = EnumObject.jGets(typeof(EnumOrderPaymentType));
            ViewBag.SampleInvoiceSetup = oSampleInvoiceSetup;
            return View(_oSampleInvoices);
        }
        public ActionResult ViewSampleInvoice(int id, int buid)
        {
            List<Currency> oCurrencys = new List<Currency>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            SampleInvoiceDetail oSampleInvoiceDetail_DO = new SampleInvoiceDetail();
            List<SampleInvoiceCharge> oSampleInvoiceCharge = new List<SampleInvoiceCharge>();
            _oSampleInvoice = new SampleInvoice();
          
            Company oCompany = new Company();
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);//Get Company
            string _sSql = "";
            if (id > 0)
            {
                _oSampleInvoice = _oSampleInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                _oSampleInvoiceDetails = _oSampleInvoice.SampleInvoiceDetails.Where(o => o.DyeingOrderID>0).ToList();
                _oSampleInvoice.SampleInvoiceDetails = _oSampleInvoice.SampleInvoiceDetails.Where(o => o.DyeingOrderID == 0).ToList();
                foreach(SampleInvoiceDetail oItem in _oSampleInvoice.SampleInvoiceDetails)
                {
                    oItem.Currency = _oSampleInvoice.CurrencySymbol;
                }
                _sSql = "SELECT * FROM View_SampleInvoiceCharge Where SampleInvoiceID = " + id + " Order By SampleInvoiceChargeID";
                oSampleInvoiceCharge = SampleInvoiceCharge.Gets(_sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                //PI Print Setup
                oExportPIPrintSetup = oExportPIPrintSetup.Get(true, buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oExportPIPrintSetup.ExportPIPrintSetupID <= 0 && oExportPIPrintSetup.BaseCurrencyID <= 0)
                {
                    _oSampleInvoice.ErrorMessage = "Please Set Base Currency in PI Print Setup";
                }
                else
                {
                    _oSampleInvoice.CurrencyID = oExportPIPrintSetup.BaseCurrencyID;
                    _oSampleInvoice.CurrencySymbol = oCurrencys.Where(x => x.CurrencyID == _oSampleInvoice.CurrencyID).FirstOrDefault().Symbol;
                    _oSampleInvoice.CurrencyName = oCurrencys.Where(x => x.CurrencyID == _oSampleInvoice.CurrencyID).FirstOrDefault().CurrencyName;
                }
            }
            if (_oSampleInvoice.ExchangeCurrencyID <= 0)
            {
                _oSampleInvoice.ExchangeCurrencyID = oCompany.BaseCurrencyID;
                _oSampleInvoice.ExchangeCurrencySymbol = oCompany.CurrencySymbol;
                _oSampleInvoice.ExchangeCurrencyName = oCompany.CurrencyName;
            }

            ContactPersonnel oContactPersonnel = new ContactPersonnel();
            oContactPersonnel.ContactPersonnelID = _oSampleInvoice.ContractorPersopnnalID;
            if (_oSampleInvoice.ContractorPersopnnalID > 0)
            {
                oContactPersonnel.Name = _oSampleInvoice.ContractorPersopnnalName;
                oContactPersonnel.Phone = _oSampleInvoice.ContractNo;
            }
            else
            {
                oContactPersonnel.Name = "--Select Contract Person--";
            }
            _oSampleInvoice.ContactPersonnels = new List<ContactPersonnel>();
            _oSampleInvoice.ContactPersonnels.Add(oContactPersonnel);
            ViewBag.OrderPaymentTypeObjs = EnumObject.jGets(typeof(EnumOrderPaymentType));
            oSampleInvoiceSetup = oSampleInvoiceSetup.GetByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (id > 0)
            {
                string sDyeingOrderID = string.Join(",", _oSampleInvoiceDetails.Select(x => x.DyeingOrderID).ToList());
                if (string.IsNullOrEmpty(sDyeingOrderID))
                    sDyeingOrderID = "0";
                oDyeingOrders = DyeingOrder.Gets("Select * from view_DyeingOrder where SampleInvoiceID=" + id + " and PaymentType  in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + "," + (int)EnumOrderPaymentType.CashOrCheque + ") and DyeingOrderID not in (" + sDyeingOrderID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
              
                oDyeingOrderReports = DyeingOrderReport.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
                foreach (DyeingOrder oDyeingOrder in oDyeingOrders)
                {
                    oSampleInvoiceDetail_DO = new SampleInvoiceDetail();
                    oSampleInvoiceDetail_DO.OrderNo = oDyeingOrder.OrderNoFull;
                    oSampleInvoiceDetail_DO.DyeingOrderID = oDyeingOrder.DyeingOrderID;
                    oSampleInvoiceDetail_DO.Qty = oDyeingOrder.Qty;
                    oSampleInvoiceDetail_DO.Amount = oDyeingOrder.Amount;
                    oSampleInvoiceDetail_DO.PaymentType = oDyeingOrder.PaymentType;
                    _oSampleInvoiceDetails.Add(oSampleInvoiceDetail_DO);
                }
            }

            ViewBag.SampleInvoiceDetails_DO = _oSampleInvoiceDetails;
            ViewBag.SampleInvoiceSetup = oSampleInvoiceSetup;
            ViewBag.DyeingOrderDetails = oDyeingOrderReports;
            ViewBag.Currencys = oCurrencys;
            _oSampleInvoice.SampleInvoiceCharges = oSampleInvoiceCharge;
            return View(_oSampleInvoice);
        }
        public ActionResult ViewSampleInvoiceApproved(int id, int buid)
        {
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            _oSampleInvoice = new SampleInvoice();
            _oSampleInvoice = _oSampleInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (id > 0)
            {
                _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ContactPersonnel oContactPersonnel = new ContactPersonnel();
            oContactPersonnel.ContactPersonnelID = _oSampleInvoice.ContractorPersopnnalID;
            if (_oSampleInvoice.ContractorPersopnnalID > 0)
            {
                oContactPersonnel.Name = _oSampleInvoice.ContractorPersopnnalName;
                oContactPersonnel.Phone = _oSampleInvoice.ContractNo;
            }
            else
            {
                oContactPersonnel.Name = "--Select Contract Person--";
            }
            _oSampleInvoice.ContactPersonnels = new List<ContactPersonnel>();
            _oSampleInvoice.ContactPersonnels.Add(oContactPersonnel);
            oDyeingOrderDetails = DyeingOrderDetail.GetsBy(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.SampleInvoiceDetails = oDyeingOrderDetails;
            ViewBag.OrderPaymentTypeObjs = EnumObject.jGets(typeof(EnumOrderPaymentType));
            ViewBag.OrderPaymentAdjTypes = EnumObject.jGets(typeof(EnumOrderPaymentAdjType));
           
            return View(_oSampleInvoice);
        }
        #region InvoiceAdjustment
        public ActionResult ViewSampleInvoiceAdjs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            // this.Session.Add(SessionInfo.AuthorizationUserOEDOs, AuthorizationUserOEDO.GetsByDBObjectAndUser("'SampleInvoice'", (int)Session[SessionInfo.currentUserID], (Guid)Session[SessionInfo.wcfSessionID]));
            _oSampleInvoices = new List<SampleInvoice>();

            _oSampleInvoices = SampleInvoice.Gets("SELECT top(200)* FROM View_SampleInvoice  where BUID in (0," + buid + ") and Isnull(ApproveBy,0)=0 and CurrentStatus in (0,1) and InvoiceType in (3,4,5)  order by SampleInvoiceID DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            //ViewBag.PaymentTypes = Enum.GetValues(typeof(EnumOrderPaymentType)).Cast<EnumOrderPaymentType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            List<EnumObject> oInvoiceTypeObjs = new List<EnumObject>();
            List<EnumObject> oInvoiceTypes = new List<EnumObject>();
            oInvoiceTypeObjs = EnumObject.jGets(typeof(EnumSampleInvoiceType));

            foreach (EnumObject oItem in oInvoiceTypeObjs)
            {
                if (oItem.id == (int)EnumSampleInvoiceType.DocCharge || oItem.id == (int)EnumSampleInvoiceType.Adjstment_Qty || oItem.id == (int)EnumSampleInvoiceType.Adjstment_Value || oItem.id == (int)EnumSampleInvoiceType.Adjstment_Commission || oItem.id == (int)EnumSampleInvoiceType.ReturnAdjustment)
                {
                    oInvoiceTypes.Add(oItem);
                }

            }
            ViewBag.InvoiceTypes = oInvoiceTypes;

           
            return View(_oSampleInvoices);
        }
        public ActionResult ViewSampleInvoiceAdj(int id, int buid)
        {
            ExportSCDO oExportSCDO = new ExportSCDO();
            List<Currency> oCurrencys = new List<Currency>();
            List<ContactPersonnel> oCPIssueTos = new List<ContactPersonnel>();
            List<ContactPersonnel> oCPDeliveryTos = new List<ContactPersonnel>();
            _oSampleInvoice = new SampleInvoice();
            _oSampleInvoice = _oSampleInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (id > 0)
            {
                _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (SampleInvoiceDetail oItem in _oSampleInvoice.SampleInvoiceDetails)
                {
                    oItem.Currency = _oSampleInvoice.CurrencySymbol;
                }
                oCPIssueTos = ContactPersonnel.Gets(_oSampleInvoice.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCPDeliveryTos = ContactPersonnel.Gets(_oSampleInvoice.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            if (_oSampleInvoice.ExportPIID > 0)
            {
                oExportSCDO = oExportSCDO.GetByPI(_oSampleInvoice.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
            }

            ViewBag.ExportSCDO = oExportSCDO;
            ViewBag.CPIssueTos = oCPIssueTos;
            ViewBag.CPDeliveryTos = oCPDeliveryTos;

            List<EnumObject> oInvoiceTypeObjs = new List<EnumObject>();
            List<EnumObject> oInvoiceTypes = new List<EnumObject>();
            oInvoiceTypeObjs = EnumObject.jGets(typeof(EnumSampleInvoiceType));

            foreach (EnumObject oItem in oInvoiceTypeObjs)
            {
                if (oItem.id == (int)EnumSampleInvoiceType.DocCharge || oItem.id == (int)EnumSampleInvoiceType.Adjstment_Qty || oItem.id == (int)EnumSampleInvoiceType.Adjstment_Value || oItem.id == (int)EnumSampleInvoiceType.Adjstment_Commission || oItem.id == (int)EnumSampleInvoiceType.ReturnAdjustment)
                {
                    oInvoiceTypes.Add(oItem);
                }

            }
            ViewBag.InvoiceTypes = oInvoiceTypes;

        
            oCurrencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Currencys = oCurrencys;
            return View(_oSampleInvoice);
        }
        #endregion
        [HttpPost]
        public JsonResult Save(SampleInvoice oSampleInvoice)
        {
            _oSampleInvoice = new SampleInvoice();
            try
            {
                _oSampleInvoice = oSampleInvoice;
                _oSampleInvoice = _oSampleInvoice.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oSampleInvoice.SampleInvoiceID > 0)
                {
                    _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   // _oSampleInvoice.DyeingOrderReports = DyeingOrderReport.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                
            }
            catch (Exception ex)
            {
                _oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_Revise(SampleInvoice oSampleInvoice)
        {
            _oSampleInvoice = new SampleInvoice();
            try
            {
                _oSampleInvoice = oSampleInvoice;
                _oSampleInvoice = _oSampleInvoice.Save_Revise(((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oSampleInvoice.SampleInvoiceID > 0)
                {
                    _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    // _oSampleInvoice.DyeingOrderReports = DyeingOrderReport.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                _oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_AddDO(SampleInvoice oSampleInvoice)
        {
            _oSampleInvoice = new SampleInvoice();
            try
            {
                _oSampleInvoice = oSampleInvoice;
                _oSampleInvoice = _oSampleInvoice.Save_AddDO(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SICSave(SampleInvoiceCharge oSampleInvoiceCharge)
        {
            SampleInvoiceCharge _oSampleInvoiceCharge = new SampleInvoiceCharge();
            try
            {
                _oSampleInvoiceCharge = oSampleInvoiceCharge;
                _oSampleInvoiceCharge = _oSampleInvoiceCharge.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleInvoiceCharge = new SampleInvoiceCharge();
                _oSampleInvoiceCharge.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoiceCharge);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SICDelete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                SampleInvoiceCharge _SampleInvoiceCharge = new SampleInvoiceCharge();
                sFeedBackMessage = _SampleInvoiceCharge.Delete(id, (int)Session[SessionInfo.currentUserID]);

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
        public JsonResult Save_Rate(SampleInvoice oSampleInvoice)
        {
            _oSampleInvoice = new SampleInvoice();
            try
            {
                _oSampleInvoice = oSampleInvoice;
                _oSampleInvoice = _oSampleInvoice.Save_Rate(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateSampleInvoice(SampleInvoice oSampleInvoice)
        {
            _oSampleInvoice = new SampleInvoice();
            try
            {
                _oSampleInvoice = oSampleInvoice;
                _oSampleInvoice = _oSampleInvoice.UpdateSampleInvoice(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateSInvoiceNo(SampleInvoice oSampleInvoice)
        {
            _oSampleInvoice = new SampleInvoice();
            try
            {
                _oSampleInvoice = oSampleInvoice.UpdateSInvoiceNo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleInvoice = new SampleInvoice();
                _oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Approve(SampleInvoice oSampleInvoice)
        {
            string sErrorMease = "";
            _oSampleInvoice = oSampleInvoice;
            try
            {
                _oSampleInvoice = _oSampleInvoice.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Cancel(SampleInvoice oSampleInvoice)
        {
            string sErrorMease = "";
            _oSampleInvoice = oSampleInvoice;
            try
            {
                _oSampleInvoice = _oSampleInvoice.Cancel(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ExportPISNA(ExportPI oExportPI)
        {
            _oSampleInvoices = new List<SampleInvoice>();
            try
            {
                _oSampleInvoices = SampleInvoice.ExportPISNA(oExportPI.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oSampleInvoice = new SampleInvoice();
                _oSampleInvoice.ErrorMessage = ex.Message;
                _oSampleInvoices = new List<SampleInvoice>();
                _oSampleInvoices.Add(_oSampleInvoice);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                SampleInvoice oSampleInvoice = new SampleInvoice();
                oSampleInvoice = oSampleInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sFeedBackMessage = oSampleInvoice.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult DeleteDetail(SampleInvoiceDetail oSampleInvoiceHistory)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oSampleInvoiceHistory.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult UpdateVoucherEffect(SampleInvoice oSampleInvoice)
        {
            try
            {
                oSampleInvoice = oSampleInvoice.UpdateVoucherEffect((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDetail_Adj(SampleInvoiceDetail oSampleInvoiceHistory)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oSampleInvoiceHistory.Delete_Adj(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult UpdateInvoiceID(SampleInvoiceDetail oSampleInvoiceHistory)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oSampleInvoiceHistory.UpdateInvoiceID(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Save_PI(SampleInvoice oSampleInvoice)
        {
            ExportPI oExportPI = new ExportPI();
            ExportPIDetail oExportPIDetail = new ExportPIDetail();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            ExportPIPrintSetup oExportPIPrintSetup = new ExportPIPrintSetup();
            List<ProductBase> oProductBases = new List<ProductBase>();
            string sYarnCount = "";
            try
            {
                if (oSampleInvoice.SampleInvoiceID > 0)
                {
                    oExportPIPrintSetup = oExportPIPrintSetup.Get(true, oSampleInvoice.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    _oSampleInvoice = _oSampleInvoice.Get(oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDyeingOrderReports = DyeingOrderReport.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oSampleInvoiceDetails = oDyeingOrderReports.GroupBy(x => new { x.ProductID, x.ProductName, x.MUName, x.MUnitID }, (key, grp) =>
                                                     new SampleInvoiceDetail
                                                     {
                                                         ProductID = key.ProductID,
                                                         ProductName = key.ProductName,
                                                         Qty = grp.Sum(p => p.Qty),
                                                         Amount = grp.Sum(p => p.Amount),
                                                         MUnitID = key.MUnitID,
                                                         UnitPrice = grp.Sum(p => p.Amount) / grp.Sum(p => p.Qty),
                                                     }).ToList();

                }

                oExportPI = new ExportPI();
                oExportPI.ExportPIID = _oSampleInvoice.ExportPIID;
                oExportPI.PINo = _oSampleInvoice.ExportPINo;
                oExportPI.ContractorID = _oSampleInvoice.ContractorID;
                oExportPI.ContractorContactPerson = _oSampleInvoice.ContractorPersopnnalID;
                oExportPI.CurrencyID = oExportPIPrintSetup.BaseCurrencyID;
                oExportPI.PaymentType = EnumPIPaymentType.LC;
                oExportPI.PaymentTypeInInt = (int)EnumPIPaymentType.LC;
                oExportPI.MKTEmpID = _oSampleInvoice.MKTEmpID;
                oExportPI.BuyerID = 0;
                oExportPI.Qty = 0;
                oExportPI.Amount = _oSampleInvoice.Amount;
                oExportPI.BUID = _oSampleInvoice.BUID; //Pls change it by BU
                oExportPI.ConversionRate = 1; //Pls change it by BU
                oExportPI.PIStatusInInt = 0;
                oExportPI.IssueDate = _oSampleInvoice.PaymentDate;
                oExportPI.ValidityDate = _oSampleInvoice.PaymentDate.AddDays(oExportPIPrintSetup.ValidityDays);
                oExportPI.BankBranchID = 0;
                oExportPI.BankAccountID = 0;
                oExportPI.IsLIBORRate = true;
                oExportPI.IsBBankFDD = true;
                oExportPI.LCTermID = 0;
                oExportPI.OverdueRate = 0;
                oExportPI.LCOpenDate = DateTime.MinValue;
                oExportPI.DeliveryDate = DateTime.MinValue;
                oExportPI.Note = "";
                oExportPI.ApprovedBy = 0;
                oExportPI.ApprovedDate = DateTime.MinValue;
                oExportPI.ExportPIPrintSetupID = oExportPIPrintSetup.ExportPIPrintSetupID;
                oExportPI.ShipmentTermInInt = (int)EnumShipmentTerms.FOB;
                oExportPI.ColorInfo = "";
                oExportPI.DepthOfShade = "";
                oExportPI.YarnCount = "";
                oExportPI.DeliveryToID = 0;
                oExportPI.PIType = EnumPIType.Open;
                oExportPI.BuyerContactPerson = 0;
                oExportPI.RateUnit = 1;
                oExportPI.NoteTwo = "";
                oExportPI.ProductNature = EnumProductNature.Dyeing;
                oExportPI.OrderSheetID = 0;
                oExportPI.SampleInvoiceIDs = _oSampleInvoice.SampleInvoiceID.ToString();

                foreach (SampleInvoiceDetail oItem in _oSampleInvoiceDetails)
                {
                    oExportPIDetail = new ExportPIDetail();
                    oExportPIDetail.ProductID = _oSampleInvoiceDetails[0].ProductID;
                    oExportPIDetail.Qty = oItem.Qty;
                    oExportPIDetail.MUnitID = oItem.MUnitID;
                    oExportPIDetail.Amount = oItem.Amount;
                    oExportPIDetail.BuyerReference = sYarnCount;
                    if (oItem.Qty > 0)
                    {
                        oExportPIDetail.UnitPrice = oItem.Amount / oItem.Qty;
                    }
                    oExportPIDetails.Add(oExportPIDetail);
                }
                if (oExportPIDetails.Count > 0)
                {
                    oProductBases = ProductBase.Gets("Select * from View_ProductBase where ProductBaseID in (Select ProductBaseID from Product where isnull(ProductBaseID,0)>0 and ProductID  In (" + string.Join(",", oExportPIDetails.Select(x => x.ProductID).ToList()) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ProductBase oitem in oProductBases)
                    {
                        sYarnCount = sYarnCount + oitem.ProductName+",";
                    }
                }

                oExportPIDetails = oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.MUName, x.MUnitID }, (key, grp) =>
                                                   new ExportPIDetail
                                                   {
                                                       BuyerReference = sYarnCount,
                                                       ProductID = key.ProductID,
                                                       ProductName = key.ProductName,
                                                       Qty = grp.Sum(p => p.Qty),
                                                       Amount = grp.Sum(p => p.Amount),
                                                       MUnitID = key.MUnitID,
                                                       UnitPrice = grp.Sum(p => p.Amount) / grp.Sum(p => p.Qty),
                                                   }).ToList();

                oExportPI.ExportPIDetails = oExportPIDetails;

                oExportPI = oExportPI.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oSampleInvoice.ExportPIID = oExportPI.ExportPIID;
                _oSampleInvoice.ExportPINo = oExportPI.PINo;
                _oSampleInvoice.ErrorMessage = oExportPI.ErrorMessage;

            }
            catch (Exception ex)
            {

                _oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveExportPIFromBill(SampleInvoice oSampleInvoice)
        {
            try
            {
                if (oSampleInvoice.ExportPIID > 0)
                {
                    if (oSampleInvoice.SampleInvoiceID <= 0) { throw new Exception("Please select a delivery bill."); }
                    oSampleInvoice = oSampleInvoice.RemoveExportPIFromBill(oSampleInvoice, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oSampleInvoice.ErrorMessage = "deleted";
                }

            }
            catch (Exception ex)
            {
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleInvoice.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult BillForExportPI(SampleInvoice oSampleInvoice)
        {
            try
            {
                oSampleInvoice = oSampleInvoice.ExportPI_Attach(oSampleInvoice, (int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Searching
        [HttpPost]
        public JsonResult GetExportPIs(ExportSCDO oExportSCDO)
        {
            List<ExportSCDO> oExportSCDOs = new List<ExportSCDO>();
            List<DyeingOrder> _oDyeingOrders = new List<DyeingOrder>();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            try
            {
                string sSQL = "Select top(100)* from View_ExportSC_DO";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oExportSCDO.PINo))
                {
                    oExportSCDO.PINo = oExportSCDO.PINo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PINo Like'%" + oExportSCDO.PINo + "%'";
                }
                if (!String.IsNullOrEmpty(oExportSCDO.ExportLCNo))
                {
                    oExportSCDO.PINo = oExportSCDO.ExportLCNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportLCNo Like'%" + oExportSCDO.ExportLCNo + "%'";
                }
                if (oExportSCDO.LCID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LCID  in(" + oExportSCDO.LCID + ")";
                }
                if (oExportSCDO.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BUID  in(" + oExportSCDO.BUID + ")";
                }
                if (oExportSCDO.ContractorID > 0 || oExportSCDO.BuyerID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID  in(" + oExportSCDO.ContractorID + "," + oExportSCDO.BuyerID + " )";
                }
                //if (!oExportSCDO.IsRevisePI)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "IsRevisePI=0";
                //}

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PIStatus in (2,3,4)"; //For Dyeing/Production Order issue->Must Checked valid PI(  Initialized = 0, RequestForApproved = 1,
                //Approved = 2,     PIIssue = 3,   BindWithLC = 4,   RequestForRevise = 5,  Cancel = 6) and Sales Contract is Approved

                sSQL = sSQL + "" + sReturn;
                oExportSCDOs = ExportSCDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
            }
            catch (Exception ex)
            {
                oExportSCDOs = new List<ExportSCDO>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportSCDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }// For Production  Order
        [HttpPost]
        public JsonResult GetbyNo(SampleInvoice oSampleInvoice)
        {
            
            _oSampleInvoices = new List<SampleInvoice>();
            string sSQL = "";
            if(!String.IsNullOrEmpty(oSampleInvoice.SampleInvoiceNo ))
            {
                oSampleInvoice.SampleInvoiceNo = oSampleInvoice.SampleInvoiceNo.Trim();
                 sSQL = "SELECT * FROM View_SampleInvoice WHERE SampleInvoiceNo like '%" +oSampleInvoice.SampleInvoiceNo+ "'";
            }
            if (!String.IsNullOrEmpty(oSampleInvoice.ExportPINo))
            {
                oSampleInvoice.ExportPINo = oSampleInvoice.ExportPINo.Trim();
                sSQL = "SELECT * FROM View_SampleInvoice WHERE SampleInvoiceNo like '%" + oSampleInvoice.ExportPINo + "'";
            }
            if (!String.IsNullOrEmpty(oSampleInvoice.OrderNo))
            {
                oSampleInvoice.OrderNo = oSampleInvoice.OrderNo.Trim();
                sSQL = "SELECT * FROM View_SampleInvoice where SampleInvoiceID in (Select DyeingOrder.SampleInvoiceID from DyeingOrder where OrderType in (2,4) and DyeingOrder.OrderNo like '%" + oSampleInvoice.OrderNo + "')";
            }
          
            _oSampleInvoices = SampleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsOrderDetailForAdjs(SampleInvoice oSampleInvoice)
        {
            _oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            List<ExportSCDetail> oExportSCDetails = new List<ExportSCDetail>();
            oExportSCDetails = ExportSCDetail.GetsByPI(oSampleInvoice.ExportPIID,((User)Session[SessionInfo.CurrentUser]).UserID);

            SampleInvoiceDetail oSampleInvoiceDetail = new SampleInvoiceDetail();
            List<SampleInvoiceDetail> oSampleInvoiceDetails = new List<SampleInvoiceDetail>();

            foreach (ExportSCDetail oItemDOD in oExportSCDetails)
            {
                oSampleInvoiceDetail = new SampleInvoiceDetail();
                oSampleInvoiceDetail.ProductID = oItemDOD.ProductID;
                oSampleInvoiceDetail.ProductCode = oItemDOD.ProductCode;
                oSampleInvoiceDetail.ProductName = oItemDOD.ProductName;
                oSampleInvoiceDetail.Qty = oItemDOD.Qty - oItemDOD.POQty;
                oSampleInvoiceDetail.UnitPrice = oItemDOD.UnitPrice;
                oSampleInvoiceDetail.Amount = oSampleInvoiceDetail.Qty * oSampleInvoiceDetail.UnitPrice;
                oSampleInvoiceDetail.SampleInvoiceID = oSampleInvoice.SampleInvoiceID;
                if (oSampleInvoiceDetail.Qty > 0)
                {
                    oSampleInvoiceDetails.Add(oSampleInvoiceDetail);
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSampleInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetsOrderOrders(SampleInvoiceDetail oSampleInvoiceDetail)
        {
            _oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            SampleInvoiceDetail oSIDetail = new SampleInvoiceDetail();
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();

            string sSQL = "Select * from View_DyeingOrder where SampleInvoiceID=0 and paymentType in (" + (int)EnumOrderPaymentType.CashOrCheque + "," + (int)EnumOrderPaymentType.AdjWithNextLC + ") and DyeingorderType in (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.SaleOrder + "," + (int)EnumOrderType.TwistOrder + "," + (int)EnumOrderType.ReConing + "," + (int)EnumOrderType.LoanOrder + "," + (int)EnumOrderType.ClaimOrder + ") and [Status] in (" + (int)EnumDyeingOrderState.Initialized + "," + (int)EnumDyeingOrderState.WatingForApprove + "," + (int)EnumDyeingOrderState.ApprovalDone + "," + (int)EnumDyeingOrderState.Req_ForLabdip + "," + (int)EnumDyeingOrderState.LabDipDone + "," + (int)EnumDyeingOrderState.InProduction + "," + (int)EnumDyeingOrderState.ReceiveInHO + "," + (int)EnumDyeingOrderState.Deliverd + ")";

            if (!String.IsNullOrEmpty(oSampleInvoiceDetail.OrderNo)) sSQL = sSQL + " And RouteSheetNo Like '%" + oSampleInvoiceDetail.OrderNo + "%'";
            if (oSampleInvoiceDetail.ContractorID > 0) sSQL = sSQL + " And ContractorID=" + oSampleInvoiceDetail.ContractorID;
            if (oSampleInvoiceDetail.PaymentType > 0) sSQL = sSQL + " And PaymentType In (" + oSampleInvoiceDetail.PaymentType + ")";
            if (oSampleInvoiceDetail.CurrencyID > 0) sSQL = sSQL + " And CurrencyID In (" + oSampleInvoiceDetail.CurrencyID + ")";
            if (!String.IsNullOrEmpty(oSampleInvoiceDetail.ErrorMessage)) sSQL = sSQL + " And DyeingOrderID not In (" + oSampleInvoiceDetail.ErrorMessage + ")";
            sSQL = sSQL + " order by Convert(int, [dbo].[SplitedStringGet](PONo, '-', 1)) ASC";

            oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach(DyeingOrder oitem in    oDyeingOrders)
            {
                oSIDetail = new SampleInvoiceDetail();
                oSIDetail.OrderNo = oitem.OrderNoFull;
                oSIDetail.OrderType = oitem.DyeingOrderType;
                oSIDetail.DyeingOrderID = oitem.DyeingOrderID;
                oSIDetail.ContractorID = oitem.ContractorID;
                oSIDetail.PaymentType = oitem.PaymentType;
                oSIDetail.OrderTypeSt = oitem.OrderTypeSt;
                oSIDetail.ContractorName = oitem.ContractorName;
                oSIDetail.ContractorPersopnnalID = oitem.ContactPersonnelID;
                oSIDetail.Amount = oitem.Amount;
                oSIDetail.Qty = oitem.Qty;
                _oSampleInvoiceDetails.Add(oSIDetail);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetsOrderOrderDetails(DyeingOrder oDyeingOrder)
        {
            _oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();

            oDyeingOrderDetails = DyeingOrderDetail.Gets(oDyeingOrder.DyeingOrderID,((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach (DyeingOrderDetail oItemDOD in oDyeingOrderDetails)
            {
                oItemDOD.OrderNo = oDyeingOrder.OrderNoFull;
                oItemDOD.OrderDate = oDyeingOrder.OrderDateSt;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetsDyeingOrderReport(SampleInvoice oSampleInvoice)
        {
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            if (oSampleInvoice.SampleInvoiceID > 0)
            {
                oDyeingOrderReports = DyeingOrderReport.Gets(oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrderReports);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion 
        
        #region Printing
        public void PrintToExcelSampleInvoice(int id)
        {
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            _oSampleInvoice = new SampleInvoice();
            //List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetailsTemp = new List<DyeingOrderDetail>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            _oSampleInvoice = _oSampleInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oSampleInvoiceSetup = oSampleInvoiceSetup.GetByBU(_oSampleInvoice.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oSampleInvoice.SampleInvoiceID > 0)
            {
                oDyeingOrders = DyeingOrder.GetsByInvoice(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrderReports = DyeingOrderReport.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oDyeingOrderDetails = DyeingOrderDetail.GetsBy(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //foreach (DyeingOrder oItem in oDyeingOrders)
                //{
                //    List<DyeingOrderDetail> oDODetails = new List<DyeingOrderDetail>();
                //    oDODetails = oDyeingOrderDetails.Where(o => o.DyeingOrderID == oItem.DyeingOrderID).ToList();
                //    foreach (DyeingOrderDetail oItemDOD in oDODetails)
                //    {
                //        oItemDOD.OrderNo = oItem.OrderNoFull;
                //        oItemDOD.OrderDate = oItem.OrderDateSt;
                //        oDyeingOrderDetailsTemp.Add(oItemDOD);
                //    }
                //}
            }

            if (oDyeingOrderReports.Count <= 0)
            {
                _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            if (oSampleInvoiceSetup.PrintNo == (int)EnumExcellColumn.A)
            {
                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 8, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add(oSampleInvoiceSetup.Name);
                    sheet.Name = oSampleInvoiceSetup.Name;

                    sheet.Column(++nColumn).Width = 15; //1
                    sheet.Column(++nColumn).Width = 20; //2
                    sheet.Column(++nColumn).Width = 30; //3
                    sheet.Column(++nColumn).Width = 15; //4
                    sheet.Column(++nColumn).Width = 15; //5
                    sheet.Column(++nColumn).Width = 15; //6
                    sheet.Column(++nColumn).Width = 15; //7

                    cell = sheet.Cells[1, 1, nRowIndex+5, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex + 3, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 4;
                    #endregion

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Note
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oSampleInvoiceSetup.PrintName; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data


                    #region Info Part
                    nColumn = 1;
                    #region 1st Row
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 1]; cell.Value = "DATE"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = _oSampleInvoice.SampleInvoiceDateST; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;

                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 1]; cell.Value = "Debit Note No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = _oSampleInvoice.InvoiceNo; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion
                    nColumn = 1;
                    #region 2nd Row
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 1]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = _oSampleInvoice.ContractorName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++; nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 1]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = _oSampleInvoice.MKTPName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion
                    nColumn = 1;
                    #region 3rd Row
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 1]; cell.Value = "Mode Of Payment"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol];
                    string sTemp = "";
                    string sTempTwo = "";
                    if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
                    {
                        sTemp = "Cheque or Cash";
                        if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                        {
                            sTempTwo = "MR No:" + _oSampleInvoice.ExportPINo;
                        }
                    }
                    if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
                    {
                        sTemp = "NEXT L/C ADJUST ";

                        if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                        {
                            sTempTwo = "P/I No:" + _oSampleInvoice.ExportPINo;
                        }
                    }
                    if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.AdjWithPI)
                    {
                        sTemp = "Adj With PI ";
                        if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                        {
                            sTempTwo = "P/I No:" + _oSampleInvoice.ExportPINo;
                        }
                    }
                    if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.FoC)
                    {
                        sTemp = "Free Of Cost";
                        if (!string.IsNullOrEmpty(_oSampleInvoice.ExportPINo))
                        {
                            sTempTwo = "MR No:" + _oSampleInvoice.ExportPINo;
                        }
                    }

                    cell.Value = sTemp + " " + sTempTwo; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //nRowIndex++;
                    //cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 3]; cell.Value = _oSampleInvoice.SampleInvoiceDateST; cell.Style.Font.Bold = true;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    nRowIndex++;
                    #endregion
                    nColumn = 1;

                    #region 4th Row
                    //cell = sheet.Cells[nRowIndex, nColumn += 5]; cell.Value = "Payment Term"; cell.Style.Font.Bold = true;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 3]; cell.Value = _oSampleInvoice.PaymentTypeSt; cell.Style.Font.Bold = true;
                    //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //nRowIndex++;
                    #endregion
                    #endregion

                    int nProductID = 0;
                    double _nTotalQty = 0;
                    double _nTotalAmount = 0;
                    int _nCount = 0;

                    if (oDyeingOrderReports.Count > 0)
                    {
                        List<DyeingOrderReport> _oDyeingOrderDetails_OrderByProduct = new List<DyeingOrderReport>();
                        _oDyeingOrderDetails_OrderByProduct = oDyeingOrderReports.OrderBy(o => o.ProductID).ToList();

                        List<DyeingOrderReport> _oDyeingOrderDetails_Product = new List<DyeingOrderReport>();
                        List<DyeingOrderReport> _oDyeingOrderDetails_Temp = new List<DyeingOrderReport>();
                        DyeingOrderReport oDyeingOrderDetail_Temp = new DyeingOrderReport();
                        foreach (DyeingOrderReport oItem in oDyeingOrderReports)
                        {
                            if (nProductID != oItem.ProductID)
                            {
                                _oDyeingOrderDetails_Temp = oDyeingOrderReports.Where(o => o.ProductID == oItem.ProductID).ToList();

                                oDyeingOrderDetail_Temp = new DyeingOrderReport();
                                oDyeingOrderDetail_Temp.OrderNo = oItem.OrderNo;
                                oDyeingOrderDetail_Temp.ProductID = oItem.ProductID;
                                oDyeingOrderDetail_Temp.ProductName = oItem.ProductName;
                                oDyeingOrderDetail_Temp.Qty = _oDyeingOrderDetails_Temp.Select(c => c.Qty).Sum();
                                _nTotalAmount = _oDyeingOrderDetails_Temp.Select(c => c.Qty * c.UnitPrice).Sum();
                                if (oDyeingOrderDetail_Temp.Qty > 0)
                                {
                                    oDyeingOrderDetail_Temp.UnitPrice = _nTotalAmount / oDyeingOrderDetail_Temp.Qty;
                                }
                                else
                                {
                                    oDyeingOrderDetail_Temp.UnitPrice = 0;
                                }
                                _oDyeingOrderDetails_Product.Add(oDyeingOrderDetail_Temp);
                                _nTotalQty = 0;
                                _nTotalAmount = 0;
                            }
                            nProductID = oItem.ProductID;
                        }

                        if (_oDyeingOrderDetails_Product.Count > 0)
                        {
                            #region Header Column
                            nColumn = 1;
                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SAMPLE NO"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "YARN TYPE"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "QTY "; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "DELIVERY QTY "; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "U/P(" + _oSampleInvoice.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount(" + _oSampleInvoice.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn];
                            if (_oSampleInvoice.ConversionRate <= 1)
                            {
                                cell.Value = "REMARKS ";
                            }
                            else
                            {
                                cell.Value = "Amount(" + _oSampleInvoice.ExchangeCurrencySymbol + ")";
                            }
                            cell.Style.Font.Bold = true; fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            nRowIndex++;
                            #endregion

                            foreach (DyeingOrderReport oItem in _oDyeingOrderDetails_Product)
                            {
                                _nCount++;
                                #region Data
                                nColumn = 1;
                                nCount++;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _nCount.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "# #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "# #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "" + oItem.UnitPrice; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice * _oSampleInvoice.ConversionRate); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex++;
                                #endregion
                                _nTotalQty = _nTotalQty + oItem.Qty;
                                _nTotalAmount = _nTotalAmount + oItem.Qty * oItem.UnitPrice;
                            }

                            #region Total
                            nColumn = 1;
                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 2]; cell.Value = " Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oDyeingOrderDetails_Product.Sum(x => x.Qty); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oDyeingOrderDetails_Product.Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oDyeingOrderDetails_Product.Sum(x => x.Qty * x.UnitPrice * _oSampleInvoice.ConversionRate); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            nRowIndex++;
                            #endregion

                            #region Total In Words
                            nColumn = 1;
                            if ((EnumOrderPaymentType)_oSampleInvoice.PaymentType == EnumOrderPaymentType.CashOrCheque)
                            {
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "In Words"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = Global.TakaWords(Math.Round((_oDyeingOrderDetails_Product.Sum(x => x.Qty * x.UnitPrice) * _oSampleInvoice.ConversionRate), 2)); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true; 
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                            }
                            else
                            {
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "In Words"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = Global.DollarWords(_oDyeingOrderDetails_Product.Sum(x => x.Qty * x.UnitPrice)); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                            }
                            nColumn = 1;
                            #endregion

                        }//_oDyeingOrderDetails_Product.Count > 0

                    }
                    else 
                    {
                        if (_oSampleInvoice.SampleInvoiceDetails.Count > 0)
                        {
                            #region Header Column
                            nColumn = 1;
                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL#"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SAMPLE NO"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PRODUCT DESCRIPTION"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "QTY"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = _oSampleInvoice.RateUnit > 1 ? "U/P(" + _oSampleInvoice.CurrencySymbol + ")/" + _oSampleInvoice.RateUnit : "U/P(" + _oSampleInvoice.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Amount(" + _oSampleInvoice.CurrencySymbol + ")"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn];
                            if (_oSampleInvoice.ConversionRate <= 1)
                            {
                                cell.Value = "REMARKS ";
                            }
                            else
                            {
                                cell.Value = "Amount(" + _oSampleInvoice.ExchangeCurrencySymbol + ")";
                            }
                            cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            nRowIndex++;
                            #endregion

                            foreach (SampleInvoiceDetail oItem in _oSampleInvoice.SampleInvoiceDetails)
                            {
                                _nCount++;
                                #region Data
                                nColumn = 1;
                                nCount++;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _nCount.ToString(); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.OrderNo; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "# #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "" + oItem.UnitPrice; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice * _oSampleInvoice.ConversionRate); cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex++;
                                #endregion
                                _nTotalQty = _nTotalQty + oItem.Qty;
                                _nTotalAmount = _nTotalAmount + oItem.Qty * oItem.UnitPrice;
                            }

                            #region Total
                            nColumn = 1;
                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 2]; cell.Value = " Total :"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oSampleInvoice.SampleInvoiceDetails.Sum(x => x.Qty); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oSampleInvoice.SampleInvoiceDetails.Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oSampleInvoice.SampleInvoiceDetails.Sum(x => x.Qty * x.UnitPrice * _oSampleInvoice.ConversionRate); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.LightGray);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            nRowIndex++;
                            #endregion

                            #region Total In Words
                            nColumn = 1;
                            if ((EnumOrderPaymentType)_oSampleInvoice.PaymentType == EnumOrderPaymentType.CashOrCheque)
                            {
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "In Words"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = Global.TakaWords(Math.Round((_oSampleInvoice.SampleInvoiceDetails.Sum(x => x.Qty * x.UnitPrice) * _oSampleInvoice.ConversionRate), 2)); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                            }
                            else
                            {
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "In Words"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = Global.DollarWords(_oSampleInvoice.SampleInvoiceDetails.Sum(x => x.Qty * x.UnitPrice)); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                            }
                            nColumn = 1;
                            #endregion
                        }//_oDyeingOrderDetails_Product.Count > 0
                    }

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Footer
                    nColumn = 1; //1st
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 1]; cell.Value =  "Prepared By"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Accounts"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = "Marketing Manager"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = "Managing Director"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;

                    nColumn = 1; //3rd
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex+2, nColumn += 1]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + 2, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + 2, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex += 2, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;

                    #endregion

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion


                    if (_oSampleInvoice.PaymentType == (int)EnumOrderPaymentType.AdjWithNextLC)
                    {
                        #region 1st Row
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = "This value will be adjusted with immediate next L/C of our company, irrespective of Yarn, Buyer and Merchandiser."; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nRowIndex++;
                        #endregion

                        #region Blank
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion


                        #region 2nd Row
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = "Approved By:"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nRowIndex++;
                        #endregion
                        #region Blank
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion
                        #region 3rd Row
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = "Full Name  :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nRowIndex++;
                        #endregion

                        #region Blank
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;
                        #endregion
                        #region 4th Row
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = "Seal and signature of management"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        nRowIndex++;
                        #endregion
                    }

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex+2, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion
                    //cell = sheet.Cells[2, nRowIndex, nRowIndex+3, nEndCol];
                    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    //fill.BackgroundColor.SetColor(Color.White);

                    nEndRow = nRowIndex;
                    nRowIndex++;
                    #endregion
                    
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=" + oSampleInvoiceSetup.Name + ".xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (oSampleInvoiceSetup.PrintNo == (int)EnumExcellColumn.B)
            {

                oDyeingOrderReports = DyeingOrderReport.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Print XL

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 10, nColumn = 1, nCount = 0;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add(oSampleInvoiceSetup.Name);
                    sheet.Name = oSampleInvoiceSetup.Name;

                    sheet.Column(++nColumn).Width = 15; //1
                    sheet.Column(++nColumn).Width = 20; //2
                    sheet.Column(++nColumn).Width = 20; //3
                    sheet.Column(++nColumn).Width = 25; //4
                    sheet.Column(++nColumn).Width = 30; //5
                    sheet.Column(++nColumn).Width = 12; //6
                    sheet.Column(++nColumn).Width = 15; //7
                    sheet.Column(++nColumn).Width = 12; //8
                    sheet.Column(++nColumn).Width = 15; //9
                    //nCol = 14;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex+3, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 4;
                    #endregion

                    #region Blank
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Note
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oSampleInvoiceSetup.PrintName; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Report Data

                    #region Info Part 
                    nColumn = 1;
                    #region 1st Row
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 2]; cell.Value = _oSampleInvoice.ContractorName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Bill No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 3]; cell.Value = oSampleInvoiceSetup.Code + "-" + _oSampleInvoice.InvoiceNo; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion
                    nColumn = 1;
                    #region 2nd Row
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Concern Person"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 2]; cell.Value = _oSampleInvoice.ContractorPersopnnalName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "PI No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 3]; cell.Value = _oSampleInvoice.ExportPINo; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion
                    nColumn = 1;
                    #region 3rd Row
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + 1, nColumn]; cell.Value = "Delivery Place"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top ; 
                    cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex + 1, nColumn += 2]; cell.Value = _oSampleInvoice.ContractorAddress; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 3]; cell.Value = _oSampleInvoice.SampleInvoiceDateST; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion
                    nColumn = 1;
                    #region 4th Row
                    cell = sheet.Cells[nRowIndex, nColumn += 5]; cell.Value = "Payment Term"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 3]; cell.Value = _oSampleInvoice.PaymentTypeSt; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion
                    #endregion

                    string _sMunit, _sCurrency;
                    #region SaleOrder
                    List<DyeingOrderReport> oDyeingSaleOrderReports = new List<DyeingOrderReport>();
                    oDyeingSaleOrderReports = oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SaleOrder).ToList();
                    if (oDyeingSaleOrderReports.Count > 0)
                    {
                        _sMunit = oDyeingSaleOrderReports[0].MUName;
                        _sCurrency = _oSampleInvoice.CurrencySymbol;
                    }
                    if (oDyeingSaleOrderReports.Count > 0)
                    {
                        #region SaleOrder Details

                        #region Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oDyeingSaleOrderReports[0].OrderTypeSt; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        int nDyeingOrderID = 0;
                        double nQty, nAmount, nTotalAmount, nTotalQty = 0;

                        foreach (DyeingOrderReport oItem in oDyeingSaleOrderReports)
                        {
                            if (nDyeingOrderID != oItem.DyeingOrderID)
                            {
                                #region SubTotal
                                if (nCount > 0)
                                {
                                    nColumn = 1;
                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Sub Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty_DC); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty_RC); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oSampleInvoice.CurrencySymbol + "" + oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";//$ #,##0.00
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                }
                                #endregion
                                #region Header Column
                                nColumn = 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Ref"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shade"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty " + "(" + oDyeingSaleOrderReports[0].MUName + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Delivery Qty " + "(" + oDyeingSaleOrderReports[0].MUName + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Return Qty " + "(" + oDyeingSaleOrderReports[0].MUName + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "U.P"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nDyeingOrderID = oItem.DyeingOrderID;
                            #region Data
                            nColumn = 1;
                            nCount++;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.OrderNo.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            if (!String.IsNullOrEmpty(oItem.StyleNo))
                            {
                                 oItem.BuyerRef= oItem.BuyerRef+", "+oItem.StyleNo;
                            }
                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.BuyerRef; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.CPName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn];
                            if (oItem.RGB == "" || oItem.RGB == null)
                            {
                                cell.Value = "" + oItem.ColorNo;
                            }
                            else
                            {
                                cell.Value = "" + oItem.RGB;
                            }
                            cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_DC; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_RC; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "" + oItem.UnitPrice; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                            nRowIndex++;
                            #endregion
                        }
                        #region SubTotal
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Sub Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty_DC); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty_RC); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "" + oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Total
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty_DC); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty_RC); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value =  "" + oDyeingSaleOrderReports.Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #endregion
                    }
                    #endregion

                    #region Sample Order
                   oDyeingSaleOrderReports = new List<DyeingOrderReport>();
                   oDyeingSaleOrderReports = oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.SampleOrder).ToList();
                    if (oDyeingSaleOrderReports.Count > 0)
                    {
                        #region SaleOrder Details

                        #region Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oDyeingSaleOrderReports[0].OrderTypeSt; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        int nDyeingOrderID = 0;
                        double nQty, nAmount, nTotalAmount, nTotalQty = 0;

                        foreach (DyeingOrderReport oItem in oDyeingSaleOrderReports)
                        {
                            if (nDyeingOrderID != oItem.DyeingOrderID)
                            {
                                #region SubTotal
                                if (nCount > 0)
                                {
                                    nColumn = 1;
                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Sub Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty_DC); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty_RC); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value =  "" + oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                }
                                #endregion
                                #region Header Column
                                nColumn = 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center ;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Ref"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shade"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty " + "(" + oDyeingSaleOrderReports[0].MUName + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Delivery Qty " + "(" + oDyeingSaleOrderReports[0].MUName + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Return Qty " + "(" + oDyeingSaleOrderReports[0].MUName + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "U.P"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nDyeingOrderID = oItem.DyeingOrderID;
                            #region Data
                            nColumn = 1;
                            nCount++;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.OrderNo.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.BuyerRef; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.CPName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn];
                            if (oItem.RGB == "" || oItem.RGB == null)
                            {
                                cell.Value = "" + oItem.ColorNo;
                            }
                            else
                            {
                                cell.Value = "" + oItem.RGB;
                            }
                            cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_DC; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_RC; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value =  oItem.UnitPrice; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndRow = nRowIndex;
                            nRowIndex++;
                            #endregion
                        }
                        #region SubTotal
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Sub Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty_DC); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty_RC); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Total
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty_DC); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty_RC); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #endregion
                    }
                    #endregion
                    #region ReConing
                    oDyeingSaleOrderReports = new List<DyeingOrderReport>();
                    oDyeingSaleOrderReports = oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.ReConing).ToList();
                    if (oDyeingSaleOrderReports.Count > 0)
                    {

                        #region SaleOrder Details

                        #region Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oDyeingSaleOrderReports[0].OrderTypeSt; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        int nDyeingOrderID = 0;
                        double nQty, nAmount, nTotalAmount, nTotalQty = 0;

                        foreach (DyeingOrderReport oItem in oDyeingSaleOrderReports)
                        {
                            if (nDyeingOrderID != oItem.DyeingOrderID)
                            {
                                #region SubTotal
                                if (nCount > 0)
                                {
                                    nColumn = 1;
                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Sub Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "" + oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                }
                                #endregion
                                #region Header Column
                                nColumn = 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Ref"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shade"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty " + "(" + oDyeingSaleOrderReports[0].MUName + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "U.P"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nDyeingOrderID = oItem.DyeingOrderID;
                            #region Data
                            nColumn = 1;
                            nCount++;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.OrderNo.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.BuyerRef; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.CPName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn];
                            if (oItem.RGB == "" || oItem.RGB == null)
                            {
                                cell.Value = "" + oItem.ColorNo;
                            }
                            else
                            {
                                cell.Value = "" + oItem.RGB;
                            }
                            cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndRow = nRowIndex;
                            nRowIndex++;
                            #endregion
                        }
                        #region SubTotal
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Sub Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Total
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #endregion
                    }
                    #endregion
                    #region TwistOrder
                    oDyeingSaleOrderReports = new List<DyeingOrderReport>();
                    oDyeingSaleOrderReports = oDyeingOrderReports.Where(o => o.DyeingOrderType == (int)EnumOrderType.TwistOrder).ToList();
                    if (oDyeingSaleOrderReports.Count > 0)
                    {
                        #region SaleOrder Details

                        #region Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oDyeingSaleOrderReports[0].OrderTypeSt; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        int nDyeingOrderID = 0;
                        double nQty, nAmount, nTotalAmount, nTotalQty = 0;

                        foreach (DyeingOrderReport oItem in oDyeingSaleOrderReports)
                        {
                            if (nDyeingOrderID != oItem.DyeingOrderID)
                            {
                                #region SubTotal
                                if (nCount > 0)
                                {
                                    nColumn = 1;
                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Sub Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "" + oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                }
                                #endregion
                                #region Header Column
                                nColumn = 1;
                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Buyer Ref"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Color"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Yarn Type"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Shade"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Qty " + "(" + oDyeingSaleOrderReports[0].MUName + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "U.P"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Value"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nDyeingOrderID = oItem.DyeingOrderID;
                            #region Data
                            nColumn = 1;
                            nCount++;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.OrderNo.ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.BuyerRef; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.CPName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn];
                            if (oItem.RGB == "" || oItem.RGB == null)
                            {
                                cell.Value = "" + oItem.ColorNo;
                            }
                            else
                            {
                                cell.Value = "" + oItem.RGB;
                            }
                            cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = "# #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nEndRow = nRowIndex;
                            nRowIndex++;
                            #endregion
                        }
                        #region SubTotal
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Sub Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Where(x => x.DyeingOrderID == nDyeingOrderID).Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Total
                        nColumn = 1;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 5]; cell.Value = " Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingSaleOrderReports.Sum(x => x.Qty * x.UnitPrice); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion

                        #endregion
                    }
                    #endregion
                    nRowIndex++;

                    #region Summary
                    nColumn = 2;
                    
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn+3]; cell.Value = "Summary"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center ;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;

                    nColumn = 2;
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = "Order Qty" + ( oDyeingOrderReports!=null?"" :" ("+oDyeingOrderReports[0].MUName + ")"); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center ;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Total Value"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center ;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;

                    if (oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => x.Qty)>0)
                    {
                        nColumn = 2;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Total Bulk Qty"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => x.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => (x.Qty * x.UnitPrice)); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " # #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    if (oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => x.Qty) > 0)
                    {
                        nColumn = 2;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Total Sample Qty"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => x.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.SampleOrder).Sum(x => (x.Qty * x.UnitPrice)); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " # #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    if (oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ReConing).Sum(x => x.Qty) > 0)
                    {
                        nColumn = 2;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Total ReConing Qty"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ReConing).Sum(x => x.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.ReConing).Sum(x => (x.Qty * x.UnitPrice)); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " # #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    if (oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.TwistOrder).Sum(x => x.Qty) > 0)
                    {
                        nColumn = 2;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Total TwistOrder Qty"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.TwistOrder).Sum(x => x.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oDyeingOrderReports.Where(c => c.DyeingOrderType == (int)EnumOrderType.TwistOrder).Sum(x => (x.Qty * x.UnitPrice)); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " # #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    if (_oSampleInvoice.Charge> 0)
                    {
                        nColumn = 2;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Add Charge"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oSampleInvoice.Charge; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " # #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    if (_oSampleInvoice.Discount > 0)
                    {
                        nColumn = 2;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Discount"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = _oSampleInvoice.Discount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " # #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    if (oDyeingOrderReports.Select(c => c.Qty).Sum()> 0)
                    {
                        nColumn = 2;
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "Grand Total "; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = oDyeingOrderReports.Sum(x => x.Qty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = "# #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = (Math.Round(_oSampleInvoice.Charge,2) + oDyeingOrderReports.Sum(x => (x.Qty * x.UnitPrice)) -Math.Round(_oSampleInvoice.Discount,2)); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Numberformat.Format = _oSampleInvoice.CurrencySymbol + " # #,##0.00";
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total In Words
                    nColumn = 1;
                    if ((EnumOrderPaymentType)_oSampleInvoice.PaymentType == EnumOrderPaymentType.CashOrCheque)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "In Words"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex,nEndCol]; cell.Value = Global.TakaWords(Math.Round((oDyeingOrderReports.Sum(x => x.Qty* x.UnitPrice) * _oSampleInvoice.ConversionRate), 2)); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;                    
                    }
                    else
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "In Words"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = Global.DollarWords(oDyeingOrderReports.Sum(x => x.Qty * x.UnitPrice)); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = _oSampleInvoice.Remark; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Footer 
                    nRowIndex++;
                    nColumn = 1; //1st
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn+=2]; cell.Value = _oSampleInvoice.PreparebyName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    nRowIndex++;


                    nColumn = 1; //2nd
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 2]; cell.Value = "_______________"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = "_______________"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = "_______________"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    nRowIndex++;

                    nColumn = 1; //3rd
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn += 2]; cell.Value = "Prepared By"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, ++nColumn]; cell.Value = "Checked By"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nEndCol]; cell.Value = "Approved By"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    nRowIndex++;
  
                    #endregion



                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex += 2, nColumn]; cell.Value = "";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    nEndRow = nRowIndex;
                    nRowIndex++;
                    #endregion
                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=" + oSampleInvoiceSetup.Name+ ".xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public ActionResult PrintSampleInvoice(int id, bool bQtyFormat, int nTitleTypeInImg)
        {
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            _oSampleInvoice = new SampleInvoice();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetailsTemp = new List<DyeingOrderDetail>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            _oSampleInvoice = _oSampleInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oSampleInvoiceSetup = oSampleInvoiceSetup.GetByBU(_oSampleInvoice.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oSampleInvoice.SampleInvoiceID > 0)
            {

                oDyeingOrders = DyeingOrder.GetsByInvoice(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrderDetails = DyeingOrderDetail.GetsBy(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (DyeingOrder oItem in oDyeingOrders)
                {
                    List<DyeingOrderDetail> oDODetails = new List<DyeingOrderDetail>();
                    oDODetails = oDyeingOrderDetails.Where(o => o.DyeingOrderID == oItem.DyeingOrderID).ToList();
                    foreach (DyeingOrderDetail oItemDOD in oDODetails)
                    {
                        oItemDOD.OrderNo = oItem.OrderNoFull;
                        oItemDOD.OrderDate = oItem.OrderDateSt;
                        oDyeingOrderDetailsTemp.Add(oItemDOD);
                    }
                }
            }

            if(oDyeingOrderDetails.Count<=0)
            {
                _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
        
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oSampleInvoiceSetup.PrintNo == (int)EnumExcellColumn.A)
            {
                rptSampleInvoice oReport = new rptSampleInvoice();
                byte[] abytes = oReport.PrepareReport(_oSampleInvoice, oCompany, oDyeingOrderDetails, oBusinessUnit, true, oSampleInvoiceSetup, nTitleTypeInImg, oDyeingOrders);
                return File(abytes, "application/pdf");
            }
            else if (oSampleInvoiceSetup.PrintNo == (int)EnumExcellColumn.B)
            {
                oDyeingOrderReports = DyeingOrderReport.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                rptSampleInvoice oReport = new rptSampleInvoice();
                byte[] abytes = oReport.PrepareReport_DyeingBill(_oSampleInvoice, oCompany, oBusinessUnit, oDyeingOrderReports, oSampleInvoiceSetup,    nTitleTypeInImg);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptSampleInvoice oReport = new rptSampleInvoice();
                byte[] abytes = oReport.PrepareReport(_oSampleInvoice, oCompany, oDyeingOrderDetails, oBusinessUnit, false, oSampleInvoiceSetup, nTitleTypeInImg, oDyeingOrders);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintSampleInvoice_F2(int id, bool bQtyFormat, int nTitleTypeInImg, int nPageHeight)
        {
            SampleInvoiceSetup oSampleInvoiceSetup = new SampleInvoiceSetup();
            _oSampleInvoice = new SampleInvoice();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetailsTemp = new List<DyeingOrderDetail>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            List<SampleInvoiceCharge> oSampleInvoiceCharges = new List<SampleInvoiceCharge>();

            _oSampleInvoice = _oSampleInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oSampleInvoiceSetup = oSampleInvoiceSetup.GetByBU(_oSampleInvoice.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oSampleInvoice.SampleInvoiceID > 0)
            {
              //  _sSql = "SELECT * FROM View_SampleInvoiceCharge Where SampleInvoiceID = " + id + " Order By SampleInvoiceChargeID";
                oSampleInvoiceCharges = SampleInvoiceCharge.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oSampleInvoice.SampleInvoiceCharges = oSampleInvoiceCharges;
                oDyeingOrders = DyeingOrder.GetsByInvoice(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrderDetails = DyeingOrderDetail.GetsBy(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (DyeingOrder oItem in oDyeingOrders)
                {
                    List<DyeingOrderDetail> oDODetails = new List<DyeingOrderDetail>();
                    oDODetails = oDyeingOrderDetails.Where(o => o.DyeingOrderID == oItem.DyeingOrderID).ToList();
                    foreach (DyeingOrderDetail oItemDOD in oDODetails)
                    {
                        oItemDOD.OrderNo = oItem.OrderNoFull;
                        oItemDOD.OrderDate = oItem.OrderDateSt;
                        oDyeingOrderDetailsTemp.Add(oItemDOD);
                    }
                }
            }

            if (oDyeingOrderDetails.Count <= 0)
            {
                _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oSampleInvoiceSetup.PrintNo == (int)EnumExcellColumn.A)
            {
                //rptSampleInvoice_F2 oReport = new rptSampleInvoice_F2();
                //byte[] abytes = oReport.PrepareReport(_oSampleInvoice, oCompany, oDyeingOrderDetails, oBusinessUnit, true, oSampleInvoiceSetup, nTitleTypeInImg);
                //return File(abytes, "application/pdf");
                rptSampleInvoice oReport = new rptSampleInvoice();
                byte[] abytes = oReport.PrepareReport(_oSampleInvoice, oCompany, oDyeingOrderDetails, oBusinessUnit, true, oSampleInvoiceSetup, nTitleTypeInImg, oDyeingOrders);
                return File(abytes, "application/pdf");

            }
            else if (oSampleInvoiceSetup.PrintNo == (int)EnumExcellColumn.B)
            {
                oDyeingOrderReports = DyeingOrderReport.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                rptSampleInvoice oReport = new rptSampleInvoice();
                byte[] abytes = oReport.PrepareReport_DyeingBillV2(_oSampleInvoice, oCompany, oBusinessUnit, oDyeingOrderReports, oSampleInvoiceSetup, nTitleTypeInImg, nPageHeight);
                return File(abytes, "application/pdf");
            }
            else if (oSampleInvoiceSetup.PrintNo == (int)EnumExcellColumn.C)
            {
               

                oDyeingOrderReports = DyeingOrderReport.Gets(_oSampleInvoice.SampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                rptSampleInvoice_F2 oReport = new rptSampleInvoice_F2();
                byte[] abytes = oReport.PrepareReport_DyeingBill(_oSampleInvoice, oCompany, oBusinessUnit, oDyeingOrderReports, oSampleInvoiceSetup, nTitleTypeInImg, nPageHeight);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptSampleInvoice_F2 oReport = new rptSampleInvoice_F2();
                byte[] abytes = oReport.PrepareReport(_oSampleInvoice, oCompany, oDyeingOrderDetails, oBusinessUnit, false, oSampleInvoiceSetup, nTitleTypeInImg);
                return File(abytes, "application/pdf");
            }
        }
        public ActionResult PrintSampleInvoice_AdjInvoice(int id, bool bIsSample)
        {
            _oSampleInvoice = new SampleInvoice();
            ExportSCDO oExportSCDO = new ExportSCDO();
            _oSampleInvoice = _oSampleInvoice.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oSampleInvoice.SampleInvoiceID > 0)
            {
                _oSampleInvoice.SampleInvoiceDetails = SampleInvoiceDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            if (_oSampleInvoice.ExportPIID > 0)
            {
                oExportSCDO = oExportSCDO.GetByPI(_oSampleInvoice.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }


            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);


            rptSampleInvoice oReport = new rptSampleInvoice();

            byte[] abytes = oReport.PrepareReport_AdjInvoice(_oSampleInvoice, oCompany, oExportSCDO, oBusinessUnit, bIsSample);

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
  
        private string FillImageUrl(string sImageName)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            return url + "Content/" + sImageName;
        }

        [HttpPost]
        public ActionResult SetSampleInvoiceListData(SampleInvoice oSampleInvoice)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSampleInvoice);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintSampleInvoiceDetails()
        {
            _oSampleInvoice = new SampleInvoice();
            try
            {
                _oSampleInvoice = (SampleInvoice)Session[SessionInfo.ParamObj];
                _oSampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice WHERE SampleInvoiceID IN (" + _oSampleInvoice.ErrorMessage + ") Order By SampleInvoiceID", (int)Session[SessionInfo.currentUserID]);
                _oSampleInvoiceDetails = SampleInvoiceDetail.Gets("SELECT * FROM View_SampleInvoiceDetail WHERE SampleInvoiceID IN (" + _oSampleInvoice.ErrorMessage + ") Order By SampleInvoiceID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleInvoice = new SampleInvoice();
                _oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oSampleInvoice.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(_oSampleInvoice.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            rptSampleInvoiceDetails oReport = new rptSampleInvoiceDetails();
            byte[] abytes = oReport.PrepareReport(_oSampleInvoices, _oSampleInvoiceDetails, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ExportToExcel()
        {
            _oSampleInvoice = new SampleInvoice();
            try
            {
                _oSampleInvoice = (SampleInvoice)Session[SessionInfo.ParamObj];
                _oSampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice WHERE SampleInvoiceID IN (" + _oSampleInvoice.ErrorMessage + ") Order By SampleInvoiceID", (int)Session[SessionInfo.currentUserID]);
                _oSampleInvoiceDetails = SampleInvoiceDetail.Gets("SELECT * FROM View_SampleInvoiceDetail WHERE SampleInvoiceID IN (" + _oSampleInvoice.ErrorMessage + ") Order By SampleInvoiceID", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleInvoice = new SampleInvoice();
                _oSampleInvoiceDetails = new List<SampleInvoiceDetail>();
            }

            if (_oSampleInvoices.Count > 0)
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oSampleInvoice.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(_oSampleInvoice.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                int count = 0, nStartCol = 2, nTotalCol = 0;

                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sample Invoice Details");
                    sheet.Name = "Sample Invoice Details";
                    sheet.Column(nStartCol++).Width = 5; //SL
                    sheet.Column(nStartCol++).Width = 18; //SAN no
                    sheet.Column(nStartCol++).Width = 12; //Date
                    sheet.Column(nStartCol++).Width = 30; //Buyer
                    sheet.Column(nStartCol++).Width = 25; //type
                    sheet.Column(nStartCol++).Width = 18; //PI no
                    sheet.Column(nStartCol++).Width = 40; //Product
                    sheet.Column(nStartCol++).Width = 15; // qty
                    sheet.Column(nStartCol++).Width = 12; //unit price
                    sheet.Column(nStartCol++).Width = 27; //Amount

                    nTotalCol = nStartCol;
                    nStartCol = 2;

                    #region Report Header
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, nTotalCol]; cell.Merge = true; cell.Value = "Sample Invoice Details"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "SAN No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Buyer"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "PI No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "U. Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = "Amount"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region data
                    int nCount = 0;
                    foreach (SampleInvoice oItem in _oSampleInvoices)
                    {
                        List<SampleInvoiceDetail> oTempSampleInvoiceDetails = new List<SampleInvoiceDetail>();
                        oTempSampleInvoiceDetails = _oSampleInvoiceDetails.Where(x => x.SampleInvoiceID == oItem.SampleInvoiceID).ToList();
                        int rowCount = (oTempSampleInvoiceDetails.Count() - 1);
                        if (rowCount <= 0) rowCount = 0;
                        nStartCol = 2;

                        #region main object
                        nCount++;
                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = nCount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.SampleInvoiceNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.SampleInvoiceDateST; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.InvoiceTypeSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, nStartCol, (rowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = oItem.ExportPINo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion

                        #region Detail
                        if (oTempSampleInvoiceDetails.Count > 0)
                        {
                            foreach (SampleInvoiceDetail oItemDetail in oTempSampleInvoiceDetails)
                            {
                                nStartCol = 8;
                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.Qty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.UnitPrice; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = oItemDetail.Amount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                rowIndex++;
                            }
                        }
                        else
                        {
                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, nStartCol++]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex++;
                        }
                        #endregion

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    cell = sheet.Cells[rowIndex, nStartCol, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = _oSampleInvoiceDetails.Sum(x => x.Qty); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = _oSampleInvoiceDetails.Sum(x => x.Amount); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Sample_Invoice_Details.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }
                #endregion

        }

        #endregion

        #region Search
        [HttpPost]
        public JsonResult AdvSearch(SampleInvoice oSampleInvoice)
        {
            _oSampleInvoices = new List<SampleInvoice>();
            try
            {
                string sSQL = MakeSQL(oSampleInvoice);
                _oSampleInvoices = SampleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleInvoices = new List<SampleInvoice>();
                //_oSampleInvoice.ErrorMessage = ex.Message;
                //_oSampleInvoices.Add(_oSampleInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(SampleInvoice oSampleInvoice)
        {
            string sParams = oSampleInvoice.ErrorMessage;
            string sDeliveryToName = "";
            int nCboOrderDate = 0;
            DateTime dFromOrderDate = DateTime.Today;
            DateTime dToOrderDate = DateTime.Today;
            int nCboInvoiceDate = 0;
            DateTime dFromInvoiceDate = DateTime.Today;
            DateTime dToInvoiceDate = DateTime.Today;
            int nCboDeliveryDate = 0;
            DateTime dFromDeliveryDate = DateTime.Today;
            DateTime dToDeliveryDate = DateTime.Today;

            int nCboMkPerson = 0;
            int nPaymentType = 0;
            int nOrderType = 0;

            string sProductIDs = "";

            string sPINo = "";
            string sInvoiceNo = "";
            string sOrderNo = "";
            string sRefNo = "";

            bool bYetNotAttachWithSalesContract = false;
            bool bYetNotReceiveLC = false;
            bool bchkYetNotAssignOrder = false;
            int nInvoiceTypeFixec = 0;
            int nInvoiceType = 0;
            int nBUID = 0;
            int nProductNature = 0;
            bool bchkYetNotAssignMKT = false;


            if (!string.IsNullOrEmpty(sParams))
            {
                _oSampleInvoice.ContractorName = Convert.ToString(sParams.Split('~')[0]);
                sDeliveryToName = Convert.ToString(sParams.Split('~')[1]);

                nCboOrderDate = Convert.ToInt32(sParams.Split('~')[2]);
                dFromOrderDate = Convert.ToDateTime(sParams.Split('~')[3]);
                dToOrderDate = Convert.ToDateTime(sParams.Split('~')[4]);
                nCboInvoiceDate = Convert.ToInt32(sParams.Split('~')[5]);
                dFromInvoiceDate = Convert.ToDateTime(sParams.Split('~')[6]);
                dToInvoiceDate = Convert.ToDateTime(sParams.Split('~')[7]);
                nCboDeliveryDate = Convert.ToInt32(sParams.Split('~')[8]);
                dFromDeliveryDate = Convert.ToDateTime(sParams.Split('~')[9]);
                dToDeliveryDate = Convert.ToDateTime(sParams.Split('~')[10]);

                nCboMkPerson = Convert.ToInt32(sParams.Split('~')[11]);
                nPaymentType = Convert.ToInt32(sParams.Split('~')[12]);
                nOrderType = Convert.ToInt32(sParams.Split('~')[13]);

                sPINo = Convert.ToString(sParams.Split('~')[14]);
                sInvoiceNo = Convert.ToString(sParams.Split('~')[15]);
                sOrderNo = Convert.ToString(sParams.Split('~')[16]);
                sRefNo = Convert.ToString(sParams.Split('~')[17]);
                sProductIDs = Convert.ToString(sParams.Split('~')[18]);

                bYetNotAttachWithSalesContract = Convert.ToBoolean(sParams.Split('~')[19]);
                bYetNotReceiveLC = Convert.ToBoolean(sParams.Split('~')[20]);

                nInvoiceTypeFixec = Convert.ToInt16(sParams.Split('~')[21]);
                nInvoiceType = Convert.ToInt16(sParams.Split('~')[22]);
                nBUID = Convert.ToInt32(sParams.Split('~')[23]);
                bchkYetNotAssignOrder = Convert.ToBoolean(sParams.Split('~')[24]);
                nProductNature = Convert.ToInt32(sParams.Split('~')[25]);

                //bchkYetNotAssignMKT = false;
                //if (sParams.Split('~').Length > 26)
                //    bool.TryParse(sParams.Split('~')[26], out bchkYetNotAssignMKT);
                
            }


            string sReturn1 = "SELECT * FROM View_SampleInvoice ";
            string sReturn = "";

            #region Contractor
            if (!String.IsNullOrEmpty(_oSampleInvoice.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oSampleInvoice.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(sDeliveryToName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SampleInvoiceID in (Select DyeingOrderID from DyeingOrder where DeliveryToID in (" + _oSampleInvoice.ContractorName + "))";
            }
            #endregion

            #region Sample Invoice Date
            if (nCboInvoiceDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboInvoiceDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboInvoiceDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboInvoiceDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboInvoiceDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DO.OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboInvoiceDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboInvoiceDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SampleInvoiceDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Order Date
            if (nCboOrderDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (nCboOrderDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where  CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) )";
                }

                else if (nCboOrderDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (nCboOrderDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  )";
                }

                else if (nCboOrderDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboOrderDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where  CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboOrderDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where  CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106))  )";
                }

            }
            #endregion

            #region Delivery Date
            if (nCboDeliveryDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (nCboDeliveryDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoiceDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) )";
                }

                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoiceDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoiceDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }

                else if (nCboDeliveryDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoiceDetail where CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoiceDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from SampleInvoiceDetail where  CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106))  )";
                }

            }
            #endregion

       

            #region nPayment Type
            if (nPaymentType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "PaymentType = " + nPaymentType;
            }
            #endregion
            #region Mkt. Person
            if (nCboMkPerson > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MKTEmpID = " + nCboMkPerson;
            }
            #endregion
            #region Invoice Type Type
            if (nInvoiceTypeFixec == 2)
            {
                Global.TagSQL(ref sReturn);
                if (nInvoiceType > 0)
                {
                    sReturn = sReturn + "InvoiceType in(" + nInvoiceType.ToString()+ ")";
                }
                else
                {
                    sReturn = sReturn + "InvoiceType in(" + (int)EnumSampleInvoiceType.DocCharge + "," + (int)EnumSampleInvoiceType.Adjstment_Qty + "," + (int)EnumSampleInvoiceType.Adjstment_Value + "," + (int)EnumSampleInvoiceType.Adjstment_Commission + "," + (int)EnumSampleInvoiceType.ReturnAdjustment + ")";
                }
            }
            else if (nInvoiceTypeFixec == 1)
            {
                Global.TagSQL(ref sReturn);
                if (nInvoiceType > 0)
                {
                    sReturn = sReturn + "InvoiceType in(" + nInvoiceType.ToString() + ")";
                }
                else
                {
                    sReturn = sReturn + "InvoiceType in(" + (int)EnumSampleInvoiceType.None + "," + (int)EnumSampleInvoiceType.SampleInvoice + "," + (int)EnumSampleInvoiceType.SalesContract+ ")";
                }
            }
            
            #endregion

            #region Yet Not Issue PI
            if (bYetNotAttachWithSalesContract)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull(ApproveBy,0)!=0 and  SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where Isnull(ExportPIID,0)<=0) ";
            }
            #endregion
            #region Yet Not Receive L/C
            if (bYetNotReceiveLC)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " isnull(ApproveBy,0)!=0 and  SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where isnull(ExportPIID,0)>0   and ExportPIID in (Select ExportPIID from ExportPI where isnull(LCID,0)<=0))";
            }
            #endregion
            #region Order/Mkt Not Assign
            if (bchkYetNotAssignOrder)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "isnull(MKTEmpID,0)<=0";
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SampleInvoiceID in (Select DOD.SampleInvoiceID from SampleInvoiceDetail as DOD where ProductID in (" + sProductIDs + "))";
            }
            #endregion

            #region Style No
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where OrderNo like '%" + sOrderNo + "%')";
            }
            #endregion
            #region Ref No
            if (!string.IsNullOrEmpty(sRefNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SampleInvoiceID in (Select DOD.SampleInvoiceID from SampleInvoiceDetail as DOD where StyleNo like '%" + sRefNo + "%')";
            }
            #endregion

            #region Invoice No
            if (!string.IsNullOrEmpty(sInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "SampleInvoiceNo like '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region P/I  No
            if (!string.IsNullOrEmpty(sPINo))
            {
                if (nInvoiceTypeFixec == 2)
                {
                    Global.TagSQL(ref sReturn);

                    sReturn = sReturn + " InvoiceType in(" + (int)EnumSampleInvoiceType.DocCharge + "," + (int)EnumSampleInvoiceType.Adjstment_Qty + "," + (int)EnumSampleInvoiceType.Adjstment_Value + "," + (int)EnumSampleInvoiceType.Adjstment_Commission +"," + (int)EnumSampleInvoiceType.ReturnAdjustment + ") AND ExportPIID>0 and ExportPIID in  (Select ExportPI.ExportPIID from ExportPI where PINo like '%" + sPINo + "%')";
                }
                else
                {
                    if (nProductNature== 0)//for yarn
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where DyeingOrderType in (2) and ExportPIID in (Select ExportPI.ExportPIID from ExportPI where PINo like '%" + sPINo + "%'))";
                    }
                    else
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "PINo like '%" + sPINo + "%'";
                    }
                }
            }
            #endregion

            #region Business Unit
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion
            #region Product Nature
            if (nProductNature>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ProductNature,0)= " + nProductNature;
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " order by SampleInvoiceID DESC";
            return sSQL;
        }
       
        #endregion
    }
    

}
