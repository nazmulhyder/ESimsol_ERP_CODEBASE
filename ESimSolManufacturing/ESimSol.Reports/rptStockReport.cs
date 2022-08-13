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

    public class rptStockReport
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyle_UnLine;
        
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<FNOrderUpdateStatus> _oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
        List<FNOrderUpdateStatus> _oTempFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
        DateTime _dStartDate = DateTime.Now;
        DateTime _dEndDate = DateTime.Now;
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        double _nQty = 0;
        string _sUserName = "";
        #endregion

        public byte[] PrepareReport(List<FNOrderUpdateStatus> oFNOrderUpdateStatuss, Company oCompany, BusinessUnit oBusinessUnit, DateTime dStartDate, DateTime dEndDate, string sUserName)
        {
            _oFNOrderUpdateStatuss = oFNOrderUpdateStatuss;
            _sUserName = sUserName;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _dStartDate = dStartDate;
            _dEndDate = dEndDate;

            #region Page Setup
            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 11f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f });
            #endregion

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oCompany, "Daily Delivery Report " + "Date : " + "(" + _dStartDate.ToString("dd MMM yyyy") + " to " + _dEndDate.ToString("dd MMM yyyy") + " ) ", 1);
            //this.PrintEmptyRow();
            this.PrintTableHeaderRight("Date : " + DateTime.Now.ToString("dd MMM yyyy"));
            this.PrintBody();
            //EnumFabricRequestType eOrderType = 0;
            //List<FNOrderUpdateStatus> oLoops = _oFNOrderUpdateStatuss.GroupBy(x => x.OrderType).Select(x => x.First()).ToList();
            //foreach (FNOrderUpdateStatus obj in oLoops)
            //{
            //    _oTempFNOrderUpdateStatuss = _oFNOrderUpdateStatuss.Where(x => x.OrderType == obj.OrderType).ToList();
            //    if (_oTempFNOrderUpdateStatuss.Count > 0)
            //    {
            //        //this.PrintEmptyRow();
            //        eOrderType = (EnumFabricRequestType)obj.OrderType;
            //        this.PrintTableHeader(eOrderType.ToString());
            //        this.DataTable();
            //    }
            //}
            //this.GrandTotal();
            //this.PrintEmptyRow();
            this.SummaryTable();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void DataTable()
        {
             PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(GetColSize());
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

             #region Table Header
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Garments", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Construstion", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Design", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Width", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Delivery", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Delivered", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Due Delivery", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            int nSL = 1;
            foreach (FNOrderUpdateStatus oItem in _oTempFNOrderUpdateStatuss)
            {
                PdfPTable oCPdfPTable = new PdfPTable(13);
                oCPdfPTable.WidthPercentage = 100;
                oCPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oCPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                oCPdfPTable.SetWidths(GetColSize());

                _oPdfPCell = new PdfPCell(new Phrase(nSL++.ToString(), _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExeNo, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Construction, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.WeaveName, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.FabricWidth, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Color, _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);
               
                 _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.OrderQty, 2).ToString(), _oFontStyle));
                 _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.DeliveryQty, 2).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.QtyOut, 2).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.YetToDelivery, 2).ToString(), _oFontStyle));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(oCPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.Colspan = 1;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            
            
            
            }
            PdfPTable oCPdfPTable1 = new PdfPTable(13);
            oCPdfPTable1.WidthPercentage = 100;
            oCPdfPTable1.HorizontalAlignment = Element.ALIGN_LEFT;
            oCPdfPTable1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oCPdfPTable1.SetWidths(GetColSize());

            #region Total
            _oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyleBold)); _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oTempFNOrderUpdateStatuss.Sum(x => x.OrderQty), 2).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oTempFNOrderUpdateStatuss.Sum(x => x.DeliveryQty), 2).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oTempFNOrderUpdateStatuss.Sum(x => x.QtyOut), 2).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable1.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oTempFNOrderUpdateStatuss.Sum(x => x.YetToDelivery), 2).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable1.AddCell(_oPdfPCell);

              #endregion

            _oPdfPCell = new PdfPCell(oCPdfPTable1);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void GrandTotal()
        {
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(GetColSize());
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyleBold)); _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oFNOrderUpdateStatuss.Sum(x => x.OrderQty), 2).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oFNOrderUpdateStatuss.Sum(x => x.DeliveryQty), 2).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oFNOrderUpdateStatuss.Sum(x => x.QtyOut), 2).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(_oFNOrderUpdateStatuss.Sum(x => x.YetToDelivery), 2).ToString(), _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
       
        public void PrintEmptyRow()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.ExtraParagraphSpace = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintTableHeader(string sHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle)); _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void PrintTableHeaderRight(string sHeader)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle)); _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private float[] GetColSize()
        {
            return new float[] { 35, 80, 75, 150, 150, 120, 95, 70, 100, 80, 65, 65, 80 };
        }

        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);
            int nSLNo = 0;

            var oFNOrderTypes = _oFNOrderUpdateStatuss.GroupBy(x => new { x.OrderType, x.OrderName }, (key, grp) =>
                                  new
                                  {
                                      OrderType = key.OrderType,
                                      OrderName = key.OrderName,
                                  }).ToList();

            PdfPTable oPdfPTable = new PdfPTable(13);
            //oPdfPTable.SetWidths(new float[] { 35, 80, 75, 150, 150, 120, 95, 70, 100, 80, 65, 65, 80 });

            //#region PO Info
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Issue information", 0, 14, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);

            //oPdfPTable.CompleteRow();


            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //#endregion
            foreach (var oItem1 in oFNOrderTypes)
            {
                var oFNOrderUpdateStatuss = _oFNOrderUpdateStatuss.Where(x => x.OrderType == oItem1.OrderType).ToList();
                #region Fabric Info
                oPdfPTable = new PdfPTable(13);
                oPdfPTable.SetWidths(new float[] { 35, 80, 75, 150, 150, 120, 95, 70, 100, 80, 65, 65, 80 });
                if (oFNOrderUpdateStatuss.Count > 0)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.OrderName, 0, 13, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                    oPdfPTable.CompleteRow();

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI No", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Dispo No", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "End Buyer", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Customer", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Construstion", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Design", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Width", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Color", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivered", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Due Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    nSLNo = 0;
                }
                #endregion

                foreach (var oItem in oFNOrderUpdateStatuss)
                {
                    oPdfPTable = new PdfPTable(13);
                    oPdfPTable.SetWidths(new float[] { 35, 80, 75, 150, 150, 120, 95, 70, 100, 80, 65, 65, 80 });
                    nSLNo++;
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.PINo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ExeNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.BuyerName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ContractorName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Construction, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WeaveName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.FabricWidth, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.Color, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.OrderQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.DeliveryQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyOut), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.YetToDelivery), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                }
                oPdfPTable = new PdfPTable(13);
                oPdfPTable.SetWidths(new float[] { 35, 80, 75, 150, 150, 120, 95, 70, 100, 80, 65, 65, 80 });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 9, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nQty = oFNOrderUpdateStatuss.Select(c => c.OrderQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nQty = oFNOrderUpdateStatuss.Select(c => c.DeliveryQty).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nQty = oFNOrderUpdateStatuss.Select(c => c.QtyOut).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nQty = oFNOrderUpdateStatuss.Select(c => c.YetToDelivery).Sum();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }

        }
        public void SummaryTable()
        {

            var oFNOrderTypes = _oFNOrderUpdateStatuss.GroupBy(x => new { x.OrderType, x.OrderName }, (key, grp) =>
                                  new
                                  {
                                      OrderType = key.OrderType,
                                      OrderName = key.OrderName,
                                      OrderQty = grp.Sum(x => x.OrderQty),
                                      DeliveryQty = grp.Sum(x => x.DeliveryQty),
                                      QtyOut = grp.Sum(x => x.QtyOut),
                                      YetToDelivery = grp.Sum(x => x.YetToDelivery),
                                  }).ToList();

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 100f, 80f, 80f, 80f, 80f, 80f, 100f });

            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Prepared By", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivered Qty" + "(" + _dStartDate.ToString("dd MMM yyyy") + " to " + _dEndDate.ToString("dd MMM yyyy") + " ) ", 0, 5, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle_UnLine);
            oPdfPTable.CompleteRow();
           // ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, _sUserName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle_UnLine);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Type", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Order Qty", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Delivered", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Due Delivery", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
       

            foreach (var oItem in oFNOrderTypes)
            {
                 oPdfPTable = new PdfPTable(7);
                 oPdfPTable.SetWidths(new float[] { 100f, 80f, 80f, 80f, 80f, 80f, 100f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.OrderName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.OrderQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.DeliveryQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.QtyOut), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.OrderQty - oItem.DeliveryQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
             oPdfPTable = new PdfPTable(7);
             oPdfPTable.SetWidths(new float[] { 100f, 80f, 80f, 80f, 80f, 80f, 100f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nQty = oFNOrderTypes.Select(c => c.OrderQty).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nQty = oFNOrderTypes.Select(c => c.DeliveryQty).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nQty = oFNOrderTypes.Select(c => c.QtyOut).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            _nQty = oFNOrderTypes.Select(c => c.YetToDelivery).Sum();
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_nQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


          
        }
    }
}



