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
    public class rptExportBillRegister
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        BusinessUnit _oBusinessUnit;
        MemoryStream _oMemoryStream = new MemoryStream();
        ImportOutstanding _oImportOutstanding = new ImportOutstanding();
        List<ExportBillReport> _oExportBillReports = new List<ExportBillReport>();
        Company _oCompany = new Company();
        string _sExportLCType = "";
        #endregion

        public byte[] PrepareReport(List<ExportBillReport> oExportBillReports, Company oCompany, BusinessUnit oBusinessUnit, int nReportLayout, int nReportType, string sDateRange,string sExportLCType)
        {
            _oExportBillReports = oExportBillReports;
            _oExportBillReports = _oExportBillReports.OrderBy(x => x.ExportBillID).ThenBy(x => x.ExportLCNo).ThenBy(x => x.PINo).ThenBy(x => x.ProductName).ToList();
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;
            _sExportLCType = sExportLCType;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(10f, 10f, 20f, 5f);
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
            _oPdfPTable.SetWidths(new float[] { 842f });
            //this.PrintHeader("Export Bill Register",sDateRange);

            if (nReportType == 1)
            {
                this.PrintHeader("Export Bill Register", "BACK TO BACK LC POSITION", sDateRange);              
                this.PrintB2BLCBody();
            }
            if (nReportType == 2)
            {
                this.PrintHeader("Export Bill Register", "ACCEPTANCE DOC. SEND TO BANK", sDateRange);            
                this.PrintAcceptanceDocBody();
            }
            if (nReportType == 3)
            {
                this.PrintHeader("Export Bill Register", "ACCEPTANCE RECEIVED FROM NEGO. BANK", sDateRange);            
                this.PrintAcceptanceRecBody();
            }
          
            _oPdfPTable.HeaderRows = 7;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();

            #endregion
        }

        #region Report Header
        private void PrintHeader(string sHeader, string sHeaderTitle, string sDateRange)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            this.PrintRow(_oBusinessUnit.Name);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            this.PrintRow(_oBusinessUnit.Address);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE); //Unit
            //this.PrintRow(sHeader);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            this.PrintRow(sHeaderTitle);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            if (_sExportLCType != "")
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("L/C Type: " + _sExportLCType, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }

            this.PrintRow(sDateRange);
        }
        #endregion

        #region Report Body

        #region B2B LC
        private PdfPTable B2BLCTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    70f,  //LC No
                                                    100f, //Bank
                                                    90f,  //OurBank
                                                    120f,  //Party Name                                                
                                                    45f,  //PI No                                               
                                                    120f,  //Commodity    
                                                    45f,  //Qty
                                                    50f,  //Amount
                                                    55f,  //ShipmentDT
                                                    55f,  //EXP DT
                                                    //50f,  //Remarks
                                                    70f,  //C/p
                                             });
            return oPdfPTable;
        }
        private void PrintB2BLCBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable = B2BLCTable();
            #endregion

            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "SL No", false);
            this.AddCellHeader(ref oPdfPTable, "BTB LC No", true);
            this.AddCellHeader(ref oPdfPTable, "Issue Bank", true);
            this.AddCellHeader(ref oPdfPTable, "Nego. Bank", true);
            this.AddCellHeader(ref oPdfPTable, "Party Name", true);
            this.AddCellHeader(ref oPdfPTable, "P/I No", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Amount", false);
            this.AddCellHeader(ref oPdfPTable, "Shipment DT", true);
            this.AddCellHeader(ref oPdfPTable, "Expire DT", true);
            //this.AddCellHeader(ref oPdfPTable, "Remarks", true);
            this.AddCellHeader(ref oPdfPTable, "C/P", true);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion

            #region Data
            int nSL = 0, nLCRowSpan = 0, nPIRowSpan = 0, nLastExportBillID=0;
            double nTotalAmount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            foreach (ExportBillReport oItem in _oExportBillReports)
            {
                nLCRowSpan = _oExportBillReports.Where(x => x.ExportBillID == oItem.ExportBillID).Count();
                nPIRowSpan = _oExportBillReports.Where(x => x.PINo == oItem.PINo && x.ExportBillID == oItem.ExportBillID).Count();

                if (oItem.ExportBillID != nLastExportBillID)
                {
                    #region Initialize Table
                    oPdfPTable = B2BLCTable();
                    #endregion
                    nSL++;
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "LEFT", nSL.ToString());
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "LEFT", oItem.ExportLCNo);
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "LEFT", oItem.BankName_Issue);
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "LEFT", oItem.BankName_Nego);
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "LEFT", oItem.ApplicantName);
                    nTotalAmount = nTotalAmount + oItem.Amount;
                }
                this.AddCell(ref oPdfPTable, oItem.PINo, true);
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty), false);

                if (oItem.ExportBillID != nLastExportBillID)
                {
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "RIGHT", oItem.Currency + Global.MillionFormat(_oExportBillReports.Where(x=>x.ExportBillID==oItem.ExportBillID).Select(x => x.Amount).First()));
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "CENTER", oItem.ShipmentDateSt);
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "CENTER", oItem.ExpiryDateSt);

                    //_oFontStyle = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.NORMAL);
                    //this.AddCell(ref oPdfPTable, nLCRowSpan, "LEFT", oItem.StateSt);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    this.AddCell(ref oPdfPTable, nLCRowSpan, "LEFT", oItem.MKTPName);
                }
                oPdfPTable.CompleteRow();
                if (oItem.ExportBillID != nLastExportBillID)
                {
                    AddTable(oPdfPTable);
                }
                nLastExportBillID = oItem.ExportBillID;
            }
  
            #endregion

            #region Total

            #region Initialize Table
            oPdfPTable = B2BLCTable();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable,"", "Total", 7);

            double nGrandTotalAmount = _oExportBillReports.GroupBy(p => p.ExportBillID).Select(grp => new ExportBillReport
            {
                Amount = grp.First().Amount
            }).Sum(x => x.Amount);
            this.AddCell(ref oPdfPTable, "LEFT", "   " + Global.MillionFormat(_oExportBillReports.Sum(x => x.Qty)) + "    " + "$ " + Global.MillionFormat(nGrandTotalAmount), 5);
           // this.AddCell(ref oPdfPTable, , false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion
        }
        
        #endregion

        #region ACCEPTANCE DOC
        private PdfPTable AcceptanceDocTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    70f,  //LC No
                                                    113f, //Bank
                                                    78f,  //OurBank
                                                    55f,  //LDBC Date                                                
                                                    55f,  //BankRcvDT                                               
                                                    123f,  //PartyName    
                                                    45f,  //PINo
                                                    125f,  //Commodity
                                                    40f,  //Qty
                                                    45f,  //Amount
                                                    //50f,  //Remarks
                                                    58f,  //C/p
                                             });
            return oPdfPTable;
        }
        private void PrintAcceptanceDocBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable = AcceptanceDocTable();
            #endregion

            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "SL No", false);
            this.AddCellHeader(ref oPdfPTable, "BTB LC No", true);
            this.AddCellHeader(ref oPdfPTable, "Issue Bank & Branch", true);
            this.AddCellHeader(ref oPdfPTable, "LDBC No", true);
            this.AddCellHeader(ref oPdfPTable, "LDBC Date", true);
            this.AddCellHeader(ref oPdfPTable, "Bank Rcv DT", true);
            this.AddCellHeader(ref oPdfPTable, "Party Name", true);
            this.AddCellHeader(ref oPdfPTable, "P/I No", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Amount", false);
            //this.AddCellHeader(ref oPdfPTable, "Remarks", true);
            this.AddCellHeader(ref oPdfPTable, "C/P", true);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion

            double nTotalAmount = 0;
            int nSL = 0, nRowSpan = 0, nLastExportBillID = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ExportBillReport oItem in _oExportBillReports)
            {
                nRowSpan = _oExportBillReports.Where(x => x.ExportBillID == oItem.ExportBillID).Count();

                if (oItem.ExportBillID != nLastExportBillID)
                {
                    #region Initialize Table
                    oPdfPTable = AcceptanceDocTable();
                    #endregion
                    nSL++;
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", nSL.ToString());
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.ExportLCNo);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.BankName_Issue + " [" + oItem.BBranchName_Issue + "]");
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.LDBCNo);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.LDBCDateSt);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.RecedFromBankDateSt);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.ApplicantName);
                    nTotalAmount = nTotalAmount + oItem.Amount;
                }

                this.AddCell(ref oPdfPTable, oItem.PINo, true);
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty), false);

                if (oItem.ExportBillID != nLastExportBillID)
                {
                    this.AddCell(ref oPdfPTable, nRowSpan, "RIGHT", oItem.Currency + Global.MillionFormat(_oExportBillReports.Where(x=>x.ExportBillID==oItem.ExportBillID).Select(x => x.Amount).First()));
                    //_oFontStyle = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.NORMAL);
                    //this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.StateSt);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.MKTPName);
                }
              
                oPdfPTable.CompleteRow();
                if (oItem.ExportBillID != nLastExportBillID)
                {
                    AddTable(oPdfPTable);
                }
                nLastExportBillID = oItem.ExportBillID;
            }
            #endregion

            #region Total

            #region Initialize Table
            oPdfPTable = AcceptanceDocTable();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable,"", "Total", 9);
            double nGrandTotalAmount = _oExportBillReports.GroupBy(p => p.ExportBillID).Select(grp => new ExportBillReport
            {
                Amount = grp.First().Amount
            }).Sum(x => x.Amount);
            this.AddCell(ref oPdfPTable, "LEFT", "   " + Global.MillionFormat(_oExportBillReports.Sum(x => x.Qty)) + "    " + "$ " + Global.MillionFormat(nGrandTotalAmount), 3);

            //this.AddCell(ref oPdfPTable, Global.MillionFormat(_oExportBillReports.Sum(x => x.Qty)), false);
           // this.AddCell(ref oPdfPTable, _oExportBillReports[0].Currency + Global.MillionFormat(_oExportBillReports.Sum(x => x.Amount)), false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion
        }

        #endregion

        #region ACCEPTANCE RECEIVED
        private PdfPTable AcceptanceRecTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    70f,  //LC No
                                                    97f, //Bank
                                                    80f,  //OurBank
                                                    120f,  //Party Name                                                
                                                    50f,  //PI No                                               
                                                    120f,  //Commodity    
                                                    50f,  //Qty
                                                    50f,  //Amount
                                                    55f,  //ShipmentDT
                                                    55f,  //EXP DT
                                                    //50f,  //Remarks
                                                    70f,  //C/p
                                             });
            return oPdfPTable;
        }
        private void PrintAcceptanceRecBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable = AcceptanceRecTable();
            #endregion

            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "SL No", false);
            this.AddCellHeader(ref oPdfPTable, "BTB LC No", true);
            this.AddCellHeader(ref oPdfPTable, "Issue Bank", true);
            this.AddCellHeader(ref oPdfPTable, "Nego. Bank", true);
            this.AddCellHeader(ref oPdfPTable, "Party Name", true);
            this.AddCellHeader(ref oPdfPTable, "P/I No", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Amount", false);
            this.AddCellHeader(ref oPdfPTable, "Maturity DT", true);
            this.AddCellHeader(ref oPdfPTable, "Payment DT", true);
            //this.AddCellHeader(ref oPdfPTable, "Remarks", true);
            this.AddCellHeader(ref oPdfPTable, "C/P", true);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion

            double nTotalAmount = 0;
            int nSL = 0, nRowSpan = 0, nPIRowSpan = 0, nLastExportBillID = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ExportBillReport oItem in _oExportBillReports)
            {
                nRowSpan = _oExportBillReports.Where(x => x.ExportBillID == oItem.ExportBillID).Count();

                if (oItem.ExportBillID != nLastExportBillID)
                {
                    #region Initialize Table
                    oPdfPTable = AcceptanceRecTable();
                    #endregion
                    nSL++;
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", nSL.ToString());
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.ExportLCNo);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.BankName_Issue);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.LDBCNo);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.ApplicantName);
                    nTotalAmount = nTotalAmount + oItem.Amount;
                }
                this.AddCell(ref oPdfPTable, oItem.PINo, true);
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty), false);

                if (oItem.ExportBillID != nLastExportBillID)
                {
                    this.AddCell(ref oPdfPTable, nRowSpan, "RIGHT", oItem.Currency + Global.MillionFormat(_oExportBillReports.Where(x => x.ExportBillID == oItem.ExportBillID).Select(x => x.Amount).First()));
                    this.AddCell(ref oPdfPTable, nRowSpan, "CENTER", oItem.MaturityDateSt);
                    this.AddCell(ref oPdfPTable, nRowSpan, "CENTER", oItem.AcceptanceDateStr);
                    //_oFontStyle = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.NORMAL);
                    //this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.StateSt);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                    this.AddCell(ref oPdfPTable, nRowSpan, "LEFT", oItem.MKTPName);
                }

                oPdfPTable.CompleteRow();
                if (oItem.ExportBillID != nLastExportBillID)
                {
                    AddTable(oPdfPTable);
                }
                nLastExportBillID = oItem.ExportBillID;
            }
            #endregion

            #region Total

            #region Initialize Table
            oPdfPTable = AcceptanceRecTable();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable,"", "Total", 7);
            double nGrandTotalAmount = _oExportBillReports.GroupBy(p => p.ExportBillID).Select(grp => new ExportBillReport
            {
                Amount = grp.First().Amount
            }).Sum(x => x.Amount);
            this.AddCell(ref oPdfPTable, "LEFT", "   " + Global.MillionFormat(_oExportBillReports.Sum(x => x.Qty)) + "    " + "$ " + Global.MillionFormat(nGrandTotalAmount), 5);

            //this.AddCell(ref oPdfPTable, Global.MillionFormat(_oExportBillReports.Sum(x => x.Amount_LC)), false);
            //this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oExportBillReports.Sum(x => x.Amount)), false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion
        }

        #endregion

        #endregion

        #region PDF HELPER
        public void AddCellHeader(ref PdfPTable oPdfPTable, string sHeader, bool IsLeft)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            //"IsLeft" Not Used
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(ref PdfPTable oPdfPTable, string sHeader, bool IsLeft)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (IsLeft)
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(ref PdfPTable oPdfPTable, string sAlignment, string sHeader, int nColSpan)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = nColSpan;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(ref PdfPTable oPdfPTable,int nRowSpan, string sAlignment, string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));

            if (sAlignment=="LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Rowspan = nRowSpan;
            oPdfPTable.AddCell(_oPdfPCell);
        }     
        public void PrintRow(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void AddTable(PdfPTable oPdfPTable)
        {
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
    }
}
