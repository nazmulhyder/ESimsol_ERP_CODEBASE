using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;




namespace ESimSol.Reports
{

    public class rptSampleOrderOutstandingMkt
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        BusinessUnit _oBusinessUnit;
        MemoryStream _oMemoryStream = new MemoryStream();
        SampleOutStanding _oSampleOutStanding = new SampleOutStanding();
        List<SampleOutStanding> _oSampleOutStandings = new List<SampleOutStanding>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(List<SampleOutStanding> oSampleOutStandings, BusinessUnit oBusinessUnit, string sStartEndDate)
        {
            _oSampleOutStandings = oSampleOutStandings;
            _oBusinessUnit = oBusinessUnit;
            //_oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//595*842
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);

            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler 
            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 40f, // SL No
                                                45f, // Code
                                                140f, //Buyer
                                                90f, //Open 
                                                90f, //Dbt
                                                90f, // Crdt
                                                90f, // blnc
                                                 });
            this.PrintHeader(sStartEndDate);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();

            #endregion
        }

        #region Report Header
        private void PrintHeader(string sStartEndDate)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            this.PrintRow(_oBusinessUnit.Name);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            this.PrintRow(_oBusinessUnit.PringReportHead);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
             this.PrintRow("Sample Outstanding Report");

             _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
             this.PrintRow(sStartEndDate);
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.PrintCell("SL No", true);
            this.PrintCell("Code", true);
            this.PrintCell("MKTP Name", true);
            this.PrintCell("Opening", false);
            this.PrintCell("Credit", false);
            this.PrintCell("Debit", false);
            this.PrintCell("Closing", false);
            _oPdfPTable.CompleteRow();

            int nSL = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (SampleOutStanding oItem in _oSampleOutStandings)
            {
                nSL++;
                this.PrintCell(nSL.ToString(), true);
                this.PrintCell(oItem.ContractorID.ToString(), true);
                this.PrintCell(oItem.MKTPName, true);
                this.PrintCell("$ "+Global.MillionFormat(oItem.Opening), false);
                this.PrintCell("$ " + Global.MillionFormat(oItem.Debit), false);
                this.PrintCell("$ " + Global.MillionFormat(oItem.Credit), false);
                this.PrintCell("$ " + Global.MillionFormat(oItem.Closing), false);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.PrintCell("", true);
            this.PrintCell("", true);
            this.PrintCell("Total", true);
            this.PrintCell("$ " + Global.MillionFormat(_oSampleOutStandings.Select(c => c.Opening).Sum()), false);
            this.PrintCell("$ " + Global.MillionFormat(_oSampleOutStandings.Select(c => c.Debit).Sum()), false);
            this.PrintCell("$ " + Global.MillionFormat(_oSampleOutStandings.Select(c => c.Credit).Sum()), false);
            this.PrintCell("$ " + Global.MillionFormat(_oSampleOutStandings.Select(c => c.Closing).Sum()), false);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
       
        #region PDF HELPER
        public void PrintCell(string sHeader, bool IsLeft)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if(IsLeft)
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintRow(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }       
        #endregion
    }


}
