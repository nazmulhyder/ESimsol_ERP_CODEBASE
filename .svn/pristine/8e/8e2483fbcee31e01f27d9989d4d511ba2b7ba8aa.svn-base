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
    public class rptDynamicStickerPrint
    {
        #region Declaration

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<object> _oStickers = new List<object>();
        DocPrintEngine _oDocPrintEngine = new DocPrintEngine();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport_Dynamic_Sticker(List<object> oStickers, Company oCompany, DocPrintEngine oDocPrintEngine)
        {
            _oCompany = oCompany;
            _oStickers = oStickers;
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

            this.PrintDynamicTable(fPageSize[0], (int)fPageSize[2], fPageSize[3], fPageSize[4], oStickers);
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintDynamicTable(float fWidth, int nStickerCount, float column_padding, float row_padding, List<object> oStickers)
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
            foreach (var oHanger in _oStickers)
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
                        else if (oItem.TableName.ToUpper().Contains("@HEADER_WITH_BARCODE_FOR_NUM"))
                        {
                            if (sFieldDatas.Length == 2)
                            {
                                string sTitle = this.ConvertData(sFieldDatas[0], oHanger);
                                int nCFData = Convert.ToInt32(this.ConvertData(sFieldDatas[1], oHanger));
                                ESimSolPdfHelper.PrintHeader_Sticker_With_BarCodeForNum(ref oPdfPTable, _oCompany, sTitle, nCFData, 0);
                            }
                        }
                        else if (oItem.TableName.ToUpper().Contains("@HEADER_WITH_BARCODE_FOR_STRING"))
                        {
                            if (sFieldDatas.Length == 2)
                            {
                                string sTitle = this.ConvertData(sFieldDatas[0], oHanger);
                                string sCFData = this.ConvertData(sFieldDatas[1], oHanger);
                                ESimSolPdfHelper.PrintHeader_Sticker_With_BarCode(ref oPdfPTable, _oCompany, sTitle, sCFData, 0);
                            }
                        }
                        else if (oItem.TableName.Contains("@HEADER"))
                        {
                            foreach (var oFData in sFieldDatas)
                            {
                                ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, oFData, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
                            }
                        }
                        else if (oItem.TableName.Contains(","))
                        {
                            #region For Bold and Normal in a Row
                            string[] sFontStyles = this.getSplits(oItem.TableName, ',');
                            if (sFontStyles.Length == sFieldDatas.Length)
                            {
                                for (int i = 0; i < sFieldDatas.Length; i++)
                                {
                                    if (sFontStyles[i].Contains("@ISBOLD"))
                                    {
                                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                                        int nIndexOf = sFieldDatas.ToList().IndexOf(sFieldDatas[i]);
                                        var sCFData = this.ConvertData(sFieldDatas[i], oHanger);
                                        ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
                                    }
                                    else if (sFontStyles[i].Contains("@HASBORDER_DOTTED"))
                                    {
                                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);
                                        var sCFData = this.ConvertData(sFieldDatas[i], oHanger);
                                        iTextSharp.text.pdf.draw.DottedLineSeparator sep = new iTextSharp.text.pdf.draw.DottedLineSeparator();

                                        sep.Percentage = 100;
                                        sep.LineWidth = 1;
                                        sep.Gap = 2f;
                                        sep.Offset = -2;
                                        sep.Alignment = Element.ALIGN_BOTTOM;
                                        Phrase oPhrase = new Phrase(sCFData, ESimSolPdfHelper.FontStyle);
                                        PdfPCell oPdfPCell = new PdfPCell(oPhrase);

                                        var nIndexOf = sFieldDatas.ToList().IndexOf(sFieldDatas[i]) % 2;
                                        if (nIndexOf != 0 && nIndexOf != -1)
                                        {
                                            oPhrase.Add(sep);
                                        }
                                        ESimSolPdfHelper.AddCell(ref oPdfPTable, oPdfPCell, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, (int)oItem.RowHeight, 0);
                                    }
                                    else if (sFontStyles[i].Contains("@HASSQUAREBORDER"))
                                    {
                                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.NORMAL);

                                        int nIndexOf = sFieldDatas.ToList().IndexOf(sFieldDatas[i]);
                                        var sCFData = this.ConvertData(sFieldDatas[i], oHanger);
                                        ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 0, (int)oItem.RowHeight);
                                    }
                                    else if (sFontStyles[i].Contains("@SQUAREBORDERWITHBOLD"))
                                    {
                                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                                        int nIndexOf = sFieldDatas.ToList().IndexOf(sFieldDatas[i]);
                                        var sCFData = this.ConvertData(sFieldDatas[i], oHanger);
                                        ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 0, (int)oItem.RowHeight);
                                    }
                                    else if (sFontStyles[i].Contains("@HASUPDOWNBORDER"))
                                    {
                                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.NORMAL);

                                        int nIndexOf = sFieldDatas.ToList().IndexOf(sFieldDatas[i]);
                                        var sCFData = this.ConvertData(sFieldDatas[i], oHanger);
                                        ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 3, 0, 0, (int)oItem.RowHeight);
                                    }
                                    else if (sFontStyles[i].Contains("@UPDOWNBORDERWITHBOLD"))
                                    {
                                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                                        int nIndexOf = sFieldDatas.ToList().IndexOf(sFieldDatas[i]);
                                        var sCFData = this.ConvertData(sFieldDatas[i], oHanger);
                                        ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 3, 0, 0, (int)oItem.RowHeight);
                                    }
                                }
                            }

                            #endregion
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
                        else if (oItem.TableName.Contains("@HASSQUAREBORDER"))
                        {
                            foreach (var oFData in sFieldDatas)
                            {
                                ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.NORMAL);

                                int nIndexOf = sFieldDatas.ToList().IndexOf(oFData);
                                var sCFData = this.ConvertData(oFData, oHanger);
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 0, (int)oItem.RowHeight);
                            }
                        }
                        else if (oItem.TableName.Contains("@SQUAREBORDERWITHBOLD"))
                        {
                            foreach (var oFData in sFieldDatas)
                            {
                                ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                                int nIndexOf = sFieldDatas.ToList().IndexOf(oFData);
                                var sCFData = this.ConvertData(oFData, oHanger);
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 15, 0, 0, (int)oItem.RowHeight);
                            }
                        }
                        else if (oItem.TableName.Contains("@HASUPDOWNBORDER"))
                        {
                            foreach (var oFData in sFieldDatas)
                            {
                                ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.NORMAL);

                                int nIndexOf = sFieldDatas.ToList().IndexOf(oFData);
                                var sCFData = this.ConvertData(oFData, oHanger);
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 3, 0, 0, (int)oItem.RowHeight);
                            }
                        }
                        else if (oItem.TableName.Contains("@UPDOWNBORDERWITHBOLD"))
                        {
                            foreach (var oFData in sFieldDatas)
                            {
                                ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, nFonts[0], iTextSharp.text.Font.BOLD);

                                int nIndexOf = sFieldDatas.ToList().IndexOf(oFData);
                                var sCFData = this.ConvertData(oFData, oHanger);
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nIndexOf])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 3, 0, 0, (int)oItem.RowHeight);
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
                //if(nRowCount % nStickerCount == 0) 
                //{
                //    nRowCount = 0;

                //    oPdfPTable_Sticker.CompleteRow();
                //    ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0, 0, false);
                //    oPdfPTable_Main.CompleteRow();

                //    ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, nTotal_Column, 0, (int)row_padding);
                //    oPdfPTable_Main.CompleteRow();
                //}
                //else if (nRowCount % 2 == 0)
                //{
                //    ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0, 0);
                //    ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0, 0, false);
                //    ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0, 0);
                //}
                //else if (nRowCount == 1)
                //{
                //    ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0, 0, false); //ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0);
                //}
                //else 
                //{
                //    //ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0, 0);
                //    ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0, 0, false);
                //    ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0, 0);
                //}

                if (nRowCount % nStickerCount == 0)
                {
                    nRowCount = 0;
                    oPdfPTable_Sticker.CompleteRow();
                    ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0, 0, false); //--- WITH BORDER
                    //ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 0, 0, 0, 0, false);
                    oPdfPTable_Main.CompleteRow();

                    ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, nTotal_Column, 0, (int)row_padding);
                    oPdfPTable_Main.CompleteRow();
                }
                else
                {
                    ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 15, 0, 0, 0, false); //--- WITH BORDER
                    //ESimSolPdfHelper.AddTable(ref oPdfPTable_Main, oPdfPTable_Sticker, 0, 0, 0, 0, false); //--- WITH OUT BORDER
                    ESimSolPdfHelper.AddCell(ref oPdfPTable_Main, "   ", Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 0, 0, 0, 0, 0);
                }

            }

            oPdfPTable_Main.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_Main, 0, 0, 0);

            _oPdfPTable.CompleteRow();
        }

        #region Supprot Funtion
        private string ConvertData(string oFData, object osticker)
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
                        var oProperty = osticker.GetType().GetProperty(sPropertyName);
                        if (oProperty != null)
                        {
                            var Data = oProperty.GetValue(osticker, null);
                            sPropertyValue = Data.ToString();
                            if (Data != null && (Data.GetType() == typeof(double))) // || Data.GetType() == typeof(int)
                            {
                                //sPropertyValue = Global.MillionFormat_Round(Convert.ToDouble(Data));
                                sPropertyValue = Convert.ToDouble(Data).ToString("#,##0.00");
                            }
                            //sPropertyValue = Data.ToString();
                        }
                        #endregion
                        ESimSolPdfHelper.FindAndReplace(ref oFData, "@" + sPropertyName, sPropertyValue);
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
