﻿using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICS.Core.Framework;
using System.Linq;
namespace ESimSol.Reports
{
    public class rptCommissionMemo
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        int _nColumns = 8;
        public iTextSharp.text.Image _oImag { get; set; }
        MemoryStream _oMemoryStream = new MemoryStream();
        List<ExportBill> _oExportBills = new List<ExportBill>();
        SalesCommissionPayable _oSalesCommissionPayable = new SalesCommissionPayable();
        List<SalesCommissionPayable> _oSalesCommissionPayables = new List<SalesCommissionPayable>();
        List<SalesCommissionPayable> _TempSalesCommissionPayables = new List<SalesCommissionPayable>();
        List<ExportPIReport> _oExportPIReports = new List<ExportPIReport>();
        List<SalesCommissionPayable> _oTempSalesCommissionPayables = new List<SalesCommissionPayable>();
        SalesComPayment _oSalesComPayment = new SalesComPayment();
        List<SalesComPayment> _oSalesComPayments = new List<SalesComPayment>();
      
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Company _oCompany = new Company();
        int _nRow = 0;
        int _nRowCount = 0;
        PdfPTable oPdfPTable = new PdfPTable(9);
        PdfPCell oPdfPCell;
        string _sMunit = "";
        string _sMunitTwo = "";
        string _sCurrency = "";
        #endregion
        //Report Type: 1--> All, 2-->Customer_View, 3-->Buyer_View
        public byte[] PrepareReport(List<ExportBill> oExportBills, List<ExportPIReport> oExportPIReports, List<SalesCommissionPayable> oSalesCommissionPayables, List<SalesComPayment> oSalesComPayments, Company oCompany, BusinessUnit oBusinessUnit, int nReportType)
        {
            _oExportBills = oExportBills;
            _oSalesCommissionPayables = oSalesCommissionPayables;
            
            _oExportPIReports = oExportPIReports;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oSalesComPayments = oSalesComPayments;
            if (_oExportPIReports.Count > 0)
            {
                _sMunit = _oExportPIReports[0].MUName;
                _sMunitTwo = _sMunit;
                _sCurrency = _oExportPIReports[0].Currency;
                if (_oExportPIReports[0].ProductNature == EnumProductNature.Hanger || _oExportPIReports[0].ProductNature == EnumProductNature.Hanger)
                {
                    _sMunitTwo = "Dzn";
                }
            }

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 5f, 5f, 2f, 2f);
            _oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter _oWriter;
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler   

            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 98;
            _oPdfPTable.SetWidths(new float[] { 595f });//height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);


            #endregion

            this.PrintHeader();
            this.Print_CommissionMemoHeader();
            this.Print_BillHeader();
            this.Print_BillDetails();
            if (oExportPIReports.Any())
            {
                 this.Print_ExportPIReports(nReportType);
            }

            if (_oSalesCommissionPayables.Any())
            {
                this.Print_HeaderForComPayable();
                this.PrintCommissionPayable();
            }
            if (_oSalesComPayments.Any())
            {
                this.Print_SalesCommisionPaymentHeader();
                this.Print_SalesCommissionPayment();
            }
            this.Print_ReportFooter();
         
            _oPdfPTable.HeaderRows = 3;
          
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
           

