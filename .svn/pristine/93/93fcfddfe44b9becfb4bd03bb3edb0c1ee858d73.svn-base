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
    public class rptFabricDeliveryOrder_B
    {
        #region Declaration
        int _nColumns = 1;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        Phrase _oPhrase = new Phrase();
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FabricDeliveryOrder _oFabricDeliveryOrder = new FabricDeliveryOrder();
        List<FabricDeliveryOrderDetail> _oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
        List<FabricDeliverySchedule> _oFabricDeliverySchedules = new List<FabricDeliverySchedule>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
      
        Company _oCompany = new Company();
        string _sMUnit = "";
      
     
        List<ExportPI> _oExportPIs = new List<ExportPI>();
        int _nCount = 0;
        int _nCount_P = 0;
        double _nTotalAmount = 0;
        double _nTotalQty = 0;
        double _nTotalDSQty = 0;
        bool _bIsInYard = true;
        #endregion
        float _nUsagesHeight = 0;
        public byte[] PrepareReport(FabricDeliveryOrder oFabricDeliveryOrder, Company oCompany, BusinessUnit oBusinessUnit, bool bIsInYard, List<ExportPI> oExportPIs)
        {
            _oFabricDeliveryOrder = oFabricDeliveryOrder;
            _oExportPIs = oExportPIs;
            _oBusinessUnit = oBusinessUnit;
            _oFabricDeliveryOrderDetails = oFabricDeliveryOrder.FDODetails;
            _oCompany = oCompany;
            if (_oFabricDeliveryOrderDetails.Count > 0)
            {
                _sMUnit = _oFabricDeliveryOrderDetails[0].MUName;
            }
            _bIsInYard = bIsInYard;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
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
           this.ExportDO();
            //this.PrintFooter();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Print Header
        private void PrintHeader()
        {
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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion
        private void ExportDO()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 0);

            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 200f, 200f });

            #region 1st Row

            _oPdfPCell = new PdfPCell(this.ExportDOFirstRowLeft());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(this.ExportDOFirstRowRight());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Delivery Point
            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.DeliveryPoint))
            {
                oPdfPTable = new PdfPTable(2);
                oPdfPTable.SetWidths(new float[] { 200, 200f });

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Delivery Point : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.DeliveryPoint, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
                

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Name of buying House
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 300f, 80f });

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Garments Contact Persone
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 150f, 110f, 130f });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPhrase = new Phrase();
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPhrase.Add(new Chunk("Mkt. Person : ", _oFontStyle));
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.MKTPerson, _oFontStyle));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Detail Table

            this.Blank(3);

            #region Title

            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 20f, 140f, 80f, 75f, 80f, 70f, 65f,15f, 50f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));// (" + _sMUnit + ")"
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty (" + _sMUnit + ")", _oFontStyle));// 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Dispo No", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion

            #region Detail

            double nTotalQty = 0;
            _nTotalAmount = 0;
            string sTemp = "";
            var oFDODetails = _oFabricDeliveryOrder.FDODetails.GroupBy(x => new { x.FabricNo, x.ProductID, x.ProductName, x.StyleNo, x.FabricWeave, x.Construction, x.FabricWidth, x.MUName, x.ProcessType, x.FinishType, x.DOPriceType, x.DOPriceTypeSt }, (key, grp) =>
                                    new
                                    {
                                        ProductID = key.ProductID,
                                        ProductName = key.ProductName,
                                        StyleNo = key.StyleNo,
                                        Color_Grp = grp.GroupBy(p => new { p.ExeNo, p.StyleNo, p.BuyerRef, p.ColorInfo, p.FEONo, p.Weight, p.Shrinkage }, (color_key, color_grp) => new
                                        {
                                            StyleNo = color_key.StyleNo,
                                            BuyerRef = color_key.BuyerRef,
                                            ExeNo = color_key.ExeNo,
                                            FEONo = color_key.FEONo,
                                            ColorInfo = color_key.ColorInfo,
                                            Qty = color_grp.Sum(p => p.Qty),
                                            NoRoll = color_grp.Count(),
                                            Shrinkage = color_key.Shrinkage,
                                            Weight = color_key.Weight,
                                        }).ToList(),
                                        Construction = key.Construction,
                                        FabricNo = key.FabricNo,
                                        FabricWidth = key.FabricWidth,
                                        FabricWeave = key.FabricWeave,
                                        FinishType = key.FinishType,// grp.Select(x => x.FinishType).FirstOrDefault(),
                                        //Qty = grp.Sum(p => p.Qty),
                                        //NoRoll = grp.Count(),
                                        MUName = key.MUName,
                                        DOPriceType = key.DOPriceType,
                                        DOPriceTypeSt = key.DOPriceTypeSt,
                                        ProcessType = key.ProcessType,
                                        Weight = grp.Select(x => x.Weight).FirstOrDefault(),
                                        Shrinkage = grp.Select(x => x.Shrinkage).FirstOrDefault(),
                                    
                                    }).ToList();
            nTotalQty = _oFabricDeliveryOrder.FDODetails.Sum(x => x.Qty);
            if (_oFabricDeliveryOrder.FDODetails.Count > 0)
            {
                int nSL = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
                foreach (var oItem in oFDODetails)
                {
                    nSL++;

                    sTemp = "";
                   
                    if (!String.IsNullOrEmpty(oItem.FabricNo))
                    {
                        sTemp = "Article: " + oItem.FabricNo;
                    }
                    if (!String.IsNullOrEmpty(oItem.FabricWeave))
                    {
                        sTemp += "\nComp:" + oItem.ProductName + ", Weave: " + oItem.FabricWeave;
                    }
                    if (!string.IsNullOrEmpty(oItem.Construction))
                    {
                        sTemp += " \nConst: " + oItem.Construction;
                    }
                    if (!string.IsNullOrEmpty(oItem.FabricWidth))
                    {
                        sTemp += " \nWidth : " + oItem.FabricWidth;
                    }
                    if (!string.IsNullOrEmpty(oItem.Weight))
                    {
                        sTemp += " Weight : " + oItem.Weight;
                    }
                    if (!string.IsNullOrEmpty(oItem.ProcessType))
                    {
                        sTemp += "\nProcess: " + oItem.ProcessType;
                    }
                    if (!string.IsNullOrEmpty(oItem.FinishType))
                    {
                        sTemp += ", Finish: " + oItem.FinishType;
                    }
                    if (!string.IsNullOrEmpty(oItem.Shrinkage))
                    {
                        sTemp = sTemp + "\nShrinkage: " + oItem.Shrinkage;
                    }

                    if (oItem.DOPriceType == EnumDOPriceType.CutPiece)
                    {
                        sTemp = sTemp + "\n" + oItem.DOPriceTypeSt;
                    }

                    int nRowSpan = oItem.Color_Grp.Count();
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (nSL.ToString()).ToString(), nRowSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTemp, nRowSpan, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.Color_List.FirstOrDefault(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, oItem.NoRoll.ToString(), nRowSpan, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref _oPdfPTable, Global.MillionFormat(oItem.Qty), nRowSpan, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, oFontStyle);

                    //oItem.Color_Grp.RemoveAt(0);
                    foreach (var oColor in oItem.Color_Grp)
                    {
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oColor.StyleNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oColor.BuyerRef, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oColor.ColorInfo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oColor.FEONo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oColor.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, oColor.ExeNo, 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        //nNoRoll = nNoRoll + oColor.NoRoll;
                    }
                   
                    oPdfPTable.CompleteRow();

                    _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    _nUsagesHeight = _nUsagesHeight + CalculatePdfPTableHeight(oPdfPTable);
                    if (_nUsagesHeight > 700)
                    {
                      
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        _nUsagesHeight = 0;
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();

                        oPdfPTable.DeleteBodyRows();
                        _oPdfPTable.DeleteBodyRows();
                        //this.HeaderWithThreeFormats(_oExportCommercialDoc.DocHeader);
                        //this.ReportHeader(_oExportCommercialDoc.DocHeader);
                        //oPdfPTable1 = new PdfPTable(6);
                        //oPdfPTable1.SetWidths(new float[] { 30f, 180, 100f, 100f, 80f, 60f });
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                        _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

                      
                    }
                }
            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Total

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 20f, 135f, 80f, 70f, 80f, 70f, 70f, 15f, 50f });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

         
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            string sWord = Global.DollarWords(nTotalQty);
            if (!String.IsNullOrEmpty(sWord))
            {
                sWord = sWord.Replace("Dollar", "");
                sWord = sWord.Replace("Only", "");
                sWord = sWord.ToUpper();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("In Words: " + sWord + "" + _sMUnit, _oFontStyle));
            _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Remarks
            this.Blank(10);

            _oPdfPCell = new PdfPCell(this.Remarks()); _oPdfPCell.Border = 0; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Signature Part
            this.Blank(15);
            float nHeight = CalculatePdfPTableHeight(_oPdfPTable);
            while (nHeight < 580f)
            {
                this.Blank(15);
                nHeight = CalculatePdfPTableHeight(_oPdfPTable);
            }

            _oPdfPCell = new PdfPCell(this.SignaturePart());
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private PdfPTable ExportDOFirstRowLeft()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 400f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name.ToUpper(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPhrase = new Phrase();
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPhrase.Add(new Chunk("Buying House : ", _oFontStyle));
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.BuyerName, _oFontStyle));

            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.FactoryAddress))
            {
                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Factory : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.FactoryAddress, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.DeliveryToName))
            {
                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Garments Name : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.DeliveryToName, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.BorderWidthBottom = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Address : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.BuyerAddress, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.BorderWidthTop = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.BuyerCPName))
            {

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Concern Person : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.BuyerCPName, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.BorderWidthTop = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPhrase = new Phrase();
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPhrase.Add(new Chunk("Contact No : ", _oFontStyle));
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPhrase.Add(new Chunk(_oFabricDeliveryOrder.BuyerCPPhone, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.BorderWidthTop = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            return oPdfPTable;
        }
        private PdfPTable ExportDOFirstRowRight()
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 200f, 60f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("DO. NO. " + _oFabricDeliveryOrder.DONo + " DATE : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.DODateSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            int nLCID=0;
            foreach (ExportPI oItem in _oExportPIs)
            {
                if (oItem.LCID > 0 && nLCID!=oItem.LCID)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase("L/C NO : " + oItem.ExportLCNo + " DATE : ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDate.ToString("dd MMM yyyy"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();

                    if (oItem.AmendmentNo > 0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase("AMENDMENT NO : " + oItem.AmendmentNo + " DATE : ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Border = 0;
                        oPdfPTable.AddCell(_oPdfPCell);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDate.ToString("dd MMM yyyy"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Border = 0;
                        oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                    }

                    oPdfPTable.CompleteRow();
                }
                nLCID = oItem.LCID;
            }

            #region Export PI List
            if (_oExportPIs.Count > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                foreach (ExportPI oItem1 in _oExportPIs)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem1.PINo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem1.IssueDateInString, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
            }
            #endregion

            return oPdfPTable;
        }
        private PdfPTable Remarks()
        {
            int nCount=0;
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 28f, 420f });

            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.Note))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.Note, _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (!string.IsNullOrEmpty(_oFabricDeliveryOrder.BuyerCPName)) 
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1 | iTextSharp.text.Font.UNDERLINE);
                _oPdfPCell = new PdfPCell(new Phrase("Contact Person  ", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.BuyerCPName + (!string.IsNullOrEmpty(_oFabricDeliveryOrder.BuyerCPPhone) ? ", " + _oFabricDeliveryOrder.BuyerCPPhone : ""), _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 30f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            if (_oFabricDeliveryOrder.FDONotes.Any()) 
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1 | iTextSharp.text.Font.UNDERLINE);
                _oPdfPCell = new PdfPCell(new Phrase("Remarks: ", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();


                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                foreach (var oitem in _oFabricDeliveryOrder.FDONotes)
                {
                    _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString() + ". ", _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oitem.Note, _oFontStyle)); _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            
            return oPdfPTable;
        }
        #region Report Header
        private void ReporttHeader()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print
         
            sHeaderName = _oFabricDeliveryOrder.FDOTypeInSt.ToUpper()+" DELIVERY ORDER";
      
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oFabricDeliveryOrder.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Order", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
        }
        #endregion

        private PdfPTable SignaturePart()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 198f, 198f, 198f });

            #region Part

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("For " + _oBusinessUnit.Name, _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20f;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_oFabricDeliveryOrder.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                iTextSharp.text.Image oImag;
                oImag = iTextSharp.text.Image.GetInstance(_oFabricDeliveryOrder.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(90f, 50f);
                _oPdfPCell = new PdfPCell(oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.PreparedByName + " \n ________________________ \n Prepared By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.CheckedByName + " \n ________________________ \n Checked By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oFabricDeliveryOrder.ApproveByName + " \n ________________________ \n Authorized By", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase("DGM (Sales & Marketing)", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(_oPdfPCell);
            //oPdfPTable1.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        private void Blank(int nFixedHeight)
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 500f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
    }
}
