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
    public class rptQCFaultPreview
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
        FNBatchQC _oFNBatchQC = new FNBatchQC();
        List<FNBatchQCDetail> _oFNBatchQCDetails = new List<FNBatchQCDetail>();
        List<FNBatchQCFault> _oFNBatchQCFaults = new List<FNBatchQCFault>();
        List<FabricProductionFault> _oFabricProductionFaults = new List<FabricProductionFault>();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(FNBatchQC oFNBatchQC, List<FNBatchQCDetail> oFNBatchQCDetails, List<FNBatchQCFault> oFNBatchQCFaults, List<FabricProductionFault> oFabricProductionFaults, Company oCompany)
        {
            _oFNBatchQC = oFNBatchQC;
            _oFNBatchQCDetails = oFNBatchQCDetails;
            _oFNBatchQCFaults = oFNBatchQCFaults;
            _oFabricProductionFaults = oFabricProductionFaults;
            _oCompany = oCompany;

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
            this.PrintHeaderDetail("FINISHED FABRIC INSPECTION REPORT", " ", " ");
            SetMainObjData();
            this.SetData();
        }
        #endregion

        private void SetMainObjData()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 14f, 18f, 7f, 16f, 14f, 31f });

            #region 1st Row
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFNBatchQC.FNExONo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFNBatchQC.Color, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFNBatchQC.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region 2nd Row
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 14f, 18f, 7f, 16f, 14f, 31f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fab. Construction", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFNBatchQC.Construction, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Yards", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFNBatchQC.BatchQty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region 3rd Row
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 14f, 18f, 7f, 16f, 14f, 31f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Inspector", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFNBatchQC.QCInchargeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "M/C No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fab. Composition", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFNBatchQC.Composition, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            #region Blank
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 14f, 18f, 7f, 16f, 14f, 31f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 7, Element.ALIGN_LEFT, BaseColor.WHITE, false, 15f, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

        }

        private void SetData()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int ColumnNumber = 12;
            int nCurrentObjNo = 0, i = 0;
            float[] ColWidthList = new float[ColumnNumber+1];
            ColWidthList[0] = 95f;
            for (i = 0; i < ColumnNumber; i++)
            {
                ColWidthList[i + 1] = 500 / ColumnNumber;
            }

            PdfPTable oPdfPTable = new PdfPTable(ColumnNumber + 1);
            oPdfPTable.SetWidths(ColWidthList);

            while (_oFNBatchQCDetails.Count > nCurrentObjNo)
            {
                #region Heder Info
                #region 1st header
                oPdfPTable = new PdfPTable(ColumnNumber + 1);
                oPdfPTable.SetWidths(ColWidthList);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Roll/Piece No", 0, 0, Element.ALIGN_LEFT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                //foreach(FNBatchQCDetail oDetail in _oFNBatchQCDetails)
                for (i = nCurrentObjNo; i < nCurrentObjNo + ColumnNumber; i++)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFNBatchQCDetails.Count < i + 1) ? "" : _oFNBatchQCDetails[i].LotNo, 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyle);
                }
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #region 2nd Header
                oPdfPTable = new PdfPTable(ColumnNumber + 1);
                oPdfPTable.SetWidths(ColWidthList);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Width (Measured)", 0, 0, Element.ALIGN_LEFT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                for (i = nCurrentObjNo; i < nCurrentObjNo + ColumnNumber; i++)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFNBatchQCDetails.Count < i + 1) ? "" : _oFNBatchQC.ActualWidth.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyle);
                }
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #region 3rd header
                oPdfPTable = new PdfPTable(ColumnNumber + 1);
                oPdfPTable.SetWidths(ColWidthList);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yards (Measured)", 0, 0, Element.ALIGN_LEFT, BaseColor.LIGHT_GRAY, true, 0, _oFontStyleBold);
                for (i = nCurrentObjNo; i < nCurrentObjNo + ColumnNumber; i++)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_oFNBatchQCDetails.Count < i + 1) ? "" : _oFNBatchQCDetails[i].Qty.ToString("#,##0.00;(#,##0.00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyle);
                }
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                #endregion

                #region data
                int FaultTotal = 0;
                foreach (var oItem in _oFabricProductionFaults)
                {
                    oPdfPTable = new PdfPTable(ColumnNumber + 1);
                    oPdfPTable.SetWidths(ColWidthList);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Name, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    for (i = nCurrentObjNo; i < nCurrentObjNo + ColumnNumber; i++)
                    {
                        if (_oFNBatchQCDetails.Count < i + 1)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        }
                        else
                        {
                            FaultTotal = _oFNBatchQCFaults.Where(x => x.FPFID == oItem.FPFID && x.FNBatchQCDetailID == _oFNBatchQCDetails[i].FNBatchQCDetailID).Select(y => y.FaultTotal).FirstOrDefault();
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (FaultTotal == 0) ? "" : FaultTotal.ToString("00;(00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        }
                    }

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                #endregion

                #region Total
                oPdfPTable = new PdfPTable(ColumnNumber + 1);
                oPdfPTable.SetWidths(ColWidthList);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Linear Points", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                for (i = nCurrentObjNo; i < nCurrentObjNo + ColumnNumber; i++)
                {
                    if (_oFNBatchQCDetails.Count < i + 1)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    }
                    else
                    {
                        FaultTotal = _oFNBatchQCFaults.Where(x => x.FNBatchQCDetailID == _oFNBatchQCDetails[i].FNBatchQCDetailID).Select(y => y.FaultTotal).Sum();
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, FaultTotal.ToString("00;(00)"), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    }
                }

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                nCurrentObjNo += ColumnNumber;
                NewPageDeclaration();
            }

        }

        public void NewPageDeclaration()
        {
            #region New Page Declare
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();            
            #endregion
        }

    }
}
