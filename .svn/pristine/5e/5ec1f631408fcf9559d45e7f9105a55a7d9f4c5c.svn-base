using System;
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
    public class WeavingYarnStockController : Controller
    {
        #region Declaration

        WeavingYarnStock _oWeavingYarnStock = new WeavingYarnStock();
        List<WeavingYarnStock> _oWeavingYarnStocks = new List<WeavingYarnStock>();
        #endregion

        #region Actions
        public ActionResult ViewWeavingYarnStock(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oWeavingYarnStocks = new List<WeavingYarnStock>();
            _oWeavingYarnStocks = WeavingYarnStock.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oWeavingYarnStocks);
        }

        #endregion

        #region Adv Search
        [HttpPost]
        public JsonResult AdvSearch(WeavingYarnStock oWeavingYarnStock)
        {
            List<WeavingYarnStock> oWeavingYarnStocks = new List<WeavingYarnStock>();
            try
            {
                string sSQL = MakeSQL(oWeavingYarnStock.ErrorMessage);
                oWeavingYarnStocks = WeavingYarnStock.Gets(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oWeavingYarnStock.ErrorMessage = ex.Message;
                oWeavingYarnStocks.Add(oWeavingYarnStock);
            }
            var jsonResult = Json(oWeavingYarnStocks, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(string sString)
        {
            int nCount = 0;
            string sDispoNoAdv = "";
            int nDispoDateAdv = 0;
            DateTime dFromDispoDateAdv = DateTime.Today;
            DateTime dToDispoDateAdv = DateTime.Today;

            if (!String.IsNullOrEmpty(sString))
            {
                sDispoNoAdv = Convert.ToString(sString.Split('~')[nCount++]);
                nDispoDateAdv = Convert.ToInt32(sString.Split('~')[nCount++]);
                dFromDispoDateAdv = Convert.ToDateTime(sString.Split('~')[nCount++]);
                dToDispoDateAdv = Convert.ToDateTime(sString.Split('~')[nCount++]);
            }

            //string sReturn1 = "select FSCDetailID,FEOSID,sum(Qty) from View_DyeingOrderFabricDetail";
            string sReturn = "";

            #region DispoNo
            if (!string.IsNullOrEmpty(sDispoNoAdv))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FSCDID in (Select dd.FabricsalesContractDetailID from FabricsalesContractDetail as dd where ExeNo like '%" + sDispoNoAdv + "%')";
            }
            #endregion

            #region Exe Date Search
            if (nDispoDateAdv > 0)
            {
                if (nDispoDateAdv == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dFromDispoDateAdv.ToString("dd MMM yyyy") + "'";
                }
                if (nDispoDateAdv == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + dFromDispoDateAdv.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dFromDispoDateAdv.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDispoDateAdv == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dFromDispoDateAdv.ToString("dd MMM yyyy") + "'";
                }
                if (nDispoDateAdv == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dFromDispoDateAdv.ToString("dd MMM yyyy") + "'";
                }
                if (nDispoDateAdv == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + dFromDispoDateAdv.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dToDispoDateAdv.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (nDispoDateAdv == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate< '" + dFromDispoDateAdv.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dToDispoDateAdv.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion
            
            #region ForwardToDOby
            //Global.TagSQL(ref sReturn);
            //sReturn = sReturn + "isnull(ForwardToDOby,0)<>0 and ProdtionType in (" + (int)EnumDispoProType.General + "," + (int)EnumDispoProType.ExcessDyeingQty + "," + (int)EnumDispoProType.ExcessFabric + ")";
            #endregion

            return sReturn;
        }
        #endregion

        #region Get
        //[HttpPost]
        //public JsonResult GetsDetailsByID(WeavingYarnStockDetail oWeavingYarnStockDetail)//Id=ContractorID
        //{
        //    try
        //    {
        //        string Ssql = "SELECT*FROM View_WeavingYarnStockDetail WHERE WeavingYarnStockID=" + oWeavingYarnStockDetail.WeavingYarnStockID + " ";
        //        _oWeavingYarnStockDetails = new List<WeavingYarnStockDetail>();
        //        _oWeavingYarnStockDetail.WeavingYarnStockDetails = WeavingYarnStockDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oWeavingYarnStockDetail.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oWeavingYarnStockDetail);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region XL
        public void ExportXL(string sParams)
        {
            _oWeavingYarnStocks = new List<WeavingYarnStock>();
            WeavingYarnStock oWeavingYarnStock = new WeavingYarnStock();
            oWeavingYarnStock.ErrorMessage = sParams;
            string sSQL = MakeSQL(oWeavingYarnStock.ErrorMessage);
            _oWeavingYarnStocks = WeavingYarnStock.Gets(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (_oWeavingYarnStocks.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 17f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Count", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Shade", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Store Rcv. Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Balance", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Weaving Rcv. Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Balance", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Beam", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Loom", Width = 30f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. Warp", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Warp Production", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. Grey Fab.", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Grey Production", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Balance", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Req. Dye", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dye Production", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Balance", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Remarks", Width = 15f, IsRotate = false });
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Weaving Yarn Stock Report");

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

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true; cell.Value = "Weaving Yarn Stock Report"; cell.Style.Font.Bold = true;
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
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    #endregion

                    #region Data
                    int nSL = 0;
                    foreach (WeavingYarnStock oItem in _oWeavingYarnStocks)
                    {
                        nStartCol = 2;
                        ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (++nSL).ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BuyerName, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.WeftCount, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Shade, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DyedYarnReqWeft.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.StoreRcvQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.StoreRcvBalance.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.WeavingRcvQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.WeavingRcvBalance.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.BeamNo, false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.LoomNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.RequiredWarpLength.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.TotalWarpProduction.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.ReqGreyFabrics.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.TotalGreyProduction.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.GreyProdBalance.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.SWQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DyeProductionQty.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.DyeBalance.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oItem.Remarks, false, ExcelHorizontalAlignment.Center, false, false);

                        nRowIndex++;

                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, nStartCol, nStartCol += 4, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.Formatter = "#,##0.00;(#,##0.00)";
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.DyedYarnReqWeft).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.StoreRcvQty).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.StoreRcvBalance).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.WeavingRcvQty).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.WeavingRcvBalance).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.RequiredWarpLength).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.TotalWarpProduction).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.ReqGreyFabrics).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.TotalGreyProduction).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.GreyProdBalance).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.SWQty).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.DyeProductionQty).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, ++nStartCol, _oWeavingYarnStocks.Sum(c => c.DyeBalance).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, ++nStartCol, nStartCol, true, ExcelHorizontalAlignment.Right, false);
                    
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Weaving_Yarn_Stock_Report.xlsx");
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
