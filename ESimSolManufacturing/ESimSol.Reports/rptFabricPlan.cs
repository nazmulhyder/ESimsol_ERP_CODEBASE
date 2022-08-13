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
    public class rptFabricPlan
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyle_UnLine;
        BaseColor _oBC = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EFF0EA"));
        int _nColspan = 10;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPTable _oPdfPTable_Swap = new PdfPTable(10);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FabricPlan> _oFabricPlanList = new List<FabricPlan>();
        FabricSCReport _oFabricExecutionOrder = new FabricSCReport();
        List<FabricPlanDetail> _oFabricPlanDetails = new List<FabricPlanDetail>();
        List<FabricPlanRepeat> _oFabricPlanRepeats = new List<FabricPlanRepeat>();
        FabricPlanOrder _oFabricPlanOrder = new FabricPlanOrder();
        Company _oCompany = new Company();
        int _nColumns = 0;
        #endregion

        #region Prepare Report
        public byte[] PrepareReport(FabricPlanOrder oFabricPlanOrder, List<FabricPlan> oFabricPlans, List<FabricPlanDetail> oFabricPlanDetails, List<FabricPlanRepeat> oFabricPlanRepeats, FabricSCReport oFabricExecutionOrder, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oFabricPlanOrder = oFabricPlanOrder;
            _oFabricPlanOrder.FabricPlans = oFabricPlans;
            _oFabricPlanList = oFabricPlans;
            _oFabricPlanDetails = oFabricPlanDetails;
            _oFabricPlanRepeats = oFabricPlanRepeats;
            _oFabricExecutionOrder = oFabricExecutionOrder;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 20f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.SetBatchInfo();
            this.PrintBody();
            //this.PrintSwaps();
            //this.PrintBodyColor();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion
        #region Color
        private void PrintBodyColor()
        {
            List<FabricPlan> oFabricPlans = new List<FabricPlan>();
            oFabricPlans = _oFabricPlanList.Where(x => x.WarpWeftType == EnumWarpWeft.Warp).ToList();
            if (oFabricPlans.Any())
                Print_PlanningColor(oFabricPlans, "Planning - Warp");

            oFabricPlans = _oFabricPlanList.Where(x => x.WarpWeftType == EnumWarpWeft.Weft).ToList();
            if (oFabricPlans.Any())
                Print_PlanningColor(oFabricPlans, "Planning - Weft");
        }

        private void Print_PlanningColor(List<FabricPlan> oFabricPlans, string sHeader)
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            int nSL = 0;
              PdfPTable oPdfPTable = new PdfPTable(15);

          
                #region Fabric Info
                oPdfPTable = new PdfPTable(5);
                oPdfPTable.SetWidths(new float[] { 23f, 110f, 115f, 100f, 150f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, sHeader, 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Type", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color Name", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "LD/Color No ", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                
               

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

                foreach (FabricPlan oItem1 in oFabricPlans)
                {
                    oPdfPTable = new PdfPTable(5);
                    oPdfPTable.SetWidths(new float[] { 23f, 110f, 115f, 100f, 150f });

                    nSL++;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSL.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.Color, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ColorNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                    BaseColor _Color = BaseColor.WHITE;
                    if (oItem1.RGB.Split(',').Length == 3)
                    {
                        _Color = new BaseColor(Convert.ToInt16(oItem1.RGB.Split(',')[0]), Convert.ToInt16(oItem1.RGB.Split(',')[1]), Convert.ToInt16(oItem1.RGB.Split(',')[2]), 1);
                    }

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.RGB, 0, 0, Element.ALIGN_LEFT, _Color, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }






            _nColumns = 0;// oWUs.Count + 3;
            //PdfPTable oPdfPTable = new PdfPTable(_nColumns);

            //float[] tablecolumns = new float[_nColumns];
            //tablecolumns[0] = 25f;
            //tablecolumns[1] = 100f;
            //for (int i = 2; i < _nColumns; i++)
            //{
            //    tablecolumns[i] = 60f;
            //}

            //oPdfPTable = new PdfPTable(_nColumns);
            //oPdfPTable.SetWidths(tablecolumns);

            //#region Yarn Planning
            //ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            //PdfPTable oPdfPTable = new PdfPTable(14);
            //oPdfPTable.SetWidths(new float[] { 
            //                                       50, //1
            //                                       20, //2
            //                                       20, //3
            //                                       20, //4
            //                                       20, //5
            //                                       20, //6
            //                                       20, //7
            //                                       20, //8
            //                                       20, //9
            //                                       20, //10                                                   
            //                                       20, //11                                                   
            //                                       20, //12 --Total                                                                                                      
            //                                       40, //13 --Percentage
            //                                       265
            //                                 });
            //ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD);

            //int nTotalCount = 0;//oFabricPlans.Sum(x=>x.CountT);

            //#endregion

           // _nColspan = 8;

           // ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
           // ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Yarn Definition", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, _nColspan, 15);

           // ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
           // Print_HeaderRow(new string[] { "Yarn", "Type", "Weight[Ne]", "Density[Inch]" });
           // ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Color", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 2, 0);
           //Print_HeaderRow(new string[] { "Image", "Color Name", "Panton No", "Labdip No" });

           // ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
           // #region Defination
        
           // foreach (var oItem in oFabricPlans)
           // {
           //     ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ProductName, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     //  ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);

           //     BaseColor _Color = BaseColor.WHITE;
           //     if (oItem.RGB.Split(',').Length == 3)
           //     {
           //         _Color = new BaseColor(Convert.ToInt16(oItem.RGB.Split(',')[0]), Convert.ToInt16(oItem.RGB.Split(',')[1]), Convert.ToInt16(oItem.RGB.Split(',')[2]), 1);
           //     }

           //     ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.RGB, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, _Color, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     ////ESimSolPdfHelper.AddPaddingCell(ref _oPdfPTable, "", _Color, 5, 0, 0, 15);
           //     //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, _Color, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.Color, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.PantonNo, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ColorNo, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
           //     _oPdfPTable.CompleteRow();
           // }
           // ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, _nColspan, 15);
           // #endregion

        }
        public void Print_HeaderRow(string[] Data)
        {
            foreach (string oItem in Data)
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE);
        }
        #endregion

        #region Print Header
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

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion
        #region Report Header
        private void ReporttHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 200f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabric ID: " + _oFabricExecutionOrder.FabricNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Fabric Pattern", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, DateTime.Now.ToString("dd-MM-yyyy hh:mm tt"), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD));
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, true);
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            List<FabricPlan> oFabricPlansTwisting = new List<FabricPlan>();
            oFabricPlansTwisting = _oFabricPlanList.Where(x => x.TwistedGroup > 0).ToList();

            oFabricPlansTwisting = oFabricPlansTwisting.GroupBy(x => new { x.TwistedGroup, x.WarpWeftType, x.EndsCount, x.FabricPlanOrderID, x.RefID, x.RefType }, (key, grp) =>
                                       new FabricPlan()
                                       {
                                           TwistedGroup = 0,
                                           RefID = key.RefID,
                                           RefType = key.RefType,
                                           FabricPlanOrderID = key.FabricPlanOrderID,
                                           ProductName = grp.Select(x => x.ProductName).FirstOrDefault(),
                                           Color = "Grindle",
                                           WarpWeftType = key.WarpWeftType,
                                           SLNo = grp.Min(p => p.SLNo),
                                           EndsCount = grp.Max(p => p.EndsCount),
                                           FabricPlanID = grp.Min(p => p.FabricPlanID),
                                       }).ToList();

            _oFabricPlanList.AddRange(oFabricPlansTwisting);

            List<FabricPlan> oFabricPlans = new List<FabricPlan>();
            oFabricPlans = _oFabricPlanList.Where(x => x.WarpWeftType == EnumWarpWeft.Warp && x.TwistedGroup == 0).ToList();
            if (oFabricPlans.Any())
                Print_Planning(oFabricPlans, "Warp plan info:");
            oFabricPlans=_oFabricPlanOrder.FabricPlans.Where(x => x.WarpWeftType == EnumWarpWeft.Warp).ToList(); 
            Print_PlanningColor(oFabricPlans, "Color - Warp");

            oFabricPlans = _oFabricPlanList.Where(x => x.WarpWeftType == EnumWarpWeft.Weft && x.TwistedGroup == 0).ToList();
            if (oFabricPlans.Any())
                Print_Planning(oFabricPlans, "Weft plan info:");
            oFabricPlans = _oFabricPlanOrder.FabricPlans.Where(x => x.WarpWeftType == EnumWarpWeft.Weft ).ToList(); 
                Print_PlanningColor(oFabricPlans, "Color - Weft");
        }

        private void Print_Planning(List<FabricPlan> oFabricPlans, string sHeader)
        {
            EnumWarpWeft eEnumWarpWeft = EnumWarpWeft.None;
            eEnumWarpWeft = oFabricPlans[0].WarpWeftType;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 500f });
            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sHeader, 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            #endregion

            if (_oFabricPlanOrder.ColumnCount <= 10) { _oFabricPlanOrder.ColumnCount = 20; }

            _nColumns = _oFabricPlanOrder.ColumnCount + 3;// oWUs.Count + 3;
            oPdfPTable = new PdfPTable(_nColumns);

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = 80f;
            tablecolumns[1] = 65f;
            //tablecolumns[2] =25f;
            for (int i = 2; i < _nColumns; i++)
            {
                tablecolumns[i] = 20f;
            }
            tablecolumns[_nColumns - 1] = 30f;

            oPdfPTable = new PdfPTable(_nColumns);
            oPdfPTable.SetWidths(tablecolumns);

            #region Yarn Planning



            int nEndsCount = 0;
            int nSL = 0;
            int nStartCol = 0;
            int nEndCol = 0;
            oFabricPlans = oFabricPlans.OrderBy(x => x.SLNo).ToList();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Yarn", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Color", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Value", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, _oFabricPlanOrder.ColumnCount, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, (eEnumWarpWeft == EnumWarpWeft.Warp) ? "Ends" : "Picks", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            foreach (var oItem in oFabricPlans)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.ProductName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Color, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, oItem.Value.ToString(), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                for (int i = 1; i <= _oFabricPlanOrder.ColumnCount; i++)
                {
                    if (_oFabricPlanDetails.FirstOrDefault() != null && _oFabricPlanDetails.FirstOrDefault().FabricPlanID > 0 && _oFabricPlanDetails.Where(b => b.FabricPlanID == oItem.FabricPlanID && b.ColNo == i).Count() > 0)
                    {
                        nEndsCount = _oFabricPlanDetails.Where(p => p.FabricPlanID == oItem.FabricPlanID && p.ColNo == i && p.FabricPlanID > 0).FirstOrDefault().EndsCount;

                    }
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, ((nEndsCount > 0) ? nEndsCount.ToString() : ""), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
                    nEndsCount = 0;
                }

                ESimSolPdfHelper.AddCell(ref oPdfPTable, ((oItem.EndsCount > 0) ? oItem.EndsCount.ToString() : ""), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 15);
            }
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            var oFabricPlanRepeats = _oFabricPlanRepeats.Where(x => x.WarpWeftType == eEnumWarpWeft).ToList();

            if (oFabricPlanRepeats.Count <= 0)
            {
                nEndsCount = oFabricPlans.Sum(p => p.EndsCount);

                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, _oFabricPlanOrder.ColumnCount + 2, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, ((nEndsCount > 0) ? nEndsCount.ToString() : ""), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);
            }
            else
            {
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);
                oPdfPTable = new PdfPTable(_nColumns);
                oPdfPTable.SetWidths(tablecolumns);

                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 2, 15);

                for (int i = 1; i <= _oFabricPlanOrder.ColumnCount; i++)
                {
                    nEndsCount = 0;
                    nSL = 0;
                    if (oFabricPlanRepeats.FirstOrDefault() != null && oFabricPlanRepeats.FirstOrDefault().RepeatNo > 0 && oFabricPlanRepeats.Where(b => b.StartCol <= i && b.EndCol >= i).Count() > 0)
                    {
                        nEndsCount = oFabricPlanRepeats.Where(p => p.StartCol <= i && p.EndCol >= i).FirstOrDefault().RepeatNo;
                        nStartCol = oFabricPlanRepeats.Where(p => p.StartCol <= i && p.EndCol >= i).FirstOrDefault().StartCol;
                        nEndCol = oFabricPlanRepeats.Where(p => p.StartCol <= i && p.EndCol >= i).FirstOrDefault().EndCol;
                        i = nEndCol;
                        nSL = nEndCol + 1 - nStartCol;
                    }


                    if (nEndsCount > 0 && nSL > 0)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, ((nEndsCount > 0) ? "x" + nEndsCount.ToString() : ""), Element.ALIGN_CENTER, Element.ALIGN_RIGHT, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, nSL, 0);
                    }
                    else if (nEndsCount <= 0)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_RIGHT, BaseColor.WHITE, iTextSharp.text.Rectangle.NO_BORDER, 0, 0, 0);
                    }

                }

                nEndsCount = oFabricPlans.Sum(p => p.EndsCount);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, ((nEndsCount > 0) ? nEndsCount.ToString() : ""), Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, iTextSharp.text.Rectangle.BOX, 0, 0, 0);
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, _oPdfPTable.NumberOfColumns);


            }


            #endregion


        }
        #endregion
        private void SetBatchInfo()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 80f, 100f, 80f, 100f, 80f, 100f });

            #region PO Info
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Information", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //oPdfPTable.CompleteRow();


            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.BuyerName, 0, 5, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUBatchCost.ColorName, 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Status", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUBatchCost.RSState.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.SCDateSt, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Status", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oDUBatchCost.RSState.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Composition", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construction", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weight", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.FabricWidth.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weave", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.Fa, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Weight", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFabricExecutionOrder.CW.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            #endregion
        }
        private void InformationPart()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.SetWidths(new float[] { 40f, 50f, 100f, 80f, 80f, 10f, 25f, 60f, 60f, 60f, 80f });

            _oPdfPCell = new PdfPCell(new Phrase("Buyer ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricExecutionOrder.BuyerName, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("Construction ", _oFontStyle));
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase(_oFabricExecutionOrder.Construction, _oFontStyle));
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Reed ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.Reed.ToString() + "/" + _oFabricPlanOrder.Dent.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("R. Size ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.RepeatSize.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricExecutionOrder.Construction, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Pick ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.Pick.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Warp ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.Warp, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Ratio ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.Ratio, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Style ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricExecutionOrder.StyleNo, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Weave ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.WeaveName, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Weft ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.Weft, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("GSM ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.GSM.ToString(), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Design ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.FabricDesignName, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Note ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricPlanOrder.Note, _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
        }

        #region Prepare Report
        public byte[] PrepareReport_V1(FabricPlanOrder oFabricPlanOrder, List<FabricPlan> oFabricPlans, List<FabricPlanDetail> oFabricPlanDetails, List<FabricPlanRepeat> oFabricPlanRepeats, FabricSCReport oFabricExecutionOrder, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName)
        {
            _oFabricPlanOrder = oFabricPlanOrder;
            _oFabricPlanList = oFabricPlans;
            _oFabricPlanDetails = oFabricPlanDetails;
            _oFabricPlanRepeats = oFabricPlanRepeats;
            _oFabricExecutionOrder = oFabricExecutionOrder;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 20f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.InformationPart();
            this.PrintBodyPattern();
            //this.PrintBody();
            //this.PrintSwaps();
            // _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBodyPattern()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 250f, 20f, 250f });
            List<FabricPlan> oFabricPlans = new List<FabricPlan>();

            List<FabricPlan> oFabricPlansTwisting = new List<FabricPlan>();
            oFabricPlansTwisting = _oFabricPlanList.Where(x => x.TwistedGroup > 0).ToList();

            oFabricPlansTwisting = oFabricPlansTwisting.GroupBy(x => new { x.TwistedGroup, x.WarpWeftType, x.EndsCount, x.FabricPlanOrderID, x.RefID, x.RefType }, (key, grp) =>
                                       new FabricPlan()
                                       {
                                           TwistedGroup = 0,
                                           RefID = key.RefID,
                                           RefType = key.RefType,
                                           FabricPlanOrderID = key.FabricPlanOrderID,
                                           ProductName = grp.Select(x => x.ProductName).FirstOrDefault(),
                                           Color = "Grindle",
                                           WarpWeftType = key.WarpWeftType,
                                           SLNo = grp.Min(p => p.SLNo),
                                           EndsCount = grp.Max(p => p.EndsCount),
                                           FabricPlanID = grp.Min(p => p.FabricPlanID),
                                       }).ToList();

            _oFabricPlanList.AddRange(oFabricPlansTwisting);

            _oFabricPlanList = _oFabricPlanList.Where(x => x.TwistedGroup == 0).ToList();

            if (_oFabricPlanList.Count > 0)
            {

                _oPdfPCell = new PdfPCell(new Phrase("Warp Pattern", _oFontStyle));
                //_oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = _oBC;

                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase("Weft Pattern", _oFontStyle));
                //_oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = _oBC;

                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();


                oFabricPlans = _oFabricPlanList.Where(x => x.WarpWeftType == EnumWarpWeft.Warp).ToList();

                _oPdfPCell = new PdfPCell(this.Pattern(oFabricPlans, EnumWarpWeft.Warp));
                //_oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);

                oFabricPlans = _oFabricPlanList.Where(x => x.WarpWeftType == EnumWarpWeft.Weft).ToList();

                _oPdfPCell = new PdfPCell(this.Pattern(oFabricPlans, EnumWarpWeft.Weft));
                //_oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }

        }
        private PdfPTable Pattern(List<FabricPlan> oFPDs, EnumWarpWeft eEnumWarpWeft)
        {
            int nRepeatNo = 0;
            int nStartCol = 0;
            int nEndCol = 0;
            int nSL = 0;
            int nEndsCount = 0;
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 80f, 80f, 30f, 30f });

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Count", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ends", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Times", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();

            List<FabricPlanDetail> oFabricPlanDetails = new List<FabricPlanDetail>();
            List<FabricPlanDetail> oFabricPlanDetailsTemp = new List<FabricPlanDetail>();
            FabricPlan oFabricPlan = new FabricPlan();


            foreach (FabricPlan oItem in oFPDs)
            {
                if (oItem.WarpWeftType == eEnumWarpWeft)
                {
                    oFabricPlanDetailsTemp.AddRange(_oFabricPlanDetails.Where(p => p.FabricPlanID == oItem.FabricPlanID).ToList());
                }
            }

            for (int i = 1; i <= _oFabricPlanOrder.ColumnCount; i++)
            {
                nRepeatNo = 0;
                nSL = 1;
                if (_oFabricPlanRepeats.FirstOrDefault() != null && _oFabricPlanRepeats.FirstOrDefault().RepeatNo > 0 && _oFabricPlanRepeats.Where(b => b.StartCol <= i && b.EndCol >= i && b.WarpWeftType == eEnumWarpWeft).Count() > 0)
                {
                    nRepeatNo = _oFabricPlanRepeats.Where(p => p.StartCol <= i && p.EndCol >= i && p.WarpWeftType == eEnumWarpWeft).FirstOrDefault().RepeatNo;
                    nStartCol = _oFabricPlanRepeats.Where(p => p.StartCol <= i && p.EndCol >= i && p.WarpWeftType == eEnumWarpWeft).FirstOrDefault().StartCol;
                    nEndCol = _oFabricPlanRepeats.Where(p => p.StartCol <= i && p.EndCol >= i && p.WarpWeftType == eEnumWarpWeft).FirstOrDefault().EndCol;
                    nSL = nEndCol + 1 - nStartCol;
                }

                oFabricPlanDetails = oFabricPlanDetailsTemp.Where(p => p.ColNo == i).ToList();
                if (oFabricPlanDetails.Any())
                {
                    foreach (FabricPlanDetail oItem in oFabricPlanDetails)
                    {
                        if (_oFabricPlanList.FirstOrDefault() != null && _oFabricPlanList.FirstOrDefault().FabricPlanID > 0 && _oFabricPlanList.Where(b => b.FabricPlanID == oItem.FabricPlanID && b.WarpWeftType == eEnumWarpWeft).Count() > 0)
                        {
                            oFabricPlan = _oFabricPlanList.Where(b => b.FabricPlanID == oItem.FabricPlanID).ToList().FirstOrDefault();
                        }

                        //nRowSpan_Twist = _oFabricPlanRepeats.Where(P => P.WarpWeftType == oFabricPlan.WarpWeftType && P.StartCol <= i && P.EndCol >= i).ToList().Count;


                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricPlan.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oFabricPlan.Color, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.EndsCount > 0) ? oItem.EndsCount.ToString() : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        if (i == nStartCol && nSL > 0)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nRepeatNo > 0) ? nRepeatNo.ToString() : ""), nSL, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        }

                        oPdfPTable.CompleteRow();
                    }
                }

            }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((eEnumWarpWeft == EnumWarpWeft.Warp) ? "Total Ends:" : "Total Picks:"), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
            nEndsCount = oFPDs.Sum(p => p.EndsCount);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((nEndsCount > 0) ? nEndsCount.ToString() : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

            oPdfPTable.CompleteRow();
            nSL = _oFabricPlanOrder.ColumnCount - oPdfPTable.Rows.Count;
            if (nSL > 0)
            {
                for (int i = 1; i <= nSL; i++)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();
                }
            }


            return oPdfPTable;
        }
        #endregion
        #endregion

    }
}