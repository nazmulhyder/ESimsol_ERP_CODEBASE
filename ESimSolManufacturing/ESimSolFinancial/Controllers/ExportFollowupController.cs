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
using System.Security;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections;

namespace ESimSolFinancial.Controllers
{
    public class ExportFollowupController : PdfViewController
    {
        string _sErrorMesage = ""; int _nReportLayout;
        ExportBillReport _oExportBillReport = new ExportBillReport();
        List<ExportBillReport> _oExportBillReports = new List<ExportBillReport>();
        List<ExportLCRegister> _oExportLCRegisters = new List<ExportLCRegister>();
        List<ExportFollowup> _oExportFollowups = new List<ExportFollowup>();
        string Formatter = "#,##0.00";
        public ActionResult View_ExportFollowups(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();

            BusinessUnit oBusinessUnit = new BusinessUnit();
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

            ViewBag.Buid = buid;
            ViewBag.BusinessUnits = oBusinessUnits;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));

            return View(oExportFollowups);
        }
        public ActionResult View_ExportFollowupSummary(int buid, string sParam)
        {
            ExportFollowup oExportFollowup = new ExportFollowup();
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            //try
            //{
            //    //string sSQL = MakeSQL_Summary(buid, sParam);
            //    //DateTime dStratDate = DateTime.Today;
            //    //DateTime dEndDate = DateTime.Today;
            //    //int nPartID = 0;
            //    //int nDateCompare = 0;
            //    //int nDivisionID = 0;
            //    //int nRefID = 0;
            //    if (!string.IsNullOrEmpty(sParam))
            //    {
            //        oExportFollowup.BUID = buid;
            //        //??? nDateCompare = Convert.ToInt32(sParam.Split('~')[0]);
            //        oExportFollowup.StartDate = Convert.ToDateTime(sParam.Split('~')[1]);
            //        oExportFollowup.EndDate = Convert.ToDateTime(sParam.Split('~')[2]);
            //        oExportFollowup.Part = Convert.ToInt32(sParam.Split('~')[3]);
            //        oExportFollowup.nReportType = Convert.ToInt32(sParam.Split('~')[4]);
            //        //nRefID = Convert.ToInt32(sParam.Split('~')[5]); //ForLayout-3
            //    }
            //    //oExportFollowups = ExportFollowup.Gets_Details(oExportFollowup, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //}
            //catch (Exception e)
            //{
                
            //}
            ViewBag.Param = sParam;
            ViewBag.Buid = buid;
            return View(oExportFollowups);
        }


        [HttpPost]
        public JsonResult GetsExportFollowupSummary(ExportFollowup oExportFollowup)
        
        {
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            try
            {
                if (!string.IsNullOrEmpty(oExportFollowup.Params))
                {
                    //oExportFollowup.BUID = buid;
                    //??? nDateCompare = Convert.ToInt32(sParam.Split('~')[0]);
                    oExportFollowup.StartDate = Convert.ToDateTime(oExportFollowup.Params.Split('~')[1]);
                    oExportFollowup.EndDate = Convert.ToDateTime(oExportFollowup.Params.Split('~')[2]);
                    oExportFollowup.Part = Convert.ToInt32(oExportFollowup.Params.Split('~')[3]);
                    oExportFollowup.nReportType = Convert.ToInt32(oExportFollowup.Params.Split('~')[4]);
                    //nRefID = Convert.ToInt32(sParam.Split('~')[5]); //ForLayout-3
                }
                oExportFollowups = ExportFollowup.Gets_Details(oExportFollowup, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception e)
            {

            }
            var jsonResult = Json(oExportFollowups, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        #region Report
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
        public ActionResult PrintExportFollowupSummery(String sTemp)
        {
            string sDateRange = "";
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            List<ExportFollowup> oExportFollowups_ExportBill = new List<ExportFollowup>();

            int nDateCompare = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            int BUID = Convert.ToInt32(sTemp.Split('~')[3]);

            
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing  to Print");
            }
            else
            {

                //oExportFollowups = ExportFollowup.GetsExportFollowup(dtStartDate, dtEndDate.AddDays(1),((User)Session[SessionInfo.CurrentUser]).UserID);

                if (nDateCompare > 0)
                {
                    if (nDateCompare == 1)
                    {
                        sDateRange = "Date : " + dtStartDate.ToString("dd MMM yyyy");
                        oExportFollowups = ExportFollowup.GetsExportFollowup(BUID, dtStartDate, dtStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                    if (nDateCompare == 5)
                    {
                        sDateRange = "Date : " + dtStartDate.ToString("dd MMM yyyy") + "--to--" + dtEndDate.ToString("dd MMM yyyy");
                        oExportFollowups = ExportFollowup.GetsExportFollowup(BUID, dtStartDate, dtEndDate.AddDays(1), ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }

            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptExportFollowup oReport = new rptExportFollowup();
            byte[] abytes = oReport.PrepareReport(oExportFollowups, oCompany, oBusinessUnit, sDateRange);
            return File(abytes, "application/pdf");
        }
        [HttpPost]
        public JsonResult SetPrintingData(List<SelectedField> oCols)
        {
            List<SelectedField> oSelectedFields = new List<SelectedField>();
            oSelectedFields = oCols;
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSelectedFields);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oSelectedFields);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintExportFollowupDetail(int buid, string sSearchStr)
        {
            ExportFollowup oExportFollowup = new ExportFollowup();
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            List<SelectedField> oSelectedFields = new List<SelectedField>();
            oSelectedFields = (List<SelectedField>)Session[SessionInfo.ParamObj];
            
            if (!string.IsNullOrEmpty(sSearchStr))
            {
                oExportFollowup.BUID = buid;
                //??? nDateCompare = Convert.ToInt32(sParam.Split('~')[0]);
                oExportFollowup.StartDate = Convert.ToDateTime(sSearchStr.Split('~')[1]);
                oExportFollowup.EndDate = Convert.ToDateTime(sSearchStr.Split('~')[2]);
                oExportFollowup.Part = Convert.ToInt32(sSearchStr.Split('~')[3]);
                oExportFollowup.nReportType = Convert.ToInt32(sSearchStr.Split('~')[4]);
                //nRefID = Convert.ToInt32(sParam.Split('~')[5]); //ForLayout-3

                oExportFollowups = ExportFollowup.Gets_Details(oExportFollowup, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                throw new Exception("Nothing  to Print");
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sDateRange = "Date : " + oExportFollowup.StartDate.ToString("dd MMM yyyy") + "--to--" + oExportFollowup.EndDate.ToString("dd MMM yyyy");

            rptExportFollowup oReport = new rptExportFollowup();
            byte[] abytes = oReport.PrepareReportForDetail(oExportFollowups, oSelectedFields, oCompany, oBusinessUnit, sDateRange, oExportFollowup.Part);
            return File(abytes, "application/pdf");
        }

        public void PrintExportFollowupDetailExcel(int buid, string sSearchStr)
        {
            string msg = "";
            ExportFollowup oExportFollowup = new ExportFollowup();
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            List<SelectedField> oSelectedFields = new List<SelectedField>();
            oSelectedFields = (List<SelectedField>)Session[SessionInfo.ParamObj];
            if (!string.IsNullOrEmpty(sSearchStr))
            {
                oExportFollowup.BUID = buid;
                //??? nDateCompare = Convert.ToInt32(sParam.Split('~')[0]);
                oExportFollowup.StartDate = Convert.ToDateTime(sSearchStr.Split('~')[1]);
                oExportFollowup.EndDate = Convert.ToDateTime(sSearchStr.Split('~')[2]);
                oExportFollowup.Part = Convert.ToInt32(sSearchStr.Split('~')[3]);
                oExportFollowup.nReportType = Convert.ToInt32(sSearchStr.Split('~')[4]);
                //nRefID = Convert.ToInt32(sParam.Split('~')[5]); //ForLayout-3

                _oExportFollowups = ExportFollowup.Gets_Details(oExportFollowup, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                throw new Exception("Nothing  to Print");
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string sDateRange = "Date : " + oExportFollowup.StartDate.ToString("dd MMM yyyy") + "--to--" + oExportFollowup.EndDate.ToString("dd MMM yyyy");

            int nPartID = oExportFollowup.Part;
            #region Report Body & Header
            if (nPartID == 4)
            {
                msg = "ExportPI Issue Followup Report";
                
            }
            else if (nPartID == 5)
            {
                msg = "ExportLC Receive Followup Report";
                
            }
            else if (nPartID == 6)
            {
                msg = "Invoice Create Followup Report";
                
            }
            else if (nPartID == 7)
            {
                msg = "Send To Party Followup Report";
                
            }
            else if (nPartID == 8)
            {
                msg = "Submit To Bank Followup Report";
                
            }
            else if (nPartID == 9)
            {
                msg = "Maturity Receive Followup Report";
                
            }
            else if (nPartID == 10)
            {
                msg = "Bill Discounted Followup Report";
                
            }
            else
            {
                msg = "Export Followup Report";
                
            }

            this.PrintBodyDetailExcel(msg, oSelectedFields, sDateRange, oBusinessUnit);

            #endregion
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        { 
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber)
            {
                if (sVal == "-")
                {
                    sVal = "0";
                }
                cell.Value = Convert.ToDouble(sVal);
            }
            else
            {
                cell.Value = sVal;
            }
            
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = Formatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        private void PrintBodyDetailExcel(string sReportHeader, List<SelectedField> oCols, string sDateRange, BusinessUnit oBusinessUnit)
        {
            List<TableHeader> table_header = new List<TableHeader>();
            List<SelectedField> _oSelectedFields = new List<SelectedField>();
            _oSelectedFields = oCols;
            var nSum = new Hashtable();
            table_header.Add(new TableHeader { Header = "SL", Width = 10f, IsRotate = false });



            foreach (SelectedField item in _oSelectedFields)
            {
                if (item.Sum == 1)
                    nSum[item.Column] = 0.00;
                table_header.Add(new TableHeader { Header = item.FieldName, Width = (float)item.Width, IsRotate = false });
            }



            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export FollowUp Report");
                sheet.Name = "Export_FollowUp_Report";

                foreach (TableHeader listItem in table_header)
                {
                    sheet.Column(nStartCol++).Width = listItem.Width;
                }

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                
                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sDateRange; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Phone + ";  " + oBusinessUnit.Email + ";  " + oBusinessUnit.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;

                nRowIndex++;
                nStartCol = 2;

                foreach (TableHeader listItem in table_header)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                nRowIndex += 1;
                int nCount = 0;
                double nQty = 0, nQtyDelivery = 0, nAmount = 0, nAmountDelivery = 0, nQtyYetToD = 0, nAmountYetToD = 0;
                var totalStartCol = 0;
                bool IsNumber = false;
                foreach (ExportFollowup oItem in _oExportFollowups)
                {

                    nStartCol = 1;
                    nCount = nCount + 1;
                    FillCell(sheet, nRowIndex, ++nStartCol, nCount.ToString(), false);



                    foreach (SelectedField item in _oSelectedFields)
                    {
                        if (item.Datatype == "Double")
                            IsNumber = true;
                        else
                            IsNumber = false;

                        var propertyValue = oItem.GetType().GetProperty(item.Column).GetValue(oItem, null);

                        if (item.Format == 1 || item.Format == 0)
                            Formatter = "#,##0.00";
                        else if (item.Format == 2)
                            Formatter = oItem.Currency + "#,##0.00";
                        
                        FillCell(sheet, nRowIndex, ++nStartCol, propertyValue.ToString(), IsNumber);

                        if (item.Sum == 1)
                        {
                            if (totalStartCol == 0)
                                totalStartCol = nStartCol-3;
                            nSum[item.Column] = (double)nSum[item.Column] + (double)propertyValue;
                        }
                            

                    }


                    
                    nRowIndex++;
                         


                }
                nStartCol = 2;
                var grn = table_header.Count - nSum.Count;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, nStartCol += totalStartCol, true, ExcelHorizontalAlignment.Right);

                Formatter = "#,##0.00";



                foreach (SelectedField item in _oSelectedFields)
                {

                    if (item.Sum == 1)
                    {
                        double value = (double)nSum[item.Column];
                        FillCell(sheet, nRowIndex, ++nStartCol, value.ToString(), true);
                    }
                    
                }

                while (nStartCol <= nEndCol)
                {
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                }



                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Export_FollowUp_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private void PrintBodyDetailLCReceive(string sReportHeader, string sDateRange, BusinessUnit oBusinessUnit)
        {
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "SL", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "LC No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "LC Date", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Party Name", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "MKTP Name", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty(D)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount(D)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty (YetToD)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amt (YetToD)", Width = 20f, IsRotate = false });


            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export FollowUp Report");
                sheet.Name = "Export_FollowUp_Report";

                foreach (TableHeader listItem in table_header)
                {
                    sheet.Column(nStartCol++).Width = listItem.Width;
                }
                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sDateRange; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Phone + ";  " + oBusinessUnit.Email + ";  " + oBusinessUnit.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;

                nRowIndex++;
                nStartCol = 2;

                foreach (TableHeader listItem in table_header)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                nRowIndex += 1;
                int nCount = 0;
                double nQty = 0, nQtyDelivery = 0, nAmount = 0, nAmountDelivery = 0, nQtyYetToD = 0, nAmountYetToD = 0;
                foreach (ExportFollowup oItem in _oExportFollowups)
                {

                    nStartCol = 1;
                    nCount = nCount + 1;
                    FillCell(sheet, nRowIndex, ++nStartCol, nCount.ToString(), false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ExportLCNo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.LCOpeningDateStr, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.PINo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ContractorName, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.MKTPName, false);
                    Formatter = "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Qty.ToString(), true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DeliveryQty.ToString(), true);
                    Formatter = oItem.Currency + "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Amount.ToString(), true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DeliveryValue.ToString(), true);
                    Formatter = "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.YetToDeliveryQty.ToString(), true);
                    Formatter = oItem.Currency + "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.YetToDeliveryValue.ToString(), true);
                    nRowIndex++;

                    nQty = nQty + oItem.Qty;
                    nQtyDelivery = nQtyDelivery + oItem.DeliveryQty;
                    nAmount = nAmount + oItem.Amount;
                    nAmountDelivery = nAmountDelivery + oItem.DeliveryValue;
                    nQtyYetToD = nQtyYetToD + oItem.YetToDeliveryQty;
                    nAmountYetToD = nAmountYetToD + oItem.YetToDeliveryValue;
                }

                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, nStartCol += 5, true, ExcelHorizontalAlignment.Right);

                Formatter = "#,##0.00";
                FillCell(sheet, nRowIndex, ++nStartCol, nQty.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nQtyDelivery.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmount.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmountDelivery.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nQtyYetToD.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmountYetToD.ToString(), true);


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Export_FollowUp_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private void PrintBodyDetailInvoiceCreate(string sReportHeader, string sDateRange, BusinessUnit oBusinessUnit)
        {
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "SL", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bill No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bill Date", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "LC No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Party Name", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "MKTP Name", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Issue", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Nego", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty(D)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount(D)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty (YetToD)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amt (YetToD)", Width = 20f, IsRotate = false });


            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export FollowUp Report");
                sheet.Name = "Export_FollowUp_Report";

                foreach (TableHeader listItem in table_header)
                {
                    sheet.Column(nStartCol++).Width = listItem.Width;
                }

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sDateRange; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Phone + ";  " + oBusinessUnit.Email + ";  " + oBusinessUnit.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;

                nRowIndex++;
                nStartCol = 2;

                foreach (TableHeader listItem in table_header)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                nRowIndex += 1;
                int nCount = 0;
                double nQty = 0, nQtyDelivery = 0, nAmount = 0, nAmountDelivery = 0, nQtyYetToD = 0, nAmountYetToD = 0;
                foreach (ExportFollowup oItem in _oExportFollowups)
                {

                    nStartCol = 1;
                    nCount = nCount + 1;
                    FillCell(sheet, nRowIndex, ++nStartCol, nCount.ToString(), false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ExportBillNo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.StartDateStr, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ExportLCNo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.PINo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ContractorName, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.MKTPName, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.SName_Issue, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.SName_Nego, false);
                    Formatter = "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Qty.ToString(), true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DeliveryQty.ToString(), true);
                    Formatter = oItem.Currency + "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Amount.ToString(), true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DeliveryValue.ToString(), true);
                    Formatter = "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.YetToDeliveryQty.ToString(), true);
                    Formatter = oItem.Currency + "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.YetToDeliveryValue.ToString(), true);
                    nRowIndex++;

                    nQty = nQty + oItem.Qty;
                    nQtyDelivery = nQtyDelivery + oItem.DeliveryQty;
                    nAmount = nAmount + oItem.Amount;
                    nAmountDelivery = nAmountDelivery + oItem.DeliveryValue;
                    nQtyYetToD = nQtyYetToD + oItem.YetToDeliveryQty;
                    nAmountYetToD = nAmountYetToD + oItem.YetToDeliveryValue;
                }

                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);

                Formatter = "#,##0.00";
                FillCell(sheet, nRowIndex, ++nStartCol, nQty.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nQtyDelivery.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmount.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmountDelivery.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nQtyYetToD.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmountYetToD.ToString(), true);


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Export_FollowUp_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private void PrintBodyDetailSendToPartyAndBank(string sReportHeader, string sObjectPropertyName, string sColumnName, string sDateRange, BusinessUnit oBusinessUnit)
        {
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "SL", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bill No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bill Date", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = sColumnName, Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Party Name", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "MKTP Name", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Issue", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Nego", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty(D)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount(D)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty (YetToD)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amt (YetToD)", Width = 20f, IsRotate = false });


            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export FollowUp Report");
                sheet.Name = "Export_FollowUp_Report";

                foreach (TableHeader listItem in table_header)
                {
                    sheet.Column(nStartCol++).Width = listItem.Width;
                }

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sDateRange; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Phone + ";  " + oBusinessUnit.Email + ";  " + oBusinessUnit.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;

                nRowIndex++;
                nStartCol = 2;
                foreach (TableHeader listItem in table_header)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                nRowIndex += 1;
                int nCount = 0;
                double nQty = 0, nQtyDelivery = 0, nAmount = 0, nAmountDelivery = 0, nQtyYetToD = 0, nAmountYetToD = 0;
                foreach (ExportFollowup oItem in _oExportFollowups)
                {

                    nStartCol = 1;
                    nCount = nCount + 1;
                    FillCell(sheet, nRowIndex, ++nStartCol, nCount.ToString(), false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ExportBillNo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.StartDateStr, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, ""+oItem.GetType().GetProperty(sObjectPropertyName).GetValue(oItem, null), false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.PINo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ContractorName, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.MKTPName, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.SName_Issue, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.SName_Nego, false);
                    Formatter = "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Qty.ToString(), true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DeliveryQty.ToString(), true);
                    Formatter = oItem.Currency + "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Amount.ToString(), true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DeliveryValue.ToString(), true);
                    Formatter = "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.YetToDeliveryQty.ToString(), true);
                    Formatter = oItem.Currency + "#,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.YetToDeliveryValue.ToString(), true);
                    nRowIndex++;

                    nQty = nQty + oItem.Qty;
                    nQtyDelivery = nQtyDelivery + oItem.DeliveryQty;
                    nAmount = nAmount + oItem.Amount;
                    nAmountDelivery = nAmountDelivery + oItem.DeliveryValue;
                    nQtyYetToD = nQtyYetToD + oItem.YetToDeliveryQty;
                    nAmountYetToD = nAmountYetToD + oItem.YetToDeliveryValue;
                }

                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                Formatter = "#,##0.00";
                FillCell(sheet, nRowIndex, ++nStartCol, nQty.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nQtyDelivery.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmount.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmountDelivery.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nQtyYetToD.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmountYetToD.ToString(), true);


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Export_FollowUp_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        private void PrintBodyDetailMaturituyAndDiscount(string sReportHeader, string sObjectPropertyName, string sColumnName, string sDateRange, BusinessUnit oBusinessUnit)
        {
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "SL", Width = 10f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bill No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bill Date", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = sColumnName, Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "LDBC No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "PI No", Width = 15f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Party Name", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "MKTP Name", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Issue", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Nego", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty(D)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amount(D)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Qty (YetToD)", Width = 20f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Amt (YetToD)", Width = 20f, IsRotate = false });


            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export FollowUp Report");
                sheet.Name = "Export_FollowUp_Report";

                foreach (TableHeader listItem in table_header)
                {
                    sheet.Column(nStartCol++).Width = listItem.Width;
                }

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sReportHeader; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = sDateRange; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;
                nRowIndex += 1;

                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nStartCol += table_header.Count - 6]; cell.Merge = true;
                cell.Value = oBusinessUnit.Phone + ";  " + oBusinessUnit.Email + ";  " + oBusinessUnit.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                nStartCol = 2;

                cell = sheet.Cells[nRowIndex, nStartCol += table_header.Count - 5, nRowIndex, table_header.Count + 1]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                nStartCol = 2;

                nRowIndex++;
                nStartCol = 2;

                foreach (TableHeader listItem in table_header)
                {
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }
                nRowIndex += 1;
                int nCount = 0;
                double nQty = 0, nQtyDelivery = 0, nAmount = 0, nAmountDelivery = 0, nQtyYetToD = 0, nAmountYetToD = 0;
                foreach (ExportFollowup oItem in _oExportFollowups)
                {

                    nStartCol = 1;
                    nCount = nCount + 1;
                    FillCell(sheet, nRowIndex, ++nStartCol, nCount.ToString(), false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ExportBillNo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.StartDateStr, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "" + oItem.GetType().GetProperty(sObjectPropertyName).GetValue(oItem, null), false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.LDBCNo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.PINo, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.ContractorName, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.MKTPName, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.SName_Issue, false);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.SName_Nego, false);
                    Formatter = " #,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Qty.ToString(), true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DeliveryQty.ToString(), true);
                    Formatter = oItem.Currency + " #,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.Amount.ToString(), true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.DeliveryValue.ToString(), true);
                    Formatter = " #,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.YetToDeliveryQty.ToString(), true);
                    Formatter = oItem.Currency + " #,##0.00";
                    FillCell(sheet, nRowIndex, ++nStartCol, oItem.YetToDeliveryValue.ToString(), true);
                    nRowIndex++;

                    nQty = nQty + oItem.Qty;
                    nQtyDelivery = nQtyDelivery + oItem.DeliveryQty;
                    nAmount = nAmount + oItem.Amount;
                    nAmountDelivery = nAmountDelivery + oItem.DeliveryValue;
                    nQtyYetToD = nQtyYetToD + oItem.YetToDeliveryQty;
                    nAmountYetToD = nAmountYetToD + oItem.YetToDeliveryValue;
                }

                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right);
                Formatter = " #,##0.00";
                FillCell(sheet, nRowIndex, ++nStartCol, nQty.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nQtyDelivery.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmount.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmountDelivery.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nQtyYetToD.ToString(), true);
                FillCell(sheet, nRowIndex, ++nStartCol, nAmountYetToD.ToString(), true);


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Export_FollowUp_Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion

        #region Statement Details
   
        private string GetDateSearch(int nDateCompare, string sDateType, DateTime dDateStart, DateTime dDateEnd) 
        {
            string sReturn = "";
            #region Date Criteria
            if (nDateCompare > 0)
            {
                if (nDateCompare == 1)
                {
                    dDateEnd = dDateStart;
                }
                sReturn =" AND "+ sDateType + " BETWEEN '" + dDateStart.ToString("dd MMM yyy") + "' AND '" + dDateEnd.ToString("dd MMM yyy") + "' ";
            }
            #endregion
            return sReturn;
        }
        #endregion

        #region Export Bill Report
        [HttpPost]
        public JsonResult GetsGridData(ExportFollowup oExportFollowup)
        {
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            List<ExportFollowup> oExportFollowups_Temp = new List<ExportFollowup>();

            int tabId = Convert.ToInt32(oExportFollowup.nReportType);
            int nDateCompare = Convert.ToInt32(oExportFollowup.Params.Split('~')[0]);
            DateTime dtStartDate = Convert.ToDateTime(oExportFollowup.Params.Split('~')[1]);
            DateTime dtEndDate = Convert.ToDateTime(oExportFollowup.Params.Split('~')[2]);
            int BUID = Convert.ToInt32(oExportFollowup.Params.Split('~')[3]);

            try
            {
                if (tabId == 1)//UnitInfo
                {
                    if (nDateCompare > 0)
                    {
                        if (nDateCompare == 1)
                        {
                            oExportFollowups = ExportFollowup.GetsExportFollowup(BUID, dtStartDate, dtStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        else if (nDateCompare == 5)
                        {
                            oExportFollowups = ExportFollowup.GetsExportFollowup(BUID, dtStartDate, dtEndDate.AddDays(1), ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
                if (tabId == 2)
                {
                    oExportFollowups = ExportFollowup.Gets_Summary(BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oExportFollowups.Count > 1) 
                    {
                        ExportFollowup oExportFollowup_Temp = new ExportFollowup();
                        oExportFollowup_Temp.BUName = "";
                        oExportFollowup_Temp.BankName = "GRAND TOTAL:";
                        oExportFollowup_Temp.Amount_LC = oExportFollowups.Sum(x => x.Amount_LC);
                        oExportFollowup_Temp.BOinHand = oExportFollowups.Sum(x => x.BOinHand); ;
                        oExportFollowup_Temp.BOInCusHand = oExportFollowups.Sum(x => x.BOInCusHand); ;
                        oExportFollowup_Temp.AcceptadBill = oExportFollowups.Sum(x => x.AcceptadBill); ;
                        oExportFollowup_Temp.NegoTransit = oExportFollowups.Sum(x => x.NegoTransit); ;
                        oExportFollowup_Temp.NegotiatedBill = oExportFollowups.Sum(x => x.NegotiatedBill); ;
                        oExportFollowup_Temp.Amount_Due = oExportFollowups.Sum(x => x.Amount_Due); ;
                        oExportFollowup_Temp.Amount_ODue = oExportFollowups.Sum(x => x.Amount_ODue); ;
                        oExportFollowup_Temp.Discounted = oExportFollowups.Sum(x => x.Discounted); ;
                        oExportFollowup_Temp.PaymentDone = oExportFollowups.Sum(x => x.PaymentDone); ;
                        oExportFollowup_Temp.BFDDRecd = oExportFollowups.Sum(x => x.BFDDRecd);
                        oExportFollowups.Add(oExportFollowup_Temp);
                    }
                }
                if (tabId == 3)
                {
                    if (nDateCompare > 0)
                    {
                        if (nDateCompare == 1)
                        {
                            oExportFollowups = ExportFollowup.Gets_BillRealize(BUID, dtStartDate, dtStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }

                        if (nDateCompare == 5)
                        {
                            oExportFollowups = ExportFollowup.Gets_BillRealize(BUID, dtStartDate, dtEndDate.AddDays(1), ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
                if (tabId == 4)//Maturity
                {
                    if (nDateCompare > 0)
                    {
                        if (nDateCompare == 1)
                        {
                            oExportFollowups = ExportFollowup.Gets_BillMaturity(BUID, dtStartDate, dtEndDate, 0);
                        }

                        if (nDateCompare == 5)
                        {
                            oExportFollowups = ExportFollowup.Gets_BillMaturity(BUID, dtStartDate, dtEndDate.AddDays(1), ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oExportFollowups = new List<ExportFollowup>();
                oExportFollowup.ErrorMessage = ex.Message;
                oExportFollowups.Add(oExportFollowup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportFollowups);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
  
    
        #endregion

       
    }
}