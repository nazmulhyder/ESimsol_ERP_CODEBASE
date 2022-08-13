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
    public class rptImportOutstandingDetails
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
        List<ImportOutStandingDetail> _oImportOutStandingDetails = new List<ImportOutStandingDetail>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(List<ImportOutStandingDetail> oImportOutStandingDetails, Company oCompany, BusinessUnit oBusinessUnit,string sHeaderName, string sDetail, string sStartEndDate, int nReportLauout)
        {
            _oImportOutStandingDetails = oImportOutStandingDetails;
            _oImportOutStandingDetails = _oImportOutStandingDetails.OrderBy(x => x.LCID).ThenBy(x => x.InvoiceID).ToList();
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(15f, 15f, 45f, 30f);
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
            _oPdfPTable.SetWidths(new float[] { 842f});
            //this.PrintHeader(sHeaderName,sStartEndDate);

            if (nReportLauout == 1)
            {
                this.PrintHeader(sHeaderName, "L/C Open" + sDetail, sStartEndDate);
                this.PrintLCOpenBody();
            }
            if (nReportLauout == 2)
            {
                this.PrintHeader(sHeaderName, "Doc Recd" + sDetail, sStartEndDate);
                this.PrintCopyDocBody();
            }
            if (nReportLauout == 3)
            {
                this.PrintHeader(sHeaderName, "Shipment" + sDetail, sStartEndDate);
                this.PrintShipmentBody();
            }
            if (nReportLauout == 4)
            {
                this.PrintHeader(sHeaderName, "Doc In Bank" + sDetail, sStartEndDate);
                this.PrintDocInBackBody();
            }
            if (nReportLauout == 5)
            {
                this.PrintHeader(sHeaderName, "Doc In CnF" + sDetail, sStartEndDate);
                this.PrintDocInCnFBody();
            }
            if (nReportLauout == 6)
            {
                this.PrintHeader(sHeaderName, "Goods In Transit" + sDetail, sStartEndDate);
                this.PrintGoodsInTransitBody();
            }
            _oPdfPTable.HeaderRows = 7;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();

            #endregion
        }

        #region Report Header
        private void PrintHeader(string sHeader,string sHeaderTitle, string sStartEndDate)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            this.PrintRow(_oBusinessUnit.Name);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            this.PrintRow(_oBusinessUnit.Address);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE); //Unit
            //this.PrintRow(sHeader);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            this.PrintRow(sHeaderTitle);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            this.PrintRow(sStartEndDate);
        }
        #endregion

        #region Report Body

        #region L/C Open
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
        private void PrintLCOpenBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable = GetLCOpenTable();

            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "File No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "PI No", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Due Inv", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            this.AddCellHeader(ref oPdfPTable, "Shipment DT", true);
            this.AddCellHeader(ref oPdfPTable, "Expire DT", true);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion

            int nSL = 0,nLCID=-9999,nInvoiceID=-9999;
            #region Data
            oPdfPTable = GetLCOpenTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
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
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty)+oItem.MUnit , false);

                //if (oItem.InvoiceID != nInvoiceID)
                //{
                //    int nRowspan = _oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID && x.InvoiceID == oItem.InvoiceID).Count();
                //    this.AddCell(ref oPdfPTable, "RIGHT", Global.MillionFormat(_oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID && x.InvoiceID == oItem.InvoiceID).Sum(x => x.value)), nRowspan);
                //}
                double a = 0;
                if (oItem.LCID != nLCID)
                {
                  

                    int nRowspan = _oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", Global.MillionFormat(_oImportOutStandingDetails.Where(x => x.LCID == oItem.LCID ).Sum(x => x.value)), nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ShipmentDateStr, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ExpireDateStr, nRowspan);
                }
                nInvoiceID = oItem.InvoiceID;
                nLCID = oItem.LCID;
            }
            AddTable(oPdfPTable);
            #endregion

            #region Total
            oPdfPTable = GetLCOpenTable();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable, "Total", 7);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion
        }
        #endregion

        #region Copy Doc
        private PdfPTable GetCopyDocTable()
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
        private void PrintCopyDocBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable = GetCopyDocTable();
            
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
            #endregion

            int nSL = 0, nLCID=0,nInvoiceID=0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            oPdfPTable = GetCopyDocTable();
            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
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
                    //this.AddCell(ref oPdfPTable, oItem.InvoiceStatusSt, true);
                }

                this.AddCell(ref oPdfPTable, oItem.ShipmentDateStr, true);
                this.AddCell(ref oPdfPTable, oItem.ExpireDateStr, true);
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty)+oItem.MUnit, false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.value), false);
                nInvoiceID = oItem.InvoiceID;
            }
            AddTable(oPdfPTable);
            #endregion

            #region Total
            oPdfPTable = GetCopyDocTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable, "Total", 9);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "", true);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        #endregion

        #region Shipment
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
        private void PrintShipmentBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable = GetShipmentTable();
           
            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            //this.AddCellHeader(ref oPdfPTable, "Product Code", false);
            this.AddCellHeader(ref oPdfPTable, "BL No", true);
            this.AddCellHeader(ref oPdfPTable, "BL Date", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Unit Price", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion

            int nSL = 0, nLCID=0, nInvoiceID=0;
            #region Data
            oPdfPTable = GetShipmentTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
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
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty)+oItem.MUnit, false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.value), false);
                //oPdfPTable.CompleteRow();
            }
            AddTable(oPdfPTable);
            #endregion

            #region Total
            oPdfPTable = GetShipmentTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable, "Total", 9);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "", true);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        #endregion

        #region Doc in Back
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
        private void PrintDocInBackBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable = GetDocInBackTable();
            #endregion

            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            //this.AddCell(ref oPdfPTable, "Product Code", false);
            this.AddCellHeader(ref oPdfPTable, "BL No", true);
            this.AddCellHeader(ref oPdfPTable, "BL Date", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Unit Price", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion

            int nSL = 0, nLCID=0, nInvoiceID=0;
            #region Data
            oPdfPTable = GetDocInBackTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
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
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty)+oItem.MUnit, false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.value), false);
                oPdfPTable.CompleteRow();
                AddTable(oPdfPTable);
            }
            #endregion

            #region Total
            oPdfPTable = GetDocInBackTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable, "Total", 9);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "", true);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        #endregion

        #region DocInCnf
        private PdfPTable GetDocInCnfTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    60f,  //FILENO 
                                                    75f,  //LC No
                                                    48f, //ImportLCDate
                                                    140f,  //Supplier Name 
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
        private void PrintDocInCnFBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable = GetDocInCnfTable();
            #endregion

            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "FIle No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C No", true);
            this.AddCellHeader(ref oPdfPTable, "L/C Date", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            //this.AddCell(ref oPdfPTable, "Product Code", false);
            this.AddCellHeader(ref oPdfPTable, "Status", true);
            this.AddCellHeader(ref oPdfPTable, "DOC No", true);
            this.AddCellHeader(ref oPdfPTable, "Send Date", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "U/P", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);

            int nSL = 0, nInvoiceID = 0;
            #region Data
            oPdfPTable = GetDocInCnfTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oImportOutStandingDetails=_oImportOutStandingDetails.OrderBy(x => x.InvoiceID).ToList();
            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
                if(oItem.InvoiceID != nInvoiceID) 
                {
                    nSL++;
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID).Count();
                    this.AddCell(ref oPdfPTable,"LEFT", oItem.FileNo, nRowspan);
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
                oPdfPTable.CompleteRow();
                nInvoiceID = oItem.InvoiceID;
            }
            #endregion
            AddTable(oPdfPTable);
            #region Total
            oPdfPTable = GetDocInCnfTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable, "Total", 10);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, "", true);
            this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(_oImportOutStandingDetails.Sum(x => x.value)), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        #endregion

        #region Goods In Transit
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
        private void PrintGoodsInTransitBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable = GetGoodsInTransitTable();
            #region HEADER
            this.AddCellHeader(ref oPdfPTable, "SL No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice No", true);
            this.AddCellHeader(ref oPdfPTable, "Invoice Date", true);
            this.AddCellHeader(ref oPdfPTable, "LC No", true);
            this.AddCellHeader(ref oPdfPTable, "Supplier Name", true);
            this.AddCellHeader(ref oPdfPTable, "Commodity", true);
            this.AddCellHeader(ref oPdfPTable, "Qty", false);
            this.AddCellHeader(ref oPdfPTable, "Unit Price", false);
            this.AddCellHeader(ref oPdfPTable, "Value", false);
            //oPdfPTable.CompleteRow();
            AddTable(oPdfPTable);
            #endregion

            int nSL = 0, nLCID=0, nInvoiceID=0;
            #region Data
            oPdfPTable = GetGoodsInTransitTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (ImportOutStandingDetail oItem in _oImportOutStandingDetails)
            {
                if (oItem.InvoiceID != nInvoiceID)
                {
                    nSL++;
                    int nRowspan = _oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID).Count();
                    this.AddCell(ref oPdfPTable, "RIGHT", nSL.ToString(), nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.InvoiceNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.DateofInvoiceStr, nRowspan);
                    //this.AddCell(ref oPdfPTable, "", oItem.ContractorName, nRowspan);
                //}
                //nInvoiceID = oItem.InvoiceID;

                ////oItem.LCID != nLCID &&
                //if (oItem.InvoiceID == nInvoiceID)
                //{
                    //int nRowspan = _oImportOutStandingDetails.Where(x => x.InvoiceID == oItem.InvoiceID).Count();
                    this.AddCell(ref oPdfPTable, "", oItem.LCNo, nRowspan);
                    this.AddCell(ref oPdfPTable, "", oItem.ContractorName, nRowspan);
                //}
                //else if (oItem.InvoiceID != nInvoiceID)
                //{
                    //this.AddCell(ref oPdfPTable, oItem.LCNo, nRowspan);
                    //this.AddCell(ref oPdfPTable, oItem.ContractorName, nRowspan);
                }
                //nLCID = oItem.LCID;
            //}
                this.AddCell(ref oPdfPTable, oItem.ProductName, true);
                this.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Qty)+oItem.MUnit, false); //+ oItem.MUnit
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.UnitPrice), false);
                this.AddCell(ref oPdfPTable, "$ " + Global.MillionFormat(oItem.value), false);
                //oPdfPTable.CompleteRow();
                nInvoiceID = oItem.InvoiceID;
            }
            AddTable(oPdfPTable);
            #endregion

            #region Total
            oPdfPTable = GetGoodsInTransitTable();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell(ref oPdfPTable, "Total", 6);
            this.AddCell(ref oPdfPTable, GetTotalQty(), false);
            this.AddCell(ref oPdfPTable, " ", false);
            this.AddCell(ref oPdfPTable, "$ " + _oImportOutStandingDetails.Sum(x => x.value), false);
            oPdfPTable.CompleteRow();
            #endregion
            AddTable(oPdfPTable);
        }
        #endregion

        #endregion
        public string GetTotalQty() 
        {
            string TotalQty = "";
           
            var oImportOutstandingDetails = _oImportOutStandingDetails.GroupBy(x => new{x.MUnit}).Select(grp => new
                {
                    MUnit = grp.Key.MUnit,
                    Qty = grp.Sum(x=>x.Qty)
                }).ToList().OrderBy(x => x.MUnit);

            foreach (var oItem in oImportOutstandingDetails)
            {
                TotalQty += oItem.Qty + oItem.MUnit+" ";
            }
            return TotalQty; 
        }

        #region PDF HELPER
       
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
            if (nRowspan>1)
            _oPdfPCell.Rowspan = nRowspan;
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
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
    }
}
