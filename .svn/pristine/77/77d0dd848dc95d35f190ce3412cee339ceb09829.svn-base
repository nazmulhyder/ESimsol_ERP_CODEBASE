using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol;
using System.Web.Script.Serialization;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;

namespace ESimSol.Controllers
{
    public class TradingDeliveryChallanController : Controller
    {
        #region Declaration
        TradingDeliveryChallan _oTradingDeliveryChallan = new TradingDeliveryChallan();
        List<TradingDeliveryChallan> _oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
        #endregion

        #region Actions
        public ActionResult ViewTradingDeliveryChallans(int buid, int menuid)
        {
            ViewBag.BUID = buid;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TradingDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
            _oTradingDeliveryChallans = TradingDeliveryChallan.GetsInitialTradingDeliveryChallans(buid, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.DeliveryBy FROM TradingDeliveryChallan AS MM WHERE ISNULL(MM.DeliveryBy,0)!=0) ORDER BY HH.UserName";
            List<User> oDeliveryUsers = new List<BusinessObjects.User>();
            BusinessObjects.User oDeliveryUser = new BusinessObjects.User();
            oDeliveryUser.UserID = 0; oDeliveryUser.UserName = "--Select Delivery User--";
            oDeliveryUsers.Add(oDeliveryUser);
            oDeliveryUsers.AddRange(BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));

