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
    public class rptHangerSticker
    {
        #region Declaration

        int _nTotalColumn = 3;

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<HangerSticker> _oHangerStickers = new List<HangerSticker>();
        DocPrintEngine _oDocPrintEngine = new DocPrintEngine();
        //List<FabricExecutionOrderDetail> _oFEODs = new List<FabricExecutionOrderDetail>();
        Company _oCompany = new Company(); 
        #endregion

        public byte[] PrepareReport(List<HangerSticker> oHangerStickers)
        {
            _oHangerStickers = oHangerStickers;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(633.13f, 942.50f).Rotate(), 35.97f, 133.10f, 35.97f, 35.97f); //A4 Size Paper = height:842   width:595 that means 1 Inch = 71.94679564691657 pixel
            _oDocument.SetMargins(35.97f, 133.10f, 35.97f, 35.97f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            oFontStyle = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.BOLD);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 250f, 10f, 250f, 10f, 250f });
            #endregion

            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
       
        private PdfPTable MakeSingleSticker(HangerSticker oHangerSticker)
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 100f, 150f });
            PdfPCell oPdfPCell;


            #region Code
            oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.ART, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Supplier
            oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.Supplier, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Origin
            oPdfPCell = new PdfPCell(new Phrase("Origin", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Bangladesh", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Content
            oPdfPCell = new PdfPCell(new Phrase("Content", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.Composition, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Construction
            oPdfPCell = new PdfPCell(new Phrase("Construction", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;            
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.Construction, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;            
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Width(Inch)
            oPdfPCell = new PdfPCell(new Phrase("Width(Inch)", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.Width, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Weight(G/M2)
            oPdfPCell = new PdfPCell(new Phrase("Weight(G/M2)", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.Remarks, _oFontStyle)); //Remarks use as Weight
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Finishing
            oPdfPCell = new PdfPCell(new Phrase("Finishing", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.Finishing, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Price
            oPdfPCell = new PdfPCell(new Phrase("Price(based on 10,000Y)", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oHangerSticker.Price) + " " + "USD/Y", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region S/Y Leadtime
            oPdfPCell = new PdfPCell(new Phrase("S/Y Leadtime", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("14-18 days", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Bulk Leadtime
            oPdfPCell = new PdfPCell(new Phrase("Bulk Leadtime", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("28-42 days", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region MOQ for Bulk
            oPdfPCell = new PdfPCell(new Phrase("MOQ for Bulk", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.MOQ, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Date
            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(oHangerSticker.Date, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Date
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("EsimSol Generated", oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Border = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            return oPdfPTable;
        }

        #region Report Body
        private void PrintBody()
        {
            if (_oHangerStickers.Count > 0)
            {                
                for (int index = 0; index < _oHangerStickers.Count; index = index + 3)
                {
                    #region Striker Data
                    if (index < _oHangerStickers.Count)
                    {
                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oHangerStickers[index]));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
                    }
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 179.87f;
                    _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 179.87f;
                    _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    if ((index + 1) < _oHangerStickers.Count)
                    {
                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oHangerStickers[index + 1]));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
                    }
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 179.87f;
                    _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 179.87f;
                    _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);


                    if ((index + 2) < _oHangerStickers.Count)
                    {
                        _oPdfPCell = new PdfPCell(MakeSingleSticker(_oHangerStickers[index + 2]));
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
                    }
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.FixedHeight = 179.87f;
                    _oPdfPCell.Border = 0;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Blank Row
                    if ((index + 3) % 9 != 0)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.Colspan = 5;
                        _oPdfPCell.FixedHeight = 8f;
                        _oPdfPCell.Border = 0;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                    #endregion
                }
            }
        }
        #endregion

        public byte[] PrepareReport_Dynamic_Sticker(List<HangerSticker> oHangerStickers, Company oCompany, DocPrintEngine oDocPrintEngine)
        {
            _oCompany = oCompany;
            _oHangerStickers = oHangerStickers;
            _oDocPrintEngine = oDocPrintEngine;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(300, 560), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(5f, 5f, 10f, 10f);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable = new PdfPTable(1);

            float[] fPageSize = this.getSplits(_oDocPrintEngine.PageSize, ',', 5);
            float[] fMargin = this.getSplits(_oDocPrintEngine.Margin, ',', 4);

            _oDocument = new Document(new iTextSharp.text.Rectangle(fPageSize[0], fPageSize[1]), fMargin[0], fMargin[1], fMargin[2], fMargin[3]);
            _oPdfPTable.SetWidths(new float[] { fPageSize[0] });
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            #endregion

            this.PrintDynamicTable(fPageSize[0], (int)fPageSize[2], fPageSize[3], fPageSize[4], oHangerStickers);
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintDynamicTable(float fWidth, int nStickerCount, float column_padding, float row_padding, List<HangerSticker> oHangerStickers)
        {
            PdfPTable oPdfPTable_Main = new PdfPTable(1);
            oPdfPTable_Main.SetWidths(new float[] { fWidth });

            float stickerWidth = fWidth;
            int nTotal_Column = (nStickerCount * 2 - 1);

            #region SET MAIN TABLE WIDTHS
            if (nStickerCount > 0) 
            {
                float[] SetWidths_Main = new float[nTotal_Column];
                oPdfPTable_Main = new PdfPTable(nTotal_Column);

                stickerWidth = (fWidth - ((nStickerCount - 1) * column_padding) / nStickerCount);

                for (int i = 0; i < nTotal_Column; i++) 
                {
                    if (i % 2 == 0) SetWidths_Main[i] = stickerWidth;
                    else SetWidths_Main[i] = column_padding;
                }
                oPdfPTable_Main.SetWidths(SetWidths_Main);
            }

            oPdfPTable_Main.DefaultCell.Border = 0;
            #endregion

            int nRowCount = 0;
            foreach (var oHanger in _oHangerStickers)
            {
                PdfPTable oPdfPTable_Sticker = new PdfPTable(1);
                oPdfPTable_Sticker.SetWidths(new float[] { stickerWidth });

                #region PRINT DOC PRINT ENGINE
                foreach (var oItem in _oDocPrintEngine.DocPrintEngineDetails)
                {
                    float[] nColumnWidths = this.getSplits(oItem.SetWidths, ',', oItem.SetWidths.Split(',').Count());
                    float[] nFonts = this.getSplits(oItem.FontSize, ',', oItem.FontSize.Split(',').Count());
                    string[] sAligns = this.getSplits(oItem.SetAligns, ',');
                    string[] sFieldDatas = this.getSplits(oItem.SetFields, '#');

                    PdfPTable oPdfPTable = new PdfPTable(nColumnWidths.Length);
                    oPdfPTable.SetWidths(nColumnWidths);
                    oPdfPTable.DefaultCell.Border = 0;

                    if (!string.IsNullOrEmpty(oItem.TableName) && !oItem.TableName.Contains("@ISNULL"))
                    {
                        if (oItem.TableName.ToUpper().Contains("@COMPANYHEADER"))
                        {
                            ESimSolPdfHelper.PrintHeader_Sticker(ref oPdfPTable, _oCompany, oItem.SetFields, 0);
                        }
                        else if (oItem.TableName.Contains("@HEADER"))
                        {
                            foreach (var oFData in sFieldDatas)
                            {
                                ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, oFData, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
                            }
                        }
                        else if (oItem.TableName.Contains("@ISBOLD"))
                        {
                            foreach (var oFData in sFieldDatas)
                            {
                                ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                                int nIndexOf = sFieldDatas.ToList().IndexOf(oFData);
                                var sCFData = this.ConvertData(oFData, oHanger);
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
                            }
                        }
                        else if (oItem.TableName.Contains("@HASBORDER_DOTTED"))
                        {
                            foreach (var oFData in sFieldDatas)
                            {
                                ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                                var sCFData = this.ConvertData(oFData, oHanger);

                                iTextSharp.text.pdf.draw.DottedLineSeparator sep = new iTextSharp.text.pdf.draw.DottedLineSeparator();

                                sep.Percentage = 100;
                                sep.LineWidth = 1;
                                sep.Gap = 2f;
                                sep.Offset = -2;
                                sep.Alignment = Element.ALIGN_BOTTOM;
                                Phrase oPhrase = new Phrase(sCFData, ESimSolPdfHelper.FontStyle);

                                PdfPCell oPdfPCell = new PdfPCell(oPhrase);

                                var nIndexOf = sFieldDatas.ToList().IndexOf(oFData) % 2;
                                if (nIndexOf != 0 && nIndexOf != -1)
                                {
                                    oPhrase.Add(sep);
                                }

                                ESimSolPdfHelper.AddCell(ref oPdfPTable, oPdfPCell, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, (int)oItem.RowHeight, 0);
                            }
                        }
                    }
                    else
                    {
                        #region PRINT FIELD DATA
                        int nCount = 0;
                        foreach (var oFData in sFieldDatas)
                        {
                            ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, (nFonts.Length != nColumnWidths.Length ? nFonts[0] : nFonts[nCount]), iTextSharp.text.Font.NORMAL);

                            var sCFData = this.ConvertData(oFData, oHanger);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData,
                                (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nCount])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, (int)oItem.RowHeight);
                            nCount++;
                        }
                        #endregion
                    }
                    oPdfPTable.CompleteRow();
                    ESimSolPdfHelper.AddTable(ref oPdfPTable_Sticker, oPdfPTable, 0, 0, 0);
                }
                #endregion

                nRowCount++;
                if (nRowCount % nStickerCount == 0)
                {
                    nRowCount = 0;

                    oPdfPTable_Sticker.CompleteRow();
                    //ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0, 0, false); //--- WITH BORDER
                    ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 0, 0, 0, 0, false);
                    oPdfPTable_Main.CompleteRow();

                    ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, nTotal_Column, 0, (int)row_padding);
                    oPdfPTable_Main.CompleteRow();
                }
                else
                {
                    //ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0, 0, false); //--- WITH BORDER
                    ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 0, 0, 0, 0, false); //--- WITH OUT BORDER
                    ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0, 0);
                }
            }

            oPdfPTable_Main.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_Main, 0, 0, 0);

            _oPdfPTable.CompleteRow();
        }

        #region Supprot Funtion
        private string ConvertData(string oFData, HangerSticker oHanger)
        {
            if (oFData.Contains("@"))
            {
                foreach (string oItem in oFData.Split(' '))
                {
                    if (oItem.Contains("@"))
                    {
                        string sPropertyValue = "";
                        string sPropertyName = oItem.Replace("@", "");

                        #region ======= GET PROPERTY VALUE ====== 
                        var oProperty = oHanger.GetType().GetProperty(sPropertyName);
                        if (oProperty != null)
                        {
                            var Data = oProperty.GetValue(oHanger, null);
                            if (Data != null && (Data.GetType() == typeof(double))) // || Data.GetType() == typeof(int)
                            {
                                sPropertyValue = Global.MillionFormat_Round(Convert.ToDouble(Data));
                            }
                            else if (Data != null)
                                sPropertyValue = Data.ToString();
                        }
                        #endregion
                        ESimSolPdfHelper.FindAndReplace1(ref oFData, "@" + sPropertyName, sPropertyValue);
                    }
                }
            }
            return oFData;
        }
        private int GetAlgin(string sAlign)
        {
            switch (sAlign.ToUpper())
            {
                case "LEFT":
                    return Element.ALIGN_LEFT;
                case "RIGHT":
                    return Element.ALIGN_RIGHT;
                case "TOP":
                    return Element.ALIGN_TOP;
                case "CENTER":
                    return Element.ALIGN_CENTER;
                default:
                    return Element.ALIGN_LEFT;
            }
        }
        private float[] getSplits(string sString, char nCH, int nLength)
        {
            if (string.IsNullOrEmpty(sString)) return null;

            int nCount = 0;
            float[] nResult = new float[nLength];

            if (nLength > 0)
            {
                if (sString.Split(nCH).Length == nLength)
                {
                    foreach (string oitem in sString.Split(nCH))
                    {
                        nResult[nCount++] = (float)Convert.ToDouble(oitem);
                    }
                }
                else throw new Exception("Sorry, String is Not In Correct Format [width, height, count, column padding, row padding]!!");
            }
            return nResult;
        }
        private string[] getSplits(string sString, char nCH)
        {
            int nCount = 0;
            string[] nResult = new string[sString.Split(nCH).Length];

            foreach (string oitem in sString.Split(nCH))
            {
                nResult[nCount++] = oitem;
            }
            return nResult;
        }
        #endregion
    }
}
