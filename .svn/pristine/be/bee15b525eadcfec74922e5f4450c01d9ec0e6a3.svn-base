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
    public class rptFabricBatchQCStockReport
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricBatchQC> _oFabricBatchQCs = new List<FabricBatchQC>();
        List<FabricBatchQCDetail> _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
        List<FabricExecutionOrderSpecification> _oFEOSs = new List<FabricExecutionOrderSpecification>();
        Company _oCompany = new Company();
        double tFinishQtyM = 0, tGreigeQtyM = 0, tGoodQtyM = 0, tGreen1QtyM = 0, tGreen2QtyM = 0, tGoodQty = 0, tRejQtyM = 0,tHardRejQty = 0, tStockInHand = 0;
        string sHeaderStatus = "";
        List<FabricQCGrade> _oFabricQCGrades = new List<FabricQCGrade>();
        #endregion

        public byte[] PrepareReport(List<FabricBatchQC> oFabricBatchQCs, List<FabricBatchQCDetail> oFabricBatchQCDetails, List<FabricExecutionOrderSpecification> oFEOSs, Company oCompany, string sMsg, List<FabricQCGrade> oFabricQCGrades)
        {
            _oFabricBatchQCs = oFabricBatchQCs;
            _oFabricBatchQCDetails = oFabricBatchQCDetails;
            _oFEOSs = oFEOSs;
            _oCompany = oCompany;
            sHeaderStatus = sMsg;
            _oFabricQCGrades = oFabricQCGrades;

            #region Page Setup
            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.LEDGER);
            _oDocument.SetMargins(15f, 15f, 10f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header

        //private void PrintHeader()
        //{
        //    #region CompanyHeader


        //    PdfPTable oPdfPTable = new PdfPTable(3);
        //    oPdfPTable.WidthPercentage = 100;
        //    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
        //    if (_oCompany.CompanyLogo != null)
        //    {
        //        _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        _oImag.ScaleAbsolute(70f, 40f);
        //        _oPdfPCell = new PdfPCell(_oImag);
        //    }
        //    else
        //    {
        //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    }
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
        //    _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    oPdfPTable.CompleteRow();


        //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
        //    _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    oPdfPTable.CompleteRow();

        //    _oPdfPCell = new PdfPCell(oPdfPTable);
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();


        //    #region ReportHeader
        //    #region Blank Space
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    #endregion
        //    #endregion

        //    #endregion
        //}
        private void PrintHeaderDetail(string sReportHead,string reporttDateRange, string sOrderType, string sDate)
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

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, reporttDateRange, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sDate, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sOrderType, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reporting Date:" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion


        }
        //private void PrintHeaderDetail2(string sReportHead, string sOrderType, string sDate)
        //{
        //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
        //    PdfPTable oPdfPTable = new PdfPTable(3);
        //    oPdfPTable.SetWidths(new float[] { 170f, 200f, 170f });
        //    #region
        //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
        //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
        //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
        //    oPdfPTable.CompleteRow();

        //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
        //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLDITALIC));
        //    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);
        //    oPdfPTable.CompleteRow();

        //    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
        //    #endregion


        //}
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintHeaderDetail("Stock Report",sHeaderStatus, " ", " ");
            this.SetData();
        }
        #endregion

        private PdfPTable InitializeTable()
        {
            int nCout = -1;
            int ColumnNumber = 15 + _oFabricQCGrades.Count;
            PdfPTable oPdfPTable = new PdfPTable(ColumnNumber);
            float[] ColWidthList = new float[ColumnNumber];
            ColWidthList[++nCout] = 60f;//diso
            ColWidthList[++nCout] = 60f;//batch
            ColWidthList[++nCout] = 75f;//customer
            ColWidthList[++nCout] = 60f;//construction
            ColWidthList[++nCout] = 55f;//fabric type
            ColWidthList[++nCout] = 60f;//req. no
            ColWidthList[++nCout] = 50f;//finish qty
            ColWidthList[++nCout] = 50f;//greige qty
            ColWidthList[++nCout] = 50f;//1st receive

            var oGradesForA = _oFabricQCGrades.Where(x => x.QCGradeType == EnumFBQCGrade.A).ToList();
            for (int i = nCout + 1; i < (nCout + 1 + oGradesForA.Count); i++)
                ColWidthList[i] = 50f;
            nCout += oGradesForA.Count;
            ColWidthList[++nCout] = 70f;//good Cause rejection
            ColWidthList[++nCout] = 60f;//Total good qty

            var oGradesForB = _oFabricQCGrades.Where(x => x.QCGradeType == EnumFBQCGrade.B).ToList();
            for (int i = nCout + 1; i < (nCout + 1 + oGradesForB.Count); i++)
                ColWidthList[i] = 60f;
            nCout += oGradesForB.Count;
            ColWidthList[++nCout] = 70f;//cause of rejection

            var oGradesForReject = _oFabricQCGrades.Where(x => x.QCGradeType == EnumFBQCGrade.Reject).ToList();
            for (int i = nCout + 1; i < (nCout + 1 + oGradesForReject.Count); i++)
                ColWidthList[i] = 60f;
            nCout += oGradesForReject.Count;
            ColWidthList[++nCout] = 70f;//cause of rejection
            ColWidthList[++nCout] = 70f;//Total Qty
            ColWidthList[++nCout] = 70f;//Stock in hand

            oPdfPTable.SetWidths(ColWidthList);

            return oPdfPTable;
        }

        private void SetData()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            int ColumnNumber = 15 + _oFabricQCGrades.Count;
            PdfPTable oPdfPTable = new PdfPTable(ColumnNumber);
            oPdfPTable = InitializeTable();

            #region Heder Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Batch No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabric Type", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Req. No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Qty(M)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige Qty(M)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "1st Received", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            var oGradesForA = _oFabricQCGrades.Where(x => x.QCGradeType == EnumFBQCGrade.A).ToList();
            for (int i = 0; i < oGradesForA.Count; i++)
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oGradesForA[i].Name + " Qty(M)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Green-2 Major Cause Of Rejection", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Good Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            var oGradesForB = _oFabricQCGrades.Where(x => x.QCGradeType == EnumFBQCGrade.B).ToList();
            for (int i = 0; i < oGradesForB.Count; i++)
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oGradesForB[i].Name + " Qty(M)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Major Cause Of Rejection", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            var oGradesForReject = _oFabricQCGrades.Where(x => x.QCGradeType == EnumFBQCGrade.Reject).ToList();
            for (int i = 0; i < oGradesForReject.Count; i++)
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oGradesForReject[i].Name + " Qty(M)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Major Cause Of Rejection", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Stock in Hand", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region data
            List<FabricBatchQCDetail> oTempFabricBatchQCDetails = new List<FabricBatchQCDetail>();
            FabricExecutionOrderSpecification _oTempFEOS = new FabricExecutionOrderSpecification();
            foreach (var oItem in _oFabricBatchQCs)
            {
                oPdfPTable = new PdfPTable(ColumnNumber);
                oPdfPTable = InitializeTable();
                oTempFabricBatchQCDetails = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID).OrderBy(y => y.StoreRcvDate).ToList();
                _oTempFEOS = _oFEOSs.Where(x => x.FEOSID == oItem.FEOSID).FirstOrDefault();
                if (_oTempFEOS == null) _oTempFEOS = new FabricExecutionOrderSpecification();

                #region Data
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oTempFEOS.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oTempFEOS.FESONo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oTempFEOS.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oTempFEOS.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oTempFEOS.Weave, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oTempFEOS.FabricNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oTempFEOS.GreigeDemand.ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oTempFEOS.RequiredWarpLength.ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                if (oTempFabricBatchQCDetails.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchQCDetails[0].StoreRcvDateInString, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    //For A
                    for (int i = 0; i < oGradesForA.Count; i++)
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchQCDetails.Where(x => x.Grade == oGradesForA[i].QCGradeType && x.FabricQCGradeID == oGradesForA[i].FabricQCGradeID).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle); //Goods
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, string.Join(",", oTempFabricBatchQCDetails.Where(x => x.Grade == EnumFBQCGrade.A).Select(c=>c.Remark)), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchQCDetails.Where(x => x.Grade == EnumFBQCGrade.A).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    //For B
                    for (int i = 0; i < oGradesForB.Count; i++)
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchQCDetails.Where(x => x.Grade == oGradesForB[i].QCGradeType && x.FabricQCGradeID == oGradesForB[i].FabricQCGradeID).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, string.Join(",", oTempFabricBatchQCDetails.Where(x => x.Grade == EnumFBQCGrade.B).Select(c => c.Remark)), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    //For Reject
                    for (int i = 0; i < oGradesForReject.Count; i++)
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchQCDetails.Where(x => x.Grade == oGradesForReject[i].QCGradeType && x.FabricQCGradeID == oGradesForReject[i].FabricQCGradeID).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, string.Join(",", oTempFabricBatchQCDetails.Where(x => x.Grade == EnumFBQCGrade.Reject).Select(c => c.Remark)), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchQCDetails.Where(x => (x.Grade == EnumFBQCGrade.A || x.Grade == EnumFBQCGrade.B || x.Grade == EnumFBQCGrade.Reject)).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oTempFabricBatchQCDetails.Where(x => x.LotBalance > 0).Sum(y => y.LotBalance).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                }
                else
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "-", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    for (int i = 0; i < oGradesForA.Count; i++)
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "0.00", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle); //Goods
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "0.00", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    //For B
                    for (int i = 0; i < oGradesForB.Count; i++)
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "0.00", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    //For Reject
                    for (int i = 0; i < oGradesForReject.Count; i++)
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "0.00", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "0.00", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "0.00", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                }

                tFinishQtyM += _oTempFEOS.RequiredWarpLength;
                tGreigeQtyM += _oTempFEOS.GreigeDemand;
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion
            }

            #region TOTAL
            oPdfPTable = new PdfPTable(ColumnNumber);
            oPdfPTable = InitializeTable();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, tFinishQtyM.ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, tGreigeQtyM.ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

            for (int i = 0; i < oGradesForA.Count; i++)
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.Grade == oGradesForA[i].QCGradeType && x.FabricQCGradeID == oGradesForA[i].FabricQCGradeID).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.Grade == EnumFBQCGrade.A).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            for (int i = 0; i < oGradesForB.Count; i++)
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.Grade == oGradesForB[i].QCGradeType && x.FabricQCGradeID == oGradesForB[i].FabricQCGradeID).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            for (int i = 0; i < oGradesForReject.Count; i++)
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.Grade == oGradesForReject[i].QCGradeType && x.FabricQCGradeID == oGradesForReject[i].FabricQCGradeID).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.Grade == EnumFBQCGrade.A || x.Grade == EnumFBQCGrade.B || x.Grade == EnumFBQCGrade.Reject)).Sum(y => y.QtyInMeter).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.LotBalance > 0).Sum(y => y.LotBalance).ToString("###,0.00;(###,0.00)"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
            #endregion
        }

    }
}
