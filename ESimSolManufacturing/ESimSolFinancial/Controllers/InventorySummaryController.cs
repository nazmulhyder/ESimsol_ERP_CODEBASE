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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class InventorySummaryController : Controller
    {

        #region Declaration
        string _sDateRange = "";
        string _sErrorMesage = "";
        string _sFormatter = "";
        InventoryTraking _oInventoryTraking = new InventoryTraking();
        List<InventoryTraking> _oInventoryTrakings = new List<InventoryTraking>();
        BusinessUnit oBusinessUnit = new BusinessUnit();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        #endregion

        public ActionResult ViewInventorySummaries(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.InventoryTracking).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oInventoryTrakings = new List<InventoryTraking>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oClientOperationSetting = new ClientOperationSetting();
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
            ViewBag.BUID = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            List<EnumObject> oGRNTypes = new List<EnumObject>();
            List<EnumObject> oTempGRNTypes = new List<EnumObject>();
            oGRNTypes = EnumObject.jGets(typeof(EnumGRNType));
            foreach (EnumObject oItem in oGRNTypes)
            {
                if ((EnumGRNType)oItem.id == EnumGRNType.ImportInvoice || (EnumGRNType)oItem.id == EnumGRNType.ImportPI || (EnumGRNType)oItem.id == EnumGRNType.LocalInvoice || (EnumGRNType)oItem.id == EnumGRNType.None || (EnumGRNType)oItem.id == EnumGRNType.PurchaseOrder || (EnumGRNType)oItem.id == EnumGRNType.WorkOrder)
                {
                    oTempGRNTypes.Add(oItem);
                }
            }
            ViewBag.GRNType = oTempGRNTypes;
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oInventoryTrakings);
        }

        [HttpPost]
        public JsonResult SearchByDate(InventoryTraking oInventoryTraking)
        {
            _oInventoryTrakings = new List<InventoryTraking>();
            try
            {
                int nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
                string sSQL = MakeSQL(oInventoryTraking);

                _oInventoryTrakings = InventoryTraking.Gets_ForInventorySummary(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, nReportLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count > 0)
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

        private string MakeSQL(InventoryTraking oInventoryTraking)
        {
            int nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
            string BUIDs = oInventoryTraking.Param.Split('~')[1];
            string WUIDs = oInventoryTraking.Param.Split('~')[2];
            string SupplierIDs = oInventoryTraking.Param.Split('~')[3];
            string TSIDs = oInventoryTraking.Param.Split('~')[4];
            string PrductIDs = oInventoryTraking.Param.Split('~')[5];
            int nPC = Convert.ToInt32(oInventoryTraking.Param.Split('~')[6]);
            string LotNo = oInventoryTraking.Param.Split('~')[7];
            double UnitPrice = Convert.ToDouble(oInventoryTraking.Param.Split('~')[8]);
            int nRefType = (int)oInventoryTraking.RefType;
            string sRefNo = oInventoryTraking.RefNo;

            string sSQL = "INSERT INTO #TempTable(LotID) SELECT Lot.LotID FROM Lot WHERE Lot.ParentType = "+(int)EnumTriggerParentsType.GRNDetailDetail+" AND Lot.LotID IN (SELECT DISTINCT IT.LotID FROM ITransaction AS IT WHERE CONVERT(DATE,CONVERT(VARCHAR(12),IT.[DateTime],106)) <= CONVERT(DATE,CONVERT(VARCHAR(12),'" + oInventoryTraking.EndDate.ToString("dd MMM yyyy") + "',106)))";

            #region Multiple BU / for Group
            if (!string.IsNullOrWhiteSpace(BUIDs))
            {
                sSQL += " AND Lot.BUID IN (" + BUIDs + ")";
            }
            #endregion

            #region BU
            if (oInventoryTraking.BUID != 0)
            {
                sSQL += " AND Lot.BUID =" + oInventoryTraking.BUID;
            }
            #endregion

            #region WU
            if (!string.IsNullOrEmpty(WUIDs))
            {
                sSQL += " AND Lot.WorkingUnitID IN (" + WUIDs + ")";
            }
            #endregion

            #region RefType
            if (nRefType != 0)
            {
                sSQL = sSQL + " AND Lot.ParentID IN (SELECT GRNDetailID FROM GRNDetail WHERE RefType = " + nRefType + ")";
            }
            #endregion

            #region RefNo
            if (sRefNo != null && sRefNo != "")
            {
                if (nRefType != 0)
                {
                    sSQL = sSQL + " AND Lot.ParentID IN (SELECT GRNDetailID FROM GRNDetail WHERE RefType = " + nRefType + " AND RefObjectID IN (" + sRefNo + ") )";
                }
            }
            #endregion

            #region Supplier
            if (!string.IsNullOrEmpty(SupplierIDs))
            {
                sSQL += " AND Lot.ContractorID IN (" + SupplierIDs + ")";
            }
            #endregion

            #region StyleID
            if (!string.IsNullOrEmpty(TSIDs))
            {
                sSQL += " AND Lot.StyleID IN (" + TSIDs + ")";
            }
            #endregion

            #region Product
            if (!string.IsNullOrEmpty(PrductIDs))
            {
                sSQL += " AND Lot.ProductID IN (" + PrductIDs + ")";
            }
            #endregion

            #region Product Category
            if (nPC != 0)
            {
                sSQL += " AND Lot.ProductID IN (SELECT ProductID FROM Product WHERE ProductCategoryID  IN  (SELECT MM.ProductCategoryID FROM dbo.fn_GetProductCategory(" + nPC + ") AS MM))";
            }
            #endregion

            #region Lot
            if (!string.IsNullOrEmpty(LotNo))
            {
                sSQL += " AND Lot.LotNo LIKE '%" + LotNo + "%'";
            }
            #endregion

            #region Unit Price
            if (UnitPrice > 0)
            {
                sSQL += " AND Lot.UnitPrice =" + UnitPrice;
            }
            #endregion

            return sSQL;
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

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(InventoryTraking oInventoryTraking)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oInventoryTraking);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintInventoryMovements(double ts)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            string _sErrorMesage = "";
            int nReportLayout = 0;
            try
            {
                oInventoryTraking = (InventoryTraking)Session[SessionInfo.ParamObj];
                nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
                _oInventoryTrakings = new List<InventoryTraking>();
                string sSQL = this.MakeSQL(oInventoryTraking);

                _oInventoryTrakings = InventoryTraking.Gets_ForInventorySummary(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, nReportLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oInventoryTrakings.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }

                oBusinessUnit = oBusinessUnit.Get(oInventoryTraking.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oInventoryTrakings = new List<InventoryTraking>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                if (oBusinessUnit == null || oBusinessUnit.BusinessUnitID <= 0)
                {
                    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                }
                else
                {
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
                }
                rptInventorySummary oReport = new rptInventorySummary();
                byte[] abytes = oReport.PrepareReport(_oInventoryTrakings, oCompany, nReportLayout);
                //byte[] abytes = null;
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        #region Excel
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber)
                cell.Value = (Convert.ToDouble(sVal)).ToString("#,###.##;(#,###.##)");
            else
                cell.Value = sVal;
            cell.Style.Font.Bold = IsBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = _sFormatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        public void PrintInventoryMovementsExcel(double ts)
        {
            int nGRNID = -999;
            string Header = "", HeaderColumn = ""; int nReportLayout = 0;

            double _nTOpeningQty = 0;
            double _nTOpeningAmt = 0;
            double _nTInQty = 0;
            double _nTInAmt = 0;
            double _nTOutQty = 0;
            double _nTOutAmt = 0;
            double _nTClosingQty = 0;
            double _nTClosingAmt = 0;

            Company oCompany = new Company();
            InventoryTraking oInventoryTraking = new InventoryTraking();
            try
            {
                oInventoryTraking = (InventoryTraking)Session[SessionInfo.ParamObj];
                nReportLayout = Convert.ToInt32(oInventoryTraking.Param.Split('~')[0]);
                _oInventoryTrakings = new List<InventoryTraking>();
                string sSQL = this.MakeSQL(oInventoryTraking);
                _oInventoryTrakings = InventoryTraking.Gets_ForInventorySummary(oInventoryTraking.StartDate, oInventoryTraking.EndDate, sSQL, nReportLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = null;// GetCompanyLogo(oCompany);
            if (oInventoryTraking.BUID > 0)
            {
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oInventoryTraking.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }

            if (_sErrorMesage == "")
            {
                #region Header
                Header = "Product & LC Wise";
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "NAME OF ITEMS", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "UNIT", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "ORIGIN", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "SUPPLIER NAME", Width = 45f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PURCHASE TYPE", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "REF NO", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "U.P.(TK)", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "OPENING STOCK", Width = 15f, IsRotate = false,
                ChildHeader = new List<TableHeader>()
                    {
                        new TableHeader { Header = "KG", Width = 5f, IsRotate = false},
                        new TableHeader { Header = "TOTAL AMOUNT", Width = 10f, IsRotate = false}
                    }
                });
                table_header.Add(new TableHeader { Header = "RECEIVED QTY", Width = 15f, IsRotate = false,
                ChildHeader = new List<TableHeader>()
                    {
                        new TableHeader { Header = "KG", Width = 5f, IsRotate = false},
                        new TableHeader { Header = "TOTAL AMOUNT", Width = 10f, IsRotate = false}
                    }
                });
                table_header.Add(new TableHeader { Header = "ISSUED QTY", Width = 15f, IsRotate = false,
                ChildHeader = new List<TableHeader>()
                    {
                        new TableHeader { Header = "KG", Width = 5f, IsRotate = false},
                        new TableHeader { Header = "TOTAL AMOUNT", Width = 10f, IsRotate = false}
                    }
                });
                table_header.Add(new TableHeader { Header = "COLSING QTY", Width = 15f, IsRotate = false,
                ChildHeader = new List<TableHeader>()
                    {
                        new TableHeader { Header = "KG", Width = 5f, IsRotate = false},
                        new TableHeader { Header = "TOTAL AMOUNT", Width = 10f, IsRotate = false}
                    }
                });
                table_header.Add(new TableHeader { Header = "REMARKS", Width = 25f, IsRotate = false });


                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("InventorySummary Register");
                    sheet.Name = "InventorySummary Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "InventorySummary Register (" + Header + ") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    //#region Group By Layout Wise
                    //var data = _oInventoryTrakings.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                    //{
                    //    HeaderName = key.ProductName,
                    //    TotalQty = grp.Sum(x => x.ReceivedQty),
                    //    Results = grp.ToList()
                    //});
                    //#endregion

                    //foreach (TableHeader listItem in table_header)
                    //{
                    //    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    //}
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 8);
                    //nRowIndex++;
                    string sCurrencySymbol = ""; int nCount = 0;
                    #region Data
                    foreach (var oItem in _oInventoryTrakings)
                    {

                        nStartCol = 2;

                        //foreach (var obj in _oInventoryTrakings)
                        //{

                            nStartCol = 2;
                            FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), false);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName, false);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.MUnit, false);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Origin, false);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.RefTypeStTemp, false);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.RefNo, false);
                            FillCell(sheet, nRowIndex, nStartCol++, oItem.UnitPriceSt, false);
                            if (oItem.OpeningQty.ToString() == "0")
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, "0", true);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, oItem.OpeningQty.ToString(), true);
                            }
                            if (oItem.OpeningAmount.ToString() == "0")
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, "0", true);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, oItem.OpeningAmount.ToString(), true);
                            }
                            if (oItem.InQty.ToString() == "0")
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, "0", true);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, oItem.InQty.ToString(), true);
                            }
                            if (oItem.InAmount.ToString() == "0")
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, "0", true);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, oItem.InAmount.ToString(), true);
                            }
                            if (oItem.OutQty.ToString() == "0")
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, "0", true);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, oItem.OutQty.ToString(), true);
                            }
                            if (oItem.OutAmount.ToString() == "0")
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, "0", true);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, oItem.OutAmount.ToString(), true);
                            }
                            if (oItem.ClosingQty.ToString() == "0")
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, "0", true);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, oItem.ClosingQty.ToString(), true);
                            }
                            if (oItem.ClosingAmount.ToString() == "0")
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, "0", true);
                            }
                            else
                            {
                                FillCell(sheet, nRowIndex, nStartCol++, oItem.ClosingAmount.ToString(), true);
                            }

                            FillCell(sheet, nRowIndex, nStartCol++, oItem.Remarks, false);

                            _nTOpeningQty = _nTOpeningQty + oItem.OpeningQty;
                            _nTOpeningAmt = _nTOpeningAmt + oItem.OpeningAmount;
                            _nTInQty = _nTInQty + oItem.InQty;
                            _nTInAmt = _nTInAmt + oItem.InAmount;
                            _nTOutQty = _nTOutQty + oItem.OutQty;
                            _nTOutAmt = _nTOutAmt + oItem.OutAmount;
                            _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                            _nTClosingAmt = _nTClosingAmt + oItem.ClosingAmount;

                            
                        //}

                        nRowIndex++;
                    }

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, _nTOpeningQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _nTOpeningAmt.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _nTInQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _nTInAmt.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _nTOutQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _nTOutAmt.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _nTClosingQty.ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _nTClosingAmt.ToString(), true, true);
                    
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 13];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=InventoryTraking(" + Header + ").xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = isBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }

        #endregion

    }
}