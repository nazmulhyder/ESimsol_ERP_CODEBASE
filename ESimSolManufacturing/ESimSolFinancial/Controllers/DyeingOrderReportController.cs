using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DyeingOrderReportController : Controller
    {
        #region Declaration
        DyeingOrderReport _oDyeingOrderReport = new DyeingOrderReport();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<DyeingOrderReport> _oDyeingOrderReports = new List<DyeingOrderReport>();
        ExportSCDO _oExportSCDO = new ExportSCDO();
        string _sDateRange = "";
        #endregion

        #region Export PI
        public ActionResult View_DyeingOrderReports(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            //ViewBag.PaymentTypes = OrderPaymentTypeObj.Gets();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            List<DUOrderSetup> oDUOrderSetupsTemp = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (DUOrderSetup oitem in oDUOrderSetups)
            {
                if (oitem.OrderType == (int)EnumOrderType.BulkOrder || oitem.OrderType == (int)EnumOrderType.DyeingOnly)
                {
                    oDUOrderSetupsTemp.Add(oitem);
                }
            }

            ViewBag.DUOrderSetups = oDUOrderSetupsTemp;
          
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oDyeingOrderReports);
        }
        public ActionResult View_SampleOrderReports(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PaymentTypes = EnumObject.jGets(typeof(EnumOrderPaymentType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.PIStatusObjs = PIStatusObj.Gets();
            //ViewBag.DepthOfShades = DepthOfShadeObj.Gets();
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            List<DUOrderSetup> oDUOrderSetupsTemp = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (DUOrderSetup oitem in oDUOrderSetups)
            {
                if (oitem.OrderType != (int)EnumOrderType.BulkOrder && oitem.OrderType != (int)EnumOrderType.DyeingOnly)
                {
                    oDUOrderSetupsTemp.Add(oitem);
                }
            }

            ViewBag.DUOrderSetups = oDUOrderSetupsTemp;
            ViewBag.BUID = buid;
            return View(_oDyeingOrderReports);
        }
        public ActionResult View_SampleOrderReportsAll(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            ViewBag.PaymentTypes = EnumObject.jGets(typeof(EnumOrderPaymentType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)); // Enum.GetValues(typeof(EnumCompareOperator)).Cast<EnumCompareOperator>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            oDUOrderSetups = DUOrderSetup.GetsActive(buid,((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DUOrderSetups = oDUOrderSetups;
            ViewBag.BUID = buid;
            return View(_oDyeingOrderReports);
        }

    
        [HttpPost]
        public JsonResult AdvSearch(DyeingOrderReport oDyeingOrderReport)
        {
            _oDyeingOrderReports = new List<DyeingOrderReport>();
            try
            {
                string sSQL = MakeSQL(oDyeingOrderReport);
                _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDyeingOrderReports = new List<DyeingOrderReport>();
                _oDyeingOrderReport.ErrorMessage = ex.Message;
                _oDyeingOrderReports.Add(_oDyeingOrderReport);
            }
            var jsonResult = Json(_oDyeingOrderReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetsByDate(DyeingOrderReport oDyeingOrderReport)
        {
            _oDyeingOrderReport = new DyeingOrderReport();
            try
            {
                string sSQL = MakeSQL(oDyeingOrderReport);
                _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
            }
            catch (Exception ex)
            {
                _oDyeingOrderReport = new DyeingOrderReport();
            }

            var jsonResult = Json(_oDyeingOrderReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
       
        private string MakeSQL(DyeingOrderReport oDyeingOrderReport)
        {
            string sParams = oDyeingOrderReport.ErrorMessage;

            string sOrderType = "";
            int ncboOrderDate = 0;
            DateTime dFromOrderDate = DateTime.Today;
            DateTime dToOrderDate = DateTime.Today;

            int ncboReviseDate = 0;
            DateTime dFromReviseDate = DateTime.Today;
            DateTime dToReviseDate = DateTime.Today;

            int nBookingStatus = 0;

            int ncboInvoiceDate = 0;
            DateTime dFromInvoiceDate = DateTime.Today;
            DateTime dToInvoiceDate = DateTime.Today;
            int nCboDeliveryDate = 0;
            DateTime dFromDeliveryDate = DateTime.Today;
            DateTime dToDeliveryDate = DateTime.Today;
            int nCboMkPerson = 0;
            string sCurrentStatus = "";
            int nPaymentType = 0;
            string sPINo = "";
            string sLCNo = "";
            string sSampleInvoiceNo = "";
            string sRefNo = "";
            string sStyleNo = "";
            string sTemp = "";
            int nBUID = 0;
            string sBuyerConcern = "";
            string sOrderNo = "";
            int nPending = 0;
         

            int nCboStatusDate = 0;
            DateTime dFromStatusDate = DateTime.Today;
            DateTime dToStatusDate = DateTime.Today;

            int nCboOrderNo = 0;
            int nOrderNoFrom = 0;
            int nOrderNoTo = 0;
            int nYear = 00;
            if (!string.IsNullOrEmpty(sParams))
            {
                sOrderType = Convert.ToString(sParams.Split('~')[0]);
                _oDyeingOrderReport.ContractorName = Convert.ToString(sParams.Split('~')[1]);
                _oDyeingOrderReport.DeliveryToName = Convert.ToString(sParams.Split('~')[2]);
                _oDyeingOrderReport.ProductName = Convert.ToString(sParams.Split('~')[3]);
                ncboOrderDate = Convert.ToInt32(sParams.Split('~')[4]);
                dFromOrderDate = Convert.ToDateTime(sParams.Split('~')[5]);
                dToOrderDate = Convert.ToDateTime(sParams.Split('~')[6]);

                ncboInvoiceDate = Convert.ToInt32(sParams.Split('~')[7]);
                dFromInvoiceDate = Convert.ToDateTime(sParams.Split('~')[8]);
                dToInvoiceDate = Convert.ToDateTime(sParams.Split('~')[9]);

                nCboDeliveryDate = Convert.ToInt32(sParams.Split('~')[10]);
                dFromDeliveryDate = Convert.ToDateTime(sParams.Split('~')[11]);
                dToDeliveryDate = Convert.ToDateTime(sParams.Split('~')[12]);
                
                nCboMkPerson = Convert.ToInt32(sParams.Split('~')[13]);
                nPaymentType = Convert.ToInt32(sParams.Split('~')[14]);

                sCurrentStatus = Convert.ToString(sParams.Split('~')[15]);
               
                sPINo = Convert.ToString(sParams.Split('~')[16]);
                sLCNo = Convert.ToString(sParams.Split('~')[17]);
                sSampleInvoiceNo = Convert.ToString(sParams.Split('~')[18]);
                sRefNo = Convert.ToString(sParams.Split('~')[19]);
                sStyleNo = Convert.ToString(sParams.Split('~')[20]);
                nBUID = Convert.ToInt32(sParams.Split('~')[21]);
                sBuyerConcern = Convert.ToString(sParams.Split('~')[22]);
                sOrderNo = Convert.ToString(sParams.Split('~')[23]);
                nPending = Convert.ToInt32(sParams.Split('~')[24]);
                
                if(sParams.Split('~').Length>=28)
                {
                    nCboOrderNo = Convert.ToInt32(sParams.Split('~')[25]);
                    nOrderNoFrom = Convert.ToInt32(sParams.Split('~')[26]);
                    nOrderNoTo = Convert.ToInt32(sParams.Split('~')[27]);
                    nYear = Convert.ToInt16(sParams.Split('~')[28]);
                }
                if (sParams.Split('~').Length > 29)
                {
                    ncboReviseDate = Convert.ToInt32(sParams.Split('~')[29]);
                    dFromReviseDate = Convert.ToDateTime(sParams.Split('~')[30]);
                    dToReviseDate = Convert.ToDateTime(sParams.Split('~')[31]);
                }
                if (sParams.Split('~').Length > 32)
                {
                    nBookingStatus = Convert.ToInt32(sParams.Split('~')[32]);
                }
              
            }


            string sReturn1 = "";
            string sReturn = "";
            sReturn1 = "SELECT * FROM View_DyeingOrderReport";

            if (sOrderType.Contains("-1"))/// all sample
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")";
            }
            else if (sOrderType.Contains("-2"))/// all sample
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DyeingOrderType not in (" + (int)EnumOrderType.LoanOrder + "," + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")";
            }
            else  
            {
                #region nOrderType
                if (!string.IsNullOrEmpty(sOrderType))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DyeingOrderType IN (" + sOrderType+")";
                }
                #endregion
            }
            #region Contractor
            if (!String.IsNullOrEmpty(_oDyeingOrderReport.ContractorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ContractorID in(" + _oDyeingOrderReport.ContractorName + ")";
            }
            #endregion

            #region Buyer Id
            if (!String.IsNullOrEmpty(_oDyeingOrderReport.DeliveryToName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DeliveryToID in(" + _oDyeingOrderReport.DeliveryToName + ")";
            }
            #endregion

            #region Product
            if (!String.IsNullOrEmpty(_oDyeingOrderReport.ProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + _oDyeingOrderReport.ProductName + ")";
            }
            #endregion

            #region Order Date
            if (ncboOrderDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboOrderDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromOrderDate.ToString("dd MMM yyyy") + " To " + dToOrderDate.ToString("dd MMM yyyy");
                }
                else if (ncboOrderDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),OrderDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromOrderDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToOrderDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromOrderDate.ToString("dd MMM yyyy") + " To " + dToOrderDate.ToString("dd MMM yyyy");
                }
            }
            #endregion
            #region Revise Date 
            if (ncboReviseDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboReviseDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReviseDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromReviseDate.ToString("dd MMM yyyy");
                }
                else if (ncboReviseDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReviseDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromReviseDate.ToString("dd MMM yyyy");
                }
                else if (ncboReviseDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReviseDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromReviseDate.ToString("dd MMM yyyy");
                }
                else if (ncboReviseDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReviseDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromReviseDate.ToString("dd MMM yyyy");
                }
                else if (ncboReviseDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReviseDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReviseDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromReviseDate.ToString("dd MMM yyyy") + " To " + dToReviseDate.ToString("dd MMM yyyy");
                }
                else if (ncboReviseDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReviseDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReviseDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReviseDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromReviseDate.ToString("dd MMM yyyy") + " To " + dToReviseDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region SampleInvoiceDate 
            if (ncboInvoiceDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                if (sOrderType.Contains(((int)EnumOrderType.SampleOrder).ToString()))
                {
                    sTemp = "SampleInvoiceDate";
                }
                else { sTemp = "IssueDate"; }

                if (ncboInvoiceDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sTemp + ",106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sTemp + ",106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sTemp + ",106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sTemp + ",106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sTemp + ",106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboInvoiceDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12)," + sTemp + ",106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromInvoiceDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToInvoiceDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Delivery Date
            if (nCboDeliveryDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (nCboDeliveryDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (nCboDeliveryDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DeliveryDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDeliveryDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDeliveryDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion

            #region Status With Date
            if (nCboStatusDate != (int)EnumCompareOperator.None)
            {
                if (!String.IsNullOrEmpty(sCurrentStatus))
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboStatusDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " EPI.ExportPIID in (Select ExportPIID from ExportPIHistory where PIStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))) ";
                    }
                    if (nCboStatusDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " EPI.ExportPIID in (Select ExportPIID from ExportPIHistory where PIStatus in (" + sCurrentStatus + ") and CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromStatusDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToStatusDate.ToString("dd MMM yyyy") + "',106)) ) ";
                    }
                }

            }
            #endregion
          

            #region nPayment Type
            if (nPaymentType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PaymentType = " + nPaymentType;
            }
            #endregion

            #region Mkt. Person
            if (nCboMkPerson > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MKTEmpID = " + nCboMkPerson;
            }
            #endregion

            #region Current Status
            if (!string.IsNullOrEmpty(sCurrentStatus))
            {
                if (nCboStatusDate == (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " [Status] IN (" + sCurrentStatus + ")";
                }
            }
            #endregion

            #region sRefNo
            if (!string.IsNullOrEmpty(sRefNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE '%" + sRefNo + "%'";
            }
            #endregion

            #region sSampleInvoiceNo
            if (!string.IsNullOrEmpty(sSampleInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SampleInvoiceNo LIKE '%" + sSampleInvoiceNo + "%'";
            }
            #endregion

            #region LC No
            if (!string.IsNullOrEmpty(sLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportLCNo LIKE '%" + sLCNo + "%' ";
            }
            #endregion
            #region PINo No
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PINo LIKE '%" + sPINo + "%' ";
            }
            #endregion

            #region Style No
            if (!string.IsNullOrEmpty(sStyleNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StyleNo LIKE '%" + sStyleNo + "%'";
            }
            #endregion

            #region Buyer Concern
            if (!string.IsNullOrEmpty(sBuyerConcern))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ( ContactPersonnelID in(" + sBuyerConcern + ") or DeliveryToContactPersonnelID in (" + sBuyerConcern + "))";
            }
            #endregion

            #region OrderNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " orderNo LIKE '%" + sOrderNo + "%'";
            }
            #endregion

            #region CBO OrderNo FROM ---> TO
            if (nYear > 0 && nOrderNoFrom > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nOrderNoTo <= 0 && nOrderNoFrom > 0)
                {
                    sReturn = sReturn + " DONo LIKE '%" + nOrderNoFrom + "%'";
                }
                else if (nOrderNoTo > 0 && nOrderNoFrom > 0)
                {
                    if (nYear == 0)
                        nYear = DateTime.Now.Year;
                    //SELECT Convert(int, [dbo].[SplitedStringGet](DONo, '-', 0)) AS nYear FROM View_DyeingOrderReport WHERE DONo Like '%-%' AND Convert(int, [dbo].[SplitedStringGet](DONo, '-', 0)) BETWEEN 16 AND 17
                    sReturn = sReturn + " DONo LIKE '%" + (nYear % 1000) + "-%' AND Convert(int, [dbo].[SplitedStringGet](DONo, '-', 1)) BETWEEN " + nOrderNoFrom + " AND " + nOrderNoTo;
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
            #region Pending Status
            if (nPending > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nPending ==1)  // Pending Bill
                {
                    sReturn = sReturn + "PaymentType in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + "," + (int)EnumOrderPaymentType.CashOrCheque + ") and isnull(SampleInvoiceID,0)=0 and [Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 2)  // Pending PI
                {
                    sReturn = sReturn + "PaymentType in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + ") and isnull(SampleInvoiceID,0)>0 and isnull(ExportPIID,0)=0 and [Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 3)  // Pending LC
                {
                    sReturn = sReturn + "PaymentType in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + ") and isnull(ExportPIID,0)>0 and ExportPIID not in (Select ExportPIID from ExportPILCMapping where Activity=1) and [Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                if (nPending == 4)  //  Bill Issue
                {
                    sReturn = sReturn + "PaymentType in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + "," + (int)EnumOrderPaymentType.CashOrCheque + ") and isnull(SampleInvoiceID,0)>0 and isnull(ExportPIID,0)=0 and [Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 5)  //  PI Issue
                {
                    sReturn = sReturn + "PaymentType in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + ") and isnull(SampleInvoiceID,0)>0 and isnull(ExportPIID,0)>0 and ExportPIID not in (Select ExportPIID from ExportPILCMapping where Activity=1) and [Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 6)  //  LC Issue
                {
                    sReturn = sReturn + "PaymentType in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + ") and isnull(ExportPIID,0)>0 and ExportPIID in (Select ExportPIID from ExportPILCMapping where Activity=1) and [Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 7)  //  Pending Payment
                {
                    sReturn = sReturn + "PaymentType in (" + (int)EnumOrderPaymentType.CashOrCheque + ") and isnull(SampleInvoiceID,0)>0 and SampleInvoiceID not in (SELECT ReferenceID FROM PaymentDetail) and [Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 8)  //   Payment Receive
                {
                    sReturn = sReturn + "PaymentType in (" + (int)EnumOrderPaymentType.CashOrCheque + ") and isnull(SampleInvoiceID,0)>0 and SampleInvoiceID not in (SELECT ReferenceID FROM PaymentDetail) and [Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 9)  //   Running Order
                {
                    sReturn = sReturn + "[Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 10)  //   Cancel Order
                {
                    sReturn = sReturn + "[Status]=" + (int)EnumDyeingOrderState.Cancelled;
                }
                else if (nPending == 11)  //  Only Issued Order not Cancel 
                {
                    sReturn = sReturn + "[Status]!=" + (int)EnumDyeingOrderState.Cancelled;
                }
            }
            #endregion
            #region Booking Status
            if (nBookingStatus > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nBookingStatus == (int)EnumDOState.Running)
                {
                    sReturn = sReturn + "StatusDOD not in (" + (int)EnumDOState.Booking + "," + (int)EnumDOState.Closed + "," + (int)EnumDOState.Cancelled + ")";
                }
                else
                {
                    sReturn = sReturn + "StatusDOD=" + nBookingStatus;
                }
                //else if (nBookingStatus == 2)
                //{
                //    sReturn = sReturn + "IsBooking=0";
                //}
            }
            #endregion
            string sSQL = sReturn1 + " " + sReturn + " Order BY Convert(int, [dbo].[SplitedStringGet](DONo, '-', 1)) DESC,DyeingOrderID,isnull(SL,0)ASC, DyeingOrderDetailID ASC";
            return sSQL;
        }

        #region Print Production Order
        public ActionResult Print_DyeingOrderReport(string sTempString)
        {
            _oDyeingOrderReports = new List<DyeingOrderReport>();
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptDyeingOrderReportAll oReport = new rptDyeingOrderReportAll();
            byte[] abytes = oReport.PrepareReport(_oDyeingOrderReports, oCompanys.First(), oBusinessUnit, _sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_PartyWiseDOReport(string sTempString)
        {
            string sReportHead = "Party Wise";
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptDyeingOrderReport oReport = new rptDyeingOrderReport();
            byte[] abytes = oReport.PrepareReport(_oDyeingOrderReports, oCompanys.First(), oBusinessUnit, _sDateRange, sReportHead);
            return File(abytes, "application/pdf");
        }
      
        public ActionResult Print_ProductWiseDOReport(string sTempString)
        {
            string sReportHead = "Product Wise";
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptDyeingOrderReport oReport = new rptDyeingOrderReport();
            byte[] abytes = oReport.PrepareReportProductWise(_oDyeingOrderReports, oCompanys.First(), oBusinessUnit, _sDateRange, sReportHead);
            return File(abytes, "application/pdf");
        }
        public void Print_BulkOrderReportXL(string sTempString)
        {

            _oDyeingOrderReports = new List<DyeingOrderReport>();
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nQty = 0;
            double nAmount = 0;



            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 14;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Bulk Order Report");
                sheet.Name = "Bulk Production Order Report";
                sheet.Column(3).Width = 40;
                sheet.Column(4).Width = 40;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;


                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Bulk Production Order Report" + _sDateRange; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Buying House"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Yarn Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Buyer Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = _oBusinessUnit.ShortName + " Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            
              
                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex++;
                #endregion

                #region Data
                foreach (DyeingOrderReport oItem in _oDyeingOrderReports)
                {

                    nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.OrderDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNoFull; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DeliveryToName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = (oItem.UnitPrice); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.CPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                  
                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.DONote; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.StatusSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nQty = nQty + oItem.Qty;
                    nAmount = nAmount + oItem.Qty * oItem.UnitPrice;


                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

             


                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BulkProductionOrderReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        #endregion
        #region Print Sample Order

        public ActionResult Print_SampleOrderReport(string sTempString)
        {
            _oDyeingOrderReports = new List<DyeingOrderReport>();
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptSampleOrderReportAll oReport = new rptSampleOrderReportAll();
            byte[] abytes = oReport.PrepareReport(_oDyeingOrderReports, oCompanys.First(), oBusinessUnit, _sDateRange);
            return File(abytes, "application/pdf");
        }
        public ActionResult Print_PartyWiseSOReport(string sTempString)
        {
            string sReportHead = "Party Wise";
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptSampleOrderReport oReport = new rptSampleOrderReport();
            byte[] abytes = oReport.PrepareReport(_oDyeingOrderReports, oCompanys.First(), oBusinessUnit, _sDateRange, sReportHead);
            return File(abytes, "application/pdf");
        }

        public ActionResult Print_ProductWiseSOReport(string sTempString)
        {
            string sReportHead = "Product Wise";
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptSampleOrderReport oReport = new rptSampleOrderReport();
            byte[] abytes = oReport.PrepareReportProductWise(_oDyeingOrderReports, oCompanys.First(), oBusinessUnit, _sDateRange, sReportHead);
            return File(abytes, "application/pdf");
        }
        public void Print_SampleOrderReportXL(string sTempString)
        {

            _oDyeingOrderReports = new List<DyeingOrderReport>();
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            double nQty = 0;
            double nAmount = 0;



            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 17;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Sample Order Report");
                sheet.Name = "Sample Order Report";
                sheet.Column(2).Width = 20;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 30;
                sheet.Column(7).Width = 30;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 30;
                sheet.Column(12).Width = 30;
                sheet.Column(13).Width = 30;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 20;
                sheet.Column(16).Width = 20;
                sheet.Column(17).Width = 20;


                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Sample Order Report" + _sDateRange; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region

                cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Buying House"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Yarn Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

             
                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Buyer Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = _oBusinessUnit.ShortName + " Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Mode Of Payment"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Invocie No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nRowIndex++;
                #endregion

                #region Data
                foreach (DyeingOrderReport oItem in _oDyeingOrderReports)
                {

                     nSL++;
                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.OrderDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.OrderNoFull; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.DeliveryToName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = (oItem.UnitPrice); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 
                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.CPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.PaymentTypeSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.SampleInvoiceNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.DONote; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.StatusSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nQty = nQty + oItem.Qty;
                    nAmount = nAmount + oItem.Qty * oItem.UnitPrice;


                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Total
                cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 8]; cell.Value = nQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 10]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SampleOrderReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        public void Print_SampleOrderReportAllXL(string sTempString)
        {

            _oDyeingOrderReports = new List<DyeingOrderReport>();
            DyeingOrderReport oDyeingOrderReport = new DyeingOrderReport();
            oDyeingOrderReport.ErrorMessage = sTempString;
            string sSQL = MakeSQL(oDyeingOrderReport);
            _oDyeingOrderReports = DyeingOrderReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessNature.Manufacturing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            int nSL = 0;
            int nCount = 0;
            double nQty = 0;
            double nAmount = 0;



            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 27;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Order Report");
                sheet.Name = "Order Report";
                sheet.Column(3).Width = 16;
                sheet.Column(4).Width = 16;
                sheet.Column(5).Width = 23;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;
                sheet.Column(12).Width = 20;
                sheet.Column(13).Width = 20;
                sheet.Column(14).Width = 20;
                sheet.Column(15).Width = 20;
                sheet.Column(16).Width = 20;
                sheet.Column(17).Width = 20;
                sheet.Column(18).Width = 20;
                sheet.Column(19).Width = 20;
                sheet.Column(20).Width = 20;
                sheet.Column(21).Width = 20;
                sheet.Column(22).Width = 20;
                sheet.Column(23).Width = 20;
                sheet.Column(24).Width = 20;
                sheet.Column(25).Width = 20;
                sheet.Column(26).Width = 20;
                sheet.Column(27).Width = 20;
                sheet.Column(28).Width = 20;


                //   nEndCol = 10;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #endregion

                #region Report Data

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Order Report" + _sDateRange; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region
                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Order Type"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Buying House"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Buyer Ref"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Style"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Yarn Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Color Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "PantonNo"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Color No(Mkt)"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Color/LD No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Match To/Lot No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Amount"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Buyer Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = _oBusinessUnit.ShortName + " Concern Person"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Mode Of Payment"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Invocie No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "P/I No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "DO Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Delivery Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Last Revise Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "Revise Note"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#####";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount = 0;
                nRowIndex++;
                #endregion

                #region Data
                foreach (DyeingOrderReport oItem in _oDyeingOrderReports)
                {

                    nSL++;
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + oItem.OrderDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.OrderNoFull; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.OrderTypeSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.DeliveryToName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.RefNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";

                    nCount++;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.PantonNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.LDNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ColorNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ApproveLotNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = (oItem.Qty); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = (oItem.UnitPrice); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = (oItem.Qty * oItem.UnitPrice); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.CPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.MKTPName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.PaymentTypeSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.SampleInvoiceNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.PINo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ExportLCNo; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = (oItem.Qty_DO); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = (oItem.Qty_DC); cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.StatusSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.DONote; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ReviseDateSt; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nCount++;
                    cell = sheet.Cells[nRowIndex, nCount]; cell.Value = oItem.ReviseNote; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nQty = nQty + oItem.Qty;
                    nAmount = nAmount + oItem.Qty * oItem.UnitPrice;
                    nCount = 0;

                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion
                nCount=0;
                nCount++;
                #region Total
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nAmount; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

           
               

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nQty=   _oDyeingOrderReports.Select(c => c.Qty_DO).Sum();

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nQty = _oDyeingOrderReports.Select(c => c.Qty_DC).Sum();
                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = nQty; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nCount++;
                cell = sheet.Cells[nRowIndex, nCount]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

               

                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=OrderReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        #endregion


        #region
        public ActionResult PrintStatement_DO(int nDOID, int nProductID, double nts)
        {
            #region Company Info
            DyeingOrder oDyeingOrder = new DyeingOrder();
            oDyeingOrder = DyeingOrder.Get(nDOID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            #endregion
            this.PrintBySampleOrder(oDyeingOrder,0);

            _oExportSCDO.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserName;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string _sMessage = "";
            rptDUDyeingOrder oReport = new rptDUDyeingOrder();
            byte[] abytes = oReport.PrepareReport(oDyeingOrder,_oExportSCDO, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");

        }
        private void PrintBySampleOrder(DyeingOrder oDyeingOrder, int nProductID)
        {
           
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            oDyeingOrder = DyeingOrder.Get(oDyeingOrder.DyeingOrderID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oDyeingOrders.Add(oDyeingOrder);
            string sSQL = "";

            _oExportSCDO.DyeingOrderDetails = new List<DyeingOrderDetail>();
            #region Production Order
         
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            if (nProductID > 0)
            {
                oDyeingOrderDetails = DyeingOrderDetail.Gets("Select * from View_DyeingOrderDetail where ProductID=" + nProductID + " and DyeingOrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrder.[Status]<9 and  DyeingOrderID=" + oDyeingOrder.DyeingOrderID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            else
            {
                oDyeingOrderDetails = DyeingOrderDetail.Gets("Select * from View_DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where [Status]<9 and DyeingOrderID=" + oDyeingOrder.DyeingOrderID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            foreach (DyeingOrder oItem in oDyeingOrders)
            {
                List<DyeingOrderDetail> oDODetails = new List<DyeingOrderDetail>();
                oDODetails = oDyeingOrderDetails.Where(o => o.DyeingOrderID == oItem.DyeingOrderID).ToList();
                foreach (DyeingOrderDetail oItemDOD in oDODetails)
                {
                    oItemDOD.OrderNo = oItem.OrderNoFull;
                    //oItemDOD.OrderDate = oItem.OrderDateSt;
                    _oExportSCDO.DyeingOrderDetails.Add(oItemDOD);
                }
            }
            #endregion
            if (_oExportSCDO.DyeingOrderDetails.Count > 0)
            {
                _oExportSCDO.MUName = _oExportSCDO.DyeingOrderDetails[0].MUnit;
                _oExportSCDO.Currency = "$";
            }
            //if (oDyeingOrders.Count > 0)
            //{
            //    _oExportSCDO.MUName = _oExportSCDO.DyeingOrderDetails[0].MUnit;
            //    _oExportSCDO.Currency = oDyeingOrders[0].CurrencyID;
            //}
            #region Delivery Order
            ////
            List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();
            sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.Sampling + ")  and OrderID=" + oDyeingOrder.DyeingOrderID;
            oDUDeliveryOrders = DUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUDeliveryOrders.Count > 0)
            {
                List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
                sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE DOD.DUDeliveryOrderID in (SELECT DO.DUDeliveryOrderID FROM View_DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.Sampling + ")   and OrderID=" + oDyeingOrder.DyeingOrderID + ")";
                oDUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DUDeliveryOrder oItem in oDUDeliveryOrders)
                {
                    if (oItem.ApproveBy != 0)
                    {
                        List<DUDeliveryOrderDetail> oDU_DCDs = new List<DUDeliveryOrderDetail>();
                        oDU_DCDs = oDUDeliveryOrderDetails.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList();
                        foreach (DUDeliveryOrderDetail oItemDCD in oDU_DCDs)
                        {
                            oItemDCD.DeliveryDate = oItem.DODate;
                            oItemDCD.OrderNo = oItem.DONoFull;
                            _oExportSCDO.DUDeliveryOrderDetails.Add(oItemDCD);
                        }
                    }
                }
                _oExportSCDO.Qty_DO = _oExportSCDO.DUDeliveryOrderDetails.Select(c => c.Qty).Sum();
            }
            ///
            #endregion

            #region Challan
            ////
            sSQL = "SELECT * FROM VIEW_DUDeliveryChallan as DC WHERE DC.OrderType in (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.Sampling + ")  and IsDelivered=1 and DC.DUDeliveryChallanID in (SELECT DCD.DUDeliveryChallanID FROM DUDeliveryChallanDetail as DCD where DCD.DODetailID in (Select DUDeliveryOrderDetail.DUDeliveryOrderDetailID from  DUDeliveryOrderDetail where DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrder where DUDeliveryOrder.OrderID=" + oDyeingOrder.DyeingOrderID + " and OrderType in (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.Sampling + "))))";
            _oExportSCDO.DUDeliveryChallans = DUDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oExportSCDO.DUDeliveryChallans.Count > 0)
            {
                List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
                sSQL = "SELECT * FROM View_DUDeliveryChallanDetail as DCD where DCD.DODetailID in (Select DUDeliveryOrderDetail.DUDeliveryOrderDetailID from  DUDeliveryOrderDetail where DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrder where DUDeliveryOrder.OrderID=" + oDyeingOrder.DyeingOrderID + " and OrderType in (" + (int)EnumOrderType.Sampling + "," + (int)EnumOrderType.SampleOrder + ")))";
                oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DUDeliveryChallan oItem in _oExportSCDO.DUDeliveryChallans)
                {
                    //if (oItem.ApproveBy != 0)
                    //{
                    List<DUDeliveryChallanDetail> oDU_DCDs = new List<DUDeliveryChallanDetail>();
                    oDU_DCDs = oDUDeliveryChallanDetails.Where(o => o.DUDeliveryChallanID == oItem.DUDeliveryChallanID).ToList();
                    foreach (DUDeliveryChallanDetail oItemDCD in oDU_DCDs)
                    {
                        oItemDCD.ChallanDate = oItem.ChallanDate.ToString("dd MMM yyyy");
                        oItemDCD.ChallanNo = oItem.ChallanNo;
                        _oExportSCDO.DUDeliveryChallanDetails.Add(oItemDCD);
                    }
                    //}
                }
                _oExportSCDO.Qty_DC = _oExportSCDO.DUDeliveryChallanDetails.Select(c => c.Qty).Sum();

                #region Return Challan

                sSQL = "SELECT * FROM View_DUReturnChallan where DUDeliveryChallanID in (" + GetDUDChallanIDs(_oExportSCDO.DUDeliveryChallans) + ")";
                _oExportSCDO.DUReturnChallans = DUReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportSCDO.DUReturnChallans.Count > 0)
                {
                    List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                    sSQL = "Select * from View_DUReturnChallanDetail where DUReturnChallanID in (SELECT RC.DUReturnChallanID FROM DUReturnChallan as RC where DUDeliveryChallanID in (" + GetDUDChallanIDs(_oExportSCDO.DUDeliveryChallans) + "))";
                    oDUReturnChallanDetails = DUReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (DUReturnChallan oItem in _oExportSCDO.DUReturnChallans)
                    {
                        //if (oItem.ApprovedBy != 0)
                        //{
                        List<DUReturnChallanDetail> oDU_DCDs = new List<DUReturnChallanDetail>();
                        oDU_DCDs = oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList();
                        foreach (DUReturnChallanDetail oItemDCD in oDU_DCDs)
                        {
                            oItemDCD.RCDate = oItem.ReturnDate.ToString("dd MMM yyyy");
                            oItemDCD.RCNo = oItem.DUReturnChallanNo;
                            _oExportSCDO.DUReturnChallanDetails.Add(oItemDCD);
                        }
                        //}
                    }
                    _oExportSCDO.Qty_RC = _oExportSCDO.DUReturnChallanDetails.Select(c => c.Qty).Sum();
                }
                #endregion End Return Challan
            }
            ///
            #endregion

            //#region Export Bill
            //////
            //if (_oExportSCDO.LCID > 0)
            //{
            //    ExportLC oExportLC = new ExportLC();
            //    oExportLC = oExportLC.Get(_oExportSCDO.LCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    _oExportSCDO.Amount_LC = oExportLC.Amount;
            //    List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
            //    List<ExportBill> oExportBills = new List<ExportBill>();
            //    sSQL = "SELECT * FROM View_ExportBill as EB WHERE [State]<>11 and EB.ExportBillID in (Select EBD.ExportBillID from ExportBillDetail as EBD where  EBD.ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID=" + _oExportSCDO.ExportPIID + "))";
            //    oExportBills = ExportBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //    if (oExportBills.Count > 0)
            //    {
            //        sSQL = "SELECT * FROM View_ExportBillDetail as EBD WHERE EBD.ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID=" + _oExportSCDO.ExportPIID + " )";
            //        oExportBillDetails = ExportBillDetail.GetsBySQL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //        //_oExportSCDO.Qty_DC = oExportBillDetails.Select(c => c.Qty).Sum();

            //    }
            //    foreach (ExportBill oItem in oExportBills)
            //    {
            //        List<ExportBillDetail> oEB_Details = new List<ExportBillDetail>();
            //        oEB_Details = oExportBillDetails.Where(o => o.ExportBillID == oItem.ExportBillID).ToList();
            //        foreach (ExportBillDetail oItemDCD in oEB_Details)
            //        {
            //            if (oItem.State >= EnumLCBillEvent.BOEInCustomerHand)
            //            {
            //                _oExportSCDO.AcceptanceIssue = _oExportSCDO.AcceptanceIssue + oItemDCD.Qty;
            //            }
            //            if (oItem.State >= EnumLCBillEvent.AcceptedBill)
            //            {
            //                _oExportSCDO.AcceptanceRcvd = _oExportSCDO.AcceptanceRcvd + oItemDCD.Qty;
            //            }

            //        }
            //        if (oItem.State >= EnumLCBillEvent.BankAcceptedBill)
            //        {
            //            _oExportSCDO.MaturityDate = oItem.MaturityDateSt;
            //            _oExportSCDO.PaymentDate = oItem.RelizationDateSt;
            //        }
            //    }
            //}
            //#endregion

            #region Claim Order
            ////
            List<DUClaimOrder> oDUClaimOrders = new List<DUClaimOrder>();
            sSQL = "SELECT * FROM View_DUClaimorder WHERE OrderType in (" + (int)EnumOrderType.TwistOrder + "," + (int)EnumOrderType.LoanOrder + "," + (int)EnumOrderType.SaleOrder + "," + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.Sampling + ")  and ParentDOID=" + oDyeingOrder.DyeingOrderID;
            oDUClaimOrders = DUClaimOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUClaimOrders.Count > 0)
            {
                List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
                sSQL = "Select * from View_DUClaimOrderDetail where DUClaimOrderID in (SELECT DUClaimorder.DUClaimOrderID FROM DUClaimorder where OrderType  in (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.Sampling + ") and OrderID =" + oDyeingOrder.DyeingOrderID + ")";
                oDUClaimOrderDetails = DUClaimOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DUClaimOrder oItem in oDUClaimOrders)
                {
                    //if (oItem.ApproveBy != 0)
                    //{
                    List<DUClaimOrderDetail> oDU_DCDs = new List<DUClaimOrderDetail>();
                    oDU_DCDs = oDUClaimOrderDetails.Where(o => o.DUClaimOrderID == oItem.DUClaimOrderID).ToList();
                    foreach (DUClaimOrderDetail oItemDCD in oDU_DCDs)
                    {
                        oItemDCD.Date = oItem.OrderDate.ToString("dd MMM yyyy");
                        oItemDCD.ClaimOrderNo = oItem.ClaimOrderNo;
                        _oExportSCDO.DUClaimOrderDetails.Add(oItemDCD);
                    }
                    //}
                }
                _oExportSCDO.Qty_Claim = _oExportSCDO.DUClaimOrderDetails.Select(c => c.Qty).Sum();
            }
            ///
            #endregion

            #region Delivery Order Claim
            ////
            if (_oExportSCDO.DUClaimOrderDetails.Count > 0)
            {
                List<DUDeliveryOrder> oDUDeliveryOrders_Claim = new List<DUDeliveryOrder>();
                sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.ClaimOrder + ")  and DOStatus>0 and DO.OrderID in (SELECT DUClaimOrderID FROM View_DUClaimorder WHERE OrderType in (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.Sampling + ")  and OrderID=" + oDyeingOrder.DyeingOrderID + ")";
                oDUDeliveryOrders_Claim = DUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUDeliveryOrders_Claim.Count > 0)
                {
                    List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails_Claim = new List<DUDeliveryOrderDetail>();
                    sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE DOD.DUDeliveryOrderID in (SELECT DUDeliveryOrderID FROM View_DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.ClaimOrder + ")  and DOStatus>0 and DO.OrderID in (SELECT DUClaimOrderID FROM View_DUClaimorder WHERE OrderType in (" + (int)EnumOrderType.SampleOrder + "," + (int)EnumOrderType.Sampling + ")  and OrderID=" + oDyeingOrder.DyeingOrderID + "))";
                    oDUDeliveryOrderDetails_Claim = DUDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (DUDeliveryOrder oItem in oDUDeliveryOrders_Claim)
                    {
                        //if (oItem.ApproveBy != 0)
                        //{
                        List<DUDeliveryOrderDetail> oDU_DCDs = new List<DUDeliveryOrderDetail>();
                        oDU_DCDs = oDUDeliveryOrderDetails_Claim.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList();
                        foreach (DUDeliveryOrderDetail oItemDCD in oDU_DCDs)
                        {
                            oItemDCD.DeliveryDate = oItem.DODate;
                            oItemDCD.OrderNo = oItem.DONoFull;
                            _oExportSCDO.DUDeliveryOrderDetails_Claim.Add(oItemDCD);
                        }
                        //}
                    }
                    _oExportSCDO.Qty_DO = _oExportSCDO.Qty_DO + _oExportSCDO.DUDeliveryOrderDetails_Claim.Select(c => c.Qty).Sum();
                }
            }
            ///
            #endregion
            #region Challan Claim
            ////
            if (_oExportSCDO.DUDeliveryOrderDetails_Claim.Count > 0)
            {
                List<DUDeliveryChallan> oDUDeliveryChallans_Claim = new List<DUDeliveryChallan>();
                sSQL = "SELECT * FROM VIEW_DUDeliveryChallan as DC WHERE DC.OrderType in (" + (int)EnumOrderType.ClaimOrder + ")  and IsDelivered=1 and DC.DUDeliveryChallanID in (SELECT DCD.DUDeliveryChallanID FROM DUDeliveryChallanDetail as DCD where DCD.DODetailID in  (" + GetDODetailIDs(_oExportSCDO.DUDeliveryOrderDetails_Claim) + "))";
                oDUDeliveryChallans_Claim = DUDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUDeliveryChallans_Claim.Count > 0)
                {
                    List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails_Claim = new List<DUDeliveryChallanDetail>();
                    sSQL = "SELECT * FROM View_DUDeliveryChallanDetail as DCD where DCD.DUDeliveryChallanID in (" + GetDUDChallanIDs(oDUDeliveryChallans_Claim) + ") and DCD.DODetailID in (" + GetDODetailIDs(_oExportSCDO.DUDeliveryOrderDetails_Claim) + ")";
                    oDUDeliveryChallanDetails_Claim = DUDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (DUDeliveryChallan oItem in oDUDeliveryChallans_Claim)
                    {
                        //if (oItem.ApproveBy != 0)
                        //{
                        List<DUDeliveryChallanDetail> oDU_DCDs = new List<DUDeliveryChallanDetail>();
                        oDU_DCDs = oDUDeliveryChallanDetails_Claim.Where(o => o.DUDeliveryChallanID == oItem.DUDeliveryChallanID).ToList();
                        foreach (DUDeliveryChallanDetail oItemDCD in oDU_DCDs)
                        {
                            oItemDCD.ChallanDate = oItem.ChallanDate.ToString("dd MMM yyyy");
                            oItemDCD.ChallanNo = oItem.ChallanNo;
                            _oExportSCDO.DUDeliveryChallanDetails_Claim.Add(oItemDCD);
                        }
                        //}
                    }
                    _oExportSCDO.Qty_DC =    _oExportSCDO.Qty_DC +_oExportSCDO.DUDeliveryChallanDetails_Claim.Select(c => c.Qty).Sum();

                    #region Return Challan
                    if (oDUDeliveryChallans_Claim.Count > 0)
                    {
                        sSQL = "SELECT * FROM View_DUReturnChallan where DUDeliveryChallanID in (" + GetDUDChallanIDs(oDUDeliveryChallans_Claim) + ")";
                        _oExportSCDO.DUReturnChallans = DUReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        if (_oExportSCDO.DUReturnChallans.Count > 0)
                        {
                            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                            sSQL = "Select * from View_DUReturnChallanDetail where DUReturnChallanID in (SELECT RC.DUReturnChallanID FROM DUReturnChallan as RC where DUDeliveryChallanID in (" + GetDUDChallanIDs(oDUDeliveryChallans_Claim) + "))";
                            oDUReturnChallanDetails = DUReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                            foreach (DUReturnChallan oItem in _oExportSCDO.DUReturnChallans)
                            {
                                //if (oItem.ApprovedBy != 0)
                                //{
                                List<DUReturnChallanDetail> oDU_DCDs_Claim = new List<DUReturnChallanDetail>();
                                oDU_DCDs_Claim = oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList();
                                foreach (DUReturnChallanDetail oItemDCD in oDU_DCDs_Claim)
                                {
                                    oItemDCD.RCDate = oItem.ReturnDate.ToString("dd MMM yyyy");
                                    oItemDCD.RCNo = oItem.DUReturnChallanNo;
                                    _oExportSCDO.DUReturnChallanDetails_Claim.Add(oItemDCD);
                                }
                                //}
                            }
                            _oExportSCDO.Qty_RC = _oExportSCDO.Qty_RC + _oExportSCDO.DUReturnChallanDetails.Select(c => c.Qty).Sum();
                        }
                    }
                    #endregion End Return Challan
                }
            }
            ///
            #endregion
        }

        private string GetDUDChallanIDs(List<DUDeliveryChallan> oDUDeliveryChallans)
        {
            string sResult = "";
            foreach (DUDeliveryChallan oItem in oDUDeliveryChallans)
            {
                sResult = oItem.DUDeliveryChallanID + "," + sResult;
            }
            if (!string.IsNullOrEmpty(sResult))
            {
                sResult = sResult.Remove((sResult.Length - 1), 1);
            }
            return sResult;
        }
        private string GetDODetailIDs(List<DUDeliveryOrderDetail> oDODetails)
        {
            string sResult = "";
            foreach (DUDeliveryOrderDetail oItem in oDODetails)
            {
                sResult = oItem.DUDeliveryOrderDetailID + "," + sResult;
            }
            if (!string.IsNullOrEmpty(sResult))
            {
                sResult = sResult.Remove((sResult.Length - 1), 1);
            }
            return sResult;
        }

        public ActionResult PrintStatusHistory(int nDOID, int nBUID, double nts)
        {
            DyeingOrder _oDyeingOrder = new DyeingOrder();
            Company oCompany = new Company();
            if (nDOID > 0)
            {
                _oDyeingOrder = DyeingOrder.Get(nDOID, (int)Session[SessionInfo.currentUserID]);
                _oDyeingOrder.DyeingOrderDetails = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrderHistory WHERE DyeingOrderID = " + nDOID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            byte[] abytes;
            rptDyeingOrderStatusHistory oReport = new rptDyeingOrderStatusHistory();
            abytes = oReport.PrepareReport(_oDyeingOrder, oCompany);
            return File(abytes, "application/pdf");
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

    }
}
