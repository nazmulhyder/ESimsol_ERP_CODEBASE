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
    public class rptPrintGreigeReport
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
        List<FabricQCGrade> _oFabricQCGrades = new List<FabricQCGrade>();
        Company _oCompany = new Company();
        string _sDateRangeText = "";
        #endregion

        public byte[] PrepareReport(List<FabricBatchQC> oFabricBatchQCs, List<FabricBatchQCDetail> oFabricBatchQCDetails, List<FabricQCGrade> oFabricQCGrades, Company oCompany, string sDateRangeText)
        {
            _oFabricBatchQCs = oFabricBatchQCs;
            _oFabricBatchQCDetails = oFabricBatchQCDetails;
            _oFabricQCGrades = oFabricQCGrades;
            _oCompany = oCompany;
            _sDateRangeText = sDateRangeText;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(15f, 15f, 5f, 25f);
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
            _oPdfPTable.HeaderRows = 3;
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
            oPdfPTable.SetWidths(new float[] { 150f, 240f, 150f });
            #region
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sReportHead, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 250f, 40f, 250f });
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
            this.PrintHeaderDetail("Daily Greige Inspection Production Report", " ", _sDateRangeText);
            this.SetData();
            this.SetSummery();
        }
        #endregion

        private void SetData()
        {
            int nSLNo = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(25);
            oPdfPTable.SetWidths(new float[] { 25f, 60f, 85f, 80f,   25f,25f,25,25f,25f,   35f,   25f,25f,25f,25f,25f,    35f,      25f,25f,25f,25f,25f,    35f,       40f, 40f, 40f });

            #region Heder Info
            oPdfPTable = new PdfPTable(25);
            oPdfPTable.SetWidths(new float[] { 25f, 60f, 85f, 80f, 25f, 25f, 25, 25f, 25f, 35f, 25f, 25f, 25f, 25f, 25f, 35f, 25f, 25f, 25f, 25f, 25f, 35f, 40f, 40f, 40f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "#SL", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "<12 Point (Black)", 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ">12 to 16 (Green-1)", 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ">16 to 20 (Green-2)", 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "20 Point Up (Red-1)", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Hard Reject Not Issued (Red-2)", 2, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Quantity (Mtr)", 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Quantity (Mtr)", 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Quantity (Mtr)", 0, 5, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reject With Issued", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Reject Without Issued", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region data
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);

            foreach (var oItem in _oFabricBatchQCs)
            {
                oPdfPTable = new PdfPTable(25);
                oPdfPTable.SetWidths(new float[] { 25f, 60f, 85f, 80f, 25f, 25f, 25, 25f, 25f, 35f, 25f, 25f, 25f, 25f, 25f, 35f, 25f, 25f, 25f, 25f, 25f, 35f, 40f, 40f, 40f });

                List<FabricBatchQCDetail> oDetail0_12 = new List<FabricBatchQCDetail>();
                oDetail0_12 = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.A).ToList();
                List<FabricBatchQCDetail> oDetail12_16 = new List<FabricBatchQCDetail>();
                oDetail12_16 = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.B).ToList();
                List<FabricBatchQCDetail> oDetail16_20 = new List<FabricBatchQCDetail>();
                oDetail16_20 = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.C).ToList();

                int n0_12 = (int)Math.Ceiling((double)(oDetail0_12.Count() / 5.00));
                int n12_16 = (int)Math.Ceiling((double)(oDetail12_16.Count() / 5.00));
                int n16_20 = (int)Math.Ceiling((double)(oDetail16_20.Count() / 5.00));
                int nRow = Math.Max(Math.Max(n0_12, n12_16),n16_20);
                n0_12 = 0; n12_16 = 0; n16_20 = 0;
                if(nRow > 0)
                {
                    for (int i = 0; i < nRow; i++)
                    {
                        if (i == 0)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), nRow, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FEONo, nRow, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BuyerName, nRow, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Construction, nRow, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        }
                        double n0_12TotalQty = 0;
                        for (int j = 0; j < 5; j++)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail0_12.Count > n0_12) ? oDetail0_12[n0_12].QtyInMeter.ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            n0_12TotalQty += (oDetail0_12.Count > n0_12) ? oDetail0_12[n0_12].QtyInMeter : 0;
                            n0_12++;                            
                        }
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, (n0_12TotalQty > 0) ? n0_12TotalQty.ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        double n12_16TotalQty = 0;
                        for (int j = 0; j < 5; j++)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail12_16.Count > n12_16) ? oDetail12_16[n12_16].QtyInMeter.ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            n12_16TotalQty += (oDetail12_16.Count > n12_16) ? oDetail12_16[n12_16].QtyInMeter : 0;
                            n12_16++;
                        }
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, (n12_16TotalQty > 0) ? n12_16TotalQty.ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        double n16_20TotalQty = 0;
                        for (int j = 0; j < 5; j++)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail16_20.Count > n16_20) ? oDetail16_20[n16_20].QtyInMeter.ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            n16_20TotalQty += (oDetail16_20.Count > n16_20) ? oDetail16_20[n16_20].QtyInMeter : 0;
                            n16_20++;                            
                        }
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, (n16_20TotalQty > 0) ? n16_20TotalQty.ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        if (i == 0)
                        {
                            List<FabricBatchQCDetail> oDetail = new List<FabricBatchQCDetail>();
                            oDetail = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.D).ToList();
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail.Count() > 0) ? oDetail.Sum(x => x.QtyInMeter).ToString("###0") : "", nRow, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            oDetail = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.E).ToList();
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail.Count() > 0) ? oDetail.Sum(x => x.QtyInMeter).ToString("###0") : "", nRow, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                            oDetail = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.F).ToList();
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail.Count() > 0) ? oDetail.Sum(x => x.QtyInMeter).ToString("###0") : "", nRow, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        }

                    }
                }
                else
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nSLNo).ToString("00"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FEONo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    for (int j = 0; j < 18; j++)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    }
                    List<FabricBatchQCDetail> oDetail = new List<FabricBatchQCDetail>();
                    oDetail = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.D).ToList();
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail.Count() > 0) ? oDetail.Sum(x => x.QtyInMeter).ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    oDetail = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.E).ToList();
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail.Count() > 0) ? oDetail.Sum(x => x.QtyInMeter).ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    oDetail = _oFabricBatchQCDetails.Where(x => x.FBQCID == oItem.FBQCID && x.GradeSL == EnumExcellColumn.F).ToList();
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oDetail.Count() > 0) ? oDetail.Sum(x => x.QtyInMeter).ToString("###0") : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                }

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            #endregion

            #region Total Info
            oPdfPTable = new PdfPTable(25);
            oPdfPTable.SetWidths(new float[] { 25f, 60f, 85f, 80f, 25f, 25f, 25, 25f, 25f, 35f, 25f, 25f, 25f, 25f, 25f, 35f, 25f, 25f, 25f, 25f, 25f, 35f, 40f, 40f, 40f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total ", 0, 9, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x=>x.GradeSL == EnumExcellColumn.A).Sum(y => y.QtyInMeter).ToString("###0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.GradeSL == EnumExcellColumn.B).Sum(y => y.QtyInMeter).ToString("###0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 5, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.GradeSL == EnumExcellColumn.C).Sum(y => y.QtyInMeter).ToString("###0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.GradeSL == EnumExcellColumn.D).Sum(y => y.QtyInMeter).ToString("###0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.GradeSL == EnumExcellColumn.E).Sum(y => y.QtyInMeter).ToString("###0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.GradeSL == EnumExcellColumn.F).Sum(y => y.QtyInMeter).ToString("###0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

        }

        private void SetSummery()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 5f, 20f, 10f, 10f, 5f, 20f, 10f, 10f, 5f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 20, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #region Info
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 5f, 20f, 10f, 10f, 5f, 20f, 10f, 10f, 5f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sample Y/D Production", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC) && x.IsYD == true).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk Y/D Production", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.OrderType == EnumFabricRequestType.Bulk && x.IsYD == true).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();
            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            //oPdfPTable = new PdfPTable(9);
            //oPdfPTable.SetWidths(new float[] { 5f, 20f, 10f, 10f, 5f, 20f, 10f, 10f, 5f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sample S/D Production", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC) && x.IsYD == false).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk S/D Production", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.OrderType == EnumFabricRequestType.Bulk && x.IsYD == false).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();
            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            //oPdfPTable = new PdfPTable(9);
            //oPdfPTable.SetWidths(new float[] { 5f, 20f, 10f, 10f, 5f, 20f, 10f, 10f, 5f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Sample Production", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Bulk Production", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.OrderType == EnumFabricRequestType.Bulk).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();
            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            //oPdfPTable = new PdfPTable(9);
            //oPdfPTable.SetWidths(new float[] { 5f, 20f, 10f, 10f, 5f, 20f, 10f, 10f, 5f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sample Y/D Good", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC) && x.IsYD == true && (x.GradeSL == EnumExcellColumn.A || x.GradeSL == EnumExcellColumn.B || x.GradeSL == EnumExcellColumn.C)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk Y/D Good", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.OrderType == EnumFabricRequestType.Bulk && x.IsYD == true && (x.GradeSL == EnumExcellColumn.A || x.GradeSL == EnumExcellColumn.B || x.GradeSL == EnumExcellColumn.C)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();
            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            //oPdfPTable = new PdfPTable(9);
            //oPdfPTable.SetWidths(new float[] { 5f, 20f, 10f, 10f, 5f, 20f, 10f, 10f, 5f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sample S/D Good", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC) && x.IsYD == false && (x.GradeSL == EnumExcellColumn.A || x.GradeSL == EnumExcellColumn.B || x.GradeSL == EnumExcellColumn.C)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk S/D Good", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.OrderType == EnumFabricRequestType.Bulk && x.IsYD == false && (x.GradeSL == EnumExcellColumn.A || x.GradeSL == EnumExcellColumn.B || x.GradeSL == EnumExcellColumn.C)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();
            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            //oPdfPTable = new PdfPTable(9);
            //oPdfPTable.SetWidths(new float[] { 5f, 20f, 10f, 10f, 5f, 20f, 10f, 10f, 5f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Sample Rejection (Red1+Red2)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC) && (x.GradeSL == EnumExcellColumn.D || x.GradeSL == EnumExcellColumn.E || x.GradeSL == EnumExcellColumn.F)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk Rejection (Red1+Red2)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.OrderType == EnumFabricRequestType.Bulk && (x.GradeSL == EnumExcellColumn.D || x.GradeSL == EnumExcellColumn.E || x.GradeSL == EnumExcellColumn.F)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();
            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 5f, 20f, 10f, 10f, 5f, 20f, 10f, 10f, 5f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 10, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Sample Production Good", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC) && (x.GradeSL == EnumExcellColumn.A || x.GradeSL == EnumExcellColumn.B || x.GradeSL == EnumExcellColumn.C)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Sample Rejection", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC) && (x.GradeSL == EnumExcellColumn.D || x.GradeSL == EnumExcellColumn.E || x.GradeSL == EnumExcellColumn.F)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Sample Production", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.Sample || x.OrderType == EnumFabricRequestType.SampleFOC)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk Yarn Dyeing Production Good", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.OrderType == EnumFabricRequestType.Bulk && x.IsYD == true && (x.GradeSL == EnumExcellColumn.A || x.GradeSL == EnumExcellColumn.B || x.GradeSL == EnumExcellColumn.C)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Bulk Solid Dyeing Production Good", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.OrderType == EnumFabricRequestType.Bulk && x.IsYD == false && (x.GradeSL == EnumExcellColumn.A || x.GradeSL == EnumExcellColumn.B || x.GradeSL == EnumExcellColumn.C)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PTL Production", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.OrderType == EnumFabricRequestType.OwnPO)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Rejection", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => (x.GradeSL == EnumExcellColumn.D || x.GradeSL == EnumExcellColumn.E || x.GradeSL == EnumExcellColumn.F)).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Fault", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricBatchQCDetails.Where(x => x.BUType == EnumBusinessUnitType.Weaving && x.FabricFaultType == EnumFabricFaultType.YarnFault).Sum(y => y.QtyInMeter).ToString("#,##0"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Average Rejection", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFabricBatchQCDetails.Where(x => (x.GradeSL == EnumExcellColumn.D || x.GradeSL == EnumExcellColumn.E || x.GradeSL == EnumExcellColumn.F)).Sum(y => y.QtyInMeter) * 100) / _oFabricBatchQCDetails.Sum(y => y.QtyInMeter)).ToString("#,##0.00;(#,##0.00)") + " %", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 30f, 15f, 15f, 20f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Average Rejection", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((_oFabricBatchQCDetails.Where(x => x.BUType == EnumBusinessUnitType.Weaving && x.FabricFaultType == EnumFabricFaultType.YarnFault).Sum(y => y.QtyInMeter) * 100) / _oFabricBatchQCDetails.Sum(y => y.QtyInMeter)).ToString("#,##0.00;(#,##0.00)") + " %", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Meter", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            #endregion

        }

    }
}
