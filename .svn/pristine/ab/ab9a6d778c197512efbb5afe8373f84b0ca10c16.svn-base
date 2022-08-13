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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;


namespace ESimSolFinancial.Controllers
{

    public class SparePartsConsumptionController : Controller
    {

        #region Declartion
        CapitalResource _oCapitalResource = new CapitalResource();
        List<CapitalResource> _oCapitalResources = new List<CapitalResource>();
        CRWiseSpareParts _oCRWiseSpareParts = new CRWiseSpareParts();
        List<CRWiseSpareParts> _oCRWiseSparePartss = new List<CRWiseSpareParts>();
        SparePartsRequisition _oSparePartsRequisition = new SparePartsRequisition();
        List<SparePartsRequisition> _oSparePartsRequisitions = new List<SparePartsRequisition>();
        SparePartsChallan _oSparePartsChallan = new SparePartsChallan();
        List<SparePartsChallan> _oSparePartsChallans = new List<SparePartsChallan>();
        SparePartsConsumptionRegister _oSparePartsConsumptionRegister = new SparePartsConsumptionRegister();
        List<SparePartsConsumptionRegister> _oSparePartsConsumptionRegisters = new List<SparePartsConsumptionRegister>();
        Product _oProduct = new Product();
        List<Product> _oProducts = new List<Product>();

        #endregion

