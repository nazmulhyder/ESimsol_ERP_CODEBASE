using System;
using System.Collections;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class InventoryTrackingWIPController : Controller
    {
        #region Declaration

        InventoryTrackingWIP _oInventoryTrackingWIP = new InventoryTrackingWIP();
        List<InventoryTrackingWIP> _oInventoryTrackingWIPs = new List<InventoryTrackingWIP>();
        #endregion

        #region Actions

        public ActionResult ViewInventoryTrackingWIP(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTrackingWIP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oInventoryTrackingWIPs = new List<InventoryTrackingWIP>();
            //_oInventoryTrackingWIPs = InventoryTrackingWIP.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oInventoryTrackingWIPs);
        }

        #endregion

        #region Get
        [HttpPost]
        public JsonResult Search(InventoryTrackingWIP oInventoryTrackingWIP)
        {
            _oInventoryTrackingWIP = new InventoryTrackingWIP();
            try
            {
                RouteSheetSetup oRSS = new RouteSheetSetup();
                oRSS = oRSS.Get(1, (int)Session[SessionInfo.currentUserID]);
                oInventoryTrackingWIP.EndDate = oInventoryTrackingWIP.EndDate.AddDays(1);
                _oInventoryTrackingWIP.MainList = InventoryTrackingWIP.Gets(oInventoryTrackingWIP, (int)Session[SessionInfo.currentUserID]);

                string Sql = "SELECT ProductID, WorkingUnitID, SUM(Qty) AS Qty, (SELECT StoreName FROM View_WorkingUnit WHERE WorkingUnitID = IT.WorkingUnitID) AS WorkingUnitName FROM ITransaction AS IT WHERE iT.TriggerParentType=106 and IT.InOutType=102 and IT.[DateTime]>='" + oInventoryTrackingWIP.StartDate.ToString("dd MMM yyyy HH:mm:ss") + "' AND IT.[DateTime]<'" + oInventoryTrackingWIP.EndDate.ToString("dd MMM yyyy HH:mm:ss") + "' AND IT.WorkingUnitID<>" + oRSS.WorkingUnitIDWIP + " GROUP BY WorkingUnitID, ProductID";
                _oInventoryTrackingWIP.IssueList = InventoryTrackingWIP.Gets(Sql, (int)Session[SessionInfo.currentUserID]);
                
                Sql = "select ProductID,Sum(GG.Qty) as Qty,Sum(GG.QtyPacking) as QtyPacking, sum(GG.Qty_RS) as Qty_RS, sum(isnull(GG.QtyRecycle,0)) as QtyRecycle ,sum(isnull(GG.QtyWastage,0)) as QtyWastage,sum(GG.Qty_RS-GG.QtyPacking-GG.QtyRecycle-GG.QtyWastage) as QtyShort from ( " +
                "select ProductID,HH.TriggerParentID, HH.Qty as Qty,HH.Qty_RS,HH.QtyPacking ,CASE WHEN isnull(HH.Qty_RS,0)>0 THEN isnull((HH.QtyRecycle),0) ELSE 0 END QtyRecycle ,CASE WHEN isnull(HH.Qty_RS,0)>0 THEN isnull((HH.QtyWastage),0) ELSE 0 END QtyWastage " +
                "from (select TriggerParentType,sum(Qty) as Qty,InOutType,WorkingUnitID,ProductID,TriggerParentID ,(Select Sum(RSD.Qty_RS) from RouteSheetDO as RSD where RSD.RSState=13 and RSD.RouteSheetID=IT.TriggerParentID ) as Qty_RS " +
                ",isnull((Select isnull(sum(Qty),0) from RSInQCDetail as RS where RS.ManagedLotID>0 and rs.RouteSheetID=IT.TriggerParentID and rs.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType not in (2,3))),0) as QtyPacking " +
                ",isnull((Select isnull(sum(Qty),0) from RSInQCDetail as RS where RS.RouteSheetID=IT.TriggerParentID and RS.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType in (2))),0) as QtyRecycle " +
                ",(Select isnull(sum(Qty),0) from RSInQCDetail as RS where rs.RouteSheetID=IT.TriggerParentID and rs.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType in (3))) as QtyWastage " +
                "from ITransaction as IT where  it.InOutType=102 and IT.WorkingUnitID in (select top 1 WorkingUnitIDWIP from RouteSheetSetup) and IT.[DateTime]>='" + oInventoryTrackingWIP.StartDate.ToString("dd MMM yyyy HH:mm:ss") + "'  and IT.[DateTime]<'" + oInventoryTrackingWIP.EndDate.ToString("dd MMM yyyy HH:mm:ss") + "' group by TriggerParentType,InOutType,WorkingUnitID,ProductID,TriggerParentID) as HH ) as GG group by GG.ProductID";
                _oInventoryTrackingWIP.ReceiveList = InventoryTrackingWIP.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                _oInventoryTrackingWIP.ITWIPsForIssue = _oInventoryTrackingWIP.IssueList.GroupBy(x => x.WorkingUnitID).Select(y => y.FirstOrDefault()).OrderBy(c => c.WorkingUnitID).ToList();
                //_oInventoryTrackingWIP.ITWIPsForReceive = _oInventoryTrackingWIP.ReceiveList.GroupBy(x => x.WorkingUnitID).Select(y => y.FirstOrDefault()).ToList();

            }
            catch (Exception ex)
            {
                _oInventoryTrackingWIP = new InventoryTrackingWIP();
                _oInventoryTrackingWIP.ErrorMessage = ex.Message;
            }
            var jsonResult = Json(_oInventoryTrackingWIP, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Pdf
        public ActionResult DetailPrint(int nProductID, DateTime dStartDate, DateTime dEndDate, double nOpeningQty, int nBUID)
        {
            _oInventoryTrackingWIPs = new List<InventoryTrackingWIP>();
            RouteSheetSetup oRSS = new RouteSheetSetup();
            Product oProduct = new Product();
            string sDateRangeMsg = "";
            if (nProductID > 0)
            {
                oRSS = oRSS.Get(1, (int)Session[SessionInfo.currentUserID]);
                oProduct = oProduct.Get(nProductID, (int)Session[SessionInfo.currentUserID]);
                string Sql = "SELECT GG.ProductID, GG.WorkingUnitID, GG.Qty, GG.TriggerParentID, GG.LotID, GG.MUnitID, GG.[DateTime] AS TransactionTime, 101 AS InOutType, WU.StoreName, Lot.LotNo, RS.RouteSheetNo AS RefNo, MeasurementUnit.Symbol AS USymbol  FROM ( "
                    +"SELECT ProductID, WorkingUnitID, SUM(Qty) AS Qty, TriggerParentID, LotID, MUnitID, [DateTime] "
                    + "FROM ITransaction AS IT WHERE iT.TriggerParentType=106 AND ProductID=" + nProductID + " and IT.InOutType=102 and IT.[DateTime]>='" + dStartDate.ToString("dd MMM yyyy HH:mm:ss") + "' AND IT.[DateTime]<'" + dEndDate.AddDays(1).ToString("dd MMM yyyy HH:mm:ss") + "' "
                    + "AND IT.WorkingUnitID<>" + oRSS.WorkingUnitIDWIP + " GROUP BY WorkingUnitID, ProductID, LotID, TriggerParentID, MUnitID, [DateTime] ) AS GG "
                    +"LEFT JOIN View_WorkingUnit AS WU ON GG.WorkingUnitID = WU.WorkingUnitID LEFT JOIN Lot ON Lot.LotID=GG.LotID "
                    +"LEFT JOIN RouteSheet AS RS ON RS.RouteSheetID=GG.TriggerParentID LEFT JOIN	MeasurementUnit ON GG.MUnitID = MeasurementUnit.MeasurementUnitID ";
                _oInventoryTrackingWIPs = InventoryTrackingWIP.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                List<InventoryTrackingWIP> oTempInventoryTrackingWIPs = new List<InventoryTrackingWIP>();
                Sql = "select GG.TriggerParentID, GG.ProductID, GG.Qty, GG.QtyPacking as QtyPacking, GG.Qty_RS as Qty_RS, GG.QtyRecycle as QtyRecycle ,GG.QtyWastage as QtyWastage,GG.Qty_RS-GG.QtyPacking-GG.QtyRecycle-GG.QtyWastage as QtyShort, GG.LotID, GG.MUnitID, GG.[DateTime] AS TransactionTime, GG.WorkingUnitID, WU.StoreName, Lot.LotNo, RS.RouteSheetNo AS RefNo, MeasurementUnit.Symbol AS USymbol, 102 AS InOutType from ( "
                    + "select ProductID,HH.TriggerParentID,LotID, MUnitID, [DateTime], WorkingUnitID, HH.Qty as Qty,HH.Qty_RS,HH.QtyPacking ,CASE WHEN isnull(HH.Qty_RS,0)>0 THEN isnull((HH.QtyRecycle),0) ELSE 0 END QtyRecycle ,CASE WHEN isnull(HH.Qty_RS,0)>0 THEN isnull((HH.QtyWastage),0) ELSE 0 END QtyWastage "
                    + "from ( select TriggerParentType,sum(Qty) as Qty,InOutType,WorkingUnitID,ProductID,TriggerParentID, LotID, MUnitID, MAX([DateTime]) as [DateTime] "
                    + ",(Select Sum(RSD.Qty_RS) from RouteSheetDO as RSD where RSD.RSState=13 and RSD.RouteSheetID=IT.TriggerParentID )as Qty_RS "
                    + ",(Select sum(Qty) from RSInQCDetail as RS where RS.ManagedLotID>0 and rs.RouteSheetID=IT.TriggerParentID and rs.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType not in (2,3))) as QtyPacking "
                    + ",(Select sum(Qty) from RSInQCDetail as RS where rs.RouteSheetID=IT.TriggerParentID and rs.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType in (2))) as QtyRecycle "
                    + ",(Select sum(Qty) from RSInQCDetail as RS where rs.RouteSheetID=IT.TriggerParentID and rs.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType in (3))) as QtyWastage "
                    + "from ITransaction as IT where  IT.ProductID=" + nProductID + " AND   it.InOutType=102 and IT.WorkingUnitID in (select WorkingUnitIDWIP from RouteSheetSetup) and IT.[DateTime]>='" + dStartDate.ToString("dd MMM yyyy HH:mm:ss") + "'  and IT.[DateTime]<'" + dEndDate.AddDays(1).ToString("dd MMM yyyy HH:mm:ss") + "' "
                    + "group by TriggerParentType,InOutType,WorkingUnitID,ProductID,TriggerParentID, LotID, MUnitID) as HH ) as GG "
                    + "LEFT JOIN View_WorkingUnit AS WU ON GG.WorkingUnitID = WU.WorkingUnitID LEFT JOIN Lot ON Lot.LotID=GG.LotID "
                    + "LEFT JOIN RouteSheet AS RS ON RS.RouteSheetID=GG.TriggerParentID LEFT JOIN	MeasurementUnit ON GG.MUnitID = MeasurementUnit.MeasurementUnitID ";
                oTempInventoryTrackingWIPs = InventoryTrackingWIP.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

                foreach (InventoryTrackingWIP oItem in oTempInventoryTrackingWIPs)
                {
                    _oInventoryTrackingWIPs.Add(oItem);
                }

            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            sDateRangeMsg = "Between " + dStartDate.ToString("dd MMM yyyy") + " To " + dEndDate.ToString("dd MMM yyyy");
            byte[] abytes;
            rptInventoryTrackingWIPDetail oReport = new rptInventoryTrackingWIPDetail();
            abytes = oReport.PrepareReport(_oInventoryTrackingWIP, _oInventoryTrackingWIPs, oProduct, oCompany, sDateRangeMsg, nOpeningQty);
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

        #region Excel
        public void ExportXL(int nBUID, DateTime dStartDate, DateTime dEndDate)
        {
            _oInventoryTrackingWIP = new InventoryTrackingWIP();
            InventoryTrackingWIP oInventoryTrackingWIP = new InventoryTrackingWIP();
            RouteSheetSetup oRSS = new RouteSheetSetup();
            oRSS = oRSS.Get(1, (int)Session[SessionInfo.currentUserID]);
            oInventoryTrackingWIP.BUID = nBUID; oInventoryTrackingWIP.StartDate = dStartDate; oInventoryTrackingWIP.EndDate = dEndDate.AddDays(1);
            _oInventoryTrackingWIP.MainList = InventoryTrackingWIP.Gets(oInventoryTrackingWIP, (int)Session[SessionInfo.currentUserID]);

            string Sql = "SELECT ProductID, WorkingUnitID, SUM(Qty) AS Qty, (SELECT StoreName FROM View_WorkingUnit WHERE WorkingUnitID = IT.WorkingUnitID) AS WorkingUnitName FROM ITransaction AS IT WHERE iT.TriggerParentType=106 and IT.InOutType=102 and IT.[DateTime]>='" + oInventoryTrackingWIP.StartDate.ToString("dd MMM yyyy HH:mm:ss") + "' AND IT.[DateTime]<'" + oInventoryTrackingWIP.EndDate.ToString("dd MMM yyyy HH:mm:ss") + "' AND IT.WorkingUnitID<>" + oRSS.WorkingUnitIDWIP + " GROUP BY WorkingUnitID, ProductID";
            _oInventoryTrackingWIP.IssueList = InventoryTrackingWIP.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

            //Sql = "SELECT ProductID, WorkingUnitID, SUM(Qty) AS Qty, (SELECT StoreName FROM View_WorkingUnit WHERE WorkingUnitID = IT.WorkingUnitID) AS WorkingUnitName FROM ITransaction AS IT WHERE iT.TriggerParentType=106 and IT.InOutType=101 and IT.[DateTime]>='" + oInventoryTrackingWIP.StartDate + "' AND IT.[DateTime]<'" + oInventoryTrackingWIP.EndDate + "' AND IT.WorkingUnitID<>" + oRSS.WorkingUnitIDWIP + " GROUP BY WorkingUnitID, ProductID";
            Sql = "select ProductID,Sum(GG.Qty) as Qty,Sum(GG.QtyPacking) as QtyPacking, sum(GG.Qty_RS) as Qty_RS, sum(isnull(GG.QtyRecycle,0)) as QtyRecycle ,sum(isnull(GG.QtyWastage,0)) as QtyWastage,sum(GG.Qty_RS-GG.QtyPacking-GG.QtyRecycle-GG.QtyWastage) as QtyShort from ( " +
                "select ProductID,HH.TriggerParentID, HH.Qty as Qty,HH.Qty_RS,HH.QtyPacking ,CASE WHEN isnull(HH.Qty_RS,0)>0 THEN isnull((HH.QtyRecycle),0) ELSE 0 END QtyRecycle ,CASE WHEN isnull(HH.Qty_RS,0)>0 THEN isnull((HH.QtyWastage),0) ELSE 0 END QtyWastage " +
                "from (select TriggerParentType,sum(Qty) as Qty,InOutType,WorkingUnitID,ProductID,TriggerParentID ,(Select Sum(RSD.Qty_RS) from RouteSheetDO as RSD where RSD.RSState=13 and RSD.RouteSheetID=IT.TriggerParentID ) as Qty_RS " +
                ",isnull((Select isnull(sum(Qty),0) from RSInQCDetail as RS where RS.ManagedLotID>0 and rs.RouteSheetID=IT.TriggerParentID and rs.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType not in (2,3))),0) as QtyPacking " +
                ",isnull((Select isnull(sum(Qty),0) from RSInQCDetail as RS where RS.RouteSheetID=IT.TriggerParentID and RS.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType in (2))),0) as QtyRecycle " +
                ",(Select isnull(sum(Qty),0) from RSInQCDetail as RS where rs.RouteSheetID=IT.TriggerParentID and rs.RSInQCSetupID in (Select RSInS.RSInQCSetupID from RSInQCSetup as RSInS where RSInS.YarnType in (3))) as QtyWastage " +
                "from ITransaction as IT where  it.InOutType=102 and IT.WorkingUnitID in (select top 1 WorkingUnitIDWIP from RouteSheetSetup) and IT.[DateTime]>='" + oInventoryTrackingWIP.StartDate.ToString("dd MMM yyyy HH:mm:ss") + "'  and IT.[DateTime]<'" + oInventoryTrackingWIP.EndDate.ToString("dd MMM yyyy HH:mm:ss") + "' group by TriggerParentType,InOutType,WorkingUnitID,ProductID,TriggerParentID) as HH ) as GG group by GG.ProductID";
            _oInventoryTrackingWIP.ReceiveList = InventoryTrackingWIP.Gets(Sql, (int)Session[SessionInfo.currentUserID]);

            _oInventoryTrackingWIP.ITWIPsForIssue = _oInventoryTrackingWIP.IssueList.GroupBy(x => x.WorkingUnitID).Select(y => y.FirstOrDefault()).OrderBy(c => c.WorkingUnitID).ToList();
            //_oInventoryTrackingWIP.ITWIPsForReceive = _oInventoryTrackingWIP.ReceiveList.GroupBy(x => x.WorkingUnitID).Select(y => y.FirstOrDefault()).ToList();


            if (_oInventoryTrackingWIP.MainList.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (nBUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Item Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Opening Qty", Width = 15f, IsRotate = false });
                foreach (InventoryTrackingWIP oItem in _oInventoryTrackingWIP.ITWIPsForIssue)
                    table_header.Add(new TableHeader { Header = oItem.WorkingUnitName, Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total In", Width = 15f, IsRotate = false });
                //foreach (InventoryTrackingWIP oItem in _oInventoryTrackingWIP.ITWIPsForReceive)
                    //table_header.Add(new TableHeader { Header = oItem.WorkingUnitName, Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Out Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fresh Packing", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Re-Cycle", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Wastage", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Short/Gain", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Total Out", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Closing Qty", Width = 15f, IsRotate = false });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Inventory Tracking WIP");

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = "Inventory Tracking WIP"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 1;
                    #endregion

                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    #endregion


                    #region Data
                    int nSL = 0;
                    double nGrandTotalOutQty = 0, nGrandTotalClosingQty = 0;
                    foreach (InventoryTrackingWIP oItem in _oInventoryTrackingWIP.MainList)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nSL).ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.OpeningQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                        foreach (InventoryTrackingWIP oObj in _oInventoryTrackingWIP.ITWIPsForIssue)
                        {
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.IssueList.Where(x=>x.ProductID == oItem.ProductID && x.WorkingUnitID == oObj.WorkingUnitID).Sum(x=>x.Qty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        }
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.IssueList.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                        //foreach (InventoryTrackingWIP oObj in _oInventoryTrackingWIP.ITWIPsForReceive)
                        //{
                        //    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.ReceiveList.Where(x => x.ProductID == oItem.ProductID && x.WorkingUnitID == oObj.WorkingUnitID).Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        //}
                        InventoryTrackingWIP oRcvObj = new InventoryTrackingWIP();
                        oRcvObj = _oInventoryTrackingWIP.ReceiveList.Where(x => x.ProductID == oItem.ProductID).FirstOrDefault();
                        double nTotalOutQty = 0;
                        if(oRcvObj != null)
                        {
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRcvObj.Qty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRcvObj.QtyPacking.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRcvObj.QtyRecycle.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRcvObj.QtyWastage.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRcvObj.QtyShort.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            nTotalOutQty = (oRcvObj.QtyPacking + oRcvObj.QtyRecycle + oRcvObj.QtyWastage + oRcvObj.QtyShort);
                            nGrandTotalOutQty += nTotalOutQty;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nTotalOutQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem.OpeningQty + _oInventoryTrackingWIP.IssueList.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty)) - oRcvObj.Qty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            nGrandTotalClosingQty += ((oItem.OpeningQty + _oInventoryTrackingWIP.IssueList.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty)) - oRcvObj.Qty);
                        }
                        else
                        {
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                            nTotalOutQty = 0; nGrandTotalOutQty += nTotalOutQty;
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, false, false);
                            ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem.OpeningQty + _oInventoryTrackingWIP.IssueList.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty))).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                            nGrandTotalClosingQty += (oItem.OpeningQty + _oInventoryTrackingWIP.IssueList.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty));
                        }

                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, ((oItem.OpeningQty + _oInventoryTrackingWIP.IssueList.Where(x => x.ProductID == oItem.ProductID).Sum(x => x.Qty)) - nTotalOutQty).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        

                        nRowIndex++;

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, nStartCol, nStartCol+=1, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)"; nStartCol++;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.MainList.Sum(x => x.OpeningQty).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    foreach (InventoryTrackingWIP oObj in _oInventoryTrackingWIP.ITWIPsForIssue)
                    {
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.IssueList.Where(x => x.WorkingUnitID == oObj.WorkingUnitID).Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    }
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.IssueList.Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Right, true, false);

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.ReceiveList.Sum(x => x.Qty).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.ReceiveList.Sum(x => x.QtyPacking).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.ReceiveList.Sum(x => x.QtyRecycle).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.ReceiveList.Sum(x => x.QtyWastage).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, _oInventoryTrackingWIP.ReceiveList.Sum(x => x.QtyShort).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nGrandTotalOutQty.ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nGrandTotalClosingQty.ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Inventory_Tracking_WIP.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

    }

}
