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
    public class rptRouteSheetYarnIssue
    {
        #region Declaration
        int _nTotalColumn = 1;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
     
        List<RouteSheetYarnOut> _oRouteSheetYarnOuts = new List<RouteSheetYarnOut>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        RouteSheetSetup _oRouteSheetSetup = new RouteSheetSetup();

        Company _oCompany = new Company();
        string _sDateRange = "";
        int _nCount = 0;
        double _nTotalQty = 0;
        int _nTotalRowCount = 0;
        #endregion

        public byte[] PrepareReport(List<RouteSheetYarnOut> oRouteSheetYarnOuts, RouteSheetSetup oRouteSheetSetup, Company oCompany, BusinessUnit oBusinessUnit,string sDateRange)
        {
            _sDateRange = sDateRange;
            _oRouteSheetYarnOuts = oRouteSheetYarnOuts;
            _oCompany = oCompany;
            _oRouteSheetSetup = oRouteSheetSetup;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.PrintBody();
            this.ReporttFooter();
            _oPdfPTable.HeaderRows = 2;
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
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion

        #region Print Header
        private void ReporttHeader()
        {

            #region Proforma Invoice Heading Print

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 100f, 100f });

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("ISSUED SLIP", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.NORMAL)));
            //oPdfPCell.Border = 0;
             oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
          

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase(" ", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("RAW YARN", FontFactory.GetFont("Tahoma", 16f, iTextSharp.text.Font.BOLD)));
            oPdfPCell.Border = 0;
            // oPdfPCell.Colspan = 2;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);
         

            oPdfPCell = new PdfPCell(new Phrase( _sDateRange, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL)));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDEFINED);

            float[] tableWidths = new float[] { 5f, 15f, 15f, 30f, 10f, 10f, 8f, 8f, 8f, 10f };
            PdfPTable oPdfPTable = new PdfPTable(10);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(tableWidths);

            var data = _oRouteSheetYarnOuts.GroupBy(x => new { x.WorkingUnitID, x.OperationUnitName }, (key, grp) => new
            {
                WorkingUnitID = key.WorkingUnitID,
                OperationUnitName = key.OperationUnitName, 
                SubTotalQty = grp.ToList().Sum(y=>y.Qty),
                SubTotalNoOfHanksCone = grp.ToList().Sum(y => y.NoOfHanksCone),
                Results = grp.ToList().OrderBy(y => y.EventTime)
            });

            foreach (var oData in data)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Store: " + oData.OperationUnitName, _oFontStyleBold));
                _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(tableWidths);

                #region Header
                oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Yarn Out Date", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("NAME OF RAW YARN", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Dye Line No", _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty(" + _oRouteSheetSetup.MUnit + ")", _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Bags/Hank", _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Dyeing Type", _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Add Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(tableWidths);
                _nCount = 0;
                foreach (var oItem in oData.Results)
                {
                    _nCount++;
                    #region Data
                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    //oPdfPCell.Border = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.EventTimeStr, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNoFull, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    //oPdfPCell.Border = 0;
                    // oPdfPCell.Colspan = 2;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    //oPdfPCell.Border = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.RouteSheetNo, _oFontStyle));
                    //oPdfPCell.Border = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    //oPdfPCell.Border = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase((oItem.NoOfHanksCone).ToString(), _oFontStyle));
                    //oPdfPCell.Border = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase((oItem.DyeingType).ToString(), _oFontStyle));
                    //oPdfPCell.Border = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //oPdfPCell.Border = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Add Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable.SetWidths(tableWidths);
                }

                #region Sub Total
                oPdfPCell = new PdfPCell(new Phrase("Sub-Total", _oFontStyleBold));
                oPdfPCell.Colspan = 6;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oData.SubTotalQty), _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(oData.SubTotalNoOfHanksCone.ToString(), _oFontStyleBold));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Add Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                oPdfPTable = new PdfPTable(10);
                oPdfPTable.SetWidths(tableWidths);

            }

            #region Grand Total
            oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyleBold));
            oPdfPCell.Colspan = 6;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((_oRouteSheetYarnOuts.Sum(x => x.Qty))), _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase((_oRouteSheetYarnOuts.Sum(x => x.NoOfHanksCone)).ToString(), _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Add Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region commented Code

            //_nTotalRowCount = _oRouteSheetYarnOuts.Count;
            //foreach(RouteSheetYarnOut oItem in _oRouteSheetYarnOuts)
            //{
            //    _nCount++;

            //    #region Data
            //    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase(oItem.EventTimeStr, _oFontStyle));
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);
 
            //    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNoFull, _oFontStyle));
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);
 
            //    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    // oPdfPCell.Colspan = 2;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase(oItem.RouteSheetNo, _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase( (oItem.NoOfHanksCone).ToString(), _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase((oItem.DyeingType).ToString(), _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    //oPdfPCell.Border = 0;
            //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    oPdfPTable.AddCell(oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //    #endregion

            //    #region Add Table
            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //    #endregion

            //    oPdfPTable = new PdfPTable(10);
            //    oPdfPTable.SetWidths(tableWidths);

            //    _nTotalQty = _nTotalQty + oItem.Qty;
            //}

            //if (_nTotalRowCount < 15)
            //{
            //    for (int i = _nTotalRowCount + 1; i <= 20; i++)
            //    {
            //        #region Blank Row
            //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        //oPdfPCell.Border = 0;
            //        oPdfPCell.FixedHeight = 15f;
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        //oPdfPCell.Border = 0;
            //         oPdfPCell.FixedHeight = 15f;
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        //oPdfPCell.Border = 0;
            //        oPdfPCell.FixedHeight = 15f;
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        //oPdfPCell.Border = 0;
            //        oPdfPCell.FixedHeight = 15f;
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        //oPdfPCell.Border = 0;
            //        oPdfPCell.FixedHeight = 15f;
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        oPdfPTable.AddCell(oPdfPCell);

            //        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        //oPdfPCell.Border = 0;
            //        oPdfPCell.FixedHeight = 15f;
            //        oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        oPdfPTable.AddCell(oPdfPCell);
            //        oPdfPTable.CompleteRow();
            //        #endregion

            //        #region Add Table
            //        _oPdfPCell = new PdfPCell(oPdfPTable);
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();
            //        #endregion

            //        oPdfPTable = new PdfPTable(10);
            //        oPdfPTable.SetWidths(tableWidths);
            //    }
            //}

            //oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            //oPdfPCell.Colspan = 6;
            //oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyleBold));
            ////oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase((_oRouteSheetYarnOuts.Sum(x => x.NoOfHanksCone)).ToString(), _oFontStyleBold));
            ////oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPTable.AddCell(oPdfPCell);

            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //oPdfPCell.Colspan = 3;
            //oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            //#region Add Table
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Print Footer
        private void ReporttFooter()
        {
            #region Proforma Invoice Heading Print

            PdfPTable oPdfPTable = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100, 140f, 100f });

            oPdfPCell = new PdfPCell(new Phrase("STORE KEEPER", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            
            oPdfPCell = new PdfPCell(new Phrase("YARN CONTROLER", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("FACTORY MANAGER", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);
         
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion
       
        private void Blank(int nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
    }
}
