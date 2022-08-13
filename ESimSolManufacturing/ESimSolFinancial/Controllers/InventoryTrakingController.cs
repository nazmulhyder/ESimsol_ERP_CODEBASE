using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class InventoryTrakingController : Controller
    {
        #region Declaration
        InventoryTraking _oInventoryTraking = new InventoryTraking();
        List<InventoryTraking> _oInventoryTrakings = new List<InventoryTraking>();
        #endregion

        #region Actions
        #region 
        public ActionResult ViewInventoryTrakings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oInventoryTrakings = new List<InventoryTraking>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ViewBag.TriggerParentTypes = EnumObject.jGets(typeof(EnumTriggerParentsType));
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            _oInventoryTrakings = InventoryTraking.Gets_WU(DateTime.Today, DateTime.Today, buid,0, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
          
           

            ViewBag.BUID = buid;
            ViewBag.RouteSheetSetup = oRouteSheetSetup;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(_oInventoryTrakings);
        }
        public ActionResult ViewInventoryTrakingsDyeing(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oInventoryTrakings = new List<InventoryTraking>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            ViewBag.TriggerParentTypes = EnumObject.jGets(typeof(EnumTriggerParentsType));
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            _oInventoryTrakings = InventoryTraking.Gets_WU(DateTime.Today, DateTime.Today, buid, 0, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BUID = buid;
            ViewBag.RouteSheetSetup = oRouteSheetSetup;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(_oInventoryTrakings);
        }
        public ActionResult ViewInventoryTraking_Product(int buid)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            ViewBag.BUID = buid;
            return View(_oInventoryTrakings);
        }
        public ActionResult ViewInventoryTraking_ProductDyeing(int buid)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            ViewBag.BUID = buid;
            return View(_oInventoryTrakings);
        }
        public ActionResult ViewInventoryTraking_Lot(int buid)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            ViewBag.BUID = buid;
            return View(_oInventoryTrakings);
        }
        public ActionResult ViewInventoryTrackingDetails(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
           
            _oInventoryTrakings = new List<InventoryTraking>();
            ViewBag.BUID = buid;

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            //ViewBag.TriggerParentTypes = EnumObject.jGets(typeof(EnumTriggerParentsType));
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == 1 || x.id == 5); ;

            return View(_oInventoryTrakings);
        }
      
        [HttpPost]
        public JsonResult SearchByDate(InventoryTraking oInventoryTraking)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            try
            {
                _oInventoryTrakings = InventoryTraking.Gets_WU(oInventoryTraking.StartDate, oInventoryTraking.EndDate, oInventoryTraking.BUID, oInventoryTraking.TriggerParentType, oInventoryTraking.ValueType,((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count>0)
                {
                    _oInventoryTrakings[0].TriggerParentType = oInventoryTraking.TriggerParentType;
                }
            }
            catch (Exception ex)
            {
                _oInventoryTraking = new InventoryTraking();
                //oRPT_DeliverySummary.ErrorMessage = ex.Message;
                _oInventoryTrakings = new List<InventoryTraking>();
            }
            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchByProduct(InventoryTraking oInventoryTraking)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            try
            {
                _oInventoryTrakings = InventoryTraking.Gets_Product(oInventoryTraking.StartDate, oInventoryTraking.EndDate, oInventoryTraking.BUID, oInventoryTraking.WorkingUnitID, oInventoryTraking.TriggerParentType, oInventoryTraking.ValueType, oInventoryTraking.MUnitID, oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTraking = new InventoryTraking();
                //oRPT_DeliverySummary.ErrorMessage = ex.Message;
                _oInventoryTrakings = new List<InventoryTraking>();
            }
            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchByProductDyeing(InventoryTraking oInventoryTraking)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            try
            {
                _oInventoryTrakings = InventoryTraking.Gets_ProductDyeing(oInventoryTraking.StartDate, oInventoryTraking.EndDate, oInventoryTraking.BUID, oInventoryTraking.WorkingUnitID, oInventoryTraking.TriggerParentType, oInventoryTraking.ValueType, oInventoryTraking.MUnitID, oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTraking = new InventoryTraking();
                //oRPT_DeliverySummary.ErrorMessage = ex.Message;
                _oInventoryTrakings = new List<InventoryTraking>();
            }
            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SearchByLot(InventoryTraking oInventoryTraking)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            try
            {
                _oInventoryTrakings = InventoryTraking.Gets_Lot(oInventoryTraking.StartDate, oInventoryTraking.EndDate, oInventoryTraking.BUID, oInventoryTraking.WorkingUnitID, oInventoryTraking.ProductID, oInventoryTraking.TriggerParentType, oInventoryTraking.ValueType, oInventoryTraking.MUnitID, oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTraking = new InventoryTraking();
                //oRPT_DeliverySummary.ErrorMessage = ex.Message;
                _oInventoryTrakings = new List<InventoryTraking>();
            }
            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetTrackingDetails(InventoryTraking oInventoryTraking)
        {
            _oInventoryTrakings = new List<InventoryTraking>();

            string sWorkingUnitIDs = oInventoryTraking.WUName;
            int nProductCategoryID = oInventoryTraking.ProductID; //Here ProductID Use as ProductCategory 
            try
            {
                _oInventoryTrakings = InventoryTraking.Gets_Qty_Value(oInventoryTraking.StartDate, oInventoryTraking.EndDate, oInventoryTraking.BUID, sWorkingUnitIDs, oInventoryTraking.CurrencyID, nProductCategoryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTraking = new InventoryTraking();
                _oInventoryTrakings = new List<InventoryTraking>();
            }
            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSql(string sTempString)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            int nBUID = 0;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            string _sWorkingUnits = "";
            string _sProducts = "";
            int _nProductID = 0;

            if (!string.IsNullOrEmpty(sTempString))
            {
                nBUID = Convert.ToInt32(sTempString.Split('~')[0]);
                startDate = Convert.ToDateTime(sTempString.Split('~')[1]);
                endDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                _sWorkingUnits = sTempString.Split('~')[3];
                if (sTempString.Split('~').Length > 4)
                    Int32.TryParse(sTempString.Split('~')[4], out _nProductID);
            }

            List<ProductSort> oProductSorts = new List<ProductSort>();
            if (_nProductID > 0)
            {
                oProductSorts = ProductSort.GetsBy(_nProductID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sProducts = _nProductID.ToString() ;
            }
            if (oProductSorts.Count > 0)
            {
                _sProducts =_sProducts+","+ string.Join(",", oProductSorts.Select(x => x.ProductID).ToList());
            }

            string sReturn1 = "SELECT [Datetime] , WorkingUnitID,  ProductID, LotID, TriggerParentType,  TriggerParentID,		InOutType,	PreviousBalance,	CASE WHEN InOutType=101 THEN Qty ELSE 0 END In_Qty, CASE WHEN InOutType=102 THEN Qty ELSE 0 END Out_Qty,	CurrentBalance, DBUserID FROM ITransaction";
            string sReturn = "";

            #region BUID
            if (nBUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotID IN(SELECT LotID FROM Lot WHERE BUID =" + nBUID + ")";
            }
            #endregion

            #region Date Time
            DateObject.CompareDateQuery(ref sReturn, "[Datetime]",5,startDate,endDate);
            _oInventoryTraking.StartDate = startDate;
            _oInventoryTraking.EndDate = endDate;
            #endregion

            #region _sWorkingUnit 
            if (_sWorkingUnits != null && _sWorkingUnits != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " WorkingUnitID IN(" + _sWorkingUnits + ")";
            }
            #endregion
            #region _sProducts
            if (_sProducts != null && _sProducts != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID in (" + _sProducts + ")";
            }
            #endregion

            return sReturn1 + sReturn;
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
        public ActionResult PrintPriview(string sTempString)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            _oInventoryTraking = new InventoryTraking();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            int nReportType = 0;
            int nValueType = 0;
            string _sHeaderName = "";
            string _sDateRange = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                _oInventoryTraking.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                _oInventoryTraking.DateType = Convert.ToInt32(sTempString.Split('~')[1]);
                _oInventoryTraking.StartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                _oInventoryTraking.EndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                _oInventoryTraking.TriggerParentType = Convert.ToInt32(sTempString.Split('~')[4]);
                _oInventoryTraking.WorkingUnitID = Convert.ToInt32(sTempString.Split('~')[5]);
                _oInventoryTraking.ProductID = Convert.ToInt32(sTempString.Split('~')[6]);
                nReportType = Convert.ToInt32(sTempString.Split('~')[7]);
                nValueType = Convert.ToInt32(sTempString.Split('~')[8]);
                _oInventoryTraking.MUnitID = Convert.ToInt32(sTempString.Split('~')[9]);
                _oInventoryTraking.CurrencyID = Convert.ToInt32(sTempString.Split('~')[10]);
              
                oBusinessUnit = oBusinessUnit.Get(_oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                WorkingUnit oWorkingUnit = new WorkingUnit();
                if (_oInventoryTraking.WorkingUnitID > 0)
                {
                    oWorkingUnit = oWorkingUnit.Get(_oInventoryTraking.WorkingUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                Product oProduct = new Product();
                if (_oInventoryTraking.ProductID > 0)
                {
                    oProduct = oProduct.Get(_oInventoryTraking.ProductID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                _sDateRange = "Date: " + _oInventoryTraking.StartDate.ToString("dd MMM yyyy") + " To " + _oInventoryTraking.EndDate.ToString("dd MMM yyyy");
                if (nReportType == 1)
                {
                    _sHeaderName = "Stock Report: All " + oBusinessUnit.ShortName + " Store";
                    _oInventoryTrakings = InventoryTraking.Gets_WU(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.TriggerParentType, nValueType,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (nReportType == 2)
                {

                    _sHeaderName = "Stock Report Store: " + oWorkingUnit.WorkingUnitName + "";
                    _oInventoryTrakings = InventoryTraking.Gets_Product(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.WorkingUnitID, _oInventoryTraking.TriggerParentType, nValueType, _oInventoryTraking.MUnitID, _oInventoryTraking.CurrencyID,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (nReportType == 3)
                {
                    _sHeaderName = "Stock Report Store: " + oWorkingUnit.WorkingUnitName + " " + ",\nProduct: " + oProduct.ProductName;
                    _oInventoryTrakings = InventoryTraking.Gets_Lot(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.WorkingUnitID, _oInventoryTraking.ProductID, _oInventoryTraking.TriggerParentType, nValueType, _oInventoryTraking.MUnitID, _oInventoryTraking.CurrencyID,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oInventoryTrakings.Count > 0)
                {
                    _oInventoryTrakings[0].TriggerParentType = _oInventoryTraking.TriggerParentType;
                }
                
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptInventoryTraking oReport = new rptInventoryTraking();
            byte[] abytes=null;
            if (nReportType == 1)
            {
                abytes = oReport.PrepareReport(_oInventoryTrakings, oCompany, oBusinessUnit, _sHeaderName, _sDateRange, nValueType);
            }
            else if (nReportType == 2)
            {
                abytes = oReport.PrepareReportProduct(_oInventoryTrakings, oCompany, oBusinessUnit, _sHeaderName, _sDateRange, nValueType);
            }
            else if (nReportType == 3)
            {
                abytes = oReport.PrepareReportLot(_oInventoryTrakings, oCompany, oBusinessUnit, _sHeaderName, _sDateRange, nValueType);
            }
            return File(abytes, "application/pdf");


        }

        public void Print_ReportPreviewXL(string sTempString)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            _oInventoryTraking = new InventoryTraking();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            int nReportType = 0;
            int nValueType = 0;
            string _sHeaderName = "";
            string _sDateRange = "";
            string _sUnit = "";
            string _sValueType = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                _oInventoryTraking.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                _oInventoryTraking.DateType = Convert.ToInt32(sTempString.Split('~')[1]);
                _oInventoryTraking.StartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                _oInventoryTraking.EndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                _oInventoryTraking.TriggerParentType = Convert.ToInt32(sTempString.Split('~')[4]);
                _oInventoryTraking.WorkingUnitID = Convert.ToInt32(sTempString.Split('~')[5]);
                _oInventoryTraking.ProductID = Convert.ToInt32(sTempString.Split('~')[6]);
                nReportType = Convert.ToInt32(sTempString.Split('~')[7]);
                nValueType = Convert.ToInt32(sTempString.Split('~')[8]);
                _oInventoryTraking.MUnitID = Convert.ToInt32(sTempString.Split('~')[9]);
                _oInventoryTraking.CurrencyID = Convert.ToInt32(sTempString.Split('~')[10]);

                if (nValueType == 1)
                {
                    _sUnit = "Unit/Currency";
                    _sValueType = "Qty/Value";
                }
                else if (nValueType == 2)
                {
                    _sUnit = "Currency";
                    _sValueType = "Value";
                }
                else
                {
                    _sUnit = "Unit";
                    _sValueType = "Qty";
                }

                oBusinessUnit = oBusinessUnit.Get(_oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                WorkingUnit oWorkingUnit = new WorkingUnit();
                if (_oInventoryTraking.WorkingUnitID > 0)
                {
                    oWorkingUnit = oWorkingUnit.Get(_oInventoryTraking.WorkingUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                Product oProduct = new Product();
                if (_oInventoryTraking.ProductID > 0)
                {
                    oProduct = oProduct.Get(_oInventoryTraking.ProductID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                _sDateRange = "Date: " + _oInventoryTraking.StartDate.ToString("dd MMM yyyy") + " To " + _oInventoryTraking.EndDate.ToString("dd MMM yyyy");
                if (nReportType == 1)
                {
                    _sHeaderName = "Stock Report: All " + oBusinessUnit.ShortName + " Store";
                    _oInventoryTrakings = InventoryTraking.Gets_WU(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.TriggerParentType, nValueType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (nReportType == 2)
                {

                    _sHeaderName = "Stock Report Store: " + oWorkingUnit.WorkingUnitName + "";
                    _oInventoryTrakings = InventoryTraking.Gets_Product(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.WorkingUnitID, _oInventoryTraking.TriggerParentType, nValueType, _oInventoryTraking.MUnitID, _oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (nReportType == 3)
                {
                    _sHeaderName = "Stock Report Store: " + oWorkingUnit.WorkingUnitName + " " + ",\nProduct: " + oProduct.ProductName;
                    _oInventoryTrakings = InventoryTraking.Gets_Lot(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.WorkingUnitID, _oInventoryTraking.ProductID, _oInventoryTraking.TriggerParentType, nValueType, _oInventoryTraking.MUnitID, _oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oInventoryTrakings.Count > 0)
                {
                    _oInventoryTrakings[0].TriggerParentType = _oInventoryTraking.TriggerParentType;
                }

            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            int nSL = 0;
            double nInQty = 0;
            double nOpeningQty = 0;
            double nClosingQty = 0;
            double nOutQty = 0;

            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 8;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Inventory Report");
                sheet.Name = "Inventory Report";
                sheet.Column(2).Width = 40;
                sheet.Column(3).Width = 10;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
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



                #region Report Header Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sHeaderName; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = true;
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
                #region WorkingUnit
                if (nReportType == 1)
                {
                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = _sUnit; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "In Qty" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Out Qty" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                   
                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (InventoryTraking oItem in _oInventoryTrakings)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.WUName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = (oItem.OpeningQty > 0) ? Global.MillionFormat(oItem.OpeningQty) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = (oItem.InQty > 0) ? Global.MillionFormat(oItem.InQty) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.OutQty > 0) ? Global.MillionFormat(oItem.OutQty) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = (oItem.ClosingQty > 0) ? Global.MillionFormat(oItem.ClosingQty) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nOpeningQty = nOpeningQty + oItem.OpeningQty;
                        nInQty = nInQty + oItem.InQty;
                        nOutQty = nOutQty + oItem.OutQty;
                        nClosingQty = nClosingQty + oItem.ClosingQty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }
                    #endregion
                    if (nValueType != 1)
                    {
                        #region Total
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = _oInventoryTrakings.Sum(x => x.OpeningQty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = _oInventoryTrakings.Sum(x => x.InQty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = _oInventoryTrakings.Sum(x => x.OutQty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = _oInventoryTrakings.Sum(x => x.ClosingQty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        #endregion
                    }
                }
                #endregion
                #region Product
                if (nReportType == 2)
                {
                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = _sUnit; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "In " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Out " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (InventoryTraking oItem in _oInventoryTrakings)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OpeningQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.InQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.OutQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.ClosingQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nOpeningQty = nOpeningQty + oItem.OpeningQty;
                        nInQty = nInQty + oItem.InQty;
                        nOutQty = nOutQty + oItem.OutQty;
                        nClosingQty = nClosingQty + oItem.ClosingQty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }
                    #endregion
                    if (nValueType != 1)
                    {
                        #region Total
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = nOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = nInQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nOutQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = nClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        #endregion
                    }
                }
                #endregion
                #region Lot
                if (nReportType == 3)
                {
                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = _sUnit; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "In  " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Out " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (InventoryTraking oItem in _oInventoryTrakings)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OpeningQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.InQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.OutQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.ClosingQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nOpeningQty = nOpeningQty + oItem.OpeningQty;
                        nInQty = nInQty + oItem.InQty;
                        nOutQty = nOutQty + oItem.OutQty;
                        nClosingQty = nClosingQty + oItem.ClosingQty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }
                    #endregion
                    if (nValueType != 1)
                    {
                        #region Total
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = nOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = nInQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nOutQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = nClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        #endregion
                    }
                }
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=InventoryReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion


        }

        public ActionResult PrintPriviewDyeing(string sTempString)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            _oInventoryTraking = new InventoryTraking();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            int nReportType = 0;
            int nValueType = 0;
            string _sHeaderName = "";
            string _sDateRange = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                _oInventoryTraking.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                _oInventoryTraking.DateType = Convert.ToInt32(sTempString.Split('~')[1]);
                _oInventoryTraking.StartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                _oInventoryTraking.EndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                _oInventoryTraking.TriggerParentType = Convert.ToInt32(sTempString.Split('~')[4]);
                _oInventoryTraking.WorkingUnitID = Convert.ToInt32(sTempString.Split('~')[5]);
                _oInventoryTraking.ProductID = Convert.ToInt32(sTempString.Split('~')[6]);
                nReportType = Convert.ToInt32(sTempString.Split('~')[7]);
                nValueType = Convert.ToInt32(sTempString.Split('~')[8]);
                _oInventoryTraking.MUnitID = Convert.ToInt32(sTempString.Split('~')[9]);
                _oInventoryTraking.CurrencyID = Convert.ToInt32(sTempString.Split('~')[10]);

                oBusinessUnit = oBusinessUnit.Get(_oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                WorkingUnit oWorkingUnit = new WorkingUnit();
                if (_oInventoryTraking.WorkingUnitID > 0)
                {
                    oWorkingUnit = oWorkingUnit.Get(_oInventoryTraking.WorkingUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                Product oProduct = new Product();
                if (_oInventoryTraking.ProductID > 0)
                {
                    oProduct = oProduct.Get(_oInventoryTraking.ProductID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                _sDateRange = "Date: " + _oInventoryTraking.StartDate.ToString("dd MMM yyyy") + " To " + _oInventoryTraking.EndDate.ToString("dd MMM yyyy");
                if (nReportType == 1)
                {
                    _sHeaderName = "Stock Report: All " + oBusinessUnit.ShortName + " Store";
                    _oInventoryTrakings = InventoryTraking.Gets_WU(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.TriggerParentType, nValueType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (nReportType == 2)
                {

                    _sHeaderName = "Stock Report Store: " + oWorkingUnit.WorkingUnitName + "";
                    _oInventoryTrakings = InventoryTraking.Gets_ProductDyeing(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.WorkingUnitID, _oInventoryTraking.TriggerParentType, nValueType, _oInventoryTraking.MUnitID, _oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (nReportType == 3)
                {
                    _sHeaderName = "Stock Report Store: " + oWorkingUnit.WorkingUnitName + " " + ",\nProduct: " + oProduct.ProductName;
                    _oInventoryTrakings = InventoryTraking.Gets_Lot(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.WorkingUnitID, _oInventoryTraking.ProductID, _oInventoryTraking.TriggerParentType, nValueType, _oInventoryTraking.MUnitID, _oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oInventoryTrakings.Count > 0)
                {
                    _oInventoryTrakings[0].TriggerParentType = _oInventoryTraking.TriggerParentType;
                }

            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptInventoryTraking oReport = new rptInventoryTraking();
            byte[] abytes = null;
            if (nReportType == 1)
            {
                abytes = oReport.PrepareReportDyeing(_oInventoryTrakings, oCompany, oBusinessUnit, _sHeaderName, _sDateRange, nValueType);
            }
            else if (nReportType == 2)
            {
                abytes = oReport.PrepareReportProductDyeing(_oInventoryTrakings, oCompany, oBusinessUnit, _sHeaderName, _sDateRange, nValueType);
            }
            else if (nReportType == 3)
            {
                abytes = oReport.PrepareReportLot(_oInventoryTrakings, oCompany, oBusinessUnit, _sHeaderName, _sDateRange, nValueType);
            }
            return File(abytes, "application/pdf");


        }

     

        #region  Print_ReportXL
        public void Print_ReportXL(string sTempString)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            _oInventoryTraking = new InventoryTraking();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            int nReportType = 0;
            int nValueType = 0;
            string _sHeaderName = "";
            string _sDateRange = "";
            string _sUnit = "";
            string _sValueType = "";

            if (!string.IsNullOrEmpty(sTempString))
            {
                _oInventoryTraking.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                _oInventoryTraking.DateType = Convert.ToInt32(sTempString.Split('~')[1]);
                _oInventoryTraking.StartDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                _oInventoryTraking.EndDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                _oInventoryTraking.TriggerParentType = Convert.ToInt32(sTempString.Split('~')[4]);
                _oInventoryTraking.WorkingUnitID = Convert.ToInt32(sTempString.Split('~')[5]);
                _oInventoryTraking.ProductID = Convert.ToInt32(sTempString.Split('~')[6]);
                nReportType = Convert.ToInt32(sTempString.Split('~')[7]);
                nValueType = Convert.ToInt32(sTempString.Split('~')[8]);

                _oInventoryTraking.MUnitID = Convert.ToInt32(sTempString.Split('~')[9]);
                _oInventoryTraking.CurrencyID = Convert.ToInt32(sTempString.Split('~')[10]);

                if (nValueType == 1)
                {
                    _sUnit = "Unit/Currency";
                    _sValueType = "Qty/Value";
                }
                else if (nValueType == 2)
                {
                    _sUnit = "Currency";
                    _sValueType = "Value";
                }
                else
                {
                    _sUnit = "Unit";
                    _sValueType = "Qty";
                }

                oBusinessUnit = oBusinessUnit.Get(_oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                WorkingUnit oWorkingUnit = new WorkingUnit();
                if (_oInventoryTraking.WorkingUnitID > 0)
                {
                    oWorkingUnit = oWorkingUnit.Get(_oInventoryTraking.WorkingUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                Product oProduct = new Product();
                if (_oInventoryTraking.ProductID > 0)
                {
                    oProduct = oProduct.Get(_oInventoryTraking.ProductID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                _sDateRange = "Date: " + _oInventoryTraking.StartDate.ToString("dd MMM yyyy") + " To" + _oInventoryTraking.EndDate.ToString("dd MMM yyyy");
                if (nReportType == 1)
                {
                    _sHeaderName = "Inventory Report: All " + oBusinessUnit.ShortName + " Store";
                    _oInventoryTrakings = InventoryTraking.Gets_WU(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.TriggerParentType, nValueType, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (nReportType == 2)
                {

                    _sHeaderName = "Inventory Report Store: " + oWorkingUnit.WorkingUnitName + "";
                    _sHeaderName = "Stock Report Store: " + oWorkingUnit.WorkingUnitName + "";
                    _oInventoryTrakings = InventoryTraking.Gets_ProductDyeing(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.WorkingUnitID, _oInventoryTraking.TriggerParentType, nValueType, _oInventoryTraking.MUnitID, _oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else if (nReportType == 3)
                {
                    _sHeaderName = "Stock Report Store: " + oWorkingUnit.WorkingUnitName + " " + ",\nProduct: " + oProduct.ProductName;
                    _oInventoryTrakings = InventoryTraking.Gets_Lot(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, _oInventoryTraking.WorkingUnitID, _oInventoryTraking.ProductID, _oInventoryTraking.TriggerParentType, nValueType, _oInventoryTraking.MUnitID, _oInventoryTraking.CurrencyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oInventoryTrakings.Count > 0)
                {
                    _oInventoryTrakings[0].TriggerParentType = _oInventoryTraking.TriggerParentType;
                }

            }


            int nSL = 0;
            double nInQty = 0;
            double nOpeningQty = 0;
            double nClosingQty = 0;
            double nOutQty = 0;


            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 8;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Inventory Report");
                sheet.Name = "Inventory Report";
                sheet.Column(2).Width = 40;
                sheet.Column(3).Width = 10;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
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



                #region Report Header Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sHeaderName; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Date Print

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = _sDateRange; cell.Style.Font.Bold = true;
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
              
                #region WorkingUnit
                if (nReportType == 1)
                {

                    #region HEADER
                    cell = sheet.Cells[nRowIndex, 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex,3]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex,4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5, nRowIndex, 10]; cell.Merge = true; cell.Value = "In Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, 16]; cell.Merge = true; cell.Value = "Out Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion


                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Store Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = _sUnit; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "GRN In" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Adj. In" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Pro. In" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Trans. In" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "SW Req. In" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Return In" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Adj. Out" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Pro. Out" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Trans. Out" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = "SW Req. Out" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = "DC/Con" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (InventoryTraking oItem in _oInventoryTrakings)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "" + oItem.WUName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = (oItem.OpeningQty > 0) ? Global.MillionFormat(oItem.OpeningQty) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = (oItem.InGRN > 0) ? Global.MillionFormat(oItem.InGRN) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.InAdj > 0) ? Global.MillionFormat(oItem.InAdj) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.InRS > 0) ? Global.MillionFormat(oItem.InRS) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.InTr > 0) ? Global.MillionFormat(oItem.InTr) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = (oItem.InTrSW > 0) ? Global.MillionFormat(oItem.InTrSW) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = (oItem.InRet > 0) ? Global.MillionFormat(oItem.InRet) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = (oItem.OutAdj > 0) ? Global.MillionFormat(oItem.OutAdj) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = (oItem.OutRS > 0) ? Global.MillionFormat(oItem.OutRS) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = (oItem.OutTr > 0) ? Global.MillionFormat(oItem.OutTr) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = (oItem.OutTrSW > 0) ? Global.MillionFormat(oItem.OutTrSW) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = ((oItem.OutCon + oItem.OutDC) > 0) ? Global.MillionFormat(oItem.OutCon + oItem.OutDC) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = (oItem.ClosingQty > 0) ? Global.MillionFormat(oItem.ClosingQty) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nOpeningQty = nOpeningQty + oItem.OpeningQty;
                        nInQty = nInQty + oItem.InQty;
                        nOutQty = nOutQty + oItem.OutQty;
                        nClosingQty = nClosingQty + oItem.ClosingQty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }
                    #endregion
                    if (nValueType != 1)
                    {
                        #region Total
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = _oInventoryTrakings.Sum(x=>x.OpeningQty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = _oInventoryTrakings.Sum(x => x.InGRN); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = _oInventoryTrakings.Sum(x => x.InAdj); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = _oInventoryTrakings.Sum(x => x.InRS); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = _oInventoryTrakings.Sum(x => x.InTr); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = _oInventoryTrakings.Sum(x => x.InTrSW); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = _oInventoryTrakings.Sum(x => x.InRet); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = _oInventoryTrakings.Sum(x => x.OutAdj); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = _oInventoryTrakings.Sum(x => x.OutRS); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = _oInventoryTrakings.Sum(x => x.OutTr); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = _oInventoryTrakings.Sum(x => x.OutTrSW); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nValue = _oInventoryTrakings.Sum(x => x.OutCon) +  _oInventoryTrakings.Sum(x => x.OutDC);
                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = nValue; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = _oInventoryTrakings.Sum(x => x.ClosingQty); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        #endregion
                    }
                }
                #endregion
                #region Product
                if (nReportType == 2)
                {

                    #region HEADER
                    cell = sheet.Cells[nRowIndex, 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5, nRowIndex, 10]; cell.Merge = true; cell.Value = "In Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11, nRowIndex, 16]; cell.Merge = true; cell.Value = "Out Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = _sUnit; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "GRN In " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Adj In " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Pro In %" + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Trans In " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "SW REQ. In " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Return In " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Adj Out " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Pro Out " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Trans Out " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = "SW Req Out " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = "DC/CON Out " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (InventoryTraking oItem in _oInventoryTrakings)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OpeningQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = (oItem.InGRN > 0) ? Global.MillionFormat(oItem.InGRN) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.InAdj > 0) ? Global.MillionFormat(oItem.InAdj) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.InRS > 0) ? Global.MillionFormat(oItem.InRS) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = (oItem.InTr > 0) ? Global.MillionFormat(oItem.InTr) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = (oItem.InTrSW > 0) ? Global.MillionFormat(oItem.InTrSW) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = (oItem.InRet > 0) ? Global.MillionFormat(oItem.InRet) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = (oItem.OutAdj > 0) ? Global.MillionFormat(oItem.OutAdj) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = (oItem.OutRS > 0) ? Global.MillionFormat(oItem.OutRS) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = (oItem.OutTr > 0) ? Global.MillionFormat(oItem.OutTr) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = (oItem.OutTrSW > 0) ? Global.MillionFormat(oItem.OutTrSW) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = ((oItem.OutCon + oItem.OutDC) > 0) ? Global.MillionFormat(oItem.OutCon + oItem.OutDC) : ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = (oItem.ClosingQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nOpeningQty = nOpeningQty + oItem.OpeningQty;
                        nInQty = nInQty + oItem.InQty;
                        nOutQty = nOutQty + oItem.OutQty;
                        nClosingQty = nClosingQty + oItem.ClosingQty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }
                    #endregion
                    if (nValueType != 1)
                    {
                        #region Total
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = nOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = _oInventoryTrakings.Sum(x => x.InGRN); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = _oInventoryTrakings.Sum(x => x.InAdj); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = _oInventoryTrakings.Sum(x => x.InRS); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = _oInventoryTrakings.Sum(x => x.InTr); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = _oInventoryTrakings.Sum(x => x.InTrSW); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = _oInventoryTrakings.Sum(x => x.InRet); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = _oInventoryTrakings.Sum(x => x.OutAdj); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = _oInventoryTrakings.Sum(x => x.OutRS); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = _oInventoryTrakings.Sum(x => x.OutTr); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = _oInventoryTrakings.Sum(x => x.OutTrSW); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        double nValue = _oInventoryTrakings.Sum(x => x.OutCon) + _oInventoryTrakings.Sum(x => x.OutDC);
                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = nValue; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = nClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        #endregion
                    }
                }
                #endregion
                #region Lot
                if (nReportType == 3)
                {
                    #region

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = _sUnit; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Opening Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "In  " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Out " + _sValueType; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Closing Balance"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#####";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nRowIndex++;
                    #endregion

                    #region Data
                    foreach (InventoryTraking oItem in _oInventoryTrakings)
                    {

                        nSL++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.MUnit; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.OpeningQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.InQty; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = (oItem.OutQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = (oItem.ClosingQty); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nOpeningQty = nOpeningQty + oItem.OpeningQty;
                        nInQty = nInQty + oItem.InQty;
                        nOutQty = nOutQty + oItem.OutQty;
                        nClosingQty = nClosingQty + oItem.ClosingQty;

                        nEndRow = nRowIndex;
                        nRowIndex++;

                    }
                    #endregion
                    if (nValueType != 1)
                    {
                        #region Total
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = nOpeningQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = nInQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nOutQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = nClosingQty; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex = nRowIndex + 1;
                        #endregion
                    }
                }
                #endregion


                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=InventoryReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

            #endregion

        }
        #endregion
       
        public void ExportToExcel_ITReport(string sTempString)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            _oInventoryTraking = new InventoryTraking();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            bool bHasValuePermission = false;
            string sWorkingUnitIDs ="";
            string _sHeaderName = "";
            string _sDateRange = "";
            string _sErrorMesage = "";
            int nProductCategoryID = 0;
            if (!string.IsNullOrEmpty(sTempString))
            {
                _oInventoryTraking.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                _oInventoryTraking.StartDate = Convert.ToDateTime(sTempString.Split('~')[1]);
                _oInventoryTraking.EndDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                sWorkingUnitIDs = sTempString.Split('~')[3];
                if (sTempString.Split('~').Count() > 4)
                {
                    nProductCategoryID = Convert.ToInt32(sTempString.Split('~')[4]);
                }                
            }

            try
            {
                oBusinessUnit = oBusinessUnit.Get(_oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                List<AuthorizationRoleMapping> oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

                if (oAuthorizationRoleMappings.Where(x => x.OperationType == EnumRoleOperationType.RateView).Any())
                    bHasValuePermission = true;

                WorkingUnit oWorkingUnit = new WorkingUnit();
                if (_oInventoryTraking.WorkingUnitID > 0)
                {
                    oWorkingUnit = oWorkingUnit.Get(_oInventoryTraking.WorkingUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                Product oProduct = new Product();
                if (_oInventoryTraking.ProductID > 0)
                {
                    oProduct = oProduct.Get(_oInventoryTraking.ProductID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                _sDateRange = "Date: " + _oInventoryTraking.StartDate.ToString("dd MMM yyyy") + " To " + _oInventoryTraking.EndDate.ToString("dd MMM yyyy");
                _sHeaderName = "Inventory Report: All " + oBusinessUnit.ShortName + " Store";
                _oInventoryTrakings = InventoryTraking.Gets_Qty_Value(_oInventoryTraking.StartDate, _oInventoryTraking.EndDate, _oInventoryTraking.BUID, sWorkingUnitIDs, _oInventoryTraking.CurrencyID, nProductCategoryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oInventoryTrakings = new List<InventoryTraking>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false, Align=TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Store", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Product Name", Width = 35f, IsRotate = false, Align = TextAlign.Center });

                table_header.Add(new TableHeader
                {
                    Header = "Qty",
                    ChildHeader = new List<TableHeader>()
                     {
                        new TableHeader { Header = "Unit", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                        new TableHeader { Header = "Openning Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                        new TableHeader { Header = "In Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                        new TableHeader { Header = "Out Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                        new TableHeader { Header = "Closing Qty", Width = 10f, IsRotate = false, Align = TextAlign.Center }
                     },
                    Width = 50f,
                    IsRotate = false,
                    Align = TextAlign.Center 
                });

                if(bHasValuePermission)
                table_header.Add(new TableHeader { Header = "Value", Align = TextAlign.Center ,
                     ChildHeader = new List<TableHeader>()
                     {
                        new TableHeader { Header = "Currency", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                        new TableHeader { Header = "Openning Value", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                        new TableHeader { Header = "In Value", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                        new TableHeader { Header = "Out Value", Width = 10f, IsRotate = false, Align = TextAlign.Center },
                        new TableHeader { Header = "Closing Value", Width = 10f, IsRotate = false, Align = TextAlign.Center }
                     },          
                     Width = 50f, IsRotate = false});

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = 14;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Inventory Tracking");
                    sheet.Name = "Inventory Tracking";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = (bHasValuePermission? 14 : 9);

                    #region Report Header
                    int nMiddleCol = (bHasValuePermission ? 8 : 4);
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nMiddleCol]; cell.Merge = true;
                    cell.Value = _sHeaderName; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nMiddleCol+1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value =  _sDateRange ; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nMiddleCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, nMiddleCol+1, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Data
                 
                    nRowIndex++;

                    nStartCol = 2;

                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);

                    int nCount = 0; nEndCol = (bHasValuePermission ? 14 : 9);
                    string sCurrencySymbol="";
                    foreach (var obj in _oInventoryTrakings)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.WUName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);

                        ExcelTool.Formatter = " #,##0;(#,##0)";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MUnit, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OpeningQty.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InQty.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OutQty.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ClosingQty.ToString(), true);

                        if (bHasValuePermission)
                        {
                            ExcelTool.Formatter = " #,##0;(#,##0)";
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Currency, false);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OpeningValue.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.InValue.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OutValue.ToString(), true);
                            ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ClosingValue.ToString(), true);
                        }
                        nRowIndex++;

                        sCurrencySymbol = obj.Currency;
                    }
                    #endregion

                    nRowIndex++;

                    var data = _oInventoryTrakings.GroupBy(x => new { x.Currency }, (key, grp) => new
                    {
                        Currency = key.Currency,    //unique dt
                        TotalOpening = grp.Sum(x => x.OpeningValue),              //All data
                        TotalIn = grp.Sum(x => x.InValue),              //All data
                        TotalOut= grp.Sum(x => x.OutValue),              //All data
                        TotalClosing = grp.Sum(x => x.ClosingValue)             //All data
                    });

                    string sTotal = " Total ";

                    if (bHasValuePermission)
                    foreach (var oItem in data) 
                    {
                        nStartCol = 10;
                        ExcelTool.Formatter = oItem.Currency + " #,##0;(#,##0)"; 
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, sTotal, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.TotalOpening.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.TotalIn.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.TotalOut.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, oItem.TotalClosing.ToString(), true);
                        sTotal = ""; nRowIndex++;
                    }

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol+1];

                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=InventoryTraking.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion
        #region Inventory Transaction
        public ActionResult ViewInventoryTransactions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
           
            _oInventoryTrakings = new List<InventoryTraking>();
            ViewBag.BUID = buid;

            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            //ViewBag.TriggerParentTypes = EnumObject.jGets(typeof(EnumTriggerParentsType));
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == 1 || x.id == 5); ;

            return View(_oInventoryTrakings);
        }
        [HttpPost]
        public JsonResult GetTransactions(InventoryTraking oInventoryTraking)
        {
            _oInventoryTrakings = new List<InventoryTraking>();

            string sWorkingUnitIDs = oInventoryTraking.WUName;
            try
            {
                string sSQL = this.MakeSql(oInventoryTraking.ErrorMessage);
                _oInventoryTrakings = InventoryTraking.Gets_ITransactions(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTraking = new InventoryTraking();
                _oInventoryTrakings = new List<InventoryTraking>();
            }
            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        public ActionResult PrintInventoryTransactions(string sTempString)
        {
            string sErrorMessage = "";
            BusinessUnit oBusinessUnit = new BusinessUnit();
            Company oCompany = new Company();
            InventoryTraking oInventoryTraking = new InventoryTraking();
            try
            {
                string sSQL = this.MakeSql(sTempString);
                _oInventoryTrakings = InventoryTraking.Gets_ITransactions(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                oBusinessUnit = oBusinessUnit.Get(1, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
                _oInventoryTraking = new InventoryTraking();
                _oInventoryTrakings = new List<InventoryTraking>();
            }

            if (_oInventoryTrakings.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!! \n" + sErrorMessage);
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();

                SelectedField oSelectedField = new SelectedField("StartDatetimeSt", "Date", 55, SelectedField.FieldType.Data, SelectedField.Alginment.CENTER); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("OPUName", "Store Name", 60, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ProductName", "Product Name", 130, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("LotNo", "Lot No", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ParentTypeEnumSt", "Parent Type", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("OpeningQty", "Opening Balance", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("InQty", "In Qty", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("OutQty", "Out Qty", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ClosingQty", "Current Balance", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);

                rptDynamicReport oReport = new rptDynamicReport(595, 842);
                oReport.SpanTotal = 4;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReport(_oInventoryTrakings.Cast<object>().ToList(), oBusinessUnit, oCompany, "Inventory Transaction Report ", oSelectedFields); //\n(" + oITransactions[0].ProductName + ", " + oITransactions[0].LotNo + ")"
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintInventoryTransactionsType(string sTempString)
        {
            string sErrorMessage = "";
            string sDateRange = "";
            BusinessUnit oBusinessUnit = new BusinessUnit();
            Company oCompany = new Company();
            InventoryTraking oInventoryTraking = new InventoryTraking();
            try
            {
                string sSQL = this.MakeSql(sTempString);
                _oInventoryTrakings = InventoryTraking.Gets_ITransactions(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                oBusinessUnit = oBusinessUnit.Get(1, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
                _oInventoryTraking = new InventoryTraking();
                _oInventoryTrakings = new List<InventoryTraking>();
            }

            if (_oInventoryTrakings.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!! \n" + sErrorMessage);
                return File(abytes, "application/pdf");
            }
            else
            {
                sDateRange = "Date " + _oInventoryTraking.StartDatetimeSt + " to " + _oInventoryTraking.EndDatetimeSt;
              
                rptDynamicReport oReport = new rptDynamicReport(595, 842);
                oReport.SpanTotal = 4;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReportDyeing(_oInventoryTrakings, oBusinessUnit, oCompany, "Inventory Transaction Report ", sDateRange); //\n(" + oITransactions[0].ProductName + ", " + oITransactions[0].LotNo + ")"
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintInventoryTransactionsTypeAllStore(string sTempString)
        {
            string sErrorMessage = "";
            string sDateRange = "";
            BusinessUnit oBusinessUnit = new BusinessUnit();
            Company oCompany = new Company();
            InventoryTraking oInventoryTraking = new InventoryTraking();
            try
            {
                string sSQL = this.MakeSql(sTempString);
                _oInventoryTrakings = InventoryTraking.Gets_ITransactions(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, 0, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                oBusinessUnit = oBusinessUnit.Get(1, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
                _oInventoryTraking = new InventoryTraking();
                _oInventoryTrakings = new List<InventoryTraking>();
            }

            if (_oInventoryTrakings.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!! \n" + sErrorMessage);
                return File(abytes, "application/pdf");
            }
            else
            {
                sDateRange = "Date " + _oInventoryTraking.StartDatetimeSt + " to " + _oInventoryTraking.EndDatetimeSt;

                rptDynamicReport oReport = new rptDynamicReport(595, 842);
                oReport.SpanTotal = 4;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReportDyeingAllStore(_oInventoryTrakings, oBusinessUnit, oCompany, "Inventory Transaction Report ", sDateRange); //\n(" + oITransactions[0].ProductName + ", " + oITransactions[0].LotNo + ")"
                return File(abytes, "application/pdf");
            }
        }
        #endregion

        #region Product Wise
        public ActionResult ViewInventoryTraking_ProductWise(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oInventoryTrakings = new List<InventoryTraking>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oBusinessUnits.Add(oBusinessUnit);
            }
            else
            {
                oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            }

           // _oInventoryTrakings = InventoryTraking.Gets_ProductWise("", buid, DateTime.Now, DateTime.Now,((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BUID = buid;
            ViewBag.RouteSheetSetup = oRouteSheetSetup;
            ViewBag.BusinessUnits = oBusinessUnits;
            return View(_oInventoryTrakings);
        }
        [HttpPost]
        public JsonResult GetsForProductWise(InventoryTraking oIT)
        {
            List<InventoryTraking> oITs_Category = new List<InventoryTraking>();
            if (oIT.ProductName == null)
            {
                oIT.ProductName = "";
            }
            try
            {
                _oInventoryTrakings = InventoryTraking.Gets_ProductWise(oIT.ProductName, oIT.BUID, oIT.StartDate, oIT.EndDate.AddDays(1), (int)Session[SessionInfo.currentUserID]);
                oITs_Category = _oInventoryTrakings.GroupBy(x => new { x.PCategoryID, x.PCategoryName }, (key, grp) =>
                                    new InventoryTraking
                                    {
                                        PCategoryID = key.PCategoryID,
                                        ProductID = 0,
                                        ProductName = "Category",
                                        PCategoryName = key.PCategoryName,
                                        InGRN =0,
                                        InAdj =0,
                                        InRS =0,
                                        InTr =0,
                                        InTrSW =0,
                                        InRet=0,
                                        OutAdj =0,
                                        OutRS =0,
                                        OutTr =0,
                                        OutRet =0,
                                        OutCon =0,
                                        OutTrSW=0,
                                        OutDC =0,
                                    }).ToList();

                oITs_Category.ForEach(x => _oInventoryTrakings.Add(x));
                _oInventoryTrakings = _oInventoryTrakings.OrderBy(o => o.PCategoryName).ThenBy(a => a.ProductID).ToList();


            }
            catch (Exception ex)
            {
                oIT.ErrorMessage = ex.Message;
                _oInventoryTrakings = new List<InventoryTraking>();
            }

            var jsonResult = Json(_oInventoryTrakings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult PrintPriviewProductWise(string sTempString)
        {
            InventoryTraking oIT = new InventoryTraking();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string _sDateRange = "";
            if (!string.IsNullOrEmpty(sTempString))
            {
                oIT.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                oIT.ProductName = sTempString.Split('~')[1];
                oIT.DateType = Convert.ToInt32(sTempString.Split('~')[2]);
                oIT.StartDate = Convert.ToDateTime(sTempString.Split('~')[3]);
                oIT.EndDate = Convert.ToDateTime(sTempString.Split('~')[4]);
                oBusinessUnit = oBusinessUnit.Get(oIT.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oInventoryTrakings = InventoryTraking.Gets_ProductWise(oIT.ProductName, oIT.BUID, oIT.StartDate, oIT.EndDate.AddDays(1), (int)Session[SessionInfo.currentUserID]);
                _sDateRange = "Date: " + oIT.StartDate.ToString("dd MMM yyyy") + " to " + oIT.EndDate.ToString("dd MMM yyyy");
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptInventoryTraking oReport = new rptInventoryTraking();
            byte[] abytes = null;
            abytes = oReport.PrepareReportProductWise(_oInventoryTrakings, oCompany, oBusinessUnit,"", _sDateRange);
            return File(abytes, "application/pdf");


        }

        public ActionResult PrintPriviewCurrentStock(string sTempString)
        {
            InventoryTraking oIT = new InventoryTraking();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            List<Lot> oLots = new List<Lot>();

            string _sDateRange = "";
            if (!string.IsNullOrEmpty(sTempString))
            {
                oIT.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                oIT.ProductName = sTempString.Split('~')[1];
                oIT.DateType = Convert.ToInt32(sTempString.Split('~')[2]);
               
                oBusinessUnit = oBusinessUnit.Get(oIT.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _sDateRange = "Date: " + oIT.StartDate.ToString("dd MMM yyyy") + " to " + oIT.EndDate.ToString("dd MMM yyyy");

                string sSQL = "Select * from View_Lot Where Balance>0.4";

                if (oIT.BUID>0)
                    sSQL = sSQL + " And BUID="+ oIT.BUID;
                if (!string.IsNullOrEmpty(oIT.ProductName))
                {
                    sSQL = sSQL + " And ProductID in (" + oIT.ProductName + ")";
                }
                else
                {
                    sSQL = sSQL + " And ProductID in (Select ProductID from Product where ProductCategoryID IN (SELECT ProductCategoryID FROM [dbo].[fn_GetProductCategory](2)))";
                }

                if (!string.IsNullOrEmpty(oIT.OPUName))
                    sSQL = sSQL + " And WorkingUnitID in (" + oIT.OPUName+")";

                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptInventoryTraking oReport = new rptInventoryTraking();
            byte[] abytes = null;
            abytes = oReport.PrepareReportCurrentStock(oLots, oCompany, oBusinessUnit, "Current Stock", _sDateRange);
            return File(abytes, "application/pdf");


        }

        #endregion

    }
}
