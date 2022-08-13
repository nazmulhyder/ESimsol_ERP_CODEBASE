using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using ESimSol.Reports;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class ProductionExecutionPlanController : Controller
    {
        #region Declaration
        ProductionExecutionPlan _oProductionExecutionPlan = new ProductionExecutionPlan();
        ProductionExecutionPlanDetail _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
        List<ProductionExecutionPlanDetail> _oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
        List<ProductionExecutionPlan> _oProductionExecutionPlans = new List<ProductionExecutionPlan>();
        List<ProductionExecutionPlanDetailBreakdown> _oProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
        ProductionExecutionPlanDetailBreakdown _oProductionExecutionPlanDetailBreakdown = new ProductionExecutionPlanDetailBreakdown();
        Company _oCompany = new Company();
        TechnicalSheetThumbnail _oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
        OrderRecap _oOrderRecap = new OrderRecap();
        #endregion

        #region function
        private List<ProductionExecutionPlanDetailBreakdown> GetProductionExecutionPlanDetailBreakdowns(int id, List<ProductionExecutionPlanDetailBreakdown> oProductionExecutionPlanDetailBreakdowns)
        {
            List<ProductionExecutionPlanDetailBreakdown> oTempProductionExecutionPlanDetailBreakdowns = new List<ProductionExecutionPlanDetailBreakdown>();
            foreach (ProductionExecutionPlanDetailBreakdown oItem in oProductionExecutionPlanDetailBreakdowns)
            {
                if (oItem.ProductionExecutionPlanDetailID == id)
                {
                    oTempProductionExecutionPlanDetailBreakdowns.Add(oItem);
                }
            }

            return oTempProductionExecutionPlanDetailBreakdowns;
        }
        #endregion

        #region Views

        public ActionResult ViewProductionExecutionPlans(int id, int buid, int menuid)//id is production Execution ID
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oProductionExecutionPlans = new List<ProductionExecutionPlan>();
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ProductionExecutionPlan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            if(id>0)
            {
                string sSQL = "SELECT * FROM View_ProductionExecutionPlan WHERE ProductionExecutionPlanID = "+id; 
                _oProductionExecutionPlans = ProductionExecutionPlan.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            return View(_oProductionExecutionPlans);
        }
        public ActionResult ViewProductionExecutionPlan(int id, int OrderRecapID)
        {
            int nBUID = 0;
            _oProductionExecutionPlanDetails = new List<ProductionExecutionPlanDetail>();
            _oProductionExecutionPlan = new ProductionExecutionPlan();
            if (id > 0)
            {
                _oProductionExecutionPlan = _oProductionExecutionPlan.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionExecutionPlan.ProductionExecutionPlanDetails = ProductionExecutionPlanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oProductionExecutionPlanDetailBreakdowns = ProductionExecutionPlanDetailBreakdown.GetsByPEPPlanID(id, (int)Session[SessionInfo.currentUserID]);
                foreach (ProductionExecutionPlanDetail oItem in _oProductionExecutionPlan.ProductionExecutionPlanDetails)
                {
                    oItem.ProductionExecutionPlanDetailBreakdowns = GetProductionExecutionPlanDetailBreakdowns(oItem.ProductionExecutionPlanDetailID, _oProductionExecutionPlanDetailBreakdowns);
                }
                nBUID = _oProductionExecutionPlan.BUID;
                OrderRecapID = _oProductionExecutionPlan.OrderRecapID;
            }
            else if (OrderRecapID>0)
            {

                _oProductionExecutionPlan = _oProductionExecutionPlan.GetByOrderRecap(OrderRecapID, (int)Session[SessionInfo.currentUserID]);
                if (_oProductionExecutionPlan.ProductionExecutionPlanID > 0)
                {
                    _oProductionExecutionPlan.ProductionExecutionPlanDetails = ProductionExecutionPlanDetail.Gets(_oProductionExecutionPlan.ProductionExecutionPlanID, (int)Session[SessionInfo.currentUserID]);
                    _oProductionExecutionPlanDetailBreakdowns = ProductionExecutionPlanDetailBreakdown.GetsByPEPPlanID(_oProductionExecutionPlan.ProductionExecutionPlanID, (int)Session[SessionInfo.currentUserID]);
                    foreach (ProductionExecutionPlanDetail oItem in _oProductionExecutionPlan.ProductionExecutionPlanDetails)
                    {
                        oItem.ProductionExecutionPlanDetailBreakdowns = GetProductionExecutionPlanDetailBreakdowns(oItem.ProductionExecutionPlanDetailID, _oProductionExecutionPlanDetailBreakdowns);
                    }
                    nBUID = _oProductionExecutionPlan.BUID;
                }
                else
                {
                    _oProductionExecutionPlan = new ProductionExecutionPlan();
                    _oOrderRecap = new OrderRecap();
                    _oOrderRecap = _oOrderRecap.Get(OrderRecapID, (int)Session[SessionInfo.currentUserID]);
                    _oProductionExecutionPlan.OrderRecapID = _oOrderRecap.OrderRecapID;
                    _oProductionExecutionPlan.RecapNo = _oOrderRecap.OrderRecapNo;
                    _oProductionExecutionPlan.StyleNo = _oOrderRecap.StyleNo;
                    _oProductionExecutionPlan.BuyerName = _oOrderRecap.BuyerName;
                    _oProductionExecutionPlan.ProductName = _oOrderRecap.ProductName;
                    _oProductionExecutionPlan.MerchandiserName = _oOrderRecap.MerchandiserName;
                    _oProductionExecutionPlan.RecapQty = _oOrderRecap.TotalQuantity;
                    _oProductionExecutionPlan.TechnicalSheetID = _oOrderRecap.TechnicalSheetID;
                    _oProductionExecutionPlan.BuyerID = _oOrderRecap.BuyerID;
                    _oProductionExecutionPlan.ProductID = _oOrderRecap.ProductID;
                    nBUID = _oOrderRecap.BUID;
                }                
            }
            ProductionTimeSetup oProductionTimeSetup = new ProductionTimeSetup();            
            string sSQL = "SELECT * FROM View_PLineConfigure WHERE MachineQty>= ISNULL((SELECT MachineQty FROM OrderRecap WHERE OrderRecapID = " + OrderRecapID + "),0) ORDER BY ProductionUnitID ASC";           
            ViewBag.PLineConfigures = PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ProductionTimeSetup = oProductionTimeSetup.GetByBU(nBUID, (int)Session[SessionInfo.currentUserID]);
            return View(_oProductionExecutionPlan);
        }  
        public ActionResult ViewProductionExecutionPlanDetail(int id, int buid, int OrderRecapID, double ts) // id is ProductionUnit ID
        {
                _oProductionExecutionPlanDetail = new ProductionExecutionPlanDetail();
                ProductionTimeSetup oProductionTimeSetup = new ProductionTimeSetup();
                List<PLineConfigure> oPLineConfigures = new List<PLineConfigure>();
                if(id>0)
                {
                    string sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitID=" + id;
                    oPLineConfigures= PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    string sSQL = "SELECT * FROM View_PLineConfigure WHERE MachineQty>= ISNULL((SELECT MachineQty FROM OrderRecap WHERE OrderRecapID = " + OrderRecapID + "),0) ORDER BY ProductionUnitID ASC";
                    oPLineConfigures = PLineConfigure.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                ViewBag.PLineConfigures = oPLineConfigures;
                ViewBag.ProductionTimeSetup = oProductionTimeSetup.GetByBU(buid, (int)Session[SessionInfo.currentUserID]);
                return PartialView(_oProductionExecutionPlanDetail);
        }
        #endregion

        #region Image
        public Image GetCoverImage(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetThumbnail.ThumbnailImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult GetCoverImageInBase64(int id)
        {

            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage == null)
            {
                oTechnicalSheetThumbnail.ThumbnailImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oTechnicalSheetThumbnail.ThumbnailImage) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save
        [HttpPost]
        public JsonResult Save(ProductionExecutionPlan oProductionExecutionPlan)
        {
            _oProductionExecutionPlan = new ProductionExecutionPlan();
            try
            {
                _oProductionExecutionPlan = oProductionExecutionPlan;
                _oProductionExecutionPlan.PlanStatus = (EnumPlanStatus)oProductionExecutionPlan.PlanStatusInInt;
                _oProductionExecutionPlan = _oProductionExecutionPlan.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionExecutionPlan = new ProductionExecutionPlan();
                _oProductionExecutionPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionExecutionPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Delete
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string smessage = "";
            try
            {
                ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
                smessage = oProductionExecutionPlan.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP Approve
        [HttpGet]
        public JsonResult Approve(int id)
        {
            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            try
            {
                oProductionExecutionPlan = oProductionExecutionPlan.Approve(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductionExecutionPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionExecutionPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region AcceptRevise
        [HttpPost]
        public JsonResult AcceptRevise(ProductionExecutionPlan oProductionExecutionPlan)
        {
            _oProductionExecutionPlan = new ProductionExecutionPlan();
            try
            {
                _oProductionExecutionPlan = oProductionExecutionPlan;
                _oProductionExecutionPlan = _oProductionExecutionPlan.AcceptRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionExecutionPlan = new ProductionExecutionPlan();
                _oProductionExecutionPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionExecutionPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP RequstRevise
        [HttpGet]
        public JsonResult RequstRevise(int id)
        {
            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            try
            {
                oProductionExecutionPlan = oProductionExecutionPlan.RequestForRevise(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oProductionExecutionPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionExecutionPlan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HTTP GetReviseLogs
        [HttpGet]
        public JsonResult GetReviseLogs(int id)
        {
            _oProductionExecutionPlans = new List<ProductionExecutionPlan>();
            try
            {
                string sSQL ="SELECT * FROM View_ProductionExecutionPlanLog WHERE ProductionExecutionPlanID = "+id;
                _oProductionExecutionPlans = ProductionExecutionPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionExecutionPlan = new ProductionExecutionPlan();
                _oProductionExecutionPlan.ErrorMessage = ex.Message;
                _oProductionExecutionPlans.Add(_oProductionExecutionPlan);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionExecutionPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Advance Search
        public ActionResult AdvanceSearch()
        {

            ProductionExecutionPlan oProductionExecutionPlan = new ProductionExecutionPlan();
            oProductionExecutionPlan.MerchandiserList = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oProductionExecutionPlan);
        }

        [HttpGet]
        public JsonResult Gets(string sTemp, double ts)
        {
            List<ProductionExecutionPlan> oProductionExecutionPlans = new List<ProductionExecutionPlan>();
            try
            {
                string sSQL = GetSQL(sTemp);
                oProductionExecutionPlans = ProductionExecutionPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oProductionExecutionPlan = new ProductionExecutionPlan();
                _oProductionExecutionPlan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionExecutionPlans);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            int nCboPlanDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dPlanStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dPlanEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            int nCboShipmentDate = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dShipmentStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dShipmentEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            string sRefNo = sTemp.Split('~')[6];
            string sStNo = sTemp.Split('~')[7];
            string sPONo = sTemp.Split('~')[8];
            string nBuyerIDs = sTemp.Split('~')[9];
            string nProductionUnitIDs = sTemp.Split('~')[10];
            int nMerchandiser = Convert.ToInt32(sTemp.Split('~')[11]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[12]);

            string sReturn1 = "SELECT * FROM View_ProductionExecutionPlan";
            string sReturn = "";
            #region Ref No

            if (sRefNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo = '" + sRefNo + "'";

            }
            #endregion

            #region Style No

            if (sStNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StyleNo = '" + sStNo + "'";

            }
            #endregion

            #region  PO No
            if (sPONo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  GUProductionOrderNo ='" + sPONo + "'";
            }
            #endregion

            #region Buyer Name
            if (!string.IsNullOrEmpty(nBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + nBuyerIDs+")";
            }
            #endregion

            #region PU
            if (!string.IsNullOrEmpty(nProductionUnitIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionExecutionPlanID IN (SELECT PEPD.ProductionExecutionPlanID FROM View_ProductionExecutionPlanDetail AS PEPD WHERE PEPD.ProductionUnitID IN (" + nProductionUnitIDs + "))";
            }
            #endregion

            #region Merchandiser Name

            if (nMerchandiser != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID = " + nMerchandiser;
            }
            #endregion

            #region BU

            if (nBUID!= 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion


            #region Date Wise

            #region Plan Date
            if (nCboPlanDate > 0)
            {
                if (nCboPlanDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate = '" + dPlanStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate != '" + dPlanStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate > '" + dPlanStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate < '" + dPlanStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate>= '" + dPlanStartDate.ToString("dd MMM yyyy") + "' AND PlanDate < '" + dPlanEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCboPlanDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PlanDate< '" + dPlanStartDate.ToString("dd MMM yyyy") + "' OR PlanDate > '" + dPlanEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            #region Shipment Date
            if (nCboShipmentDate > 0)
            {
                if (nCboShipmentDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate = '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboShipmentDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboShipmentDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboShipmentDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCboShipmentDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate>= '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCboShipmentDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate< '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion
            #endregion
            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " BuyerID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Print

        public ActionResult PrintList(string sID)
        {
            _oProductionExecutionPlan = new ProductionExecutionPlan();
            _oCompany = new Company();
            string sSQL = "SELECT * FROM View_ProductionExecutionPlan WHERE ProductionExecutionPlanID IN (" + sID + ")";
            _oProductionExecutionPlan.ProductionExecutionPlans = ProductionExecutionPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oProductionExecutionPlan.Company = _oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            _oProductionExecutionPlan.Company.CompanyLogo = GetCompanyLogo(_oProductionExecutionPlan.Company);
            rptProductionExecutionPlanList oReport = new rptProductionExecutionPlanList();
            
            byte[] abytes = oReport.PrepareReport(_oProductionExecutionPlan,"Production Execution Plan List");
            return File(abytes, "application/pdf");
        }

        public ActionResult Preview(int id)
        {
            _oProductionExecutionPlan = new ProductionExecutionPlan();
            Company oCompany = new Company();
            _oProductionExecutionPlan = _oProductionExecutionPlan.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oProductionExecutionPlan.ProductionExecutionPlanDetails = ProductionExecutionPlanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oProductionExecutionPlan.TechnicalSheetThumbnail = _oTechnicalSheetThumbnail.GetFrontImage(_oProductionExecutionPlan.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oProductionExecutionPlan.Company = oCompany;
            rptProductionExecutionPlan oReport = new rptProductionExecutionPlan();

            byte[] abytes = oReport.PrepareReport(_oProductionExecutionPlan);
            return File(abytes, "application/pdf");
        }

        public Image GetCompanyLogo(Company oCompany)
        {
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
        }

        #endregion Print


    }
    
    
}