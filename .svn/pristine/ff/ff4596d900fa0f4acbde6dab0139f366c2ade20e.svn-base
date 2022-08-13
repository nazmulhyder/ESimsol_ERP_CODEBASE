using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Data;
using System.Data.OleDb;

namespace ESimSolFinancial.Controllers
{
    public class LotTrakingController : Controller
    {
        #region Declaration
        LotTraking _oLotTraking = new LotTraking();
        List<LotTraking> _oLotTrakings = new List<LotTraking>();
        #endregion

        #region Actions
        #region 
        public ActionResult View_LotTrakings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLotTrakings = new List<LotTraking>();
            ViewBag.TriggerParentTypes = EnumObject.jGets(typeof(EnumTriggerParentsType));
            ViewBag.BUID = buid;
            return View(_oLotTrakings);
        }
       
     

        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot> _oLots = new List<Lot>();
            try
            {
                string sSQL = "select top(100)* from View_Lot";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oLot.LotNo))
                {
                    oLot.LotNo = oLot.LotNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LotNo Like'%" + oLot.LotNo + "%'";
                }
                if (oLot.BUID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "BUID=" + oLot.BUID;
                }
                if (oLot.WorkingUnitID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "WorkingUnitID=" + oLot.WorkingUnitID;
                }
                if (oLot.ProductID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ProductID=" + oLot.ProductID;
                }
                if (oLot.ParentID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " ParentType=103 and ParentID in (Select InvoiceProductID from InvoiceProduct where InvoiceProduct.InvoiceID in (Select Invoice.InvoiceID from Invoice where InvoiceID=" + oLot.ParentID + "))";
                    sReturn = sReturn + "ParentID in (Select GRNDetailID from GRNDetail where GRNID in (Select GRN.GRNID from GRN where GRN.GRNType in (2) and GRN.RefObjectID in (" + oLot.ParentID + ")))";
                }
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID=" + oLot.BUID + " and ParentType in (" + (int)EnumTriggerParentsType.AdjustmentDetail + "," + (int)EnumTriggerParentsType.GRNDetailDetail + "," + (int)EnumTriggerParentsType.DUProGuideLineDetail + ")";

                sSQL = sSQL + "" + sReturn;
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                
            }
            catch (Exception ex)
            {
                //_oLotTraking = new Lot();
                //oRPT_DeliverySummary.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Gets_Invoice(ImportInvoice oInvoice)
        {
            List<ImportInvoice> oInvoices = new List<ImportInvoice>();

            oInvoices = ImportInvoice.Gets("select top(100)* from View_ImportInvoice where BUID=" + oInvoice.BUID + " and ImportInvoiceNo like '%" + oInvoice.ImportInvoiceNo + "%'", (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Gets_LotTraking(LotTraking oLotTraking)
        {
            _oLotTrakings = LotTraking.Gets_Lot(1,oLotTraking.LotNo, (int)Session[SessionInfo.currentUserID]);
            _oLotTrakings = _oLotTrakings.OrderBy(x => x.WorkingUnitID).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oLotTrakings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


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

        #region Searching

        #endregion

        #region report
        public ActionResult PrintLotTracking(string sTempString)
        {
            string sDateRange = "";
            string sReportHeader = "";
            int nLotID = 0;
            int nWorkingUnitID = 0;
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            WorkingUnit oWorkingUnit = new WorkingUnit();
             List<LotTraking> oLotTrakings = new List<LotTraking>();
            try
            {
                _oLotTrakings = LotTraking.Gets_Lot(1,sTempString, (int)Session[SessionInfo.currentUserID]);
                oLotTrakings = _oLotTrakings.OrderBy(x => x.WorkingUnitID).ToList();
                nLotID = Convert.ToInt32(sTempString.Split(',')[0]);
                //InvoiceProduct oInvoiceProduct = new InvoiceProduct();
                ImportInvoice oInvoice = new ImportInvoice();
                _oLotTraking = new LotTraking();
               
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (LotTraking oitem in oLotTrakings)
                {
                    _oLotTraking.LotNo = oitem.LotNo;
                    if (nWorkingUnitID != oitem.WorkingUnitID)
                    {
                        oWorkingUnit = new WorkingUnit();
                        oWorkingUnit.WorkingUnitID = oitem.WorkingUnitID;
                        oWorkingUnit.LocationName = oitem.WUName;
                        oWorkingUnits.Add(oWorkingUnit);
                        nWorkingUnitID = oitem.WorkingUnitID;
                    }
                }

                //oWorkingUnits = WorkingUnit.Gets("SELECT top(6)* FROM View_WorkingUnit WHERE IsActive = 1 AND IsStore = 1 and WorkingUnitID in (Select ITransaction.WorkingUnitID from ITransaction where LotID in (" + sLotIDs + ") ) ", (Guid)Session[SessionInfo.wcfSessionID]);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            }
            catch (Exception ex)
            {
                _oLotTrakings=new List<LotTraking>();
            }
            rptLotTraking oReport = new rptLotTraking();
            byte[] abytes = oReport.PrepareReportStoreWise(_oLotTrakings, oCompany, oWorkingUnits, _oLotTraking, oBusinessUnit);
            return File(abytes, "application/pdf");


        }

        public ActionResult PrintStockLotTracking(string sTempString, double nts)
        {
            _oLotTrakings = new List<LotTraking>();
            List<Lot> oLots = new List<Lot>();
            Lot oLot = new Lot();
            string sSQL = "";
            string nLotIDs = sTempString.Split('~')[0];
            int nbuid =Convert.ToInt32 (sTempString.Split('~')[1]);
            sSQL = "select * from View_Lot where BUID IN ( " + nbuid + " ) AND LotID in ( " + nLotIDs + " ) or ParentLotID in (select LotID from View_Lot where LotID in ( " + nLotIDs + ") )";
            oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oLotTrakings = LotTraking.Gets_Lot(nbuid, nLotIDs, (int)Session[SessionInfo.currentUserID]);
            _oLotTrakings = _oLotTrakings.OrderBy(x => x.WorkingUnitID).ToList();
                     Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nbuid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptLotTraking oReport = new rptLotTraking();
            byte[] abytes = oReport.PrepareReport(oLots, _oLotTrakings, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
     
        }
        #endregion 

    }
}
