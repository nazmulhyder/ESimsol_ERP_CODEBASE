using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class ImportLCCodeController : Controller
    {
        #region Declaration 
        ImportLCCode _oImportLCCode = new ImportLCCode();
        List<ImportLCCode> _oImportLCCodes = new List<ImportLCCode>();
        #endregion

        public ActionResult ViewImportLCCodes(int MenuId)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, MenuId);

            _oImportLCCodes = ImportLCCode.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oImportLCCodes);
        }

        public ActionResult ViewImportLCCode(int id)
        {
            ImportLCCode _oImportLCCode = new ImportLCCode();
            if (id > 0)
            {
                _oImportLCCode = _oImportLCCode.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            
            return View(_oImportLCCode);
        }

        [HttpPost]
        public JsonResult Save(ImportLCCode oImportLCCode)
        {
            _oImportLCCode = new ImportLCCode();
            try
            {
                _oImportLCCode = oImportLCCode;
                _oImportLCCode = _oImportLCCode.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportLCCode = new ImportLCCode();
                _oImportLCCode.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oImportLCCode);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ImportLCCode oImportLCCode)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oImportLCCode.Delete(oImportLCCode.ImportLCCodeID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(ImportLCCode oImportLCCode)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportLCCode);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ImportLCCodeExcel(double ts)
        {
            Company oCompany = new Company();
            List<ImportLCCode> oImportLCCodes = new List<ImportLCCode>();
            ImportLCCode oReturnImportLCCode = new ImportLCCode();

            try
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

                oImportLCCodes = ImportLCCode.Gets((int)Session[SessionInfo.currentUserID]);

                if (oImportLCCodes.Count()>0)
                {
                    ExportToExcel(oImportLCCodes, oCompany);
                }
                else
                {
                    throw new Exception(oReturnImportLCCode.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Import LC Code");
                    sheet.Name = "Import LC Code";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Import LC Code.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        private void ExportToExcel(List<ImportLCCode> ImportLCCodes, Company oCompany)
        {
            int rowIndex = 2;
            int colIndex = 0;
            ExcelRange cell;
            Border border;
            ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Import LC Code");
                sheet.Name = "Import LC Code";
                //sheet.View.FreezePanes(5, 13);
                #region Declare Column
                
                sheet.Column(++colIndex).Width = 8;  //SL
                sheet.Column(++colIndex).Width = 20; //LC Code
                sheet.Column(++colIndex).Width = 20;//LC Status
                sheet.Column(++colIndex).Width = 20;//Remarks

                #endregion
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 3]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex++;
                #endregion

                #region Column Header
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "LC Code"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "LC Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex++]; cell.Merge = true; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;

                #endregion

                int nCount = 0;
                #region Report Body
                foreach (ImportLCCode oItem in ImportLCCodes)
                {
                    colIndex = 1;
                    nCount++;
                    
                        cell = sheet.Cells[rowIndex + 1, colIndex++]; cell.Value = nCount.ToString();
                        fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex + 1, colIndex++]; cell.Value = oItem.LCCode;
                        fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex + 1, colIndex++]; cell.Value = oItem.LCNature;
                        fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex + 1, colIndex++]; cell.Value = oItem.Remarks;
                        fill = cell.Style.Fill; cell.Style.WrapText = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    rowIndex++;
                }
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Import LC Code.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }
    }
}