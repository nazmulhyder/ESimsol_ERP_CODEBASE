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
    public class rptBTMA_Doc
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        BTMA _oBTMA = new BTMA();
        DocPrintEngine _oDocPrintEngine = new DocPrintEngine();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(BTMA oBTMA, Company oCompany, DocPrintEngine oDocPrintEngine)
        {
            _oBTMA = oBTMA;
            _oCompany = oCompany;
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

                if (!string.IsNullOrEmpty(oItem.TableName) && !oItem.TableName.Contains("@ISNULL") ) 
                {
                    #region PRINT TABLE
                    if ((_oBTMA.BUType == (int)EnumBusinessUnitType.Weaving || _oBTMA.BUType == (int)EnumBusinessUnitType.Finishing) && oItem.TableName.Contains("@FABRIC")) 
                    {
                        if (oItem.TableName.Split(',').Count() > 1)
                            nTableRowHeight = (float)Convert.ToDouble(oItem.TableName.Split(',')[1]);
               
                        PrintDetailTable(ref oPdfPTable, nFonts, sAligns, sFieldDatas, (float)oItem.RowHeight, nTableRowHeight);
                        continue;
                    }
                    else if (!(_oBTMA.BUType == (int)EnumBusinessUnitType.Weaving || _oBTMA.BUType == (int)EnumBusinessUnitType.Finishing) && oItem.TableName.Contains("@YARN"))
                    {
                        if (oItem.TableName.Split(',').Count() > 1)
                            nTableRowHeight = (float)Convert.ToDouble(oItem.TableName.Split(',')[1]);

                        PrintDetailTable(ref oPdfPTable, nFonts, sAligns, sFieldDatas, (float)oItem.RowHeight, nTableRowHeight);
                        continue;
                    }
                    else if (!(_oBTMA.BUType == (int)EnumBusinessUnitType.Weaving || _oBTMA.BUType == (int)EnumBusinessUnitType.Finishing) && oItem.TableName.Contains("IF_YARN")) 
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, nColumnWidths.Length, 0, (float)oItem.RowHeight);

                        oPdfPTable.CompleteRow();
                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 0); continue;
                    }
                    else if ((_oBTMA.BUType == (int)EnumBusinessUnitType.Weaving || _oBTMA.BUType == (int)EnumBusinessUnitType.Finishing) && oItem.TableName.Contains("IF_FABRIC"))
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, nColumnWidths.Length, 0, (float)oItem.RowHeight);

                        oPdfPTable.CompleteRow();
                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 0); continue;
                    }
                    else if ( oItem.TableName.Contains("IF_FABRIC") ||  oItem.TableName.Contains("IF_YARN"))
                    { 
                        /*DO NOTHING*/ 
                    }
                    else
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, nColumnWidths.Length, 0, (float)oItem.RowHeight);

                        oPdfPTable.CompleteRow();
                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 0); continue;
                    }
                    #endregion
                }
               
                if (oItem.SetFields.Contains("@ISNULL") || string.IsNullOrEmpty(oItem.SetFields))
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, nColumnWidths.Length, oItem.RowHeight);
                }
                else
                {
                    #region PRINT FIELD DATA
                    int nCount = 0;
                    foreach (var oFData in sFieldDatas)
                    {
                        ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, (nFonts.Length != nColumnWidths.Length? nFonts[0]:nFonts[nCount]) , iTextSharp.text.Font.NORMAL);

                        var sCFData = this.ConvertData(oFData);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData,
                            (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[nCount])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, oItem.RowHeight);
                        nCount++;
                    }
                    #endregion
                }
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 0);
            }
        }
        private void PrintDetailTable(ref PdfPTable oPdfPTable, float[] nFonts, string[] sAligns, string[] sFieldDatas, float TableHeight, float RowHeight)
        {
            int nCount = 0;
            int tableMaxColumn = 6;
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
       
            foreach (var oItem in _oBTMA.BTMADetails)
            {
                int i = 0;
                foreach (var oFData in sFieldDatas)
                {
                    var sCFData = this.ConvertData(oFData, oItem);
                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont(_oDocPrintEngine.FontName, (nFonts.Length != sFieldDatas.Length ? nFonts[0] : nFonts[i]), iTextSharp.text.Font.NORMAL);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, sCFData, (sFieldDatas.Length != sAligns.Length ? Element.ALIGN_LEFT : GetAlgin(sAligns[i])), Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, RowHeight);
                    i++;
                }
                nCount++;
            }

            while (nCount < tableMaxColumn)
            {
                nCount++;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, sFieldDatas.Length, RowHeight);
            }

            if (TableHeight > tableMaxColumn * RowHeight)
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, sFieldDatas.Length, (TableHeight - tableMaxColumn * RowHeight));
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable, 0, 0, 0);
        }
        #endregion

        #region Supprot Funtion
        private string ConvertData(string oFData)
        {
            if(oFData.Contains("@"))
            {
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@MLCNO", _oBTMA.MasterLCNos);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@MLCDATE", _oBTMA.MasterLCDates);
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@MLCDATE", _oBTMA.MushakDateST);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@GARMENTSQTY", _oBTMA.GarmentsQty);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@YLCDATE", (_oBTMA.BUType == (int)EnumBusinessUnitType.Dyeing || _oBTMA.BUType == (int)EnumBusinessUnitType.Spinning) ? _oBTMA.LCDateST : "");
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@YLCVALUE", (_oBTMA.TextileUnit == (int)EnumBusinessUnitType.Dyeing || _oBTMA.TextileUnit == (int)EnumBusinessUnitType.Spinning) ? _oBTMA.LCDateST : "");
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@FLCDATE", (_oBTMA.BUType == (int)EnumBusinessUnitType.Weaving || _oBTMA.BUType == (int)EnumBusinessUnitType.Finishing) ? _oBTMA.LCDateST : "");
                //ESimSolPdfHelper.FindAndReplace(ref oFData, "@FLCVALUE", (_oBTMA.TextileUnit==(int)EnumBusinessUnitType.Weaving || _oBTMA.TextileUnit==(int)EnumBusinessUnitType.Finishing)?_oBTMA.LCDateST:"");
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@GARMENTSQTY", _oBTMA.GarmentsQty);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@AMOUNTLC", (_oBTMA.Amount <= 0 ? "" : Global.MillionFormat(_oBTMA.Amount)));
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@AMOUNTBILL", (_oBTMA.Amount_Bill <= 0 ? "" : Global.MillionFormat(_oBTMA.Amount_Bill)));
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@IMPORTLCNO", _oBTMA.ImportLCNo);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@IMPORTLCDATE", _oBTMA.ImportLCDateST);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@AMOUNTIMPORTLC", (_oBTMA.Amount_ImportLC<=0?"": Global.MillionFormat(_oBTMA.Amount_ImportLC)) );
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BANK", _oBTMA.BankName);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BRANCH", _oBTMA.BranchName);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@EXPIRYDATE", _oBTMA.LCExpireDateST);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@MUSHAKNO", _oBTMA.MushakNo);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@MUSHAKDATE", _oBTMA.MushakDateST);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@SUPPLIERNAME", _oBTMA.SupplierName);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@SUPPLIERADDRESS", _oBTMA.SupplierAddress);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@GATEPASSNO", _oBTMA.GatePassNo);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@GATEPASSDATE", _oBTMA.GatePassDateST);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@TEXTILEUNIT", _oBTMA.BUName);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BUNAME", _oBTMA.BUName);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BUADDRESS", _oBTMA.BUAddress);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@EXPORTLCNO", _oBTMA.ExportLCNo);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@LCDATE", _oBTMA.LCDateST);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@PARTYNAME", _oBTMA.PartyName);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@PARTYADDRESS", _oBTMA.PartyAddress);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@CURRENCY", _oBTMA.Currency);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@CURRENCYNAME", _oBTMA.CurrencyName);
                //DeliveryChallanNo = "";
            }
            return oFData;
        }
        private string ConvertData(string oFData, BTMADetail oBTMADetail)
        {
            if (oFData.Contains("@"))
            {
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@PRODUCTNAME", oBTMADetail.ProductName);
           
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@QTYTWO", Global.MillionFormat_Round(oBTMADetail.QtyTwo));
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@QTY", Global.MillionFormat_Round(oBTMADetail.Qty));
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@UNITPRICE",  Global.MillionFormat_Round(oBTMADetail.UnitPrice));
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@DELIVERYDATE", _oBTMA.DeliveryDateST);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@INVOICEDATE", _oBTMA.InvoiceDateST);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@BILLNO", _oBTMA.ExportBillNo);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@PINO", oBTMADetail.PINo);
            
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@UNITTWO", oBTMADetail.MUNameTwo);
                ESimSolPdfHelper.FindAndReplace(ref oFData, "@UNIT", oBTMADetail.MUName);
                
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
            float [] nResult = new float[nLength];
            
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