            ViewBag.DeliveryUsers = oDeliveryUsers;
            ViewBag.DateCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.AmountCompareOperatorObjs = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oTradingDeliveryChallans);
        }

        public ActionResult ViewTradingDeliveryChallan(int id, int siid, double ts)
        {
            _oTradingDeliveryChallan = new TradingDeliveryChallan();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            int nSaleInvoiceID = siid;
            if (id > 0)
            {
                _oTradingDeliveryChallan = _oTradingDeliveryChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oTradingDeliveryChallan.TradingDeliveryChallanDetails = TradingDeliveryChallanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                nSaleInvoiceID = _oTradingDeliveryChallan.TradingSaleInvoiceID;
            }
            else
            {
                #region MAP Invoice to TradingDeliveryChallan
                TradingDeliveryChallanDetail oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
                List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails = new List<TradingDeliveryChallanDetail>();
                TradingSaleInvoice oTradingSaleInvoice = new TradingSaleInvoice();
                List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
                oTradingSaleInvoice = oTradingSaleInvoice.Get(siid, (int)Session[SessionInfo.currentUserID]);
                oTradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(siid, (int)Session[SessionInfo.currentUserID]);

                _oTradingDeliveryChallan = new TradingDeliveryChallan();
                _oTradingDeliveryChallan.TradingDeliveryChallanID = 0;
                _oTradingDeliveryChallan.BUID = oTradingSaleInvoice.BUID;
                _oTradingDeliveryChallan.TradingSaleInvoiceID = oTradingSaleInvoice.TradingSaleInvoiceID;
                _oTradingDeliveryChallan.ChallanNo = "";
                _oTradingDeliveryChallan.ChallanDate = DateTime.Today;
                _oTradingDeliveryChallan.BuyerID = oTradingSaleInvoice.BuyerID;
                _oTradingDeliveryChallan.StoreID = 0;
                _oTradingDeliveryChallan.CurrencyID = oTradingSaleInvoice.CurrencyID;
                _oTradingDeliveryChallan.NetAmount = 0;
                _oTradingDeliveryChallan.Note = "N/A";
                _oTradingDeliveryChallan.DeliveryBy = 0;
                _oTradingDeliveryChallan.DeliveryByName = "";
                _oTradingDeliveryChallan.BUName = oTradingSaleInvoice.BUName;
                _oTradingDeliveryChallan.BuyerName = oTradingSaleInvoice.BuyerName;
                _oTradingDeliveryChallan.StoreName = "";
                _oTradingDeliveryChallan.CurrencyName = oTradingSaleInvoice.CurrencyName;
                _oTradingDeliveryChallan.CurrencySymbol = oTradingSaleInvoice.CurrencySymbol;
                _oTradingDeliveryChallan.InvoiceNo = oTradingSaleInvoice.InvoiceNo;

                foreach (TradingSaleInvoiceDetail oItem in oTradingSaleInvoiceDetails)
                {
                    if (oItem.YetToChallanQty > 0)
                    {
                        oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
                        oTradingDeliveryChallanDetail.TradingDeliveryChallanDetailID = 0;
                        oTradingDeliveryChallanDetail.TradingDeliveryChallanID = 0;
                        oTradingDeliveryChallanDetail.TradingSaleInvoiceDetailID = oItem.TradingSaleInvoiceDetailID;
                        oTradingDeliveryChallanDetail.ProductID = oItem.ProductID;
                        oTradingDeliveryChallanDetail.ItemDescription = oItem.ItemDescription;
                        oTradingDeliveryChallanDetail.UnitID = oItem.MeasurementUnitID;
                        oTradingDeliveryChallanDetail.ChallanQty = oItem.YetToChallanQty;
                        oTradingDeliveryChallanDetail.YetToChallanQty = oItem.YetToChallanQty;
                        oTradingDeliveryChallanDetail.UnitPrice = oItem.UnitPrice;
                        oTradingDeliveryChallanDetail.Amount = (oItem.UnitPrice * oItem.YetToChallanQty);
                        oTradingDeliveryChallanDetail.ProductCode = oItem.ProductCode;
                        oTradingDeliveryChallanDetail.ProductName = oItem.ProductName;
                        oTradingDeliveryChallanDetail.UnitName = oItem.UnitName;
                        oTradingDeliveryChallanDetail.Symbol = oItem.Symbol;
                        oTradingDeliveryChallanDetails.Add(oTradingDeliveryChallanDetail);
                    }
                }
                _oTradingDeliveryChallan.TradingDeliveryChallanDetails = oTradingDeliveryChallanDetails;
                #endregion
            }
            oWorkingUnits = WorkingUnit.GetsPermittedStore(_oTradingDeliveryChallan.BUID, EnumModuleName.TradingDeliveryChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            _oTradingDeliveryChallan.WorkingUnits = oWorkingUnits;
            ViewBag.InvoiceDetails = TradingSaleInvoiceDetail.Gets(nSaleInvoiceID, (int)Session[SessionInfo.currentUserID]);
            return View(_oTradingDeliveryChallan);
        }

        [HttpPost]
        public JsonResult Save(TradingDeliveryChallan oTradingDeliveryChallan)
        {
            _oTradingDeliveryChallan = new TradingDeliveryChallan();

            try
            {
                _oTradingDeliveryChallan = oTradingDeliveryChallan;
                _oTradingDeliveryChallan = _oTradingDeliveryChallan.Save((int)Session[SessionInfo.currentUserID]);
                TradingSaleInvoice oTradingSaleInvoice = new TradingSaleInvoice();
                _oTradingDeliveryChallan.TradingSaleInvoice = oTradingSaleInvoice.Get(_oTradingDeliveryChallan.TradingSaleInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingDeliveryChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingDeliveryChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReloadTradingDeliveryChallanProduct(TradingDeliveryChallan oTradingDeliveryChallan)
        {
            TradingDeliveryChallanDetail oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
            List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails = new List<TradingDeliveryChallanDetail>();
            List<TradingDeliveryChallanDetail> oTempTradingDeliveryChallanDetails = new List<TradingDeliveryChallanDetail>();
            try
            {
                List<TradingSaleInvoiceDetail> oTradingSaleInvoiceDetails = new List<TradingSaleInvoiceDetail>();
                oTradingSaleInvoiceDetails = TradingSaleInvoiceDetail.Gets(oTradingDeliveryChallan.TradingSaleInvoiceID, (int)Session[SessionInfo.currentUserID]);
                oTempTradingDeliveryChallanDetails = TradingDeliveryChallanDetail.Gets(oTradingDeliveryChallan.TradingDeliveryChallanID, (int)Session[SessionInfo.currentUserID]);
                TradingDeliveryChallanDetail oTempTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
                foreach (TradingSaleInvoiceDetail oItem in oTradingSaleInvoiceDetails)
                {
                    oTempTradingDeliveryChallanDetail = this.GetExistingTradingDeliveryChallan(oTradingDeliveryChallan.TradingDeliveryChallanID, oItem.ProductID, oTempTradingDeliveryChallanDetails);
                    if ((oTempTradingDeliveryChallanDetail.ChallanQty + oItem.YetToChallanQty) > 0)
                    {
                        oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
                        oTradingDeliveryChallanDetail.TradingDeliveryChallanDetailID = oTempTradingDeliveryChallanDetail.TradingDeliveryChallanDetailID;
                        oTradingDeliveryChallanDetail.TradingDeliveryChallanID = oTempTradingDeliveryChallanDetail.TradingDeliveryChallanID;
                        oTradingDeliveryChallanDetail.TradingSaleInvoiceDetailID = oItem.TradingSaleInvoiceDetailID;
                        oTradingDeliveryChallanDetail.ProductID = oItem.ProductID;
                        oTradingDeliveryChallanDetail.ItemDescription = oItem.ItemDescription;
                        oTradingDeliveryChallanDetail.UnitID = oItem.MeasurementUnitID;
                        oTradingDeliveryChallanDetail.ChallanQty = (oTempTradingDeliveryChallanDetail.ChallanQty + oItem.YetToChallanQty);
                        oTradingDeliveryChallanDetail.YetToChallanQty = (oTempTradingDeliveryChallanDetail.ChallanQty + oItem.YetToChallanQty);
                        oTradingDeliveryChallanDetail.UnitPrice = oItem.UnitPrice;
                        oTradingDeliveryChallanDetail.Amount = (oItem.UnitPrice * oTradingDeliveryChallanDetail.ChallanQty);
                        oTradingDeliveryChallanDetail.ProductCode = oItem.ProductCode;
                        oTradingDeliveryChallanDetail.ProductName = oItem.ProductName;
                        oTradingDeliveryChallanDetail.UnitName = oItem.UnitName;
                        oTradingDeliveryChallanDetail.Symbol = oItem.Symbol;
                        oTradingDeliveryChallanDetails.Add(oTradingDeliveryChallanDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                oTradingDeliveryChallanDetails = new List<TradingDeliveryChallanDetail>();
                oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
                oTradingDeliveryChallanDetail.ErrorMessage = ex.Message;
                oTradingDeliveryChallanDetails.Add(oTradingDeliveryChallanDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTradingDeliveryChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private TradingDeliveryChallanDetail GetExistingTradingDeliveryChallan(int nTradingDeliveryChallanID, int nProductID, List<TradingDeliveryChallanDetail> oTradingDeliveryChallanDetails)
        {
            TradingDeliveryChallanDetail oTradingDeliveryChallanDetail = new TradingDeliveryChallanDetail();
            if (nTradingDeliveryChallanID > 0)
            {
                foreach (TradingDeliveryChallanDetail oItem in oTradingDeliveryChallanDetails)
                {
                    if (oItem.ProductID == nProductID)
                    {
                        return oTradingDeliveryChallanDetail;
                    }
                }
            }
            return oTradingDeliveryChallanDetail;
        }

        [HttpPost]
        public JsonResult Delete(TradingDeliveryChallan oTradingDeliveryChallan)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oTradingDeliveryChallan.Delete((int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Disburse(TradingDeliveryChallan oTradingDeliveryChallan)
        {
            _oTradingDeliveryChallan = new TradingDeliveryChallan();
            try
            {
                _oTradingDeliveryChallan = oTradingDeliveryChallan;
                _oTradingDeliveryChallan = _oTradingDeliveryChallan.Disburse((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingDeliveryChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingDeliveryChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult PrintTradingDeliveryChallan(int id, double ts)
        {
            _oTradingDeliveryChallan = new TradingDeliveryChallan();
            _oTradingDeliveryChallan = _oTradingDeliveryChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTradingDeliveryChallan.TradingDeliveryChallanDetails = TradingDeliveryChallanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptTradingDeliveryChallan oReport = new rptTradingDeliveryChallan();
            byte[] abytes = oReport.PrepareReport(_oTradingDeliveryChallan, oCompany);
            return File(abytes, "application/pdf");

        }

        public ActionResult PrintTradingDeliveryChallans(string ids, double ts)
        {
            _oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
            string sSql = "SELECT * FROM View_TradingDeliveryChallan WHERE TradingDeliveryChallanID IN (" + ids + ") ORDER BY TradingDeliveryChallanID";
            _oTradingDeliveryChallans = TradingDeliveryChallan.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptTradingDeliveryChallans oReport = new rptTradingDeliveryChallans();
            byte[] abytes = oReport.PrepareReport(_oTradingDeliveryChallans, oCompany);
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
        public JsonResult WaitForDisburse(TradingDeliveryChallan oTradingDeliveryChallan)
        {
            _oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
            try
            {
                string sSQL = "SELECT * FROM View_TradingDeliveryChallan WHERE BUID = " + oTradingDeliveryChallan.BUID.ToString() + " AND ISNULL(DeliveryBy,0)=0 ORDER BY TradingDeliveryChallanID ASC";
                _oTradingDeliveryChallans = TradingDeliveryChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingDeliveryChallan = new TradingDeliveryChallan();
                _oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
                _oTradingDeliveryChallan.ErrorMessage = ex.Message;
                _oTradingDeliveryChallans.Add(_oTradingDeliveryChallan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingDeliveryChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvanceSearch(TradingDeliveryChallan oTradingDeliveryChallan)
        {
            _oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
            try
            {
                string sSQL = this.GetSQL(oTradingDeliveryChallan.Note, oTradingDeliveryChallan.BUID);
                _oTradingDeliveryChallans = TradingDeliveryChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTradingDeliveryChallan = new TradingDeliveryChallan();
                _oTradingDeliveryChallans = new List<TradingDeliveryChallan>();
                _oTradingDeliveryChallan.ErrorMessage = ex.Message;
                _oTradingDeliveryChallans.Add(_oTradingDeliveryChallan);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTradingDeliveryChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sSearchingData, int nBUID)
        {
            string sChallanNo = Convert.ToString(sSearchingData.Split('~')[0]);
            string sInvoiceNo = Convert.ToString(sSearchingData.Split('~')[1]);
            EnumCompareOperator eChallanDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[2]);
            DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[3]);
            DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[4]);
            EnumCompareOperator eInvoiceAmount = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[5]);
            double nStartAmount = Convert.ToDouble(sSearchingData.Split('~')[6]);
            double nEndAmount = Convert.ToDouble(sSearchingData.Split('~')[7]);
            int nDeliveryBy = Convert.ToInt32(sSearchingData.Split('~')[8]);
            string sBuyerIDs = Convert.ToString(sSearchingData.Split('~')[9]);
            string sProductIDs = Convert.ToString(sSearchingData.Split('~')[10]);

            string sReturn1 = "SELECT * FROM View_TradingDeliveryChallan";
            string sReturn = "";

            #region BUID
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID.ToString();
            }
            #endregion

            #region InvoiceSLNo
            if (sChallanNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo LIKE '%" + sChallanNo + "%'";
            }
            #endregion

            #region InvoiceNo
            if (sInvoiceNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " InvoiceNo LIKE '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region Challan Date
            if (eChallanDate != EnumCompareOperator.None)
            {
                if (eChallanDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eChallanDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eChallanDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eChallanDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eChallanDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (eChallanDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region TradingDeliveryChallanAmount
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

            #region Delivery By
            if (nDeliveryBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(DeliveryBy,0) = " + nDeliveryBy.ToString();
            }
            #endregion

            #region BuyerIDs
            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Products
            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " TradingDeliveryChallanID IN (SELECT TT.TradingDeliveryChallanID FROM TradingDeliveryChallanDetail AS TT WHERE TT.ProductID IN(" + sProductIDs + "))";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

    }
}
