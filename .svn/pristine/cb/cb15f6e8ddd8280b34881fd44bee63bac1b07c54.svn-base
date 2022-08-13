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
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
	public class FARegisterSummeryController : Controller
	{
		#region Declaration

		FARegisterSummery _oFARegisterSummery = new FARegisterSummery();
		List<FARegisterSummery> _oFARegisterSummerys = new  List<FARegisterSummery>();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewFARegisterSummery(string Param, int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			//this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'FARegisterSummery'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oFARegisterSummerys = new List<FARegisterSummery>();
            if(!string.IsNullOrEmpty(Param))
            {
                string sBUIDs = Param.Split('~')[0];
                DateTime dStartDate = Convert.ToDateTime(Param.Split('~')[1]);
                DateTime dEndDate = Convert.ToDateTime(Param.Split('~')[2]);
                int nProductCategoryID = Convert.ToInt32(Param.Split('~')[3]);
                int nLayout = Convert.ToInt32(Param.Split('~')[4]);//1:category wise;2:Product wise
                _oFARegisterSummerys = FARegisterSummery.Gets(sBUIDs, dStartDate, dEndDate, nProductCategoryID, nLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
			return View(_oFARegisterSummerys);
		}

	#endregion

        #region Search
        public JsonResult Search(FARegisterSummery oFARegisterSummery)
        {
            List<FARegisterSummery> oFARegisterSummerys = new List<FARegisterSummery>();
            FARegisterSummery _oFARegisterSummery = new FARegisterSummery();
            string sBUIDs = oFARegisterSummery.Param.Split('~')[0];
            DateTime dStartDate = Convert.ToDateTime(oFARegisterSummery.Param.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oFARegisterSummery.Param.Split('~')[2]);
            int nProductCategoryID= Convert.ToInt32(oFARegisterSummery.Param.Split('~')[3]);
            int nLayout = Convert.ToInt32(oFARegisterSummery.Param.Split('~')[4]);//1:category wise;2:Product wise
            oFARegisterSummerys = new List<FARegisterSummery>();
            oFARegisterSummerys = FARegisterSummery.Gets(sBUIDs, dStartDate, dEndDate, nProductCategoryID, nLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);
            var jsonResult = Json(oFARegisterSummerys, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Print
        public ActionResult PrintFARegisterSummerys(string Param)
        {
            string sBUIDs = Param.Split('~')[0];
            DateTime dStartDate = Convert.ToDateTime(Param.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(Param.Split('~')[2]);
            int nProductCategoryID = Convert.ToInt32(Param.Split('~')[3]);
            int nLayout = Convert.ToInt32(Param.Split('~')[4]);//1:category wise;2:Product wise
            _oFARegisterSummerys = FARegisterSummery.Gets(sBUIDs, dStartDate, dEndDate, nProductCategoryID, nLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(Convert.ToInt32(sBUIDs.Split(',')[0]), ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptFARegisterSummery oReport = new rptFARegisterSummery();
            byte[] abytes = oReport.PrepareReport(_oFARegisterSummerys, oBusinessUnit, oCompany, Param);
            return File(abytes, "application/pdf");

        }

        public void PrintFARegisterSummeryInXL(string Param)
        {
            string sBUIDs = Param.Split('~')[0];
            DateTime dStartDate = Convert.ToDateTime(Param.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(Param.Split('~')[2]);
            int nProductCategoryID = Convert.ToInt32(Param.Split('~')[3]);
            int nLayout = Convert.ToInt32(Param.Split('~')[4]);//1:category wise;2:Product wise
            _oFARegisterSummerys = FARegisterSummery.Gets(sBUIDs, dStartDate, dEndDate, nProductCategoryID, nLayout, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(Convert.ToInt32(sBUIDs.Split(',')[0]), ((User)Session[SessionInfo.CurrentUser]).UserID);
             oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);

             #region Export To Excel
             int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
             ExcelRange cell;
             ExcelRange HeaderCell;
             ExcelFill fill;
             OfficeOpenXml.Style.Border border;
             using (var excelPackage = new ExcelPackage())
             {
                 excelPackage.Workbook.Properties.Author = "ESimSol";
                 excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                 var sheet = excelPackage.Workbook.Worksheets.Add("Fixed Asset Register Summery");
                 sheet.Name = "Fixed Asset Register Summery";
                 sheet.Column(2).Width = 5;//SL
                 sheet.Column(3).Width = 50;
                 sheet.Column(4).Width = 25;
                 sheet.Column(5).Width = 25;
                 sheet.Column(6).Width = 25;
                 sheet.Column(7).Width = 10;
                 sheet.Column(8).Width = 25;
                 sheet.Column(9).Width = 25;
                 sheet.Column(10).Width = 25;
                 sheet.Column(11).Width = 25;
                 nEndCol = 11;

                 #region Report Header
                 cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                 cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                 cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 nRowIndex = nRowIndex + 1;

                 cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                 cell.Value = "Schedule of Fixed Capital Assets as at " + Convert.ToDateTime(Param.Split('~')[2]).ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                 cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 nRowIndex = nRowIndex + 1;
                 #endregion

                 #region Column Header
                 nStartRow = nRowIndex;
                 cell = sheet.Cells[nRowIndex, 2, nRowIndex + 1, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Merge = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                 cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 cell = sheet.Cells[nRowIndex, 3, nRowIndex + 1, 3]; cell.Value = "Particulars"; cell.Style.Font.Bold = true; cell.Merge = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Merge = true;cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 cell = sheet.Cells[nRowIndex, 4, nRowIndex,6]; cell.Value = "COST"; cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Merge = true;cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 cell = sheet.Cells[nRowIndex, 7, nRowIndex+1,7]; cell.Value = "Rate"; cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Merge = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 cell = sheet.Cells[nRowIndex, 8,nRowIndex,10]; cell.Value = "Depraciation"; cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Merge = true;cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 cell = sheet.Cells[nRowIndex, 11, nRowIndex + 1, 11]; cell.Value = "Written Down Value as On \n" + dEndDate.ToString("dd.MM.yyyy");  cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Merge = true;cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 nRowIndex = nRowIndex + 1;


                 cell = sheet.Cells[nRowIndex, 4]; cell.Value = "AS On \n" + dStartDate.ToString("dd.MM.yyyy"); cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                 cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Addition During the Year"; cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;      cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                 cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Total AS on \n" + dEndDate.ToString("dd.MM.yyyy"); cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                 cell = sheet.Cells[nRowIndex, 8]; cell.Value = "AS On\n" + dStartDate.ToString("dd.MM.yyyy"); cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                 cell = sheet.Cells[nRowIndex, 9]; cell.Value = "During the Year"; cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                 cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Total AS on \n" + dEndDate.ToString("dd.MM.yyyy"); cell.Style.Font.Bold = true;
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 
                 HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 8];
                 nRowIndex = nRowIndex + 1;
                 #endregion

                 #region Report Data
                 int nCount = 0; int TempSubGroupHeadID = _oFARegisterSummerys[0].SubGroupHeadID;
                 Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                 string sStartCell = "", sEndCell = "", sFormula = "";
                 nStartRow = nRowIndex;
                 foreach (FARegisterSummery oItem in _oFARegisterSummerys)
                 {
                     if (TempSubGroupHeadID != oItem.SubGroupHeadID)
                     {
                         nCount = 0;//Reset
                         #region  Print subtotal
                         cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Sub-Total ="; cell.Style.Font.Bold = true; cell.Merge = true;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                         cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                         border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                         aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + nEndRow.ToString()));
                         for (int i = 4; i <= 11; i++)
                         {
                              sStartCell = Global.GetExcelCellName(nStartRow, i);
                              sEndCell = Global.GetExcelCellName(nEndRow, i);
                             if(i==7)
                             {
                                 cell = sheet.Cells[nRowIndex, i]; cell.Style.Font.Bold = false; cell.Value = "";
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                                 cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                             }
                             else
                             {
                                 cell = sheet.Cells[nRowIndex, i]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                                 cell.Style.Font.Bold = true;
                                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                                 cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                             }
                              
                         }
                         nRowIndex++;
                         #endregion
                         TempSubGroupHeadID = oItem.SubGroupHeadID;//set value
                         nStartRow = nRowIndex;    
                     }
                     nCount++;                    
                     cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; 
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.ReportViewLayout == 1 ? oItem.CategoryName : oItem.ProductName; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.AssetOpeningAmount; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.AssetAdditionAmount; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                     cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.TotalAssetAmount; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                     cell = sheet.Cells[nRowIndex, 7]; cell.Value = Global.MillionFormat(oItem.DeprRate)+"%"; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; 
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                     cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.DeprOpeningAmount; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.DeprAdditionAmount; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.TotalDeprAmount; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.EndingAssetAmount; cell.Style.Font.Bold = false;
                     border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                     cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                     border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                     nEndRow = nRowIndex;
                     nRowIndex++;
                 }

                 #region  Print Last subtotal
                 cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Sub-Total ="; cell.Style.Font.Bold = true; cell.Merge = true;
                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                 cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + nEndRow.ToString()));
                 for (int i = 4; i <= 11; i++)
                 {
                     sStartCell = Global.GetExcelCellName(nStartRow, i);
                     sEndCell = Global.GetExcelCellName(nEndRow, i);
                     if (i == 7)
                     {
                         cell = sheet.Cells[nRowIndex, i]; cell.Style.Font.Bold = false; cell.Value = "";
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                         cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                         border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                     }
                     else
                     {
                         cell = sheet.Cells[nRowIndex, i]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                         cell.Style.Font.Bold = true;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                         cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                         border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     }
                 }
                 nRowIndex++;
                 #endregion

                 #region Grand Total
                 cell = sheet.Cells[nRowIndex, 2, nRowIndex, 3]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true; cell.Merge = true;
                 border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                 cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                 border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                 
                 for (int i = 4; i <= 11; i++)
                 {
                     #region Formula
                     sFormula = "";
                     if (aGrandTotals.Count > 0)
                     {
                         sFormula = "SUM(";
                         for (int j = 1; j <= aGrandTotals.Count; j++)
                         {
                             nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[j.ToString()]).Split(',')[0]);
                             nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[j.ToString()]).Split(',')[1]);
                             sStartCell = Global.GetExcelCellName(nStartRow, i);
                             sEndCell = Global.GetExcelCellName(nEndRow, i);
                             sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                         }
                         if (sFormula.Length > 0)
                         {
                             sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                         }
                         sFormula = sFormula + ")";
                     }
                     else
                     {
                         sStartCell = Global.GetExcelCellName(nStartRow, i);
                         sEndCell = Global.GetExcelCellName(nEndRow, i);
                         sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                     }
                     #endregion
   
                     if (i == 7)
                     {
                         cell = sheet.Cells[nRowIndex, i]; cell.Style.Font.Bold = false; cell.Value = "";
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                         cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                         border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                     }
                     else
                     {
                         cell = sheet.Cells[nRowIndex, i]; cell.Style.Font.Bold = false; cell.Formula = sFormula;
                         cell.Style.Font.Bold = true;
                         border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                         cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                         border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     }
                 }
                 nRowIndex++;
                 #endregion

                 #endregion
                 cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                 fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                 fill.BackgroundColor.SetColor(Color.White);

                 Response.ClearContent();
                 Response.BinaryWrite(excelPackage.GetAsByteArray());
                 Response.AddHeader("content-disposition", "attachment; filename=FARegisterSummery.xlsx");
                 Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                 Response.Flush();
                 Response.End();
             }
             #endregion
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
    }

}
