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
    public class SelectedField
    {
        public SelectedField()
        {
            Column = "";
            FieldName = "";
            Caption = "";
            Width = 0;
            Datatype = "";
            Field = FieldType.Data;
            Algin = Alginment.LEFT;
            Format = 0;
            Sum = 0;
        }
        public SelectedField(string FieldName, string Caption, float Width, FieldType Field, Alginment Align)
        {
            this.FieldName = FieldName;
            this.Caption = Caption;
            this.Width = Width;
            this.Field = Field;
            this.Total = 0;
            this.Algin = Align;
        }
        public SelectedField(string FieldName, string Caption, float Width, FieldType Field)
        {
            this.FieldName = FieldName;
            this.Caption = Caption;
            this.Width = Width;
            this.Field = Field;
            this.Total = 0;
        }
        public SelectedField(string Column, string FieldName, string Datatype)
        {
            this.Column = Column;
            this.FieldName = FieldName;
            this.Datatype = Datatype;
        }
        public string Column { get; set; }
        public string FieldName { get; set; }
        public string Caption { get; set; }
        public float Width { get; set; }
        public string Datatype { get; set; }
        public double Total { get; set; }
        public int Format { get; set; }
        public int Sum { get; set; }
        public FieldType Field { get; set; }
        public Alginment Algin { get; set; }
        public enum FieldType { Data, Total }
        public enum Alginment { LEFT, RIGHT, CENTER }
        public List<SelectedField> SelectedFields { get; set; }
    }
    
    public class rptDynamicReport
    {
        public rptDynamicReport(int PageWidth, int pageHeight)
        {
            this._nPageWidth = PageWidth;
            this._nPageHeight = pageHeight;
            this.SpanTotal = 0;
        }

        #region Declaration
        public int _nColumns = 0, FooterRowHeight = 10;
        int _nPageWidth { get; set; }
        int _nPageHeight { get; set; }
        public int SpanTotal { get; set; } //colSpanForTotal
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyle_Bold_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable;
        PdfPCell _oPdfPCell;

        public string SLNo = "SL", DateRange="";
        public float FirstColumn = 15f;

        public bool PrintESimSolFooter = false, PrintSignatureList=false;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<object> _oObjects = new List<object>();
        SelectedField _oSelectedField = new SelectedField();
        List<SelectedField> _oSelectedFields = new List<SelectedField>();
        List<InventoryTraking> _oInventoryTrakings = new List<InventoryTraking>();
        string _sDateRange = "";

        public string[] SignatureList; 
        public string[] DataList;
        #endregion

        public byte[] PrepareReport(List<Object> oObjects, BusinessUnit oBusinessUnit, Company oCompany, string sHeader,List<SelectedField> oSelectedFields)
        {
            _oObjects = oObjects;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oSelectedFields = oSelectedFields;

            #region Page Setup
            _nColumns = oSelectedFields.Count+1;

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = FirstColumn;
            for (int i = 1; i < _nColumns; i++)
            {
                tablecolumns[i] = Convert.ToInt32(oSelectedFields[i-1].Width);
            }

            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _nPageHeight), 0f, 0f, 0f, 0f);
    
            _oDocument.SetMargins(20f, 20f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            if (this.PrintESimSolFooter) 
            {
                PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
                PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                ESimSolFooter PageEventHandler = new ESimSolFooter();
            }

            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);
            #endregion

            this.PrintHeader(sHeader);
            this.PrintBody();

            if (this.PrintSignatureList)
            {
                this.PrintFooter();
                _oPdfPTable.CompleteRow();
            }

            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sHeader)
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
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nColumns; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; 
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #endregion

            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthBottom = 0.5f;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            if (!string.IsNullOrEmpty(this.DateRange))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(DateRange, _oFontStyle)); _oPdfPCell.Colspan = _oSelectedFields.Count() + 1;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(SLNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            #region RowHeader
            foreach (SelectedField oItem_Field in _oSelectedFields)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem_Field.Caption, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPTable.CompleteRow();
            #endregion

            #region Data
            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            foreach (Object oItem in _oObjects)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                #region Get Field Data
                foreach (SelectedField oItem_Field in _oSelectedFields)
                {
                    var Data=oItem.GetType().GetProperty(oItem_Field.FieldName).GetValue(oItem, null);
                    if (Data != null && (Data.GetType() == typeof(double) || Data.GetType() == typeof(int)))
                    {
                        oItem_Field.Total+=( Convert.ToDouble(Data));

                        if (Data.GetType() == typeof(int))
                            _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat_Round( Convert.ToDouble(Data)), _oFontStyle));
                        if (Data.GetType() == typeof(double))
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Convert.ToDouble(Data)), _oFontStyle));

                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(Data.ToString(), _oFontStyle));

                        if (oItem_Field.Algin == SelectedField.Alginment.LEFT)
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        else if(oItem_Field.Algin==SelectedField.Alginment.RIGHT)
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        else if (oItem_Field.Algin == SelectedField.Alginment.CENTER)
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion
            }
            _oPdfPTable.CompleteRow();
            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            int nStart = 0;
            if (SpanTotal > 0) 
            {
                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = SpanTotal;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                foreach (SelectedField oItem_Field in _oSelectedFields)
                {
                    nStart++;
                    if (SpanTotal<=nStart && oItem_Field.Field == SelectedField.FieldType.Total)
                    {
                        //var Data = Global.MillionFormat(_oObjects.Sum(x => Convert.ToDouble(x.GetType().GetProperty(oItem_Field.FieldName).GetValue(oItem_Field, null))));
                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem_Field.Total), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else if (SpanTotal <= nStart)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                }
                _oPdfPTable.CompleteRow();
                #endregion
            }
            #endregion
        }
        #endregion

        #region Footer
        private void PrintFooter()
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, 1);
            ESimSolPdfHelper.Signature(ref _oPdfPTable, SignatureList, DataList, _nPageWidth - FooterRowHeight, FooterRowHeight, _oSelectedFields.Count() + 1);
        }
        #endregion

        #region IT
        public byte[] PrepareReportDyeing(List<InventoryTraking> oInventoryTrakings, BusinessUnit oBusinessUnit, Company oCompany, string sHeader, string sDateRange)
        {
            
            _oInventoryTrakings = oInventoryTrakings;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _sDateRange = sDateRange;
            _nColumns = 1;
            #region Page Setup
            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(PageSize.A4, 5f, 5f, 2f, 2f);
            //_oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter _oWriter;
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler   

            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 98;
            _oPdfPTable.SetWidths(new float[] { 595f });//height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);


            #endregion


            this.PrintHeader(sHeader);
            this.PrintReportHeader();

            if (this.PrintSignatureList)
            {
                this.PrintFooter();
                _oPdfPTable.CompleteRow();
            }

            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintReportHeader()
        {
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 50f, 30f, 90f, 30f, 90f, 50f });

            #region PO Info
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Issue information", 0, 8, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //oPdfPTable.CompleteRow();
            if (_oInventoryTrakings.Count > 0)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Store Name", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oInventoryTrakings[0].WorkingUnitName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Name", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, ":" + _oInventoryTrakings[0].ProductCode + " " + _oInventoryTrakings[0].ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                if (_sDateRange.Contains("12:00 AM"))
                {
                    _sDateRange = _sDateRange.Replace("12:00 AM", "");
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _sDateRange, 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                ////                            WtPerBag = grp.Sum(p => p.WtPerBag),
                var oITs = _oInventoryTrakings.GroupBy(x => new { x.ParentType }, (key, grp) =>
                                        new InventoryTraking
                                        {
                                            ParentType = key.ParentType,
                                            InQty = grp.Sum(p => p.InQty),
                                            OutQty = grp.Sum(p => p.OutQty),
                                        }).ToList();
                oITs = oITs.OrderBy(x => x.ParentType).ToList();

                foreach(InventoryTraking oitem in oITs)
                {
                    oInventoryTrakings = _oInventoryTrakings.Where(x => x.ParentType == oitem.ParentType).ToList();
                    PrintBodyIT(oInventoryTrakings);
                }
            }
          
            #endregion
                       
        }
        private void PrintBodyIT(List<InventoryTraking> oInventoryTrakings)
        {
            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 80f, 130f, 85f, 75f, 75f, 80f,80f});
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);
            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oInventoryTrakings[0].ParentTypeEnumSt, 0, 7, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((EnumTriggerParentsType)oInventoryTrakings[0].ParentType).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((EnumTriggerParentsType)oInventoryTrakings[0].ParentType).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((EnumTriggerParentsType)oInventoryTrakings[0].ParentType).ToString(), 0, 2, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);

            oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product Name", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "LotNo", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "InQty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "OutQty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ref. No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "User Name", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            //double nQtySRSQty = 0, nQtySRMQty = 0, nDyeingQty = 0;
            #endregion
            foreach (var oItem in oInventoryTrakings)
            {
                //nQtySRSQty = 0; nQtySRMQty = 0;
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 80f, 130f, 85f, 75f, 75f, 80f, 80f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDatetimeSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InQty>0)?Global.MillionFormat(oItem.InQty):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutQty>0)?Global.MillionFormat(oItem.OutQty):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RefNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.UserName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 80f, 130f, 85f, 75f, 75f, 80f, 80f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oInventoryTrakings.Sum(x => x.InQty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oInventoryTrakings.Sum(x => x.OutQty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


        }

        public byte[] PrepareReportDyeingAllStore(List<InventoryTraking> oInventoryTrakings, BusinessUnit oBusinessUnit, Company oCompany, string sHeader, string sDateRange)
        {

            _oInventoryTrakings = oInventoryTrakings;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _sDateRange = sDateRange;
            _nColumns = 1;
            #region Page Setup
            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(PageSize.A4, 5f, 5f, 2f, 2f);
            //_oDocument.SetPageSize(PageSize.A4);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter _oWriter;
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler   

            _oDocument.Open();
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.WidthPercentage = 98;
            _oPdfPTable.SetWidths(new float[] { 595f });//height:842   width:595
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);


            #endregion


            this.PrintHeader(sHeader);
            this.PrintReportHeaderAllStore();

            if (this.PrintSignatureList)
            {
                this.PrintFooter();
                _oPdfPTable.CompleteRow();
            }

            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintReportHeaderAllStore()
        {
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC);

            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 50f, 30f, 90f, 30f, 90f, 50f });

            #region PO Info
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Raw Issue information", 0, 8, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //oPdfPTable.CompleteRow();
            if (_oInventoryTrakings.Count > 0)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Name", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Yarn Name :" + _oInventoryTrakings[0].ProductCode + " " + _oInventoryTrakings[0].ProductName, 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "" , 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                if (_sDateRange.Contains("12:00 AM"))
                {
                    _sDateRange = _sDateRange.Replace("12:00 AM", "");
                }
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _sDateRange, 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                ////                            WtPerBag = grp.Sum(p => p.WtPerBag),
                var oITs = _oInventoryTrakings.GroupBy(x => new { x.WUName,x.WorkingUnitID }, (key, grp) =>
                                        new InventoryTraking
                                        {
                                            WorkingUnitID = key.WorkingUnitID,
                                            WUName = key.WUName,
                                            InQty = grp.Sum(p => p.InQty),
                                            OutQty = grp.Sum(p => p.OutQty),
                                        }).ToList();
                oITs = oITs.OrderBy(x => x.WorkingUnitID).ToList();

                foreach (InventoryTraking oitem in oITs)
                {
                    oInventoryTrakings = _oInventoryTrakings.Where(x => x.WorkingUnitID == oitem.WorkingUnitID).ToList();
                    if (oInventoryTrakings.Count > 0)
                    {
                        PrintBodyITAllStore(oInventoryTrakings);
                    }
                }
            }

            #endregion

        }
        private void PrintBodyITAllStore(List<InventoryTraking> oInventoryTrakings)
        {
            oInventoryTrakings = oInventoryTrakings.OrderBy(x => x.StartDate).ToList();

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 80f, 130f, 85f, 75f, 75f, 80f, 80f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLDITALIC);
            #region PO Info
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, oInventoryTrakings[0].WorkingUnitName, 0, 7, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((EnumTriggerParentsType)oInventoryTrakings[0].ParentType).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((EnumTriggerParentsType)oInventoryTrakings[0].ParentType).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((EnumTriggerParentsType)oInventoryTrakings[0].ParentType).ToString(), 0, 2, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyle_UnLine);

            oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLDITALIC);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Date", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Type ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "LotNo", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "InQty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "OutQty", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Ref. No", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "User Name", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            //double nQtySRSQty = 0, nQtySRMQty = 0, nDyeingQty = 0;
            #endregion
            foreach (var oItem in oInventoryTrakings)
            {
                //nQtySRSQty = 0; nQtySRMQty = 0;
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.SetWidths(new float[] { 80f, 130f, 85f, 75f, 75f, 80f, 80f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.StartDatetimeSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ParentTypeEnumSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InQty > 0) ? Global.MillionFormat(oItem.InQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutQty > 0) ? Global.MillionFormat(oItem.OutQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.RefNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.UserName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            }
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.SetWidths(new float[] { 80f, 130f, 85f, 75f, 75f, 80f, 80f });
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 3, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oInventoryTrakings.Sum(x => x.InQty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oInventoryTrakings.Sum(x => x.OutQty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 2, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


        }
        #endregion

    }
}
