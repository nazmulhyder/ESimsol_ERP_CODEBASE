using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICS.Core.Framework;
using System.Linq;
using System.Web;
namespace ESimSol.Reports
{
    public class rptExportBill_BuyerLetter
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        ExportBill _oExportBill = new ExportBill();
        DocPrintEngine _oDocPrintEngine = new DocPrintEngine();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        public byte[] PrepareReport(ExportBill oExportBill, Company oCompany,BusinessUnit oBusinessUnit, DocPrintEngine oDocPrintEngine)
        {
            _oExportBill = oExportBill;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oDocPrintEngine = oDocPrintEngine;

            #region Page Setup

            float[] fPageSize = this.getSplits(_oDocPrintEngine.PageSize, ',', 2);
            float[] fMargin = this.getSplits(_oDocPrintEngine.Margin, ',', 4);

            _oDocument = new Document(new iTextSharp.text.Rectangle(fPageSize[0], fPageSize[1]), fMargin[0], fMargin[1], fMargin[2], fMargin[3]);
            _oPdfPTable.SetWidths(new float[] { fPageSize[0] });
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            #endregion

            this.PrintTable();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Print Body
        private void PrintTable()
        {
            foreach (var oItem in _oDocPrintEngine.DocPrintEngineDetails)
            {
                float[] nColumnWidths = this.getSplits(oItem.SetWidths, ',', oItem.SetWidths.Split(',').Count());
                float[] nFonts = this.getSplits(oItem.FontSize, ',', oItem.FontSize.Split(',').Count());
                string[] sAligns = this.getSplits(oItem.SetAligns, ',');
                string[] sFieldDatas = this.getSplits(oItem.SetFields, '#');

                PdfPTable oPdfPTable = new PdfPTable(nColumnWidths.Length);
                oPdfPTable.SetWidths(nColumnWidths);
                oPdfPTable.DefaultCell.Border = 0;

                float nTableRowHeight = 10f;

                if (!string.IsNullOrEmpty(oItem.TableName) && !oItem.TableName.Contains("@ISNULL"))
                {
                    if (oItem.TableName.ToUpper().Contains("@COMPANYHEADER_BU"))
                    {
                        ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oBusinessUnit, _oCompany, oItem.SetFields, 1);
                    }
                    else if (oItem.TableName.ToUpper().Contains("@COMPANYHEADER"))
                    {
                        ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oCompany, oItem.SetFields, 1);
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

                            var sCFData = this.ConvertData(oFData);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0);
                        }
                    }
                }
                else if (oItem.SetFields.Contains("@ISNULL") || string.IsNullOrEmpty(oItem.SetFields))
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, nColumnWidths.Length, (int)oItem.RowHeight,0);
                }
                else
                {
                    #region PRINT FIELD DATA
                    int nCount = 0;
                    foreach (var oFData in sFieldDatas)
                    {
                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, (nFonts.Length != nColumnWidths.Length ? nFonts[0] : nFonts[nCount]), iTextSharp.text.Font.NORMAL);

                        var sCFData = this.ConvertData(oFData);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData,
                            (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nCount])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, (int)oItem.RowHeight);
                        nCount++;
                    }
                    #endregion
                }
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 0);
            }
        }
        #endregion

        #region Supprot Funtion
        private string ConvertData(string oFData)
        {
            if (oFData.Contains("@"))
            {
                double TotalUP_Ampunt = _oExportBill.ExportBillDetails.Sum(x => x.Amount);// +_oExportBill.Amount_Tips;
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@CURRENTDATE", DateTime.Now.ToString("dd MMM yyyy"));

                ESimSolPdfHelper.FindAndReplace(ref oFData, "@SendToPartyDate", _oExportBill.SendToPartySt);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@SendToParty", _oExportBill.SendToPartySt);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BILLAMOUNT", _oExportBill.Currency+" "+ _oExportBill.AmountSt); // Global.TakaFormat( + " (" + Global.TakaWords(_oExportBill.Amount_Tips) + ")");
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@ExportBillNo", _oExportBill.ExportBillNoSt);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BillNo", _oExportBill.ExportBillNoSt);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BillQty", Global.MillionFormat(_oExportBill.Qty)); //+ " "+ _oExportBill.MUnit);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BillDate", _oExportBill.AcceptanceDateStr);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@PINo", _oExportBill.PINo);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@ExportLCNo", _oExportBill.ExportLCNo);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@LCTerms", _oExportBill.LCTermsName);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@LCDate", _oExportBill.LCOpeningDatest);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@PartyName", _oExportBill.ApplicantName);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@PartyAddress", _oExportBill.ApplicantAddress);
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
                else throw new Exception("Sorry, String is Not In Correct Format!!");
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
