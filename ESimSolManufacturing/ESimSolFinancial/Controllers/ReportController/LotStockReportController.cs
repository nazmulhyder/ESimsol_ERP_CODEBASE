using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ESimSolFinancial.Models;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using System.Data;


namespace ESimSolFinancial.Controllers.ReportController
{
    public class LotStockReportController : Controller
    {
        #region Declaration
        LotStockReport _oLotStockReport = new LotStockReport();
        List<LotStockReport> _oLotStockReports = new List<LotStockReport>();
        #endregion

        #region Actions
        public ActionResult View_LotStockReports(int buid, int menuid)
        {
            ViewBag.BUID = buid;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oLotStockReports = new List<LotStockReport>();
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oLotStockReports);
        }
        public ActionResult View_LotsToAssign(int buid, int menuid)
        {
            ViewBag.BUID = buid;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oLotStockReports = new List<LotStockReport>();
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oLotStockReports);
        }
        public ActionResult View_LotToAssignDU(int id)
        {
            Lot oLot = new Lot();
            FabricLotAssign oFabricLotAssign = new FabricLotAssign();

            oLot = oLot.Get(id, (int)Session[SessionInfo.currentUserID]);
            oFabricLotAssign.FabricLotAssigns = FabricLotAssign.Gets("SELECT * FROM View_FabricLotAssign WHERE LotID=" + id, (int)Session[SessionInfo.currentUserID]);

            oFabricLotAssign.LotID = oLot.LotID;
            oFabricLotAssign.LotNo = oLot.LotNo;
            oFabricLotAssign.ProductName = oLot.ProductName;
            oFabricLotAssign.ProductCode = oLot.ProductCode;
            oFabricLotAssign.Qty = oLot.Balance;
            oFabricLotAssign.Qty_Order = oFabricLotAssign.FabricLotAssigns.Sum(x => x.Qty);
            oFabricLotAssign.BuyerName = oLot.ContractorName;
            //oFabricLotAssign.ParentID = oLot.ParentID;
            //oFabricLotAssign.ParentType = (int)oLot.ParentType;
            //oFabricLotAssign.UnitPrice = oLot.UnitPrice;
            //oFabricLotAssign.WorkingUnitID = oLot.WorkingUnitID;
            oFabricLotAssign.OperationUnitName = oLot.OperationUnitName;

            return View(oFabricLotAssign);
        }
        public ActionResult View_LotStockReportDetails(int id)
        {
            List<InventoryTransaction> _oInventoryTransactions = new List<InventoryTransaction>();
            _oInventoryTransactions = InventoryTransaction.GetsStockByLotID(id, (int)Session[SessionInfo.currentUserID]);
            return View(_oInventoryTransactions);
        }

        [HttpPost]
        public JsonResult GetsIssueStore(LotStockReport oLotStockReport)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            try
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, (int)Session[SessionInfo.currentUserID]);
                //oIssueStores = WorkingUnit.GetsPermittedStore(oLotStockReport.BUID, EnumModuleName.LotStockReport, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                oIssueStores = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
                string sSQL = "Select * from View_WorkingUnit Where IsActive=1 And IsStore=1 And BUID=" + oBusinessUnit.BusinessUnitID;
                oIssueStores = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                WorkingUnit oIssueStore = new WorkingUnit();
                oIssueStore.ErrorMessage = ex.Message;
                oIssueStores.Add(oIssueStore);
            }
            var jsonResult = Json(oIssueStores, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsIssueStore_Permission(LotStockReport oLotStockReport)
        {
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            try
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, (int)Session[SessionInfo.currentUserID]);
                //oIssueStores = WorkingUnit.GetsPermittedStore(oLotStockReport.BUID, EnumModuleName.LotStockReport, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
             //   oIssueStores = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
                //string sSQL = "Select * from View_WorkingUnit Where IsActive=1 And IsStore=1 And BUID=" + oBusinessUnit.BusinessUnitID;
                //oIssueStores = WorkingUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oIssueStores = WorkingUnit.GetsPermittedStore(oLotStockReport.BUID, EnumModuleName.DyeingOrder, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                WorkingUnit oIssueStore = new WorkingUnit();
                oIssueStore.ErrorMessage = ex.Message;
                oIssueStores.Add(oIssueStore);
            }
            var jsonResult = Json(oIssueStores, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsLotStockReport(LotStockReport oLotStockReport)
        {
            _oLotStockReports = new List<LotStockReport>();
            try
            {
                _oLotStockReports = LotStockReport.GetsRPTLot(oLotStockReport, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLotStockReport = new LotStockReport();
                _oLotStockReport.ErrorMessage = ex.Message;
                _oLotStockReports.Add(_oLotStockReport);
            }

            //--------------DetailView To------ ProductView------
            List<LotStockReport> oLotStockReportProduct = new List<LotStockReport>();
            oLotStockReportProduct = _oLotStockReports.GroupBy(x => new {x.OperationUnitName , x.ProductID}).Select(grp => new LotStockReport
                {
                    ProductID = grp.Key.ProductID,
                    OperationUnitName = grp.Key.OperationUnitName,
                    ProductName = grp.First().ProductName,
                    CategoryName = grp.First().CategoryName,
                    ProductName_Base = grp.First().ProductName_Base,
                    ProductCode = grp.First().ProductCode,
                    Qty_Total = grp.Sum(x => x.Qty_Total),
                    Balance = grp.Sum(x => x.Balance)
                }).ToList();

            var tuple = new Tuple<List<LotStockReport>, List<LotStockReport>>(new List<LotStockReport>(), new List<LotStockReport>());
            tuple = new Tuple<List<LotStockReport>, List<LotStockReport>>(_oLotStockReports, oLotStockReportProduct);

            var jsonResult = Json(tuple, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult AdvSearch_Assign(LotStockReport oLotStockReport)
        {
            _oLotStockReports = new List<LotStockReport>();
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            try
            {
                oIssueStores = WorkingUnit.GetsPermittedStore(oLotStockReport.BUID, EnumModuleName.DyeingOrder, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
                oLotStockReport.SearchingCriteria = string.Join(",",oIssueStores.Select(x=>x.WorkingUnitID));
                _oLotStockReports = LotStockReport.GetsAll_RPTLot(oLotStockReport, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLotStockReport = new LotStockReport();
                _oLotStockReport.ErrorMessage = ex.Message;
                _oLotStockReports.Add(_oLotStockReport);
            }

            //--------------DetailView To------ ProductView------
            List<LotStockReport> oLotStockReportProduct = new List<LotStockReport>();
            oLotStockReportProduct = _oLotStockReports.GroupBy(x => new {x.OperationUnitName , x.ProductID}).Select(grp => new LotStockReport
                {
                    ProductID = grp.Key.ProductID,
                    OperationUnitName = grp.Key.OperationUnitName,
                    ProductName = grp.First().ProductName,
                    CategoryName = grp.First().CategoryName,
                    ProductName_Base = grp.First().ProductName_Base,
                    ProductCode = grp.First().ProductCode,
                    Qty_Total = grp.Sum(x => x.Qty_Total),
                    Balance = grp.Sum(x => x.Balance)
                }).ToList();

            var tuple = new Tuple<List<LotStockReport>, List<LotStockReport>>(new List<LotStockReport>(), new List<LotStockReport>());
            tuple = new Tuple<List<LotStockReport>, List<LotStockReport>>(_oLotStockReports, oLotStockReportProduct);

            var jsonResult = Json(tuple, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
       
        #region Lot Assign Function

        [HttpPost]
        public JsonResult Save_LotParent(LotParent oLotParent)
        {
            try
            {
                oLotParent = oLotParent.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLotParent = new LotParent();
                oLotParent.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotParent);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete_LotParent(LotParent oLotParent)
        {
            try
            {
                if (oLotParent.LotParentID <= 0) { throw new Exception("Please select an valid item."); }
                oLotParent.ErrorMessage = oLotParent.Delete(oLotParent.LotParentID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLotParent = new LotParent();
                oLotParent.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLotParent.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsAll_LotStockReport(LotStockReport oLotStockReport)
        {
            _oLotStockReports = new List<LotStockReport>();
            try
            {
                _oLotStockReports = LotStockReport.GetsAll_RPTLot(oLotStockReport, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLotStockReport = new LotStockReport();
                _oLotStockReport.ErrorMessage = ex.Message;
                _oLotStockReports.Add(_oLotStockReport);
            }

            //--------------DetailView To------ ProductView------
            List<LotStockReport> oLotStockReportProduct = new List<LotStockReport>();
            oLotStockReportProduct = _oLotStockReports.GroupBy(x => new { x.OperationUnitName, x.ProductID }).Select(grp => new LotStockReport
            {
                ProductID = grp.Key.ProductID,
                OperationUnitName = grp.Key.OperationUnitName,
                ProductName = grp.First().ProductName,
                CategoryName = grp.First().CategoryName,
                ProductName_Base = grp.First().ProductName_Base,
                ProductCode = grp.First().ProductCode,
                Qty_Total = grp.Sum(x => x.Qty_Total),
                Balance = grp.Sum(x => x.Balance)
            }).ToList();

            var tuple = new Tuple<List<LotStockReport>, List<LotStockReport>>(new List<LotStockReport>(), new List<LotStockReport>());
            tuple = new Tuple<List<LotStockReport>, List<LotStockReport>>(_oLotStockReports, oLotStockReportProduct);

            var jsonResult = Json(tuple, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetsDyeingOrderDetails_OFF(LotParent oLotParent)
        {
            Lot oLot = new Lot();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<LotParent> oLotParents = new List<LotParent>();
            try
            {
                oLot = oLot.Get(oLotParent.LotID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oDyeingOrderDetails = DyeingOrderDetail.Gets(
                    "SELECT top(50)* FROM View_DyeingOrderDetail WHERE ProductID>0"
                    + (string.IsNullOrEmpty(oLotParent.DyeingOrderNo) ? "" : " AND OrderNo Like '%"+oLotParent.DyeingOrderNo+"%' ") 
                    // + " AND ContractorID=" + oLot.ContractorID
                    + " AND DyeingOrderID Not IN (SELECT DyeingOrderID FROM LoTParent) AND  DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE ISNULL(IsInHouse,0)=1)  ORDER BY OrderDate", 
                    ((User)Session[SessionInfo.CurrentUser]).UserID);

                oLotParents = oDyeingOrderDetails.GroupBy(x => new { x.DyeingOrderID, x.OrderNo, x.ProductID, x.ProductName}, (key, grp) => new LotParent
                {
                    ProductID = key.ProductID,
                    ProductName = key.ProductName,
                    DyeingOrderID = key.DyeingOrderID,
                    DyeingOrderNo = key.OrderNo,
                    OrderDateInString = grp.Select(x=>x.OrderDate).FirstOrDefault(),
                    Qty = grp.Sum(x => x.Qty), 
                    UnitPrice = grp.Max(x => x.UnitPrice)
                }).ToList();
            }
            catch (Exception e)
            {
                oLotParents.Add(new LotParent() { ErrorMessage = e.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oLotParents);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetsDyeingOrderDetails(FabricLotAssign oFabricLotAssign)
        {
            Lot oLot = new Lot();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            try
            {
                oLot = oLot.Get(oFabricLotAssign.LotID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
                oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets("SELECT TOP (100)* FROM View_DyeingOrderFabricDetail WHERE (ISNULL(Qty,0)-ISNULL(Qty_Assign,0))>0 "
                                                     + (string.IsNullOrEmpty(oFabricLotAssign.ExeNo) ? "" : " AND ExeNo Like '%" + oFabricLotAssign.ExeNo + "%' ")
                                                    +" ORDER BY FSCDetailID, SLNo", (int)Session[SessionInfo.currentUserID]);

                oFabricLotAssigns = oDyeingOrderFabricDetails.Where(x=>x.Qty_YetToAssign>0).GroupBy(x => new { x.FEOSDID, x.ExeNo, x.ProductID, x.ProductName,x.BuyerName,x.CustomerName }, (key, grp) => new FabricLotAssign
                {
                    ProductID = key.ProductID,
                    ProductName = key.ProductName,
                    BuyerName = key.BuyerName,
                    CustomerName = key.CustomerName,
                    FEOSDID = key.FEOSDID,
                    ExeNo = key.ExeNo,
                    Qty = grp.Sum(x => x.Qty),
                    Qty_Order = grp.Sum(x => x.Qty),
                    Balance = grp.Sum(x => x.Qty),
                    Qty_Assign = grp.Sum(x => x.Qty_Assign),
                    ColorName = grp.Select(x=>x.ColorName).FirstOrDefault(),
                    WarpWeftType= grp.Select(x=>x.WarpWeftType).FirstOrDefault(),
                }).ToList();
            }
            catch (Exception e)
            {
                oFabricLotAssigns.Add(new FabricLotAssign() { ErrorMessage = e.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oFabricLotAssigns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Print
        public ActionResult PrintLotStockReport(String sTemp)
        {
            List<LotStockReport> oLotStockReportList = new List<LotStockReport>();
            LotStockReport oLotStockReport = new LotStockReport();
            int nLayout = 0,nTab=0;
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                oLotStockReport.BUID = Convert.ToInt32(sTemp.Split('~')[0]);
                oLotStockReport.SearchingCriteria = Convert.ToString(sTemp.Split('~')[1]);
                nLayout = Convert.ToInt32(sTemp.Split('~')[2]);
                nTab = Convert.ToInt32(sTemp.Split('~')[3]);

                _oLotStockReports = LotStockReport.GetsRPTLot(oLotStockReport, (int)Session[SessionInfo.currentUserID]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oLotStockReport.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit WHERE WorkingUnitID IN (" + oLotStockReport.SearchingCriteria + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sHeader = "Stock Report For " + oWorkingUnits[0].OperationUnitName;
            for (int i = 1; i < oWorkingUnits.Count; i++)
                sHeader += ", " + oWorkingUnits[i].OperationUnitName;

            if (nTab == 2) 
            {
                //--------------DetailView To------ ProductView------
                List<LotStockReport> oLotStockReportProductList = new List<LotStockReport>();
                oLotStockReportProductList = _oLotStockReports.GroupBy(x => new { x.OperationUnitName, x.ProductID }).Select(grp => new LotStockReport
                {
                    ProductID = grp.Key.ProductID,
                    ProductBaseID = grp.First().ProductBaseID,
                    ProductCategoryID = grp.First().ProductCategoryID,
                    OperationUnitName = grp.Key.OperationUnitName,
                    ProductName = grp.First().ProductName,
                    CategoryName = grp.First().CategoryName,
                    ProductName_Base = grp.First().ProductName_Base,
                    ProductCode = grp.First().ProductCode,
                    Qty_Total = grp.Sum(x => x.Qty_Total),
                    Balance = grp.Sum(x => x.Balance)
                }).ToList();
                _oLotStockReports = oLotStockReportProductList;
            }

            rptLotStockReport oReport = new rptLotStockReport();
            byte[] abytes = oReport.PrepareReport(_oLotStockReports, oCompany, oBusinessUnit,nTab, nLayout,sHeader);
            return File(abytes, "application/pdf");
        }
        public void PrintToExcel(string sTempString)
        {
            List<LotStockReport> oLotStockReportList = new List<LotStockReport>();
            LotStockReport oLotStockReport = new LotStockReport();
            int nLayout = 0, nTab=0;
            if (string.IsNullOrEmpty(sTempString))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {
                oLotStockReport.BUID = Convert.ToInt32(sTempString.Split('~')[0]);
                oLotStockReport.SearchingCriteria = Convert.ToString(sTempString.Split('~')[1]);
                nLayout = Convert.ToInt32(sTempString.Split('~')[2]);
                nTab = Convert.ToInt32(sTempString.Split('~')[3]);
                _oLotStockReports = LotStockReport.GetsRPTLot(oLotStockReport, (int)Session[SessionInfo.currentUserID]);

                if (nTab == 2)
                {
                    //--------------DetailView To------ ProductView------
                    List<LotStockReport> oLotStockReportProductList = new List<LotStockReport>();
                    oLotStockReportProductList = _oLotStockReports.GroupBy(x => new { x.OperationUnitName, x.ProductID }).Select(grp => new LotStockReport
                    {
                        ProductID = grp.Key.ProductID,
                        ProductBaseID = grp.First().ProductBaseID,
                        ProductCategoryID = grp.First().ProductCategoryID,
                        OperationUnitName = grp.Key.OperationUnitName,
                        ProductName = grp.First().ProductName,
                        CategoryName = grp.First().CategoryName,
                        ProductName_Base = grp.First().ProductName_Base,
                        ProductCode = grp.First().ProductCode,
                        Qty_Total = grp.Sum(x => x.Qty_Total),
                        Balance = grp.Sum(x => x.Balance)
                    }).ToList();
                    _oLotStockReports = oLotStockReportProductList;
                }
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oLotStockReport.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnits = WorkingUnit.Gets("SELECT * FROM View_WorkingUnit WHERE WorkingUnitID IN (" + oLotStockReport.SearchingCriteria + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sHeader = "Stock Report For " + oWorkingUnits[0].OperationUnitName;
            for (int i = 1; i < oWorkingUnits.Count; i++)
                sHeader += ", " + oWorkingUnits[i].OperationUnitName;

            #region Print XL

            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 12, nColumn = 1, nCount = 0, nImportLCID = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LotStockReport");
                sheet.Name = sHeader;

                sheet.Column(++nColumn).Width = 10; //SL No
                sheet.Column(++nColumn).Width = 20; //category
                sheet.Column(++nColumn).Width = 20; //base
                sheet.Column(++nColumn).Width = 40; //P

                if (nTab == 1)
                {
                    sheet.Column(++nColumn).Width = 15; //lc
                    sheet.Column(++nColumn).Width = 15; //lot
                    sheet.Column(++nColumn).Width = 15; //inv
                }
                sheet.Column(++nColumn).Width = 15; //tqty
                sheet.Column(++nColumn).Width = 25; //blnc

                if (nTab == 1)
                sheet.Column(++nColumn).Width = 15; //contractor

                sheet.Column(++nColumn).Width = 15; //store

                if (nTab == 2)
                    nEndCol = 8;
                //nCol = 12;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.Font.UnderLine = false;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sHeader; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true; cell.Style.Font.UnderLine = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Report Data

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Header
                nColumn = 1;
                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Category Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Base Product"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Product Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (nTab == 1)
                {
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "L/C No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Lot No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Invoice No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Total Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Issue Qty"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Balance"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (nTab == 1)
                {
                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Supllier"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = "Store"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex++;
                #endregion

                #region Data

                foreach (LotStockReport oItem in _oLotStockReports)
                {
                    nColumn = 1;
                    nCount++;
                    //int nRowSpan = oLotStockReports.Where(PIR => PIR.ImportLCID == oItem.ImportLCID).ToList().Count - 1;
                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = nCount; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.CategoryName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName_Base; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (nTab == 1)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.LCNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.LotNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = oItem.InvoiceNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    cell = sheet.Cells[nRowIndex, ++nColumn, nRowIndex, nColumn]; cell.Value = "" + oItem.Qty_Total; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "# #,##0.00";
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Qty_Total-oItem.Balance; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.Balance; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "# #,##0.00";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (nTab == 1)
                    {
                        cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    cell = sheet.Cells[nRowIndex, ++nColumn]; cell.Value = oItem.OperationUnitName; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nEndRow = nRowIndex;
                    nRowIndex++;
                }
                #endregion

                #region Total
                int nDeduction = 4;
                if (nTab == 2)
                    nDeduction = 3;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - nDeduction--]; cell.Value = "" + "Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
    
                double nValue = _oLotStockReports.Select(c => c.Qty_Total).Sum();
                cell = sheet.Cells[nRowIndex, nEndCol - nDeduction--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "# #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oLotStockReports.Select(c => (c.Qty_Total-c.Balance)).Sum();
                cell = sheet.Cells[nRowIndex, nEndCol - nDeduction--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "# #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nValue = _oLotStockReports.Select(c => c.Balance).Sum();
                cell = sheet.Cells[nRowIndex, nEndCol - nDeduction--]; cell.Value = nValue; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "# #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - nDeduction--]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "$ #,##0.00";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                if (nTab == 1)
                {
                    cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "$ #,##0.00";
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
                if (nTab == 1)
                    Response.AddHeader("content-disposition", "attachment; filename=LotStockReport_Details.xlsx");
                else
                    Response.AddHeader("content-disposition", "attachment; filename=LotStockReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Order Wise Lot Report
        public ActionResult View_OrderWiseLotReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<FabricLotAssign> oLotParents = new List<FabricLotAssign>();

            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(oLotParents);
        }

        [HttpPost]
        public JsonResult AdvSearch(FabricLotAssign oFabricLotAssign)
        {
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            string sSQL = GetSQL(oFabricLotAssign);
            if (sSQL == "Error")
            {
                oFabricLotAssign = new FabricLotAssign();
                oFabricLotAssign.ErrorMessage = "Please select a searching critaria.";
                oFabricLotAssigns = new List<FabricLotAssign>();
            }
            else
            {
                oFabricLotAssigns = new List<FabricLotAssign>();
                oFabricLotAssigns = FabricLotAssign.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFabricLotAssigns.Count == 0)
                {
                    oFabricLotAssigns = new List<FabricLotAssign>();
                }
            }
            var jsonResult = Json(oFabricLotAssigns, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string GetSQL(FabricLotAssign oFabricLotAssign)
        {
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = oRouteSheetSetup.Get(1, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            string sReturn1 = "SELECT * FROM View_FabricLotAssign";
            string sReturn = "";

            #region Order Date 
            //string sSQL_OrderDate = "";
            //bool isValid= DateObject.CompareDateQuery(ref sSQL_OrderDate, "OrderDate", oFabricLotAssign.SearchByOrderDate);
            //if (isValid) 
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder " + sSQL_OrderDate + ")";
            //}
            #endregion

            DateObject.CompareDateQuery(ref sReturn, "DBServerDateTime", oFabricLotAssign.SearchByAssingDate);

            #region Buyer
            if (!string.IsNullOrEmpty(oFabricLotAssign.BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN ( " + oFabricLotAssign.BuyerName + ")";
            }
            #endregion

            #region DyeingOrderNo
            if (!string.IsNullOrEmpty(oFabricLotAssign.ExeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExeNo LIKE '%" + oFabricLotAssign.ExeNo + "%'";
            }
            #endregion

            #region LotNo
            if (!string.IsNullOrEmpty(oFabricLotAssign.LotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo LIKE '%" + oFabricLotAssign.LotNo + "%'";
            }
            #endregion
            
            sReturn = sReturn1 + sReturn+" ORDER BY LotID, LotNo";
            return sReturn;
        }

        #region Print Order Wise Lot
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(FabricLotAssign oFabricLotAssign)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricLotAssign);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Print_OrderWiseLot(int buid)
        {
            FabricLotAssign oFabricLotAssign = new FabricLotAssign();
            oFabricLotAssign = (FabricLotAssign)Session[SessionInfo.ParamObj];
            string sSQL = this.GetSQL(oFabricLotAssign);
            rptErrorMessage oReport = new rptErrorMessage();

            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            oFabricLotAssigns = FabricLotAssign.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
            string sHeader = "Order Wise Lot Report";

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (oFabricLotAssigns.Count == 0)
            {
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();
                SelectedField oSelectedField = new SelectedField("SLNo", "SL No", 18, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("FabricLotAssignDateSt", "Assign Date", 75, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("DyeingOrderNo", "Order No", 45, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ContractorName", "Buyer Name", 90, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ProductName", "Yarn Type", 110, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("OperationUnitName", "Color", 75, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
                //oSelectedField = new SelectedField("LotNo", "Lot No", 50, SelectedField.FieldType.Data); oSelectedFields.Add(oSelectedField);
               
                oSelectedField = new SelectedField("Qty_Order", "Order Qty", 36, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("WarpWeftTypeSt", "Warp/Weft", 35, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Balance", "Assign Qty", 43, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Soft", "Req. Qty", 36, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Batch_Out", "Batch Qty", 41, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty", "Balance", 36, SelectedField.FieldType.Total); oSelectedFields.Add(oSelectedField);

                rptFabricLotAssignReport oReport_Lot = new rptFabricLotAssignReport();
                byte[] abytes = oReport_Lot.PrepareReport(oFabricLotAssigns, oBusinessUnit, oCompany, sHeader, oSelectedFields);
                return File(abytes, "application/pdf");
            }
        }

        public void Print_OrderWiseLot_Excel(int buid)
        {
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            FabricLotAssign oFabricLotAssign = new FabricLotAssign();
            oFabricLotAssign = (FabricLotAssign)Session[SessionInfo.ParamObj];
            string sSQL = this.GetSQL(oFabricLotAssign);

            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            oFabricLotAssigns = FabricLotAssign.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFabricLotAssigns.Count > 0)
            {
                #region Header
                //M/C No.	Buyer	 Order No	Batch No	Yarn Type	Color	Batch Qty(KG)	Batch Qty(KG)	From Stock	Remarks
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Assign Date", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Order No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                //table_header.Add(new TableHeader { Header = "Order Date", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Product", Width = 40f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Color", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Store", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Lot No", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Balance", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Order Qty", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Assign Qty", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Soft Qty", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Batch Qty", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                //table_header.Add(new TableHeader { Header = "Consume Qty", Width = 20f, IsRotate = false, Align = TextAlign.Left });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Wise Lot Report");
                    sheet.Name = "Order Wise Lot Report";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    nEndCol = 16;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Order Wise Lot Report"; cell.Style.Font.Bold = true;
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

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    int nCount = 1; nEndCol = table_header.Count() + nStartCol;

                    foreach (var obj in oFabricLotAssigns)
                    {
                        nCount++;
                        nStartCol = 2;
                       
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, nCount.ToString(), false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FabricLotAssignDateSt, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExeNo, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BuyerName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ColorName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OperationUnitName, false);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LotNo, false);

                        ExcelTool.Formatter = " ##,##,##0.00;(##,##0.00)";
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_Order.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Balance.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_Req.ToString(), true);
                        ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_RS.ToString(), true);
                        //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty_Consume.ToString(), true);
                        nRowIndex++;
                    }
                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, " Total:", nRowIndex, nRowIndex, nStartCol, 9, true, ExcelHorizontalAlignment.Right, true);
                    nStartCol = 10;
                    ExcelTool.FillCellMerge(ref sheet, oFabricLotAssigns.Sum(x => x.Qty).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, oFabricLotAssigns.Sum(x => x.Qty_Order).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, oFabricLotAssigns.Sum(x => x.Balance).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, oFabricLotAssigns.Sum(x => x.Qty_Req).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellMerge(ref sheet, oFabricLotAssigns.Sum(x => x.Qty_RS).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right, true);
                    //ExcelTool.FillCellMerge(ref sheet, oFabricLotAssigns.Sum(x => x.Qty_Consume).ToString(), nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right, true);
                
                    nRowIndex++;
                    #endregion

                    nRowIndex++;

                    #endregion

                    nStartCol = 2;
                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Daily-Yarn-Dyeing-Production-Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
         
        #endregion

        #endregion
        public ActionResult PrintOrderWise(string sParam, string sLCNo, int nBUID)
        {
            string sWUIDs = "";
            Lot oLot = new Lot();
            List<Lot> oLots = new List<Lot>();
            FabricLotAssign oFabricLotAssign = new FabricLotAssign();
            List<ITransaction> oITransactions=new List<ITransaction>();
            List<ITransaction> oITransactionsDes = new List<ITransaction>();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();

            oWorkingUnits = WorkingUnit.GetsPermittedStore(nBUID, EnumModuleName.DyeingOrder, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);

            sWUIDs = string.Join(",", oWorkingUnits.Select(x => x.WorkingUnitID).Distinct());

            if (String.IsNullOrEmpty(sWUIDs)) { sWUIDs = "0"; }

            oLots = Lot.Gets("SELECT * FROM View_Lot WHERE workingunitID in (" + sWUIDs + ") and LotID IN (" + sParam + ")  ", (int)Session[SessionInfo.currentUserID]);
            //oLots = Lot.Gets("SELECT * FROM View_Lot WHERE LotID IN (" + sParam + ") or ParentLotID in (" + sParam + ")", (int)Session[SessionInfo.currentUserID]);
            oITransactions = ITransaction.Gets("SELECT * FROM View_ITransaction WHERE TriggerParentType in (" + (int)EnumTriggerParentsType.AdjustmentDetail + "," + (int)EnumTriggerParentsType.GRNDetailDetail + " ) and workingunitID in (" + sWUIDs + ") and  LotID IN (" + string.Join(",", oLots.Select(x => x.LotID)) + ") AND InOutType IN (" + (int)EnumInOutType.Receive + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            oITransactionsDes = ITransaction.Gets("SELECT * FROM View_ITransaction WHERE  workingunitID in (" + sWUIDs + ") and TriggerParentType=" + (int)EnumTriggerParentsType.AdjustmentDetail + " and LotID IN (" + string.Join(",", oLots.Select(x => x.LotID)) + ") AND InOutType IN (" + (int)EnumInOutType.Disburse + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (sLCNo.Split('~').Count() > 0)
                sLCNo = string.Join(", " , sLCNo.Split('~').ToList().Distinct());

            //oITransactions=ITransaction.Gets(id, (int)Session[SessionInfo.currentUserID])
            //oLotParent.LotID = oLot.LotID;

            oLot.LotNo = string.Join(",", oLots.Select(x => x.LotNo).Distinct());
            oLot.ProductName = string.Join(",", oLots.Select(x => x.ProductName).Distinct());
            oLot.Balance = oLots.Sum(x => x.Balance);
            oLot.ContractorName = string.Join(",", oLots.Where(x=> !string.IsNullOrEmpty(x.ContractorName)).Select(x => x.ContractorName).Distinct());

            oFabricLotAssign.LotNo = string.Join(",", oLots.Select(x => x.LotNo).Distinct());
            oFabricLotAssign.Qty = oITransactions.Sum(x => x.Qty);
            if (oITransactionsDes.Count > 0)
            {
                oFabricLotAssign.Qty = oFabricLotAssign.Qty - oITransactionsDes.Sum(x => x.Qty);
            }
            oFabricLotAssign.ProductName = string.Join(",", oLots.Select(x => x.ProductName).Distinct());
            oFabricLotAssign.Balance = oLots.Sum(x => x.Balance);
            oFabricLotAssign.BuyerName = string.Join(",", oLots.Select(x => x.ContractorName).Distinct());
            //oFabricLotAssign.WorkingUnitID = oLot.WorkingUnitID;

            oFabricLotAssign.FabricLotAssigns = FabricLotAssign.Gets("SELECT * FROM View_FabricLotAssign WHERE LotID IN (" + string.Join(",", oLots.Select(x => x.LotID)) + ")", (int)Session[SessionInfo.currentUserID]);
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oLots.Select(x => x.BUID).FirstOrDefault(), ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptLotTraking oReport = new rptLotTraking();
            byte[] abytes = oReport.PrepareLotWiseOrderReport(oLot, oFabricLotAssign, oCompany, oBusinessUnit, sLCNo);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintRawLot_Order_Req_RS(string sParam, int buid)
        {
            string sTemp = "";
            LotStockReport oLotStockReport = new LotStockReport();
            List<FabricLotAssign> oFabricLotAssigns = new List<FabricLotAssign>();
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
            List<FabricExecutionOrderYarnReceive> oFabricEOYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            oLotStockReport.Params = sParam;
            oLotStockReport.BUID = buid;
            _oLotStockReports = LotStockReport.GetsAll_RPTLot(oLotStockReport, (int)Session[SessionInfo.currentUserID]);
           // oLots = Lot.Gets("SELECT * FROM View_Lot WHERE LotID IN (" + sParam + ")  ", (int)Session[SessionInfo.currentUserID]);
            sTemp = string.Join(",", _oLotStockReports.Select(x => x.LotID));
            if (string.IsNullOrEmpty(sTemp)) { sTemp = "0"; }
            if (_oLotStockReports.Count > 0)
            {
                oFabricLotAssigns = FabricLotAssign.Gets("select   HH.OrderNo as ExeNo,sum(HH.Qty) as Qty,Sum(HH.Qty_Order) as Qty_Order,HH.ProductID,ProductName,LotID,HH.DyeingOrderID  from (select FEOS.Qty as Qty_Order, DO.OrderNo, FLA.Qty,FLA.ProductID,Product.ProductName,LotID,FEOS.DyeingOrderID  from fabricLotAssign as FLA LEFT JOIN  DyeingorderFabricDetail as FEOS ON FEOS.FEOSDID= FLA.FEOSDID   LEFT JOIN  dbo.DyeingOrder as DO ON DO.DyeingOrderID= FEOS.DyeingOrderID    LEFT JOIN  dbo.Product ON Product.ProductID= FLA.ProductID  where LotID in (" + sTemp + ")) as HH group by HH.OrderNo, HH.ProductID,ProductName,LotID,HH.DyeingOrderID", (int)Session[SessionInfo.currentUserID]);
                oDURequisitionDetails = DURequisitionDetail.Gets(" Select HH.ParentLotID, DestinationLotID,HH.ProductID,DyeingOrderID,Sum(HH.Qty) as Qty ,(Select sum(Qty) from DURequisitionDetail where productID=HH.ProductID and DyeingOrderID=HH.DyeingOrderID and LotID=HH.DestinationLotID and DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE isnull(DURequisition.ReceiveByID,0)<>0 and  RequisitionType=102))as LotQty from (Select case when Lot.ParentLotID<=0 then DUReqD.LotID else Lot.ParentLotID end as ParentLotID,DestinationLotID,DUReqD.ProductID,DyeingOrderID,Qty from DURequisitionDetail as DUReqD LEFT JOIN  dbo.Lot ON Lot.LotID= DUReqD.LotID LEFT JOIN  dbo.Product ON Product.ProductID= DUReqD.ProductID   where (DUReqD.LotID in (" + sTemp + ") or DUReqD.LotID in (Select LotID from Lot Where ParentLotID>0 and ParentLotID in (" + sTemp + ") )) and  DURequisitionID IN (SELECT DURequisitionID FROM DURequisition WHERE isnull(DURequisition.ReceiveByID,0)<>0 and  RequisitionType=101) ) as HH group by HH.ParentLotID, DestinationLotID,HH.ProductID,DyeingOrderID", (int)Session[SessionInfo.currentUserID]);

                oFabricEOYarnReceives = FabricExecutionOrderYarnReceive.Gets(" Select HH.ParentLotID, DestinationLotID,HH.ProductID,DyeingOrderID,Sum(HH.ReceiveQty) as ReceiveQty   ,(Select sum(ReceiveQty) from FabricExecutionOrderYarnReceive   where  FEOSDID=HH.DyeingOrderID and IssueLotID=HH.DestinationLotID and WYRequisitionID IN (SELECT WYRequisitionID FROM WYRequisition WHERE isnull(WYRequisition.ReceivedBy,0)<>0 and isnull(WYarnType,0)=2 and RequisitionType=102))as ReqQty   from ( Select case when Lot.ParentLotID<=0 then DUReqD.IssueLotID else Lot.ParentLotID end as ParentLotID,DestinationLotID,DOFD.ProductID,DOFD.DyeingOrderID,ReceiveQty  from FabricExecutionOrderYarnReceive as DUReqD LEFT JOIN  dbo.Lot as Lot ON Lot.LotID= DUReqD.IssueLotID LEFT JOIN  dbo.DyeingorderFabricDetail as DOFD ON DOFD.FEOSDID= DUReqD.FEOSDID LEFT JOIN  dbo.Product ON Product.ProductID=  Lot.ProductID where (DUReqD.IssueLotID in (" + sTemp + ") or DUReqD.IssueLotID in (Select LotID from Lot Where ParentLotID>0 and ParentLotID in (" + sTemp + ") )) and  WYRequisitionID IN (SELECT WYRequisitionID FROM WYRequisition WHERE isnull(WYarnType,0)=2 and isnull(WYRequisition.ReceivedBy,0)<>0 and  RequisitionType=101) )"
                 + "as HH group by HH.ParentLotID, DestinationLotID,ProductID,DyeingOrderID ", (int)Session[SessionInfo.currentUserID]);
   

                sTemp = string.Join(",", oDURequisitionDetails.Select(x => x.DestinationLotID));
                if (string.IsNullOrEmpty(sTemp)) { sTemp = "0"; }
                oRSRawLots = RSRawLot.Gets(" Select * ,(Select Top(1)RSDO.DyeingOrderID from View_RouteSheetDO as RSDO where RouteSheetID=RSR.RouteSheetID ) as DyeingOrderID  from View_RSRawLot as RSR where  LotID in (" + sTemp + ")", (int)Session[SessionInfo.currentUserID]);

             
            }
       
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptLotTraking oReport = new rptLotTraking();
            byte[] abytes = oReport.PrepareRawLot_Order_Req_RS(_oLotStockReports, oFabricLotAssigns, oDURequisitionDetails, oRSRawLots, oCompany, oBusinessUnit, oFabricEOYarnReceives);
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
        public void PrintExcelSingle(string sParam)
        {
            Lot oLot = new Lot();
            List<Lot> oLots = new List<Lot>();
            FabricLotAssign oFabricLotAssign = new FabricLotAssign();
            List<ITransaction> oITransactions = new List<ITransaction>();
            oLots = Lot.Gets("SELECT * FROM View_Lot WHERE LotID IN (" + sParam + ") OR LotID IN (SELECT Lot.ParentLotID FROM Lot WHERE Lot.LotID IN (" + sParam + "))", (int)Session[SessionInfo.currentUserID]);
            oITransactions = ITransaction.Gets("SELECT * FROM View_ITransaction WHERE LotID IN (" + string.Join(",", oLots.Select(x => x.LotID)) + ") AND InOutType IN (" + (int)EnumInOutType.Receive + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            //oITransactions=ITransaction.Gets(id, (int)Session[SessionInfo.currentUserID])
            //oFabricLotAssign.LotID = oLot.LotID;

            oLot.LotNo = string.Join(",", oLots.Select(x => x.LotNo).Distinct());
            oLot.ProductName = string.Join(",", oLots.Select(x => x.ProductName).Distinct());
            oLot.Balance = oLots.Sum(x => x.Balance);
            oLot.ContractorName = string.Join(",", oLots.Where(x => !string.IsNullOrEmpty(x.ContractorName)).Select(x => x.ContractorName).Distinct());

            oFabricLotAssign.LotNo = string.Join(",", oLots.Select(x => x.LotNo).Distinct());
            oFabricLotAssign.Qty = oITransactions.Sum(x => x.Qty);
            oFabricLotAssign.ProductName = string.Join(",", oLots.Select(x => x.ProductName).Distinct());
            oFabricLotAssign.Balance = oLots.Sum(x => x.Balance);
            oFabricLotAssign.BuyerName = string.Join(",", oLots.Select(x => x.BuyerName).Distinct());
            //oFabricLotAssign.ParentID = oLot.ParentID;
            //oFabricLotAssign.ParentType = (int)oLot.ParentType;
            //oFabricLotAssign.UnitPrice = oLot.UnitPrice;
            oFabricLotAssign.WorkingUnitID = oLot.WorkingUnitID;

            oFabricLotAssign.FabricLotAssigns = FabricLotAssign.Gets("SELECT * FROM View_FabricLotAssign WHERE LotID IN (" + string.Join(",", oLots.Select(x => x.LotID)) + ")", (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oLots.Select(x => x.BUID).FirstOrDefault(), ((User)Session[SessionInfo.CurrentUser]).UserID);

            //rptLotTraking oReport = new rptLotTraking();
            //byte[] abytes = oReport.PrepareLotWiseOrderReport(oLot, oFabricLotAssign, oCompany, oBusinessUnit);


            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = 5;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("RAW LOT FOLLOW UP");
                sheet.Name = "RAW LOT FOLLOW UP";

                sheet.DefaultColWidth = 40;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "RAW LOT FOLLOW UP"; cell.Style.Font.Bold = true;
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

                #region Data
                Lot oParentLot = oLots.Where(x => x.ParentLotID == 0).First();

                nRowIndex++;
                nStartCol = 2;

                #region first header
                nStartCol = 2;
                int SaveRowIndex = nRowIndex;
                ExcelTool.FillCellMerge(ref sheet, "RAW LOT FOLLOW UP", nRowIndex, nRowIndex, nStartCol, nEndCol, true, ExcelHorizontalAlignment.Center, false);
                nStartCol = 2;
                nRowIndex++;
                #endregion

                #region First Table
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "Raw Lot No", 1, false, 13, false, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, oFabricLotAssign.LotNo, 1, false, 13, false, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "Supplier Name", 1, false, 13, false, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, oFabricLotAssign.BuyerName, 1, false, 13, false, true, true, true, true);

                nStartCol = 2;
                nRowIndex++;
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "Yarn Type", 1, false, 13, false, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, oParentLot.ProductName, 1, false, 12, false, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "", 1, false, 12, false, false, false, false, false);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "", 1, false, 12, false, false, false, false, true);

                nStartCol = 2;
                nRowIndex++;
                #endregion

                #region Table header
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "TTL RCV QTY", 2, false, 13, true, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "ORDER USED IN", 2, false, 13, true, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "QTY", 2, false, 13, true, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "BALANCE QTY", 2, false, 13, true, true, true, true, true);
                nRowIndex++;
                nStartCol = 2;
                #endregion

                bool bflag = true;
                double TtlQty = 0;
                int nCount = 0;
                TtlQty = oFabricLotAssign.FabricLotAssigns.Sum(x => x.Qty);
                nCount = oFabricLotAssign.FabricLotAssigns.Count;


                foreach (FabricLotAssign oItem in oFabricLotAssign.FabricLotAssigns)
                {
                    if(bflag)
                    {
                        ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, oFabricLotAssign.Qty.ToString() + " kg", 2, false, 12, false, true, true, true, false);
                    }
                    else
                    {
                        ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "", 2, false, 12, false, true, false, false, false);
                    }
                    ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, oItem.ExeNo, 2, false, 12, false, true, true, true, true);
                    ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, Global.MillionFormat(oItem.Qty).ToString() + " ", 2, false, 12, false, true, true, true, true);
                    if(bflag)
                    {
                        ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, Global.MillionFormat(oFabricLotAssign.Qty - TtlQty).ToString() + " kg", 2, false, 12, false, true, true, true, false);
                    }
                    else
                    {
                        ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "", 2, false, 12, false, false, true, false, false);
                    }
                    bflag = false;
                    nRowIndex++;
                    nStartCol=2;
                }
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, "", 2, false, 12, false, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, " Total:", 1, false, 12, false, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, Global.MillionFormat(TtlQty).ToString() + " kg", 3, false, 12, false, true, true, true, true);
                ExcelTool.FillCellOne(sheet, nRowIndex, nStartCol++, " ", 2, false, 12, false, true, true, true, true);

                #endregion
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=RAW LOT FOLLOW UP.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        
        #region Stock Wise Lot Transaction Report
        public ActionResult View_StockWiseLotReport(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oLotStockReports = new List<LotStockReport>();
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

            return View(_oLotStockReports);
        }
        [HttpPost]
        public JsonResult GetTransactions(LotStockReport oLotStockReport)
        {
            _oLotStockReports = new List<LotStockReport>();

            string sTempString = oLotStockReport.Params;
            try
            {
                int nBUID = 0;
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                string sWorkingUnits = "";

                if (!string.IsNullOrEmpty(sTempString))
                {
                    nBUID = Convert.ToInt32(sTempString.Split('~')[0]);
                    startDate = Convert.ToDateTime(sTempString.Split('~')[1]);
                    endDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                    sWorkingUnits = sTempString.Split('~')[3];
                }

                _oLotStockReports = LotStockReport.Gets_StockWiseLotReport(startDate, endDate, sWorkingUnits, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oLotStockReport = new LotStockReport();
                _oLotStockReports = new List<LotStockReport>();
            }
            var jsonResult = Json(_oLotStockReports, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult Print_StockWiseLotReport(string sTempString)
        {
            string sErrorMessage = "";
            BusinessUnit oBusinessUnit = new BusinessUnit();
            Company oCompany = new Company();
            LotStockReport oLotStockReport = new LotStockReport();
            try
            {
                int nBUID = 0;
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                string sWorkingUnits = "";

                if (!string.IsNullOrEmpty(sTempString))
                {
                    nBUID = Convert.ToInt32(sTempString.Split('~')[0]);
                    startDate = Convert.ToDateTime(sTempString.Split('~')[1]);
                    endDate = Convert.ToDateTime(sTempString.Split('~')[2]);
                    sWorkingUnits = sTempString.Split('~')[3];
                }

                _oLotStockReports = LotStockReport.Gets_StockWiseLotReport(startDate, endDate, sWorkingUnits, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                oBusinessUnit = oBusinessUnit.Get(1, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMessage = ex.Message;
                _oLotStockReport = new LotStockReport();
                _oLotStockReports = new List<LotStockReport>();
            }

            if (_oLotStockReports.Count == 0)
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!! \n" + sErrorMessage);
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();

                SelectedField oSelectedField = new SelectedField("ReceiveDateSt", "Issue Date", 55, SelectedField.FieldType.Data, SelectedField.Alginment.CENTER); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("IssueDateSt", "Recevie Date", 55, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ProductName", "Product Name", 130, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                // oSelectedField = new SelectedField("OperationUnitName", "Store", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("LotNo", "Lot No", 47, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("MUnit", "MUnit", 37, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Opening", "Opening Qty", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Received", "Receive Qty", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Issue", "Issue Qty", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Qty_Balance", "Balance", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);

                rptDynamicReport oReport = new rptDynamicReport(595, 842);
                oReport.SpanTotal = 5;//ColSpanForTotal
                oReport.FirstColumn = 35;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReport(_oLotStockReports.Cast<object>().ToList(), oBusinessUnit, oCompany, "Store Wise Transaction Report ", oSelectedFields); //\n(" + oITransactions[0].ProductName + ", " + oITransactions[0].LotNo + ")"
                return File(abytes, "application/pdf");
            }
        }
        #endregion
    }
}