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
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Diagnostics;

namespace ESimSolFinancial.Controllers
{
    public class DORegisterController : Controller
    {
        #region Declaration
        DORegister _oDORegister = new DORegister();
        List<DORegister> _oDORegisters = new List<DORegister>();
        string _sFormatter;
        string _sErrorMesage;
        #endregion

        public ActionResult View_DORegister(int buid, int menuid)
        {
            _oDORegister = new DORegister();
            _oDORegisters = new List<DORegister>();

            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ReportLayouts = EnumObject.jGets(typeof(EnumReportLayout)).Where(x => x.id == (int)EnumReportLayout.ProductCatagoryWise || x.id == (int)EnumReportLayout.PartyWise);
            ViewBag.ProductCategorys = ProductCategory.GetsBUWiseLastLayer(buid, (int)Session[SessionInfo.currentUserID]);
            return View(_oDORegisters);
        }
        #region Search
        public JsonResult AdvSearch(DORegister oDORegister)
        {
            List<DORegister> oDORegisters = new List<DORegister>();
            try
            {
                string sSql = GetSQL(oDORegister);
                oDORegisters = DORegister.Gets(sSql, oDORegister.ReportLayout, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDORegister.ErrorMessage = ex.Message;
                oDORegisters.Add(oDORegister);
            }
            var jSonResult = Json(oDORegisters, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        private string GetSQL(DORegister oDORegister)
        {
            int nCount = 3;
            int nDateCriteria_Issue = Convert.ToInt32(oDORegister.ErrorMessage.Split('~')[nCount++]);
            DateTime dStart_Issue = Convert.ToDateTime(oDORegister.ErrorMessage.Split('~')[nCount++]),
                    dEnd_Issue = Convert.ToDateTime(oDORegister.ErrorMessage.Split('~')[nCount++]);

            string sSQL = " ";

            #region ExportPI
            string string1 = "ExportSCID IN (SELECT ExportSCID FROM ExportSC WHERE ExportPIID IN ( SELECT ExportPIID FROM ExportPI WHERE BUID= " + oDORegister.BUID;
           
            if (!string.IsNullOrEmpty(oDORegister.PINo))
            {
                string1 += " AND PINo Like '%"+oDORegister.PINo+"%'";
            }
            if (!string.IsNullOrEmpty(oDORegister.BuyerName))
            {
                Global.TagSQL(ref string1);
                string1 = string1 + " BuyerID IN (" + oDORegister.BuyerName + ")";
            }
            if (!string.IsNullOrEmpty(oDORegister.CustomerName))
            {
                Global.TagSQL(ref string1);
                string1 = string1 + " ContractorID IN (" + oDORegister.CustomerName + ")";
            }
           
            DateObject.CompareDateQuery(ref string1, "IssueDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            sSQL= string1 += " ))";
            #endregion

            if (!string.IsNullOrEmpty(oDORegister.ProductCategoryName) && !oDORegister.ProductCategoryName.Equals("0"))
            {
                sSQL += " AND ProductID IN (SELECT ProductID FROM Product WHERE ProductCategoryID="+oDORegister.ProductCategoryName+")";
            }

            return sSQL;
        }
        #endregion

        #region Print
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(DORegister oDORegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDORegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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
      
        public ActionResult PrintDORegister(double ts)
        {
            DORegister oDORegister = new DORegister();
            try
            {
                _sErrorMesage = "";
                _oDORegisters = new List<DORegister>();
                oDORegister = (DORegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oDORegister);
                _oDORegisters = DORegister.Gets(sSQL, oDORegister.ReportLayout, (int)Session[SessionInfo.currentUserID]);
                if (_oDORegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDORegisters = new List<DORegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oDORegister.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptDORegisters oReport = new rptDORegisters();
                byte[] abytes = oReport.PrepareReport(_oDORegisters, oCompany, (EnumReportLayout)oDORegister.ReportLayout, "");
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
                cell.Value = Convert.ToDouble(sVal);
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
        public void ExportToExcelDORegister(double ts)
        {
            int ExportPIID = -999;
            string Header = "", HeaderColumn = "";

            Company oCompany = new Company();
            DORegister oDORegister = new DORegister();
            try
            {
                _sErrorMesage = "";
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oDORegisters = new List<DORegister>();
                oDORegister = (DORegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oDORegister);
                _oDORegisters = DORegister.Gets(sSQL, oDORegister.ReportLayout, (int)Session[SessionInfo.currentUserID]);
                if (_oDORegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDORegisters = new List<DORegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC File No Name", Width = 15f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Customer Name", Width = 45f, IsRotate = false });

                if (oDORegister.ReportLayout == (int)EnumReportLayout.ProductCatagoryWise)
                    table_header.Add(new TableHeader { Header = "Buyer Name", Width = 45f, IsRotate = false });
                else
                    table_header.Add(new TableHeader { Header = "Product Catagory", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Product Name", Width = 35f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Style No", Width = 25f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "PI Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yet To DO", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yet To Challan", Width = 15f, IsRotate = false });
                #endregion

                #region Layout Wise Header
                if (oDORegister.ReportLayout == (int)EnumReportLayout.ProductCatagoryWise)
                {
                    Header = "Product Catagory Wise"; HeaderColumn = "Catagory Name : ";
                }
                else 
                {
                    Header = "Buyer Wise"; HeaderColumn = "Buyer Name : ";
                }
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Purchase Order Register");
                    sheet.Name = "Purchase Order Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Purchase Order Register (" + Header + ") "; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 8, nRowIndex, 10]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    List<DORegister> oTempDORegister = new List<DORegister>();

                    var data = oTempDORegister.GroupBy(x => new { x.BuyerID, x.BuyerName }, (key, grp) => new
                    {
                        HeaderName = key.BuyerName,
                        Total_Qty = grp.Sum(x => x.Qty),
                        Total_DOQty = grp.Sum(x => x.DOQty),
                        Total_ChallanQty = grp.Sum(x => x.ChallanQty),
                        Total_YetToDO = grp.Sum(x => x.YetToDO),
                        Total_YetToChallan = grp.Sum(x => x.YetToChallan),
                        Results = grp.ToList()
                    });

                    #region Group By Layout Wise
                    if (oDORegister.ReportLayout == (int)EnumReportLayout.ProductCatagoryWise)
                    {
                        data = _oDORegisters.GroupBy(x => new { x.ProductCategoryName }, (key, grp) => new
                        {
                            HeaderName = key.ProductCategoryName,
                            Total_Qty = grp.Sum(x => x.Qty),
                            Total_DOQty = grp.Sum(x => x.DOQty),
                            Total_ChallanQty = grp.Sum(x => x.ChallanQty),
                            Total_YetToDO = grp.Sum(x => x.YetToDO),
                            Total_YetToChallan = grp.Sum(x => x.YetToChallan),
                            Results = grp.ToList()
                        });
                    }
                    else
                    {
                        data = _oDORegisters.GroupBy(x => new { x.BuyerID, x.BuyerName }, (key, grp) => new
                        {
                            HeaderName = key.BuyerName,
                            Total_Qty = grp.Sum(x => x.Qty),
                            Total_DOQty = grp.Sum(x => x.DOQty),
                            Total_ChallanQty = grp.Sum(x => x.ChallanQty),
                            Total_YetToDO = grp.Sum(x => x.YetToDO),
                            Total_YetToChallan = grp.Sum(x => x.YetToChallan),
                            Results = grp.ToList()
                        });
                    }
                    #endregion

                    string sCurrencySymbol = "";
                    #region Data
                    foreach (var oItem in data)
                    {
                        nRowIndex++;

                        nStartCol = 2;
                        FillCellMerge(ref sheet, HeaderColumn + oItem.HeaderName, nRowIndex, nRowIndex, nStartCol, nEndCol + 1, true, ExcelHorizontalAlignment.Left);

                        nRowIndex++;
                        foreach (TableHeader listItem in table_header)
                        {
                            cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        ExportPIID = 0;
                        nRowIndex++; int nCount = 0, nRowSpan = 0;
                        foreach (var obj in oItem.Results)
                        {
                            #region Order Wise Merge
                            if (ExportPIID != obj.ExportPIID)
                            {
                                if (nCount > 0)
                                {
                                    nStartCol = 8;
                                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);

                                    _sFormatter = " #,##0;(#,##0)";
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.Qty).ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.DOQty).ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.YetToDO).ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.ChallanQty).ToString(), true, true);
                                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.YetToChallan).ToString(), true, true);
                                    nRowIndex++;
                                }

                                nStartCol = 2;
                                nRowSpan = oItem.Results.Where(OrderR => OrderR.ExportPIID == obj.ExportPIID).ToList().Count;

                                FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.PINo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.PIDateSt.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.LCFileNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                                FillCellMerge(ref sheet, obj.CustomerName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);

                                if (oDORegister.ReportLayout == (int)EnumReportLayout.ProductCatagoryWise)
                                    FillCellMerge(ref sheet, obj.BuyerName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++);
                            }
                            #endregion

                            nStartCol = 7;
                            if (oDORegister.ReportLayout != (int)EnumReportLayout.ProductCatagoryWise)
                                FillCell(sheet, nRowIndex, nStartCol++, obj.ProductCategoryName, false);
                            else nStartCol++;

                            FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.ColorName, false);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.StyleNo, false);
                           
                            _sFormatter = " #,##0;(#,##0)";
                            FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.DOQty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.YetToDO.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.ChallanQty.ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, obj.YetToChallan.ToString(), true);
                            nRowIndex++;

                            ExportPIID = obj.ExportPIID;
                        }

                        nStartCol = 8;
                        FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 2, true, ExcelHorizontalAlignment.Right);
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.Qty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.DOQty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.YetToDO).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.ChallanQty).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.YetToChallan).ToString(), true, true);
                        nRowIndex++;

                        nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                        FillCellMerge(ref sheet, Header + " Sub Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Total_Qty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Total_DOQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Total_YetToDO.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Total_ChallanQty.ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Total_YetToChallan.ToString(), true, true);
                        nRowIndex++;
                    }

                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Grand Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.Total_Qty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.Total_DOQty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.Total_YetToDO).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.Total_ChallanQty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, data.Sum(x => x.Total_YetToChallan).ToString(), true, true);
                    nRowIndex++;

                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, 15];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=DORegister(" + Header + ").xlsx");
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
            Debug.WriteLine(startRowIndex + " -Row--> " + endRowIndex + " || " + startColIndex + " -Column--> " + endColIndex + " || Data: " + sVal);
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

        #endregion
    }
}
