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
    public class rptFNProductionBatchConsumtion
    {
        #region Declaration
        int _nTotalColumn = 10;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();

        FNBatch _oFNBatch = new FNBatch();
        List<FNBatch> _oFNBatchs = new List<FNBatch>();
        List<FNBatchCard> _oFNBCards = new List<FNBatchCard>();
        List<FNProductionBatch> _oFNPBatchs = new List<FNProductionBatch>();
        List<FNProductionConsumption> _oFNPConsumptions = new List<FNProductionConsumption>();
        
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();

        string _sMessage = "", sCurrencySymbol="";
        BaseColor _oBC = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EFF0EA"));
        #endregion

        public byte[] PrepareReport(FNBatch oFNBatch, List<FNBatchCard> oFNBCards, List<FNProductionConsumption> oFNPConsumptions, Company oCompany, BusinessUnit oBU)
        {
            _oFNBatch = oFNBatch;
            _oFNBCards = oFNBCards;
            _oFNPConsumptions = oFNPConsumptions;
            
            _oCompany = oCompany;
            _oBusinessUnit = oBU;

            #region Page Setup     
            _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 842f), 0f, 0f, 0f, 0f); //A4:842*595   
            _oDocument.SetMargins(10f, 10f, 5f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] {595f});
            #endregion

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oBusinessUnit, _oCompany, "Production Consumtion Statement (Batch)", 0);
            this.PrintFirstTable();
            this.PrintBody();

            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public byte[] PrepareReport(List<FNBatch> oFNBatchs, List<FNBatchCard> oFNBCards, List<FNProductionConsumption> oFNPConsumptions, Company oCompany, BusinessUnit oBU)
        {
            _oFNBatchs = oFNBatchs;

            if (oFNBatchs.Any())
            _oFNBatch = oFNBatchs.FirstOrDefault();
           
            _oFNBCards = oFNBCards;
            _oFNPConsumptions = oFNPConsumptions;

            _oCompany = oCompany;
            _oBusinessUnit = oBU;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 842f), 0f, 0f, 0f, 0f); //A4:842*595   
            _oDocument.SetMargins(10f, 10f, 5f, 10f);
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

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oBusinessUnit, _oCompany, "Production Consumtion Statement", 0);
            this.PrintFirstTable_Dispo();
            this.PrintBody_Dispo();

            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        public void PrintFirstTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER; ;
            oPdfPTable.SetWidths(new float[] { 50, 105, 60, 185, 50, 135});
            iTextSharp.text.Font _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
           
            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 1st Row
            _oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" "+(_oFNBatch.BatchNo), _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oFNBatch.BuyerName, _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MKT Ref", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oFNBatch.FabricNo, _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd Row
            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oFNBatch.SCNoFull, _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PINo", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oFNBatch.Params, _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(("Total Qty"), _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat(_oFNPConsumptions.Sum(x=>x.Qty)), _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBody()
        {
            #region FN Production Batch

            _nTotalColumn = 9;
            PdfPTable oPdfPTable = new PdfPTable(_nTotalColumn);

            int nColCount = 0;
            float[] nColWidths = new float[_nTotalColumn];

            //#SL  Process  Item Code   Name Of Dyes & Chemicals    Unit 
            nColWidths[nColCount++] = 35f; //25f
            nColWidths[nColCount++] = 80f; //115f
            nColWidths[nColCount++] = 60f; //155f
            nColWidths[nColCount++] = 120f; //275f
            nColWidths[nColCount++] = 30f; //295f

            // Lot No      Required Qty        UP   Amount
            nColWidths[nColCount++] = 80f; //335f
            nColWidths[nColCount++] = 60f; //415f
            nColWidths[nColCount++] = 60f; //495f
            nColWidths[nColCount++] = 60f; //555f
            oPdfPTable.SetTotalWidth(nColWidths);

            int nCount = 0, nFNTreatmentProcessID = 0;
            double nGrand_Total_Qty = 0, nSub_Total_Qty = 0;
            double nGrand_Total_Amount = 0, nSub_Total_Amount = 0;

            EnumFNTreatment ePrevious_FNTreatment = EnumFNTreatment.None;

            foreach (var oFNBCard in _oFNBCards)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);

                List<FNProductionConsumption> oFNProductionConsumptions_ForThisBatch = new List<FNProductionConsumption>();
                oFNProductionConsumptions_ForThisBatch = _oFNPConsumptions.Where(x => x.FNBatchCardID == oFNBCard.FNBatchCardID).ToList();

                if (oFNProductionConsumptions_ForThisBatch.Count() <= 0) continue;

                if (ePrevious_FNTreatment != oFNBCard.FNTreatment)
                {
                    nCount = 0;

                    if (ePrevious_FNTreatment != EnumFNTreatment.None)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                        #region Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Treatment Wise Sub Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSub_Total_Qty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSub_Total_Amount), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        nSub_Total_Qty = nSub_Total_Amount = 0;
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    }


                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oFNBCard.FNTreatmentSt, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, _nTotalColumn, 0);

                    #region Heading Print

                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                    string[] Headers = new string[] { "#SL", "Process", "Item Code", "Name Of Dyes & Chemicals", "Unit", "Lot No", "Qty", "Unit Price", "Amount" };
                    ESimSolPdfHelper.AddCells(ref oPdfPTable, Headers, Element.ALIGN_LEFT, 15, BaseColor.LIGHT_GRAY);

                    #endregion

                    #region ADD To Main Table

                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                    oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);

                    #endregion
                }

                foreach (var oItem in oFNProductionConsumptions_ForThisBatch)
                {
                    #region Data

                    if (nFNTreatmentProcessID != oFNBCard.FNTreatmentProcessID)
                    {
                        nCount++;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = oFNProductionConsumptions_ForThisBatch.Count();
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oFNBCard.FNProcess, _oFontStyle)); _oPdfPCell.Rowspan = oFNProductionConsumptions_ForThisBatch.Count();
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Symbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice) + oItem.CurrencySymbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty * oItem.UnitPrice) + oItem.CurrencySymbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    sCurrencySymbol = oItem.CurrencySymbol;
                    nFNTreatmentProcessID = oFNBCard.FNTreatmentProcessID;
                    nSub_Total_Qty += oItem.Qty;
                    nSub_Total_Amount += oItem.Qty * oItem.UnitPrice;
                    nGrand_Total_Qty += oItem.Qty;
                    nGrand_Total_Amount += oItem.Qty * oItem.UnitPrice;
                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oFNProductionConsumptions_ForThisBatch.Sum(x => x.Qty)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oFNProductionConsumptions_ForThisBatch.Sum(x => x.Qty * x.UnitPrice)) + sCurrencySymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                #region Add To Main Table
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);
                #endregion

                ePrevious_FNTreatment = oFNBCard.FNTreatment;
            }
            #region Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Treatment Wise Sub Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSub_Total_Qty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSub_Total_Amount) + sCurrencySymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrand_Total_Qty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrand_Total_Amount) + sCurrencySymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Add To Main Table
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 35f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Signature Print

            //oPdfPTable = new PdfPTable(5);
            //oPdfPTable.SetWidths(new float[] {  109f, //Requisitioned
            //                                    109f, //Approved
            //                                    109f, //Received
            //                                    109f, //Issued
            //                                    109f //Manager
            //                            });
            //oPdfPTable.WidthPercentage = 100;
            //_oPdfPCell = new PdfPCell(new Phrase("Requisitioned By:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Approved By:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Received By:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Issued By:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Manager Store:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }

        public void PrintFirstTable_Dispo()
        {
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER; ;
            oPdfPTable.SetWidths(new float[] { 50, 105, 60, 185, 50, 135 });
            iTextSharp.text.Font _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region 1st Row
            _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + (_oFNBatch.FNExONo), _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oFNBatch.BuyerName, _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MKT Ref", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oFNBatch.FabricNo, _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd Row
            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oFNBatch.SCNoFull, _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PINo", _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + _oFNBatch.Params, _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(("Total Qty"), _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" " + Global.MillionFormat(_oFNPConsumptions.Sum(x => x.Qty)), _oFontStyle));
            _oPdfPCell.Border = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBody_Dispo()
        {
            #region FN Production Batch

            _nTotalColumn = 9;
            PdfPTable oPdfPTable = new PdfPTable(_nTotalColumn);

            int nColCount = 0;
            float[] nColWidths = new float[_nTotalColumn];

            //#SL  Process  Item Code   Name Of Dyes & Chemicals    Unit 
            nColWidths[nColCount++] = 35f; //25f
            nColWidths[nColCount++] = 80f; //115f
            nColWidths[nColCount++] = 60f; //155f
            nColWidths[nColCount++] = 120f; //275f
            nColWidths[nColCount++] = 30f; //295f

            // Lot No      Required Qty        UP   Amount
            nColWidths[nColCount++] = 80f; //335f
            nColWidths[nColCount++] = 60f; //415f
            nColWidths[nColCount++] = 60f; //495f
            nColWidths[nColCount++] = 60f; //555f
            oPdfPTable.SetTotalWidth(nColWidths);

            int nCount = 0, nFNTreatmentProcessID = 0;
            double nGrand_Total_Qty = 0, nSub_Total_Qty = 0;
            double nGrand_Total_Amount = 0, nSub_Total_Amount = 0;

            EnumFNTreatment ePrevious_FNTreatment = EnumFNTreatment.None;

            foreach (var oFNBCard in _oFNBCards)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);

                List<FNProductionConsumption> oFNProductionConsumptions_ForThisBatch = new List<FNProductionConsumption>();
                oFNProductionConsumptions_ForThisBatch = _oFNPConsumptions.Where(x => x.FNBatchCardID == oFNBCard.FNBatchCardID).ToList();

                if (oFNProductionConsumptions_ForThisBatch.Count() <= 0) continue;

                if (ePrevious_FNTreatment != oFNBCard.FNTreatment)
                {
                    nCount = 0;

                    if (ePrevious_FNTreatment != EnumFNTreatment.None)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                        #region Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Treatment Wise Sub Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSub_Total_Qty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSub_Total_Amount), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        nSub_Total_Qty = nSub_Total_Amount = 0;
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    }

                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, oFNBCard.FNTreatmentSt, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, _nTotalColumn, 0);

                    #region Heading Print

                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                    string[] Headers = new string[] { "#SL", "Process", "Item Code", "Name Of Dyes & Chemicals", "Unit", "Lot No", "Qty", "Unit Price", "Amount" };
                    ESimSolPdfHelper.AddCells(ref oPdfPTable, Headers, Element.ALIGN_LEFT, 15, BaseColor.LIGHT_GRAY);

                    #endregion

                    #region ADD To Main Table

                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                    oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);

                    #endregion
                }

                foreach (var oItem in oFNProductionConsumptions_ForThisBatch)
                {
                    #region Data

                    if (nFNTreatmentProcessID != oFNBCard.FNTreatmentProcessID)
                    {
                        nCount++;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = oFNProductionConsumptions_ForThisBatch.Count();
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oFNBCard.FNProcess, _oFontStyle)); _oPdfPCell.Rowspan = oFNProductionConsumptions_ForThisBatch.Count();
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Symbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice) + oItem.CurrencySymbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty * oItem.UnitPrice) + oItem.CurrencySymbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    sCurrencySymbol = oItem.CurrencySymbol;
                    nFNTreatmentProcessID = oFNBCard.FNTreatmentProcessID;
                    nSub_Total_Qty += oItem.Qty;
                    nSub_Total_Amount += oItem.Qty * oItem.UnitPrice;
                    nGrand_Total_Qty += oItem.Qty;
                    nGrand_Total_Amount += oItem.Qty * oItem.UnitPrice;
                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oFNProductionConsumptions_ForThisBatch.Sum(x => x.Qty)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oFNProductionConsumptions_ForThisBatch.Sum(x => x.Qty * x.UnitPrice)) + sCurrencySymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                #region Add To Main Table
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);
                #endregion

                ePrevious_FNTreatment = oFNBCard.FNTreatment;
            }
            #region Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Treatment Wise Sub Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSub_Total_Qty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSub_Total_Amount) + sCurrencySymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total", _oFontStyle)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrand_Total_Qty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrand_Total_Amount) + sCurrencySymbol, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Add To Main Table
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            oPdfPTable = new PdfPTable(_nTotalColumn); oPdfPTable.SetTotalWidth(nColWidths);
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 35f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Signature Print

            //oPdfPTable = new PdfPTable(5);
            //oPdfPTable.SetWidths(new float[] {  109f, //Requisitioned
            //                                    109f, //Approved
            //                                    109f, //Received
            //                                    109f, //Issued
            //                                    109f //Manager
            //                            });
            //oPdfPTable.WidthPercentage = 100;
            //_oPdfPCell = new PdfPCell(new Phrase("Requisitioned By:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Approved By:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Received By:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Issued By:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Manager Store:\nDate:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion
    }
}
