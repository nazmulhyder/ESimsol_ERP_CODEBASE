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
    public class DUDeliveryLotController : Controller
    {
        #region Declaration
        DUDeliveryLot _oDUDeliveryLot = new DUDeliveryLot();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<DUDeliveryLot> _oDUDeliveryLots = new List<DUDeliveryLot>();
        string _sDateRange = "";
        #endregion

        #region DUDeliveryLot
        public ActionResult View_DUDeliveryLot(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            //ViewBag.PaymentTypes = OrderPaymentTypeObj.Gets();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = oOrderTypes;

            return View(_oDUDeliveryLots);

        }
        public ActionResult View_DUHardWindingStock(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.BUType = EnumObject.jGets(typeof(EnumBusinessUnitType));
            //ViewBag.PaymentTypes = OrderPaymentTypeObj.Gets();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.OrderTypes = DUOrderSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            //ViewBag.OrderTypes = oOrderTypes;

            return View(_oDUDeliveryLots);
        }
    
        [HttpPost]
        public JsonResult AdvSearch(DUDeliveryLot oDUDeliveryLot)
        {
            _oDUDeliveryLots = new List<DUDeliveryLot>();
            try
            {
                string sParams = oDUDeliveryLot.ErrorMessage;
                int nWorkingUnitID = 0;
                int nOrderType = 0;
                int nReportType = 0;
                int nBUID = 0;

                if (!string.IsNullOrEmpty(sParams))
                {
                    nWorkingUnitID = Convert.ToInt32(sParams.Split('~')[0]);
                    nOrderType = Convert.ToInt32(sParams.Split('~')[1]);
                    nReportType = Convert.ToInt32(sParams.Split('~')[2]);
                }

                _oDUDeliveryLots = DUDeliveryLot.Gets( nOrderType,nWorkingUnitID, nReportType,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryLots = new List<DUDeliveryLot>();
                _oDUDeliveryLot.ErrorMessage = ex.Message;
                _oDUDeliveryLots.Add(_oDUDeliveryLot);
            }
            var jsonResult = Json(_oDUDeliveryLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(string sString)
        {
            int nCboDate = 0;
            DateTime dFromDODate = DateTime.Now;
            DateTime dToDODate = DateTime.Now;
            string _sContractorIds = "";
            string _sProductIDs = "";
            int nOrderType = 0;
            int nWorkingUnit = 0;
            string sOrderNo = "";
            int _nBUID = 0;
            string sLotNo = "";

            int nCount = 0;
            nCboDate = Convert.ToInt32(sString.Split('~')[nCount++]);
            dFromDODate = Convert.ToDateTime(sString.Split('~')[nCount++]);
            dToDODate = Convert.ToDateTime(sString.Split('~')[nCount++]);
            _sContractorIds = sString.Split('~')[nCount++];
            _sProductIDs = sString.Split('~')[nCount++];
            nOrderType = Convert.ToInt32(sString.Split('~')[nCount++]);
            nWorkingUnit = Convert.ToInt32(sString.Split('~')[nCount++]);
            sLotNo = sString.Split('~')[nCount++];
            sOrderNo = sString.Split('~')[nCount++];
            _nBUID = Convert.ToInt32(sString.Split('~')[nCount++]);

            //string sReturn1 = "Select LotID, DODID from DULotDistribution";
            string sReturn = " ";

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " DBServerDateTime", nCboDate, dFromDODate, dToDODate);
            #endregion

            #region Buyer
            if (!string.IsNullOrEmpty(_sContractorIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DODID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where ContractorID in (" + _sContractorIds + ")))";
             
            }
            #endregion

            #region Product
            if (!string.IsNullOrEmpty(_sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DODID in (Select DyeingOrderDetailID from DyeingOrderDetail where ProductID in (" + _sProductIDs + "))";
            }
            #endregion

            #region OrderType
            if (nOrderType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DODID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrderType in (" + nOrderType + ")))";
            }
            #endregion

            #region WorkingUnit
            if (nWorkingUnit>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "LotID in (select LotID from Lot where Balance>0 and WorkingUnitID=" + nWorkingUnit + " )";
            }
            #endregion

            #region PI No
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "LotID in (select LotID from Lot where Balance>0 and LotNo like '%" + sLotNo + "' )";
            }
            #endregion
            #region OrderNo
            if (!string.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DODID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where OrderNo '%" + sOrderNo + "%'))";
            }
            #endregion

            //#region BUID
            //if (_nBUID>0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " BUID = " +  _nBUID;
            //}
            //#endregion

            return sReturn;
        }
         [HttpPost]
        public JsonResult AdvanchSearch(DUDeliveryLot oDUDeliveryLot) 
        {
            string sTemp = oDUDeliveryLot.ErrorMessage;
            string sSQL = MakeSQL(sTemp);
            try
            {
                _oDUDeliveryLots = DUDeliveryLot.GetsFromAdv(sSQL, oDUDeliveryLot.ReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryLots = new List<DUDeliveryLot>();
                _oDUDeliveryLot.ErrorMessage = ex.Message;
                _oDUDeliveryLots.Add(_oDUDeliveryLot);
            }
            var jsonResult = Json(_oDUDeliveryLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region DUHardWindingStock
        public JsonResult AdvSearchHWStock(DUDeliveryLot oDUDeliveryLot)
        {
            string sTemp = oDUDeliveryLot.ErrorMessage;
            string sSQL = MakeSQLWindingStock(sTemp);
            try
            {
                _oDUDeliveryLots = DUDeliveryLot.GetsDUHardWindingStock(sSQL, oDUDeliveryLot.ReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryLots = new List<DUDeliveryLot>();
                _oDUDeliveryLot.ErrorMessage = ex.Message;
                _oDUDeliveryLots.Add(_oDUDeliveryLot);
            }
            var jsonResult = Json(_oDUDeliveryLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQLWindingStock(string sString)
        {
            int nCboDate = 0;
            DateTime dFromDODate = DateTime.Now;
            DateTime dToDODate = DateTime.Now;
            string _sContractorIds = "";
            string _sProductIDs = "";
            int nOrderType = 0;
            int nStockType = 0;
            string sOrderNo = "";
            int _nBUID = 0;
            string sLotNo = "";

            int nCount = 0;
            nCboDate = Convert.ToInt32(sString.Split('~')[nCount++]);
            dFromDODate = Convert.ToDateTime(sString.Split('~')[nCount++]);
            dToDODate = Convert.ToDateTime(sString.Split('~')[nCount++]);
            _sContractorIds = sString.Split('~')[nCount++];
            _sProductIDs = sString.Split('~')[nCount++];
            nOrderType = Convert.ToInt32(sString.Split('~')[nCount++]);
            nStockType = Convert.ToInt32(sString.Split('~')[nCount++]);
            sLotNo = sString.Split('~')[nCount++];
            sOrderNo = sString.Split('~')[nCount++];
            _nBUID = Convert.ToInt32(sString.Split('~')[nCount++]);

            //string sReturn1 = "Select LotID, DODID from DULotDistribution";
            string sReturn = " ";

            #region DATE SEARCH
            //DateObject.CompareDateQuery(ref sReturn, " DBServerDateTime", nCboDate, dFromDODate, dToDODate);
            #endregion
            #region QCDate
            if (nCboDate != (int)EnumCompareOperator.None)
            {
                if (nCboDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (nCboDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (nCboDate == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (nCboDate == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (nCboDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDODate.ToString("dd MMM yyyy") + "',106))";
                }
                else if (nCboDate == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(DATE,CONVERT(VARCHAR(12),QCDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromDODate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToDODate.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion
            #region Buyer
            if (!string.IsNullOrEmpty(_sContractorIds))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where ContractorID in (" + _sContractorIds + ")))";

            }
            #endregion

            #region Product
            if (!string.IsNullOrEmpty(_sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where ProductID in (" + _sProductIDs + "))";
            }
            #endregion

            #region OrderType
            if (nOrderType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrderType in (" + nOrderType + ")))";
            }
            #endregion

            #region Stock Type
            if (nStockType > 0)
            {
                string _SText = " AND RouteSheetID IN (SELECT ParentID FROM  Lot WHERE LOT.ParentType = 106 AND LOT.Balance>0.3)";

                Global.TagSQL(ref sReturn);
                if (nStockType == 1)
                {
                     sReturn = sReturn + "Warp IN (0,1,2)";
                }
                if (nStockType == 2)
                {
                    sReturn = sReturn + "Warp IN (0,1)";
                }
                if (nStockType == 3)
                {
                    sReturn = sReturn + "Warp IN (2)";
                }
                
                sReturn = sReturn + _SText;
            }
            #endregion

            #region PI No
            //if (!string.IsNullOrEmpty(sLotNo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "LotID in (select LotID from Lot where Balance>0 and LotNo like '%" + sLotNo + "' )";
            //}
            #endregion
            #region OrderNo
            //if (!string.IsNullOrEmpty(sOrderNo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + "DODID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where OrderNo '%" + sOrderNo + "%'))";
            //}
            #endregion

            return sReturn;
        }

        [HttpPost]
        public JsonResult SearchByOrderLotNo(DUDeliveryLot oDUDeliveryLot)
        {
            _oDUDeliveryLots = new List<DUDeliveryLot>();
            string sReturn = " ";
            try
            {
                #region Lot No
                if (!string.IsNullOrEmpty(oDUDeliveryLot.LotNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "RouteSheetID IN (SELECT ParentID FROM  Lot WHERE LOT.ParentType = 106 AND LOTNo like '%" + oDUDeliveryLot.LotNo + "%' )";
                }
                #endregion
                #region Order No
                if (!string.IsNullOrEmpty(oDUDeliveryLot.OrderNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DyeingOrderDetailID in (Select DyeingOrderDetailID from DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where OrderNo like '%" + oDUDeliveryLot.OrderNo + "%'))";

                }
                #endregion
                _oDUDeliveryLots = DUDeliveryLot.GetsDUHardWindingStock(sReturn, oDUDeliveryLot.ReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryLots = new List<DUDeliveryLot>();
                _oDUDeliveryLot.ErrorMessage = ex.Message;
                _oDUDeliveryLots.Add(_oDUDeliveryLot);
            }
            var jsonResult = Json(_oDUDeliveryLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult DUHardWindingStockList(string sValue)
        {
            string sSQL = MakeSQLWindingStock(sValue);
            try
            {
                int nReportType = Convert.ToInt32(sValue.Split('~')[10]);
                _oDUDeliveryLots = DUDeliveryLot.GetsDUHardWindingStock(sSQL, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oDUDeliveryLots = new List<DUDeliveryLot>();
                _oDUDeliveryLot.ErrorMessage = ex.Message;
                _oDUDeliveryLots.Add(_oDUDeliveryLot);
            }
            if (_oDUDeliveryLots.Count > 0)
            {
                Company oCompany = new Company();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                rptDUHardWindingStockList oReport = new rptDUHardWindingStockList();
                byte[] abytes = oReport.PrepareReport(_oDUDeliveryLots, oCompany, "");
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                return File(abytes, "application/pdf");
            } 
        }
        [HttpPost]
        public JsonResult GetAllDULotDistribution(DULotDistribution oDULotDistribution)
        {
            DULotDistribution _oDULotDistribution = new DULotDistribution();
            List<DULotDistribution> _oDULotDistributions = new List<DULotDistribution>();
            try
            {
                String SQL = "Select * from View_DULotDistribution where LotID=" + oDULotDistribution.LotID + " and DODID in (Select DODID_Des from DULotDistributionLog where DODID_Source=" + oDULotDistribution.DODID + ")";
                _oDULotDistributions = DULotDistribution.Gets(SQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDULotDistribution = new DULotDistribution();
                _oDULotDistribution.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDULotDistributions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult DULotDistributionDelete(int id)
        //{
        //    string sFeedBackMessage = "";
        //    try
        //    {
        //        DULotDistribution _DULotDistribution = new DULotDistribution();
        //        sFeedBackMessage = _DULotDistribution.Delete(id, (int)Session[SessionInfo.currentUserID]);

        //    }
        //    catch (Exception ex)
        //    {
        //        sFeedBackMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(sFeedBackMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        #endregion

         #region Print
         //public ActionResult Print_DUDeliveryLotReport(string sTempString)
        //{
        //    _oDUDeliveryLots = new List<DUDeliveryLot>();
        //    DUDeliveryLot oDUDeliveryLot = new DUDeliveryLot();
        //    oDUDeliveryLot.ErrorMessage = sTempString;
        //    string sSQL = MakeSQL(oDUDeliveryLot);
        //    _oDUDeliveryLots = DUDeliveryLot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    rptDUDeliveryLotAll oReport = new rptDUDeliveryLotAll();
        //    byte[] abytes = oReport.PrepareReport(_oDUDeliveryLots, oCompanys.First(), oBusinessUnit, _sDateRange);
        //    return File(abytes, "application/pdf");
        //}
         
        #endregion
         #region Excel
         public void PrintRpt_Excel(string sTempString, int nReportType)
         {
             DUDeliveryLot oDUDeliveryLot = new DUDeliveryLot();
             List<DUDeliveryLot> oDUDeliveryLots = new List<DUDeliveryLot>();
             string sSQL = MakeSQL(sTempString);
             oDUDeliveryLots = DUDeliveryLot.GetsFromAdv(sSQL, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);

             Company oCompany = new Company();
             oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
             BusinessUnit oBusinessUnit = new BusinessUnit();
             oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

             if (nReportType == 1)
             {
                 #region Header
                 List<TableHeader> table_header = new List<TableHeader>();
                 table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Right });
                 table_header.Add(new TableHeader { Header = "Yarn", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "Product Name", Width = 55f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "Color", Width = 35f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "Dye Lot No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "Order Qty", Width = 12f, IsRotate = false, Align = TextAlign.Right });
                 table_header.Add(new TableHeader { Header = "Stock in Hand", Width = 12f, IsRotate = false, Align = TextAlign.Right });
                 table_header.Add(new TableHeader { Header = "Delivered Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right });
                 table_header.Add(new TableHeader { Header = "Order No", Width = 12f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "P/I No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "L/C No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "OrderType", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "Location", Width = 15f, IsRotate = false, Align = TextAlign.Left });

                 #endregion
                 #region Export Excel
                 int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                 ExcelRange cell; ExcelFill fill;
                 OfficeOpenXml.Style.Border border;

                 using (var excelPackage = new ExcelPackage())
                 {
                     excelPackage.Workbook.Properties.Author = "ESimSol";
                     excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                     var sheet = excelPackage.Workbook.Worksheets.Add("Delivery Stock Report");
                     sheet.Name = "Delivery Stock Report";

                     ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                     #region Report Header
                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                     cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex++;

                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = "Delivery Stock Report"; cell.Style.Font.Bold = true;
                     cell.Style.WrapText = true;
                     cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex = nRowIndex + 1;
                     #endregion

                     #region Address & Date
                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                     cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex++;

                     #endregion

                     nRowIndex++;
                     nStartCol = 2;
                     ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                     nEndCol = table_header.Count() + nStartCol;

                     int nCol = 1;
                     double dOrderQTY = 0;
                     double dStockInHand = 0;
                     double dDeliveredQty = 0;
                     foreach (var obj in oDUDeliveryLots)
                     {
                         nStartCol = 2;
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCol++.ToString(), true);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Contractor, false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Product.ToString(), false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ColorName.ToString(), false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LotNo.ToString(), false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_Order, 2).ToString(), true);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Balance, 2).ToString(), true);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_DC, 2).ToString(), true);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OrderNoFull, false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo.ToString(), false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCNo.ToString(), false, false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OrderTypeSt.ToString(), false, false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LocationName, false, false);
                         nRowIndex++;
                         dOrderQTY = dOrderQTY + obj.Qty_Order;
                         dStockInHand = dStockInHand + obj.Balance;
                         dDeliveredQty = dDeliveredQty + obj.Qty_DC;

                     }
                     nStartCol = 2;
                     ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 6, true, ExcelHorizontalAlignment.Right, true);
                     ExcelTool.FillCell(sheet, nRowIndex, 7, Math.Round(dOrderQTY, 2).ToString(), true, true);
                     ExcelTool.FillCell(sheet, nRowIndex, 8, Math.Round(dStockInHand, 2).ToString(), true, true);
                     ExcelTool.FillCell(sheet, nRowIndex, 9, Math.Round(dDeliveredQty, 2).ToString(), true, true);
                     ExcelTool.FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, 10, 13, true, ExcelHorizontalAlignment.Right, true);
                    

                     cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                     fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                     fill.BackgroundColor.SetColor(Color.White);

                     Response.ClearContent();
                     Response.BinaryWrite(excelPackage.GetAsByteArray());
                     Response.AddHeader("content-disposition", "attachment; filename=Delivery Stock Report.xlsx");
                     Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                     Response.Flush();
                     Response.End();
                 }
                 #endregion
             }
             if (nReportType == 2)
             {
                 #region Header
                 List<TableHeader> table_header = new List<TableHeader>();
                 table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Right });
                 table_header.Add(new TableHeader { Header = "Yarn", Width = 70f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "Balance Qty(LBS)", Width = 20f, IsRotate = false, Align = TextAlign.Right });
                 table_header.Add(new TableHeader { Header = "Delivered Qty", Width = 20f, IsRotate = false, Align = TextAlign.Right });

                 #endregion
                 #region Export Excel
                 int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                 ExcelRange cell; ExcelFill fill;
                 OfficeOpenXml.Style.Border border;

                 using (var excelPackage = new ExcelPackage())
                 {
                     excelPackage.Workbook.Properties.Author = "ESimSol";
                     excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                     var sheet = excelPackage.Workbook.Worksheets.Add("Delivery Stock Report");
                     sheet.Name = "Delivery Stock Report";

                     ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                     #region Report Header
                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                     cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex++;

                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = "Delivery Stock Report"; cell.Style.Font.Bold = true;
                     cell.Style.WrapText = true;
                     cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex = nRowIndex + 1;
                     #endregion

                     #region Address & Date
                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                     cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex++;

                     #endregion

                     nRowIndex++;
                     nStartCol = 2;
                     ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                     nEndCol = table_header.Count() + nStartCol;

                     int nCol = 1;
                     double dBalance = 0;
                     double dDeliveredQty = 0;
                     foreach (var obj in oDUDeliveryLots)
                     {
                         nStartCol = 2;
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCol++.ToString(), true);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Product.ToString(), false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Balance, 2).ToString(), true);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_DC, 2).ToString(), true);
                         nRowIndex++;
                         dBalance = dBalance + obj.Balance;
                         dDeliveredQty = dDeliveredQty + obj.Qty_DC;

                     }
                     nStartCol = 2;
                     ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Right, true);
                     ExcelTool.FillCell(sheet, nRowIndex, 4, Math.Round(dBalance, 2).ToString(), true, true);
                     ExcelTool.FillCell(sheet, nRowIndex, 5, Math.Round(dDeliveredQty, 2).ToString(), true, true);

                     cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                     fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                     fill.BackgroundColor.SetColor(Color.White);

                     Response.ClearContent();
                     Response.BinaryWrite(excelPackage.GetAsByteArray());
                     Response.AddHeader("content-disposition", "attachment; filename=Delivery Stock Report.xlsx");
                     Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                     Response.Flush();
                     Response.End();
                 }
                 #endregion
             }
             if (nReportType == 3)
             {
                 #region Header
                 List<TableHeader> table_header = new List<TableHeader>();
                 table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Right });
                 table_header.Add(new TableHeader { Header = "Buyer Name", Width = 35f, IsRotate = false, Align = TextAlign.Left });
                 table_header.Add(new TableHeader { Header = "Balance Qty(LBS)", Width = 20f, IsRotate = false, Align = TextAlign.Right });
                 table_header.Add(new TableHeader { Header = "Delivered Qty", Width = 20f, IsRotate = false, Align = TextAlign.Right });

                 #endregion
                 #region Export Excel
                 int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                 ExcelRange cell; ExcelFill fill;
                 OfficeOpenXml.Style.Border border;

                 using (var excelPackage = new ExcelPackage())
                 {
                     excelPackage.Workbook.Properties.Author = "ESimSol";
                     excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                     var sheet = excelPackage.Workbook.Worksheets.Add("Delivery Stock Report");
                     sheet.Name = "Delivery Stock Report";

                     ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                     #region Report Header
                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                     cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex++;

                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = "Delivery Stock Report"; cell.Style.Font.Bold = true;
                     cell.Style.WrapText = true;
                     cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex = nRowIndex + 1;
                     #endregion

                     #region Address & Date
                     cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                     cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                     cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                     nRowIndex++;

                     #endregion

                     nRowIndex++;
                     nStartCol = 2;
                     ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                     nEndCol = table_header.Count() + nStartCol;

                     int nCol = 1;
                     double dBalance = 0;
                     double dDeliveredQty = 0;
                     foreach (var obj in oDUDeliveryLots)
                     {
                         nStartCol = 2;
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCol++.ToString(), true);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Contractor.ToString(), false);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Balance, 2).ToString(), true);
                         ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_DC, 2).ToString(), true);
                         nRowIndex++;
                         dBalance = dBalance + obj.Balance;
                         dDeliveredQty = dDeliveredQty + obj.Qty_DC;
                     }
                     nStartCol = 2;
                     ExcelTool.FillCellMerge(ref sheet, "Total :", nRowIndex, nRowIndex, nStartCol, 3, true, ExcelHorizontalAlignment.Right, true);
                     ExcelTool.FillCell(sheet, nRowIndex, 4, Math.Round(dBalance, 2).ToString(), true, true);
                     ExcelTool.FillCell(sheet, nRowIndex, 5, Math.Round(dDeliveredQty, 2).ToString(), true, true);

                     cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                     fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                     fill.BackgroundColor.SetColor(Color.White);

                     Response.ClearContent();
                     Response.BinaryWrite(excelPackage.GetAsByteArray());
                     Response.AddHeader("content-disposition", "attachment; filename=Delivery Stock Report.xlsx");
                     Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                     Response.Flush();
                     Response.End();
                 }
                 #endregion
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


        #endregion

      

     
    }
}
