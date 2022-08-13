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
using ESimSolFinancial.Controllers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class OrderSummaryController : Controller
    {

        #region DECLARATION
        OrderRecap _oOrderRecap = new OrderRecap();
        List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
        TAP _oTAP = new TAP();
        MasterLCDetail oMasterLCDetail = new MasterLCDetail();
        #endregion
        #region View Action
        public ActionResult ViewOrderSummary(int id, int buid, int menuid)//ORID
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oOrderRecap = new OrderRecap();
            _oOrderRecap = _oOrderRecap.Get(id,(int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.RecapBillOfMaterials = RecapBillOfMaterial.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.RawMaterialSourcings = RawMaterialSourcing.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.CostSheets = CostSheet.Gets("SELECT * FROM View_CostSheet WHERE TechnicalSheetID = (SELECT top 1 TechnicalSheetID FROM OrderRecap WHERE OrderRecapID = "+id+") ", (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.TAP = _oTAP.GetByRecap(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.GUProductionOrders = GUProductionOrder.Gets_bySalorderID(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.ProductionExecutionPlanDetails = ProductionExecutionPlanDetail.Gets("SELECT * FROM View_ProductionExecutionPlanDetail AS HH WHERE HH.ProductionExecutionPlanID IN (SELECT ProductionExecutionPlanID FROM ProductionExecutionPlan WHERE OrderRecapID="+id+")", (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.MasterLCDetail = oMasterLCDetail.GetByOrderRecap(id, (int)Session[SessionInfo.currentUserID]);
            List<CommercialInvoiceDetail> oInvoices = new List<CommercialInvoiceDetail>();
            oInvoices = CommercialInvoiceDetail.Gets("SELECT SUM(ISNULL(InvoiceQty,0)) AS InvoiceQty FROM View_CommercialInvoiceDetail WHERE OrderRecapID = "+id, (int)Session[SessionInfo.currentUserID]);
            @ViewBag.TotalInvoiceQty = Global.MillionFormatActualDigit(oInvoices[0].InvoiceQty);//set invoice qty
            return View(_oOrderRecap);
        }
        #endregion

        #region Print Action
        public ActionResult PrintRawMaterialSourcing(int id)
        {
            Company oCompany = new Company();
            _oOrderRecap = new OrderRecap();
            List<RawMaterialSourcing> oRawMaterialSourcings = new List<RawMaterialSourcing>();
            List<MeasurementSpecAttachment> oMeasurementSpecAttachments = new List<MeasurementSpecAttachment>();
            _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.RawMaterialSourcings = RawMaterialSourcing.Gets(id, (int)Session[SessionInfo.currentUserID]);    
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderRecap.Company = oCompany;

            rptRawMaterialSourcing oReport = new rptRawMaterialSourcing();
            byte[] abytes = oReport.PrepareReport(_oOrderRecap);
            return File(abytes, "application/pdf");
        }


        public ActionResult PrintOrderSummary(int id)
        {
            Company oCompany = new Company();
            _oOrderRecap = new OrderRecap();
            _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.RecapBillOfMaterials = RecapBillOfMaterial.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.RawMaterialSourcings = RawMaterialSourcing.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.CostSheets = CostSheet.Gets("SELECT * FROM View_CostSheet WHERE TechnicalSheetID = (SELECT top 1 TechnicalSheetID FROM OrderRecap WHERE OrderRecapID = " + id + ") ", (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.TAP = _oTAP.GetByRecap(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.GUProductionOrders = GUProductionOrder.Gets_bySalorderID(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.ProductionExecutionPlanDetails = ProductionExecutionPlanDetail.Gets("SELECT * FROM View_ProductionExecutionPlanDetail AS HH WHERE HH.ProductionExecutionPlanID IN (SELECT ProductionExecutionPlanID FROM ProductionExecutionPlan WHERE OrderRecapID=" + id + ")", (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.MasterLCDetail = oMasterLCDetail.GetByOrderRecap(id, (int)Session[SessionInfo.currentUserID]);
            List<CommercialInvoiceDetail> oInvoices = new List<CommercialInvoiceDetail>();
            oInvoices = CommercialInvoiceDetail.Gets("SELECT SUM(ISNULL(InvoiceQty,0)) AS InvoiceQty FROM View_CommercialInvoiceDetail WHERE OrderRecapID = " + id, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderRecap.Company = oCompany;

            rptOrderSummary oReport = new rptOrderSummary();
            byte[] abytes = oReport.PrepareReport(_oOrderRecap, oInvoices[0].InvoiceQty);
            return File(abytes, "application/pdf");
        }
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
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