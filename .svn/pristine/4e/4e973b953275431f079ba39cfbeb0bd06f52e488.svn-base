using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ESimSol.Reports;
using System.IO;//--
using System.Collections.Generic;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;

namespace ESimSol.Reports
{
    public class rptFNPackingTransferNew
    {
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        int _nColumns = 1;
        double _nQty = 0;
        int _nCount = 0;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        Company _oCompany = new Company();
        MemoryStream _oMemoryStream = new MemoryStream();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        public iTextSharp.text.Image _oImag { get; set; }
        BaseColor oCustom_Color = ESimSolPdfHelper.Custom_BaseColor(new int[] { 67, 127, 117 });
        int nRowHeight = 14;
        List<ApprovalHead> _oApprovalHeads = new System.Collections.Generic.List<ApprovalHead>();
        bool _bIsMeter = false;
        public byte[] PrepareReport(FNBatchQC oFNBatchQC, List<FNBatchQCDetail> oFNBatchQCDetails, List<FNBatchQCFault> oFNBatchQCFaults, Company oCompany, BusinessUnit oBusinessUnit, List<ApprovalHead> oApprovalHeads, bool IsMeter)
        {
            #region Page Setup
            _bIsMeter = IsMeter;
            _oBusinessUnit = oBusinessUnit;
            _oApprovalHeads = oApprovalHeads;
            _oCompany = oCompany;
            _oDocument = new Document(PageSize.A4, 5f, 5f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 95; // Use 100% of the page
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER; // Page Center Position

            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler  

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 560f });
            #endregion