        #region Based On Capital Resource
        public ActionResult ViewSPConsumptionCR(int isMenu, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            if (this.Session[SessionInfo.ParamObj] != null && isMenu==0)
            {
                oSPCRegister = (SparePartsConsumptionRegister)Session[SessionInfo.ParamObj];
            }
            else
            {
                oSPCRegister.StartDate = DateTime.Today.AddDays(-30);
                oSPCRegister.EndDate = DateTime.Today;
                oSPCRegister.BUID = buid;
            }
            oSPCRegister.ReportLayout = EnumReportLayout.SPConsumptionCR;
            string sSQL = this.GetsSQL(oSPCRegister, isMenu);
            oSPCRegister.SparePartsConsumptionRegisters = SparePartsConsumptionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ResourcesType = CapitalResource.GetsResourceType(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(((User)Session[SessionInfo.CurrentUser]), (int)Session[SessionInfo.currentUserID]);
            return View(oSPCRegister);
        }
        public ActionResult ViewSPConsumptionCRWiseParts(int nCRID, int buid)
        {
            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            if (this.Session[SessionInfo.ParamObj] != null)
            {
                oSPCRegister = (SparePartsConsumptionRegister)Session[SessionInfo.ParamObj];
            }
            else
            {
                oSPCRegister.StartDate = DateTime.Today.AddDays(-30);
                oSPCRegister.EndDate = DateTime.Today;
                oSPCRegister.BUID = buid;
            }
            oSPCRegister.ReportLayout = EnumReportLayout.SPConsumptionCRWiseParts;
            if (nCRID > 0)
            {
                oSPCRegister.CRID = nCRID;
                string sSQL = this.GetsSQL(oSPCRegister, 0);
                oSPCRegister.SparePartsConsumptionRegisters = SparePartsConsumptionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                CapitalResource oCR = new CapitalResource();
                oCR = CapitalResource.Get(nCRID, (int)Session[SessionInfo.currentUserID]);
                oSPCRegister.CRName = oCR.Name;
                oSPCRegister.CRCode = oCR.Code;
                oSPCRegister.BUID = oCR.BUID;
                oSPCRegister.ResourceType = oCR.ParentID;
            }
            ViewBag.ResourcesType = CapitalResource.GetsResourceType(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(((User)Session[SessionInfo.CurrentUser]), (int)Session[SessionInfo.currentUserID]);
            return View(oSPCRegister);
        }

        #endregion

        #region Based On Spare Parts
        public ActionResult ViewSPConsumptionParts(int isMenu,int buid, int menuid) //ViewSPConsumptionParts  // Spare Parts Wise Consumption
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            if (this.Session[SessionInfo.ParamObj] != null && isMenu == 0)
            {
                oSPCRegister = (SparePartsConsumptionRegister)Session[SessionInfo.ParamObj];
            }
            else
            {
                oSPCRegister.StartDate = DateTime.Today.AddDays(-30);
                oSPCRegister.EndDate = DateTime.Today;
                oSPCRegister.BUID = buid;
            }
            oSPCRegister.ReportLayout = EnumReportLayout.SPConsumptionParts;
            string sSQL = this.GetsSQL(oSPCRegister, isMenu);
            oSPCRegister.SparePartsConsumptionRegisters = SparePartsConsumptionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ResourcesType = CapitalResource.GetsResourceType(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(((User)Session[SessionInfo.CurrentUser]), (int)Session[SessionInfo.currentUserID]);
            return View(oSPCRegister);
        }
        public ActionResult ViewSPConsumptionPartsWiseCR(int nProductID, int buid) //ViewSPConsumptionPartsWiseCR
        {


            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            if (this.Session[SessionInfo.ParamObj] != null)
            {
                oSPCRegister = (SparePartsConsumptionRegister)Session[SessionInfo.ParamObj];
            }
            else
            {
                oSPCRegister.StartDate = DateTime.Today.AddDays(-30);
                oSPCRegister.EndDate = DateTime.Today;
                oSPCRegister.BUID = buid;
            }
            oSPCRegister.ReportLayout = EnumReportLayout.SPConsumptionPartsWiseCR;
            if (nProductID > 0)
            {
                oSPCRegister.ProductID = nProductID;
                string sSQL = this.GetsSQL(oSPCRegister, 0);
                oSPCRegister.SparePartsConsumptionRegisters = SparePartsConsumptionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                Product oP = new Product();
                oP = oP.Get(nProductID, (int)Session[SessionInfo.currentUserID]);
                oSPCRegister.ProductName = oP.ProductName;
                oSPCRegister.ProductCode = oP.ProductCode;
                oSPCRegister.BUID = oP.BUID;
            }

            ViewBag.ResourcesType = CapitalResource.GetsResourceType(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(((User)Session[SessionInfo.CurrentUser]), (int)Session[SessionInfo.currentUserID]);
            return View(oSPCRegister);
        }
        #endregion

        #region Common View
        public ActionResult ViewSPConsumptionTransaction(int buid, int nCRID, int nSPID)
        {
            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            if (this.Session[SessionInfo.ParamObj] != null)
            {
                oSPCRegister = (SparePartsConsumptionRegister)Session[SessionInfo.ParamObj];
            }
            else
            {
                oSPCRegister.StartDate = DateTime.Today.AddDays(-30);
                oSPCRegister.EndDate = DateTime.Today;
                oSPCRegister.BUID = buid;
            }
            oSPCRegister.ReportLayout = EnumReportLayout.SPConsumptionTransaction;
            if (nCRID > 0 && nSPID > 0)
            {
                oSPCRegister.CRID = nCRID;
                oSPCRegister.ProductID = nSPID;
                string sSQL = this.GetsSQL(oSPCRegister, 0);
                oSPCRegister.SparePartsConsumptionRegisters = SparePartsConsumptionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                CapitalResource oCR = new CapitalResource();
                oCR = CapitalResource.Get(nCRID, (int)Session[SessionInfo.currentUserID]);
                oSPCRegister.CRName = oCR.Name;
                oSPCRegister.CRCode = oCR.Code;
                oSPCRegister.BUID = oCR.BUID;
                oSPCRegister.ResourceType = oCR.ParentID;
                Product oP = new Product();
                oP = oP.Get(nSPID, (int)Session[SessionInfo.currentUserID]);
                oSPCRegister.ProductCode = oP.ProductCode;
                oSPCRegister.ProductName = oP.ProductName;
            }
            ViewBag.ResourcesType = CapitalResource.GetsResourceType(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(((User)Session[SessionInfo.CurrentUser]), (int)Session[SessionInfo.currentUserID]);
            return View(oSPCRegister);
        }
        #endregion

        #region Utility Functions
        [HttpPost]
        public ActionResult SetSessionData(SparePartsConsumptionRegister oSparePartsConsumptionRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSparePartsConsumptionRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSparePartsConsumptionRegister.SearchingCiteria);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetsSQL(SparePartsConsumptionRegister oTempSPCRegister, int isMenu)
        {
            string sReturnSQL = "";
            string sBaseSQL = "";
            string sWhereClause = "";
            string sGroupBySQL = "";
            string sOrderBySQL = "";
            string sDateFieldName = "";
            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            if (this.Session[SessionInfo.ParamObj] != null && isMenu==0)
            {
                oSPCRegister = (SparePartsConsumptionRegister)Session[SessionInfo.ParamObj];
            }
            else
            {
                oSPCRegister = oTempSPCRegister;
            }

            #region Base SQL
            if (oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionCR || oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionPartsWiseCR)
            {
                sBaseSQL = "SELECT HH.CRID, HH.CRCode, HH.CRName, HH.CRModel, HH.ResourceType, HH.ResourceTypeName, SUM(HH.Amount) AS Amount FROM View_SparePartsConsumptionRegister AS HH";
                sGroupBySQL = "GROUP BY HH.CRID, HH.CRCode, HH.CRName, HH.CRModel, HH.ResourceType, HH.ResourceTypeName";
                sOrderBySQL = "ORDER BY HH.CRName ASC";
                sDateFieldName = "IssueDate";
            }
            else if (oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionCRWiseParts || oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionParts)
            {
                sBaseSQL = "SELECT  HH.ProductID, HH.ProductCode, HH.ProductName, HH.MUnitName, SUM(HH.ConsumptionQty) AS ConsumptionQty, SUM(HH.Amount) AS Amount FROM View_SparePartsConsumptionRegister AS HH  ";
                sGroupBySQL = "GROUP BY HH.ProductID, HH.ProductCode, HH.ProductName, HH.MUnitName";
                sOrderBySQL = "ORDER BY HH.ProductName ASC";
                sDateFieldName = "IssueDate";
            }
            else if (oSPCRegister.ReportLayout == EnumReportLayout.SPConsumptionTransaction)
            {
                sBaseSQL = "SELECT HH.SparePartsChallanDetailID, HH.SparePartsChallanID, HH.SparePartsRequisitionDetailID, HH.SparePartsRequisitionID, HH.CRName, HH.CRCode, HH.CRBrand, HH.ProductID, HH.ProductCode, HH.ProductName, HH.LotID, HH.LotNo, HH.ChallanNo, HH.ChallanBy, HH.ChallanByName, HH.ChallanDate, HH.RequisitionNo, HH.RequisitionBy, HH.RequisitionByName, HH.ApprovedBy, HH.ApprovedByName, HH.StoreID, HH.StoreName, HH.ConsumptionQty, HH.Amount, HH.UnitPrice, HH.MUnitName, HH.LocationName, HH.Currency FROM View_SparePartsConsumptionRegister AS HH";
                sGroupBySQL = "";
                sOrderBySQL = "ORDER BY HH.ChallanDate ASC";
                sDateFieldName = "ChallanDate";
            }
            #endregion

            #region Date Search
            Global.TagSQL(ref sWhereClause);
            sWhereClause = sWhereClause + "CONVERT(Date,Convert(Varchar(12), " + sDateFieldName + ",106))>= CONVERT(Date,Convert(Varchar(12),'" + oSPCRegister.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12)," + sDateFieldName + ",106)) < CONVERT(Date,Convert(Varchar(12),'" + oSPCRegister.EndDate.ToString("dd MMM yyyy") + "',106))";
            #endregion

            #region BUID
            if (oSPCRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.BUID = " + oSPCRegister.BUID.ToString();
            }
            #endregion
            #region Capital Resource ID
            if (oSPCRegister.CRID > 0)
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.CRID = " + oSPCRegister.CRID;
            }
            #endregion
            #region Capital Resource Name
            if (!string.IsNullOrEmpty(oSPCRegister.CRName))
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.CRName LIKE '%" + oSPCRegister.CRName.Trim() + "%'";
            }
            #endregion
            #region Capital Resource Code
            if (!string.IsNullOrEmpty(oSPCRegister.CRCode))
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.CRCode LIKE '%" + oSPCRegister.CRCode.Trim() + "%'";
            }
            #endregion
            #region Capital Resource Store
            if (!string.IsNullOrEmpty(oSPCRegister.StoreName))
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.StoreName LIKE '%" + oSPCRegister.StoreName.Trim() + "%'";
            }
            #endregion
            #region Capital Resource Type
            if (oSPCRegister.ResourceType > 0)
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.ResourceType = " + oSPCRegister.ResourceType;
            }
            #endregion
            #region Product Name
            if (!string.IsNullOrEmpty(oSPCRegister.ProductName))
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.ProductName LIKE '%" + oSPCRegister.ProductName.Trim() + "%'";
            }
            #endregion
            #region Product Code
            if (!string.IsNullOrEmpty(oSPCRegister.ProductCode))
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.ProductCode LIKE '%" + oSPCRegister.ProductCode.Trim() + "%'";
            }
            #endregion
            #region Challan No
            if (!string.IsNullOrEmpty(oSPCRegister.ChallanNo))
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.ChallanNo LIKE '%" + oSPCRegister.ChallanNo.Trim() + "%'";
            }
            #endregion
            #region Requisition No
            if (!string.IsNullOrEmpty(oSPCRegister.RequisitionNo))
            {
                Global.TagSQL(ref sWhereClause);
                sWhereClause = sWhereClause + " HH.RequisitionNo LIKE '%" + oSPCRegister.RequisitionNo.Trim() + "%'";
            }
            #endregion
            
            sReturnSQL = sBaseSQL + " " + sWhereClause + " " + sGroupBySQL + " " + sOrderBySQL;
            return sReturnSQL;
        }

        #endregion

        #region Gets Data
        [HttpPost]
        public JsonResult Gets(SparePartsConsumptionRegister oSPCRegister)
        {
            oSPCRegister = new SparePartsConsumptionRegister();
            string sSQL = this.GetsSQL(oSPCRegister, 0);
            oSPCRegister.SparePartsConsumptionRegisters = SparePartsConsumptionRegister.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var jSonResult = Json(oSPCRegister, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        #endregion

        #region Reports
        private SparePartsConsumptionRegister GetListForPrint(int nReportlayout, int buid)
        {
            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            if (this.Session[SessionInfo.ParamObj] != null)
            {
                oSPCRegister = (SparePartsConsumptionRegister)Session[SessionInfo.ParamObj];
            }
            else
            {
                oSPCRegister.StartDate = DateTime.Today.AddDays(-30);
                oSPCRegister.EndDate = DateTime.Today;
                oSPCRegister.BUID = buid;
            }
            oSPCRegister.ReportLayout = (EnumReportLayout)nReportlayout;
            string sSQL = this.GetsSQL(oSPCRegister, 0);
            oSPCRegister.SparePartsConsumptionRegisters = SparePartsConsumptionRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return oSPCRegister;
        }
        public ActionResult PrintCapitalResourceList(int nBuid, int nReportLayout, double nts)
        {
            int nCompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            oSPCRegister = GetListForPrint(nReportLayout, nBuid);
            rptSPConsumptionCR oReport = new rptSPConsumptionCR();
            byte[] abytes = oReport.PrepareReport(oSPCRegister, oCompany, oBusinessUnit);
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
    }
}