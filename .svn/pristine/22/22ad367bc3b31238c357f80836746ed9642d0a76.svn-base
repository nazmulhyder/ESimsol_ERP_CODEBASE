using System;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;

namespace ESimSol.Reports
{
    public class rptFabricPlanning
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        int _nColspan = 10;
        PdfPTable _oPdfPTable = new PdfPTable(10);
        PdfPTable _oPdfPTable_Swap = new PdfPTable(10);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricPlanning> _oFabricPlanningList = new List<FabricPlanning>();
        FabricSCReport _oFabricSCReport = new FabricSCReport();
        #endregion

        #region Prepare Report
        public byte[] PrepareReport(List<FabricPlanning> oFabricPlanningList, FabricSCReport oFabricSCReport, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oFabricPlanningList = oFabricPlanningList;
            _oFabricSCReport = oFabricSCReport;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //842*595
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                   70, //1
                                                   20, //2
                                                   40, //3
                                                   40, //4
                                                   30, //5
                                                   30, //6
                                                   60, //7
                                                   70, //8
                                                   60, //9
                                                   50, //10
                                             });
            #endregion

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany,sHeaderName,_nColspan);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, this.InformationPart(_oFabricSCReport), 15, 0, 10);

            this.PrintBody();
            //this.PrintSwaps();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion
        
        #region Report Body
        private void PrintBody()
        {
            List<FabricPlanning> oFabricPlannings = new List<FabricPlanning>();
            oFabricPlannings = _oFabricPlanningList.Where(x => x.IsWarp==true).ToList();
            if (oFabricPlannings.Any())
            Print_Planning(oFabricPlannings,"Planning - Warp");

            oFabricPlannings = _oFabricPlanningList.Where(x => x.IsWarp == false).ToList();
            if (oFabricPlannings.Any())
            Print_Planning(oFabricPlannings, "Planning - Weft" );
        }

        private void Print_Planning(List<FabricPlanning> oFabricPlannings, string sHeader) 
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeader, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, _nColspan, 15);
            int nCount = 0;

            #region Yarn Planning
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.SetWidths(new float[] { 
                                                   50, //1
                                                   20, //2
                                                   20, //3
                                                   20, //4
                                                   20, //5
                                                   20, //6
                                                   20, //7
                                                   20, //8
                                                   20, //9
                                                   20, //10                                                   
                                                   20, //11                                                   
                                                   20, //12 --Total                                                                                                      
                                                   40, //13 --Percentage
                                                   265
                                             });
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);

            int nTotalCount=oFabricPlannings.Sum(x=>x.CountT);

            foreach (var oItem in oFabricPlannings)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Yarn " + (++nCount), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count1.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count2.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count3.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count4.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count5.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count6.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count7.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count8.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count9.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Count10.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);

                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.CountT.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Math.Round((double)(oItem.CountT*100)/nTotalCount,2).ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);

                ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 0, 15);
            }
            ESimSolPdfHelper.AddTable(ref _oPdfPTable,oPdfPTable,0,0,10);

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, _nColspan, 15);
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Yarn Definition", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, _nColspan, 15);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            Print_HeaderRow(new string[] { "Yarn", "Type", "Weight[Ne]", "Density[Inch]" });
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Color", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
            Print_HeaderRow(new string[] { "Image", "Color Name", "Panton No", "Labdip No" });

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            #region Defination
            nCount = 0;
            foreach (var oItem in oFabricPlannings)
            {
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ProductName, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                //  ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);

                BaseColor _Color = BaseColor.WHITE;
                if (oItem.RGB.Split(',').Length == 3)
                {
                    _Color = new BaseColor(Convert.ToInt16(oItem.RGB.Split(',')[0]), Convert.ToInt16(oItem.RGB.Split(',')[1]), Convert.ToInt16(oItem.RGB.Split(',')[2]), 1);
                }

                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.RGB, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, _Color, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                //ESimSolPdfHelper.AddPaddingCell(ref _oPdfPTable, "", _Color, 5, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, _Color, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.Color, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.PantonNo, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ColorNo, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            }
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, _nColspan, 15);
            #endregion
        }

        private void PrintSwaps() 
        {
            List<FabricPlanning> oFabricPlannings = new List<FabricPlanning>();
            List<FabricPlanning> oFabricPlannings_Weft = new List<FabricPlanning>();

            oFabricPlannings = _oFabricPlanningList.Where(x => x.IsWarp == true).ToList();
            oFabricPlannings_Weft = _oFabricPlanningList.Where(x => x.IsWarp == false).ToList();

            int nTotalWarp = oFabricPlannings.Sum(x => x.CountT);
            float[] sWidths = new float[nTotalWarp];

            for (int i = 0; i < nTotalWarp; i++)
                sWidths[i] = 495 / 88;

            _oPdfPTable_Swap = new PdfPTable(nTotalWarp);
            _oPdfPTable_Swap.SetWidths(sWidths);

            foreach (var oitem in oFabricPlannings) 
            {
                PrintSwap(oitem.Count1, oitem.RGB); PrintSwap(oitem.Count2, oitem.RGB); PrintSwap(oitem.Count3, oitem.RGB); PrintSwap(oitem.Count4, oitem.RGB); PrintSwap(oitem.Count5, oitem.RGB);
                PrintSwap(oitem.Count6, oitem.RGB); PrintSwap(oitem.Count7, oitem.RGB); PrintSwap(oitem.Count8, oitem.RGB); PrintSwap(oitem.Count9, oitem.RGB); PrintSwap(oitem.Count10, oitem.RGB);
            }

            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, _nColspan, 15);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 15);
            _oPdfPTable_Swap.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, _oPdfPTable_Swap, 15, 0, 8);
        }
        public void PrintSwap(int Count, string sColor) 
        {
            while (Count > 0) 
            {
                BaseColor _Color = BaseColor.WHITE;
                if (sColor.Split(',').Length == 3)
                {
                    _Color = new BaseColor(Convert.ToInt16(sColor.Split(',')[0]), Convert.ToInt16(sColor.Split(',')[1]), Convert.ToInt16(sColor.Split(',')[2]), 1);
                }
                AddCell(ref _oPdfPTable_Swap, _Color, 0.5f); Count--;
            }
        }

        #endregion

        public void Print_HeaderRow(string[] Data) 
        {
            foreach (string oItem in Data)
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
        }
        private void AddCell(ref PdfPTable oPdfPTable,  BaseColor oBaseColor, float nBorder)
        {
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.BackgroundColor = oBaseColor;
            _oPdfPCell.BorderWidth = nBorder;
            _oPdfPCell.BorderColor= BaseColor.WHITE;
            _oPdfPCell.MinimumHeight = 8f; 
            oPdfPTable.AddCell(_oPdfPCell);
        }
        private PdfPTable InformationPart(FabricSCReport oFabricSCReport)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, 1);
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.SetWidths(new float[] { 40f, 100f, 50f, 80f, 50f,  60f, 50f, 60f, 60f, 70f });

            _oPdfPCell = new PdfPCell(new Phrase("PO No ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(oFabricSCReport.SCNoFull, _oFontStyleBold));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

          
            _oPdfPCell = new PdfPCell(new Phrase("Date ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

           
            _oPdfPCell = new PdfPCell(new Phrase(oFabricSCReport.SCDateSt, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

         
            _oPdfPCell = new PdfPCell(new Phrase("P No ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(oFabricSCReport.FabricNo, _oFontStyleBold));//oFabric.Reed.ToString() + "/" + oFabric.Dent.ToString()
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

         
            _oPdfPCell = new PdfPCell(new Phrase("H/L No ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

           
            _oPdfPCell = new PdfPCell(new Phrase(oFabricSCReport.ExeNo, _oFontStyle));//oFabric.RepeatSize
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

          
            _oPdfPCell = new PdfPCell(new Phrase("Buyer ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

           
            _oPdfPCell = new PdfPCell(new Phrase(oFabricSCReport.ContractorName, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

          
            _oPdfPCell = new PdfPCell(new Phrase("Construction ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

           
            _oPdfPCell = new PdfPCell(new Phrase(oFabricSCReport.Construction, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

         
            _oPdfPCell = new PdfPCell(new Phrase("Reed ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//oFabric.Reed.ToString() + "/" + oFabric.Dent.ToString()
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

          
            _oPdfPCell = new PdfPCell(new Phrase("R. Size ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

 
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//oFabric.RepeatSize
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
           _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

        
            return oPdfPTable;
        }
    }
}