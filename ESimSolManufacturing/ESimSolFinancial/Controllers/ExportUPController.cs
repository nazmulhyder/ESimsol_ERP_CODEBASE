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
using ESimSol.BusinessObjects.ReportingObject;
using System.Dynamic;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ExportUPController : PdfViewController
    {

        #region ExportUP
        public ActionResult ViewExportUPs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<ExportUP> oExportUPs = new List<ExportUP>();
            string sSQL = "Select * from View_ExportUP Where ISNULL(ApproveByID,0)=0 AND BUID ="+buid;
            oExportUPs = ExportUP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(oExportUPs);

        }

        public ActionResult ViewExportUP(int nId)
        {
            ExportUP oExportUP = new ExportUP();
            oExportUP = ExportUP.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oExportUP.ExportUPID > 0)
            {
                string sSQL = "Select * from View_ExportUPDetail Where ExportUPID = " + oExportUP.ExportUPID + "";
                oExportUP.ExportUPDetails = ExportUPDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(oExportUP);

        }

        [HttpPost]
        public JsonResult Save(ExportUP oExportUP)
        {
            ExportUP _oExportUP = new ExportUP();
            try
            {
                _oExportUP = oExportUP.IUD(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oExportUP = new ExportUP();
                _oExportUP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportUP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ExportUP oExportUP)
        {
            try
            {
                if (oExportUP.ExportUPID <= 0) { throw new Exception("Please select an valid item."); }
                oExportUP.ErrorMessage = oExportUP.Delete(oExportUP.ExportUPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportUP = new ExportUP();
                oExportUP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportUP.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveExportUPDetail(ExportUPDetail oExportUPDetail)
        {
            try
            {
                if (oExportUPDetail.ExportUPDetailID <= 0)
                {
                    oExportUPDetail = oExportUPDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oExportUPDetail = oExportUPDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oExportUPDetail = new ExportUPDetail();
                oExportUPDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportUPDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteExportUPDetail(ExportUPDetail oExportUPDetail)
        {
            try
            {
                if (oExportUPDetail.ExportUPDetailID <= 0) { throw new Exception("Please select an valid item."); }
                oExportUPDetail = oExportUPDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportUPDetail = new ExportUPDetail();
                oExportUPDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportUPDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Approve(ExportUP oExportUP)
        {
            ExportUP _oExportUP = new ExportUP();
            try
            {
                if (oExportUP.ExportUPID <= 0)
                    throw new Exception("Invalid Export UP");

                _oExportUP = oExportUP.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oExportUP = new ExportUP();
                _oExportUP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportUP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Get(ExportUP oExportUP)
        {
            try
            {
                if (oExportUP.ExportUPID <= 0)
                    throw new Exception("Select a valid item from list");

                oExportUP = ExportUP.Get(oExportUP.ExportUPID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oExportUP.ExportUPID > 0)
                {
                    string sSQL="Select * from View_ExportUPDetail Where ExportUPID="+oExportUP.ExportUPID;
                    oExportUP.ExportUPDetails = ExportUPDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oExportUP = new ExportUP();
                oExportUP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportUP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(ExportUP oExportUP)
        {
            List<ExportUP> oExportUPs = new List<ExportUP>();
            try
            {
                string sSQL = "Select * from View_ExportUP Where ExportUPID<>0";

                if (!string.IsNullOrEmpty(oExportUP.UPNo))
                    sSQL += " And UPNo Like '%" + oExportUP.UPNo.Trim() + "%'";

                oExportUPs = ExportUP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oExportUPs = new List<ExportUP>();

                oExportUP = new ExportUP();
                oExportUP.ErrorMessage = ex.Message;
                oExportUPs.Add(oExportUP);
     
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportUPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvSearch(ExportUP oExportUP)
        {
            List<ExportUP> oExportUPs = new List<ExportUP>();
            ExportUP _oExportUP = new ExportUP();
            string sSQL = MakeSQL(oExportUP);
            if (sSQL == "Error")
            {
                _oExportUP = new ExportUP();
                _oExportUP.ErrorMessage = "Please select a searching critaria.";
                oExportUPs = new List<ExportUP>();
            }
            else
            {
                oExportUPs = new List<ExportUP>();
                oExportUPs = ExportUP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oExportUPs.Count == 0)
                {
                    oExportUPs = new List<ExportUP>();
                }
            }
            var jsonResult = Json(oExportUPs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private string MakeSQL(ExportUP oExportUP)
        {
            string sParams = oExportUP.ErrorMessage;

            string sLCNo = "", sUDNo = "";
            int nDateCriteria_QuotationDate = 0;
            DateTime dStart_QuotationDate = DateTime.Today,
                     dEnd_QuotationDate = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sLCNo = sParams.Split('~')[nCount++];
                sUDNo = sParams.Split('~')[nCount++];
                nDateCriteria_QuotationDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_QuotationDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_QuotationDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT * FROM View_ExportUP AS EB";
            string sReturn = "";

            #region LCNo
            if (!string.IsNullOrEmpty(sLCNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportUPID IN ( SELECT ExportUPID FROM View_ExportUPDetail WHERE ExportLCNo LIKE '%" + sLCNo + "%')";
            }
            #endregion

            #region UDNo
            if (!string.IsNullOrEmpty(sUDNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportUPID IN ( SELECT ExportUPID FROM View_ExportUPDetail WHERE ExportUDNo LIKE '%" + sUDNo + "%')";
            }
            #endregion

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.ExportUPDate", nDateCriteria_QuotationDate, dStart_QuotationDate, dEnd_QuotationDate);
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }


        public void ExportInExcel(FormCollection fc)
        {

            List<ExportUP> oExportUPs = new List<ExportUP>();
            List<ExportUPDetail> oExportUPDetails = new List<ExportUPDetail>();
            string sExportUPID = fc["ExportUPIDs"];
            int nBUID = Convert.ToInt32(fc["BUID"]);

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oCompany = oCompany.Get(((User)Session[SessionInfo.CurrentUser]).CompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            if (!string.IsNullOrEmpty(sExportUPID))
            {
                string sSQL = "Select * from View_ExportUP Where ExportUPID In (" + sExportUPID + ")";
                oExportUPs = ExportUP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oExportUPs.Any() && oExportUPs.First().ExportUPID > 0)
                {
                    sSQL = "Select * from View_ExportUPDetail Where ExportUPID In (" + string.Join(",", oExportUPs.Select(x => x.ExportUPID)) + ")";
                    oExportUPDetails = ExportUPDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                int rowIndex = 1;
                int nMaxColumn = 0;
                int colIndex = 2;
                ExcelRange cell;
                ExcelFill fill;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Export UP List");
                    sheet.Name = "Export UP List";

                    #region Coloums

                    string[] columnHead = new string[] { "SL", "UP No", "Value($)", "UD No", "LC/ No", "Date", "UD Rcv Date", "A. No", "Party Name", "Realised Value($)" };
                    int[] colWidth = new int[] { 6, 15, 15, 18, 18, 15, 15, 10, 25, 15 };

                    for (int i = 0; i < colWidth.Length; i++)
                    {
                        sheet.Column(colIndex).Width = colWidth[i];
                        colIndex++;
                    }
                    nMaxColumn = colIndex;

                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2, rowIndex, columnHead.Length + 1]; cell.Merge = true; cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, columnHead.Length + 1]; cell.Merge = true; cell.Value = oBusinessUnit.PringReportHead; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, columnHead.Length + 1]; cell.Merge = true; cell.Value = "UP Report"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    colIndex = 2;
                    rowIndex++;
                    for (int i = 0; i < columnHead.Length; i++)
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = columnHead[i]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                        colIndex++;
                    }
                    rowIndex++;
                    #endregion

                    #region Boby
                    if (oExportUPs.Any() && oExportUPs.First().ExportUPID > 0)
                    {
                        int nCount = 0;
                        foreach (ExportUP oItem in oExportUPs)
                        {

                            var details=oExportUPDetails.Where(x=>x.ExportUPID==oItem.ExportUPID).ToList();
                            int nSpan = (details.Any())? details.Count()-1 : 0;

                            colIndex = 2;


                            cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = (++nCount).ToString(); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = oItem.UPNoWithYear; cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, colIndex, rowIndex + nSpan, colIndex++]; cell.Merge = true; cell.Value = details.Sum(x => x.Amount); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
     
                            if(details.Any())
                            {
                                int index = colIndex;

                                 foreach (ExportUPDetail oEUPD in details)
                                 {
                                    colIndex = index;
                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEUPD.ExportUDNo; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEUPD.ExportLCNo; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEUPD.LCOpeningDateStr; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEUPD.UDReceiveDateStr; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEUPD.ANo; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEUPD.ApplicantName; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                               
                                     cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oEUPD.Amount; cell.Style.Font.Bold = false;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                     rowIndex++;
                                 }
                            }
                            else
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                               
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex++;
                            }

                        }
                    }
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=ExportUPList.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                
                   
            }
        }

        public ActionResult PrintUPSummary(FormCollection fc)
        {
            
            List<ExportUP> oExportUPs = new List<ExportUP>();
            List<ExportUPDetail> oExportUPDetails = new List<ExportUPDetail>();
            string sExportUPID = fc["ExportUPIDs"];
            int nBUID = Convert.ToInt32(fc["BUID"]);

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oCompany = oCompany.Get(((User)Session[SessionInfo.CurrentUser]).CompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            
            if (!string.IsNullOrEmpty(sExportUPID))
            {
                string sSQL = "Select * from View_ExportUP Where ExportUPID In (" + sExportUPID + ")";
                oExportUPs = ExportUP.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oExportUPs.Any() && oExportUPs.First().ExportUPID > 0)
                {
                    sSQL = "Select * from View_ExportUPDetail Where ExportUPID In (" + string.Join(",", oExportUPs.Select(x => x.ExportUPID)) + ")";
                    oExportUPDetails = ExportUPDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }

            rptExportUPSummary oReport = new rptExportUPSummary();
            byte[] abytes = oReport.PrepareReport(oExportUPs, oExportUPDetails, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
   
        #endregion

        #region ExportUP Add Tips
        //public ActionResult View_AddTipsnSpeedsForUP(int nId, double ts)
        //{
        //    TipsnSpeed oTipsnSpeed = new TipsnSpeed();
        //    List<TipsnSpeed> oTipsnSpeeds = new List<TipsnSpeed>();
        //    ExportUP oExportUP = new ExportUP();
        //    oExportUP = ExportUP.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    string sSQL = "";
        //    if (oExportUP.ExportUPID > 0)
        //    {
        //         sSQL = "Select * from View_ExportUPDetail Where ExportUPID = " + oExportUP.ExportUPID + "";
        //        oExportUP.ExportUPDetails = ExportUPDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        sSQL = "Select * from View_TipsnSpeed Where ReferenceID = " + oExportUP.ExportUPID;
        //        oTipsnSpeeds = TipsnSpeed.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    sSQL = "Select * From TipsType Where ReferenceType = " + (int)EnumReferenceType.UP;
        //    List<TipsType> oTipsTypes = TipsType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.TipsTypes = oTipsTypes;
        //    ViewBag.TipsnSpeeds = oTipsnSpeeds;
        //    ViewBag.Currencys = Currency.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    ViewBag.BaseCurrencyID = oCompany.BaseCurrencyID;

        //    return View(oExportUP);

        //}
        #endregion

    }
}