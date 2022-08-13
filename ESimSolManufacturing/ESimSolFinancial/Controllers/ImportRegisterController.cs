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
using System.Diagnostics;

namespace ESimSolFinancial.Controllers
{
	public class ImportRegisterController : Controller
	{
		#region Declaration
        ImportRegister _oImportRegister = new ImportRegister();
        List<ImportRegister> _oImportRegisters = new List<ImportRegister>();
		#endregion

		#region Actions
		public ActionResult ViewImportRegisters(int menuid, int buid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            List<ImportRegister> oImportRegisters = new List<ImportRegister>();
            ViewBag.InvoiceTypes = EnumObject.jGets(typeof(EnumImportPIType));
            ViewBag.ProductTypes = EnumObject.jGets(typeof(EnumProductNature));
            ViewBag.ImportRegisters = oImportRegisters;
            ViewBag.BUID = buid;

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
			return View(_oImportRegister);
		}
        #endregion
        public JsonResult AdvanceSearch(ImportRegister oImportRegister)
        {
            List<ImportRegister> oImportRegisters = new List<ImportRegister>();
            try
            {
                oImportRegister = MakeObject(oImportRegister.ErrorMessage);
                string sSQL = MakeSQLForReport(oImportRegister);
                oImportRegisters = oImportRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportRegister.ErrorMessage = ex.Message;
                oImportRegisters.Add(oImportRegister);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportRegisters);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private ImportRegister MakeObject(string sString)
        {
            ImportRegister oImportRegister = new ImportRegister();
            if (!String.IsNullOrEmpty(sString))
            {
                int nCount = 0;
                oImportRegister.Params = sString;
                oImportRegister.BUID = Convert.ToInt32(sString.Split('~')[nCount++]);
                oImportRegister.SupplierName = Convert.ToString(sString.Split('~')[nCount++]);
                oImportRegister.ImportLCNo = Convert.ToString(sString.Split('~')[nCount++]);
                oImportRegister.ImportPINo = Convert.ToString(sString.Split('~')[nCount++]);
                oImportRegister.ImportInvoiceNo = Convert.ToString(sString.Split('~')[nCount++]);
                oImportRegister.InvoiceType = (EnumImportPIType)Convert.ToInt32(sString.Split('~')[nCount++]);
                oImportRegister.ProductType = (EnumProductNature)Convert.ToInt32(sString.Split('~')[nCount++]);
            }
            return oImportRegister;
        }
        private string MakeSQLForReport(ImportRegister oImportRegister)
        {
            string sReturn1 = "Select * from View_ImportRegister";
            string sReturn = "";
            string sParams = oImportRegister.Params;
            int cboPIDate = 0;
            DateTime PIStartDate = DateTime.Today;
            DateTime PIEndDate = DateTime.Today;

            #region BUID
            if (oImportRegister.BUID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + oImportRegister.BUID;
            }
            #endregion
            #region Party Name
            if (!string.IsNullOrEmpty(oImportRegister.SupplierName) && oImportRegister.SupplierName != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SupplierID IN (" + oImportRegister.SupplierName + ")";
            }
            #endregion
            #region LC No
            if (!string.IsNullOrEmpty(oImportRegister.ImportLCNo) && oImportRegister.ImportLCNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportLCNo LIKE '%" + oImportRegister.ImportLCNo + "%'";
            }
            #endregion
            #region Import PI No
            if (!string.IsNullOrEmpty(oImportRegister.ImportPINo) && oImportRegister.ImportPINo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPINo LIKE '%" + oImportRegister.ImportPINo + "%'";
            }
            #endregion
            #region Import Invoice No
            if (!string.IsNullOrEmpty(oImportRegister.ImportInvoiceNo) && oImportRegister.ImportInvoiceNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportInvoiceNo LIKE '%" + oImportRegister.ImportInvoiceNo + "%'";
            }
            #endregion
            #region Get Value From Param (Date Search)
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 7;
                cboPIDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                PIStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                PIEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
            }
            #endregion
            #region PI DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " PIDate", cboPIDate, PIStartDate, PIEndDate);
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ImportRegister oImportRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void GetExcel(string sParam)
        {
            ImportRegister oImportRegister = new ImportRegister();
            oImportRegister = (ImportRegister)Session[SessionInfo.ParamObj];
            oImportRegister = MakeObject(oImportRegister.Params);
            string sSQL = MakeSQLForReport(oImportRegister);
            List<ImportRegister> oImportRegisters = new List<ImportRegister>();
            oImportRegisters = oImportRegister.Gets(sSQL + " order by ImportLCID, ImportPIID, ImportInvoiceNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "PI No", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Commodity /Description", Width = 40f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI Quantity", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "PI Amount", Width = 20f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Party Name", Width = 40f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "L/C No", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "L/C Date", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Sales Cont. No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Sales Cont. Date", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "L/C Advising Bank", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "L/C Amount", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "INV. No.", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "INV. Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "INV. Value", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Due Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Bill Of Loading No & Date", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Bill Of Entrt No & Date", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "GRN Quantity", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "GRN Date", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Acceptence Date", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Maturity Value", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Maturity Date", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Payment Date", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Import Register";
                var sheet = excelPackage.Workbook.Worksheets.Add("Import Register");
                sheet.Name = "Import Register";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 25;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Import Register"; cell.Style.Font.Bold = true;
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
                nEndCol = table_header.Count() + nStartCol;

                int nRowSpan = 0;
                int nSL = 1;



                int nImportLCID = 0;
                int nImportPIID = 0;
                string sImportInvoiceNo = "";

                foreach (var obj in oImportRegisters)
                {
                    nStartCol = 2;

                    ExcelTool.Formatter = "";
                    if (nImportLCID != obj.ImportLCID)
                    {
                        int nTempRowSpan = oImportRegisters.Where(x => x.ImportLCID.Equals(obj.ImportLCID)).Count() - 1;
                        ExcelTool.FillCellMerge(ref sheet, nSL++.ToString(), nRowIndex, nRowIndex + nTempRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                    }
                    else
                    {
                        nStartCol++;
                    }
                    if(nImportPIID!=obj.ImportPIID)
                    {
                        int nTempRowSpan = oImportRegisters.Where(x => x.ImportPIID.Equals(obj.ImportPIID)).Count() - 1;
                        ExcelTool.FillCellMerge(ref sheet, obj.ImportPINo, nRowIndex, nRowIndex + nTempRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ImportPIDateSt, nRowIndex, nRowIndex + nTempRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);                    
                    }
                    else
                    {
                        nStartCol = nStartCol+2;
                    }
                    ExcelTool.FillCellMerge(ref sheet, obj.ProductName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.PIQtySt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.PIAmountSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.PartyName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                    if(nImportLCID != obj.ImportLCID)
                    {
                        int nTempRowSpan = oImportRegisters.Where(x => x.ImportLCID.Equals(obj.ImportLCID)).Count() - 1;
                        ExcelTool.FillCellMerge(ref sheet, obj.ImportLCNo, nRowIndex, nRowIndex + nTempRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ImportLCDateSt, nRowIndex, nRowIndex + nTempRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    }
                    else
                    {
                        nStartCol = nStartCol + 2;
                    }
                    ExcelTool.FillCellMerge(ref sheet, obj.ImportPINo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.ImportPIDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.IssueBankName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.LCAmountSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.ImportInvoiceNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.ImportInvoiceDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.InvoiceValueSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.InvoiceDueValueSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.BillofLoadingDateSt + "/" +obj.BillofLoadingNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.BillofEntrtDateSt + "/" + obj.BillOfEntrtNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.GoodsRcvQtySt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.GoodsRcvDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.AcceptanceDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.MaturityValueSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.MaturityDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);
                    ExcelTool.FillCellMerge(ref sheet, obj.PaymentDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Center, ExcelVerticalAlignment.Top, false);

                    nRowIndex++;
                    nImportLCID = obj.ImportLCID;
                    nImportPIID = obj.ImportPIID;
                    sImportInvoiceNo = obj.ImportInvoiceNo;
                }
                //#region Grand Total
                //double dGrandTotal = oImportRegisters.Sum(x => x.Qty);
                //nStartCol = 2;
                //ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 18, true, ExcelHorizontalAlignment.Right, true);
                //ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 19, 19, true, ExcelHorizontalAlignment.Right, true);
                //#endregion


                #endregion
                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ImportRegister Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
    }
}
