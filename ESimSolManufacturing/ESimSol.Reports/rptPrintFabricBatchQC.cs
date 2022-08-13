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
    public class rptPrintFabricBatchQC
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
        FabricBatchQC _oFabricBatchQC = new FabricBatchQC();
        FabricSCReport _oFabricSCReport = new FabricSCReport();
        FabricExecutionOrderSpecification _oFEOS= new FabricExecutionOrderSpecification();
        List<FabricBatchQCDetail> _oFabricBatchQCDetails = new List<FabricBatchQCDetail>();
        List<FabricBatchQCCheck> _FabricBatchQCChecks = new List<FabricBatchQCCheck>();
        Company _oCompany = new Company();
        List<ApprovalHead> _oApprovalHeads = new List<ApprovalHead>();
        #endregion

        public byte[] PrepareReport(FabricBatchQC oFabricBatchQC, List<FabricBatchQCCheck> oFabricBatchQCChecks, Company oCompany, List<ApprovalHead> oApprovalHeads, FabricSCReport oFabricSCReport, FabricExecutionOrderSpecification oFEOS)
        {
            _oFabricBatchQC = oFabricBatchQC;
            _oFabricBatchQCDetails = oFabricBatchQC.FabricBatchQCDetails;
            _oFabricSCReport = oFabricSCReport;
            _oFEOS = oFEOS;
            _FabricBatchQCChecks = oFabricBatchQCChecks;
            _oCompany = oCompany;
            _oApprovalHeads = oApprovalHeads;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
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
            this.PrintHeaderDetail("Quality Assurance Department", " ", " ");
            this.Set1stPart();
            this.Set2ndPart();
            this.Set3rdPart();
            this.SetSignature();
        }
        #endregion

        private void Set3rdPart()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 50f, 8f, 42f });

            _oPdfPCell = new PdfPCell(FirstTableOf3rdPart()); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Swatch", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 50f, 8f, 42f });
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Note: ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns; _oPdfPCell.FixedHeight = 60f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
        }

        private void Set2ndPart()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 35f, 25f, 20f, 20f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabric Details : ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 8f, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            #region Object
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction(Finished)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.Construction + "        ->[" + _oFabricSCReport.ConstructionPI + "]", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weave/Design", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.Reed.ToString() + "/" + _oFEOS.Dent.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Loom No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFabricBatchQCDetails.Count<=0)? "": _oFabricBatchQCDetails[0].LoomNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige EPI", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.Ends.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Matching With/Reference", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige PPI", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEOS.Picks.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Required Greige Width", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(Math.Round(_oFEOS.RequiredWarpLength)) + " Meter", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige Width", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

            double nTotalEnds = 0;
            if (_oFEOS.IsTEndsAdd) { nTotalEnds = Math.Round(_oFEOS.TotalEnds + _oFEOS.TotalEndsAdd); } else { nTotalEnds = Math.Round(_oFEOS.TotalEnds - _oFEOS.TotalEndsAdd); }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat((nTotalEnds) / _oFEOS.Ends) + "\xB''", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finished Width(Inch)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFEOS.IsOutSide == true && _oFEOS.ContractorID > 0) ? "" : Math.Round(_oFEOS.FinishWidth - 1, 2).ToString() + "\xB''" + "/" + Math.Round(_oFEOS.FinishWidth, 2).ToString() + "\xB''"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reed Width(Inch)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFEOS.ReedWidth), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 15f, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

        }

        private void Set1stPart()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 35f, 25f, 20f, 20f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige Fabric First Metre Approval Sheet", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 8f, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            #region Object
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date & Shift", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQC.QCStartDateTime.ToString("dd MMM yy") + " " + ((_oFabricBatchQCDetails.Count <= 0) ? "" : _oFabricBatchQCDetails[0].ShiftName), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Greige Order Quantity(Meter)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(Math.Round(_oFEOS.OrderQtyInMeter)) + " Meter", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Type", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.FinishTypeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricSCReport.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 15f, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

        }

        private PdfPTable FirstTableOf3rdPart()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 20f, 50f, 40f });

            #region Object
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Checking Parameter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Comments", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            int nCount = 1;
            foreach (FabricBatchQCCheck oItem in _FabricBatchQCChecks)
            {
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                nCount++;
                oPdfPTable.CompleteRow();
            }
            #endregion

            return oPdfPTable;
        }

        private void SetSignature()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            float nTableHeight = CalculatePdfPTableHeight(_oPdfPTable);
            float _nfixedHight = 790 - (float)nTableHeight;
            if (_nfixedHight > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = _nfixedHight;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


            #region Signature
            string[] signatureList = new string[_oApprovalHeads.Count];
            string[] dataList = new string[_oApprovalHeads.Count];

            for (int j = 1; j <= _oApprovalHeads.Count; j++)
            {
                signatureList[j - 1] = (_oApprovalHeads[j - 1].Name);
                dataList[j - 1] = "";
            }

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(this.GetSignature(535f, dataList, signatureList, 20f)); _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = _oPdfPTable.NumberOfColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion


        }

        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 535f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
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
