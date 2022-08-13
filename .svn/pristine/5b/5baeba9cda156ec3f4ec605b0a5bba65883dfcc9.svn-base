using System;
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
using System.Collections;
using System.Collections.Generic;
using ESimSolFinancial.Controllers.ReportController;
namespace ESimSolFinancial.Controllers
{
    public class DUStatementController : Controller
    {
        #region Declaration
        DUStatement _oDUStatement = new DUStatement();
        List<DUStatement> _oDUStatements = new List<DUStatement>();
        SampleInvoiceSetup _oSampleInvoiceSetup = new SampleInvoiceSetup();
        List<ExportLC> _oExportLCs = new List<ExportLC>();
        string _sSQL = "";
        #endregion
        public ActionResult ViewDUStatement(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
         
            ViewBag.BusinessUnitTypeInInt = 0;
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
                ViewBag.BusinessUnitTypeInInt = oBusinessUnit.BusinessUnitTypeInInt;
                
            }else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
                
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            _oDUStatements = new List<DUStatement>();
            try
            {
                //  _oDUStatements = DUStatement.Gets(_oDUStatement.StartDate, _oDUStatement.EndDate.AddDays(1), buid, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
                return View(_oDUStatements);
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View(_oDUStatements);
            }
        }

        #region Picker
        [HttpPost]
        public JsonResult GetExportPIs(ExportSCDO oExportSCDO)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = "Select Top(100)* from View_ExportPI";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oExportSCDO.PINo))
                {
                    oExportSCDO.PINo = oExportSCDO.PINo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PINo Like'%" + oExportSCDO.PINo + "%'";
                }
                if (oExportSCDO.BuyerID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " BuyerID  = " + oExportSCDO.BuyerID;
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
            

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PIStatus NOT IN ("+ (int)EnumPIStatus.Cancel +")"; //For Dyeing/Production Order issue->Must Checked valid PI(  Initialized = 0, RequestForApproved = 1,

                //sReturn = sReturn + " PIStatus in (0,1,2,3,4)"; //For Dyeing/Production Order issue->Must Checked valid PI(  Initialized = 0, RequestForApproved = 1,

                //Approved = 2,     PIIssue = 3,   BindWithLC = 4,   RequestForRevise = 5,  Cancel = 6) and Sales Contract is Approved

                sSQL = sSQL + "" + sReturn + " Order By IssueDate DESC";
                oExportPIs = ExportPI.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportPIs = new List<ExportPI>();
                ExportPI oExportPI = new ExportPI();
                oExportPI.ErrorMessage = ex.Message;
                oExportPIs.Add(oExportPI);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }// For Production  Order

        [HttpPost]
        public JsonResult GetSampleInvoices(SampleInvoice oSampleInvoice)
        {
            List<SampleInvoice> _oSampleInvoices = new List<SampleInvoice>();
            string sSQL = "SELECT Top(100)* FROM View_SampleInvoice";
            string sTempSQL = "";


            if (!String.IsNullOrEmpty(oSampleInvoice.SampleInvoiceNo))
            {
                oSampleInvoice.SampleInvoiceNo = oSampleInvoice.SampleInvoiceNo.Trim();
                Global.TagSQL(ref sTempSQL);
                sTempSQL += " SampleInvoiceNo like '%" + oSampleInvoice.SampleInvoiceNo + "'";
            }
            if (!String.IsNullOrEmpty(oSampleInvoice.OrderNo))
            {
                oSampleInvoice.OrderNo = oSampleInvoice.OrderNo.Trim();
                Global.TagSQL(ref sTempSQL);
                sTempSQL += " SampleInvoiceID in (Select DyeingOrder.SampleInvoiceID from DyeingOrder where OrderType in (2,4) and DyeingOrder.DyeingOrderID like '%" + oSampleInvoice.OrderNo + "')";
            }
            if (oSampleInvoice.ContractorID > 0)
            {
                Global.TagSQL(ref sTempSQL);
                sTempSQL += " ContractorID =" + oSampleInvoice.ContractorID;
            } 
            if (oSampleInvoice.BUID > 0)
            {
                Global.TagSQL(ref sTempSQL);
                sTempSQL += " BUID =" + oSampleInvoice.BUID;
            }
            sSQL = sSQL+ sTempSQL + " Order By SampleInvoiceDate DESC";
            try 
            {  
                _oSampleInvoices = SampleInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oSampleInvoices = new List<SampleInvoice>();
                oSampleInvoice = new SampleInvoice();
                oSampleInvoice.ErrorMessage = ex.Message;
                _oSampleInvoices.Add(oSampleInvoice);
            }
           
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExportLCs(ExportLC oExportLC)
        {
            List<ExportLC> _oExportLCs = new List<ExportLC>();
            string sReturn1 = "SELECT Top(100)* FROM View_ExportLC ";
            string sReturn = "";

            #region Export LC NO
            if (!string.IsNullOrEmpty(oExportLC.ExportLCNo))
            {
                oExportLC.ExportLCNo = oExportLC.ExportLCNo.Trim();
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportLCNo LIKE '%" + oExportLC.ExportLCNo + "%'";
            }
            #endregion

            #region BUID
            if (oExportLC.BUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID=" + oExportLC.BUID;
            }
            #endregion

            string sSQL = sReturn1 + sReturn + " Order By OpeningDate DESC";

            try 
            {
                _oExportLCs = ExportLC.GetsSQL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportLCs = new List<ExportLC>();
                oExportLC = new ExportLC();
                oExportLC.ErrorMessage = ex.Message;
                _oExportLCs.Add(oExportLC);
            }
           
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportLCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetDyeingOrders(DyeingOrder oDyeingOrder)
        {
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();

            string sSQL = "Select Top(100)* from View_DyeingOrder where DyeingOrderID>0";

            if (!String.IsNullOrEmpty(oDyeingOrder.OrderNo))
                sSQL = sSQL + " And OrderNo Like '%" + oDyeingOrder.OrderNo + "%'";
            if (oDyeingOrder.ContractorID > 0)
                sSQL = sSQL + " And ContractorID=" + oDyeingOrder.ContractorID;
            if (oDyeingOrder.BUID > 0)
                sSQL = sSQL + " And   DyeingOrderType in (Select OrderType from DUOrderSetup where BUID=" + oDyeingOrder.BUID + " )";
            
            try
            {
                oDyeingOrders = DyeingOrder.Gets(sSQL + "  Order By OrderDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDyeingOrders = new List<DyeingOrder>();
                oDyeingOrder = new DyeingOrder();
                oDyeingOrder.ErrorMessage = ex.Message;
                oDyeingOrders.Add(oDyeingOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDUDOrders(DUDeliveryOrder oDUDeliveryOrder)
        {
          
            List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();

            string sSQL = "Select Top(100)* From View_DUDeliveryOrder WHERE DUDeliveryOrderID>0";

            if (!String.IsNullOrEmpty(oDUDeliveryOrder.DONo))
                sSQL = sSQL + " And DONo Like '%" + oDUDeliveryOrder.DONo + "%'"; 
            if (oDUDeliveryOrder.ContractorID > 0)
                sSQL = sSQL + " And ContractorID=" + oDUDeliveryOrder.ContractorID;
         
          
              if (oDUDeliveryOrder.OrderID > 0)
                  sSQL = sSQL + " And DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + oDUDeliveryOrder.OrderID + "))";

              if (oDUDeliveryOrder.ExportPIID > 0)
                  sSQL = sSQL + " And DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where ExportPIID=" + oDUDeliveryOrder.ExportPIID + ")))";

           
            try
            {
                oDUDeliveryOrders = DUDeliveryOrder.Gets(sSQL + "  Order By DODate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDUDeliveryOrders = new List<DUDeliveryOrder>();
                oDUDeliveryOrder = new DUDeliveryOrder();
                oDUDeliveryOrder.ErrorMessage = ex.Message;
                oDUDeliveryOrders.Add(oDUDeliveryOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDUDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsFabricDO(FabricDeliveryOrder oFabricDeliveryOrder)
        {

            List<FabricDeliveryOrder> oFabricDeliveryOrders = new List<FabricDeliveryOrder>();

            string sSQL = "Select Top(100)* From View_FabricDeliveryOrder WHERE FDOID>0";

            if (!String.IsNullOrEmpty(oFabricDeliveryOrder.DONo))
                sSQL = sSQL + " And DONo Like '%" + oFabricDeliveryOrder.DONo + "%'";
            if (oFabricDeliveryOrder.ContractorID > 0)
                sSQL = sSQL + " And ContractorID=" + oFabricDeliveryOrder.ContractorID;
            if (oFabricDeliveryOrder.ExportPIID > 0)
                sSQL = sSQL + " And  FDOID in (Select FDOID from FabricDeliveryOrderDetail where ExportPIID="+ oFabricDeliveryOrder.ExportPIID+")" ;
            if (oFabricDeliveryOrder.FEOID > 0)
                sSQL = sSQL + " And  FDOID in (Select FDOID from FabricDeliveryOrderDetail where FEOID=" + oFabricDeliveryOrder.FEOID + ")";

            try
            {
                oFabricDeliveryOrders = FabricDeliveryOrder.Gets(sSQL + "  Order By DODate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricDeliveryOrders = new List<FabricDeliveryOrder>();
                oFabricDeliveryOrder = new FabricDeliveryOrder();
                oFabricDeliveryOrder.ErrorMessage = ex.Message;
                oFabricDeliveryOrders.Add(oFabricDeliveryOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Bill Statement Report
        public ActionResult PrintDUStatementReport(int nBUID, int nExportLCID, int nVersionNo, int nExportPIID, int nDyeingOrderID, int nDOID, int nSampleInvoiceID)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion
            _oSampleInvoiceSetup = new SampleInvoiceSetup();
            _oSampleInvoiceSetup = _oSampleInvoiceSetup.GetByBU(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #region Bill Statements
            if (nDOID > 0)
            {
                this.BillStatement_DO(nBUID, nDOID);
            }
            else if (nDyeingOrderID > 0)
            {
                this.BillStatement_Order(nBUID, nDyeingOrderID);
            }
            if (nSampleInvoiceID > 0)
            {
                this.BillStatement_SampleInvoice(nBUID, nSampleInvoiceID);
            }
            else if (nExportPIID > 0)
            {
                this.BillStatement_ExportPI(nBUID, nExportPIID);
            }
            else if (nExportLCID > 0)//&& nVersionNo >= 0
            {
                this.BillStatement_LC(nBUID, nExportLCID, nVersionNo);
            }
            //else if (nExportLCID > 0 && nVersionNo < 0)
            //{
            //    this.BillStatement_LC(nBUID, nExportLCID, nVersionNo);
            //    Hashtable headerTable = new Hashtable();
            //    headerTable.Add(++nKey, "BUYER NAME");
            //    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorName);
            //    headerTable.Add(++nKey, "Bill No");
            //    headerTable.Add(++nKey, _oSampleInvoiceSetup.Code + "-" + _oDUStatement.SampleInvoices[0].InvoiceNo);
            //    headerTable.Add(++nKey, "Concern Person");
            //    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorPersopnnalName);
            //    headerTable.Add(++nKey, "PI No");
            //    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ExportPINo);
            //    headerTable.Add(++nKey, "Date");
            //    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].SampleInvoiceDateST);
            //    headerTable.Add(++nKey, "Payment Term");
            //    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].PaymentTypeSt);
            //    _oDUStatement.HeaderTable = headerTable;
            //}
            #endregion

            _oDUStatement.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserName;
            _oDUStatement.BUID = nBUID;
            _oDUStatement.BusinessUnitType = oBusinessUnit.BusinessUnitType;
            _oDUStatement.Company = oCompany;
            _oDUStatement.BusinessUnit = oBusinessUnit;

            if (_oDUStatement.SampleInvoices == null || _oDUStatement.SampleInvoices.Count<=0) 
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Sample Invoices Are Not Found!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            else if (_oDUStatement.DyeingOrderReports == null || _oDUStatement.DyeingOrderReports.Count <= 0)
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Dyeing Orders Are Not Found!");
                return File(aErrorMessagebytes, "application/pdf");
            }

            rptDUStatement oReport = new rptDUStatement();
            byte[] abytes = oReport.PrepareBillStatementReport(_oDUStatement, _oSampleInvoiceSetup, 1);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintDUStatementReport_F2(int nBUID, int nExportLCID, int nVersionNo, int nExportPIID, int nDyeingOrderID, int nDOID, int nSampleInvoiceID)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion
            _oSampleInvoiceSetup = new SampleInvoiceSetup();
            _oSampleInvoiceSetup = _oSampleInvoiceSetup.GetByBU(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #region Bill Statements
            if (nDOID > 0)
            {
                this.BillStatement_DO(nBUID, nDOID);
            }
            else if (nDyeingOrderID > 0)
            {
                this.BillStatement_Order(nBUID, nDyeingOrderID);
            }
            if (nSampleInvoiceID > 0)
            {
                this.BillStatement_SampleInvoice(nBUID, nSampleInvoiceID);
            }
            else if (nExportPIID > 0)
            {
                this.BillStatement_ExportPI(nBUID, nExportPIID);
            }
            else if (nExportLCID > 0)//&& nVersionNo >= 0
            {
                this.BillStatement_LC(nBUID, nExportLCID, nVersionNo);
            }
            #endregion

            _oDUStatement.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserName;
            _oDUStatement.BUID = nBUID;
            _oDUStatement.BusinessUnitType = oBusinessUnit.BusinessUnitType;
            _oDUStatement.Company = oCompany;
            _oDUStatement.BusinessUnit = oBusinessUnit;

            if (_oDUStatement.SampleInvoices == null || _oDUStatement.SampleInvoices.Count <= 0)
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Sample Invoices Are Not Found!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            else if (_oDUStatement.DyeingOrderReports == null || _oDUStatement.DyeingOrderReports.Count <= 0)
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Dyeing Orders Are Not Found!");
                return File(aErrorMessagebytes, "application/pdf");
            }

            rptDUStatement oReport = new rptDUStatement();
            byte[] abytes = oReport.PrepareBillStatementReport_F2(_oDUStatement, _oSampleInvoiceSetup, 1);
            return File(abytes, "application/pdf");
        }
        private void BillStatement_DO(int nBUID, int nDOID)//___UNDER__CONSTRACTION
        {
            _oDUStatement.DUDeliveryOrders = DUDeliveryOrder.Gets("SELECT * FROM View_DUDeliveryOrder as DO WHERE  DOStatus>0 and DO.DUDeliveryOrderID=" + nDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("select * from View_DyeingOrderReport Where DyeingOrderID=" + nDyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where SampleInvoiceID in (select SampleInvoiceID from View_DyeingOrderReport Where DyeingOrderID=" + nDyeingOrderID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            int nKey = 0;
            if (_oDUStatement.SampleInvoices.Count > 0)
            {
                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "BUYER NAME");      headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorName);
                headerTable.Add(++nKey, "Concern Person");  headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorPersopnnalName);
                headerTable.Add(++nKey, "Bill No");         headerTable.Add(++nKey, _oSampleInvoiceSetup.Code + "-" + _oDUStatement.SampleInvoices[0].InvoiceNo);
                headerTable.Add(++nKey, "PI No");           headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ExportPINo);
                headerTable.Add(++nKey, "Date");            headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].SampleInvoiceDateST);
                headerTable.Add(++nKey, "Payment Term");    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].PaymentTypeSt);
                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
          
        }
        private void BillStatement_Order(int nBUID, int nDyeingOrderID)
        {
            _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("select * from View_DyeingOrderReport Where DyeingOrderID="+nDyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where SampleInvoiceID in (select SampleInvoiceID from View_DyeingOrderReport Where DyeingOrderID="+nDyeingOrderID+")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            int nKey = 0;
            if (_oDUStatement.SampleInvoices.Count > 0)
            {
                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "BUYER NAME");      headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorName);
                headerTable.Add(++nKey, "Concern Person");  headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorPersopnnalName);
                headerTable.Add(++nKey, "Bill No");         headerTable.Add(++nKey, _oSampleInvoiceSetup.Code + "-" + _oDUStatement.SampleInvoices[0].InvoiceNo);
                headerTable.Add(++nKey, "PI No");           headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ExportPINo);
                headerTable.Add(++nKey, "Date");            headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].SampleInvoiceDateST);
                headerTable.Add(++nKey, "Payment Term");    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].PaymentTypeSt);
                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
        }
        private void BillStatement_SampleInvoice(int nBUID, int nSampleInvoiceID)
        {
            _oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where SampleInvoiceID in (" + nSampleInvoiceID + ") and BUID in (0," + nBUID + ") and  CurrentStatus not in (" + (int)EnumSampleInvoiceStatus.Canceled + ")  order by SampleInvoiceID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets(nSampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            int nKey = 0;
            if (_oDUStatement.SampleInvoices.Count > 0)
            {
                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "Bill No");         headerTable.Add(++nKey, _oSampleInvoiceSetup.Code + "-" + _oDUStatement.SampleInvoices[0].InvoiceNo);
                headerTable.Add(++nKey, "PI No");           headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ExportPINo);
                headerTable.Add(++nKey, "Bill Date");       headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].SampleInvoiceDateST);
                headerTable.Add(++nKey, "BUYER NAME");      headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorName);
                headerTable.Add(++nKey, "Concern Person");  headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorPersopnnalName);
                headerTable.Add(++nKey, "Payment Term");    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].PaymentTypeSt);
                _oDUStatement.HeaderTable = headerTable;
                #endregion 
            }
        }
        private void BillStatement_ExportPI(int nBUID, int nExportPIID)
        {
            ExportPI oExportPIID = new ExportPI();
            oExportPIID = oExportPIID.Get(nExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where ExportPIID in (" + nExportPIID + ") and BUID in (0," + nBUID + ") and  CurrentStatus not in (" + (int)EnumSampleInvoiceStatus.Canceled + ")  order by SampleInvoiceID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oDUStatement.SampleInvoices != null && _oDUStatement.SampleInvoices.Count>0)
            {
                _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("select * from View_DyeingOrderReport Where SampleInvoiceID>0 and SampleInvoiceID In (" + string.Join(",", _oDUStatement.SampleInvoices.Select(x => x.SampleInvoiceID).ToList()) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                int nKey = 0;

                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "PI No");           headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ExportPINo);

                if (_oDUStatement.SampleInvoices.Count > 1)
                {
                    headerTable.Add(++nKey, "PI Date");         headerTable.Add(++nKey,oExportPIID.IssueDateInString);
                }
                else
                {
                        headerTable.Add(++nKey, "Bill No");     headerTable.Add(++nKey, _oSampleInvoiceSetup.Code + "-" + _oDUStatement.SampleInvoices[0].SampleInvoiceNo);
                }

                if (_oDUStatement.SampleInvoices[0].PaymentType == (int)EnumOrderPaymentType.CashOrCheque)
                {
                    headerTable.Add(++nKey, "MR No");           headerTable.Add(++nKey, _oSampleInvoiceSetup.Code + "-" + _oDUStatement.SampleInvoices[0].MRNo);
                }
                else {
                        headerTable.Add(++nKey, "Bill Date");   headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].SampleInvoiceDateST);
                }
                headerTable.Add(++nKey, "Payment Term");    headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].PaymentTypeSt);
                headerTable.Add(++nKey, "BUYER NAME");      headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorName);
                headerTable.Add(++nKey, "Concern Person");  headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorPersopnnalName);
                #endregion

                _oDUStatement.HeaderTable = headerTable;
            }
        }
        private void BillStatement_LC(int nBUID, int nExportLCID, int nVersionNo)
        {
            ExportLC oExportLC = new ExportLC();
            oExportLC = oExportLC.Get(nExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (nVersionNo >= 0)
            {
                _oDUStatement.ExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from ExportPILCmapping where isnull(VersionNo,0)=" + nVersionNo + " and Activity=1 and ExportLCID=" + nExportLCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oDUStatement.ExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from ExportPILCmapping where Activity=1 and ExportLCID=" + nExportLCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ") and BUID in (0," + nBUID + ") and  CurrentStatus not in (" + (int)EnumSampleInvoiceStatus.Canceled + ")  order by SampleInvoiceID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oDUStatement.SampleInvoices != null && _oDUStatement.SampleInvoices.Count!=0)
                _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("Select * From View_DyeingOrderReport Where SampleInvoiceID>0 and SampleInvoiceID In (" + string.Join(",", _oDUStatement.SampleInvoices.Select(x => x.SampleInvoiceID).ToList()) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            int nKey = 0;
            if (_oDUStatement.SampleInvoices.Count > 0)
            {
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "LC No");headerTable.Add(++nKey, oExportLC.ExportLCNo);
                headerTable.Add(++nKey, "PI No");headerTable.Add(++nKey, string.Join(", ", _oDUStatement.SampleInvoices.Select(x=>x.ExportPINo)));
                headerTable.Add(++nKey, "BUYER NAME");headerTable.Add(++nKey, string.Join(", ", _oDUStatement.SampleInvoices.Select(x => x.ContractorName)));
                headerTable.Add(++nKey, "Concern Person");headerTable.Add(++nKey, string.Join(", ", _oDUStatement.SampleInvoices.Select(x => x.ContractorPersopnnalName)));
                if (_oDUStatement.SampleInvoices.Count > 1)
                {
                    headerTable.Add(++nKey, "LC Date");headerTable.Add(++nKey,oExportLC.OpeningDateST);
                }
                else
                {
                    headerTable.Add(++nKey, "Date");headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].SampleInvoiceDateST);
                }
                headerTable.Add(++nKey, "Payment Term");headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].PaymentTypeSt);
                _oDUStatement.HeaderTable = headerTable;
            }
        }
        #endregion

        #region DO  Delivery Statement Report
        public ActionResult PrintDOStatementReport(int nBUID, int nExportLCID, int nVersionNo, int nExportPIID, int nDyeingOrderID, int nDOID, int nSampleInvoiceID, int nDOType)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            #endregion
            _oSampleInvoiceSetup = new SampleInvoiceSetup();
            _oSampleInvoiceSetup = _oSampleInvoiceSetup.GetByBU(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
                #region Dyeing Delivery Statements
                if (nDOID > 0)
                {
                    this.DUDeliveryStatement_DO(nBUID,nDOID);
                }
                else if (nDyeingOrderID > 0)
                {
                    this.DUDeliveryStatement_Order(nBUID, nDyeingOrderID);
                }
                else if (nSampleInvoiceID > 0)
                {
                    this.DUDeliveryStatement_SampleInvoice(nBUID, nSampleInvoiceID);
                }
                if (nExportPIID > 0)
                {
                    this.DUDeliveryStatement_ExportPI(nBUID, nExportPIID);
                }
                if (nExportLCID > 0)
                {
                    this.DUDeliveryStatement_LC(nBUID, nExportLCID, nVersionNo);
                }
                if (_oDUStatement.DyeingOrderReports.Count > 0)
                {
                    _oDUStatement.DUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets("SELECT * FROM View_DUDeliveryChallanRegister as DO WHERE  DO.DyeingOrderDetailID in (" + string.Join(",", _oDUStatement.DyeingOrderReports.Select(x => x.DyeingOrderDetailID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                    #region Return Challan
                    _sSQL = "Select * from View_DUReturnChallanDetail where DUDeliveryChallanDetailID in  (" + string.Join(",", _oDUStatement.DUDeliveryChallanRegisters.Select(x => x.DUDeliveryChallanDetailID).ToList()) + ")";
                    _oDUStatement.DUReturnChallanDetails = DUReturnChallanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (_oDUStatement.DUReturnChallanDetails.Count > 0)
                    {
                        _sSQL = "SELECT * FROM View_DUReturnChallan where isnull(ApprovedBy,0)!=0 and DUReturnChallanID in  (" + string.Join(",", _oDUStatement.DUReturnChallanDetails.Select(x => x.DUReturnChallanID).Distinct().ToList()) + ")";
                        _oDUStatement.DUReturnChallans = DUReturnChallan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    if (_oDUStatement.DUReturnChallans.Count > 0)
                    {
                        List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                      
                        foreach (DUReturnChallan oItem in _oDUStatement.DUReturnChallans)
                        {
                            oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList().ForEach(o => o.RCNo = oItem.DUReturnChallanNo);
                            oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList().ForEach(o => o.RCDate = oItem.ReturnDate.ToString("dd MMM yyyy"));

                        }
                        _oDUStatement.DUReturnChallanDetails = oDUReturnChallanDetails;
                    }
                    #endregion End Return Challan
                


                #endregion

                //if (_oDUStatement.DUDeliveryChallanDetails.Count <= 0)
                //{
                //    rptErrorMessage oErrorReport = new rptErrorMessage();
                //    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Delivery Challan Are Not Found!");
                //    return File(aErrorMessagebytes, "application/pdf");
                //}
            }
            else 
            {
                #region Fabric Delivery Statements
                if (nDOID > 0)
                {
                    this.DOStatement_DO_Fabric(nBUID, nDOID);
                }
                else if (nDyeingOrderID > 0)
                {
                    this.DOStatement_Order_Fabric(nBUID, nDyeingOrderID);
                }
                if (nSampleInvoiceID > 0)
                {
                    this.DOStatement_SampleInvoice_Fabric(nBUID, nSampleInvoiceID);

                    if (_oDUStatement.SampleInvoices == null || _oDUStatement.SampleInvoices.Count <= 0)
                    {
                        rptErrorMessage oErrorReport = new rptErrorMessage();
                        byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Sample Invoices Are Not Found!");
                        return File(aErrorMessagebytes, "application/pdf");
                    }
                }
                else if (nExportPIID > 0)
                {
                    this.DOStatement_ExportPI_Fabric(nBUID, nExportPIID);
                }
                else if (nExportLCID > 0)//&& nVersionNo >= 0
                {
                    this.DOStatement_LC_Fabric(nBUID, nExportLCID, nVersionNo);
                }
                #endregion

                if (_oDUStatement.FabricDeliveryChallans.Count <= 0)
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Fabric Delivery Challan Are Not Found!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
            }

            _oDUStatement.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserName;
            _oDUStatement.BUID = nBUID;
            _oDUStatement.BusinessUnitType = oBusinessUnit.BusinessUnitType;
            _oDUStatement.Company = oCompany;
            _oDUStatement.BusinessUnit = oBusinessUnit;

            rptDUStatement oReport = new rptDUStatement();
            byte[] abytes = oReport.PrepareReport_DO(_oDUStatement, nDOType);
            return File(abytes, "application/pdf");
        }

        #region Func For Dyeing Statement
        private void DUDeliveryStatement_DO(int nBUID, int nDOID)
        {
         
            _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID =" + nDOID + "";
            _oDUStatement.DUDeliveryOrders = DUDeliveryOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //this.DOStatement_DO(nBUID, nDOID);
            _sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE DUDeliveryOrderID in (" + string.Join(",", _oDUStatement.DUDeliveryOrders.Select(x => x.DUDeliveryOrderID).ToList()) + ")";
            _oDUStatement.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oDUStatement.DUDeliveryOrderDetails.Any())
            {
                _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("SELECT * FROM View_DyeingOrderReport WHERE DyeingOrderDetailID in (" + string.Join(",", _oDUStatement.DyeingOrderReports.Select(x => x.DyeingOrderDetailID).ToList()) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _sSQL = "SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from View_DyeingOrderReport where [Status]<9 and DyeingOrderDetailID>0 and DyeingOrderDetailID in(Select DyeingOrderDetailID from DUDeliveryOrderDetail where DUDeliveryOrderID=" + nDOID + "))";
            _oDUStatement.ExportPIs = ExportPI.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _sSQL = "SELECT * FROM View_ExportLC where ExportLCID in (Select ExportLCID from View_ExportPILCMapping where Activity=1 and ExportPIID in (" + string.Join(",", _oDUStatement.DUDeliveryOrders.Select(x => x.ExportPIID).ToList()) + "))";
            _oExportLCs = ExportLC.GetsSQL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nKey = 0;
            if (_oDUStatement.DUDeliveryOrders.Count > 0)
            {
                _oDUStatement.Title = "DO No";
                _oDUStatement.TitleNo = _oDUStatement.DUDeliveryOrders[0].DONoFull;
                _oDUStatement.BuyerName = _oDUStatement.DUDeliveryOrders[0].ContractorName;

                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "P/I No"); headerTable.Add(++nKey,string.Join(",",_oDUStatement.ExportPIs.Select(x => x.PINo_Full).Distinct().ToList()));
                headerTable.Add(++nKey, "P/I Date"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.IssueDate).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C No"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.ExportLCNo).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C Date"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.OpeningDateST).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C Value");  headerTable.Add(++nKey,  _oExportLCs.Select(x => x.Amount).Distinct().Sum());
                //headerTable.Add(++nKey, "PI No");           headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].PINo);
                //headerTable.Add(++nKey, "LC No");           headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].ExportLCNo);
                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
        }
        private void DUDeliveryStatement_Order(int nBUID, int nDyeingOrderID)
        {
            _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("SELECT * FROM View_DyeingOrderReport WHERE DyeingOrderID =" + nDyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE DyeingOrderDetailID>0 and DyeingOrderDetailID  in (" + string.Join(",", _oDUStatement.DyeingOrderReports.Select(x => x.DyeingOrderDetailID).ToList()) + ")";
            _oDUStatement.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oDUStatement.DUDeliveryOrderDetails.Any())
            {
                _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID in (" + string.Join(",", _oDUStatement.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderID).Distinct().ToList()) + ")";
                _oDUStatement.DUDeliveryOrders = DUDeliveryOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _sSQL = "SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from DyeingOrder where [Status]<9 and DyeingOrderID=" + nDyeingOrderID + ")";
            _oDUStatement.ExportPIs = ExportPI.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oDUStatement.ExportPIs.Count > 0)
            {
                _sSQL = "SELECT * FROM View_ExportLC where ExportLCID in (Select ExportLCID from View_ExportPILCMapping where Activity=1 and ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + "))";
                _oExportLCs = ExportLC.GetsSQL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            int nKey = 0;
            if (_oDUStatement.DyeingOrderReports.Count > 0)
            {
                _oDUStatement.Title = "Order No";
                _oDUStatement.TitleNo = _oDUStatement.DyeingOrderReports[0].OrderNoFull;
                _oDUStatement.TitleDate = _oDUStatement.DyeingOrderReports[0].OrderDateSt;
                _oDUStatement.BuyerName = _oDUStatement.DyeingOrderReports[0].ContractorName;
                _oDUStatement.MUName = _oDUStatement.DyeingOrderReports[0].MUName;
                _oDUStatement.Currency = _oDUStatement.DyeingOrderReports[0].Currency;

                #region Header
                Hashtable headerTable = new Hashtable();
                if (_oDUStatement.ExportPIs.Count > 0)
                {
                    headerTable.Add(++nKey, "P/I No"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.PINo_Full).Distinct().ToList()));
                    headerTable.Add(++nKey, "P/I Date"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.IssueDateInString).Distinct().ToList()));
                    headerTable.Add(++nKey, "P/I Value"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.Currency + "" + _oDUStatement.ExportPIs.Select(x => x.Amount).Sum()));
                }
                if (_oExportLCs.Count > 0)
                {
                    headerTable.Add(++nKey, "L/C No"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.ExportLCNo).Distinct().ToList()));
                    headerTable.Add(++nKey, "L/C Date"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.OpeningDateST).Distinct().ToList()));
                    headerTable.Add(++nKey, "L/C Value"); headerTable.Add(++nKey, _oDUStatement.Currency + "" + _oExportLCs.Select(x => x.Amount).Sum());
                }
               
                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
        }
        private void DUDeliveryStatement_SampleInvoice(int nBUID, int nSampleInvoiceID)
        {
            _oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where SampleInvoiceID in (" + nSampleInvoiceID + ") and BUID in (0," + nBUID + ") and  CurrentStatus not IN (" + (int)EnumSampleInvoiceStatus.Canceled + ")  order by SampleInvoiceID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("SELECT * FROM View_DyeingOrderReport WHERE SampleInvoiceID =" + nSampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE DyeingOrderDetailID>0 and DyeingOrderDetailID  in (" + string.Join(",", _oDUStatement.DyeingOrderReports.Select(x => x.DyeingOrderDetailID).ToList()) + ")";
            _oDUStatement.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oDUStatement.DUDeliveryOrderDetails.Any())
            {
                _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID in (" + string.Join(",", _oDUStatement.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderID).Distinct().ToList()) + ")";
                _oDUStatement.DUDeliveryOrders = DUDeliveryOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _sSQL = "SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from DyeingOrder where [Status]<9 and SampleInvoiceID=" + nSampleInvoiceID + ")";
            _oDUStatement.ExportPIs = ExportPI.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _sSQL = "SELECT * FROM View_ExportLC where ExportLCID in (Select ExportLCID from View_ExportPILCMapping where Activity=1 and ExportPIID in (" + string.Join(",", _oDUStatement.DUDeliveryOrders.Select(x => x.ExportPIID).ToList()) + "))";
            _oExportLCs = ExportLC.GetsSQL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nKey = 0;
            if (_oDUStatement.SampleInvoices.Count > 0)
            {

                _oDUStatement.Title = "Invoice/Bill No";
                _oDUStatement.TitleNo = _oDUStatement.SampleInvoices[0].SampleInvoiceNo;
                _oDUStatement.BuyerName = _oDUStatement.SampleInvoices[0].ContractorName;

                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "P/I No"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.PINo_Full).Distinct().ToList()));
                headerTable.Add(++nKey, "P/I Date"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.IssueDate).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C No"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.ExportLCNo).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C Date"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.OpeningDateST).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C Value"); headerTable.Add(++nKey, _oExportLCs.Select(x => x.Amount).Distinct().Sum());
             
                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
        }
        private void DUDeliveryStatement_ExportPI(int nBUID, int nExportPIID)//___UNDER__CONSTRACTION
        {

            _sSQL = "SELECT * FROM View_ExportPI where ExportPIID=" + nExportPIID + "";
            _oDUStatement.ExportPIs = ExportPI.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("SELECT * FROM View_DyeingOrderReport WHERE ExportPIID =" + nExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE DyeingOrderDetailID>0 and DyeingOrderDetailID  in (" + string.Join(",", _oDUStatement.DyeingOrderReports.Select(x => x.DyeingOrderDetailID).ToList()) + ")";
            _oDUStatement.DUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oDUStatement.DUDeliveryOrderDetails.Any())
            {
                _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID in (" + string.Join(",", _oDUStatement.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderID).Distinct().ToList()) + ")";
                _oDUStatement.DUDeliveryOrders = DUDeliveryOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _sSQL = "SELECT * FROM View_ExportLC where ExportLCID in (Select ExportLCID from View_ExportPILCMapping where Activity=1 and ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + "))";
            _oExportLCs = ExportLC.GetsSQL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nKey = 0;
            if (_oDUStatement.ExportPIs.Count > 0)
            {

                _oDUStatement.Title = "P/I No";
                _oDUStatement.TitleNo = _oDUStatement.ExportPIs[0].PINo_Full;
                _oDUStatement.BuyerName = _oDUStatement.ExportPIs[0].ContractorName;
                _oDUStatement.TitleDate = _oDUStatement.ExportPIs[0].IssueDateInString;

                #region Header
                Hashtable headerTable = new Hashtable();
                if (_oExportLCs.Count > 0)
                {
                    //headerTable.Add(++nKey, "P/I No"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.PINo_Full).Distinct().ToList()));
                    //headerTable.Add(++nKey, "P/I Date"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.IssueDate).Distinct().ToList()));
                    headerTable.Add(++nKey, "L/C No"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.ExportLCNo).Distinct().ToList()));
                    headerTable.Add(++nKey, "L/C Date"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.OpeningDateST).Distinct().ToList()));
                    headerTable.Add(++nKey, "L/C Value"); headerTable.Add(++nKey, _oExportLCs.Select(x => x.Amount).Distinct().Sum());
                }

                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
        }
        private void DUDeliveryStatement_LC(int nBUID, int nExportLCID, int nVersionNo)//___UNDER__CONSTRACTION
        {
            ExportLC oExportLC = new ExportLC();
            oExportLC = oExportLC.Get(nExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (nVersionNo >= 0)
            {
                _oDUStatement.ExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from ExportPILCmapping where isnull(VersionNo,0)=" + nVersionNo + " and Activity=1 and ExportLCID=" + nExportLCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oDUStatement.ExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from ExportPILCmapping where Activity=1 and ExportLCID=" + nExportLCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oDUStatement.DyeingOrderReports = DyeingOrderReport.Gets("SELECT * FROM View_DyeingOrderReport WHERE ExportPIID in ("+ string.Join(",", _oDUStatement.DUDeliveryOrders.Select(x => x.ExportPIID).ToList()) +")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nKey = 0;
            if (_oDUStatement.ExportPIs.Count > 0)
            {

                _oDUStatement.Title = "P/I No";
                _oDUStatement.TitleNo = _oDUStatement.ExportPIs[0].PINo_Full;
                _oDUStatement.BuyerName = _oDUStatement.ExportPIs[0].ContractorName;

                #region Header
                Hashtable headerTable = new Hashtable();
                //headerTable.Add(++nKey, "P/I No"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.PINo_Full).Distinct().ToList()));
                //headerTable.Add(++nKey, "P/I Date"); headerTable.Add(++nKey, string.Join(",", _oDUStatement.ExportPIs.Select(x => x.IssueDate).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C No"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.ExportLCNo).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C Date"); headerTable.Add(++nKey, string.Join(",", _oExportLCs.Select(x => x.OpeningDateST).Distinct().ToList()));
                headerTable.Add(++nKey, "L/C Value"); headerTable.Add(++nKey, _oExportLCs.Select(x => x.Amount).Distinct().Sum());
                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }

        }
        #endregion

        #region Func For Fabric Statement
        private void DOStatement_DO_Fabric(int nBUID, int nDOID)
        {
            _oDUStatement.FabricDeliveryChallans = FabricDeliveryChallan.Gets("SELECT * FROM View_FabricDeliveryChallan AS DO WHERE  DO.FDOID=" + nDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDUStatement.FabricDeliveryChallanDetails = FabricDeliveryChallanDetail.Gets("SELECT * FROM View_FabricDeliveryChallanDetail AS DO WHERE  DO.FDOID IN (SELECT * FROM View_FabricDeliveryChallan AS DO WHERE  DO.FDOID=" + nDOID+")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nKey = 0;
            if (_oDUStatement.FabricDeliveryChallans.Count > 0)
            {
                _oDUStatement.DUReturnChallans = DUReturnChallan.Gets("SELECT * FROM View_DUReturnChallan WHERE DUDeliveryChallanID IN (" + string.Join(",", _oDUStatement.DUDeliveryChallanRegisters.Select(x => x.DUDeliveryChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUStatement.DUReturnChallans.Count > 0)
                    _oDUStatement.DUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanID IN (" + string.Join(",", _oDUStatement.DUReturnChallans.Select(x => x.DUReturnChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "BUYER"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].ContractorName);
                headerTable.Add(++nKey, "Concern Person"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].CPName);
                headerTable.Add(++nKey, "LC Value"); headerTable.Add(++nKey, "");

                headerTable.Add(++nKey, "DO No"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].DONo);
                headerTable.Add(++nKey, "PI No"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].PINo);
                headerTable.Add(++nKey, "LC No"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].ExportLCNo);

                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
        }
        private void DOStatement_Order_Fabric(int nBUID, int nDyeingOrderID)
        {
            _oDUStatement.DUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets("SELECT * FROM View_DUDeliveryChallanRegister as DO WHERE  DO.OrderID=" + nDyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //_oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where SampleInvoiceID in (select SampleInvoiceID from View_DyeingOrderReport Where DyeingOrderID=" + nDyeingOrderID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nKey = 0;
            if (_oDUStatement.DUDeliveryChallanRegisters.Count > 0)
            {
                _oDUStatement.DUReturnChallans = DUReturnChallan.Gets("SELECT * FROM View_DUReturnChallan WHERE DUDeliveryChallanID IN (" + string.Join(",", _oDUStatement.DUDeliveryChallanRegisters.Select(x => x.DUDeliveryChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUStatement.DUReturnChallans.Count > 0)
                    _oDUStatement.DUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanID IN (" + string.Join(",", _oDUStatement.DUReturnChallans.Select(x => x.DUReturnChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "BUYER"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].ContractorName);
                headerTable.Add(++nKey, "Concern Person"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].CPName);
                headerTable.Add(++nKey, "LC Value"); headerTable.Add(++nKey, "");

                headerTable.Add(++nKey, "Order No"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].OrderNoFull);
                headerTable.Add(++nKey, "PI No"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].PINo);
                headerTable.Add(++nKey, "LC No"); headerTable.Add(++nKey, _oDUStatement.DUDeliveryChallanRegisters[0].ExportLCNo); _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
        }
        private void DOStatement_SampleInvoice_Fabric(int nBUID, int nSampleInvoiceID)
        {
            _oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where SampleInvoiceID in (" + nSampleInvoiceID + ") and BUID in (0," + nBUID + ") and  CurrentStatus not IN (" + (int)EnumSampleInvoiceStatus.Canceled + ")  order by SampleInvoiceID", ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nKey = 0;
            if (_oDUStatement.SampleInvoices.Count > 0)
            {
                _oDUStatement.DUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets("SELECT * FROM View_DUDeliveryChallanRegister as DO WHERE  DO.OrderID IN (SELECT DyeingOrderID FROM View_DyeingOrder WHERE SampleInvoiceID IN (" + string.Join(",", _oDUStatement.SampleInvoices.Select(x => x.SampleInvoiceID)) + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUStatement.DUDeliveryChallanRegisters != null && _oDUStatement.DUDeliveryChallanRegisters.Count > 0)
                    _oDUStatement.DUReturnChallans = DUReturnChallan.Gets("SELECT * FROM View_DUReturnChallan WHERE DUDeliveryChallanID IN (" + string.Join(",", _oDUStatement.DUDeliveryChallanRegisters.Select(x => x.DUDeliveryChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUStatement.DUReturnChallans != null && _oDUStatement.DUReturnChallans.Count > 0)
                    _oDUStatement.DUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanID IN (" + string.Join(",", _oDUStatement.DUReturnChallans.Select(x => x.DUReturnChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "Bill No"); headerTable.Add(++nKey, _oSampleInvoiceSetup.Code + "-" + _oDUStatement.SampleInvoices[0].InvoiceNo);
                headerTable.Add(++nKey, "PI No"); headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ExportPINo);
                headerTable.Add(++nKey, "Bill Date"); headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].SampleInvoiceDateST);
                headerTable.Add(++nKey, "BUYER NAME"); headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorName);
                headerTable.Add(++nKey, "Concern Person"); headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].ContractorPersopnnalName);
                headerTable.Add(++nKey, "Payment Term"); headerTable.Add(++nKey, _oDUStatement.SampleInvoices[0].PaymentTypeSt);
                _oDUStatement.HeaderTable = headerTable;
                #endregion
            }
        }
        private void DOStatement_ExportPI_Fabric(int nBUID, int nExportPIID)//___UNDER__CONSTRACTION
        {
            ExportPI oExportPI = new ExportPI();
            oExportPI = oExportPI.Get(nExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where ExportPIID in (" + nExportPIID + ") and BUID in (0," + nBUID + ") and  CurrentStatus not in (" + (int)EnumSampleInvoiceStatus.Canceled + ")  order by SampleInvoiceID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDUStatement.DUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets("SELECT * FROM View_DUDeliveryChallanRegister as DO WHERE  DO.DUDeliveryChallanID IN (SELECT DUDeliveryChallanID FROM View_DUDeliveryChallan WHERE ExportPIID=" + nExportPIID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oDUStatement.DUDeliveryChallanRegisters != null && _oDUStatement.DUDeliveryChallanRegisters.Count > 0)
            {
                _oDUStatement.DUReturnChallans = DUReturnChallan.Gets("SELECT * FROM View_DUReturnChallan WHERE DUDeliveryChallanID IN (" + string.Join(",", _oDUStatement.DUDeliveryChallanRegisters.Select(x => x.DUDeliveryChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUStatement.DUReturnChallans != null && _oDUStatement.DUReturnChallans.Count > 0)
                    _oDUStatement.DUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanID IN (" + string.Join(",", _oDUStatement.DUReturnChallans.Select(x => x.DUReturnChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                int nKey = 0;
                #region Header
                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "PI No"); headerTable.Add(++nKey, oExportPI.PINo_Full);
                headerTable.Add(++nKey, "PI Date"); headerTable.Add(++nKey, oExportPI.IssueDateInString);
                headerTable.Add(++nKey, "LC No"); headerTable.Add(++nKey, oExportPI.ExportLCNo);

                headerTable.Add(++nKey, "BUYER NAME"); headerTable.Add(++nKey, oExportPI.ContractorName);
                headerTable.Add(++nKey, "MKT Person"); headerTable.Add(++nKey, oExportPI.MKTPName);
                headerTable.Add(++nKey, "Payment Term"); headerTable.Add(++nKey, oExportPI.PaymentTypeSt);
                #endregion

                _oDUStatement.HeaderTable = headerTable;
            }
        }
        private void DOStatement_LC_Fabric(int nBUID, int nExportLCID, int nVersionNo)//___UNDER__CONSTRACTION
        {
            ExportLC oExportLC = new ExportLC();
            oExportLC = oExportLC.Get(nExportLCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (nVersionNo >= 0)
            {
                _oDUStatement.ExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from ExportPILCmapping where isnull(VersionNo,0)=" + nVersionNo + " and Activity=1 and ExportLCID=" + nExportLCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oDUStatement.ExportPIs = ExportPI.Gets("SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from ExportPILCmapping where Activity=1 and ExportLCID=" + nExportLCID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            //_oDUStatement.SampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice  where ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ") and BUID in (0," + nBUID + ") and  CurrentStatus not in (" + (int)EnumSampleInvoiceStatus.Canceled + ")  order by SampleInvoiceID", ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oDUStatement.DUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets("SELECT * FROM View_DUDeliveryChallanRegister as DO WHERE  DO.DUDeliveryChallanID IN (SELECT DUDeliveryChallanID FROM View_DUDeliveryChallan where ExportPIID IN (Select ExportPIID from ExportPILCmapping where Activity=1 and ExportLCID=" + nExportLCID + "))", ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nKey = 0;
            if (_oDUStatement.DUDeliveryChallanRegisters != null && _oDUStatement.DUDeliveryChallanRegisters.Count > 0)
            {
                _oDUStatement.DUReturnChallans = DUReturnChallan.Gets("SELECT * FROM View_DUReturnChallan WHERE DUDeliveryChallanID IN (" + string.Join(",", _oDUStatement.DUDeliveryChallanRegisters.Select(x => x.DUDeliveryChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUStatement.DUReturnChallans != null && _oDUStatement.DUReturnChallans.Count > 0)
                    _oDUStatement.DUReturnChallanDetails = DUReturnChallanDetail.Gets("SELECT * FROM View_DUReturnChallanDetail WHERE DUReturnChallanID IN (" + string.Join(",", _oDUStatement.DUReturnChallans.Select(x => x.DUReturnChallanID)) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                Hashtable headerTable = new Hashtable();
                headerTable.Add(++nKey, "LC No"); headerTable.Add(++nKey, oExportLC.ExportLCNo);
                headerTable.Add(++nKey, "PI No"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.SampleInvoices.Select(x => x.ExportPINo)));
                headerTable.Add(++nKey, "BUYER NAME"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.SampleInvoices.Select(x => x.ContractorName)));
                headerTable.Add(++nKey, "Concern Person"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.SampleInvoices.Select(x => x.ContractorPersopnnalName)));

                headerTable.Add(++nKey, "LC Date"); headerTable.Add(++nKey, oExportLC.OpeningDateST);
                headerTable.Add(++nKey, "LC Term"); headerTable.Add(++nKey, oExportLC.LCTermsName);
                _oDUStatement.HeaderTable = headerTable;
            }
        }
        #endregion

     
    
        #endregion

        #region All Statement Report
        public ActionResult PrintStatement_All(int nBUID, int nExportLCID, int nVersionNo, int nExportPIID, int nDyeingOrderID, int nDOID, int nSampleInvoiceID)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oDUStatement.BUID = nBUID;
            _oDUStatement.Company = oCompany;
            _oDUStatement.BusinessUnit = oBusinessUnit;
            _oDUStatement.BusinessUnitType = oBusinessUnit.BusinessUnitType;
            #endregion
          
            #region DO Statements
            if (nDOID > 0)
            {
                this.DUStatement_DO_All( nBUID,  nDyeingOrderID,  nDOID,  nSampleInvoiceID);
            }
            else if (nDyeingOrderID > 0)
            {
                this.DUStatement_DO_All(nBUID, nDyeingOrderID, nDOID, nSampleInvoiceID);
            }
            if (nSampleInvoiceID > 0)
            {
                this.DUStatement_DO_All(nBUID, nDyeingOrderID, nDOID, nSampleInvoiceID);
            }
            else if (nExportPIID > 0)
            {
                this.DUStatement_PI_All(nBUID, nExportPIID, nExportLCID, nVersionNo);
            }
            else if (nExportLCID > 0)//&& nVersionNo >= 0
            {
                this.DUStatement_PI_All(nBUID, nExportPIID, nExportLCID, nVersionNo);
            }
            #endregion

            _oDUStatement.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserName;
          

            //if ((_oDUStatement.DyeingOrderReports == null || _oDUStatement.DyeingOrderReports.Count <= 0) && (_oDUStatement.SampleInvoices ==null ||_oDUStatement.SampleInvoices.Count <= 0))
            //{
            //    rptErrorMessage oErrorReport = new rptErrorMessage();
            //    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("Sorry, No Data Found!");
            //    return File(aErrorMessagebytes, "application/pdf");
            //}

            rptDUStatement oReport = new rptDUStatement();
            byte[] abytes = oReport.PrepareReport_DU(_oDUStatement);
            return File(abytes, "application/pdf");
        }
        private void DUStatement_PI_All(int nBUID, int nExportPIID, int nExportLCID, int nVersionNo)
        {
            #region PI
            ///Gets PI///
            ExportPI oExportPI = new ExportPI();
            if (nExportPIID > 0)
            {
                oExportPI = oExportPI.Get(nExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUStatement.ExportPIs.Add(oExportPI);
                _oDUStatement.ExportPIDetails = ExportPIDetail.GetsByPI(nExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUStatement.StatementType = EnumStatementType.PI;
            }
            else if(nExportLCID>0 && nVersionNo<0)
            {
                _sSQL = "SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from ExportPILCMapping where Activity=1 and ExportLCID=" + nExportLCID +" )";
                _oDUStatement.ExportPIs = ExportPI.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sSQL = "SELECT * FROM View_ExportPIDetail where ExportPIID in (Select ExportPIID from ExportPILCMapping where Activity=1 and ExportLCID=" + nExportLCID +" )";
                _oDUStatement.ExportPIDetails = ExportPIDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUStatement.StatementType = EnumStatementType.LC;
            }
            else if (nExportLCID > 0 && nVersionNo >=0)
            {
                _sSQL = "SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from ExportPILCMapping where Activity=1 and ExportLCID=" + nExportLCID + "  and VersionNo=" + nVersionNo + " )";
                _oDUStatement.ExportPIs = ExportPI.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sSQL = "SELECT * FROM View_ExportPIDetail where ExportPIID in (Select ExportPIID from ExportPILCMapping where Activity=1 and ExportLCID=" + nExportLCID + "  and VersionNo=" + nVersionNo + " )";
                _oDUStatement.ExportPIDetails = ExportPIDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUStatement.StatementType = EnumStatementType.LC;
            }
            ///end PI
            #endregion
            #region Export LC & Bill
            if (_oDUStatement.ExportPIs.Count> 0)
            {
                ExportLC oExportLC = new ExportLC();
                oExportLC = oExportLC.Get(_oDUStatement.ExportPIs[0].LCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
                List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
                List<ExportBill> oExportBills = new List<ExportBill>();
                if (oExportLC.ExportLCID > 0)
                {
                    _sSQL = "SELECT * FROM View_ExportBill as EB WHERE EB.ExportLCID=" + oExportLC.ExportLCID + " and [State]<>11 and EB.ExportBillID in (Select EBD.ExportBillID from ExportBillDetail as EBD where  EBD.ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ")))";
                    oExportBills = ExportBill.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oExportBills.Count > 0)
                {
                    _sSQL = "SELECT * FROM View_ExportBillDetail as EBD WHERE EBD.ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID in  (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ") )";
                    oExportBillDetails = ExportBillDetail.GetsBySQL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                _oDUStatement.ExportBills = oExportBills;
                _oDUStatement.ExportLC = oExportLC;
                _oDUStatement.MaturityDate = "";
                _oDUStatement.PaymentDate ="";
                foreach (ExportBill oItem in oExportBills)
                {
                    List<ExportBillDetail> oEB_Details = new List<ExportBillDetail>();
                    oEB_Details = oExportBillDetails.Where(o => o.ExportBillID == oItem.ExportBillID).ToList();
                    foreach (ExportBillDetail oItemDCD in oEB_Details)
                    {
                        if (oItem.State >= EnumLCBillEvent.BOEInCustomerHand)
                        {
                            _oDUStatement.AcceptanceIssue = _oDUStatement.AcceptanceIssue + oItemDCD.Qty;
                        }
                        if (oItem.State >= EnumLCBillEvent.BuyerAcceptedBill)
                        {
                            _oDUStatement.AcceptanceRcvd = _oDUStatement.AcceptanceRcvd + oItemDCD.Qty;
                                          
                        }

                    }
                    if (oItem.State >= EnumLCBillEvent.BankAcceptedBill)
                    {
                        _oDUStatement.MaturityDate = oItem.MaturityDateSt;
                        _oDUStatement.PaymentDate = oItem.RelizationDateSt;
                    }
                }
            }
            #endregion
            if (_oDUStatement.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
                #region Production Order
                List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
                if (_oDUStatement.ExportPIs.Count > 0)
                {
                    oDyeingOrderReports = DyeingOrderReport.Gets("Select * from View_DyeingOrderReport where [Status]<9 and  ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                _oDUStatement.DyeingOrderReports = oDyeingOrderReports;
                _oDUStatement.Currency = "$";
                if (oDyeingOrderReports.Count > 0)
                {
                    _oDUStatement.MUName = oDyeingOrderReports[0].MUName;
                }
                #endregion
                #region Delivery Order
                List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();

                List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
                List<DUDeliveryOrderDetail> oDUDeliveryOrderDetailsTwo = new List<DUDeliveryOrderDetail>();
                if (_oDUStatement.ExportPIs.Any())
                {
                    _sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE ExportSCDetailID>0 and ExportSCDetailID in (Select ExportSCDetailID from ExportSCDetail where ExportSCID in (Select ExportSCID from ExportSC where ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ") ))  and DOD.DUDeliveryOrderID in (SELECT DO.DUDeliveryOrderID FROM DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ") )";
                    oDUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    _sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE DyeingOrderDetailID>0 and DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where [Status]<9 and DyeingOrderType not in ( " + (int)EnumOrderType.ClaimOrder + "," + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + " ) and ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ")))";
                    oDUDeliveryOrderDetailsTwo = DUDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDUDeliveryOrderDetailsTwo.ForEach(x => oDUDeliveryOrderDetails.Add(x));

                    if (oDUDeliveryOrderDetails.Any())
                    {
                        _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID in (" + string.Join(",", oDUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderID).Distinct().ToList()) + ")";

                        oDUDeliveryOrders = DUDeliveryOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }

                if (oDUDeliveryOrders.Count > 0)
                {
                    _oDUStatement.DUDeliveryOrderDetails = oDUDeliveryOrderDetails;
                    foreach (DUDeliveryOrder oItem in oDUDeliveryOrders)
                    {
                        if (oItem.ApproveBy != 0)
                        {
                            _oDUStatement.DUDeliveryOrderDetails.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList().ForEach(o => o.OrderNo = oItem.DONoFull);
                            _oDUStatement.DUDeliveryOrderDetails.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList().ForEach(o => o.DeliveryDate = oItem.DODate);
                        }
                    }

                }
                #endregion
                #region DU DeliveryChallan
                if (_oDUStatement.DUDeliveryOrderDetails.Count > 0)
                {
                    _sSQL = "SELECT * FROM VIEW_DUDeliveryChallan as DC WHERE  IsDelivered=1 and DC.DUDeliveryChallanID in (SELECT DCD.DUDeliveryChallanID FROM DUDeliveryChallanDetail as DCD where DCD.DODetailID in (" + string.Join(",", _oDUStatement.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderDetailID).ToList()) + "))";
                    _oDUStatement.DUDeliveryChallans = DUDeliveryChallan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oDUStatement.DUDeliveryChallans.Count > 0)
                {
                    List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
                    _sSQL = "SELECT * FROM View_DUDeliveryChallanDetail as DCD where DCD.DODetailID in (" + string.Join(",", _oDUStatement.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderDetailID).ToList()) + ")";
                    oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    _oDUStatement.DUDeliveryChallanDetails = oDUDeliveryChallanDetails;
                    foreach (DUDeliveryChallan oItem in _oDUStatement.DUDeliveryChallans)
                    {
                        oDUDeliveryChallanDetails.Where(o => o.DUDeliveryChallanID == oItem.DUDeliveryChallanID).ToList().ForEach(o => o.ChallanNo = oItem.ChallanNo);
                        oDUDeliveryChallanDetails.Where(o => o.DUDeliveryChallanID == oItem.DUDeliveryChallanID).ToList().ForEach(o => o.ChallanDate = oItem.ChallanDate.ToString("dd MMM yyyy"));

                    }
                    _oDUStatement.DUDeliveryChallanDetails = oDUDeliveryChallanDetails;

                    #region Return Challan
                    if (_oDUStatement.DUDeliveryChallans.Count > 0)
                    {
                        _sSQL = "SELECT * FROM View_DUReturnChallan where isnull(ApprovedBy,0)!=0 and DUDeliveryChallanID in  (" + string.Join(",", _oDUStatement.DUDeliveryChallans.Select(x => x.DUDeliveryChallanID).ToList()) + ")";
                        _oDUStatement.DUReturnChallans = DUReturnChallan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    if (_oDUStatement.DUReturnChallans.Count > 0)
                    {
                        List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                        _sSQL = "Select * from View_DUReturnChallanDetail where DUReturnChallanID in (SELECT DUReturnChallanID FROM DUReturnChallan where isnull(ApprovedBy,0)!=0 and DUDeliveryChallanID in  (" + string.Join(",", _oDUStatement.DUDeliveryChallans.Select(x => x.DUDeliveryChallanID).ToList()) + "))";
                        oDUReturnChallanDetails = DUReturnChallanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        foreach (DUReturnChallan oItem in _oDUStatement.DUReturnChallans)
                        {
                            oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList().ForEach(o => o.RCNo = oItem.DUReturnChallanNo);
                            oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList().ForEach(o => o.RCDate = oItem.ReturnDate.ToString("dd MMM yyyy"));

                        }
                        _oDUStatement.DUReturnChallanDetails = oDUReturnChallanDetails;
                    }
                    #endregion End Return Challan
                }
                #endregion
                #region Claim Order
                ////
                //List<DUClaimOrder> oDUClaimOrders = new List<DUClaimOrder>();
                //sSQL = "SELECT * FROM View_DUClaimorder WHERE OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")  and OrderID=" + oExportSCDO.ExportSCID;
                _sSQL = "SELECT * FROM View_DUClaimorder WHERE  ParentDOID in (Select DyeingOrderID from DyeingOrder where DyeingOrder.ExportPIID in  (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + "))";
                _oDUStatement.DUClaimOrders = DUClaimOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUStatement.DUClaimOrders.Count > 0)
                {
                    List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
                    _sSQL = "Select * from View_DUClaimOrderDetail where DUClaimOrderID in  (" + string.Join(",", _oDUStatement.DUClaimOrders.Select(x => x.DUClaimOrderID).ToList()) + ")";
                    oDUClaimOrderDetails = DUClaimOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (DUClaimOrder oItem in _oDUStatement.DUClaimOrders)
                    {
                        //if (oItem.ApproveBy != 0)
                        //{
                        List<DUClaimOrderDetail> oDU_DCDs = new List<DUClaimOrderDetail>();
                        oDU_DCDs = oDUClaimOrderDetails.Where(o => o.DUClaimOrderID == oItem.DUClaimOrderID).ToList();
                        foreach (DUClaimOrderDetail oItemDCD in oDU_DCDs)
                        {
                            //oItemDCD.Date = oItem.OrderDate.ToString("dd MMM yyyy");
                            oItemDCD.ClaimOrderNo = oItem.ClaimOrderNo;
                            _oDUStatement.DUClaimOrderDetails.Add(oItemDCD);
                        }
                        //}
                    }
                }
                ///
                #endregion
                _oDUStatement.Qty_DC = _oDUStatement.DUDeliveryChallanDetails.Select(c => c.Qty).Sum();
                _oDUStatement.Qty_DC = _oDUStatement.Qty_DC + _oDUStatement.DUDeliveryChallanDetails_Claim.Select(c => c.Qty).Sum();
                _oDUStatement.Qty_PO = _oDUStatement.DyeingOrderReports.Select(c => c.Qty).Sum();
               
                _oDUStatement.Qty_DO = _oDUStatement.Qty_DO + _oDUStatement.DUDeliveryOrderDetails.Select(c => c.Qty).Sum();
            }
            if (_oDUStatement.BusinessUnitType == EnumBusinessUnitType.Weaving || _oDUStatement.BusinessUnitType == EnumBusinessUnitType.Finishing)
            {
                _sSQL = "SELECT * FROM View_FabricDeliveryChallan WHERE FDOID IN (SELECT FDOID  FROM FabricDeliveryOrderDetail WHERE ExportPIID IN (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + "))";

                //sSQL = "SELECT * FROM View_FabricDeliveryChallan";
                _oDUStatement.FabricDeliveryChallans = FabricDeliveryChallan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUStatement.FabricDeliveryChallans.Count > 0)
                {
                    List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
                    _sSQL = "SELECT * FROM View_FabricDeliveryChallanDetail as FDCD WHERE FDCD.FDODID IN (SELECT FDOD.FDODID FROM FabricDeliveryOrderDetail as FDOD WHERE ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + "))";
                    oFabricDeliveryChallanDetails = FabricDeliveryChallanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (FabricDeliveryChallan oItem in _oDUStatement.FabricDeliveryChallans)
                    {
                        List<FabricDeliveryChallanDetail> oFDCDs = new List<FabricDeliveryChallanDetail>();
                        oFDCDs = oFabricDeliveryChallanDetails.Where(o => o.FDCID == oItem.FDCID).ToList();
                        foreach (FabricDeliveryChallanDetail oItemFDCD in oFDCDs)
                        {
                            //oItemFDCD.ChallanDate = oItem.IssueDate;
                            oItemFDCD.ChallanNo = oItem.ChallanNo;
                            _oDUStatement.FabricDeliveryChallanDetails.Add(oItemFDCD);
                        }
                    }
                    _oDUStatement.Qty_DC = _oDUStatement.FabricDeliveryChallanDetails.Select(c => c.Qty * c.UnitPrice).Sum();
                }
                _oDUStatement.Qty_DC = _oDUStatement.DUDeliveryChallanDetails.Select(c => c.Qty).Sum();
                _oDUStatement.Qty_DC = _oDUStatement.Qty_DC + _oDUStatement.DUDeliveryChallanDetails_Claim.Select(c => c.Qty).Sum();
            
            }
             _oDUStatement.Qty_Total =  _oDUStatement.ExportPIs.Select(c => c.Qty).Sum();
            
           
             //_oDUStatement. = _oDUStatement.Qty_Total - _oDUStatement.Qty_PO;
            #region HeaderTable
            int nKey = 0;
            if (_oDUStatement.ExportPIs.Count > 0)
            {
                Hashtable headerTable = new Hashtable();
                //1
                if (_oDUStatement.StatementType == EnumStatementType.PI)
                {
                    headerTable.Add(++nKey, "P/I No"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportPIs.Select(x => x.PINo).Distinct()));
                    headerTable.Add(++nKey, "P/I Date"); if (_oDUStatement.ExportPIs.Count > 0) { headerTable.Add(++nKey, _oDUStatement.ExportPIs[0].IssueDateInString); } else { headerTable.Add(++nKey, ""); }
                    headerTable.Add(++nKey, "P/I Amount"); headerTable.Add(++nKey, _oDUStatement.Currency + "" + Global.MillionFormatActualDigit(_oDUStatement.ExportPIs.Select(c => c.Amount).Sum()));
                }
                //2
                headerTable.Add(++nKey, "L/C No"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportPIs.Select(x => x.ExportLCNo).Distinct()));
                headerTable.Add(++nKey, "L/C Date"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportLC.OpeningDateST));
                headerTable.Add(++nKey, "L/C Amount"); headerTable.Add(++nKey, _oDUStatement.Currency + "" + Global.MillionFormat(_oDUStatement.ExportLC.Amount));
                //3
                headerTable.Add(++nKey, "Account Of"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportPIs.Select(x => x.ContractorName).Distinct()));
                string nValue = "--";
                if (_oDUStatement.AcceptanceIssue > 0)
                    nValue = Global.MillionFormatActualDigit(_oDUStatement.AcceptanceIssue ) + " " + _oDUStatement.ExportPIDetails[0].MUName;
                headerTable.Add(++nKey, "Acceptance Issue"); headerTable.Add(++nKey, nValue);
                headerTable.Add(++nKey, "Maturity Date"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.MaturityDate));
                //4
                headerTable.Add(++nKey, "Buying House"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportPIs.Select(x => x.BuyerName).Distinct()));

                nValue = "--";
                if (_oDUStatement.AcceptanceRcvd > 0)
                    nValue = Global.MillionFormatActualDigit(_oDUStatement.AcceptanceRcvd) + " " + _oDUStatement.ExportPIDetails[0].MUName;
                headerTable.Add(++nKey, "Acceptance Recd"); headerTable.Add(++nKey, nValue);
                headerTable.Add(++nKey, "Payment Date"); headerTable.Add(++nKey, _oDUStatement.PaymentDate);
                //5
                headerTable.Add(++nKey, "Factory Concern"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportPIs.Select(x => x.ContractorContactPersonName).Distinct()));
                headerTable.Add(++nKey, "L/C Status"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportPIs.Select(x => x.ExportLCStatusSt).Distinct()));
                headerTable.Add(++nKey, "UD Recvd Date"); headerTable.Add(++nKey, "");
                //6
                headerTable.Add(++nKey, "Buying Concern"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportPIs.Select(x => x.BuyerContactPersonName).Distinct()));
                headerTable.Add(++nKey, "Concern"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.ExportPIs.Select(x => x.MKTPName).Distinct()));
                headerTable.Add(++nKey, ""); headerTable.Add(++nKey, "");
                _oDUStatement.HeaderTable = headerTable;
            }
            #endregion
        }
        private void DUStatement_DO_All(int nBUID, int nDyeingOrderID, int nDOID, int nSampleInvoiceID)
        {
            #region Production Order
            SampleInvoice oSampleInvoice = new SampleInvoice();
            List<SampleInvoice> oSampleInvoices = new List<SampleInvoice>();
            List<DyeingOrderReport> oDyeingOrderReports = new List<DyeingOrderReport>();
            if (nDOID > 0)
            {
                oDyeingOrderReports = DyeingOrderReport.Gets("Select * from View_DyeingOrderReport where [Status]<9 and DyeingOrderDetailID>0 and DyeingOrderDetailID in(Select DyeingOrderDetailID from DUDeliveryOrderDetail where DUDeliveryOrderID=" + nDOID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUStatement.StatementType = EnumStatementType.DeliveryOrder;
            }
            else if (nDyeingOrderID > 0)
            {
                oDyeingOrderReports = DyeingOrderReport.Gets("Select * from View_DyeingOrderReport where [Status]<9 and DyeingOrderID=" + nDyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region ORDER STATUS
                _oDUStatement.DUProGuideLineDetails_Receive = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) != " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID=" + nDyeingOrderID + ") ORDER BY ProductID", (int)Session[SessionInfo.currentUserID]);
                _oDUStatement.DUProGuideLineDetails_Return = DUProGuideLineDetail.Gets("SELECT * FROM View_DUProGuideLineDetail WHERE DUProGuideLineID IN (SELECT DUProGuideLineID FROM DUProGuideLine WHERE ISNULL(ReceiveByID,0) !=0 AND ISNULL(InOutType,0) = " + (int)EnumInOutType.Disburse + " AND  DyeingOrderID=" + nDyeingOrderID + ") ORDER BY ProductID", (int)Session[SessionInfo.currentUserID]);
                _oDUStatement.DURequisitionDetails = DURequisitionDetail.Gets("SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID = " + nDyeingOrderID + "  AND DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE ISNULL(ReceiveByID,0)<>0 ) ", (int)Session[SessionInfo.currentUserID]);
                _oDUStatement.LotParents = LotParent.Gets("SELECT * FROM View_LotParent WHERE DyeingOrderID = " + nDyeingOrderID + " OR DyeingOrderID_Out = " + nDyeingOrderID, (int)Session[SessionInfo.currentUserID]);
                #endregion

                _oDUStatement.StatementType = EnumStatementType.ProductionOrder;
                if (oDyeingOrderReports.Count > 0)
                {
                    oSampleInvoices = SampleInvoice.Gets("SELECT * FROM View_SampleInvoice where SampleInvoiceID in (Select SampleInvoiceID from DyeingOrder where [Status]<9 and DyeingOrderID="+ nDyeingOrderID+")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oSampleInvoices.Count > 0)
                    {
                        oSampleInvoice = oSampleInvoices[0];
                    }
                    _oDUStatement.Currency = oDyeingOrderReports[0].Currency;
                }
            }
            else if (nSampleInvoiceID > 0)
            {
                oSampleInvoice = oSampleInvoice.Get(nSampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrderReports = DyeingOrderReport.Gets("Select * from View_DyeingOrderReport where [Status]<9 and SampleInvoiceID=" + nSampleInvoiceID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUStatement.StatementType = EnumStatementType.Invoice;
                _oDUStatement.SampleInvoices = new List<SampleInvoice>();
                _oDUStatement.SampleInvoices.Add(oSampleInvoice);
                _oDUStatement.Currency = oSampleInvoice.CurrencySymbol;
            }
            _oDUStatement.DyeingOrderReports = oDyeingOrderReports;
            //_oDUStatement.SampleInvoices = oDyeingOrderReports;
          
            if (oDyeingOrderReports.Count > 0)
            {
                _oDUStatement.MUName = oDyeingOrderReports[0].MUName;
            }
            #endregion
            #region Delivery Order
            List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();
            List<DUDeliveryOrder> oDUDeliveryOrdersTwo = new List<DUDeliveryOrder>();
            if (nDOID > 0)
            {
                _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID =" + nDOID;
            }
            else if (nDyeingOrderID > 0)
            {
                _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + nDyeingOrderID + "))";
            }
            else if (nSampleInvoiceID > 0)
            {
                _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where SampleInvoiceID=" + nSampleInvoiceID + ")))";
            }
       
            oDUDeliveryOrders = DUDeliveryOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUDeliveryOrders.Count > 0)
            {
                List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
               
                if (nDOID > 0)
                {
                    _sSQL = "Select * from View_DUDeliveryOrderDetail where DUDeliveryOrderID in(" + string.Join(",", oDUDeliveryOrders.Select(x => x.DUDeliveryOrderID).ToList()) + ")";
                }
                else if (nDyeingOrderID > 0)
                {
                   // _sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + nDyeingOrderID + "))";
                    _sSQL = "Select * from View_DUDeliveryOrderDetail where DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID=" + nDyeingOrderID + ")";
                }
                else if (nSampleInvoiceID > 0)
                {
                    _sSQL = "Select * from View_DUDeliveryOrderDetail where  DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where SampleInvoiceID=" + nSampleInvoiceID + "))";
                }
                oDUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oDUStatement.DUDeliveryOrderDetails = oDUDeliveryOrderDetails;
                foreach (DUDeliveryOrder oItem in oDUDeliveryOrders)
                {
                    if (oItem.ApproveBy != 0)
                    {
                        _oDUStatement.DUDeliveryOrderDetails.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList().ForEach(o => o.OrderNo = oItem.DONoFull);
                        _oDUStatement.DUDeliveryOrderDetails.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList().ForEach(o => o.DeliveryDate = oItem.DODate);
                        //_oDUStatement.DUDeliveryOrderDetails.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList().ForEach(o => o.UnitPrice = oItem.DODate);
                    }
                }
                _oDUStatement.DUDeliveryOrders = oDUDeliveryOrders;
            }
            #endregion
            #region Challan
            if (_oDUStatement.DUDeliveryOrderDetails.Count > 0)
            {
                _sSQL = "SELECT * FROM VIEW_DUDeliveryChallan as DC WHERE  IsDelivered=1 and DC.DUDeliveryChallanID in (SELECT DCD.DUDeliveryChallanID FROM DUDeliveryChallanDetail as DCD where DCD.DODetailID in (" + string.Join(",", _oDUStatement.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderDetailID).ToList()) + "))";
                _oDUStatement.DUDeliveryChallans = DUDeliveryChallan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (_oDUStatement.DUDeliveryChallans.Count > 0)
            {
                List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
                _sSQL = "SELECT * FROM View_DUDeliveryChallanDetail as DCD where DCD.DODetailID in (" + string.Join(",", _oDUStatement.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderDetailID).ToList()) + ")";
                oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDUStatement.DUDeliveryChallanDetails = oDUDeliveryChallanDetails;
                foreach (DUDeliveryChallan oItem in _oDUStatement.DUDeliveryChallans)
                {
                    oDUDeliveryChallanDetails.Where(o => o.DUDeliveryChallanID == oItem.DUDeliveryChallanID).ToList().ForEach(o => o.ChallanNo = oItem.ChallanNo);
                    oDUDeliveryChallanDetails.Where(o => o.DUDeliveryChallanID == oItem.DUDeliveryChallanID).ToList().ForEach(o => o.ChallanDate = oItem.ChallanDate.ToString("dd MMM yyyy"));

                }
                _oDUStatement.DUDeliveryChallanDetails = oDUDeliveryChallanDetails;

                #region Return Challan
                if (_oDUStatement.DUDeliveryChallans.Count > 0)
                {
                    _sSQL = "SELECT * FROM View_DUReturnChallan where isnull(ApprovedBy,0)!=0 and DUDeliveryChallanID in  (" + string.Join(",", _oDUStatement.DUDeliveryChallans.Select(x => x.DUDeliveryChallanID).ToList()) + ")";
                    _oDUStatement.DUReturnChallans = DUReturnChallan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oDUStatement.DUReturnChallans.Count > 0)
                {
                    List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                    _sSQL = "Select * from View_DUReturnChallanDetail where DUReturnChallanID in (SELECT DUReturnChallanID FROM DUReturnChallan where isnull(ApprovedBy,0)!=0 and DUDeliveryChallanID in  (" + string.Join(",", _oDUStatement.DUDeliveryChallans.Select(x => x.DUDeliveryChallanID).ToList()) + "))";
                    oDUReturnChallanDetails = DUReturnChallanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DUReturnChallan oItem in _oDUStatement.DUReturnChallans)
                    {
                        oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList().ForEach(o => o.RCNo = oItem.DUReturnChallanNo);
                        oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList().ForEach(o => o.RCDate = oItem.ReturnDate.ToString("dd MMM yyyy"));

                    }
                    _oDUStatement.DUReturnChallanDetails = oDUReturnChallanDetails;
                }
                #endregion End Return Challan
            }
            #endregion
            #region Claim Order
            ////
            //List<DUClaimOrder> oDUClaimOrders = new List<DUClaimOrder>();
            if (_oDUStatement.DyeingOrderReports.Count > 0)
            {
                _sSQL = "SELECT * FROM View_DUClaimorder WHERE  ParentDOID  in  (" + string.Join(",", _oDUStatement.DyeingOrderReports.Select(x => x.DyeingOrderID).Distinct().ToList()) + ")";
                _oDUStatement.DUClaimOrders = DUClaimOrder.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (_oDUStatement.DUClaimOrders.Count > 0)
            {
                List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
                _sSQL = "Select * from View_DUClaimOrderDetail where DUClaimOrderID in  (" + string.Join(",", _oDUStatement.DUClaimOrders.Select(x => x.DUClaimOrderID).ToList()) + ")";
                oDUClaimOrderDetails = DUClaimOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DUClaimOrder oItem in _oDUStatement.DUClaimOrders)
                {
                    //if (oItem.ApproveBy != 0)
                    //{
                    List<DUClaimOrderDetail> oDU_DCDs = new List<DUClaimOrderDetail>();
                    oDU_DCDs = oDUClaimOrderDetails.Where(o => o.DUClaimOrderID == oItem.DUClaimOrderID).ToList();
                    foreach (DUClaimOrderDetail oItemDCD in oDU_DCDs)
                    {
                        //oItemDCD.Date = oItem.OrderDate.ToString("dd MMM yyyy");
                        oItemDCD.ClaimOrderNo = oItem.ClaimOrderNo;
                        _oDUStatement.DUClaimOrderDetails.Add(oItemDCD);
                    }
                    //}
                }

            }
            ///
            #endregion
            #region PI
            ///Gets PI///
            ExportPI oExportPI = new ExportPI();

            if (oDyeingOrderReports.Count>0)
            {
                if (nDOID > 0)
                {
                    _sSQL = "SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from View_DyeingOrderReport where [Status]<9 and DyeingOrderDetailID>0 and DyeingOrderDetailID in(Select DyeingOrderDetailID from DUDeliveryOrderDetail where DUDeliveryOrderID=" + nDOID + "))";
                }
                else if (nDyeingOrderID > 0)
                {
                    _sSQL = "SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from DyeingOrder where [Status]<9 and DyeingOrderID=" + nDyeingOrderID + ")";
                }
                else if (nSampleInvoiceID > 0)
                {
                    _sSQL = "SELECT * FROM View_ExportPI where ExportPIID in (Select ExportPIID from DyeingOrder where [Status]<9 and SampleInvoiceID=" + nSampleInvoiceID + ")";
                }
                _oDUStatement.ExportPIs = ExportPI.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oDUStatement.ExportPIs.Count > 0)
                {
                    _sSQL = "SELECT * FROM View_ExportPIDetail where ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + " )";
                    _oDUStatement.ExportPIDetails = ExportPIDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               
            }
           
            ///end PI
            #endregion
            #region Export LC & Bill
            if (_oDUStatement.ExportPIs.Count > 0)
            {
                ExportLC oExportLC = new ExportLC();
                oExportLC = oExportLC.Get(_oDUStatement.ExportPIs[0].LCID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
                List<ExportBill> oExportBills = new List<ExportBill>();
                if (oExportLC.ExportLCID > 0)
                {
                    _sSQL = "SELECT * FROM View_ExportBill as EB WHERE EB.ExportLCID=" + oExportLC.ExportLCID + " and [State]<>11 and EB.ExportBillID in (Select EBD.ExportBillID from ExportBillDetail as EBD where  EBD.ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID in (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ")))";
                    oExportBills = ExportBill.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oExportBills.Count > 0)
                {
                    _sSQL = "SELECT * FROM View_ExportBillDetail as EBD WHERE EBD.ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID in  (" + string.Join(",", _oDUStatement.ExportPIs.Select(x => x.ExportPIID).ToList()) + ") )";
                    oExportBillDetails = ExportBillDetail.GetsBySQL(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                _oDUStatement.ExportBills = oExportBills;
                _oDUStatement.ExportLC = oExportLC;
                _oDUStatement.MaturityDate = "";
                _oDUStatement.PaymentDate = "";
                foreach (ExportBill oItem in oExportBills)
                {
                    List<ExportBillDetail> oEB_Details = new List<ExportBillDetail>();
                    oEB_Details = oExportBillDetails.Where(o => o.ExportBillID == oItem.ExportBillID).ToList();
                    foreach (ExportBillDetail oItemDCD in oEB_Details)
                    {
                        if (oItem.State >= EnumLCBillEvent.BOEInCustomerHand)
                        {
                            _oDUStatement.AcceptanceIssue = _oDUStatement.AcceptanceIssue + oItemDCD.Qty;
                        }
                        if (oItem.State >= EnumLCBillEvent.BuyerAcceptedBill)
                        {
                            _oDUStatement.AcceptanceRcvd = _oDUStatement.AcceptanceRcvd + oItemDCD.Qty;

                        }

                    }
                    if (oItem.State >= EnumLCBillEvent.BankAcceptedBill)
                    {
                        _oDUStatement.MaturityDate = oItem.MaturityDateSt;
                        _oDUStatement.PaymentDate = oItem.RelizationDateSt;
                    }
                }
            }
            #endregion

            _oDUStatement.Qty_Total = _oDUStatement.ExportPIs.Select(c => c.Qty).Sum();
            _oDUStatement.Qty_PO = _oDUStatement.DyeingOrderReports.Select(c => c.Qty).Sum();
           // _oDUStatement.Qty_DO = _oDUStatement.DUDeliveryOrderDetails.Select(c => c.Qty).Sum();
            _oDUStatement.Qty_DO = _oDUStatement.Qty_DO + _oDUStatement.DUDeliveryOrderDetails.Select(c => c.Qty).Sum();
            _oDUStatement.Qty_DC = _oDUStatement.DUDeliveryChallanDetails.Select(c => c.Qty).Sum();
            _oDUStatement.Qty_DC = _oDUStatement.Qty_DC + _oDUStatement.DUDeliveryChallanDetails_Claim.Select(c => c.Qty).Sum();
            //_oDUStatement. = _oDUStatement.Qty_Total - _oDUStatement.Qty_PO;
            #region HeaderTable
            int nKey = 0;
            //if (_oDUStatement.ExportPIs.Count > 0)
            //{
                Hashtable headerTable = new Hashtable();
               
                if (_oDUStatement.StatementType == EnumStatementType.ProductionOrder)
                {
                    //1
                    headerTable.Add(++nKey, "Order No"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.DyeingOrderReports.Select(x => x.OrderNoFull).Distinct()));
                    headerTable.Add(++nKey, "Order Date"); if (_oDUStatement.DyeingOrderReports.Count > 0) { headerTable.Add(++nKey, _oDUStatement.DyeingOrderReports[0].OrderDateSt); } else { headerTable.Add(++nKey, ""); }
                    headerTable.Add(++nKey, "Amount"); headerTable.Add(++nKey, _oDUStatement.Currency + "" + Global.MillionFormatActualDigit(_oDUStatement.DyeingOrderReports.Select(c => c.Qty).Sum()));
                    //2
                    if (oSampleInvoice.SampleInvoiceID > 0)
                    {
                        headerTable.Add(++nKey, "Bill No"); headerTable.Add(++nKey, oSampleInvoice.SampleInvoiceNo);
                        headerTable.Add(++nKey, "Bill Date"); if (_oDUStatement.DyeingOrderReports.Count > 0) { headerTable.Add(++nKey, oSampleInvoice.SampleInvoiceDateST); } else { headerTable.Add(++nKey, ""); }
                        headerTable.Add(++nKey, "Amount"); headerTable.Add(++nKey, _oDUStatement.Currency + "" + Global.MillionFormat(oSampleInvoice.Amount));
                    }
                }
                else  if (_oDUStatement.StatementType == EnumStatementType.Invoice)
                {
                    //1
                    headerTable.Add(++nKey, "Bill No"); headerTable.Add(++nKey, oSampleInvoice.SampleInvoiceNo);
                    headerTable.Add(++nKey, "Bill Date"); if (_oDUStatement.DyeingOrderReports.Count > 0) { headerTable.Add(++nKey, oSampleInvoice.SampleInvoiceDateST); } else { headerTable.Add(++nKey, ""); }
                    headerTable.Add(++nKey, "Amount"); headerTable.Add(++nKey, _oDUStatement.Currency + "" + Global.MillionFormat(oSampleInvoice.Amount));
                }
                else if (_oDUStatement.StatementType == EnumStatementType.DeliveryOrder)
                {
                    //1
                    headerTable.Add(++nKey, "DO No"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.DUDeliveryOrders.Select(x => x.DONoFull).Distinct()));
                    headerTable.Add(++nKey, "DO Date"); if (_oDUStatement.DUDeliveryOrders.Count > 0) { headerTable.Add(++nKey, _oDUStatement.DUDeliveryOrders[0].DODateSt); } else { headerTable.Add(++nKey, ""); }
                    headerTable.Add(++nKey, "Amount"); headerTable.Add(++nKey, _oDUStatement.Currency + "" + Global.MillionFormat(_oDUStatement.DUDeliveryOrderDetails.Select(c => (c.Qty*c.UnitPrice)).Sum()));
                }

                //3
                headerTable.Add(++nKey, "Account Of"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.DyeingOrderReports.Select(x => x.ContractorName).Distinct()));
                headerTable.Add(++nKey, "Concern"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.DyeingOrderReports.Select(x => x.CPName).Distinct()));
                headerTable.Add(++nKey, ""); headerTable.Add(++nKey, string.Join(", ", ""));

                //3
                headerTable.Add(++nKey, "Buying House"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.DyeingOrderReports.Select(x => x.DeliveryToName).Distinct()));
                headerTable.Add(++nKey, "Concern"); headerTable.Add(++nKey, string.Join(", ", ""));
                headerTable.Add(++nKey, ""); headerTable.Add(++nKey, string.Join(", ", ""));

                  //3
                headerTable.Add(++nKey, "Concern"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.DyeingOrderReports.Select(x => x.MKTPName).Distinct()));
                headerTable.Add(++nKey, ""); headerTable.Add(++nKey, string.Join(", ", ""));
                headerTable.Add(++nKey, ""); headerTable.Add(++nKey, string.Join(", ", ""));

                //2
                if (_oDUStatement.ExportPIs.Count > 0)
                {
                    headerTable.Add(++nKey, "P/I No"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.DyeingOrderReports.Select(x => x.PINo).Distinct()));
                    headerTable.Add(++nKey, "L/C No"); headerTable.Add(++nKey, string.Join(", ", _oDUStatement.DyeingOrderReports.Select(x => x.ExportLCNo).Distinct()));
                    headerTable.Add(++nKey, "Amount"); headerTable.Add(++nKey, _oDUStatement.Currency + "" + Global.MillionFormatActualDigit(_oDUStatement.ExportPIs.Select(c => c.Amount).Sum()));
                }
                _oDUStatement.HeaderTable = headerTable;
            //}
            #endregion
        }
        private void DUStatement_FabricSC_All(int nBUID, int nFabricSCID, int nDOID, int nSampleInvoiceID)
        {
            //BusinessUnit oBusinessUnit = new BusinessUnit();
            //oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //_oDUStatement.BUID = nBUID;
       
            //_oDUStatement.BusinessUnit = oBusinessUnit;
            //_oDUStatement.BusinessUnitType = oBusinessUnit.BusinessUnitType;
            FabricDeliveryOrder oFDO = new FabricDeliveryOrder();
            List<FabricDeliveryOrder> oFDOs = new List<FabricDeliveryOrder>();
            List<FabricDeliveryOrderDetail> oFDOD = new List<FabricDeliveryOrderDetail>();
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            if (nDOID > 0)
            {
                oFDOD = FabricDeliveryOrderDetail.Gets(nDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFDO = oFDO.Get(nDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (nFabricSCID > 0)
            {
                _sSQL = "Select * from View_FabricDeliveryOrderDetail where FEOID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractID=" +nFabricSCID+ ")";
                oFDOD = FabricDeliveryOrderDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFDOD.Count > 0)
                {
                    oFDOs = FabricDeliveryOrder.Gets("Select * from View_FabricDeliveryOrder where FDOID in (" + string.Join(",", oFDOD.Select(x => x.FDOID).ToList()) + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                _sSQL = "SELECT * FROM View_FabricDeliveryChallanDetail as DCD WHERE DCD.FSCDID in (Select FabricSalesContractDetailID from FabricSalesContractDetail where FabricSalesContractID=" +nFabricSCID+")";
                oFabricDeliveryChallanDetails = FabricDeliveryChallanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sSQL = "SELECT * FROM View_FabricDeliveryChallan WHERE DisburseBy <> 0 AND FDOID =" + nDOID + "";
                _oDUStatement.FabricDeliveryChallans = FabricDeliveryChallan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }


          
            if (_oDUStatement.BusinessUnitType == EnumBusinessUnitType.Weaving)
            {
                _sSQL = "SELECT * FROM View_FabricDeliveryChallan WHERE DisburseBy <> 0 AND FDOID =" + nDOID + "";
                _oDUStatement.FabricDeliveryChallans = FabricDeliveryChallan.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oDUStatement.FabricDeliveryChallans.Count > 0)
                {
                 
                    string sFDCIDs = string.Join(",", _oDUStatement.FabricDeliveryChallans.Select(x => x.FDCID));
                    if (!string.IsNullOrEmpty(sFDCIDs))
                    {
                        _sSQL = "SELECT * FROM View_FabricDeliveryChallanDetail as DCD WHERE DCD.FDCID IN (" + sFDCIDs + ")";
                        oFabricDeliveryChallanDetails = FabricDeliveryChallanDetail.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                    foreach (FabricDeliveryChallan oItem in _oDUStatement.FabricDeliveryChallans)
                    {
                        List<FabricDeliveryChallanDetail> oFDCDs = new List<FabricDeliveryChallanDetail>();
                        oFDCDs = oFabricDeliveryChallanDetails.Where(o => o.FDCID == oItem.FDCID).ToList();
                        foreach (FabricDeliveryChallanDetail oItemFDCD in oFDCDs)
                        {
                            oItemFDCD.ChallanDate = oItem.IssueDate;
                            oItemFDCD.ChallanNo = oItem.ChallanNo;
                            _oDUStatement.FabricDeliveryChallanDetails.Add(oItemFDCD);
                        }
                    }
                  //  _oDUStatement. = _oDUStatement.FabricDeliveryChallanDetails.Select(c => c.Qty * c.UnitPrice).Sum();
                }
            }
           

            
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
    }
}
