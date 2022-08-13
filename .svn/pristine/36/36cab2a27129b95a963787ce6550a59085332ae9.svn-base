using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{
    public class rptPrintShiftWiseReport
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricBatchLoom> _oFabricBatchLooms = new List<FabricBatchLoom>();
        List<FabricBatchLoomDetail> _oFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
        List<RSShift> _oRSShifts = new List<RSShift>();
        Company _oCompany = new Company();
        string _sDateMsg = "";
        #endregion

        public byte[] PrepareReport(List<FabricBatchLoom> oFabricBatchLooms, List<FabricBatchLoomDetail> oFabricBatchLoomDetails, List<RSShift> oRSShifts, Company oCompany, string sDateMsg)
        {
            _oFabricBatchLooms = oFabricBatchLooms;//.OrderBy(y => y.MachineCode).ThenBy(x => x.FMID).ToList();
            _oFabricBatchLoomDetails = oFabricBatchLoomDetails;
            _oRSShifts = oRSShifts;
            _oCompany = oCompany;
            _sDateMsg = sDateMsg;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 15f, 15f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
           // _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header

        private void PrintHeader()
        {
            #region CompanyHeader


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void PrintHeaderDetail(string sReportHead, string sOrderType, string sDate)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 170f, 200f, 170f });
            #region
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintHeaderDetail("Shift Wise Production Report", " ", _sDateMsg);
            this.SetData();
        }
        #endregion


        private void SetData()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(16);
            oPdfPTable.SetWidths(new float[] { 60f, 38f, 40f, 55f, 75f, 90f, 50f, 65f, 60f, 80f, 40f, 40f, 40f, 40f, 50f, 70f });

            #region Heder Info
            oPdfPTable = new PdfPTable(16);
            oPdfPTable.SetWidths(new float[] { 60f, 38f, 40f, 55f, 75f, 90f, 50f, 65f, 60f, 80f, 40f, 40f, 40f, 40f, 50f, 70f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shift", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loom No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Ratio", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fab. Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Design", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "RPM", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Eff%", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Warp BK", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weft BK", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Production (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loom Production (M)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            var data = _oFabricBatchLooms.GroupBy(x => new { x.FMID, x.MachineCode }, (key, grp) => new
            {
                FMID = key.FMID,
                MachineCode = key.MachineCode,
                Results = grp.ToList()
            });

            List<FabricBatchLoomDetail> oTempFabricBatchLoomDetails = new List<FabricBatchLoomDetail>();
            FabricBatchLoomDetail oTempFabricBatchLoomDetail = new FabricBatchLoomDetail();

            #region Data
            double nGrandTotalProduction = 0;
            foreach (var oData in data)
            {
                
                foreach (var oItem in oData.Results)
                {
                    oPdfPTable = new PdfPTable(16);
                    oPdfPTable.SetWidths(new float[] { 60f, 38f, 40f, 55f, 75f, 90f, 50f, 65f, 60f, 80f, 40f, 40f, 40f, 40f, 50f, 70f });

                    double nTotalProduction = 0;
                    oTempFabricBatchLoomDetails = _oFabricBatchLoomDetails.Where(x => x.FabricBatchLoomID == oItem.FabricBatchLoomID).OrderBy(y => y.FabricBatchLoomID).ToList();
                    if (oTempFabricBatchLoomDetails.Count > 0)
                    {
                        foreach (RSShift oShift in _oRSShifts)
                        {
                            oTempFabricBatchLoomDetail = oTempFabricBatchLoomDetails.Where(x => x.ShiftID == oShift.RSShiftID && x.FabricBatchLoomID == oItem.FabricBatchLoomID).FirstOrDefault();
                            if (oTempFabricBatchLoomDetail != null)
                                nTotalProduction += oTempFabricBatchLoomDetail.QtyInM;
                        }
                        nGrandTotalProduction += nTotalProduction;
                        for (int k = 0; k < _oRSShifts.Count; k++)
                        {
                            oTempFabricBatchLoomDetail = oTempFabricBatchLoomDetails.Where(x => x.ShiftID == _oRSShifts[k].RSShiftID && x.FabricBatchLoomID == oItem.FabricBatchLoomID).FirstOrDefault();
                            if (oTempFabricBatchLoomDetail != null)
                            {
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLoomDetail.FinishDate.ToString("dd MMM yy"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRSShifts[k].Name, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                if (k == 0)
                                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MachineCode, _oRSShifts.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FEONo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                if (k == 0)
                                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, _oRSShifts.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLoomDetail.FabricType, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FabricWeave, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                if (k == 0)
                                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Construction, _oRSShifts.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLoomDetail.RPM.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLoomDetail.Efficiency.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLoomDetail.Warp.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLoomDetail.Weft.ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchLoomDetail.QtyInM.ToString("###,0.00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                                if (k == 0)
                                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nTotalProduction.ToString("###,0.00"), _oRSShifts.Count, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);


                            }
                            else
                            {
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oRSShifts[k].Name, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                if(k == 0)
                                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MachineCode, _oRSShifts.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                if (k == 0)
                                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, _oRSShifts.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                if (k == 0)
                                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Construction, _oRSShifts.Count, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                                if (k == 0)
                                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nTotalProduction.ToString("###,0.00"), _oRSShifts.Count, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                            }
                        }

                    }

                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                }

            }

            #endregion


            #region Grand Total
            oPdfPTable = new PdfPTable(16);
            oPdfPTable.SetWidths(new float[] { 60f, 38f, 40f, 55f, 75f, 90f, 50f, 65f, 60f, 80f, 40f, 40f, 40f, 40f, 50f, 70f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total   ", 0, 15, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, nGrandTotalProduction.ToString("###,0.00"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }


    }
}
