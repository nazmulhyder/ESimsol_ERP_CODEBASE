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
using ICS.Core;
using ICS.Core.Utility;

namespace ESimSol.Reports
{
    public class rptFNInspectionSticker
    {
        PdfWriter _oWriter;
        Document _oDocument;

        PdfPTable _oPdfPTable = new PdfPTable(1);
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        float nRowHeight = 18f;

        public byte[] PrepareReport(FNBatchQC oFNBatchQC, List<FNBatchQCDetail> oFNBatchQCDetails ,Company oCompany, bool IsA4)
        {
            _oCompany = oCompany;
            #region Page Setup

            if (IsA4)
                _oDocument = new Document(PageSize.A4, 15f, 15f, 60f, 40f);
            else
                _oDocument = new Document(new iTextSharp.text.Rectangle(250, 260), 5f, 5f, 10f, 5f);

            _oPdfPTable.WidthPercentage = 95; // Use 100% of the page
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER; // Page Center Position

            PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            //_oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //_oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PageEventHandler.PrintDocumentGenerator = true;
            //PageEventHandler.PrintPrintingDateTime = true;
            //_oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler  

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { (IsA4)? 500f :230f });
            #endregion


            #region Document Data/ Print

            PrintSticker(oFNBatchQC, oFNBatchQCDetails, IsA4);

           
            #endregion
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private static PdfPTable TableSticker()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 230f });
            return oPdfPTable;

        }

        private static PdfPTable TableA4()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 250f, 30f, 250f });
            return oPdfPTable;

        }

        private PdfPTable PageHeader()
        {
            PdfPTable oPdfPTable = TableSticker();
            Font oFontStyle = FontFactory.GetFont("Tahoma", 16f, iTextSharp.text.Font.NORMAL);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oCompany.Name, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, true);

            oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finishing Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, true);

            oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oCompany.FactoryAddress, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, true);

            oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL | Font.UNDERLINE);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "FINAL INSPECTION", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 20, oFontStyle, true);
            return oPdfPTable;
        }

        private PdfPTable PrintBody(FNBatchQC oFNBatchQC, FNBatchQCDetail oFNBatchQCDetail, Font oFontStyle)
        {
            PdfPTable oPdfPTable = TableSticker();

            ESimSolItexSharp.PushTableInCell(ref oPdfPTable, PageHeader(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date: " + oFNBatchQCDetail.DBServerDateStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Roll No: " + oFNBatchQCDetail.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No: " + oFNBatchQC.FNExONo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer Name: " + oFNBatchQC.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grade: " + oFNBatchQCDetail.GradeStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade: " + oFNBatchQCDetail.ShadeStr, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Shade: " + oFNBatchQC.Color, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            if (string.IsNullOrEmpty(oFNBatchQC.ConstructionPI)) { oFNBatchQC.ConstructionPI = oFNBatchQC.Construction; }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction: " + oFNBatchQC.ConstructionPI, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);

            //PdfPTable oPdfPTable = Table_FNQCDetail();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Composition: " + oFNBatchQC.Composition, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weave: " + oFNBatchQC.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            //ESimSolItexSharp.PushTableInCell(ref oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Length: " + ((oFNBatchQCDetail.Qty > 0) ? Global.MillionFormat(oFNBatchQCDetail.Qty) + " (Y)" : ""), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finish Width: " + oFNBatchQC.FinishWidth, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Finished Type: " + oFNBatchQC.FinishTypeName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, nRowHeight, oFontStyle, true);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, nRowHeight, oFontStyle, true);
           
            oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, " Goods once out not be taken back. Pls out the fabrics accroding the batch No.", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, true);

            return oPdfPTable;
        }

        private void PrintSticker(FNBatchQC oFNBatchQC, List<FNBatchQCDetail> oFNBatchQCDetails, bool IsA4)
        {

           
            if (oFNBatchQCDetails.Any())
            {
                if (IsA4)
                {
                    Font oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);
                    for (int i = 0; i < oFNBatchQCDetails.Count(); i += 2)
                    {
                        PdfPTable oPdfPTable = TableA4();
                        bool hasValue = ((i + 1) < oFNBatchQCDetails.Count());

                        ESimSolItexSharp.PushTableInCell(ref oPdfPTable, PrintBody(oFNBatchQC, oFNBatchQCDetails[i], oFontStyle), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, false);

                        if (hasValue)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle);
                            ESimSolItexSharp.PushTableInCell(ref oPdfPTable, PrintBody(oFNBatchQC, oFNBatchQCDetails[i + 1], oFontStyle), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
                        }
                        else
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, false);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, false);
                        }

                        // Sub Table Push Into Parent Table
                        ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                        // Row Complete For Making Space
                        //if ((i + 1) % 4 != 0)
                        //{
                        //    ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 80f, oFontStyle, true);
                        //}
                        if (i % 2 == 0 && i % 4 != 0)
                        {
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0f, oFontStyle, true);

                        //    _oDocument.Add(_oPdfPTable);
                        //    _oDocument.NewPage();
                        //    _oPdfPTable.DeleteBodyRows();
                        }
                        else
                        {
                            ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 80f, oFontStyle, true);
                        }
                    }
                }
                else
                {
                    Font oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    nRowHeight = 0;
                    foreach (FNBatchQCDetail oFNBQCD in oFNBatchQCDetails)
                    {
                        var oPdfPTable = PrintBody(oFNBatchQC, oFNBQCD, oFontStyle);
                        ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    }
                }
            }
            else
            {
                Font oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oFNBatchQC.ErrorMessage, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, oFontStyle, true);

            }

        }

        
    }
}