            #region Document Data/ Print
            this.PrintHeader("",false);
            PageHeader(oCompany, oBusinessUnit);
            PrintBody(oFNBatchQC, oFNBatchQCDetails, oFNBatchQCFaults);
            #endregion

           
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintHeader(string sTitle, bool isB)
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            //#region ReportHeader
            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase(sTitle, FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, (isB ? oCustom_Color : BaseColor.BLACK))));
            //_oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 0; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.NORMAL)));
            //_oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 5f; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            //#region DateTime
            //_oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd MMM yyy HH:mm"), _oFontStyle));
            //_oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.ExtraParagraphSpace = 10f; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

        }
        private void PageHeader(Company oCompany, BusinessUnit oBusinessUnit)
        {

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 150f, 150f, 150f });
         
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL | Font.UNDERLINE);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Inspection Report", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 20, _oFontStyle);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finishing Unit", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 20, _oFontStyle);
            oPdfPTable.CompleteRow();
          //  ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

           // _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
           // ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Finishing Unit", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle, true);

           //_oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL | Font.UNDERLINE);
           //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "Inspection Report", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, 20, _oFontStyle, true);
        }

        private static PdfPTable Table_Info()
        {
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    40f,
                                                    4f,
                                                    200f, 
                                                    65f,
                                                    150f,
                                             });
            return oPdfPTable;

        }

        private static PdfPTable Table_FNQCDetail()
        {
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   25f, //"SL", 
                                                   51f, //"Roll No"
                                                   45f, //"Roll length In Yds", 
                                                   45f, //"Shade"
                                                   60f, //"Cuttable width(Inch)",
                                                   60f, // "GSM", 
                                                   45f, // "Bowl Skew(%)",
                                                   45f, // "Points Per roll",
                                                   51f, // "Points Per 100sq Yds",
                                                   45f, // "Pass/Fail" 
                                             });
            return oPdfPTable;
        }

        //private void MakeTableHeader(ref PdfPTable oPdfPTable)
        //{
        //    Font oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

        //    string[] tableHeader = new string[] { "SL", "Roll No", "Qty(Y)", "Shade", "Total Points Per 100sq Yds", "Point", "Pass" };
        //    foreach (string head in tableHeader)
        //    {
        //        ESimSolItexSharp.SetCellValue(ref oPdfPTable, head, 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, oFontStyleBold);
        //    }
        //    oPdfPTable.CompleteRow();
        //}
        private void MakeTableHeader(ref PdfPTable oPdfPTable)
        {
            Font oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            Font oFontStyleHeaderBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            string[] tableHeader = 
                new string[] { 
                    "SL",
                    "Roll No", 
                    "Roll length "+((_bIsMeter)?"In Meter":"In Yds"), 
                    "Shade",
                    "Cuttable width(Inch)",
                    "GSM", 
                    "Bowl Skew(%)",
                    "Points Per roll",
                    "Points Per 100sq "+((_bIsMeter)?" Meter":" Yds"), 
                    "Pass/Fail" 
                };
            foreach (string head in tableHeader)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, head, 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, oFontStyleHeaderBold);
            }
            oPdfPTable.CompleteRow();
        }

        private void SetValueToParentTable(ref int nStartIndex, int nIteration, int nData, FNBatchQC oFNBatchQC, List<FNBatchQCDetail> oFNBatchQCDetails, List<FNBatchQCFault> oFNBatchQCFaults)
        {
            Font oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = Table_FNQCDetail();
            int i = 0;

            double nInspectionPoint = 0;
            double nFaultPoint = 0;
            double nTFaultPoint = 0;
            _nQty = 0;
            _nCount = 0;
            MakeTableHeader(ref oPdfPTable);
            for (i = nStartIndex; i < nIteration + nStartIndex; i++)
            {
                nInspectionPoint = 0;
                nFaultPoint = 0;
                nTFaultPoint = 0;
                if (i < nData)
                {
                    nTFaultPoint = (oFNBatchQCFaults.Where(x => x.FNBatchQCDetailID == oFNBatchQCDetails[i].FNBatchQCDetailID).Sum(p => (p.FaultPoint * p.NoOfFault)));
                    nFaultPoint = nTFaultPoint * 36 * 100;  //(oFNBatchQCFaults.Where(x => x.FNBatchQCDetailID == oFNBatchQCDetails[i].FNBatchQCDetailID).Sum(p => (p.FaultPoint * p.NoOfFault)) * 36 * 100);
                    var nWidth = oFNBatchQCDetails[i].Qty * oFNBatchQC.ActualWidth;
                    if (nFaultPoint > 0 && nWidth > 0)
                        nInspectionPoint = (nFaultPoint / nWidth);
                    _nQty = _nQty + oFNBatchQCDetails[i].Qty;
                    _nCount = _nCount + 1;
                }
          
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i + 1).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFNBatchQCDetails[i].LotNo : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? Global.MillionFormat(oFNBatchQCDetails[i].Qty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFNBatchQCDetails[i].ShadeStr : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFNBatchQC.ActualWidth.ToString() : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFNBatchQCDetails[i].GSM.ToString() : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFNBatchQCDetails[i].Bowl_Skew.ToString() : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nTFaultPoint <= 0)) ? "" : nTFaultPoint.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nInspectionPoint), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (i < nData) ? oFNBatchQCDetails[i].IsPassedStr : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, nRowHeight, oFontStyle);

            }
            nStartIndex = i;
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "N.B: All rolls are inspected in four point system", 0, 7, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: " + Global.MillionFormat(_nQty)+((_bIsMeter) ? " M" : " Y") +"   ||   Rolls: " + _nCount.ToString(), 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, false);

            _oPdfPTable.CompleteRow();
        }

        private void PrintBody(FNBatchQC oFNBatchQC, List<FNBatchQCDetail> oFNBatchQCDetails, List<FNBatchQCFault> oFNBatchQCFaults)
        {
            Font oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            Font oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = null;

            

            #region Table Rows
            if (oFNBatchQCDetails.Any())
            {
                int nData = oFNBatchQCDetails.Count();
                double nQtyl=oFNBatchQCDetails.Sum(x => x.Qty);
                //int nRows = (nData / 2) + ((nData % 2 == 0) ? 0 : 1);
                int nRows = nData;
                int nCount = 0;
                for (int i = 0; i < nRows; i++)
                {
                    #region Top Part
                    nRowHeight = 0;
                    oPdfPTable = Table_Info();
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.FNExONo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Consturction:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.ConstructionPI, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    oPdfPTable.CompleteRow();

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabrics Type:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    oPdfPTable.CompleteRow();

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.Color, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finsihed Type:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.FinishTypeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    oPdfPTable.CompleteRow();

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_bIsMeter) ? Global.MillionFormat(Global.GetMeter(oFNBatchQC.ExeQty,5))+" M" : Global.MillionFormat(oFNBatchQC.ExeQty)+"Y")  + "  Inspection Qty: " + Global.MillionFormat(nQtyl) + ((_bIsMeter) ? " M" : " Y"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finsihed Width:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.FinishWidth, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    oPdfPTable.CompleteRow();

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Style No ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.StyleNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Ref :", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFNBatchQC.BuyerRef, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    oPdfPTable.CompleteRow();
                    nRowHeight = 14;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, nRowHeight, oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    oPdfPTable.CompleteRow();

                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, nRowHeight, oFontStyle, true);
                    #endregion

                    //if (i < 40)
                    //{
                    //    SetValueToParentTable(ref nCount, 40, nData, oFNBatchQC, oFNBatchQCDetails, oFNBatchQCFaults);
                    //    i += 40;
                    //}
                    //else
                    //{
                    //    SetValueToParentTable(ref nCount, 42, nData, oFNBatchQC, oFNBatchQCDetails, oFNBatchQCFaults);
                    //    i += 42;
                    //}

                    SetValueToParentTable(ref nCount, 42, nData, oFNBatchQC, oFNBatchQCDetails, oFNBatchQCFaults);
                    i += 42;

                    //oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

                    //oPdfPTable = Table_Info();
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "N.B: All rolls are inspected in four point system", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, false, nRowHeight, oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total: " + Global.MillionFormat(oFNBatchQCDetails.Sum(x => x.Qty)) + " Y   ||   Rolls: " + oFNBatchQCDetails.Count(), 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyleBold);
                    //oPdfPTable.CompleteRow();
                    //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                    oPdfPTable = Table_Info();
                    string[] signatureList = new string[_oApprovalHeads.Count];
                    string[] dataList = new string[_oApprovalHeads.Count];

                    for (int j = 1; j <= _oApprovalHeads.Count; j++)
                    {
                        signatureList[j - 1] = (_oApprovalHeads[j - 1].Name);
                        dataList[j - 1] = "";
                    }

                    #region Authorized Signature
                    _oPdfPCell = new PdfPCell(this.GetSignature(535f, dataList, signatureList, 20f)); _oPdfPCell.Border = 0;
                    _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
            }
            #endregion

        }
        private PdfPTable GetSignature(float nTableWidth, string[] oData, string[] oSignatureSetups, float nBlankRowHeight)
        {
            iTextSharp.text.Font _oFontStyle;
            PdfPCell _oPdfPCell;
            int nSignatureCount = oSignatureSetups.Length;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (nSignatureCount <= 0)
            {
                #region Blank Table
                PdfPTable oPdfPTable = new PdfPTable(1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { nTableWidth });

                if (nBlankRowHeight <= 0)
                {
                    nBlankRowHeight = 10f;
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                return oPdfPTable;
                #endregion
            }
            else
            {

                PdfPTable oPdfPTable = new PdfPTable(nSignatureCount);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                #region

                #endregion

                int nColumnCount = -1;
                float[] columnArray = new float[nSignatureCount];
                foreach (string oItem in oSignatureSetups)
                {
                    nColumnCount++;
                    columnArray[nColumnCount] = nTableWidth / nSignatureCount;
                }
                oPdfPTable.SetWidths(columnArray);

                #region Blank Row
                if (nBlankRowHeight > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Colspan = nSignatureCount; _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion

                nColumnCount = 0;
                for (int i = 0; i < oSignatureSetups.Length; i++)
                {
                    if (nSignatureCount == 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else if (nSignatureCount == 2)
                    {
                        if (nColumnCount == 0)
                        {

                            _oPdfPCell = new PdfPCell(new Phrase(" " + oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oData[i] + "\n_________________\n" + oSignatureSetups[i] + " ", _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    nColumnCount++;
                }
                return oPdfPTable;
            }
        }
    }
}
