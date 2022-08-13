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
using ESimSol.Reports;

namespace ESimSol.BusinessObjects
{
    public class rptRouteSheetQCReport
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DUOrderRS> _oDUOrderRSs = new List<DUOrderRS>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        int _nColCount = 15;
        #endregion
        public byte[] PrepareReport(List<DUOrderRS> oDUOrderRSs, BusinessUnit oBusinessUnit, Company oCompany, string sHeader)
        {
            _oDUOrderRSs = oDUOrderRSs;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(10f, 10f, 5f, 20f);

            _oPdfPTable = new PdfPTable(1);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] {842});
            #endregion

            #region Report Body & Header
            oCompany.Name = _oBusinessUnit.Name;

            this.PrintHeader(sHeader);
            this.PrintBody(1);
            _oPdfPTable.HeaderRows = 3;
            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sReportHeader)
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 300f, 400f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //_oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        public PdfPTable GetDetailsTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(_nColCount);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[]{
                                                20f,
                                                80f,
                                                85f,
                                                95f,// BUYER --5
                                                80f, 


                                                60f, // DY QTY
                                                55f, // Color
                                                50f, // RS
                                                45f, // 9
                                                
                                                45f,
                                                45f,
                                                45f,
                                                45f,

                                                45f,
                                                45f,
            });
            return oPdfPTable;
        }
      
        #region Report Body Sale Invoice
        private void PrintBody(int eReportLayout)
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,0,20);

            #region Layout Wise Header (Declaration)

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);

            string sHeader = "", sHeaderCoulmn = "";

            sHeader = "Date Basis Report Shift Wise"; sHeaderCoulmn = "Date : ";

            #endregion

            #region Group By Layout Wise
            var dataGrpList = _oDUOrderRSs.GroupBy(x => new { QCDateSt = x.QCDateSt, ShiftName = x.ShiftName }, (key, grp) => new
            {
                HeaderName = key.QCDateSt + " [" + key.ShiftName + "]",
                Results = grp.OrderBy(x=>x.ProductName).ThenBy(x=> x.ProductID).ToList()
            });
            #endregion

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = GetDetailsTable();


            #region Column Header

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            string[] excelCaptios = new string[] { "", "", "", "", "", "A",  "", "B", "C", "D", "E=B+C+D", "F=E-A", "G","H=B-A","I" };
            ESimSolPdfHelper.AddCells(ref oPdfPTable, excelCaptios,Element.ALIGN_CENTER, 15);
            oPdfPTable.CompleteRow();

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "#SL", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Product", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Raw Lot No & Batch/Dye Lot", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Order Info", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Buyer Name", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "QC Approve By", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Dyeing Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
           
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Color Name", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Batch / Dye Lot", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
           
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Packing Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Shading Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Wastage Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Recycle Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Finishing Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Delivery Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, "Balance Qty", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Process Gain/Loss", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Process G/L (%)", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Fresh Y Gain/Loss", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Fresh Y G/L (%)", Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            _oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            string sNumberFormate = "#,##0.00;(#,##0.00)";
            string sPercentageFormate = "##0.00;(##0.00)";

            #region Date Wise Loop
            foreach (var oItem in dataGrpList)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeaderCoulmn+oItem.HeaderName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                #region Data List Wise Loop

                int nCount = 0;
                foreach (var obj in oItem.Results)
                {
                    #region DATA
                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable = GetDetailsTable();

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.LotNo + Environment.NewLine + obj.RouteSheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.OrderNo + Environment.NewLine + obj.BuyerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.OrderQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.BuyerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.QCApproveByName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.RouteSheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.PackingQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.ShadingQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.WastageQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.RecycleQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.FinishQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.QtyDC), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(obj.BalanceQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (obj.ExcessShortQty).ToString(sNumberFormate), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (obj.Qty != 0 ? (obj.ExcessShortQty * 100 / obj.Qty) : 0).ToString(sPercentageFormate), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (obj.BalanceQty).ToString(sNumberFormate), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (obj.Qty != 0 ? (obj.BalanceQty * 100 / obj.Qty) : 0).ToString(sPercentageFormate), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    oPdfPTable.CompleteRow();
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                    #endregion
                }

                if (oItem.Results.Count() > 0)
                {
                    #region Sub Total (Layout Wise)
                    oPdfPTable = new PdfPTable(11);
                    oPdfPTable = GetDetailsTable();
                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, sHeader + " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 5, 0);

                    double nTotalDyeingQty = oItem.Results.Sum(x => x.Qty);
                    double nTotalExcessShortQty = oItem.Results.Sum(x => x.ExcessShortQty);
                    double nExcessShortPercentage = (nTotalDyeingQty!=0 ?  (nTotalExcessShortQty * 100 / nTotalDyeingQty) : 0 );

                    double nTotalBalanceQty = oItem.Results.Sum(x => x.BalanceQty);
                    double nBalancePercentage = (nTotalDyeingQty != 0 ? (nTotalExcessShortQty * 100 / nTotalDyeingQty) : 0);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nTotalDyeingQty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);


                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.PackingQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.ShadingQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.QtyDC)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.BalanceQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.FinishQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (nTotalExcessShortQty).ToString(sNumberFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (nExcessShortPercentage).ToString(sPercentageFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.Qty_Consume)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (nTotalBalanceQty).ToString(sNumberFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (nBalancePercentage).ToString(sPercentageFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                    
                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    oPdfPTable.CompleteRow();
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                    #endregion
                }
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0,0,0,10);
                #endregion
            }
            #region Grand Total (Layout Wise)
            oPdfPTable = new PdfPTable(11);
            oPdfPTable = GetDetailsTable();
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, " Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 5, 0);

            double nTotalDyeingQty_GT = _oDUOrderRSs.Sum(x => x.Qty);
            double nTotalExcessShortQty_GT = _oDUOrderRSs.Sum(x => x.ExcessShortQty);
            double nExcessShortPercentage_GT = (nTotalDyeingQty_GT != 0 ? (nTotalExcessShortQty_GT * 100 / nTotalDyeingQty_GT) : 0);

            double nTotalBalanceQty_GT = _oDUOrderRSs.Sum(x => x.BalanceQty);
            double nBalancePercentage_GT = (nTotalDyeingQty_GT != 0 ? (nTotalExcessShortQty_GT * 100 / nTotalDyeingQty_GT) : 0);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(nTotalDyeingQty_GT), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);


            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.PackingQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.ShadingQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.QtyDC)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.BalanceQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(_oDUOrderRSs.Sum(x => x.FinishQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, (nTotalExcessShortQty_GT).ToString(sNumberFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, (nExcessShortPercentage_GT).ToString(sPercentageFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormat(oItem.Results.Sum(x => x.Qty_Consume)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, (nTotalBalanceQty_GT).ToString(sNumberFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, (nBalancePercentage_GT).ToString(sPercentageFormate), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion
            #endregion
        }
        #endregion
    }
}