            #endregion
        }
        #endregion
        private void Print_CommissionMemoHeader()
        {
            #region ReportHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 90;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Commission Memo(Invoice)", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion
        }

        #region Invoice Information
        private void Print_BillHeader()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 80f, 120f, 80f, 120f, 160f });
            

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPCell = new PdfPCell(new Phrase("LDBC  ", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":  " + _oSalesCommissionPayables[0].LDBCNo, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Invoice No", oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":  " + _oSalesCommissionPayables[0].ExportBillNo, oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern : " + _oExportPIReports[0].MKTPName, oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Buyer Name  ", oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":  "+_oExportPIReports[0].ContractorName , oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Invoice Value  ", oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":  " + _oSalesCommissionPayables[0].Currency + " " + Global.MillionFormat(_oSalesCommissionPayables[0].Amount_Bill).ToString(), oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Buying House :" + _oExportPIReports[0].BuyerName, oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            if (_oExportBills.Count > 0)
            {

                oPdfPCell = new PdfPCell(new Phrase("LC No  ", oFontStyle));

                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Border = 0;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(":  " + _oSalesCommissionPayables[0].ExportLCNo, oFontStyle));
                oPdfPCell.Colspan = 4;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Border = 0;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();


                oPdfPCell = new PdfPCell(new Phrase("LC Value " + " (" + _oSalesCommissionPayables[0].Currency + ")", oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Border = 0;
                oPdfPTable.AddCell(oPdfPCell);
                double LCValue = 0;


                oPdfPCell = new PdfPCell(new Phrase(":  " + _oSalesCommissionPayables[0].Currency + " " + Global.MillionFormat(_oExportBills[0].Amount_LC).ToString(), oFontStyle));
                oPdfPCell.Colspan = 4;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Border = 0;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
        #region BillDetails
        private void Print_BillDetails()
        {
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 50f,50f, 50f, 20f, 55f, 55f, 20f, 55f });
            if (_oExportBills.Any())
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

                oPdfPCell = new PdfPCell(new Phrase("Invoice Commission Information", _oFontStyle));
                oPdfPCell.Colspan = 9;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Maturity Information", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.Colspan = 4;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Realization Information", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.Colspan = 3;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                #region ExportBillDetailsColumn Header
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                oPdfPCell = new PdfPCell(new Phrase("Total Commission", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Maturity Rcv. Date", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                
                oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Commission", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Realization Date", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Commission", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region ExportDetailBody
                foreach (ExportBill oItem in _oExportBills)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                 
                    double ComMaturityAmount = (_oSalesCommissionPayables.Where(x => x.ExportBillID == oItem.ExportBillID).Any()) ? _oSalesCommissionPayables.Where(m => m.ExportBillID == oItem.ExportBillID).Sum(y => y.MaturityAmount) : 0;
                    double ComRealizationAmount = (_oSalesCommissionPayables.Where(x => x.ExportBillID == oItem.ExportBillID).Any()) ? _oSalesCommissionPayables.Where(m => m.ExportBillID == oItem.ExportBillID).Sum(y => y.RealizeAmount) : 0;

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(ComMaturityAmount + ComRealizationAmount).ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                   
                    oPdfPCell = new PdfPCell(new Phrase(oItem.MaturityReceivedDateSt, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    
                    oPdfPCell = new PdfPCell(new Phrase(oItem.MaturityDateSt, _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    double ComMaturityPercentage = (_oSalesCommissionPayables.Where(x => x.ExportBillID == oItem.ExportBillID).Any()) ? _oSalesCommissionPayables.Where(m => m.ExportBillID == oItem.ExportBillID).First().Percentage_Maturity : 0;
                    oPdfPCell = new PdfPCell(new Phrase(ComMaturityPercentage + " %", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(ComMaturityAmount).ToString(), _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.RelizationDateSt, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    double comRealizationPercentage = 100 - ComMaturityPercentage;
                    oPdfPCell = new PdfPCell(new Phrase(comRealizationPercentage.ToString() + " %", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    if (ComRealizationAmount > 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(ComRealizationAmount).ToString(), _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle));
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                   oPdfPTable.CompleteRow();

                }
                #endregion

            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        #endregion
        #region ExportPIReports
        private void Print_ExportPIReports(int nReportType)
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 45f, 120f, 50f, 30f, 40f, 40f, 30f, 40f, 45f });
            int RowHeight = 18;
            int nCount = 0;
            double nQty = 0;
            double nQty_Com = 0;
            double nValue = 0;
            double nComvalue = 0;

            if (nReportType == 1)//ForAll
            {
                oPdfPTable = new PdfPTable(11);
                oPdfPTable.SetWidths(new float[] {  35f, 115f, 39f, 28f, 40f, 30f, 30f, 35f, 29f, 32f, 33f });
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("PI Information", _oFontStyle));
            oPdfPCell.Colspan = (nReportType == 1 ? 12 : 10); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            oPdfPCell.Colspan = (nReportType == 1 ? 12 : 10); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.FixedHeight = 3;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region PIDetails Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            //oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Qty(" + _sMunit + ")", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            if (_oExportPIReports.Count > 0)
            {
                if (_oExportPIReports[0].RateUnit > 1)
                {
                    oPdfPCell = new PdfPCell(new Phrase("Rate\n/" + _oExportPIReports[0].RateUnit + "\n(" + _sMunit + ")", _oFontStyle));
                }
                else
                {
                    oPdfPCell = new PdfPCell(new Phrase("Rate(" + _sCurrency + ")", _oFontStyle));
                }
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("Rate(" + _sCurrency + ")", _oFontStyle));
            }
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Amount(" + _sCurrency + ")", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Com. Qty\n(" + _sMunitTwo + ")", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase((nReportType == 3 ? "Buyer Rate" : "C. Rate") + "\n(" + _sMunitTwo + ")", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase((nReportType == 3 ? "Buyer C.Amount" : "Com.\nAmount"), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);


            if (nReportType == 1)//ForAll
            {
                oPdfPCell = new PdfPCell(new Phrase("Buyer Rate\n(" + _sMunitTwo + ")", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Amount", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPCell = new PdfPCell(new Phrase("Actual Rate\n(" + _sMunitTwo + ")", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            #region PIDetails Body
            string sProductName = "";
            foreach (ExportPIReport oItem in _oExportPIReports)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //nCount++;
                //oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                //oPdfPCell.FixedHeight = RowHeight;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                 sProductName = "";
                // sProductName = oItem.ProductName;
                if (oItem.IsApplySizer)
                {
                    sProductName = oItem.ProductName;
                }
                else
                {
                   
                     if (!string.IsNullOrEmpty(oItem.ProductDescription)) { sProductName +=   oItem.ProductDescription+"\n";}
                     if (!string.IsNullOrEmpty(oItem.ReferenceCaption)) { sProductName += oItem.ReferenceCaption + "# "; }
                     if (!string.IsNullOrEmpty(oItem.ProductName)) { sProductName += oItem.ProductName; }
                  
                    if (!string.IsNullOrEmpty(oItem.SizeName)) { sProductName += ", SIZE: " + oItem.SizeName; }
                    if (oItem.ColorID != 0) { sProductName += "\nCOLOR: " + oItem.ColorName; }
                    if (oItem.ModelReferenceID != 0) { sProductName += ", MODEL: " + oItem.ModelReferenceName; }
                }
                if (!string.IsNullOrEmpty(oItem.FabricNo)) { sProductName += ", Mkt Ref: " + oItem.FabricNo; }
                if (!string.IsNullOrEmpty(oItem.Construction)) { sProductName += ", Const: " + oItem.Construction; }



                oPdfPCell = new PdfPCell(new Phrase(sProductName, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + oItem.UnitPrice.ToString("0.000"), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + oItem.AmountST, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.QtyCom).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormat((nReportType == 3 ? oItem.CRateTwo : oItem.CRate)).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormatActualDigit((nReportType == 3 ? oItem.CRateTwo : oItem.CRate) * (oItem.QtyCom)), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                if (nReportType == 1)//ForAll
                {
                    oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + "" + Global.MillionFormatActualDigit(oItem.CRateTwo), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.CRateTwo * oItem.Qty), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                }

                oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormatActualDigit(oItem.UnitPrice - (nReportType == 3 ? oItem.CRateTwo : (nReportType == 1 ? (oItem.CRateTwo + oItem.CRate) : oItem.CRate))), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                nQty = nQty + oItem.Qty;
                nQty_Com = nQty_Com + (oItem.QtyCom);
                nValue = nValue + oItem.UnitPrice * (oItem.Qty / oItem.RateUnit);
                nComvalue = nComvalue + (nReportType == 3 ? oItem.CRateTwo : oItem.CRate) * oItem.QtyCom;
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 2;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nQty).ToString() + "\n" + _oExportPIReports[0].MUName, _oFontStyle));
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_sCurrency + " " + Global.MillionFormat(nValue).ToString(), _oFontStyle));
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nQty_Com).ToString() + "\n" + _sMunitTwo, _oFontStyle));
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oExportPIReports[0].Currency + " " + Global.MillionFormat(nComvalue).ToString(), _oFontStyle));
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            if (nReportType == 1)//ForAll
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(_sCurrency + "" + Global.MillionFormat(_oExportPIReports.Sum(x => (x.CRateTwo * x.QtyCom))), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion
        #region Commission payable

        #region Sales Comm Payable Header
        private void Print_HeaderForComPayable()
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 12f, 120f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });



            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("Sales Commission Payable Information ", _oFontStyle));
            oPdfPCell.Colspan = 9; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            oPdfPCell.Colspan = 9; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.FixedHeight = 3;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        #endregion

        #region sales Com Payable Body
        private void PrintCommissionPayable()
        {
            if (_oSalesCommissionPayables.Any())
            {

                _oSalesCommissionPayables = _oSalesCommissionPayables.OrderBy(x => x.CPName).ToList();
                _oSalesCommissionPayables.ForEach(x => _oTempSalesCommissionPayables.Add(x));

                this.PrintHeaderRowInBody(_oSalesCommissionPayables[0]);

                while (_oSalesCommissionPayables.Count > 0)
                {
                    var oResults = _oSalesCommissionPayables.Where(x => x.CPName == _oSalesCommissionPayables[0].CPName).ToList();
                    _oSalesCommissionPayables.RemoveAll(x => x.CPName == oResults[0].CPName);

                    this.PrintBody(oResults);
                }
                PrintGrandTotal();
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }

        private void PrintHeaderRowInBody(SalesCommissionPayable oSalesCommissionPayable)
        {
            oPdfPTable.SetWidths(new float[] { 12f, 120f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("Person Name   :" + oSalesCommissionPayable.CPName, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPCell = new PdfPCell(new Phrase("[  " + oSalesCommissionPayable.ContractorName + "  ]", _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Con.#: " + oSalesCommissionPayable.Phone, _oFontStyle)); oPdfPCell.Colspan = 3; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();



            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


            oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Commission Amount", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Maturity Commission", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Realization Commission", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Adj. Amount", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Payable Amount", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Paid Amount", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
        }

        private void PrintBody(List<SalesCommissionPayable> oSalesCommissionPayables)
        {
            oPdfPTable.SetWidths(new float[] { 12f, 120f, 50f, 50f, 50f, 50f, 50f, 50f, 50f });
            int RowHeight = 18;
            var Currency = oSalesCommissionPayables[0].Currency;
            int nCount = 0;
            double TotalCommissionAmount = 0;
            double TotalMaturityAmount = 0;
            double TotalRealizeAmount = 0;
            double TotalAdjAmount = 0;
            double TotalPaidAmount = 0;
            double TotalBalance = 0;
            double TotalPayableAmount = 0;


            foreach (SalesCommissionPayable oItem in oSalesCommissionPayables)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                nCount++;

                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.CommissionAmount).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.MaturityAmount).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.RealizeAmount).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.AdjDeduct + oItem.AdjPayable).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.MaturityAmount + oItem.RealizeAmount - oItem.AdjDeduct - oItem.AdjPayable).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.Amount_Paid).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(oItem.MaturityAmount + oItem.RealizeAmount - oItem.AdjDeduct - oItem.AdjPayable - oItem.Amount_Paid).ToString(), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPTable.CompleteRow();

                TotalCommissionAmount = TotalCommissionAmount + oItem.CommissionAmount;
                TotalMaturityAmount = TotalMaturityAmount + oItem.MaturityAmount;
                TotalRealizeAmount = TotalRealizeAmount + oItem.RealizeAmount;
                TotalAdjAmount = TotalAdjAmount + oItem.AdjDeduct+ oItem.AdjPayable;
                TotalPayableAmount = TotalPayableAmount + (oItem.MaturityAmount + oItem.RealizeAmount - oItem.AdjDeduct - oItem.AdjPayable);
                TotalPaidAmount = TotalPaidAmount + oItem.Amount_Paid;
                TotalBalance = TotalBalance + (oItem.MaturityAmount + oItem.RealizeAmount - oItem.AdjDeduct - oItem.AdjPayable - oItem.Amount_Paid);

            }
            if (oSalesCommissionPayables.Count > 1)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle));  oPdfPCell.Colspan = 2;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalCommissionAmount).ToString(), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalMaturityAmount).ToString(), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalRealizeAmount).ToString(), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalAdjAmount).ToString(), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalPayableAmount).ToString(), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalPaidAmount).ToString(), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalBalance).ToString(), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = 12; oPdfPCell.Colspan = 11;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.Border = 0; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

            }



            if (_oSalesCommissionPayables.Any())
            {
                this.PrintHeaderRowInBody(_oSalesCommissionPayables[0]);
            }
        }

        private void PrintGrandTotal()
        {
            int RowHeight = 18;
            var Currency = _oTempSalesCommissionPayables[0].Currency;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle));  oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            double TotalCommissionAmount = _oTempSalesCommissionPayables.Sum(x => x.CommissionAmount);

            oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalCommissionAmount).ToString(), _oFontStyle)); 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            double TotalMaturityAmount = _oTempSalesCommissionPayables.Sum(x => x.MaturityAmount);
            oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalMaturityAmount).ToString(), _oFontStyle)); 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            double TotalRealizeAmount = _oTempSalesCommissionPayables.Sum(x => x.RealizeAmount);
            oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalRealizeAmount).ToString(), _oFontStyle)); 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            double TotalAdjAmount = _oTempSalesCommissionPayables.Sum(x => x.AdjAdd);

            oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalAdjAmount).ToString(), _oFontStyle)); 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            double TotalPayableAmount = TotalMaturityAmount + TotalRealizeAmount - TotalAdjAmount;

            oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalPayableAmount).ToString(), _oFontStyle)); 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);


            double TotalPaidAmount = _oTempSalesCommissionPayables.Sum(x => x.Amount_Paid);

            oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalPaidAmount).ToString(), _oFontStyle)); 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            double TotalBalance = TotalMaturityAmount + TotalRealizeAmount - TotalAdjAmount - TotalPaidAmount;
            oPdfPCell = new PdfPCell(new Phrase(Currency + Global.MillionFormat(TotalBalance).ToString(), _oFontStyle)); 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


        }
        #endregion

        #endregion
        #region Sales Commission Payment Detail
        private void Print_SalesCommisionPaymentHeader()
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;

            oPdfPTable.SetWidths(new float[] { 12f, 55f, 120f, 40f, 40f, 40f, 40f, 40f, 50f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            oPdfPCell = new PdfPCell(new Phrase("Sales Commission Payment Information ", _oFontStyle));
            oPdfPCell.Colspan = 9; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            oPdfPCell.Colspan = 9; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPCell.FixedHeight = 3;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }

        private void Print_SalesCommissionPayment()
        {
            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            int nCount = 0;
            int RowHeight = 18;

            oPdfPTable.SetWidths(new float[] { 20f, 55f, 55f, 200f, 50f, 50f, 50f });
            double nTotalComPayment = _oSalesComPayments.Sum(x => x.Amount);
            double nTotalAmountInBaseCurrency = 0;
            foreach (var oitem in _oSalesComPayments)
            {
                nTotalAmountInBaseCurrency = nTotalAmountInBaseCurrency + (oitem.CRate * oitem.Amount);
            }
            string currency = _oSalesComPayments[0].Currency;
            var oApprovedPayments = _oSalesComPayments.Where(x => x.ApproveBy != 0).ToList();
            _oSalesComPayments.RemoveAll(x => x.ApproveBy != 0);

            #region For Approved Payments
            if (oApprovedPayments.Any())
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("M.R No", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("M.R Date", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Payment Description ", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("C.Rate", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPTable.CompleteRow();

                foreach (SalesComPayment oItem in oApprovedPayments)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MRNo, _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MRDateStr, _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    if (oItem.PaymentType == EnumPayment_CommissionType.Cash)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oItem.PaymentTypeStr, _oFontStyle));
                        
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    else if (oItem.PaymentType == EnumPayment_CommissionType.Document)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oItem.PaymentTypeStr + "  " + oItem.PaymentModeStr + "  " + oItem.BankNameWithBranch + "  " + oItem.AccountName + "  " + oItem.AccountNo, _oFontStyle));
                        
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                     else 
                    {
                        oPdfPCell = new PdfPCell(new Phrase("SAN  : "+ "  " + oItem.SampleInvoiceNo + "  Date: " + oItem.SampleInvoiceDateStr , _oFontStyle));

                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    if (_oExportBills.Count > 0)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(_oExportBills[0].Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                    }
                    else
                    {
                        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.CRate).ToString(), _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.CRate * oItem.Amount).ToString(), _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();

                }

            }
            #endregion

            #region For UnApproved payments
            if (_oSalesComPayments.Any())
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

                oPdfPCell = new PdfPCell(new Phrase("Waiting For Approve Payment", _oFontStyle));
                oPdfPCell.Colspan = 7; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Colspan = 7; oPdfPCell.Border = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("MR No", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Mr Date", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("payment Description ", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("C.Rate", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPTable.CompleteRow();
                nCount = 0;
                foreach (SalesComPayment oItem in _oSalesComPayments)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    nCount++;
                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MRNo, _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MRDateStr, _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);
                    if (oItem.PaymentType == EnumPayment_CommissionType.Cash)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oItem.PaymentTypeStr, _oFontStyle));
                        
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    else if (oItem.PaymentType == EnumPayment_CommissionType.Document)
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oItem.PaymentTypeStr + "  " + oItem.PaymentModeStr + "  " + oItem.BankNameWithBranch + "  " + oItem.AccountName + "  " + oItem.AccountNo, _oFontStyle));
                        
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                     else 
                    {
                        oPdfPCell = new PdfPCell(new Phrase("SAN  : "+ "  " + oItem.SampleInvoiceNo + "  Date: " + oItem.SampleInvoiceDateStr , _oFontStyle));

                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    oPdfPCell = new PdfPCell(new Phrase(_oExportBills[0].Currency + " " + Global.MillionFormat(oItem.Amount).ToString(), _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.CRate).ToString(), _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.CRate * oItem.Amount).ToString(), _oFontStyle));
                    
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();



                }
            }
            #endregion

            oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = 4;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oExportBills[0].Currency + " " + Global.MillionFormat(nTotalComPayment).ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(currency+" "+Global.MillionFormat(nTotalAmountInBaseCurrency).ToString(), _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion
        #region ReportFooter
        private void Print_ReportFooter()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 150f, 150f, 150f });
            int RowHeight = 15;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            //if (_oSalesCommissions.Count > 0)
            //{
            //    oPdfPCell = new PdfPCell(new Phrase(_oSalesCommissions[0].RequestedByName, oFontStyle));
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.FixedHeight = RowHeight;
            //    oPdfPCell.Border = 0;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase(_oSalesCommissions[0].ApproveByName, oFontStyle));
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.FixedHeight = RowHeight;
            //    oPdfPCell.Border = 0;
            //    oPdfPTable.AddCell(oPdfPCell);
            //}
            //else
            //{
                oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.FixedHeight = RowHeight;
                oPdfPCell.Border = 0;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.FixedHeight = RowHeight;
                oPdfPCell.Border = 0;
                oPdfPTable.AddCell(oPdfPCell);
            //}



            oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("MKT. Concern", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Commercial Concern", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Account Manager", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Managing Director", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.FixedHeight = RowHeight;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion
    }
    }

