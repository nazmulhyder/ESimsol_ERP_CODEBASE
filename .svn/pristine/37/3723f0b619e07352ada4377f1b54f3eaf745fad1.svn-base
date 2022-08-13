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
    public class rptImportOutstandingAll
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<ImportOutStandingDetail> _oImportOutStandingDetails = new List<ImportOutStandingDetail>();
        ImportOutStandingDetail _oImportOutStandingDetail = new ImportOutStandingDetail();
        ImportOutstanding _oImportOutstanding = new ImportOutstanding();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Company _oCompany = new Company();
        int _nBUID = 0;
        int _CurrencyID = 0;
        int _nUserID = 0;
        string _sTableHead = "";
        #endregion
        public byte[] PrepareReportProduct(Company oCompany, BusinessUnit oBusinessUnit, int nBUID, int CurrencyID, int nUserID)
        {
            _oCompany = oCompany;
            _nBUID = nBUID;
            _CurrencyID = CurrencyID;
            _nUserID = nUserID;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(10f, 10f, 15f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            

            _oPdfPTable.SetWidths(new float[] { 842f });
            #endregion
            this.PrintHeader();
            this.PrintRowHeader();
            #region Get Data

            this.PrintLCOpen("L/C Open:");
            this.PrintRow("");
            this.PrintCopyDoc("Doc Recd:");
            this.PrintRow("");
            this.PrintShipment("Shipment:");
            this.PrintRow("");
            this.PrintDocInBack("Doc In Bank:");
            this.PrintRow("");
            this.PrintDocInCnf("Doc In CnF:");
            this.PrintRow("");
            this.PrintGoodsInTransit("Goods In Transit:");
            #endregion
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintLCOpen(string sHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(11);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetLCOpenTable();
            _sTableHead = sHeader;

            #region HEADER
            this.PrintARow(sHeader, ref oPdfPTable, 11);
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "File No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "PI No", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Due Inv ", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            this.AddCellHeader(ref oPdfPTable, "Shipment DT", true);
            this.AddCellHeader(ref oPdfPTable, "Expire DT", true);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetLCOpenTable();
            #endregion

            int nSL = 0, nLCID = -9999, nInvoiceID = -9999;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oImportOutStandingDetails = ImportOutStandingDetail.Gets(_nBUID, 0, 0, _CurrencyID, _oImportOutstanding.StartDate.AddYears(-1), _oImportOutstanding.EndDate, 1, (string.IsNullOrEmpty(_oImportOutstanding.ErrorMessage))?0:Convert.ToInt32(_oImportOutstanding.ErrorMessage), _nUserID);
            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
                if (oItem.LCID != nLCID && nSL>0)
                {
                    oPdfPTable.CompleteRow();
                    AddTable(oPdfPTable);
                    oPdfPTable = GetLCOpenTable();
                }
                if (oItem.LCID != nLCID)
                {
                    nInvoiceID = -9999;
                    nSL++;
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", nSL.ToString(), nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.FileNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.LCNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ImportLCDateStr, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ContractorName, nRowspan);
                }
                this.AddCell(ref oPdfPTable, oItem.PINo, true);
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty) + oItem.MUnit, false);
                double a = 0;
                if (oItem.LCID != nLCID)
                {
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", Global.MillionFormat(_oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID).Sum(x => x.value)), nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ShipmentDateStr, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ExpireDateStr, nRowspan);
                }
                nInvoiceID = oItem.InvoiceID;
                nLCID = oItem.LCID;                              
            }
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetLCOpenTable();
            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetLCOpenTable();
            this.AddCell(ref oPdfPTable, "Total", 7);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            this.AddCell(ref oPdfPTable, "", 0);
            this.AddCell(ref oPdfPTable, "", 0);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion
        }
        private PdfPTable GetLCOpenTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    40f,  //File No
                                                    75f,  //LC No
                                                    55f, //ImportLCDate
                                                    150f,  //Supplier Name  
                                                    80f,  //PI No
                                                    180f,  //Commodity                                               
                                                    50f,  //Yet To Inv Qty
                                                    65f,  //Value
                                                    55f,  //ShipmentDT
                                                    55f,  //EXP DT
                                             });
            return oPdfPTable;
        }
        private void PrintCopyDoc(string sHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetCopyDoc();
            _sTableHead = sHeader;


            this.PrintARow(sHeader, ref oPdfPTable, 12);
            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            this.AddCellHeader(ref oPdfPTable, "Shipment DT", true);
            this.AddCellHeader(ref oPdfPTable, "Expire DT", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "U/P", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetCopyDoc();
            #endregion

            int nSL = 0, nLCID = 0, nInvoiceID = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oImportOutStandingDetails = ImportOutStandingDetail.Gets(_nBUID, 0, 0, _CurrencyID, _oImportOutstanding.StartDate.AddYears(-1), _oImportOutstanding.EndDate, 2, (string.IsNullOrEmpty(_oImportOutstanding.ErrorMessage))?0: Convert.ToInt32(_oImportOutstanding.ErrorMessage),_nUserID);
            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
                if (oItem.LCID != nLCID && nSL>0)
                {
                    oPdfPTable.CompleteRow();
                    AddTable(oPdfPTable);
                    oPdfPTable = GetCopyDoc();
                }
                if (oItem.LCID != nLCID)
                {
                    nSL++;
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", nSL.ToString(), nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.LCNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ImportLCDateStr, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ContractorName, nRowspan);
                }
                if (oItem.InvoiceID != nInvoiceID && oItem.LCID == nLCID)
                {
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID && x.LCID == oItem.LCID).Count();
                    this.AddCell(ref oPdfPTable, "", oItem.InvoiceNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.DateofInvoiceStr, nRowspan);
                }
                else if (oItem.LCID != nLCID)
                {
                    this.AddCell(ref oPdfPTable, oItem.InvoiceNo, true);
                    this.AddCell(ref oPdfPTable, oItem.DateofInvoiceStr, true);
                }

                this.AddCell(ref oPdfPTable, oItem.ShipmentDateStr, true);
                this.AddCell(ref oPdfPTable, oItem.ExpireDateStr, true);
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty) + oItem.MUnit, false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.value), false);
                nInvoiceID = oItem.InvoiceID;
                nLCID = oItem.LCID;
            }
            //oPdfPTable.CompleteRow();
            //AddTable(oPdfPTable);
            //oPdfPTable = GetCopyDoc();
            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetCopyDoc();
            this.AddCell(ref oPdfPTable, "Total", 9);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "", true);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        private PdfPTable GetCopyDoc()
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    75f,  //LC No
                                                    55f, //ImportLCDate
                                                    135f,  //Supplier Name  
                                                    75f,  //Invoice No
                                                    55f, //InvDate
                                                    55f,  //SendDate
                                                    55f,  //EXP DT
                                                    142f,  //Commodity  
                                                    55f,  //Qty
                                                    35f,  //UP
                                                    60f,  //LC Value
                                             });
            #endregion
            return oPdfPTable;
        }
        private void PrintShipment(string sHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetShipmentTable();
            _sTableHead = sHeader;


            #region HEADER
            this.PrintARow(sHeader, ref oPdfPTable, 12);
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "BL No", true);
            this.AddCellHeader(ref oPdfPTable, "BL Date", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Unit Price", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetShipmentTable();
            
            #endregion

            int nSL = 0, nLCID = 0, nInvoiceID = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oImportOutStandingDetails = ImportOutStandingDetail.Gets(_nBUID, 0, 0, _CurrencyID, _oImportOutstanding.StartDate.AddYears(-1), _oImportOutstanding.EndDate, 3, (string.IsNullOrEmpty(_oImportOutstanding.ErrorMessage)) ? 0 : Convert.ToInt32(_oImportOutstanding.ErrorMessage), _nUserID);

            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
                if (oItem.LCID != nLCID && nSL > 0)
                {
                    oPdfPTable.CompleteRow();
                    AddTable(oPdfPTable);
                    oPdfPTable = GetShipmentTable();
                }
                if (oItem.LCID != nLCID)
                {
                    nSL++;
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", nSL.ToString(), nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.LCNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ImportLCDateStr, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ContractorName, nRowspan);
                }
                nLCID = oItem.LCID;
                if (oItem.InvoiceID != nInvoiceID && oItem.LCID == nLCID)
                {
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID).Count();
                    this.AddCell(ref oPdfPTable, "", oItem.InvoiceNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.DateofInvoiceStr, nRowspan);
                }
                else if (oItem.LCID != nLCID)
                {
                    this.AddCell(ref oPdfPTable, oItem.InvoiceNo, true);
                    this.AddCell(ref oPdfPTable, oItem.DateofInvoiceStr, true);
                }
                nInvoiceID = oItem.InvoiceID;
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, oItem.BLNo, true);
                this.AddCell(ref oPdfPTable, oItem.BLDateStr, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty) + oItem.MUnit, false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.value), false);
            }
            ////oPdfPTable.CompleteRow();
            if (oPdfPTable.Rows.Count>0)
            AddTable(oPdfPTable);
            ///oPdfPTable = GetShipmentTable();
            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetShipmentTable();
            this.AddCell(ref oPdfPTable, "Total", 9);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "", true);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        private PdfPTable GetShipmentTable()
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    75f,  //LC No
                                                    55f, //ImportLCDate
                                                    132f,  //Supplier Name  
                                                    75f,  //Invoice No
                                                    55f, //InvDate                              
                                                    70f,  //status
                                                    55f,  //File
                                                    120f,  //Commodity 
                                                    55f,  //Qty
                                                    45f,  //UP
                                                    55f,  //LC Value
                                             });
            #endregion
            return oPdfPTable;
        }
        private void PrintDocInBack(string sHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(12);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetDocInBackTable();
            _sTableHead = sHeader;


            #region HEADER
            this.PrintARow(sHeader, ref oPdfPTable, 12);
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            this.AddCellHeader(ref oPdfPTable, "BL No", true);
            this.AddCellHeader(ref oPdfPTable, "BL Date", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Unit Price", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetDocInBackTable();
            
            #endregion

            int nSL = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oImportOutStandingDetails = ImportOutStandingDetail.Gets(_nBUID, 0, 0, _CurrencyID, _oImportOutstanding.StartDate.AddYears(-1), _oImportOutstanding.EndDate, 4, (string.IsNullOrEmpty(_oImportOutstanding.ErrorMessage)) ? 0 : Convert.ToInt32(_oImportOutstanding.ErrorMessage), _nUserID);

            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
                nSL++;
                int nRowspan = 1;
                this.AddCell(ref oPdfPTable, "RIGHT", nSL.ToString(), nRowspan);
                this.AddCell(ref oPdfPTable, "", oItem.LCNo, nRowspan);
                this.AddCell(ref oPdfPTable, "", oItem.ImportLCDateStr, nRowspan);
                this.AddCell(ref oPdfPTable, "", oItem.ContractorName, nRowspan);
                this.AddCell(ref oPdfPTable, "", oItem.InvoiceNo, nRowspan);
                this.AddCell(ref oPdfPTable, "", oItem.DateofInvoiceStr, nRowspan);
                this.AddCell(ref oPdfPTable, oItem.InvoiceNo, true);
                this.AddCell(ref oPdfPTable, oItem.DateofInvoiceStr, true);
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty) + oItem.MUnit, false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.value), false);
                oPdfPTable.CompleteRow();
                AddTable(oPdfPTable);
                oPdfPTable = GetDocInBackTable();
            }
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetDocInBackTable();
            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetDocInBackTable();
            this.AddCell(ref oPdfPTable, "Total", 9);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "", true);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        private PdfPTable GetDocInBackTable()
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    75f,  //LC No
                                                    55f, //ImportLCDate
                                                    132f,  //Supplier Name  
                                                    75f,  //Invoice No
                                                    55f, //InvDate
                                                    70f,  //status
                                                    55f,  //File
                                                    120f,  //Commodity                                               
                                                    55f,  //Qty
                                                    45f,  //UP
                                                    55f,  //LC Value
                                             });
            #endregion
            return oPdfPTable;
        }
        private void PrintDocInCnf(string sHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(14);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetDocInCnfTable();
            _sTableHead = sHeader;


            #region HEADER
            this.PrintARow(sHeader, ref oPdfPTable, 14);
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "FIle No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            this.AddCellHeader(ref oPdfPTable, "Status", true);
            this.AddCellHeader(ref oPdfPTable, "DOC No", true);
            this.AddCellHeader(ref oPdfPTable, "Send Date", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "U/P", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetDocInCnfTable();
            #endregion

            int nSL = 0, nInvoiceID = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oImportOutStandingDetails = _oImportOutStandingDetails.OrderBy(x => x.InvoiceID).ToList();
            _oImportOutStandingDetails = ImportOutStandingDetail.Gets(_nBUID, 0, 0, _CurrencyID, _oImportOutstanding.StartDate.AddYears(-1), _oImportOutstanding.EndDate, 5, (string.IsNullOrEmpty(_oImportOutstanding.ErrorMessage)) ? 0 : Convert.ToInt32(_oImportOutstanding.ErrorMessage), _nUserID);

            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
                nSL++;
                if (oItem.InvoiceID != nInvoiceID && nSL>1)
                {
                    oPdfPTable.CompleteRow();
                    AddTable(oPdfPTable);
                    oPdfPTable = GetDocInCnfTable();
                }
                if (oItem.InvoiceID != nInvoiceID)
                {
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", nSL.ToString(), nRowspan);
                    this.AddCell(ref oPdfPTable, "LEFT", oItem.FileNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.LCNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ImportLCDateStr, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ContractorName, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.InvoiceNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.DateofInvoiceStr, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.InvoiceStatusSt, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.DocNo, nRowspan);
                }
                this.AddCell(ref oPdfPTable, oItem.CnFSendDateStr, true);
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty) + oItem.MUnit, false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                if (oItem.InvoiceID != nInvoiceID)
                {
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", Global.MillionFormat(_oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID).Sum(x => x.value)), nRowspan);
                }
                nInvoiceID = oItem.InvoiceID;
            }
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetDocInCnfTable();
            #endregion
            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetDocInCnfTable();
            this.AddCell(ref oPdfPTable, "Total", 11);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "", true);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);

        }
        private PdfPTable GetDocInCnfTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    20f, //SL No
                                                    60f,  //FILENO 
                                                    75f,  //LC No
                                                    48f, //ImportLCDate
                                                    120f,  //Supplier Name 
                                                    75f,  //Invoice No
                                                    48f, //InvDate
                                                    43f,  //status
                                                    35f,  //DOCNO
                                                    48f,  //SendDate
                                                    127f,  //Commodity  
                                                    55f,  //Qty
                                                    30f,  //UP
                                                    57f,  //LC Value
                                             });
            return oPdfPTable;
        }
        private void PrintGoodsInTransit(string sHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(9);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetGoodsInTransitTable();
            _sTableHead = sHeader;


            #region HEADER
            this.PrintARow(sHeader, ref oPdfPTable, 9);
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            this.AddCellHeader(ref oPdfPTable, "LC No", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Unit Price", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetGoodsInTransitTable();
            
            #endregion

            int nSL = 0, nInvoiceID = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oImportOutStandingDetails = ImportOutStandingDetail.Gets(_nBUID, 0, 0, _CurrencyID, _oImportOutstanding.StartDate.AddYears(-1), _oImportOutstanding.EndDate, 6, (string.IsNullOrEmpty(_oImportOutstanding.ErrorMessage)) ? 0 : Convert.ToInt32(_oImportOutstanding.ErrorMessage), _nUserID);

            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
                if (oItem.InvoiceID != nInvoiceID && nSL > 0)
                {
                    oPdfPTable.CompleteRow();
                    AddTable(oPdfPTable);
                    oPdfPTable = GetGoodsInTransitTable();
                }
                if (oItem.InvoiceID != nInvoiceID)
                {
                    nSL++;
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", nSL.ToString(), nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.InvoiceNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.DateofInvoiceStr, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.LCNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ContractorName, nRowspan);
                }
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty) + oItem.MUnit, false); //+ oItem.MUnit
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.value), false);
                nInvoiceID = oItem.InvoiceID;
            }
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            oPdfPTable = GetGoodsInTransitTable();
            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPTable = GetGoodsInTransitTable();
            this.AddCell(ref oPdfPTable, "Total", 6);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, " ", false);
            this.AddCell(ref oPdfPTable, "$ " + _oImportOutStandingDetails.Sum(x => x.value), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        private PdfPTable GetGoodsInTransitTable()
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    80f,  //Invoice No
                                                    80f, //Inv Date
                                                    80f,  //LC No
                                                    160f,  //Supplier Name                                                
                                                    120f,  //Commodity                                               
                                                    70f,  //LC Qty
                                                    70f,  //UP
                                                    70f,  //LC Value
                                             });
            #endregion
            return oPdfPTable;
        }
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 210f, 421f, 210f });

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
            
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Date: " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year, _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));_oPdfPCell.MinimumHeight = 30;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #endregion
        }
        public void AddCellHeader(ref PdfPTable oPdfPTable, string sHeader, bool IsLeft)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (IsLeft)
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
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
        public void AddCell(ref PdfPTable oPdfPTable, string sHeader, int nColSpan)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = nColSpan;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(ref PdfPTable oPdfPTable, string sAlgin, string sHeader, int nRowspan)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;

            if (sAlgin.Equals("RIGHT"))
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            if (nRowspan > 1)
                _oPdfPCell.Rowspan = nRowspan;
            oPdfPTable.AddCell(_oPdfPCell);
        }
        private void PrintARow(string sHeaderName, ref PdfPTable oPdfPTable, int nColSpan)
        {
            #region  Heading Print
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = nColSpan;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintADateRow(ref PdfPTable oPdfPTable, int nColSpan)
        {
            #region  Heading Print
            _oPdfPCell = new PdfPCell(new Phrase("Date: " + DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = nColSpan;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
        }
        public void PrintRow(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintRowHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_sTableHead, _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void AddTable(PdfPTable oPdfPTable)
        {
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public string GetTotalQty()
        {
            string TotalQty = "";

            var oImportOutstandingDetails = _oImportOutStandingDetails.GroupBy(x => new { x.MUnit }).Select(grp => new
            {
                MUnit = grp.Key.MUnit,
                Qty = grp.Sum(x => x.Qty)
            }).ToList().OrderBy(x => x.MUnit);

            foreach (var oItem in oImportOutstandingDetails)
            {
                TotalQty += oItem.Qty + oItem.MUnit + " ";
            }
            return TotalQty;
        }

    }
}

