using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;

using ESimSol.BusinessObjects.ReportingObject;


namespace ESimSol.Reports
{
    public class rptExportUPSummary
    {
        #region Declaration
        int _nColumns = 10;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        public iTextSharp.text.Image _oImag { get; set; }
        PdfPTable _oPdfPTable = new PdfPTable(10);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<ExportUP> _oExportUPs = new List<ExportUP>();
        List<ExportUPDetail> _oExportUPDetails = new List<ExportUPDetail>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion


        #region Constructor

        public rptExportUPSummary() { }

        #endregion

        public byte[] PrepareReport(List<ExportUP> oExportUPs, List<ExportUPDetail> oExportUPDetails, Company oCompany, BusinessUnit oBusinessUnit)
        {

            _oExportUPs = oExportUPs;
            _oExportUPDetails = oExportUPDetails;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 100;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 25f, 50f, 60f, 65f, 65f, 55f, 55f, 35f, 80f, 60f }); //height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            #endregion
            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintHeader()
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

   
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Export UP Report", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            string[] columnHead = new string[] { "SL", "UP No", "Value($)", "UD No", "LC/ No", "Date", "UD Rcv Date", "A. No", "Party Name", "Realised Value($)" };


            foreach (string columnName in columnHead)
            {
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, columnName, 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyle);
            }


            if (_oExportUPs.Any() && _oExportUPs.First().ExportUPID > 0)
            {
                int nCount = 0;
                foreach (ExportUP oItem in _oExportUPs)
                {

                    var details = _oExportUPDetails.Where(x => x.ExportUPID == oItem.ExportUPID).ToList();
                    int nSpan = (details.Any()) ? details.Count() : 0;

                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, (++nCount).ToString(), nSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.UPNoWithYear, nSpan, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(details.Sum(x => x.Amount)), nSpan, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    if (details.Any())
                    {
                        foreach (ExportUPDetail oEUPD in details)
                        {
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oEUPD.ExportUDNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oEUPD.ExportLCNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oEUPD.LCOpeningDateStr, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oEUPD.UDReceiveDateStr, 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oEUPD.ANo.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oEUPD.ApplicantName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oEUPD.Amount), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                            _oPdfPTable.CompleteRow();
                        }
                    }
                    else
                    {
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                        _oPdfPTable.CompleteRow();
                    }
                    
                }
            }


        }

     
    }
}
