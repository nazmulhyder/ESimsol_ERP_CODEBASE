using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSol.Controllers
{
    public class ProductStockController : Controller
    {
        #region Declare

        List<Product> _oProducts = new List<Product>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        #endregion

        #region Actions
        public ActionResult ViewProductStock(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Product).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oClientOperationSetting = new ClientOperationSetting();
            _oProducts = new List<Product>();
            _oProducts = Product.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oProducts);
        }
        public ActionResult ViewProductDetailsStock(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Lot).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oClientOperationSetting = new ClientOperationSetting();
            List<Lot> oLots = new List<Lot>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            #region Gets Store & Report Layout
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.BUWiseGets(buid, (int)Session[SessionInfo.currentUserID]));
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.SelectedBUID = buid;
            ViewBag.BusinessUnitType = oBusinessUnit.BusinessUnitType;

            ViewBag.ReportLayOuts = EnumObject.jGets(typeof(EnumStockReportLayout));
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oLots);
        }

        [HttpPost]
        public JsonResult GetPIInfo(PTUUnit2Distribution oPTUUnit2Distribution)
        {

            List<PTUUnit2Distribution> oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            try
            {
                string sSQL = "SELECT * FROM View_PTUUnit2Distribution";
                string sReturn = "";
                if (oPTUUnit2Distribution.LotID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "LotID=" + oPTUUnit2Distribution.LotID;
                }
                sSQL = sSQL + "" + sReturn;
                oPTUUnit2Distributions = PTUUnit2Distribution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            }
            catch (Exception ex)
            {
                oPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPTUUnit2Distributions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsProductStocks(Lot oLot)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                string sSQL = this.GetSQL(oLot.PCID, oLot.LocationName, oLot.Layout, oLot.BUID, oLot.ProductName, oLot.LotNo, oLot.SizeName, oLot.SupplierName, oLot.StyleNo, oLot.ColorName, oLot.BuyerName, oLot.MCDia, oLot.ErrorMessage);
                DataSet oLotsDataSet = Lot.GetsDataSet(sSQL, (int)Session[SessionInfo.currentUserID]);
                DataTable oLotsDataTable = oLotsDataSet.Tables[0];
                oLots = Lot.CreateObjects(oLotsDataTable);
            }
            catch (Exception ex)
            {
                oLots = new List<Lot>();
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots.Add(oLot);
            }
            var jsonResult = Json(oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string GetSQL(int pcid, string WorkingUnitIDs, int layout, int buid, string sProductName, string sLotNo, string sSizeName, string sSupplierIDs, string sStyleIDs, string sColorName, string sBuyerName, string sDiaOrGSM, string sIsOnlyZeroBalanceShow)
        {

            string sReturn1 = "";
            string sReturn = "";
            if (sProductName == null) { sProductName = ""; }
            if (sLotNo == null) { sLotNo = ""; }

            #region Basic Query
            if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductWise)
            {
                sReturn1 = "SELECT HH.ProductID, HH.ProductCode, HH.ProductName, HH.MUName, HH.ReportUnitSymbol,  HH.ProductCategoryID, HH.ProductCategoryName,  HH.ProductBaseID, HH.ProductGroupName, HH.SizeName,  SUM(HH.Balance) AS Balance,  SUM(HH.ReportingBalance) AS ReportingBalance,   CASE WHEN SUM(ISNULL(HH.Balance,0))>0 THEN SUM(HH.Balance*HH.UnitPrice)/SUM(ISNULL(HH.Balance,0)) ELSE AVG(UnitPrice) END AS UnitPrice,  SUM(HH.Balance*HH.UnitPrice) AS StockValue,  0 AS WorkingUnitID, '' AS LocationName, '' AS OperationUnitName, 0 AS LotID, '' AS LotNo, '' AS ColorName, '' AS SupplierName , '' AS StyleNo, 0 AS WeightPerCartoon, 0 AS ConePerCartoon, '' AS BuyerName  , '' AS MCDia, '' AS FinishDia, '' AS GSM, AVG(FCUnitPrice) AS FCUnitPrice, FCSymbol, '' as Shade, '' as Stretch_Length, '' AS RackNo FROM View_Lot AS HH";
            }
            else if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndColorWise)
            {
                sReturn1 = "SELECT HH.ProductID, HH.ProductCode, HH.ProductName, HH.ColorID, HH.ColorName, HH.MUName, HH.ReportUnitSymbol,   SUM(HH.Balance) AS Balance,  SUM(HH.ReportingBalance) AS ReportingBalance,   AVG(HH.UnitPrice) AS UnitPrice, SUM(HH.Balance*HH.UnitPrice) AS StockValue, 0 AS  ProductCategoryID, '' AS ProductCategoryName,   0 AS ProductBaseID, '' AS ProductGroupName, '' AS SizeName,     0 AS WorkingUnitID, '' AS LocationName, '' AS OperationUnitName, 0 AS LotID, '' AS LotNo, '' AS ColorName, '' AS SupplierName , '' AS StyleNo, 0 AS WeightPerCartoon, 0 AS ConePerCartoon, '' AS BuyerName , '' AS MCDia, '' AS FinishDia, '' AS GSM, FCUnitPrice, FCSymbol  ,'' as Shade, '' as Stretch_Length, '' AS RackNo FROM View_Lot AS HH";
            }
            else if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise)
            {
                sReturn1 = "SELECT HH.ProductID, HH.ProductCode, HH.ProductName,  HH.ColorID, HH.ColorName, HH.WorkingUnitID, HH.LocationName, HH.OperationUnitName, HH.MUName, HH.ReportUnitSymbol, SUM(HH.Balance) AS Balance,  SUM(HH.ReportingBalance) AS ReportingBalance, AVG(HH.UnitPrice) AS UnitPrice, SUM(HH.Balance*HH.UnitPrice) AS StockValue,  0 AS  ProductCategoryID, '' AS ProductCategoryName,   0 AS ProductBaseID, '' AS ProductGroupName, '' AS SizeName, 0 AS LotID, '' AS LotNo, '' AS ColorName, '' AS SupplierName , '' AS StyleNo, 0 AS WeightPerCartoon, 0 AS ConePerCartoon, '' AS BuyerName , '' AS MCDia, '' AS FinishDia, '' AS GSM, FCUnitPrice, FCSymbol  ,'' as Shade, '' as Stretch_Length, '' AS RackNo FROM View_Lot AS HH";
            }
            else if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
            {
                sReturn1 = "SELECT HH.LotID, HH.LotNo, HH.ProductID, HH.ProductCode, HH.ProductName,  HH.ColorID, HH.ColorName, HH.SizeName,  HH.WorkingUnitID, HH.LocationName, HH.OperationUnitName, HH.ColorName, HH.MUName, HH.ReportUnitSymbol, HH.SupplierName, HH.StyleNo,  HH.Balance, HH.ReportingBalance, HH.UnitPrice, (HH.Balance*HH.UnitPrice) AS StockValue, HH.WeightPerCartoon, HH.ConePerCartoon, HH.BuyerName, HH.MCDia, HH.FinishDia, HH.GSM, FCUnitPrice, FCSymbol, HH.Shade, HH.Stretch_Length, (HH.ShelfName+'-'+HH.RackNo) AS RackNo  ,0 AS  ProductCategoryID, '' AS ProductCategoryName,   0 AS ProductBaseID, '' AS ProductGroupName  FROM View_Lot AS HH";
            }
            else
            {
                sReturn1 = "SELECT * FROM View_Lot AS HH";
            }
            #endregion

            #region Deafult Cluse
            if (sIsOnlyZeroBalanceShow == "True" && (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Balance = 0.00 ";
            }
            else if (sIsOnlyZeroBalanceShow == "True" && (EnumStockReportLayout)layout == EnumStockReportLayout.ProductWise)
            {
                
            }
            else
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ROUND(HH.Balance,0) > 0 ";
            }
            #endregion

            #region BUID
            if (buid != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.BUID = " + buid.ToString();
            }
            #endregion

            #region Contractor
            if (!string.IsNullOrWhiteSpace(sSupplierIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.SupplierID IN (" + sSupplierIDs + ")";
            }
            #endregion

            #region Style
            if (!string.IsNullOrWhiteSpace(sStyleIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.StyleID IN (" + sStyleIDs + ")";
            }
            #endregion
            #region Product Category
            if (pcid != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ProductID IN (SELECT NN.ProductID FROM View_Product AS NN WHERE NN.ProductCategoryID IN (SELECT MM.ProductCategoryID FROM dbo.fn_GetProductCategory(" + pcid + ") AS MM))";
            }
            #endregion

            #region Product Name
            if (!string.IsNullOrWhiteSpace(sProductName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " (ISNULL(HH.ProductName,'')+ISNULL(HH.ProductCode,'')) LIKE '%" + sProductName + "%'";
            }
            #endregion

            #region Lot No
            if (!string.IsNullOrWhiteSpace(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo = '" + sLotNo + "'";
            }
            #endregion

            #region sSizeName
            if (!string.IsNullOrWhiteSpace(sSizeName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SizeName = '" + sSizeName + "'";
            }
            #endregion

            #region sColorName
            if (!string.IsNullOrWhiteSpace(sColorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ColorName LIKE '%" + sColorName + "%'";
            }
            #endregion

            #region sBuyerName
            if (!string.IsNullOrWhiteSpace(sBuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerName LIKE '%" + sBuyerName + "%'";
            }
            #endregion
            #region sMCDia, Finish Dia OR GSM
            if (!string.IsNullOrWhiteSpace(sDiaOrGSM))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MCDia+FinishDia+GSM LIKE '%" + sDiaOrGSM + "%'";
            }
            #endregion


            #region WorkingUnitIDs
            if (!string.IsNullOrEmpty(WorkingUnitIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.WorkingUnitID IN (" + WorkingUnitIDs + ")";
            }
            #endregion

            #region Group By
            if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductWise)
            {
                sReturn = sReturn + " GROUP BY HH.ProductName, HH.ProductID, HH.ProductCode, HH.ProductName, HH.MUName, HH.ReportUnitSymbol , HH.ProductCategoryID, HH.ProductBaseID, HH.ProductCategoryName, HH.ProductGroupName, HH.SizeName, HH.FCSymbol";
            }
            else if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndColorWise)
            {
                sReturn = sReturn + " GROUP BY HH.ProductName, HH.ProductID, HH.ColorID,  HH.ProductCode,  HH.ColorName, HH.MUName, HH.ReportUnitSymbol, HH.FCUnitPrice, HH.FCSymbol,  HH.RackNo";
            }
            else if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise)
            {
                sReturn = sReturn + " GROUP BY HH.ProductName, HH.ProductID, HH.ColorID, HH.WorkingUnitID, HH.ProductCode,  HH.LocationName, HH.OperationUnitName, HH.ColorName, HH.MUName, HH.ReportUnitSymbol, HH.FCUnitPrice, HH.FCSymbol,  HH.RackNo";
            }
            else if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
            {
                sReturn = sReturn + " ORDER BY  HH.ProductName, HH.ProductID, HH.ColorID, HH.WorkingUnitID, HH.LotID,  HH.SupplierName, HH.StyleNo, HH.BuyerName, HH.MCDia, HH.FinishDia, HH.GSM, HH.FCUnitPrice, HH.FCSymbol,  HH.RackNo ";    //,HH.WeightPerCartoon, HH.ConePerCartoon 
            }
            else
            {
                sReturn = sReturn + " ORDER BY  HH.ProductName, HH.ProductID, HH.ColorID, HH.WorkingUnitID, HH.LotID";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        public ActionResult PrintProductDetailsStock(int pcid, string WorkingUnitIDs, int layout, int buid, string productname, string LotNo, string SizeName, string SupplierIDs, string StyleIDs, string ColorName, string BuyerName, string sDiaOrGSM, double ts)
        {
            List<Lot> oLots = new List<Lot>();
            string sSQL = this.GetSQL(pcid, WorkingUnitIDs, layout, buid, productname, LotNo, SizeName, SupplierIDs, StyleIDs, ColorName, BuyerName, sDiaOrGSM, "False");
            DataSet oLotsDataSet = Lot.GetsDataSet(sSQL, (int)Session[SessionInfo.currentUserID]);
            DataTable oLotsDataTable = oLotsDataSet.Tables[0];
            oLots = Lot.CreateObjects(oLotsDataTable);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Lot).ToString() + "," + ((int)EnumModuleName.Product).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            byte[] abytes = null;
            if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndColorWise)
            {
                //ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
                //oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.StockReportFormat, (int)Session[SessionInfo.currentUserID]);
                //if (Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.StockReportWithValue)
                if (bIsRateView)
                {
                    rptProductDetailsStock oReport = new rptProductDetailsStock();
                    abytes = oReport.PrepareProductWiseReport(oLots, oCompany, oBusinessUnit, layout);
                }
                else
                {
                    rptProductDetailsStockQty oReport = new rptProductDetailsStockQty();
                    abytes = oReport.PrepareProductWiseReport(oLots, oCompany, oBusinessUnit, layout);
                }
            }
            else
            {
                _oClientOperationSetting = new ClientOperationSetting();
                _oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
                rptProuductDetails_StockAndLotWise oReport = new rptProuductDetails_StockAndLotWise();
                abytes = oReport.PrepareProductAndStockWiseReport(oLots, oCompany, oBusinessUnit, _oClientOperationSetting, layout);
            }

            return File(abytes, "application/pdf");
        }

        public ActionResult PrintGroupOrCategoryWise(int pcid, string WorkingUnitIDs, int layout, int buid, string productname, string LotNo, string SizeName, string SupplierIDs, string StyleIDs, string ColorName, string BuyerName, string sDiaOrGSM, bool IsCategoryWise, double ts)
        {
            List<Lot> oLots = new List<Lot>();
            string sSQL = this.GetSQL(pcid, WorkingUnitIDs, layout, buid, productname, LotNo, SizeName, SupplierIDs, StyleIDs, ColorName, BuyerName, sDiaOrGSM, "True");
            DataSet oLotsDataSet = Lot.GetsDataSet(sSQL, (int)Session[SessionInfo.currentUserID]);
            DataTable oLotsDataTable = oLotsDataSet.Tables[0];
            oLots = Lot.CreateObjects(oLotsDataTable);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            byte[] abytes = null;
            rptProductGroupWithCategoryWise oReport = new rptProductGroupWithCategoryWise();
            abytes = oReport.PrepareProductWiseReport(oLots, oCompany, oBusinessUnit, layout, IsCategoryWise);
            return File(abytes, "application/pdf");
        }

        public void GroupOrCategoryWiseXL(int pcid, string WorkingUnitIDs, int layout, int buid, string productname, string LotNo, string SizeName, string SupplierIDs, string StyleIDs, string ColorName, string BuyerName, string sDiaOrGSM, bool IsCategoryWise, double ts)
        {
            List<Lot> oLots = new List<Lot>();
            string sSQL = this.GetSQL(pcid, WorkingUnitIDs, layout, buid, productname, LotNo, SizeName, SupplierIDs, StyleIDs, ColorName, BuyerName, sDiaOrGSM, "True");
            DataSet oLotsDataSet = Lot.GetsDataSet(sSQL, (int)Session[SessionInfo.currentUserID]);
            DataTable oLotsDataTable = oLotsDataSet.Tables[0];
            oLots = Lot.CreateObjects(oLotsDataTable);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            PrepareProductWiseXL(oLots, oCompany, oBusinessUnit, layout, IsCategoryWise);
        }
        public void PrepareProductWiseXL(List<Lot> oLots, Company oCompany, BusinessUnit oBusinessUnit, int layout, bool IsCategoryWise)
        {
            if (IsCategoryWise)
            {
                this.CategoryWiseXL(oLots, oCompany, oBusinessUnit, layout);
            }
            else
            {
                this.GroupWiseXL(oLots, oCompany, oBusinessUnit, layout);
            }
        }
        private void CategoryWiseXL(List<Lot> oLots, Company oCompany, BusinessUnit oBusinessUnit, int layout)
        {
            int _nRowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            oLots = oLots.OrderBy(x => x.ProductCategoryID).ToList();
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Category Wise Stock Report");
                sheet.Name = "Production_Report";
                int nCount = 2;
                sheet.Column(nCount++).Width = 6; //SL
                sheet.Column(nCount++).Width = 10; //Code
                sheet.Column(nCount++).Width = 45; //Product Name
                sheet.Column(nCount++).Width = 12; //Size
                sheet.Column(nCount++).Width = 15; //Stock Qty
                sheet.Column(nCount++).Width = 10; //Symbol
                sheet.Column(nCount++).Width = 15; //Price
                sheet.Column(nCount++).Width = 15; //Value
                sheet.Column(nCount++).Width = 15; //FC Price


                #region Report Header
                cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _nRowIndex = _nRowIndex + 1;
                cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _nRowIndex = _nRowIndex + 1;
                cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                 cell.Value = oCompany.Phone + "," + oCompany.Email + "," + oCompany.WebAddress; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _nRowIndex = _nRowIndex + 1;

                cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                cell.Value = "Category Wise Stock Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                _nRowIndex = _nRowIndex + 2;
                #endregion

                #region Column Header
                _nRowIndex = _nRowIndex + 1;
                nCount = 0;
                int nCategoryID = 0;
                foreach (Lot oItem in oLots)
                {
                    nCount++;
                    if (nCategoryID != oItem.ProductCategoryID)
                    {
                        _nRowIndex += 1;

                        cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                        cell.Value = "@" + oItem.ProductCategoryName; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        _nRowIndex = _nRowIndex + 1;

                        cell = sheet.Cells[_nRowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 3]; cell.Value = "Code"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 5]; cell.Value = "Size"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 6]; cell.Value = "Stock Qty"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 7]; cell.Value = "MUnit"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 8]; cell.Value = "Price(" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 9]; cell.Value = "Total Value(" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 10]; cell.Value = "Price(" + oLots[0].FCSymbol + ")"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        _nRowIndex += 1;
                        nCategoryID = oItem.ProductCategoryID;
                        nCount = 1;//Reset
                    }
                    cell = sheet.Cells[_nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 3]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 5]; cell.Value = oItem.SizeName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 6]; cell.Value = oItem.ReportingBalance; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##0.00;(##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 7]; cell.Value = oItem.ReportUnitSymbol; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 8]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##0.00;(##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 9]; cell.Value = oItem.StockValue; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##0.00;(##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 10]; cell.Value = oItem.FCUnitPrice; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##0.000;(##0.000)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    _nRowIndex = _nRowIndex + 1;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Category_Wise_Excel.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private void GroupWiseXL(List<Lot> oLots, Company oCompany, BusinessUnit oBusinessUnit, int layout)
        {
            int _nRowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            oLots = oLots.OrderBy(x => x.ProductBaseID).ToList();
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Group Wise Stock Report");
                sheet.Name = "Production_Report";
                int nCount = 2;
                sheet.Column(nCount++).Width = 6; //SL
                sheet.Column(nCount++).Width = 10; //Code
                sheet.Column(nCount++).Width = 45; //Product Name
                sheet.Column(nCount++).Width = 12; //Size
                sheet.Column(nCount++).Width = 15; //Stock Qty
                sheet.Column(nCount++).Width = 10; //Symbol
                sheet.Column(nCount++).Width = 15; //Price
                sheet.Column(nCount++).Width = 15; //Value
                sheet.Column(nCount++).Width = 15; //FC Price


                #region Report Header
                cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _nRowIndex = _nRowIndex + 1;
                cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _nRowIndex = _nRowIndex + 1;
                cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                cell.Value = oCompany.Phone + "," + oCompany.Email + "," + oCompany.WebAddress; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                _nRowIndex = _nRowIndex + 1;

                cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 10]; cell.Merge = true;
                cell.Value = "Group Wise Stock Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                _nRowIndex = _nRowIndex + 2;
                #endregion

                #region Column Header
                _nRowIndex = _nRowIndex + 1;
                nCount = 0;
                int nGroupID = 0;
                foreach (Lot oItem in oLots)
                {
                    nCount++;
                    if (nGroupID != oItem.ProductBaseID)
                    {
                        _nRowIndex += 1;

                        cell = sheet.Cells[_nRowIndex, 2, _nRowIndex, 9]; cell.Merge = true;
                        cell.Value = "@" + oItem.ProductGroupName + " (" + oItem.ProductCategoryName + ")"; cell.Style.Font.Bold = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        _nRowIndex = _nRowIndex + 1;

                        cell = sheet.Cells[_nRowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 3]; cell.Value = "Code"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 5]; cell.Value = "Size"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 6]; cell.Value = "Stock Qty"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 7]; cell.Value = "MUnit"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 8]; cell.Value = "Price(" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 9]; cell.Value = "Total Value(" + oCompany.CurrencySymbol + ")"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[_nRowIndex, 10]; cell.Value = "Price(" + oLots[0].FCSymbol + ")"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        _nRowIndex += 1;
                        nGroupID = oItem.ProductBaseID;
                        nCount = 1;//Reset
                    }

                    cell = sheet.Cells[_nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 3]; cell.Value = oItem.ProductCode; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 4]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 5]; cell.Value = oItem.SizeName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 6]; cell.Value = oItem.ReportingBalance; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##0.00;(##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 7]; cell.Value = oItem.ReportUnitSymbol; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 8]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##0.00;(##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 9]; cell.Value = oItem.StockValue; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##0.00;(##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[_nRowIndex, 10]; cell.Value = oItem.FCUnitPrice; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "##0.000;(##0.000)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    _nRowIndex = _nRowIndex + 1;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Group_Wise_Excel.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public void ExcelExport(int pcid, string WorkingUnitIDs, int layout, int buid, string productname, string LotNo, string SizeName, string SupplierIDs, string StyleIDs, string ColorName, string BuyerName, string sDiaOrGSM, double ts)
        {
            List<Lot> oLots = new List<Lot>();
            string sSQL = this.GetSQL(pcid, WorkingUnitIDs, layout, buid, productname, LotNo, SizeName, SupplierIDs, StyleIDs, ColorName, BuyerName, sDiaOrGSM, "False");
            DataSet oLotsDataSet = Lot.GetsDataSet(sSQL, (int)Session[SessionInfo.currentUserID]);
            DataTable oLotsDataTable = oLotsDataSet.Tables[0];
            oLots = Lot.CreateObjects(oLotsDataTable);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Lot).ToString() + "," + ((int)EnumModuleName.Product).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            //_oClientOperationSetting = new ClientOperationSetting();
            //_oClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 2;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Stock Report");
                sheet.Name = "Stock Report";

                sheet.Column(nEndCol).Width = 20; nEndCol++; //SL 
                sheet.Column(nEndCol).Width = 20; nEndCol++; //Code
                sheet.Column(nEndCol).Width = 40; nEndCol++; //Product Name
                sheet.Column(nEndCol).Width = 20; nEndCol++; //M. Unit
                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndColorWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    sheet.Column(nEndCol).Width = 20; nEndCol++; //Color
                }
                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    sheet.Column(nEndCol).Width = 20; nEndCol++; //store
                }
                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    sheet.Column(nEndCol).Width = 20; nEndCol++; //Lot
                    if (bIsRateView)
                    {
                        sheet.Column(nEndCol).Width = 25; nEndCol++; //Style No
                    }
                }
                sheet.Column(nEndCol).Width = 20; nEndCol++; //Stock Qty
                sheet.Column(nEndCol).Width = 20; //Reporting Qty
                //nEndCol = 10;

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
                cell.Value = "Stock Report"; cell.Style.Font.Bold = true;
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
                int nTempCol = 2;
                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "SL No"; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Code"; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Product Name"; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "M. Unit"; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndColorWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Color Name"; cell.Style.Font.Bold = true; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Store Name"; cell.Style.Font.Bold = true; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Lot No"; cell.Style.Font.Bold = true; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if (_oClientOperationSetting.ValueInString == "Yes")
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Style No"; cell.Style.Font.Bold = true; nTempCol++;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                }
                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Stock Qty"; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Reporting Qty"; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (bIsRateView)
                {
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Value"; cell.Style.Font.Bold = true; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }


                nRowIndex++;
                #endregion

                #region Data
                int nSL = 0; double nQty = 0, nReportingQty = 0;
                foreach (Lot oItem in oLots)
                {
                    nSL++;
                    nTempCol = 2;
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "" + nSL; cell.Style.Font.Bold = false; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "" + oItem.ProductCode; cell.Style.Font.Bold = false; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "" + oItem.ProductName; cell.Style.Font.Bold = false; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "" + oItem.MUName; cell.Style.Font.Bold = false; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndColorWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "" + oItem.ColorName; cell.Style.Font.Bold = false; nTempCol++;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "" + oItem.WorkingUnitName; cell.Style.Font.Bold = false; nTempCol++;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "" + oItem.LotNo; cell.Style.Font.Bold = false; nTempCol++;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (_oClientOperationSetting.ValueInString == "Yes")
                        {
                            cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "" + oItem.StyleNo; cell.Style.Font.Bold = false; nTempCol++;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                    }
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = oItem.Balance; cell.Style.Font.Bold = false; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = oItem.ReportingBalance; cell.Style.Font.Bold = false; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bIsRateView)
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false; nTempCol++;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Numberformat.Format = oItem.Currency + " #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = oItem.UnitPrice * oItem.Balance; cell.Style.Font.Bold = false; nTempCol++;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        cell.Style.Numberformat.Format = oItem.Currency + " #,##0.00";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    }


                    nQty = nQty + oItem.Balance;
                    nReportingQty = nReportingQty + oItem.ReportingBalance;
                    nEndRow = nRowIndex;
                    nRowIndex++;
                }
                #endregion

                #region Total
                nTempCol = 2;
                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = false; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = false; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Numberformat.Format = "";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndColorWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = true; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndStoreWise || (EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = true; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                if ((EnumStockReportLayout)layout == EnumStockReportLayout.ProductAndLotWise)
                {
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = true; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    if (_oClientOperationSetting.ValueInString == "Yes")
                    {
                        cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = true; nTempCol++;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                }
                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = "Total :"; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = nQty; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = nReportingQty; cell.Style.Font.Bold = true; nTempCol++;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (bIsRateView)
                {
                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = false; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.Numberformat.Format = "";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nTempCol]; cell.Value = ""; cell.Style.Font.Bold = false; nTempCol++;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    cell.Style.Numberformat.Format = " #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=StockReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
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
        public ActionResult ProductWiseStock(string ids, double ts)
        {
            _oProducts = new List<Product>();
            string sSql = "SELECT * FROM View_Product WHERE ProductID IN (" + ids + ") ORDER BY ProductName ASC";
            _oProducts = Product.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptProductWiseStock oReport = new rptProductWiseStock();
            byte[] abytes = oReport.PrepareReport(_oProducts, oCompany);
            return File(abytes, "application/pdf");
        }

        [HttpPost]
        public JsonResult GetsCategoryWiseProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            if (oProduct.ProductCategoryID > 0)
            {
                string sSQL = "SELECT * FROM View_Product WHERE ProductCategoryID IN (SELECT HH.ProductCategoryID FROM dbo.fn_GetProductCategory(" + oProduct.ProductCategoryID.ToString() + ") AS HH) ORDER BY ProductName ASC";
                oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                oProducts = Product.Gets((int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print Product Ledger
        public ActionResult PrintProductLedger(int productId, string dtFrom, string dtTo, int lotId, int wuId, int colorId, int buId, double ts)
        {

            int nProductID = 0;
            DateTime dtTransactionFrom, dtTransactionTo;
            dtTransactionFrom = dtTransactionTo = DateTime.MinValue;

            if (!int.TryParse(productId.ToString(), out nProductID) || productId <= 0)
                throw new Exception("Invalid salesman.");
            if (!DateTime.TryParse(dtFrom, out dtTransactionFrom))
                throw new Exception("Start date is not valid.");
            if (!DateTime.TryParse(dtTo, out dtTransactionTo))
                throw new Exception("End date is not valid.");


            string sReturn = "Select LotID from Lot AS HH Where HH.LotID>0 ";
            #region BUID
            if (buId != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.BUID = " + buId.ToString();
            }
            #endregion

            #region Product
            if (productId != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ProductID = " + productId.ToString();
            }
            #endregion


            #region Color
            if (colorId != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.ColorID = " + colorId.ToString();
            }
            #endregion

            #region Store
            if (wuId != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.WorkingUnitID = " + wuId.ToString();
            }
            #endregion

            #region Lot
            if (lotId != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " HH.LotID = " + lotId.ToString();
            }
            #endregion




            Product oProduct = new Product();
            oProduct = oProduct.Get(productId, (int)Session[SessionInfo.currentUserID]);

            bool bIsRateView = false;
            List<AuthorizationRoleMapping> oAuthorizationRoleMapping = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMapping = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Lot).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            oAuthorizationRoleMapping = oAuthorizationRoleMapping.Where(x => x.OperationTypeInt == (int)EnumRoleOperationType.RateView).ToList();
            if (oAuthorizationRoleMapping.Count > 0)
            {
                bIsRateView = true;
            }

            string sSQL = "Select * from View_ITransaction Where CONVERT(Date,TransactionTime) Between '" + dtTransactionFrom.ToString("dd MMM yyyy") + "' And '" + dtTransactionTo.ToString("dd MMM yyyy") + "' ";
            if (productId > 0)
                sSQL += " And ProductID= " + productId.ToString() + "";

            sSQL += " And LotID In (" + sReturn + ")";
            sSQL += " Order By ITransactionID ASC ";
            List<ITransaction> oITransactions = new List<ITransaction>();
            oITransactions = ITransaction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptProductTransactionLedger oReport = new rptProductTransactionLedger();
            byte[] abytes = oReport.PrepareReport(oProduct, oITransactions, oCompany, bIsRateView);
            return File(abytes, "application/pdf");

        }
        #endregion
    }

}
