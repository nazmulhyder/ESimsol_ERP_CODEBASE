using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class DeliveryChallanController :Controller
    {
        #region Declaration
        DeliveryChallan _oDeliveryChallan = new DeliveryChallan();
        List<DeliveryChallan> _oDeliveryChallans = new List<DeliveryChallan>();
        DeliveryChallanDetail _oDeliveryChallanDetail = new DeliveryChallanDetail();
        List<DeliveryChallanDetail> _oDeliveryChallanDetails = new List<DeliveryChallanDetail>();
        #endregion

        #region Actions

        public ActionResult ViewDeliveryChallans(int ProductNature, int buid, int menuid)
        {
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oDeliveryChallans = new List<DeliveryChallan>();
            string sSQL = "Select * from View_DeliveryChallan Where BUID=" + buid + " AND ProductNature = " + ProductNature + " AND ChallanStatus = " + (int)EnumChallanStatus.Initialized + " ORDER BY DeliveryChallanID ASC";
            _oDeliveryChallans = DeliveryChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oDeliveryChallans);
        }

        public ActionResult ViewDeliveryChallan(int id, int buid)
        {
            _oDeliveryChallan = new DeliveryChallan();
            if (id > 0)
            {
                _oDeliveryChallan = _oDeliveryChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryChallan.DeliveryChallanDetails = DeliveryChallanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }

            ViewBag.EnumChallanTypes = Enum.GetValues(typeof(EnumChallanType)).Cast<EnumChallanType>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.DeliveryChallan, EnumStoreType.IssueStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oDeliveryChallan);
        }


        [HttpPost]
        public JsonResult Save(DeliveryChallan oDeliveryChallan)
        {
            _oDeliveryChallan = new DeliveryChallan();
            try
            {
                short nDBOperation = (short)((oDeliveryChallan.DeliveryChallanID <= 0) ? EnumDBOperation.Insert : EnumDBOperation.Update);
                _oDeliveryChallan = oDeliveryChallan.IUD(nDBOperation, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryChallan = new DeliveryChallan();
                _oDeliveryChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliveryChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(DeliveryChallan oDeliveryChallan)
        {
            string sFeedBackMessage = "";
            try
            {
                if (oDeliveryChallan.DeliveryChallanID <= 0)
                    throw new Exception("Invalid delivery challan.");
                oDeliveryChallan = oDeliveryChallan.IUD((short)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryChallan.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Approve(DeliveryChallan oDeliveryChallan)
        {
            try
            {
                oDeliveryChallan = oDeliveryChallan.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDeliveryChallan = new DeliveryChallan();
                oDeliveryChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Searching
        private string GetSQL(string sTemp)
        {

            int nBUID = Convert.ToInt32(sTemp.Split('~')[0]);
            string sChallanNo = sTemp.Split('~')[1];
            string sDONo = sTemp.Split('~')[2];
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dtDCFrom = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dtDCTo = Convert.ToDateTime(sTemp.Split('~')[5]);
            string sBuyerIDs = sTemp.Split('~')[6];
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[7]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[8]);
            int nProductNature = Convert.ToInt32(sTemp.Split('~')[9]);
            string sPINo = sTemp.Split('~')[10];
            string sLotNo = sTemp.Split('~')[11];
            string sReturn1 = "SELECT * FROM View_DeliveryChallan";
            string sReturn = "";


            #region BU
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region ProductNature
            if (nProductNature != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ProductNature,0) = " + nProductNature;
            }
            #endregion

            #region sChallanNo
            if (!string.IsNullOrEmpty(sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo Like '%" + sChallanNo.Trim() + "%'";
            }
            #endregion

            #region DO No
            if (!string.IsNullOrEmpty(sDONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DONo Like '%" + sDONo.Trim() + "%'";
            }
            #endregion

            #region PI No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo Like '%" + sPINo.Trim() + "%' AND RefType = "+(int)EnumRefType.ExportPI;
            }
            #endregion

            #region Lot No
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DeliveryChallanID IN (SELECT DeliveryChallanID FROM View_DeliveryChallanDetail WHERE LotNo LIKE '%" + sLotNo + "%' )";
            }
            #endregion

            #region Order Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChallanDate = '" + dtDCFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChallanDate != '" + dtDCFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChallanDate > '" + dtDCFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChallanDate < '" + dtDCFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChallanDate>= '" + dtDCFrom.ToString("dd MMM yyyy") + "' AND ChallanDate < '" + dtDCTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ChallanDate < '" + dtDCFrom.ToString("dd MMM yyyy") + "' OR ChallanDate > '" + dtDCTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region IsApproved
            if (IsCheckedApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApproveBy,0) != 0";
            }
            #endregion

            #region IsNotApproved
            if (IsCheckedNotApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ISNULL(ApproveBy,0) = 0";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY DeliveryChallanID ASC";
            return sReturn;
        }

        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<DeliveryChallan> oDeliveryChallans = new List<DeliveryChallan>();
            try
            {
                string sSQL = GetSQL(Temp);
                oDeliveryChallans = DeliveryChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDeliveryChallans = new List<DeliveryChallan>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryChallans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets Delivery Order from different Module

        [HttpPost]
        public JsonResult GetsDeliveryOrderBUWise(DeliveryOrder oDeliveryOrder)
        {
            List<DeliveryOrder> oDeliveryOrders = new List<DeliveryOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_DeliveryOrder WHERE BUID = " + oDeliveryOrder.BUID + " AND ProductNature = " + oDeliveryOrder.ProductNatureInInt + "  AND ISNULL(ApproveBy,0)!=0 AND ISNULL(YetToDeliveryChallanQty,0)>0";
                if (oDeliveryOrder.ContractorID > 0)
                {
                    sSQL += " And ContractorID =" + oDeliveryOrder.ContractorID + "";
                }
                if (!string.IsNullOrEmpty(oDeliveryOrder.DONo.Trim()))
                {
                    sSQL += " And DONo Like '%" + oDeliveryOrder.DONo.Trim() + "%'";
                }
                oDeliveryOrders = DeliveryOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDeliveryOrder = new DeliveryOrder();
                oDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDeliveryOrderDetail(DeliveryOrder oDeliveryOrder)
        {
            List<DeliveryOrderDetail> oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_DeliveryOrderDetail WHERE DeliveryOrderID = " + oDeliveryOrder.DeliveryOrderID + " AND ISNULL(YetToDeliveryChallanQty,0)>0";
                oDeliveryOrderDetails = DeliveryOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region UpdateVehicleTime
        [HttpPost]
        public JsonResult UpdateVehicleTime(DeliveryChallan oDeliveryChallan)
        {
            _oDeliveryChallan = new DeliveryChallan();
            try
            {
                _oDeliveryChallan = oDeliveryChallan.UpdateVehicleTime((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryChallan = new DeliveryChallan();
                _oDeliveryChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliveryChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        [HttpPost]
        public ActionResult SetDeliveryChallanListData(DeliveryChallan oDeliveryChallan)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDeliveryChallan);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintDeliveryChallans()
        {
            _oDeliveryChallan = new DeliveryChallan();
            try
            {
                _oDeliveryChallan = (DeliveryChallan)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_DeliveryChallan WHERE DeliveryChallanID IN (" + _oDeliveryChallan.Note + ") Order By DeliveryChallanID";
                _oDeliveryChallan.DeliveryChallans = DeliveryChallan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryChallan = new DeliveryChallan();
                _oDeliveryChallans = new List<DeliveryChallan>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oDeliveryChallan.Company = oCompany;

            string Messge = "Delivery Challan List";
            rptDeliveryChallans oReport = new rptDeliveryChallans();
            byte[] abytes = oReport.PrepareReport(_oDeliveryChallan, Messge);
            return File(abytes, "application/pdf");
        }

        public ActionResult DeliveryChallanPrintPreview(int id, bool bIsChallanPrint, bool IsReportingUnit)
        {
            _oDeliveryChallan = new DeliveryChallan();
            _oDeliveryChallanDetails = new List<DeliveryChallanDetail>();
            List<DeliveryChallanDetail> oDistinctProductList = new List<DeliveryChallanDetail>();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            if (id > 0)
            {
                _oDeliveryChallan = _oDeliveryChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryChallanDetails = DeliveryChallanDetail.Gets(_oDeliveryChallan.DeliveryChallanID, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryChallan.BusinessUnit = oBusinessUnit.Get(_oDeliveryChallan.BUID, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryChallan.DeliveryChallanDetails = _oDeliveryChallanDetails; 
            }
            else
            {
                _oDeliveryChallan.BusinessUnit = new BusinessUnit();
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oDeliveryChallan.Company = oCompany;
            byte[] abytes;
            if(bIsChallanPrint)
            {
                rptDeliveryChallan oReport = new rptDeliveryChallan();
                abytes = oReport.PrepareReport(_oDeliveryChallan, IsReportingUnit);
            }
            else
            {
                rptDeliveryChallanGatePass oReport = new rptDeliveryChallanGatePass();
                abytes = oReport.PrepareReport(_oDeliveryChallan, IsReportingUnit);
            }            
            return File(abytes, "application/pdf");
        }

        #endregion Print

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
    }

}
