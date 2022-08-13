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
    public class SampleRequestRegisterController:Controller
    {
        string _sDateRange = "";
        string _sErrorMesage = "";
        SampleRequestRegister _oSampleRequestRegister = new SampleRequestRegister();

        List<SampleRequestRegister> _oSampleRequestRegisters = new List<SampleRequestRegister>();
        #region Action
        public ActionResult ViewSampleRequestRegister(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oSampleRequestRegister = new SampleRequestRegister();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            return View(_oSampleRequestRegister);
        }
        #endregion
        #region Set Session
        public ActionResult SetSessionSearchCriteria(SampleRequestRegister oSampleRequestRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oSampleRequestRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Print
        public Image GetCompanyLogo(Company oCompany)
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
        public ActionResult PrintSampleRequestRegister(double ts)
        {
            SampleRequestRegister oSampleRequestRegister = new SampleRequestRegister();
            string _sErrorMesage = "";
            try
            {
                _oSampleRequestRegisters = new List<SampleRequestRegister>();
                oSampleRequestRegister = (SampleRequestRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oSampleRequestRegister);
                _oSampleRequestRegisters = SampleRequestRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oSampleRequestRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oSampleRequestRegisters = new List<SampleRequestRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oSampleRequestRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oSampleRequestRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptSampleRequestRegister oReport = new rptSampleRequestRegister();
                byte[] abytes = oReport.PrepareReport(_oSampleRequestRegisters, oCompany, oSampleRequestRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");


            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }
        #endregion
        #region Excel
        public void ExcelSampleRequestRegister(double ts)
        {
            SampleRequestRegister oSampleRequestRegister = new SampleRequestRegister();
            string _sErrorMesage = "";
            try
            {
                _oSampleRequestRegisters = new List<SampleRequestRegister>();
                oSampleRequestRegister = (SampleRequestRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oSampleRequestRegister);
                _oSampleRequestRegisters = SampleRequestRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oSampleRequestRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oSampleRequestRegisters = new List<SampleRequestRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oSampleRequestRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oSampleRequestRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }
                double SubTotalQty = 0, GrandTotalQty = 0;
                if (oSampleRequestRegister.ReportLayout == EnumReportLayout.DateWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Date Wise Sample Request");
                        sheet.Name = "Date Wise Sample Request";
                        sheet.Column(2).Width = 8;
                        sheet.Column(3).Width = 13;
                        sheet.Column(4).Width = 18;
                        sheet.Column(5).Width = 12;

                        sheet.Column(6).Width = 12;
                        sheet.Column(7).Width = 18;
                        sheet.Column(8).Width = 12;
                        sheet.Column(9).Width = 18;
                        sheet.Column(10).Width = 18;

                        #region Report Header
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = "Date Wise Sample Request"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion
                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Request No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Party Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Contact Person"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Quantity"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Remarks"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion
                        rowIndex++;
                        int num = 0;
                        int nSampleRequestId = 0; int nRowSpan = 0; string RequestDate = "";
                        foreach (SampleRequestRegister oItem in _oSampleRequestRegisters)
                        {
                            if (nSampleRequestId != oItem.SampleRequestID)
                            {
                                #region subtotal
                                if (SubTotalQty > 0)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    
                                    cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rowIndex += 1;
                                    SubTotalQty = 0;

                                }
                                #endregion
                                if (RequestDate != oItem.RequestDateInString)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Request Date : @ " + oItem.RequestDateInString; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rowIndex++;
                                }
                                int rowCount = (_oSampleRequestRegisters.Count(x => x.SampleRequestID == oItem.SampleRequestID) - 1);
                                rowCount = (rowCount == -1) ? 0 : rowCount;
                                cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = ++num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                                cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.RequestNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.Quantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            SubTotalQty = SubTotalQty + oItem.Quantity;
                            GrandTotalQty = GrandTotalQty + oItem.Quantity;
                            nSampleRequestId = oItem.SampleRequestID;
                            RequestDate = oItem.RequestDateInString;
                            rowIndex += 1;
                        }
                        #region subtotal
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        #region grand total
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total: "; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion
                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Date_Wise_Sample_Request.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();

                    }
                    #endregion
                }

                if (oSampleRequestRegister.ReportLayout == EnumReportLayout.PartyWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Party Wise Sample Request");
                        sheet.Name = "Party Wise Sample Request";
                        sheet.Column(2).Width = 8;
                        sheet.Column(3).Width = 13;
                        sheet.Column(4).Width = 18;
                        sheet.Column(5).Width = 12;

                        sheet.Column(6).Width = 12;
                        sheet.Column(7).Width = 18;
                        sheet.Column(8).Width = 12;
                        sheet.Column(9).Width = 18;
                        sheet.Column(10).Width = 18;

                        #region Report Header
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = "Party Wise Sample Request"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion
                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Request No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Product Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Request Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Contact Person"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Quantity"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Remarks"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion
                        rowIndex++;
                        int num = 0;
                        int nSampleRequestId = 0; int nRowSpan = 0; int nPartyID=0;
                        foreach (SampleRequestRegister oItem in _oSampleRequestRegisters)
                        {
                            if (nSampleRequestId != oItem.SampleRequestID)
                            {
                                #region subtotal
                                if (SubTotalQty > 0)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rowIndex += 1;
                                    SubTotalQty = 0;

                                }
                                #endregion
                                if (nPartyID != oItem.ContractorID)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Party Name : @ " + oItem.ContractorName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rowIndex++;
                                }
                                int rowCount = (_oSampleRequestRegisters.Count(x => x.SampleRequestID == oItem.SampleRequestID) - 1);
                                rowCount = (rowCount == -1) ? 0 : rowCount;
                                cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = ++num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                                cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.RequestNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.RequestDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.Quantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            SubTotalQty = SubTotalQty + oItem.Quantity;
                            GrandTotalQty = GrandTotalQty + oItem.Quantity;
                            nSampleRequestId = oItem.SampleRequestID;
                            nPartyID = oItem.ContractorID;
                            rowIndex += 1;
                        }
                        #region subtotal
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        #region grand total
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total: "; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion
                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Patry_Wise_Sample_Request.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();

                    }
                    #endregion
                }

                if (oSampleRequestRegister.ReportLayout == EnumReportLayout.ProductWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Product Wise Sample Request");
                        sheet.Name = "Product Wise Sample Request";
                        sheet.Column(2).Width = 8;
                        sheet.Column(3).Width = 13;
                        sheet.Column(4).Width = 18;
                        sheet.Column(5).Width = 12;

                        sheet.Column(6).Width = 12;
                        sheet.Column(7).Width = 18;
                        sheet.Column(8).Width = 12;
                        sheet.Column(9).Width = 18;
                        sheet.Column(10).Width = 18;

                        #region Report Header
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = "Product Wise Sample Request"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion
                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Request No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Party Name"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Color"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Request Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Contact Person"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Quantity"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Remarks"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion
                        rowIndex++;
                        int num = 0;
                        int nSampleRequestId = 0; int nRowSpan = 0; int nProduct = 0;
                        foreach (SampleRequestRegister oItem in _oSampleRequestRegisters)
                        {
                            if (nProduct != oItem.ProductID)
                            {
                                #region subtotal
                                if (SubTotalQty > 0)
                                {
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total:"; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rowIndex += 1;
                                    SubTotalQty = 0;

                                }
                                #endregion
                                
                                    cell = sheet.Cells[rowIndex, 2, rowIndex, 10]; cell.Merge = true; cell.Value = "Party Name : @ " + oItem.ContractorName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    rowIndex++;

                                    int rowCount = (_oSampleRequestRegisters.Count(x => x.SampleRequestID == oItem.SampleRequestID && x.ProductID == oItem.ProductID) - 1);
                                rowCount = (rowCount == -1) ? 0 : rowCount;
                                cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = ++num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                                cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.RequestNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            cell = sheet.Cells[rowIndex, 4]; cell.Merge = true; cell.Value = oItem.ContractorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.ColorName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.UnitName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                            cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.RequestDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 8]; cell.Merge = true; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 9]; cell.Merge = true; cell.Value = oItem.Quantity.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            SubTotalQty = SubTotalQty + oItem.Quantity;
                            GrandTotalQty = GrandTotalQty + oItem.Quantity;
                            nSampleRequestId = oItem.SampleRequestID;
                            nProduct = oItem.ProductID;
                            rowIndex += 1;
                        }
                        #region subtotal
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        rowIndex = rowIndex + 1;
                        #endregion

                        #region grand total
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Grand Total: "; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = GrandTotalQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        #endregion
                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Patry_Wise_Sample_Request.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();

                    }
                    #endregion
                }
            }
        }
        #endregion
        #region GetSql
        private string GetSQL(SampleRequestRegister oSampleRequestRegister)
        {
            //string _sDateRange = "";
            string sSearchingData = oSampleRequestRegister.ErrorMessage;
            EnumCompareOperator eSampleRequestDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dSampleRequestStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dSampleRequestEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            string sApprovalStatus = sSearchingData.Split('~')[3];

            #region make date range
            if (eSampleRequestDate == EnumCompareOperator.EqualTo)
            {
                _sDateRange = "Receive Date: " + dSampleRequestStartDate.ToString("dd MMM yyyy");
            }
            else if (eSampleRequestDate == EnumCompareOperator.Between)
            {
                _sDateRange = "Receive Date: " + dSampleRequestStartDate.ToString("dd MMM yyyy") + " - To - " + dSampleRequestEndDate.ToString("dd MMM yyyy");
            }
            else if (eSampleRequestDate == EnumCompareOperator.NotEqualTo)
            {
                _sDateRange = "Receive Date: Not Equal to " + dSampleRequestStartDate.ToString("dd MMM yyyy");
            }
            else if (eSampleRequestDate == EnumCompareOperator.GreaterThan)
            {
                _sDateRange = "Receive Date: Greater Than " + dSampleRequestStartDate.ToString("dd MMM yyyy");
            }
            else if (eSampleRequestDate == EnumCompareOperator.SmallerThan)
            {
                _sDateRange = "Receive Date: Smaller Than " + dSampleRequestStartDate.ToString("dd MMM yyyy");
            }
            else if (eSampleRequestDate == EnumCompareOperator.NotBetween)
            {
                _sDateRange = "Receive Date: Not Between " + dSampleRequestStartDate.ToString("dd MMM yyyy") + " - To - " + dSampleRequestEndDate.ToString("dd MMM yyyy");
            }
            #endregion

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            if (oSampleRequestRegister.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " BUID =" + oSampleRequestRegister.BUID.ToString();
            }
            #endregion

            #region FabricRequestNo
            if (!string.IsNullOrEmpty(oSampleRequestRegister.RequestNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " RequestNo LIKE'%" + oSampleRequestRegister.RequestNo + "%'";
            }
            #endregion

            #region ContractorID
            if (oSampleRequestRegister.ContractorID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ContractorID =" + oSampleRequestRegister.ContractorID;
            }
            #endregion

            #region ProductID
            if (oSampleRequestRegister.ProductID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " ProductID =" + oSampleRequestRegister.ProductID;
            }
            #endregion

           

            #region Receive Date
            if (eSampleRequestDate != EnumCompareOperator.None)
            {
                if (eSampleRequestDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSampleRequestStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eSampleRequestDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSampleRequestStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eSampleRequestDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSampleRequestStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eSampleRequestDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSampleRequestStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eSampleRequestDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSampleRequestStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSampleRequestEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eSampleRequestDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),RequestDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSampleRequestStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dSampleRequestEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

           

            #region Report Layout
            if (oSampleRequestRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_SampleRequestRegister ";
                sOrderBy = " ORDER BY  RequestDate, SampleRequestID, SampleRequestDetailID ASC";
            }
            else if (oSampleRequestRegister.ReportLayout == EnumReportLayout.ProductWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_SampleRequestRegister ";
                sOrderBy = " ORDER BY  ProductID, SampleRequestID, SampleRequestDetailID ASC";
            }
            else if (oSampleRequestRegister.ReportLayout == EnumReportLayout.PartyWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_SampleRequestRegister ";
                sOrderBy = " ORDER BY  ContractorID, SampleRequestID, SampleRequestDetailID ASC";
            }
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }
        #endregion
    }
}