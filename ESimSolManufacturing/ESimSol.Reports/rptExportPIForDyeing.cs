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
using System.Linq;

namespace ESimSol.Reports
{
    public class rptExportPIForDyeing
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        int _nTotalColumn = 1;

        PdfPTable _oPdfPTable = new PdfPTable(1);
       
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        bool _bIsInKg = true;
        bool _bIsInYard = true;
        int _nTitleTypeInImg = 0;
        float _nUsagesHeight = 0;
        string _sWaterMark = "";
        ExportPI _oExportPI = new ExportPI();
        List<ExportPIDetail> _oExportPIDetails = new List<ExportPIDetail>();
        ExportPIPrintSetup _oExportPIPrintSetup = new ExportPIPrintSetup();
        List<ExportPITandCClause> _oExportPITandCClauses = new List<ExportPITandCClause>();
        BankBranch _oBankBranch = new BankBranch();
        Phrase _oPhrase = new Phrase();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

        public byte[] PrepareReport(ExportPI oExportPI, Company oCompany, bool bPrintFormat, int nTitleTypeInImg, BusinessUnit oBusinessUnit)
        {

            _oBusinessUnit = oBusinessUnit;
            _oExportPI = oExportPI;
            _oExportPIDetails = oExportPI.ExportPIDetails;
            _oExportPITandCClauses = oExportPI.ExportPITandCClauses;
            _oExportPIPrintSetup = oExportPI.ExportPIPrintSetup;
            _oCompany = oCompany;
            _bIsInKg = bPrintFormat;
            _nTitleTypeInImg = nTitleTypeInImg;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(40f, 30f, 30f, 3f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    525f //Articale
                                              });
            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.WaterMark(40f, 30f, 30f, 3f);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            if (_nTitleTypeInImg == 1)//normal
            {
                _oPdfPCell = new PdfPCell(this.PrintHeader_Common());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthBottom = 0; 
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else if (_nTitleTypeInImg == 2)//pad
            {
                PrintHeader_Blank();
            }
            else if (_nTitleTypeInImg == 3)//imge
            {
                _oPdfPCell = new PdfPCell(this.LoadCompanyTitle());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            if (_oExportPI.PaymentType == EnumPIPaymentType.NonLC)
            {
                _oPdfPCell = new PdfPCell(new Phrase("INVOICE", _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("PROFORMA INVOICE", _oFontStyle));
            }
            //_oPdfPCell = new PdfPCell(new Phrase("PROFORMA INVOICE", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop =1f; _oPdfPCell.FixedHeight = 25; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintHeader_Blank()
        {
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.FixedHeight = 100f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {            
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region PI No & Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("P.I.No : " + _oExportPI.PINo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oExportPI.IssueDate.ToString("dd MMMM, yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region Buyer & PI Information
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("APPLICANT/BUYER NAME:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BENEFICIARY:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oExportPI.DeliveryToID > 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase("Master Buyer: " + _oExportPI.DeliveryToName, _oFontStyle));
                 _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Detail Column Header
            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 20f, 150f, 75f, 65f, 60f, 62f, 60f, 62f, 74f });
            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DYED YARN FOR 100% EXPORT ORIENTED FACTORY", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        
            _oPdfPCell = new PdfPCell(new Phrase("POUND", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("UNIT PRICE(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("KILOGRAM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION OF GOODS", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("QUALITY", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("H.S.CODE", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("QTY(" + (_bIsInKg == false ? "KG" : "LBS") + ")", _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase("QTY", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. PRICE(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("QTY", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. PRICE(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("AMOUNT(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Details Data
            string sTemp = ""; sMUName = "";          
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            foreach (ExportPIDetail oItem in _oExportPIDetails)
            {
                nCount++;
                oPdfPTable = new PdfPTable(9);
                oPdfPTable.SetWidths(new float[] { 20f, 150f, 75f, 65f, 60f, 62f, 60f, 62f, 74f });
                                
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(oItem.ProductName, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));
                if (oItem.ShadeType != EnumDepthOfShade.None)
                {
                    _oPhrase.Add(new Chunk(" [" + oItem.ShadeType + " Shade]", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                }
                if (!String.IsNullOrEmpty(oItem.HSCode))
                {
                    sTemp =sTemp+ "\nH.S.CODE: " + oItem.HSCode;
                }
                if (oItem.PackingType>0)
                {
                    sTemp = sTemp + "\nPacking: " + (EumDyeingType)oItem.PackingType;
                }
                if (oItem.DyeingType > 0)
                {
                    sTemp = sTemp + "\nDyeing Process: " + (EumDyeingType)oItem.DyeingType+" Dyeing";
                }
               
                if (!String.IsNullOrEmpty(oItem.ColorInfo))
                {
                    sTemp = sTemp + "\nColor: " + oItem.ColorInfo;
                }
                if (!String.IsNullOrEmpty(oItem.StyleNo))
                {
                    sTemp = sTemp + "\nStyle No: " + oItem.StyleNo;
                }
                if (!String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    sTemp = sTemp + " Buyer Ref: " + oItem.BuyerReference;
                }
                if (!String.IsNullOrEmpty(oItem.ExportQuality))
                {
                    sTemp = sTemp + ", Quality:" + oItem.ExportQuality;
                }
                if (oItem.SaleType == EnumProductionType.Commissioning)
                {
                    sTemp = sTemp + "\n " + "Dyeing Charge";
                }
                _oPhrase.Add(new Chunk("" + sTemp, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
               // _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + " [Dark Shade]", _oFontStyle));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //if (!String.IsNullOrEmpty(oItem.ExportQuality))
                //{
                //    sTemp = "" + oItem.ExportQuality;
                //}
                //_oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //if (!String.IsNullOrEmpty(oItem.HSCode))
                //{
                //    sTemp = "" + oItem.HSCode;
                //}
                //_oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                sTemp = "";
                //oItem.Qty = (_bIsInKg == false ? Global.GetKG(oItem.Qty, 10) : oItem.Qty);
                nTotalQty = nTotalQty + oItem.Qty;
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //oItem.UnitPrice = (_bIsInKg == false ? Global.GetLBS(oItem.UnitPrice, 10) : oItem.UnitPrice);                

                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Global.GetKG(oItem.Qty, 10)), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //oItem.UnitPrice = (_bIsInKg == false ? Global.GetLBS(oItem.UnitPrice, 10) : oItem.UnitPrice);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency+""+System.Math.Round(Global.GetLBS(oItem.UnitPrice, 10), 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((oItem.Qty * oItem.UnitPrice), 2), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice;

                sMUName = oItem.MUName;
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 20f, 150f, 75f, 65f, 60f, 62f, 60f, 62f, 74f });

        

            #region Total Summary
            _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle)); //sMUName
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

    
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((Global.GetKG(nTotalQty, 10))), _oFontStyle)); //sMUName
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + Global.MillionFormat(System.Math.Round(System.Math.Round(nTotalValue, 2), 2)), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word : US " + Global.DollarWords(nTotalValue), _oFontStyle));
            }
            _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Transport Mode, Place of Shipment & Place of Destination
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 186.66f, 186.66f, 186.67f, 186.67f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT TERM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF SHIPMENT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MODE OF TRANSPORT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF DESTINATION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ShipmentTermSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfShipment, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.ShipmentBy, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfDelivery, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Terms & Conditions Data :
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetTermsAndConditionTable()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ADVISING BANK
            #region ADVISING BANK
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("ADVISING BANK:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Bank Data
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetAdviseBankTable()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Authorized Signature
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 2f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.AcceptanceBy, _oFontStyle));
            _oPdfPCell.FixedHeight = 60f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.For, _oFontStyle));
            _oPdfPCell.FixedHeight = 60f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 60f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_oExportPI.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oExportPI.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(80f, 50f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.ExportPIPrintSetup.AcceptanceBy == "" || _oExportPI.ExportPIPrintSetup.AcceptanceBy == "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();



            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            this.Note();
        }

        #endregion

        #region Function
        private PdfPTable GetTermsAndConditionTable_Caption()
        {
            PdfPTable oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);
            int i = 0;
           
            foreach (ExportPITandCClause oItem in _oExportPITandCClauses)
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString("00") + ".", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CaptionName, _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("" + oItem.TermsAndCondition, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                oTermsAndConditionTable.CompleteRow();
            }

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            /// Total Hight -Margian -Uses Height
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 700 - 40 - 30 - _nUsagesHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            return oTermsAndConditionTable;
        }
        private PdfPTable GetTermsAndConditionTable()
        {
            PdfPTable oTermsAndConditionTable = new PdfPTable(7);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                100f, //Style Name 
                                                                40f,  // Dept
                                                                145f, //Description/Composition 
                                                                50f,  //Shipment Date
                                                                50f,  //Quantity
                                                                40f,  //Unit Price
                                                                50f,  //Amount 
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int i = 0;
            string sTerms = "";
            if (_oExportPI.LCTermID > 0)
            {
                sTerms = "Terms Of L/C: EX Factory by an Irrevocable Letter of credit " + _oExportPI.LCTermsName + " from the Date Of Acceptance";
            }
            if (!string.IsNullOrEmpty(sTerms))
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString() + ") " + sTerms, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);
                oTermsAndConditionTable.CompleteRow();
            }
            sTerms = "";
            if (_oExportPI.IsBBankFDD)
            {
                sTerms = sTerms + "Terms Of payment should be done in US Dollar by Bangladesh Bank FDD.";
            }

            if (!string.IsNullOrEmpty(sTerms))
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString() + ") " + sTerms, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);
                oTermsAndConditionTable.CompleteRow();
            }
            sTerms = "";
            if (_oExportPI.IsLIBORRate)
            {
                sTerms = "Interest has to be paid at LIBOR for Usance Period.";
                if (_oExportPI.OverdueRate > 0)
                {
                    sTerms = sTerms + " and ";
                }
                else
                {
                    sTerms = sTerms + ".";
                }

            }
            if (_oExportPI.OverdueRate > 0)
            {
                sTerms = sTerms + " Overdue interest has to be paid at @" + _oExportPI.OverdueRate.ToString() + " % P.A. for overdue period.";
            }
            if (!string.IsNullOrEmpty(sTerms))
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString() + ") " + sTerms, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);
                oTermsAndConditionTable.CompleteRow();
            }
            foreach (ExportPITandCClause oItem in _oExportPITandCClauses)
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString() + ") " + oItem.TermsAndCondition, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);
                oTermsAndConditionTable.CompleteRow();
            }
            return oTermsAndConditionTable;
        }
        private PdfPTable GetAdviseBankTable()
        {
            PdfPTable oAdviseBankTable = new PdfPTable(1);
            oAdviseBankTable.WidthPercentage = 100;
            oAdviseBankTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oAdviseBankTable.SetWidths(new float[] {                                                                 
                                                        300f, //Style Name                                    
                                                   });
            #region Bank Name
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.BankName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            #endregion

            #region Branch
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            BankBranch oBankBranch = new BankBranch();
            oBankBranch = BankBranch.Get(_oExportPI.BankBranchID, _oExportPI.CurrentUserId);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.BranchName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            oAdviseBankTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.BranchAddress, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            oAdviseBankTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Swift Code :" + oBankBranch.SwiftCode, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            oAdviseBankTable.CompleteRow();
            #endregion

            #region Bank Address
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            oAdviseBankTable.CompleteRow();
            #endregion
            return oAdviseBankTable;
        }
        private PdfPTable GetAdviseBankTable_AccountNo()
        {
            PdfPTable oAdviseBankTable = new PdfPTable(2);
            oAdviseBankTable.WidthPercentage = 100;
            oAdviseBankTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oAdviseBankTable.SetWidths(new float[] {                                                                 
                                                        350f,150f //Style Name                                    
                                                   });
            //#region Bank Name
            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase(_oExportPI.BankName, _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //oAdviseBankTable.AddCell(_oPdfPCell);
            //#endregion
            _oPhrase = new Phrase();
            #region Bank Name
            _oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            BankBranch oBankBranch = new BankBranch();
            oBankBranch = BankBranch.Get(_oExportPI.BankBranchID, _oExportPI.CurrentUserId);
            _oPhrase.Add(new Chunk("ADVISING BANK: ", FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(_oExportPI.BankName, FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD)));
            _oPhrase.Add(new Chunk("  Swift Code: ", FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL)));
            _oPhrase.Add(new Chunk(oBankBranch.SwiftCode, FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            _oPhrase = new Phrase();
            if (_oExportPI.BankAccountID > 0)
            {
                _oPhrase.Add(new Chunk("Account No: ", FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL)));
                _oPhrase.Add(new Chunk(_oExportPI.BankAccountNo, FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD)));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oAdviseBankTable.AddCell(_oPdfPCell);
            }
            else
            {

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Border = 0;
                oAdviseBankTable.AddCell(_oPdfPCell);
            }

            oAdviseBankTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Branch:" + oBankBranch.BranchName + ", " + oBankBranch.Address, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2;
            oAdviseBankTable.AddCell(_oPdfPCell);
            oAdviseBankTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Swift Code :" + oBankBranch.SwiftCode, _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Border = 0;
            //oAdviseBankTable.AddCell(_oPdfPCell);
            //oAdviseBankTable.CompleteRow();
            #endregion

            return oAdviseBankTable;
        }
        #endregion
        private PdfPTable LoadCompanyLogo()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(1);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 100f });
            iTextSharp.text.Image oImag;

            if (_oCompany.CompanyLogo != null)
            {
                oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(70f, 25f);
                oPdfPCell1 = new PdfPCell(oImag);
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell1.FixedHeight = 25f;
                oPdfPCell1.Border = 0;
                oPdfPCell1.PaddingRight = 0f;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(oPdfPCell1);
            }
            return oPdfPTable1;
        }
        private PdfPTable LoadCompanyTitle()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(1);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 400f });
            iTextSharp.text.Image oImag;

            if (_oCompany.CompanyTitle != null)
            {
                oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyTitle, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(530f, 65f);
                oPdfPCell1 = new PdfPCell(oImag);
                oPdfPCell1.Border = 0;
                oPdfPCell1.FixedHeight = 100f;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPTable1.AddCell(oPdfPCell1);
            }
            return oPdfPTable1;
        }
        private PdfPTable AddBU_FooderLogo()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(1);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 400f });
            iTextSharp.text.Image oImag;

            if (_oExportPI.BU_Footer != null)
            {
                oImag = iTextSharp.text.Image.GetInstance(_oExportPI.BU_Footer, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(540f, 40f);
                oPdfPCell1 = new PdfPCell(oImag);
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell1.MinimumHeight = 20f;
                oPdfPCell1.Border = 0;
                oPdfPCell1.PaddingRight = 0f;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(oPdfPCell1);
            }
            return oPdfPTable1;
        }        
        private PdfPTable PrintHeader_Common()
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
                _oImag.ScaleAbsolute(62f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //if (!string.IsNullOrEmpty(_oBusinessUnit.Note))
            //{
                
            //    _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Note, FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLDITALIC)));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //}

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
            return oPdfPTable;
        }
        private void Note()
        {
            float nUsagesHeight = 0;
            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);


            /// Total Hight -Margian -Uses Height
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 842 - 40 - 30 - nUsagesHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(this.PrintNote());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private PdfPTable PrintNote()
        {
            string sTemp = "";
            PdfPTable oPdfPTable1 = new PdfPTable(3);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 200f,250f, 150f });
            if (!String.IsNullOrEmpty(_oExportPI.Note))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
                oPdfPCell1 = new PdfPCell(new Phrase("Note : " + _oExportPI.Note, _oFontStyle));
                oPdfPCell1.Border = 0; oPdfPCell1.BorderWidthBottom = 0;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPCell1.Colspan = 3;
                oPdfPTable1.AddCell(oPdfPCell1);
                oPdfPTable1.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            if (!String.IsNullOrEmpty(_oExportPI.BuyerName) && _oExportPI.BuyerName != _oExportPI.ContractorName)
            {
              
                oPdfPCell1 = new PdfPCell(new Phrase("Buying House\n" + _oExportPI.BuyerName, _oFontStyle));
            }
            else
            {
                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell1.BackgroundColor = BaseColor.WHITE;
            oPdfPCell1.Border = 0;
            oPdfPTable1.AddCell(oPdfPCell1);

            if (!String.IsNullOrEmpty(_oExportPI.ContractorContactPersonName))
            {
                sTemp ="Factory: "+ _oExportPI.ContractorContactPersonName;
            }
            
             if (!String.IsNullOrEmpty(_oExportPI.BuyerContactPersonName))
            {
                sTemp = sTemp + " Buying: " + _oExportPI.BuyerContactPersonName;
            }

             if (!String.IsNullOrEmpty(sTemp))
            {
                oPdfPCell1 = new PdfPCell(new Phrase("Concern Person \n" + sTemp, _oFontStyle));
            }
            else
            {
                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell1.BackgroundColor = BaseColor.WHITE;
            oPdfPCell1.Border = 0;
            oPdfPTable1.AddCell(oPdfPCell1);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            oPdfPCell1.Border = 0; oPdfPCell1.BorderWidthBottom = 0;
            oPdfPCell1 = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person\n" + _oExportPI.MKTPName, _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCell1.BackgroundColor = BaseColor.WHITE;
            oPdfPCell1.Border = 0;
            oPdfPTable1.AddCell(oPdfPCell1);
            oPdfPTable1.CompleteRow();

            return oPdfPTable1;
        }        
        ////
        #region For WU -Print Style, Constraction,
        public byte[] PrepareReport_WU(ExportPI oExportPI, Company oCompany, bool bPrintFormat, int nTitleTypeInImg, BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = oBusinessUnit;
         
            _oExportPI = oExportPI;
            _oExportPIDetails = oExportPI.ExportPIDetails;
            _oExportPITandCClauses = oExportPI.ExportPITandCClauses;
            _oExportPIPrintSetup = oExportPI.ExportPIPrintSetup;
            _oCompany = oCompany;
            _bIsInYard = bPrintFormat;
            _nTitleTypeInImg = nTitleTypeInImg;


            _oPdfPTable = new PdfPTable(5);
            _nTotalColumn = 5;
            _oPdfPTable.SetWidths(new float[] { 25f, 335f, 60f, 65f, 75f });

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 10f, 5f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion

            this.PrintHeader();
            this.WaterMark(30f, 30f, 10f, 5f);
            this.PrintBody_WU();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBody_WU()
        {
            #region Buyer & PI Information
            #region Detail Table header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 0);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            //PdfPTable oPdfPTable = new PdfPTable(4);
            //oPdfPTable.SetWidths(new float[] { 50f, 250f, 200f, 100f });

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("P.I.No :", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportPI.PINo, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("Date :", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportPI.IssueDate.ToString("MMMM dd, yyyy"), _oFontStyle_Bold));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 0);
            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            ////this.Note(_nTotalColumn);

            //_oPdfPCell = new PdfPCell(this.GetContractorInfo());
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region PI No & Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("P.I.No : " + _oExportPI.PINo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oExportPI.IssueDate.ToString("MMMM dd, yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //oPdfPTable = new PdfPTable(2);
            //oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region Buyer & PI Information
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("APPLICANT/BUYER NAME:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BENEFICIARY:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.Border = 0; 
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
            #endregion

            #region Transport Mode, Place of Shipment & Place of Destination
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 186.66f, 186.66f, 186.67f, 186.67f });

            _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT TERM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF SHIPMENT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MODE OF TRANSPORT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF DESTINATION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ShipmentTermSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfShipment, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.ShipmentBy, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfDelivery, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region PI Details
            #region Detail Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);

            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //Ratin
            _oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Buyer Ref.", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty(" + (_bIsInYard ? "Yard" : "Meter") + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("UNIT PRICE(" + _oExportPI.Currency + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("AMOUNT(" + _oExportPI.Currency + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            string sTemp = "";
            string sTotalWidth = "";
            int nProductID = 0;
            int nProcessType = 0;
            int nFabricWeave = 0;

            #region Details Data
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);
            foreach (ExportPIDetail oItem in _oExportPIDetails)
            {
                //ExportPI
                nCount++;
                if (nProductID != oItem.ProductID || nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {
                    if (oItem.ProcessType != 0)
                    {
                        sTemp = oItem.ProcessTypeName;
                    }
                    if (oItem.FabricWeave != 0)
                    {
                        sTemp = sTemp + " " + oItem.FabricWeaveName;
                    }

                    //if (_oExportPI.BUID == EnumBusinessUnitType.Weaving)
                    //{
                    //    sTemp = sTemp + " " + " Fabric";
                    //}

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + " " + sTemp, _oFontStyle_Bold));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    sTemp = "";
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                }
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                sTemp = "";
                if (!string.IsNullOrEmpty(oItem.Construction))
                {
                    sTemp = oItem.Construction;
                }

                if (!string.IsNullOrEmpty(oItem.StyleNo))
                {
                    sTemp = sTemp + "   Style : " + oItem.StyleNo;
                    if (!string.IsNullOrEmpty(oItem.ColorInfo))
                    {
                        sTemp = sTemp + ",";
                    }
                }
                if (!string.IsNullOrEmpty(oItem.ColorInfo))
                {
                    sTemp = sTemp + " Color : " + oItem.ColorInfo;
                }
                if (!string.IsNullOrEmpty(oItem.FabricWidth))
                {
                    sTemp = sTemp + ", Width : " + oItem.FabricWidth;
                }
                if (oItem.FinishType != 0)
                {
                    sTemp = sTemp + ", Finish Type : " + oItem.FinishTypeName;
                }
                if (!string.IsNullOrEmpty(oItem.BuyerReference))
                {
                    sTemp = sTemp + ", Buyer Ref : " + oItem.BuyerReference;
                }

                _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                sTemp = "";

                oItem.Qty = (_bIsInYard ? oItem.Qty : Global.GetMeter(oItem.Qty, 2));
                oItem.UnitPrice = (_bIsInYard ? oItem.UnitPrice : oItem.UnitPriceTwo);


                nTotalQty += oItem.Qty;
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty, 2), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


              

                _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));

                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((oItem.Qty * oItem.UnitPrice)), _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice;
                sTotalWidth = oItem.FabricWidth;
                sMUName = oItem.MUName;
                nProductID = oItem.ProductID;
                nProcessType = (int)oItem.ProcessType;
                nFabricWeave = (int)oItem.FabricWeave;
                _oPdfPTable.CompleteRow();
            }

            #region Total Summary
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty,4), _oFontStyle)); //sMUName
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle)); //sMUName
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + Global.MillionFormat(nTotalValue), _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.DollarWords(nTotalValue), _oFontStyle));
            }
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #endregion

            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Terms & Conditions Data :
            ////_oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL);
            ////_oPdfPCell = new PdfPCell(new Phrase("Conditions :", _oFontStyle));
            ////_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ////_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            ////_oPdfPCell.BackgroundColor = BaseColor.WHITE;

            ////_oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(this.GetTermsAndConditionTable_Caption()));
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            PdfPTable oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);
            int i = 0;

            foreach (ExportPITandCClause oItem in _oExportPITandCClauses)
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString("00") + ".", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CaptionName, _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("" + oItem.TermsAndCondition, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                oTermsAndConditionTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }


             oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            /// Total Hight -Margian -Uses Height
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 560 - 40 - 30 - _nUsagesHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

         
            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 8f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ADVISING BANK
            _oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("ADVISING BANK:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region AdviseBankTable
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetAdviseBankTable()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 8f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; 
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Authorized Signature
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 2f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.AcceptanceBy, _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.For, _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            if (_oExportPI.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oExportPI.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(80f, 70f);

                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.ExportPIPrintSetup.AcceptanceBy == "" || _oExportPI.ExportPIPrintSetup.AcceptanceBy == "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region For WU -Print Style, Constraction,
        public byte[] PrepareReport_WUTwo(ExportPI oExportPI, Company oCompany, bool bPrintFormat, int nTitleTypeInImg, BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = oBusinessUnit;

            _oExportPI = oExportPI;
            _oExportPIDetails = oExportPI.ExportPIDetails;
            _oExportPITandCClauses = oExportPI.ExportPITandCClauses;
            _oExportPIPrintSetup = oExportPI.ExportPIPrintSetup;
            _oCompany = oCompany;
            _bIsInYard = bPrintFormat;
            _nTitleTypeInImg = nTitleTypeInImg;


            _oPdfPTable = new PdfPTable(5);
            _nTotalColumn = 5;
            _oPdfPTable.SetWidths(new float[] { 25f, 335f, 60f, 65f, 75f });

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(35f, 15f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.WaterMark(35f, 15f, 5f, 30f);
            this.PrintBody_WUTwo();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void WaterMark(float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(marginLeft, marginRight, marginTop, marginBottom);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;

            if (_oExportPI.ApprovedBy == 0)
            {
                _sWaterMark = "Unauthorised";
            }
            if (_oExportPI.ApprovedBy != 0 && _oExportPI.PIStatus == EnumPIStatus.Cancel)
            {
                _sWaterMark = "Canceled";
            }
            ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            ESimSolWM_Footer.WMFontSize = 75;
            ESimSolWM_Footer.WMRotation = 45;
            ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
            PageEventHandler.WaterMark = _sWaterMark; //Footer print with page event handler
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
        }
        #region Report Header
        private void ReporttHeader()
        {
            #region Proforma Invoice Heading Print
            if (_oExportPI.ApprovedBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised ", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.MinimumHeight = 10; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            if (_oExportPI.ApprovedBy!=0 && _oExportPI.PIStatus == EnumPIStatus.Cancel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Cancel ", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.MinimumHeight = 10; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
        }
        #endregion
        private void PrintBody_WUTwo()
        {
            #region Buyer & PI Information
            #region Detail Table header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.0f, 0);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);

         

            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region PI No & Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            if (_oExportPI.PaymentType == EnumPIPaymentType.NonLC)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Invoice No :   " + _oExportPI.PINo, _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("P.I.No :   " + _oExportPI.PINo, _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oExportPI.IssueDate.ToString("MMMM dd, yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region PO No & Date
            if (!string.IsNullOrEmpty(_oExportPI.OrderSheetNo))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Bill No :   " + _oExportPI.OrderSheetNo, _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("PO No :   " + _oExportPI.OrderSheetNo, _oFontStyle));
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Buyer & PI Information
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("APPLICANT/BUYER NAME:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BENEFICIARY:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.DeliveryToID > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Delivery to", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE)));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(new Phrase("VAT/BIN No: " + _oExportPI.ExportPIPrintSetup.BINNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oExportPI.DeliveryToID > 0)
            {
                Contractor oContractor = new Contractor();
                oContractor = oContractor.Get(_oExportPI.DeliveryToID, 0);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.DeliveryToName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
               
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oContractor.Address, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.Border = 0; 
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
            #endregion

            #region Transport Mode, Place of Shipment & Place of Destination
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 186.66f, 186.66f, 186.67f, 186.67f });

            _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT TERM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF SHIPMENT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MODE OF TRANSPORT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF DESTINATION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ShipmentTermSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfShipment, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.ShipmentBy, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfDelivery, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight =5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Dyeing
            if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
                _oPdfPCell = new PdfPCell(this.ProductDetails_DU());
            }
            else if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving || _oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Finishing)
            {
                _oPdfPCell = new PdfPCell(this.ProductDetails_WU());
            }
            else
            {
                _oPdfPCell = new PdfPCell(this.ProductDetails_WU());
            }
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            //#region Terms & Conditions Data :
            ////_oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL);
            ////_oPdfPCell = new PdfPCell(new Phrase("Conditions :", _oFontStyle));
            ////_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ////_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            ////_oPdfPCell.BackgroundColor = BaseColor.WHITE;

            ////_oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(this.GetTermsAndConditionTable_Caption()));
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;

            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //#endregion
            #region Terms & Conditions Data :

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            if (_nUsagesHeight > 490)
            {
               

                _nUsagesHeight = 0;
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                oPdfPTable.DeleteBodyRows();
                _oPdfPTable.DeleteBodyRows();
                this.PrintHeader();
                this.ReporttHeader();
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            }

            ////_oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL);
            ////_oPdfPCell = new PdfPCell(new Phrase("Conditions :", _oFontStyle));
            ////_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ////_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            ////_oPdfPCell.BackgroundColor = BaseColor.WHITE;

            ////_oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(this.GetTermsAndConditionTable_Caption()));
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            PdfPTable oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);
            int i = 0;

            foreach (ExportPITandCClause oItem in _oExportPITandCClauses)
            {

                oTermsAndConditionTable = new PdfPTable(4);
                oTermsAndConditionTable.WidthPercentage = 100;
                oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString("00") + ".", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CaptionName, _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("" + oItem.TermsAndCondition, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                oTermsAndConditionTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
                _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }


            oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            /// Total Hight -Margian -Uses Height
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 800 - 100 - 30 - _nUsagesHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Colspan = _nTotalColumn;   _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            //#region ADVISING BANK
            //_oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            //   BankBranch oBankBranch = new BankBranch();
            //oBankBranch = BankBranch.Get(_oExportPI.BankBranchID, _oExportPI.CurrentUserId);
            //_oPdfPCell = new PdfPCell(new Phrase("ADVISING BANK: " + _oExportPI.BankName+ ","+"  Swift Code: " + oBankBranch.SwiftCode, _oFontStyle));
            //_oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //_oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL);
            //_oPdfPCell = new PdfPCell(new Phrase("Branch:"+oBankBranch.BranchName + ", "+ oBankBranch.Address, _oFontStyle));
            //_oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            #region AdviseBankTable
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetAdviseBankTable_AccountNo()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion



            #region Authorized Signature
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 2f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.AcceptanceBy, _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.For, _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            if (_oExportPI.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oExportPI.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(80f, 70f);

                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.ExportPIPrintSetup.AcceptanceBy == "" || _oExportPI.ExportPIPrintSetup.AcceptanceBy == "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oPdfPCell = new PdfPCell(this.PrintNote_WU());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();





            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_nTitleTypeInImg == 3)//imge
            {
                _oPdfPCell = new PdfPCell(this.AddBU_FooderLogo());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
          }
        }
        private PdfPTable PrintNote_WU()
        {
            string sTemp = "";
            PdfPTable oPdfPTable1 = new PdfPTable(3);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 200f, 250f, 150f });
            if (!String.IsNullOrEmpty(_oExportPI.Note))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
                oPdfPCell1 = new PdfPCell(new Phrase("Note : " + _oExportPI.Note, _oFontStyle));
                oPdfPCell1.Border = 0; oPdfPCell1.BorderWidthBottom = 0;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPCell1.Colspan = 3;
                oPdfPTable1.AddCell(oPdfPCell1);
                oPdfPTable1.CompleteRow();
            }
     

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
           
            oPdfPCell1 = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person:" + _oExportPI.MKTPName, _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell1.BackgroundColor = BaseColor.WHITE;
            oPdfPCell1.Border = 0;
            oPdfPCell1.Colspan = 3;
            oPdfPTable1.AddCell(oPdfPCell1);
            oPdfPTable1.CompleteRow();

            return oPdfPTable1;
        }
        private PdfPTable PrintNote_DUD()
        {
            string sTemp = "";
            PdfPTable oPdfPTable1 = new PdfPTable(3);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 200f, 250f, 150f });
            if (!String.IsNullOrEmpty(_oExportPI.Note))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
                oPdfPCell1 = new PdfPCell(new Phrase("Note : " + _oExportPI.Note, _oFontStyle));
                oPdfPCell1.Border = 0; oPdfPCell1.BorderWidthBottom = 0;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPCell1.Colspan = 3;
                oPdfPTable1.AddCell(oPdfPCell1);
                oPdfPTable1.CompleteRow();
            }


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);

            oPdfPCell1 = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person:" + _oExportPI.MKTPName +""+( (string.IsNullOrEmpty(_oExportPI.ContractorContactPersonName)) ? "" : " Buyer Concern Person: " + _oExportPI.ContractorContactPersonName), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell1.BackgroundColor = BaseColor.WHITE;
            oPdfPCell1.Border = 0;
            oPdfPCell1.Colspan = 3;
            oPdfPTable1.AddCell(oPdfPCell1);
            oPdfPTable1.CompleteRow();

            return oPdfPTable1;
        }
        private PdfPTable ProductDetails_DU()
        {
            #region Detail Column Header
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 20f, 250f, 90f, 70f, 55f, 75f });
            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description of Goods ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("QTY(" + (_bIsInYard == false ? "LBS" : "KG") + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("UNIT PRICE(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("AMOUNT(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region Details Data
            string sTemp = ""; sMUName = "";
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
          
            foreach (ExportPIDetail oItem in _oExportPIDetails)
            {
                nCount++;
                //oPdfPTable = new PdfPTable(7);
                //oPdfPTable.SetWidths(new float[] { 20f, 265f, 75f, 65f, 60f, 55f, 75f });

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPhrase = new Phrase();

                //string input = "123- abcd33";
                //sTemp = new String(oItem.ProductName.Where(c => c != '/' && c != '%' && c != '-' && (c < '0' || c > '9')).ToArray());
                //_oPhrase.Add(new Chunk(chars + " " + oItem.ProductName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));

                _oPhrase.Add(new Chunk(oItem.ProductDescription + " " + oItem.ProductName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                sTemp = "";
                if (!String.IsNullOrEmpty(oItem.ExportQuality))
                {
                    sTemp = "" + oItem.ExportQuality;
                }
               // _oPhrase.Add(new Chunk("\nShade: " + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                if (!String.IsNullOrEmpty(oItem.ExportQuality))
                {
                    _oPhrase.Add(new Chunk("\nShade: " + oItem.ExportQuality, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                }

                // _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + " [Dark Shade]", _oFontStyle));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (String.IsNullOrEmpty(oItem.ColorInfo))
                {
                    oItem.ColorInfo = "Average\n(Yarn Dyeing Charge)";
                }
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorInfo, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                sTemp = "";
                oItem.Qty = (_bIsInKg == false ? Global.GetKG(oItem.Qty, 10) : oItem.Qty);
                nTotalQty = nTotalQty + oItem.Qty;
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oItem.UnitPrice = (_bIsInKg == false ? Global.GetLBS(oItem.UnitPrice, 10) : oItem.UnitPrice);
           
               
                _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((oItem.Qty * oItem.UnitPrice), 2), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice;

                sMUName = oItem.MUName;
                oPdfPTable.CompleteRow();

                //_oPdfPCell = new PdfPCell(oPdfPTable);
                //_oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
            }

            //oPdfPTable = new PdfPTable(7);
            //oPdfPTable.SetWidths(new float[] { 20f, 265f, 75f, 65f, 60f, 55f, 75f });


            #region Total Summary
         //   _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle_Bold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyle_Bold)); //sMUName
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + Global.MillionFormat(System.Math.Round(System.Math.Round(nTotalValue, 2), 2)), _oFontStyle_Bold));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle_Bold));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word : US " + Global.DollarWords(nTotalValue), _oFontStyle_Bold));
            }
            _oPdfPCell.Colspan = 6; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion


            return oPdfPTable;
        }
        private PdfPTable ProductDetails_WU()
        {
            #region  Details
            #region Detail Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);

            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

            oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Style/Buyer Ref", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Qty(" + (_bIsInYard ? "Yard" : "Meter") + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();



            #endregion

            string sTemp = "";
            string sConst = "";
            string sTotalWidth = "";
            string sConstruction = "";
            int nProductID = 0;
            int nProcessType = 0;
            string sFabricWeaveName="";
            string sFabricWidth = "";
            string sFinishTypeName = "";
            int _nCount_Raw = 0;
            int _nCount_Raw_Style = 0;
            
            string sStyleNo = "";
            string sBuyerReference = "";

            _oExportPIDetails = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.StyleNo, x.ColorInfo, x.UnitPrice, x.BuyerReference, x.MUnitID, x.ExportQuality, x.IsDeduct }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessType = key.ProcessType,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeave = key.FabricWeave,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     FinishType = key.FinishType,
                                                     StyleNo = key.StyleNo,
                                                     ColorInfo = key.ColorInfo,
                                                     BuyerReference = key.BuyerReference,
                                                     Qty = grp.Sum(p => p.Qty),
                                                     UnitPrice = key.UnitPrice,
                                                     MUnitID = key.MUnitID,
                                                     ExportQuality = key.ExportQuality,
                                                     IsDeduct = key.IsDeduct
                                                 }).ToList();

            foreach (ExportPIDetail oItem in _oExportPIDetails)
            {
                if (String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    oItem.StyleNo = oItem.StyleNo;
                }
                else
                {
                    oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
                }
            }
            _oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessType).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
            //_oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ToList();
            #region Details Data
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            foreach (ExportPIDetail oItem in _oExportPIDetails)
            {
                //oPdfPTable = new PdfPTable(6);
                //oPdfPTable.SetWidths(new float[] { 230f, 100f, 110f, 108f, 108f, 108f });

                //oItem.Qty = (_bIsInYard ? oItem.Qty : Global.GetMeter(oItem.Qty, 2));
                //oItem.UnitPrice = (_bIsInYard ? oItem.UnitPrice : oItem.UnitPriceTwo);


                #region PrintDetail
                //_nCount++;
                if (nProductID != oItem.ProductID || sConstruction != oItem.Construction || nProcessType != (int)oItem.ProcessType || sFabricWeaveName != oItem.FabricWeaveName || sFabricWidth != oItem.FabricWidth || sFinishTypeName != oItem.FinishTypeName)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {
                    sTemp = "";
                    sConst = "";
                    if (oItem.ProcessType != 0)
                    {
                        oItem.ProductName = oItem.ProductName + ", " + oItem.ProcessTypeName;
                    }
                    if (oItem.MUnitID > 1)
                    {
                        oItem.ProductName = oItem.ProductName + " Fabrics";
                    }
                    else
                    {
                        if (oItem.IsDeduct)
                        {
                            oItem.ProductName = oItem.ProductName + "(Deduct)";
                        }
                        else
                        {
                            oItem.ProductName = oItem.ProductName;
                        }
                    }
                    if (!string.IsNullOrEmpty(oItem.Construction))
                    {
                        sConst = sConst + "Const: " + oItem.Construction;
                    }

                    if (oItem.FabricWeave != 0)
                    {
                        sTemp = sTemp + " " + oItem.FabricWeaveName;
                    }

                    if (!string.IsNullOrEmpty(oItem.FabricWidth))
                    {
                        sTemp = sTemp + ", Width : " + oItem.FabricWidth;
                    }
                    if (oItem.FinishType != 0)
                    {
                        sTemp = sTemp + ", Finish Type : " + oItem.FinishTypeName;
                    }
                    _nCount_Raw = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProcessTypeName == oItem.ProcessTypeName && x.Construction == oItem.Construction && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName).Count();

                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                    //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                    _oPdfPCell = new PdfPCell(_oPhrase);

                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, 0)));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportQuality, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;

                    //_oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);
                }


                if (nProductID != oItem.ProductID || sConstruction != oItem.Construction || nProcessType != (int)oItem.ProcessType || sFabricWeaveName != oItem.FabricWeaveName || sFabricWidth != oItem.FabricWidth || sFinishTypeName != oItem.FinishTypeName || sStyleNo != oItem.StyleNo)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {
                    _nCount_Raw_Style = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.Construction == oItem.Construction && x.ProcessType == (int)oItem.ProcessType && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.StyleNo == oItem.StyleNo).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = _nCount_Raw_Style;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);
                }

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo + " " + oItem.BuyerReference, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.MinimumHeight = 40f;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorInfo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.IsDeduct)
                {
                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                    _oPdfPCell = new PdfPCell(new Phrase("("+_oExportPI.Currency + "" + Global.MillionFormat(oItem.Qty * oItem.UnitPrice)+")", _oFontStyle));
                }
                else {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(oItem.Qty * oItem.UnitPrice), _oFontStyle));
                }
               
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                //_nUsagesHeight = CalculatePdfPTableHeight(oPdfPTable);
                //if (_nUsagesHeight > 450)
                //{
                //    _oPdfPCell = new PdfPCell(oPdfPTable);
                //    _oPdfPCell.Colspan = _nTotalColumn;
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //    _oPdfPTable.CompleteRow();

                //    _nUsagesHeight = 0;
                //    //_oDocument.Add(_oPdfPTable);
                //    _oDocument.NewPage();
                //    //oPdfPTable.DeleteBodyRows();
                //    _oPdfPTable.DeleteBodyRows();
                //    oPdfPTable = new PdfPTable(7);

                //    oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });
                //    this.PrintHeader();
                //    this.ReporttHeader();
                //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                //    _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                //}


                #endregion
                if (oItem.IsDeduct) { nTotalValue = nTotalValue - oItem.Qty * oItem.UnitPrice; }
                else { nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice; nTotalQty += oItem.Qty; }
               
                sTotalWidth = oItem.FabricWidth;
                sMUName = oItem.MUName;
                nProductID = oItem.ProductID;
                nProcessType = (int)oItem.ProcessType;
              
                sConstruction = oItem.Construction;
                sStyleNo = oItem.StyleNo;
                sBuyerReference = oItem.BuyerReference;

                sFabricWeaveName = oItem.FabricWeaveName;
                sFabricWidth = oItem.FabricWidth;
                sFinishTypeName = oItem.FinishTypeName;
               
            }


            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + (_bIsInYard ? " Y" : "M"), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(nTotalValue), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

          

            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.DollarWords(nTotalValue), _oFontStyle));
            }
            _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_nUsagesHeight = CalculatePdfPTableHeight(oPdfPTable);

            //if (_nUsagesHeight > 400)
            //{
            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.Colspan = _nTotalColumn;
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();

            //    _nUsagesHeight = 0;
            //    _oDocument.Add(_oPdfPTable);
            //    _oDocument.NewPage();
            //    //oPdfPTable.DeleteBodyRows();
            //    _oPdfPTable.DeleteBodyRows();
            //    oPdfPTable = new PdfPTable(7);

            //    oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });
            //    this.PrintHeader();
            //    this.ReporttHeader();
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //    _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            //}

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion

            #endregion

            return oPdfPTable;
        }
        #endregion
        #region For WU Type C:  -Print Style, Constraction,
        public byte[] PrepareReport_WU_Type_C(ExportPI oExportPI, Company oCompany, bool bPrintFormat, int nTitleTypeInImg, BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = oBusinessUnit;

            _oExportPI = oExportPI;
            _oExportPIDetails = oExportPI.ExportPIDetails;
            _oExportPITandCClauses = oExportPI.ExportPITandCClauses;
            _oExportPIPrintSetup = oExportPI.ExportPIPrintSetup;
            _oCompany = oCompany;
            _bIsInYard = bPrintFormat;
            _nTitleTypeInImg = nTitleTypeInImg;
            _nTotalColumn = 8;
            _oPdfPTable = new PdfPTable(_nTotalColumn);
             //200f,  80f,80f, 140f, 70f, 70f, 75f
            _oPdfPTable.SetWidths(new float[] { 138f, 32f, 60f,60f, 100f, 70f, 61f, 74f });

            //_oPdfPTable = new PdfPTable(5);
            //_nTotalColumn = 5;
            //_oPdfPTable.SetWidths(new float[] { 25f, 335f, 60f, 65f, 75f });

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(35f, 15f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion

            this.PrintHeader_slogan();
            this.ReporttHeader();
            this.PrintBody_WU_Type_C();
            this.ProductDetails_WU_Type_C2();
            this.PrintBody2_WUTwo();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Print Header
        private void PrintHeader_slogan()
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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("(Committed With Client)", FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLDITALIC)));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

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
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            if (_oExportPI.PaymentType == EnumPIPaymentType.NonLC)
            {
                _oPdfPCell = new PdfPCell(new Phrase("INVOICE", _oFontStyle));
            }
            else
            {
                if (_oExportPI.PIType == EnumPIType.SalesContract)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("SALES CONTRACT ", _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("PROFORMA INVOICE", _oFontStyle));
                }
            }
            //_oPdfPCell = new PdfPCell(new Phrase("PROFORMA INVOICE", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1f; _oPdfPCell.FixedHeight = 25; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            //#region ReportHeader
            //#region Blank Space
            //_oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //#endregion
            //#endregion

            #endregion
        }
        #endregion
        private void PrintBody_WU_Type_C()
        {
            #region Buyer & PI Information
            #region Detail Table header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 0);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);



            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region PI No & Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            if (_oExportPI.PaymentType == EnumPIPaymentType.NonLC)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Invoice No :   " + _oExportPI.PINo, _oFontStyle_Bold));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("P.I.No :   " + _oExportPI.PINo, _oFontStyle_Bold));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oExportPI.IssueDate.ToString("MMMM dd, yyyy"), _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region PO No & Date
            if (!string.IsNullOrEmpty(_oExportPI.OrderSheetNo))
            {
                //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("PO No :   " + _oExportPI.OrderSheetNo, _oFontStyle_Bold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Buyer & PI Information
            _oFontStyle = FontFactory.GetFont("Tahoma",8f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("APPLICANT NAME:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BENEFICIARY:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            Contractor oContractor = new Contractor();
            
            if (_oExportPI.BuyerID > 0)
            {
                oContractor = oContractor.Get(_oExportPI.BuyerID, 0);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            _oPhrase = new Phrase();
            if (!string.IsNullOrEmpty(_oExportPI.BuyerName))
            {
            _oPhrase.Add(new Chunk("BUYER NAME:", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE)));
            _oPhrase.Add(new Chunk(" "+_oExportPI.BuyerName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            }
            _oPdfPCell = new PdfPCell(_oPhrase);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (!String.IsNullOrEmpty(_oExportPI.ExportPIPrintSetup.BINNo))
            {
                _oPdfPCell = new PdfPCell(new Phrase("VAT/BIN No: " + _oExportPI.ExportPIPrintSetup.BINNo, _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("" + _oExportPI.ExportPIPrintSetup.BINNo, _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.DeliveryToID > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Delivery to", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE)));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oExportPI.DeliveryToID > 0)
            {
                 oContractor = new Contractor();
                oContractor = oContractor.Get(_oExportPI.DeliveryToID, 0);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.DeliveryToName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oContractor.Address, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.Border = 0; 
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
            #endregion

            #region Transport Mode, Place of Shipment & Place of Destination
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 186.66f, 186.66f, 186.67f, 186.67f });

            _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT TERM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF SHIPMENT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MODE OF TRANSPORT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF DESTINATION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ShipmentTermSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfShipment, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.ShipmentBy, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfDelivery, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            //#region ADVISING BANK
            //_oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            //   BankBranch oBankBranch = new BankBranch();
            //oBankBranch = BankBranch.Get(_oExportPI.BankBranchID, _oExportPI.CurrentUserId);
            //_oPdfPCell = new PdfPCell(new Phrase("ADVISING BANK: " + _oExportPI.BankName+ ","+"  Swift Code: " + oBankBranch.SwiftCode, _oFontStyle));
            //_oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //_oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL);
            //_oPdfPCell = new PdfPCell(new Phrase("Branch:"+oBankBranch.BranchName + ", "+ oBankBranch.Address, _oFontStyle));
            //_oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            
            //#region Authorized Signature
            //oPdfPTable = new PdfPTable(3);
            //oPdfPTable.SetWidths(new float[] { 200f, 100f, 2f });

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.AcceptanceBy, _oFontStyle));
            //_oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.For, _oFontStyle));
            //_oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //if (_oExportPI.Signature == null)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //}
            //else
            //{
            //    _oImag = iTextSharp.text.Image.GetInstance(_oExportPI.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    _oImag.ScaleAbsolute(80f, 70f);

            //    _oPdfPCell = new PdfPCell(_oImag);
            //}
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            //if (_oExportPI.ExportPIPrintSetup.AcceptanceBy == "" || _oExportPI.ExportPIPrintSetup.AcceptanceBy == "N/A")
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //}
            //else
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //}

            //_oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion
            //_oPdfPCell = new PdfPCell(this.PrintNote_WU());
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();





            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 25;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //if (_nTitleTypeInImg == 3)//imge
            //{
            //    _oPdfPCell = new PdfPCell(this.AddBU_FooderLogo());
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //    _oPdfPCell.Border = 0;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPCell.Colspan = _nTotalColumn;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
        }
        private PdfPTable ProductDetails_WU_Type_C()
        {
            #region  Details
            #region Detail Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);
            List<ExportPIDetail> oExportPIDetailsTemp = new List<ExportPIDetail>();
            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 200f,  80f,80f, 140f, 70f, 70f, 75f });

            oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle_Bold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

        

            oPdfPCell = new PdfPCell(new Phrase("Style", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Qty(" + (_bIsInYard ? "Yard" : "Meter") + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();



            #endregion

            string sTemp = "";
            string sConst = "";
            string sTotalWidth = "";
            string sConstruction = "";
            string sFabricNo = "";
            int nProductID = 0;
            int nProcessType = 0;
            string sFabricWeaveName = "";
            string sFabricWidth = "";
            string sFinishTypeName = "";
            int _nCount_Raw = 0;
            int _nCount_Raw_Style = 0;

            string sStyleNo = "";
            string sBuyerReference = "";

            oExportPIDetailsTemp = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.FabricNo, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.StyleNo, x.ColorInfo, x.UnitPrice, x.BuyerReference, x.MUnitID, x.ExportQuality, x.IsDeduct, x.Weight, x.Shrinkage,x.ProductDescription }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     FabricNo=key.FabricNo,
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessType = key.ProcessType,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeave = key.FabricWeave,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     FinishType = key.FinishType,
                                                     StyleNo = key.StyleNo,
                                                     ColorInfo = key.ColorInfo,
                                                     BuyerReference = key.BuyerReference,
                                                     Qty = grp.Sum(p => p.Qty),
                                                     UnitPrice = key.UnitPrice,
                                                     MUnitID = key.MUnitID,
                                                     ExportQuality = key.ExportQuality,
                                                     IsDeduct = key.IsDeduct,
                                                     Weight = key.Weight,
                                                     Shrinkage = key.Shrinkage,
                                                     ProductDescription = key.ProductDescription
                                                 }).ToList();

            //foreach (ExportPIDetail oItem in _oExportPIDetails)
            //{
            //    if (String.IsNullOrEmpty(oItem.BuyerReference))
            //    {
            //        oItem.StyleNo = oItem.StyleNo;
            //    }
            //    else
            //    {
            //        oItem.StyleNo = oItem.StyleNo + "/" + oItem.BuyerReference;
            //    }
            //}
            oExportPIDetailsTemp = oExportPIDetailsTemp.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessType).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
            
            #region Details Data
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            foreach (ExportPIDetail oItem in oExportPIDetailsTemp)
            {
                //oItem.ProductDescription = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProcessTypeName == oItem.ProcessTypeName && x.Construction == oItem.Construction && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName ).ToList().FirstOrDefault().ProductDescription;
                //oPdfPTable = new PdfPTable(6);
                //oPdfPTable.SetWidths(new float[] { 230f, 100f, 110f, 108f, 108f, 108f });

                //oItem.Qty = (_bIsInYard ? oItem.Qty : Global.GetMeter(oItem.Qty, 2));
                //oItem.UnitPrice = (_bIsInYard ? oItem.UnitPrice : oItem.UnitPriceTwo);


                #region PrintDetail
                //_nCount++;
                if (sFabricNo != oItem.FabricNo || nProductID != oItem.ProductID || sConstruction != oItem.Construction || nProcessType != (int)oItem.ProcessType || sFabricWeaveName != oItem.FabricWeaveName || sFabricWidth != oItem.FabricWidth || sFinishTypeName != oItem.FinishTypeName)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {
                    sTemp = "";
                    sConst = "";
                    if (oItem.FabricWeave != 0)
                    {
                        oItem.ProductName ="Comp:"+ oItem.ProductName + ", Weave: " + oItem.FabricWeaveName;
                    }
                    if (!String.IsNullOrEmpty(oItem.FabricNo))
                    {
                        oItem.ProductName ="Article: "+oItem.FabricNo+"\n"+oItem.ProductName;
                    }
                    //else
                    //{
                        if (oItem.IsDeduct)
                        {
                            oItem.ProductName = oItem.ProductName + "(Deduct)";
                        }
                        //else
                        //{
                        //    oItem.ProductName = oItem.ProductName;
                        //}
                    //}
                    if (!string.IsNullOrEmpty(oItem.Construction))
                    {
                        sConst = sConst + "Const: " + oItem.Construction;
                    }
                    if (!string.IsNullOrEmpty(oItem.FabricWidth))
                    {
                        sTemp = sTemp + "Width : " + oItem.FabricWidth;
                    }
                    if (!string.IsNullOrEmpty(oItem.Weight))
                    {
                        sTemp = sTemp + " Weight : " + oItem.Weight;
                    }
                    if (oItem.ProcessType != 0)
                    {
                        sTemp = sTemp + "\nProcess: " + oItem.ProcessTypeName;
                    }
                    if (oItem.FinishType != 0)
                    {
                        sTemp = sTemp + ", Finish: " + oItem.FinishTypeName;
                    }
                     if (!string.IsNullOrEmpty(oItem.Shrinkage))
                    {
                        sTemp = sTemp + "\nShrinkage: " + oItem.Shrinkage;
                    }
                     if (!string.IsNullOrEmpty(oItem.ProductDescription))
                     {
                         sTemp = sTemp + "\n" + oItem.ProductDescription;
                     }

                     _nCount_Raw = oExportPIDetailsTemp.Where(x => x.ProductID == oItem.ProductID && x.FabricNo == oItem.FabricNo && x.ProcessTypeName == oItem.ProcessTypeName && x.Construction == oItem.Construction && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName).Count();

                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oItem.ProductName + "\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                    //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                    _oPdfPCell = new PdfPCell(_oPhrase);

                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, 0)));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Rowspan = _nCount_Raw;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    oPdfPTable.AddCell(_oPdfPCell);

                    //_oPdfPCell = new PdfPCell(new Phrase(oItem.ExportQuality, _oFontStyle));
                    //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    //_oPdfPCell.Rowspan = _nCount_Raw;
                    //_oPdfPCell.Border = 0;
                    //_oPdfPCell.BorderWidthLeft = 0.5f;
                    //_oPdfPCell.BorderWidthTop = 0.5f;

                    ////_oPdfPCell.MinimumHeight = 40f;
                    //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //oPdfPTable.AddCell(_oPdfPCell);
                }


                if (sFabricNo != oItem.FabricNo || nProductID != oItem.ProductID || sConstruction != oItem.Construction || nProcessType != (int)oItem.ProcessType || sFabricWeaveName != oItem.FabricWeaveName || sFabricWidth != oItem.FabricWidth || sFinishTypeName != oItem.FinishTypeName || sStyleNo != oItem.StyleNo)/// nProcessType != (int)oItem.ProcessType || nFabricWeave != (int)oItem.FabricWeave)
                {
                    _nCount_Raw_Style = oExportPIDetailsTemp.Where(x => x.ProductID == oItem.ProductID && x.FabricNo == oItem.FabricNo && x.Construction == oItem.Construction && x.ProcessType == (int)oItem.ProcessType && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.StyleNo == oItem.StyleNo).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.MinimumHeight = 40f;
                    _oPdfPCell.Rowspan = _nCount_Raw_Style;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                 
                }


                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerReference, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorInfo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                //_oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.IsDeduct)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.AmountSt, _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(oItem.Qty * oItem.UnitPrice), _oFontStyle));
                }

                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.MinimumHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);



                oPdfPTable.CompleteRow();
                #endregion
                if (oItem.IsDeduct) { nTotalValue = nTotalValue - oItem.Qty * oItem.UnitPrice; }
                else { nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice; }
                if (oItem.MUnitID > 1)
                {
                    nTotalQty += oItem.Qty;
                }
                sTotalWidth = oItem.FabricWidth;
                sMUName = oItem.MUName;
                nProductID = oItem.ProductID;
                sFabricNo = oItem.FabricNo;
                nProcessType = (int)oItem.ProcessType;

                sConstruction = oItem.Construction;
                sStyleNo = oItem.StyleNo;
                sBuyerReference = oItem.BuyerReference;

                sFabricWeaveName = oItem.FabricWeaveName;
                sFabricWidth = oItem.FabricWidth;
                sFinishTypeName = oItem.FinishTypeName;

            }


            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            ////_oPdfPCell.MinimumHeight = 40f;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + (_bIsInYard ? " Y" : "M"), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(nTotalValue), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);



            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word : US " + Global.DollarWords(nTotalValue), _oFontStyle));
            }
            _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _nUsagesHeight = CalculatePdfPTableHeight(oPdfPTable);


            #endregion

            #endregion

            return oPdfPTable;
        }
        private void ProductDetails_WU_Type_C2()
        {
            #region  Details
            #region Detail Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);

            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }

            //PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

            oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle_Bold));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Style", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Qty(" + (_bIsInYard ? "Yard" : "Meter") + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            _oPdfPTable.CompleteRow();


            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion

            string sTemp = "";
            string sConst = "";



            _oExportPIDetails = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.FabricNo, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.StyleNo, x.ColorInfo, x.UnitPrice, x.BuyerReference, x.MUnitID, x.ExportQuality, x.ShadeType, x.IsDeduct, x.Weight, x.Shrinkage, x.ProductDescription }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     FabricNo = key.FabricNo,
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessType = key.ProcessType,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeave = key.FabricWeave,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     FinishType = key.FinishType,
                                                     StyleNo = key.StyleNo,
                                                     ColorInfo = key.ColorInfo,
                                                     BuyerReference = key.BuyerReference,
                                                     Qty = grp.Sum(p => p.Qty),
                                                     UnitPrice = key.UnitPrice,
                                                     MUnitID = key.MUnitID,
                                                     ExportQuality = key.ExportQuality,
                                                     ShadeType = key.ShadeType,
                                                     IsDeduct = key.IsDeduct,
                                                     Weight = key.Weight,
                                                     Shrinkage = key.Shrinkage,
                                                     ProductDescription = key.ProductDescription
                                                 }).ToList();


            _oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessType).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
            //_oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ToList();
            #region Details Data
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            List<ExportPIDetail> oExportPIDetailFab = new List<ExportPIDetail>();
            List<ExportPIDetail> oExportPIDetailColors = new List<ExportPIDetail>();
            List<ExportPIDetail> oExportPIDetailStyleNo = new List<ExportPIDetail>();

            oExportPIDetailFab = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.FabricNo, x.FabricID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.IsDeduct, x.ExportQuality, x.ShadeType, x.Weight, x.Shrinkage, x.ProductDescription }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     FabricNo = key.FabricNo,
                                                     FabricID = key.FabricID,
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessType = key.ProcessType,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeave = key.FabricWeave,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     FinishType = key.FinishType,
                                                     //StyleNo = key.StyleNo,
                                                     //ColorInfo = key.ColorInfo,
                                                     //BuyerReference = key.BuyerReference,
                                                     //Qty = grp.Sum(p => p.Qty),
                                                     //UnitPrice = key.UnitPrice,
                                                     //MUnitID = key.MUnitID,
                                                     ShadeType = key.ShadeType,
                                                     ExportQuality = key.ExportQuality,
                                                     IsDeduct = key.IsDeduct,
                                                     Weight = key.Weight,
                                                     Shrinkage = key.Shrinkage,
                                                     ProductDescription = key.ProductDescription
                                                 }).ToList();
            bool bFlag = false;
            bool bFlagTwo = false;
            bool bIsNewPage = false;
            foreach (ExportPIDetail oItem in oExportPIDetailFab)
            {
                oExportPIDetailStyleNo = new List<ExportPIDetail>();
                //oExportPIDetailColor = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.MUnitID == oItem.MUnitID && x.IsDeduct == oItem.IsDeduct).ToList();
                oExportPIDetailStyleNo = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.FabricNo == oItem.FabricNo && x.ExportQuality == oItem.ExportQuality && x.ShadeType == oItem.ShadeType && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct && x.Weight == oItem.Weight && x.Shrinkage == oItem.Shrinkage && x.ProductDescription == oItem.ProductDescription).ToList();
                bFlag = true;

                sTemp = "";
                sConst = "";
                if (oItem.FabricWeave != 0)
                {
                    oItem.ProductName = "Comp:" + oItem.ProductName + ", Weave: " + oItem.FabricWeaveName;
                }
                if (!String.IsNullOrEmpty(oItem.FabricNo))
                {
                    oItem.ProductName = "Article: " + oItem.FabricNo + "\n" + oItem.ProductName;
                }
                //else
                //{
                if (oItem.IsDeduct)
                {
                    oItem.ProductName = oItem.ProductName + "(Deduct)";
                }
               
                if (!string.IsNullOrEmpty(oItem.Construction))
                {
                    sConst = sConst + "Const: " + oItem.Construction;
                }

                if (!string.IsNullOrEmpty(oItem.FabricWidth))
                {
                    sTemp = sTemp + "F.Width : " + oItem.FabricWidth;
                }
                if (!string.IsNullOrEmpty(oItem.Weight))
                {
                    sTemp = sTemp + " Weight : " + oItem.Weight;
                }
                if (oItem.ProcessType != 0)
                {
                    sTemp = sTemp + "\nProcess: " + oItem.ProcessTypeName;
                }
                if (oItem.FinishType != 0)
                {
                    sTemp = sTemp + ", Finish: " + oItem.FinishTypeName;
                }
                if (!string.IsNullOrEmpty(oItem.Shrinkage))
                {
                    sTemp = sTemp + "\nShrinkage: " + oItem.Shrinkage;
                }
                if (!string.IsNullOrEmpty(oItem.ProductDescription))
                {
                    sTemp = sTemp + "\n" + oItem.ProductDescription;
                }


                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell = new PdfPCell(_oPhrase);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, 0)));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                //_oPdfPCell.Rowspan = _nCount_Raw;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.ShadeType != EnumDepthOfShade.None)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ShadeType.ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportQuality, _oFontStyle));
                }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_oPdfPCell.Rowspan = _nCount_Raw;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPTable.AddCell(_oPdfPCell);


                oExportPIDetailStyleNo = oExportPIDetailStyleNo.GroupBy(x => new { x.StyleNo ,x.BuyerReference}, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     StyleNo = key.StyleNo,
                                                     BuyerReference = key.BuyerReference,
                                                 }).ToList();

                oExportPIDetailStyleNo = oExportPIDetailStyleNo.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.BuyerReference).ThenBy(c => c.ColorInfo).ToList();
                foreach (ExportPIDetail oItem2 in oExportPIDetailStyleNo)
                {
                    //oExportPIDetailColors = _oExportPIDetails.Where(x => x.StyleNo == oItem2.StyleNo).ToList();
                    oExportPIDetailColors = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.FabricNo == oItem.FabricNo && x.StyleNo == oItem2.StyleNo && x.BuyerReference == oItem2.BuyerReference && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct && x.ExportQuality == oItem.ExportQuality && x.ShadeType == oItem.ShadeType && x.Shrinkage == oItem.Shrinkage && x.Weight == oItem.Weight && x.ProductDescription == oItem.ProductDescription).ToList();
                    if (!bFlag)
                    {
                        //if (bIsNewPage)
                        //{
                        //    _oPhrase = new Phrase();
                        //    _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                        //    //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                        //    _oPdfPCell = new PdfPCell(_oPhrase);
                        //}
                        //else
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //}
                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BorderWidthLeft = 0.5f;
                        if (bIsNewPage)
                        {
                            _oPdfPCell.BorderWidthTop = 0.5f;
                        }
                        else
                        {
                            _oPdfPCell.BorderWidthTop = 0;
                        }
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BorderWidthLeft = 0.5f;
                        if (bIsNewPage)
                        {
                            _oPdfPCell.BorderWidthTop = 0.5f;
                        }
                        else
                        {
                            _oPdfPCell.BorderWidthTop = 0;
                        }
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }

                    bFlag = false;

                    //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oItem2.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem2.BuyerReference, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    oExportPIDetailColors = oExportPIDetailColors.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
                    bFlagTwo = true;
                    foreach (ExportPIDetail oItem3 in oExportPIDetailColors)
                    {
                        if (!bFlagTwo)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                        }
                        bFlagTwo = false;
                        _oPdfPCell = new PdfPCell(new Phrase(oItem3.ColorInfo, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem3.Qty), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem3.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                        //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        if (oItem.IsDeduct)
                        {
                            //_oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                            _oPdfPCell = new PdfPCell(new Phrase("(" + _oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice) + ")", _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice), _oFontStyle));
                        }

                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        bIsNewPage = false;
                        _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                        if (_nUsagesHeight > 700)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.Colspan = _nTotalColumn;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0;
                            _oPdfPCell.BorderWidthRight = 0;
                            _oPdfPCell.BorderWidthTop = 0.5f;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();

                            _nUsagesHeight = 0;
                            _oDocument.Add(_oPdfPTable);
                            _oDocument.NewPage();
                            //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
                            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
                            //_oDocument.SetMargins(35f, 15f, 5f, 30f);
                            //oPdfPTable.DeleteBodyRows();
                            _oPdfPTable.DeleteBodyRows();
                            _nTotalColumn = 8;
                            _oPdfPTable = new PdfPTable(_nTotalColumn);
                            _oPdfPTable.SetWidths(new float[] { 138f, 32f, 60f, 60f, 100f, 70f, 61f, 74f });
                            _oPdfPTable.WidthPercentage = 100;
                            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            this.PrintHeader();
                            this.ReporttHeader();
                            //isNewPage = true;
                            //this.WaterMark(35f, 15f, 5f, 30f);
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                            bIsNewPage = true;
                        }
                    }

                }

            }
            nTotalQty = _oExportPIDetails.Where(c => c.MUnitID > 1).Sum(x => x.Qty);
            //nTotalQty = _oExportPIDetails.Sum(x => x.Qty);
            nTotalValue = _oExportPIDetails.Where(c => c.IsDeduct == false).Sum(x => x.Qty * x.UnitPrice);
            nTotalValue = nTotalValue - _oExportPIDetails.Where(c => c.IsDeduct == true).Sum(x => x.Qty * x.UnitPrice);
            //oPdfPTable = new PdfPTable(7);
            //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 5;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + (_bIsInYard ? " Y" : "M"), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(nTotalValue), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);


            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.DollarWords(nTotalValue), _oFontStyle));
            }
            _oPdfPCell.Colspan = 8; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            #endregion

            #endregion


        }
        #endregion
        #region Wiving One
        public byte[] PrepareReport_WUOne(ExportPI oExportPI, Company oCompany, bool bPrintFormat, int nTitleTypeInImg, BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = oBusinessUnit;

            _oExportPI = oExportPI;
            _oExportPIDetails = oExportPI.ExportPIDetails;
            _oExportPITandCClauses = oExportPI.ExportPITandCClauses;
            _oExportPIPrintSetup = oExportPI.ExportPIPrintSetup;
            _oCompany = oCompany;
            _bIsInYard = bPrintFormat;
            _nTitleTypeInImg = nTitleTypeInImg;
            _nTotalColumn = 7;
            _oPdfPTable = new PdfPTable(_nTotalColumn);

            _oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(35f, 15f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion


            this.PrintHeader();
            this.ReporttHeader();
            this.WaterMark(35f, 15f, 5f, 30f);
            this.PrintBody_WUOne();
            this.PrintBodyProduct_WUOne();
            this.PrintBody2_WUOne();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBody_WUOne()
        {
            #region Buyer & PI Information
            #region Detail Table header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.0f, 0);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);


            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region PI No & Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            if (_oExportPI.PaymentType == EnumPIPaymentType.NonLC)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Invoice No :   " + _oExportPI.PINo, _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("P.I.No :   " + _oExportPI.PINo, _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oExportPI.IssueDate.ToString("MMMM dd, yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region PO No & Date
            if (!string.IsNullOrEmpty(_oExportPI.OrderSheetNo))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Bill No :   " + _oExportPI.OrderSheetNo, _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("PO No :   " + _oExportPI.OrderSheetNo, _oFontStyle));
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Buyer & PI Information
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("APPLICANT/BUYER NAME:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BENEFICIARY:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.DeliveryToID > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Delivery to", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE)));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(new Phrase("VAT/BIN No: " + _oExportPI.ExportPIPrintSetup.BINNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oExportPI.DeliveryToID > 0)
            {
                Contractor oContractor = new Contractor();
                oContractor = oContractor.Get(_oExportPI.DeliveryToID, 0);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.DeliveryToName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oContractor.Address, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.Border = 0; 
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
            #endregion

            #region Transport Mode, Place of Shipment & Place of Destination
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 186.66f, 186.66f, 186.67f, 186.67f });

            _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT TERM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF SHIPMENT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MODE OF TRANSPORT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF DESTINATION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ShipmentTermSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfShipment, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.ShipmentBy, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfDelivery, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

         
         
        }
      
        //private void PrintBodyProduct_WUOne()
        //{
        //    #region  Details
        //    #region Detail Column Header
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);

        //    string sMUName = "";
        //    if (_oExportPIDetails.Count > 0)
        //    {
        //        sMUName = _oExportPIDetails[0].MUName;
        //    }

        //    //PdfPTable oPdfPTable = new PdfPTable(7);
        //    PdfPCell oPdfPCell;
        //    //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

        //    oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle_Bold));
        //    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPTable.AddCell(oPdfPCell);

        //    //oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle_Bold));
        //    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    //_oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase("Style/Buyer Ref", _oFontStyle_Bold));
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPTable.AddCell(oPdfPCell);


        //    oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle_Bold));
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPTable.AddCell(oPdfPCell);


        //    oPdfPCell = new PdfPCell(new Phrase("Qty(" + (_bIsInYard ? "Yard" : "Meter") + ")", _oFontStyle_Bold));
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPTable.AddCell(oPdfPCell);

        //    oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
        //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPTable.AddCell(oPdfPCell);

        //    _oPdfPTable.CompleteRow();


        //    //_oPdfPCell = new PdfPCell(oPdfPTable);
        //    //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    //_oPdfPTable.CompleteRow();

        //    #endregion

        //    string sTemp = "";
        //    string sConst = "";
           
          

        //    _oExportPIDetails = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.StyleNo, x.ColorInfo, x.UnitPrice, x.BuyerReference, x.MUnitID, x.ExportQuality, x.IsDeduct }, (key, grp) =>
        //                                         new ExportPIDetail
        //                                         {
        //                                             ProductID = key.ProductID,
        //                                             ProductName = key.ProductName,
        //                                             Construction = key.Construction,
        //                                             ProcessType = key.ProcessType,
        //                                             ProcessTypeName = key.ProcessTypeName,
        //                                             FabricWeave = key.FabricWeave,
        //                                             FabricWeaveName = key.FabricWeaveName,
        //                                             FabricWidth = key.FabricWidth,
        //                                             FinishTypeName = key.FinishTypeName,
        //                                             FinishType = key.FinishType,
        //                                             StyleNo = key.StyleNo,
        //                                             ColorInfo = key.ColorInfo,
        //                                             BuyerReference = key.BuyerReference,
        //                                             Qty = grp.Sum(p => p.Qty),
        //                                             UnitPrice = key.UnitPrice,
        //                                             MUnitID = key.MUnitID,
        //                                             ExportQuality = key.ExportQuality,
        //                                             IsDeduct = key.IsDeduct
        //                                         }).ToList();

        //    foreach (ExportPIDetail oItem in _oExportPIDetails)
        //    {
        //        if (String.IsNullOrEmpty(oItem.BuyerReference))
        //        {
        //            oItem.StyleNo = oItem.StyleNo;
        //        }
        //        else
        //        {
        //            oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
        //        }
        //    }
        //    _oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessType).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
        //    //_oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ToList();
        //    #region Details Data
        //    int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
        //    _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

        //    List<ExportPIDetail> oExportPIDetailFab = new List<ExportPIDetail>();
        //    List<ExportPIDetail> oExportPIDetailColors = new List<ExportPIDetail>();
        //    List<ExportPIDetail> oExportPIDetailStyleNo = new List<ExportPIDetail>();

        //    oExportPIDetailFab = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.IsDeduct, x.ExportQuality}, (key, grp) =>
        //                                         new ExportPIDetail
        //                                         {
        //                                             ProductID = key.ProductID,
        //                                             ProductName = key.ProductName,
        //                                             Construction = key.Construction,
        //                                             ProcessType = key.ProcessType,
        //                                             ProcessTypeName = key.ProcessTypeName,
        //                                             FabricWeave = key.FabricWeave,
        //                                             FabricWeaveName = key.FabricWeaveName,
        //                                             FabricWidth = key.FabricWidth,
        //                                             FinishTypeName = key.FinishTypeName,
        //                                             FinishType = key.FinishType,
        //                                             //StyleNo = key.StyleNo,
        //                                             //ColorInfo = key.ColorInfo,
        //                                             //BuyerReference = key.BuyerReference,
        //                                             //Qty = grp.Sum(p => p.Qty),
        //                                             //UnitPrice = key.UnitPrice,
        //                                             //MUnitID = key.MUnitID,
        //                                             ExportQuality = key.ExportQuality,
        //                                             IsDeduct = key.IsDeduct
        //                                         }).ToList();
        //    bool bFlag = false;
        //    bool bFlagTwo = false;
        //    bool bIsNewPage = false;
        //    foreach (ExportPIDetail oItem in oExportPIDetailFab)
        //    {
        //        oExportPIDetailStyleNo = new List<ExportPIDetail>();
        //        //oExportPIDetailColor = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.MUnitID == oItem.MUnitID && x.IsDeduct == oItem.IsDeduct).ToList();
        //        oExportPIDetailStyleNo = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct).ToList();
        //        bFlag = true;
           
        //        sTemp = "";
        //        sConst = "";
        //        if (oItem.ProcessType != 0)
        //        {
        //            oItem.ProductName = oItem.ProductName + ", " + oItem.ProcessTypeName;
        //        }
        //        if (oItem.MUnitID > 1)
        //        {
        //            oItem.ProductName = oItem.ProductName + " Fabrics";
        //        }
        //        else
        //        {
        //            if (oItem.IsDeduct)
        //            {
        //                oItem.ProductName = oItem.ProductName + "(Deduct)";
        //            }
        //            else
        //            {
        //                oItem.ProductName = oItem.ProductName;
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(oItem.Construction))
        //        {
        //            sConst = sConst + "Const: " + oItem.Construction;
        //        }

        //        if (oItem.FabricWeave != 0)
        //        {
        //            sTemp = sTemp + " " + oItem.FabricWeaveName;
        //        }

        //        if (!string.IsNullOrEmpty(oItem.FabricWidth))
        //        {
        //            sTemp = sTemp + ", Width : " + oItem.FabricWidth;
        //        }
        //        if (oItem.FinishType != 0)
        //        {
        //            sTemp = sTemp + ", Finish Type : " + oItem.FinishTypeName;
        //        }
        //        if (!string.IsNullOrEmpty(oItem.ExportQuality))
        //        {
        //            sTemp = sTemp + ", Shade : " + oItem.ExportQuality;
        //        }


        //        _oPhrase = new Phrase();
        //        _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
        //        //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
        //        _oPdfPCell = new PdfPCell(_oPhrase);

        //        //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, 0)));
        //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
        //        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
        //        //_oPdfPCell.Rowspan = _nCount_Raw;
        //        _oPdfPCell.Border = 0;
        //        _oPdfPCell.BorderWidthLeft = 0.5f;
        //        _oPdfPCell.BorderWidthTop = 0.5f;
        //        _oPdfPCell.BorderWidthBottom = 0;
        //        _oPdfPTable.AddCell(_oPdfPCell);

        //        //_oPdfPCell = new PdfPCell(new Phrase(oItem.ExportQuality, _oFontStyle));
        //        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        ////_oPdfPCell.Rowspan = _nCount_Raw;
        //        //_oPdfPCell.Border = 0;
        //        //_oPdfPCell.BorderWidthLeft = 0.5f;
        //        //_oPdfPCell.BorderWidthTop = 0.5f;
        //        //_oPdfPCell.BorderWidthBottom = 0;
        //        //_oPdfPTable.AddCell(_oPdfPCell);

            
        //        oExportPIDetailStyleNo=oExportPIDetailStyleNo.GroupBy(x => new { x.StyleNo}, (key, grp) =>
        //                                         new ExportPIDetail
        //                                         {
        //                                             StyleNo = key.StyleNo,
        //                                         }).ToList();

        //        oExportPIDetailStyleNo = oExportPIDetailStyleNo.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
        //        foreach (ExportPIDetail oItem2 in oExportPIDetailStyleNo)
        //        {
        //            //oExportPIDetailColors = _oExportPIDetails.Where(x => x.StyleNo == oItem2.StyleNo).ToList();
        //            oExportPIDetailColors = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.StyleNo == oItem2.StyleNo  && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct).ToList();
        //            if (!bFlag)
        //            {
        //                //if (bIsNewPage)
        //                //{
        //                //    _oPhrase = new Phrase();
        //                //    _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
        //                //    //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
        //                //    _oPdfPCell = new PdfPCell(_oPhrase);
        //                //}
        //                //else
        //                //{
        //                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                //}

        //                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                _oPdfPCell.Border = 0;
        //                _oPdfPCell.BorderWidthLeft = 0.5f;
        //                if (bIsNewPage)
        //                {
        //                    _oPdfPCell.BorderWidthTop = 0.5f;
        //                }
        //                else
        //                {
        //                    _oPdfPCell.BorderWidthTop = 0;
        //                }
        //                _oPdfPCell.BorderWidthBottom = 0;
        //                _oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                _oPdfPCell.Border = 0;
        //                _oPdfPCell.BorderWidthLeft = 0.5f;
        //                if (bIsNewPage)
        //                {
        //                    _oPdfPCell.BorderWidthTop = 0.5f;
        //                }
        //                else
        //                {
        //                    _oPdfPCell.BorderWidthTop = 0;
        //                }
        //                _oPdfPCell.BorderWidthBottom = 0;
        //                _oPdfPTable.AddCell(_oPdfPCell);
        //            }

        //            bFlag = false;
                   
        //                //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
        //                _oPdfPCell = new PdfPCell(new Phrase(oItem2.StyleNo, _oFontStyle));
        //                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                _oPdfPCell.Border = 0;
        //                _oPdfPCell.BorderWidthLeft = 0.5f;
        //                _oPdfPCell.BorderWidthTop = 0.5f;
        //                _oPdfPCell.BorderWidthBottom = 0;
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                _oPdfPTable.AddCell(_oPdfPCell);

        //                oExportPIDetailColors = oExportPIDetailColors.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
        //                bFlagTwo = true;
        //                foreach (ExportPIDetail oItem3 in oExportPIDetailColors)
        //             {
        //                 if (!bFlagTwo)
        //                 {
        //                     _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                     _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                     _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                     _oPdfPCell.Border = 0;
        //                     _oPdfPCell.BorderWidthLeft = 0.5f;
        //                     _oPdfPCell.BorderWidthTop = 0;
        //                     _oPdfPCell.BorderWidthBottom = 0;
        //                     _oPdfPTable.AddCell(_oPdfPCell);

        //                     //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                     //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                     //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                     //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                     //_oPdfPCell.Border = 0;
        //                     //_oPdfPCell.BorderWidthLeft = 0.5f;
        //                     //_oPdfPCell.BorderWidthTop = 0;
        //                     //_oPdfPCell.BorderWidthBottom = 0;
        //                     //_oPdfPTable.AddCell(_oPdfPCell);

        //                     //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
        //                     _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                     _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                     _oPdfPCell.Border = 0;
        //                     _oPdfPCell.BorderWidthLeft = 0.5f;
        //                     _oPdfPCell.BorderWidthTop = 0;
        //                     _oPdfPCell.BorderWidthBottom = 0;
        //                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                     _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                     _oPdfPTable.AddCell(_oPdfPCell);

        //                 }
        //                 bFlagTwo = false;
        //                 _oPdfPCell = new PdfPCell(new Phrase(oItem3.ColorInfo, _oFontStyle));
        //                 _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                 //_oPdfPCell.MinimumHeight = 40f;
        //                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                 _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                 _oPdfPTable.AddCell(_oPdfPCell);

        //                 _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem3.Qty), _oFontStyle));
        //                 _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                 //_oPdfPCell.MinimumHeight = 40f;
        //                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                 _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                 _oPdfPTable.AddCell(_oPdfPCell);

        //                 _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem3.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
        //                 //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
        //                 _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                 //_oPdfPCell.MinimumHeight = 40f;
        //                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                 _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                 _oPdfPTable.AddCell(_oPdfPCell);
        //                 if (oItem.IsDeduct)
        //                 {
        //                     //_oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
        //                     _oPdfPCell = new PdfPCell(new Phrase("(" + _oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice) + ")", _oFontStyle));
        //                 }
        //                 else
        //                 {
        //                     _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice), _oFontStyle));
        //                 }

        //                 _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                 //_oPdfPCell.MinimumHeight = 40f;
        //                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                 _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        
        //                 _oPdfPTable.AddCell(_oPdfPCell);
        //                 _oPdfPTable.CompleteRow();

        //                 bIsNewPage = false;
        //                 _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
        //                 if (_nUsagesHeight > 775)
        //                 {
        //                     _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
        //                     _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


        //                     _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //                     _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //                     _oPdfPCell.Colspan = _nTotalColumn;
        //                     _oPdfPCell.Border = 0;
        //                     _oPdfPCell.BorderWidthLeft = 0;
        //                     _oPdfPCell.BorderWidthRight = 0;
        //                     _oPdfPCell.BorderWidthTop = 0.5f;
        //                     _oPdfPCell.BorderWidthBottom = 0;
        //                     _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //                     _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //                     _oPdfPTable.AddCell(_oPdfPCell);
        //                     _oPdfPTable.CompleteRow();

        //                     _nUsagesHeight = 0;
        //                     _oDocument.Add(_oPdfPTable);
        //                     _oDocument.NewPage();
        //                     //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
        //                     //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
        //                     //_oDocument.SetMargins(35f, 15f, 5f, 30f);
        //                     //oPdfPTable.DeleteBodyRows();
        //                     _oPdfPTable.DeleteBodyRows();
        //                     _nTotalColumn = 6;
        //                     _oPdfPTable = new PdfPTable(_nTotalColumn);
        //                     _oPdfPTable.SetWidths(new float[] { 210f,  140f, 140f, 70f, 70f, 75f });
        //                     _oPdfPTable.WidthPercentage = 100;
        //                     _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            
        //                     this.PrintHeader();
        //                     this.ReporttHeader();
        //                     //isNewPage = true;
        //                     //this.WaterMark(35f, 15f, 5f, 30f);
        //                     _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
        //                     _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
        //                     bIsNewPage = true;
        //                 }
        //             }

        //        }

        //        //oPdfPTable = new PdfPTable(7);
        //        //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });



        //        //if (oItem.IsDeduct) { nTotalValue = nTotalValue - oItem2.Qty * oItem2.UnitPrice; }
        //        //else { nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice; nTotalQty += oItem.Qty; }

        //        //oItem.OrderSheetDetailID = -1;

        //        //sTotalWidth = oItem.FabricWidth;
        //        //sMUName = oItem.MUName;
        //        //nProductID = oItem.ProductID;
        //        //nProcessType = (int)oItem.ProcessType;

        //        //sConstruction = oItem.Construction;
        //        //sStyleNo = oItem.StyleNo;
        //        //sBuyerReference = oItem.BuyerReference;

        //        //sFabricWeaveName = oItem.FabricWeaveName;
        //        //sFabricWidth = oItem.FabricWidth;
        //        //sFinishTypeName = oItem.FinishTypeName;

        //        //_nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
               

        //        //_oPdfPCell = new PdfPCell(_oPdfPTable);
        //        //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //        //_oPdfPTable.CompleteRow();

        //    }
        //    nTotalQty = _oExportPIDetails.Where(c => c.MUnitID>1).Sum(x => x.Qty);
        //    //nTotalQty = _oExportPIDetails.Sum(x => x.Qty);
        //    nTotalValue = _oExportPIDetails.Where(c => c.IsDeduct == false).Sum(x => x.Qty * x.UnitPrice);
        //    nTotalValue =nTotalValue- _oExportPIDetails.Where(c => c.IsDeduct == true).Sum(x => x.Qty * x.UnitPrice);
        //    //oPdfPTable = new PdfPTable(7);
        //    //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

        //    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle_Bold));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + (_bIsInYard ? " Y" : "M"), _oFontStyle_Bold));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(nTotalValue), _oFontStyle_Bold));
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    //_oPdfPCell.MinimumHeight = 40f;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //    _oPdfPTable.AddCell(_oPdfPCell);


        //    if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
        //    {
        //        _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
        //    }
        //    else
        //    {
        //        _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.DollarWords(nTotalValue), _oFontStyle));
        //    }
        //    _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    //_oPdfPCell = new PdfPCell(oPdfPTable);
        //    //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    //_oPdfPTable.CompleteRow();

         
        //    #endregion

        //    #endregion

            
        //}
    
        private void PrintBody2_WUOne()
        {
            #region Terms & Conditions Data :

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            if (_nUsagesHeight > 490)
            {
                _nUsagesHeight = 0;
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                //oPdfPTable.DeleteBodyRows();
                _oPdfPTable.DeleteBodyRows();
                //_nTotalColumn = 7;
                //_oPdfPTable = new PdfPTable(_nTotalColumn);
                //_oPdfPTable.WidthPercentage = 100;
                //_oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                this.PrintHeader();
                this.ReporttHeader();
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            }


            PdfPTable oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);
            int i = 0;

            foreach (ExportPITandCClause oItem in _oExportPITandCClauses)
            {

                oTermsAndConditionTable = new PdfPTable(4);
                oTermsAndConditionTable.WidthPercentage = 100;
                oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString("00") + ".", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CaptionName, _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("" + oItem.TermsAndCondition, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                oTermsAndConditionTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
                _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }


            oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            /// Total Hight -Margian -Uses Height
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 800 - 100 - 30 - _nUsagesHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        

            #region AdviseBankTable
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetAdviseBankTable_AccountNo()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion



            #region Authorized Signature
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 2f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.AcceptanceBy, _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.For, _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            if (_oExportPI.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oExportPI.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(80f, 70f);

                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.ExportPIPrintSetup.AcceptanceBy == "" || _oExportPI.ExportPIPrintSetup.AcceptanceBy == "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oPdfPCell = new PdfPCell(this.PrintNote_WU());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();





            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_nTitleTypeInImg == 3)//imge
            {
                _oPdfPCell = new PdfPCell(this.AddBU_FooderLogo());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        private void PrintBody2_WUTwo()
        {
            #region Terms & Conditions Data :

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            if (_nUsagesHeight > 490)
            {
                _nUsagesHeight = 0;
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                //oPdfPTable.DeleteBodyRows();
                _oPdfPTable.DeleteBodyRows();
                //_nTotalColumn = 7;
                //_oPdfPTable = new PdfPTable(_nTotalColumn);
                //_oPdfPTable.WidthPercentage = 100;
                //_oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                this.PrintHeader();
                this.ReporttHeader();
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            }


            PdfPTable oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);
            int i = 0;
            ExportPITandCClause oExportPITandCClause = new ExportPITandCClause();
            string sTerms = "";
            if (_oExportPI.PaymentType == EnumPIPaymentType.LC)
            {
                if (_oExportPI.LCTermID > 0)
                {
                    sTerms = "L/C Payment: Irrevocable " + _oExportPI.LCTermsName + ".";
                }
                if (_oExportPI.IsBBankFDD)
                {
                    sTerms = sTerms + " Letter of credit. 100% payment will be made in US Dollar by Bangladesh Bank’s FDD.";
                }

                if (!String.IsNullOrEmpty(sTerms))
                {
                    oExportPITandCClause = new ExportPITandCClause();
                    oExportPITandCClause.ExportTnCCaptionID = 0;
                    oExportPITandCClause.ExportPITandCClauseID = 0;
                    oExportPITandCClause.CaptionName = "Payment";
                    oExportPITandCClause.TermsAndCondition = sTerms;
                    _oExportPITandCClauses.Add(oExportPITandCClause);
                }
                sTerms = "";
                if (_oExportPI.IsLIBORRate)
                {
                    sTerms = "Interest has to be paid at LIBOR rate for Usance period";
                    if (_oExportPI.OverdueRate > 0)
                    {
                        sTerms = sTerms + " and ";
                    }
                    else
                    {
                        sTerms = sTerms + ".";
                    }

                }
                if (_oExportPI.OverdueRate > 0)
                {
                    sTerms = sTerms + "Overdue interest has to be paid at @" + _oExportPI.OverdueRate.ToString() + " % P.A. for overdue period.";
                }
                if (!String.IsNullOrEmpty(sTerms))
                {
                    oExportPITandCClause = new ExportPITandCClause();
                    oExportPITandCClause.ExportTnCCaptionID = 0;
                    oExportPITandCClause.ExportPITandCClauseID = 0;
                    oExportPITandCClause.CaptionName = "Interest";
                    oExportPITandCClause.TermsAndCondition = sTerms;
                    _oExportPITandCClauses.Add(oExportPITandCClause);
                }
            }
            else
            {
                if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)
                {
                    sTerms = "Payment will be made within BDT by 100% irrevocable. " + _oExportPI.LCTermsName;
                }
                else
                { sTerms = "Payment will be made within US dollar by 100% irrevocable. " + _oExportPI.LCTermsName; }
                if (!String.IsNullOrEmpty(sTerms))
                {
                    oExportPITandCClause = new ExportPITandCClause();
                    oExportPITandCClause.ExportTnCCaptionID = 0;
                    oExportPITandCClause.ExportPITandCClauseID = 0;
                    oExportPITandCClause.CaptionName = "Payment";
                    oExportPITandCClause.TermsAndCondition = sTerms;
                    _oExportPITandCClauses.Add(oExportPITandCClause);
                }
            }

            _oExportPITandCClauses = _oExportPITandCClauses.OrderBy(o => o.ExportPITandCClauseID).ToList();
            foreach (ExportPITandCClause oItem in _oExportPITandCClauses)
            {

                oTermsAndConditionTable = new PdfPTable(4);
                oTermsAndConditionTable.WidthPercentage = 100;
                oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString("00") + ".", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CaptionName, _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("" + oItem.TermsAndCondition, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                oTermsAndConditionTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
                _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }


            oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            /// Total Hight -Margian -Uses Height
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 800 - 100 - 30 - _nUsagesHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion



            #region AdviseBankTable
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetAdviseBankTable_AccountNo()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion



            #region Authorized Signature
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 2f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.AcceptanceBy, _oFontStyle));
            _oPdfPCell.FixedHeight = 45f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.For, _oFontStyle));
            _oPdfPCell.FixedHeight = 45f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.MinimumHeight = 45f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            if (_oExportPI.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oExportPI.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(45f, 35f);

                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.ExportPIPrintSetup.AcceptanceBy == "" || _oExportPI.ExportPIPrintSetup.AcceptanceBy == "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oPdfPCell = new PdfPCell(this.PrintNote_WU());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();





            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_nTitleTypeInImg == 3)//imge
            {
                _oPdfPCell = new PdfPCell(this.AddBU_FooderLogo());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        private void PrintBodyProduct_WUTwo()
        {
            #region  Details
            #region Detail Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);

            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }

            //PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

            oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle_Bold));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Style", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Qty(" + (_bIsInYard ? "Yard" : "Meter") + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            _oPdfPTable.CompleteRow();


            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion

            string sTemp = "";
            string sConst = "";
            //string sTotalWidth = "";
            //string sConstruction = "";
            //int nProductID = 0;
            //int nProcessType = 0;
            //string sFabricWeaveName = "";
            //string sFabricWidth = "";
            //string sFinishTypeName = "";
            //int _nCount_Raw = 0;
            //int _nCount_Raw_Style = 0;

            //string sStyleNo = "";
            //string sBuyerReference = "";

            //foreach (ExportPIDetail oItem in _oExportPIDetails)
            //{
            //    oItem.Shrinkage = (String.IsNullOrEmpty(oItem.Weight) ? "" : oItem.Weight.Trim()); 
            //    oItem.Weight = (String.IsNullOrEmpty(oItem.Weight)?"":oItem.Weight.Trim());
            //}

            _oExportPIDetails = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.FabricNo, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.StyleNo, x.ColorInfo, x.UnitPrice, x.BuyerReference, x.MUnitID, x.ExportQuality,x.ShadeType, x.IsDeduct, x.Weight, x.Shrinkage, x.ProductDescription }, (key, grp) =>
                                                  new ExportPIDetail
                                                  {
                                                      FabricNo = key.FabricNo,
                                                      ProductID = key.ProductID,
                                                      ProductName = key.ProductName,
                                                      Construction = key.Construction,
                                                      ProcessType = key.ProcessType,
                                                      ProcessTypeName = key.ProcessTypeName,
                                                      FabricWeave = key.FabricWeave,
                                                      FabricWeaveName = key.FabricWeaveName,
                                                      FabricWidth = key.FabricWidth,
                                                      FinishTypeName = key.FinishTypeName,
                                                      FinishType = key.FinishType,
                                                      StyleNo = key.StyleNo,
                                                      ColorInfo = key.ColorInfo,
                                                      BuyerReference = key.BuyerReference,
                                                      Qty = grp.Sum(p => p.Qty),
                                                      UnitPrice = key.UnitPrice,
                                                      MUnitID = key.MUnitID,
                                                      ExportQuality = key.ExportQuality,
                                                      ShadeType = key.ShadeType,
                                                      IsDeduct = key.IsDeduct,
                                                      Weight = key.Weight,
                                                      Shrinkage = key.Shrinkage,
                                                      ProductDescription = key.ProductDescription
                                                  }).ToList();
            _oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessType).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
         
            //_oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessType).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
            //_oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ToList();
          
            #region Details Data
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
            nTotalQty = _oExportPIDetails.Where(c => c.MUnitID > 1).Sum(x => x.Qty);
            nTotalValue = _oExportPIDetails.Where(c => c.IsDeduct == false).Sum(x => x.Qty * x.UnitPrice);
            nTotalValue = nTotalValue - _oExportPIDetails.Where(c => c.IsDeduct == true).Sum(x => x.Qty * x.UnitPrice);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            List<ExportPIDetail> oExportPIDetailFab = new List<ExportPIDetail>();
            List<ExportPIDetail> oExportPIDetailColors = new List<ExportPIDetail>();
            List<ExportPIDetail> oExportPIDetailStyleNo = new List<ExportPIDetail>();

            oExportPIDetailFab = _oExportPIDetails.GroupBy(x => new { x.FabricNo, x.ProductID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.IsDeduct, x.ExportQuality, x.ShadeType, x.Shrinkage, x.Weight }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     ProductID = key.ProductID,
                                                     FabricNo = key.FabricNo,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessType = key.ProcessType,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeave = key.FabricWeave,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     FinishType = key.FinishType,
                                                     Weight = key.Weight,
                                                     Shrinkage = key.Shrinkage,
                                                     ShadeType = key.ShadeType,
                                                    
                                                     ExportQuality = key.ExportQuality,
                                                     IsDeduct = key.IsDeduct
                                                 }).ToList();
            bool bFlag = false;
            bool bFlagTwo = false;
            bool bIsNewPage = false;
            foreach (ExportPIDetail oItem in oExportPIDetailFab)
            {
                oExportPIDetailStyleNo = new List<ExportPIDetail>();
                //oExportPIDetailColor = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.MUnitID == oItem.MUnitID && x.IsDeduct == oItem.IsDeduct).ToList();
                oExportPIDetailStyleNo = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID & x.FabricNo==oItem.FabricNo  && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct).ToList();
                bFlag = true;

                sTemp = "";
                sConst = "";
                if (oItem.FabricWeave != 0)
                {
                    oItem.ProductName = "Comp:" + oItem.ProductName + ", Weave: " + oItem.FabricWeaveName;
                }
                if (!String.IsNullOrEmpty(oItem.FabricNo))
                {
                    oItem.ProductName = "Article: " + oItem.FabricNo + "\n" + oItem.ProductName;
                }
                //else
                //{
                if (oItem.IsDeduct)
                {
                    oItem.ProductName = oItem.ProductName + "(Deduct)";
                }
                //else
                //{
                //    oItem.ProductName = oItem.ProductName;
                //}
                //}
                if (!string.IsNullOrEmpty(oItem.Construction))
                {
                    sConst = sConst + "Const: " + oItem.Construction;
                }
                if (!string.IsNullOrEmpty(oItem.FabricWidth))
                {
                    sTemp = sTemp + "Width : " + oItem.FabricWidth;
                }
                if (!string.IsNullOrEmpty(oItem.Weight))
                {
                    sTemp = sTemp + " Weight : " + oItem.Weight;
                }
                if (oItem.ProcessType != 0)
                {
                    sTemp = sTemp + "\nProcess: " + oItem.ProcessTypeName;
                }
                if (oItem.FinishType != 0)
                {
                    sTemp = sTemp + ", Finish: " + oItem.FinishTypeName;
                }
                if (!string.IsNullOrEmpty(oItem.Shrinkage))
                {
                    sTemp = sTemp + "\nShrinkage: " + oItem.Shrinkage;
                }
                if (!string.IsNullOrEmpty(oItem.ProductDescription))
                {
                    sTemp = sTemp + "\n" + oItem.ProductDescription;
                }
                if (oItem.ShadeType != EnumDepthOfShade.None)
                {
                    sTemp = sTemp + "\nShade:" + oItem.ShadeType.ToString();
                }

                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + " " + sTemp, _oFontStyle));
                //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell = new PdfPCell(_oPhrase);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, 0)));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.Rowspan = oExportPIDetailStyleNo.Count;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.ExportQuality, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                ////_oPdfPCell.Rowspan = _nCount_Raw;
                //_oPdfPCell.Border = 0;
                //_oPdfPCell.BorderWidthLeft = 0.5f;
                //_oPdfPCell.BorderWidthTop = 0.5f;
                //_oPdfPCell.BorderWidthBottom = 0;
                //_oPdfPTable.AddCell(_oPdfPCell);

                oExportPIDetailStyleNo = oExportPIDetailStyleNo.GroupBy(x => new { x.StyleNo,x.BuyerReference }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     StyleNo = key.StyleNo,
                                                     BuyerReference = key.BuyerReference
                                                 }).ToList();

                oExportPIDetailStyleNo = oExportPIDetailStyleNo.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(b => b.BuyerReference).ThenBy(c => c.ColorInfo).ToList();
                foreach (ExportPIDetail oItem2 in oExportPIDetailStyleNo)
                {
                    //oExportPIDetailColors = _oExportPIDetails.Where(x => x.StyleNo == oItem2.StyleNo).ToList();
                    oExportPIDetailColors = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.FabricNo==oItem.FabricNo   && x.StyleNo == oItem2.StyleNo && x.BuyerReference == oItem2.BuyerReference && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct).ToList();
                    if (!bFlag)
                    {
                        //if (bIsNewPage)
                        //{
                        //    _oPhrase = new Phrase();
                        //    _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                        //    //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                        //    _oPdfPCell = new PdfPCell(_oPhrase);
                        //}
                        //else
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //}

                        //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //_oPdfPCell.Border = 0;
                        //_oPdfPCell.BorderWidthLeft = 0.5f;
                        //if (bIsNewPage)
                        //{
                        //    _oPdfPCell.BorderWidthTop = 0.5f;
                        //}
                        //else
                        //{
                        //    _oPdfPCell.BorderWidthTop = 0;
                        //}
                        //_oPdfPCell.BorderWidthBottom = 0;
                        //_oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //_oPdfPCell.Border = 0;
                        //_oPdfPCell.BorderWidthLeft = 0.5f;
                        //if (bIsNewPage)
                        //{
                        //    _oPdfPCell.BorderWidthTop = 0.5f;
                        //}
                        //else
                        //{
                        //    _oPdfPCell.BorderWidthTop = 0;
                        //}
                        //_oPdfPCell.BorderWidthBottom = 0;
                        //_oPdfPTable.AddCell(_oPdfPCell);
                    }

                    bFlag = false;
                    //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oItem2.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Rowspan = oExportPIDetailColors.Count;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem2.BuyerReference, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.Rowspan = oExportPIDetailColors.Count;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    oExportPIDetailColors = oExportPIDetailColors.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(b => b.BuyerReference).ThenBy(c => c.ColorInfo).ToList();
                    bFlagTwo = true;
                    foreach (ExportPIDetail oItem3 in oExportPIDetailColors)
                    {

                       // _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.StyleNo == oItem2.StyleNo && x.BuyerReference == oItem2.BuyerReference && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct).ToList();

                        _oExportPIDetails.RemoveAll(x => x.ProductID == oItem3.ProductID && x.FabricNo==oItem.FabricNo &&  x.ColorInfo == oItem3.ColorInfo && x.StyleNo == oItem2.StyleNo && x.StyleNo == oItem2.StyleNo && x.BuyerReference == oItem3.BuyerReference && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct);
                        if (!bFlagTwo)
                        {
                            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //_oPdfPCell.Border = 0;
                            //_oPdfPCell.BorderWidthLeft = 0.5f;
                            //_oPdfPCell.BorderWidthTop = 0;
                            //_oPdfPCell.BorderWidthBottom = 0;
                            //_oPdfPTable.AddCell(_oPdfPCell);

                            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //_oPdfPCell.Border = 0;
                            //_oPdfPCell.BorderWidthLeft = 0.5f;
                            //_oPdfPCell.BorderWidthTop = 0;
                            //_oPdfPCell.BorderWidthBottom = 0;
                            //_oPdfPTable.AddCell(_oPdfPCell);

                            ////_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            //_oPdfPCell.Border = 0;
                            //_oPdfPCell.BorderWidthLeft = 0.5f;
                            //_oPdfPCell.BorderWidthTop = 0;
                            //_oPdfPCell.BorderWidthBottom = 0;
                            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //_oPdfPTable.AddCell(_oPdfPCell);

                        }
                        bFlagTwo = false;
                        _oPdfPCell = new PdfPCell(new Phrase(oItem3.ColorInfo, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem3.Qty), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem3.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                        //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        if (oItem.IsDeduct)
                        {
                            //_oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                            _oPdfPCell = new PdfPCell(new Phrase("(" + _oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice) + ")", _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice), _oFontStyle));
                        }

                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        bIsNewPage = false;
                        _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                        if (_nUsagesHeight > 700)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.Colspan = _nTotalColumn;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0;
                            _oPdfPCell.BorderWidthRight = 0;
                            _oPdfPCell.BorderWidthTop = 0.5f;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();

                            _nUsagesHeight = 0;
                            _oDocument.Add(_oPdfPTable);
                            _oDocument.NewPage();
                            //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
                            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
                            //_oDocument.SetMargins(35f, 15f, 5f, 30f);
                            //oPdfPTable.DeleteBodyRows();
                            _oPdfPTable.DeleteBodyRows();
                            _nTotalColumn = 7;
                            _oPdfPTable = new PdfPTable(_nTotalColumn);
                            _oPdfPTable.SetWidths(new float[] { 180f, 105f, 100f, 120f, 70f, 70f, 75f });
                            _oPdfPTable.WidthPercentage = 100;
                            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            this.PrintHeader();
                            this.ReporttHeader();
                            //isNewPage = true;
                            //this.WaterMark(35f, 15f, 5f, 30f);
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                            bIsNewPage = true;
                        }
                    }

                }

                //oPdfPTable = new PdfPTable(7);
                //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });



                //if (oItem.IsDeduct) { nTotalValue = nTotalValue - oItem2.Qty * oItem2.UnitPrice; }
                //else { nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice; nTotalQty += oItem.Qty; }

                //oItem.OrderSheetDetailID = -1;

                //sTotalWidth = oItem.FabricWidth;
                //sMUName = oItem.MUName;
                //nProductID = oItem.ProductID;
                //nProcessType = (int)oItem.ProcessType;

                //sConstruction = oItem.Construction;
                //sStyleNo = oItem.StyleNo;
                //sBuyerReference = oItem.BuyerReference;

                //sFabricWeaveName = oItem.FabricWeaveName;
                //sFabricWidth = oItem.FabricWidth;
                //sFinishTypeName = oItem.FinishTypeName;

                //_nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);


                //_oPdfPCell = new PdfPCell(_oPdfPTable);
                //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();

            }
            //nTotalQty = _oExportPIDetails.Where(c => c.MUnitID > 1).Sum(x => x.Qty);
            ////nTotalQty = _oExportPIDetails.Sum(x => x.Qty);
            //nTotalValue = _oExportPIDetails.Where(c => c.IsDeduct == false).Sum(x => x.Qty * x.UnitPrice);
            //nTotalValue = nTotalValue - _oExportPIDetails.Where(c => c.IsDeduct == true).Sum(x => x.Qty * x.UnitPrice);
            //oPdfPTable = new PdfPTable(7);
            //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + (_bIsInYard ? " Y" : "M"), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(nTotalValue), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);


            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.DollarWords(nTotalValue), _oFontStyle));
            }
            _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            #endregion

            #endregion


        }

        private void PrintBodyProduct_WUOne()
        {
            #region  Details
            #region Detail Column Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, 1);

            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }

            //PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

            oPdfPCell = new PdfPCell(new Phrase("Description of Goods", _oFontStyle_Bold));
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Style/Buyer Ref", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Qty(" + (_bIsInYard ? "Yard" : "Meter") + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Unit Price(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportPI.Currency + ")", _oFontStyle_Bold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(oPdfPCell);

            _oPdfPTable.CompleteRow();


            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion

            string sTemp = "";
            string sConst = "";
           


            _oExportPIDetails = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.StyleNo, x.ColorInfo, x.UnitPrice, x.BuyerReference, x.MUnitID, x.ExportQuality, x.ShadeType, x.IsDeduct }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessType = key.ProcessType,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeave = key.FabricWeave,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     FinishType = key.FinishType,
                                                     StyleNo = key.StyleNo,
                                                     ColorInfo = key.ColorInfo,
                                                     BuyerReference = key.BuyerReference,
                                                     Qty = grp.Sum(p => p.Qty),
                                                     UnitPrice = key.UnitPrice,
                                                     MUnitID = key.MUnitID,
                                                     ExportQuality = key.ExportQuality,
                                                     ShadeType = key.ShadeType,
                                                     IsDeduct = key.IsDeduct
                                                 }).ToList();

            foreach (ExportPIDetail oItem in _oExportPIDetails)
            {
                if (String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    oItem.StyleNo = oItem.StyleNo;
                }
                else
                {
                    oItem.StyleNo = oItem.StyleNo + " " + oItem.BuyerReference;
                }
            }
            _oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ThenBy(a => a.Construction).ThenBy(a => a.ProcessType).ThenBy(a => a.FabricWeaveName).ThenBy(a => a.FabricWidth).ThenBy(a => a.FinishTypeName).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
            //_oExportPIDetails = _oExportPIDetails.OrderBy(o => o.ProductID).ToList();
            #region Details Data
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            List<ExportPIDetail> oExportPIDetailFab = new List<ExportPIDetail>();
            List<ExportPIDetail> oExportPIDetailColors = new List<ExportPIDetail>();
            List<ExportPIDetail> oExportPIDetailStyleNo = new List<ExportPIDetail>();

            oExportPIDetailFab = _oExportPIDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.Construction, x.ProcessType, x.ProcessTypeName, x.FabricWeave, x.FabricWeaveName, x.FabricWidth, x.FinishTypeName, x.FinishType, x.IsDeduct, x.ExportQuality,x.ShadeType }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     ProductID = key.ProductID,
                                                     ProductName = key.ProductName,
                                                     Construction = key.Construction,
                                                     ProcessType = key.ProcessType,
                                                     ProcessTypeName = key.ProcessTypeName,
                                                     FabricWeave = key.FabricWeave,
                                                     FabricWeaveName = key.FabricWeaveName,
                                                     FabricWidth = key.FabricWidth,
                                                     FinishTypeName = key.FinishTypeName,
                                                     FinishType = key.FinishType,
                                                     //StyleNo = key.StyleNo,
                                                     //ColorInfo = key.ColorInfo,
                                                     //BuyerReference = key.BuyerReference,
                                                     //Qty = grp.Sum(p => p.Qty),
                                                     //UnitPrice = key.UnitPrice,
                                                     //MUnitID = key.MUnitID,
                                                     ShadeType = key.ShadeType,
                                                     ExportQuality = key.ExportQuality,
                                                     IsDeduct = key.IsDeduct
                                                 }).ToList();
            bool bFlag = false;
            bool bFlagTwo = false;
            bool bIsNewPage = false;
            oExportPIDetailFab = oExportPIDetailFab.OrderByDescending(o => o.Construction).ToList();
            foreach (ExportPIDetail oItem in oExportPIDetailFab)
            {
                oExportPIDetailStyleNo = new List<ExportPIDetail>();
                //oExportPIDetailColor = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.MUnitID == oItem.MUnitID && x.IsDeduct == oItem.IsDeduct).ToList();
                oExportPIDetailStyleNo = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.ExportQuality == oItem.ExportQuality && x.ShadeType == oItem.ShadeType && x.ProductName == oItem.ProductName && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct).ToList();
                bFlag = true;

                sTemp = "";
                sConst = "";
                if (oItem.ProcessType != 0)
                {
                    oItem.ProductName = oItem.ProductName + ", " + oItem.ProcessTypeName;
                }
                if (oItem.MUnitID > 1)
                {
                    oItem.ProductName = oItem.ProductName + " Fabrics";
                }
                else
                {
                    if (oItem.IsDeduct)
                    {
                        oItem.ProductName = oItem.ProductName + "(Deduct)";
                    }
                    else
                    {
                        oItem.ProductName = oItem.ProductName;
                    }
                }
                if (!string.IsNullOrEmpty(oItem.Construction))
                {
                    sConst = sConst + "Const: " + oItem.Construction;
                }

                if (oItem.FabricWeave != 0)
                {
                    sTemp = sTemp + " " + oItem.FabricWeaveName;
                }

                if (!string.IsNullOrEmpty(oItem.FabricWidth))
                {
                    sTemp = sTemp + ", Width : " + oItem.FabricWidth;
                }
                if (oItem.FinishType != 0)
                {
                    sTemp = sTemp + ", Finish Type : " + oItem.FinishTypeName;
                }



                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                _oPdfPCell = new PdfPCell(_oPhrase);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, 0)));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                //_oPdfPCell.Rowspan = _nCount_Raw;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.ShadeType != EnumDepthOfShade.None)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ShadeType.ToString(), _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportQuality, _oFontStyle));
                }
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_oPdfPCell.Rowspan = _nCount_Raw;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthTop = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0;
                _oPdfPTable.AddCell(_oPdfPCell);


                oExportPIDetailStyleNo = oExportPIDetailStyleNo.GroupBy(x => new { x.StyleNo }, (key, grp) =>
                                                 new ExportPIDetail
                                                 {
                                                     StyleNo = key.StyleNo,
                                                 }).ToList();

                oExportPIDetailStyleNo = oExportPIDetailStyleNo.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
                foreach (ExportPIDetail oItem2 in oExportPIDetailStyleNo)
                {
                    //oExportPIDetailColors = _oExportPIDetails.Where(x => x.StyleNo == oItem2.StyleNo).ToList();
                    oExportPIDetailColors = _oExportPIDetails.Where(x => x.ProductID == oItem.ProductID && x.StyleNo == oItem2.StyleNo && x.Construction == oItem.Construction && x.ProcessType == oItem.ProcessType && x.ProcessTypeName == oItem.ProcessTypeName && x.FabricWeave == oItem.FabricWeave && x.FabricWeaveName == oItem.FabricWeaveName && x.FabricWidth == oItem.FabricWidth && x.FinishTypeName == oItem.FinishTypeName && x.FinishType == oItem.FinishType && x.IsDeduct == oItem.IsDeduct && x.ExportQuality == oItem.ExportQuality && x.ShadeType == oItem.ShadeType).ToList();
                    if (!bFlag)
                    {
                        //if (bIsNewPage)
                        //{
                        //    _oPhrase = new Phrase();
                        //    _oPhrase.Add(new Chunk(oItem.ProductName + ",\n" + sConst + "\n" + sTemp, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                        //    //_oPhrase.Add(new Chunk(" ["+oItem.ExportQuality+"]", FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL)));
                        //    _oPdfPCell = new PdfPCell(_oPhrase);
                        //}
                        //else
                        //{
                        //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        //}

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BorderWidthLeft = 0.5f;
                        if (bIsNewPage)
                        {
                            _oPdfPCell.BorderWidthTop = 0.5f;
                        }
                        else
                        {
                            _oPdfPCell.BorderWidthTop = 0;
                        }
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.BorderWidthLeft = 0.5f;
                        if (bIsNewPage)
                        {
                            _oPdfPCell.BorderWidthTop = 0.5f;
                        }
                        else
                        {
                            _oPdfPCell.BorderWidthTop = 0;
                        }
                        _oPdfPCell.BorderWidthBottom = 0;
                        _oPdfPTable.AddCell(_oPdfPCell);
                    }

                    bFlag = false;

                    //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oItem2.StyleNo, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.Border = 0;
                    _oPdfPCell.BorderWidthLeft = 0.5f;
                    _oPdfPCell.BorderWidthTop = 0.5f;
                    _oPdfPCell.BorderWidthBottom = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    oExportPIDetailColors = oExportPIDetailColors.OrderBy(o => o.ProductID).ThenBy(b => b.StyleNo).ThenBy(c => c.ColorInfo).ToList();
                    bFlagTwo = true;
                    foreach (ExportPIDetail oItem3 in oExportPIDetailColors)
                    {
                        if (!bFlagTwo)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPTable.AddCell(_oPdfPCell);

                            //_nCount_Raw = oExportPIDetailColors.Where(x => x.StyleNo == oItem2.StyleNo).Count();
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0.5f;
                            _oPdfPCell.BorderWidthTop = 0;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPTable.AddCell(_oPdfPCell);

                        }
                        bFlagTwo = false;
                        _oPdfPCell = new PdfPCell(new Phrase(oItem3.ColorInfo, _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem3.Qty), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem3.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                        //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.UnitPrice), _oFontStyle));
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        if (oItem.IsDeduct)
                        {
                            //_oPdfPCell = new PdfPCell(new Phrase(oItem.AmountSt, _oFontStyle));
                            _oPdfPCell = new PdfPCell(new Phrase("(" + _oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice) + ")", _oFontStyle));
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(oItem3.Qty * oItem3.UnitPrice), _oFontStyle));
                        }

                        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //_oPdfPCell.MinimumHeight = 40f;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();

                        bIsNewPage = false;
                        _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                        if (_nUsagesHeight > 775)
                        {
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            _oPdfPCell.Colspan = _nTotalColumn;
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.BorderWidthLeft = 0;
                            _oPdfPCell.BorderWidthRight = 0;
                            _oPdfPCell.BorderWidthTop = 0.5f;
                            _oPdfPCell.BorderWidthBottom = 0;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();

                            _nUsagesHeight = 0;
                            _oDocument.Add(_oPdfPTable);
                            _oDocument.NewPage();
                            //_oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
                            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
                            //_oDocument.SetMargins(35f, 15f, 5f, 30f);
                            //oPdfPTable.DeleteBodyRows();
                            _oPdfPTable.DeleteBodyRows();
                            _nTotalColumn = 7;
                            _oPdfPTable = new PdfPTable(_nTotalColumn);
                            _oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });
                            _oPdfPTable.WidthPercentage = 100;
                            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            this.PrintHeader();
                            this.ReporttHeader();
                            //isNewPage = true;
                            //this.WaterMark(35f, 15f, 5f, 30f);
                            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                            bIsNewPage = true;
                        }
                    }

                }

                //oPdfPTable = new PdfPTable(7);
                //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });



                //if (oItem.IsDeduct) { nTotalValue = nTotalValue - oItem2.Qty * oItem2.UnitPrice; }
                //else { nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice; nTotalQty += oItem.Qty; }

                //oItem.OrderSheetDetailID = -1;

                //sTotalWidth = oItem.FabricWidth;
                //sMUName = oItem.MUName;
                //nProductID = oItem.ProductID;
                //nProcessType = (int)oItem.ProcessType;

                //sConstruction = oItem.Construction;
                //sStyleNo = oItem.StyleNo;
                //sBuyerReference = oItem.BuyerReference;

                //sFabricWeaveName = oItem.FabricWeaveName;
                //sFabricWidth = oItem.FabricWidth;
                //sFinishTypeName = oItem.FinishTypeName;

                //_nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);


                //_oPdfPCell = new PdfPCell(_oPdfPTable);
                //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();

            }
            nTotalQty = _oExportPIDetails.Where(c => c.MUnitID > 1).Sum(x => x.Qty);
            //nTotalQty = _oExportPIDetails.Sum(x => x.Qty);
            nTotalValue = _oExportPIDetails.Where(c => c.IsDeduct == false).Sum(x => x.Qty * x.UnitPrice);
            nTotalValue = nTotalValue - _oExportPIDetails.Where(c => c.IsDeduct == true).Sum(x => x.Qty * x.UnitPrice);
            //oPdfPTable = new PdfPTable(7);
            //oPdfPTable.SetWidths(new float[] { 200f, 55f, 125f, 125f, 70f, 70f, 75f });

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty) + (_bIsInYard ? " Y" : "M"), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + Global.MillionFormat(nTotalValue), _oFontStyle_Bold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.MinimumHeight = 40f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPTable.AddCell(_oPdfPCell);


            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.DollarWords(nTotalValue), _oFontStyle));
            }
            _oPdfPCell.Colspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            #endregion

            #endregion


        }
    
#endregion

        #region Only Dyeing Four
        public byte[] PrepareReport_D(ExportPI oExportPI, Company oCompany, bool bPrintFormat, int nTitleTypeInImg, BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = oBusinessUnit;

            _oExportPI = oExportPI;
            _oExportPIDetails = oExportPI.ExportPIDetails;
            _oExportPITandCClauses = oExportPI.ExportPITandCClauses;
            _oExportPIPrintSetup = oExportPI.ExportPIPrintSetup;
            _oCompany = oCompany;
            _bIsInYard = bPrintFormat;
            _nTitleTypeInImg = nTitleTypeInImg;


            _oPdfPTable = new PdfPTable(5);
            _nTotalColumn = 5;
            _oPdfPTable.SetWidths(new float[] { 25f, 335f, 60f, 65f, 75f });

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(35f, 15f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            //ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.WaterMark(35f, 15f, 5f, 30f);
            this.PrintBody_D();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBody_D()
        {
            #region Buyer & PI Information
            #region Detail Table header
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.0f, 0);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);



            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region PI No & Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            if (_oExportPI.PaymentType == EnumPIPaymentType.NonLC)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Invoice No :   " + _oExportPI.PINo, _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("P.I.No :   " + _oExportPI.PINo, _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oExportPI.IssueDate.ToString("MMMM dd, yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region PO No & Date
            if (!string.IsNullOrEmpty(_oExportPI.OrderSheetNo))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
                if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Bill No :   " + _oExportPI.OrderSheetNo, _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("PO No :   " + _oExportPI.OrderSheetNo, _oFontStyle));
                }
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion

            #region Buyer & PI Information
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("APPLICANT/BUYER NAME:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BENEFICIARY:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.DeliveryToID > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Delivery to", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL | iTextSharp.text.Font.UNDERLINE)));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(new Phrase("VAT/BIN No: " + _oExportPI.ExportPIPrintSetup.BINNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oExportPI.DeliveryToID > 0)
            {
                Contractor oContractor = new Contractor();
                oContractor = oContractor.Get(_oExportPI.DeliveryToID, 0);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.DeliveryToName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oContractor.Address, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.Border = 0; 
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
            #endregion

            #region Transport Mode, Place of Shipment & Place of Destination
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 186.66f, 186.66f, 186.67f, 186.67f });

            _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT TERM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF SHIPMENT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MODE OF TRANSPORT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF DESTINATION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ShipmentTermSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfShipment, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.ShipmentBy, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfDelivery, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Dyeing
            if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Dyeing)
            {
                _oPdfPCell = new PdfPCell(this.ProductDetails_D());
            }
            else if (_oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Weaving || _oBusinessUnit.BusinessUnitType == EnumBusinessUnitType.Finishing)
            {
                _oPdfPCell = new PdfPCell(this.ProductDetails_WU());
            }
            else
            {
                _oPdfPCell = new PdfPCell(this.ProductDetails_WU());
            }
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
           
            #region Terms & Conditions Data :

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            if (_nUsagesHeight > 450)
            {


                _nUsagesHeight = 0;
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                oPdfPTable.DeleteBodyRows();
                _oPdfPTable.DeleteBodyRows();
                this.PrintHeader();
                this.ReporttHeader();
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            }

            ////_oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.NORMAL);
            ////_oPdfPCell = new PdfPCell(new Phrase("Conditions :", _oFontStyle));
            ////_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ////_oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            ////_oPdfPCell.BackgroundColor = BaseColor.WHITE;

            ////_oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(this.GetTermsAndConditionTable_Caption()));
            //_oPdfPCell.Colspan = _nTotalColumn;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            PdfPTable oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.BOLD);
            int i = 0;

            foreach (ExportPITandCClause oItem in _oExportPITandCClauses)
            {

                oTermsAndConditionTable = new PdfPTable(4);
                oTermsAndConditionTable.WidthPercentage = 100;
                oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString("00") + ".", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CaptionName, _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("" + oItem.TermsAndCondition, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 5;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);

                oTermsAndConditionTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
                _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }


            oTermsAndConditionTable = new PdfPTable(4);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                15, //Style Name 
                                                                52,  // Dept
                                                                5f,  //Shipment Date
                                                                345f, //Description/Composition 
                                                               
                                                          });

            _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);

            /// Total Hight -Margian -Uses Height
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 800 - 100 - 30 - _nUsagesHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oTermsAndConditionTable);
            _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region AdviseBankTable
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetAdviseBankTable_AccountNo()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Blank Cell
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 5f;
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Authorized Signature
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] {2f , 200f, 100f, 2f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 50f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.AcceptanceBy, _oFontStyle));
            _oPdfPCell.FixedHeight = 50f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.For, _oFontStyle));
            _oPdfPCell.FixedHeight = 50f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 50f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_oExportPI.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oExportPI.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(80f, 44f);

                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_oExportPI.ExportPIPrintSetup.AcceptanceBy == "" || _oExportPI.ExportPIPrintSetup.AcceptanceBy == "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oPdfPCell = new PdfPCell(this.PrintNote_DUD());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_nTitleTypeInImg == 3)//imge
            {
                _oPdfPCell = new PdfPCell(this.AddBU_FooderLogo());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
        }
        private PdfPTable ProductDetails_D()
        {
            #region Detail Column Header
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 20f, 150f, 68f, 72f, 60f, 62f, 60f, 62f, 74f });
            string sMUName = "";
            if (_oExportPIDetails.Count > 0)
            {
                sMUName = _oExportPIDetails[0].MUName;
            }
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //--DYED YARN FOR 100% EXPORT ORIENTED FACTORY
            _oPdfPCell = new PdfPCell(new Phrase("DYED YARN FOR 100% EXPORT ORIENTED FACTORY", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("POUND", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("UNIT PRICE(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("KILOGRAM", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION OF GOODS", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("P.P CONE(Pcs)", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("H.S.CODE", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("QTY(" + (_bIsInKg == false ? "KG" : "LBS") + ")", _oFontStyle));
            _oPdfPCell = new PdfPCell(new Phrase("QTY", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. PRICE(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("QTY", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("U. PRICE(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("AMOUNT(" + _oExportPI.Currency.ToUpper() + ")", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region Details Data
            string sTemp = ""; sMUName = "";
            int nCount = 0; double nTotalQty = 0; double nTotalValue = 0;

            foreach (ExportPIDetail oItem in _oExportPIDetails)
            {
                nCount++;
                //oPdfPTable = new PdfPTable(7);
                //oPdfPTable.SetWidths(new float[] { 20f, 265f, 75f, 65f, 60f, 55f, 75f });

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPhrase = new Phrase();

                //string input = "123- abcd33";
                //sTemp = new String(oItem.ProductName.Where(c => c != '/' && c != '%' && c != '-' && (c < '0' || c > '9')).ToArray());
                //_oPhrase.Add(new Chunk(chars + " " + oItem.ProductName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));

                _oPhrase.Add(new Chunk( oItem.ProductName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
                sTemp = "";
                if (oItem.ShadeType != EnumDepthOfShade.None)
                {
                    _oPhrase.Add(new Chunk("\nShade:" + oItem.ShadeType, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                }
                if (!String.IsNullOrEmpty(oItem.HSCode))
                {
                    sTemp = sTemp + "\nH.S.CODE: " + oItem.HSCode;
                }
                if (oItem.PackingType > 0)
                {
                    sTemp = sTemp + "\nPacking: " + (EumDyeingType)oItem.PackingType;
                }
                if (oItem.DyeingType > 0)
                {
                    sTemp = sTemp + "\nDyeing Process: " + (EumDyeingType)oItem.DyeingType + " Dyeing";
                }

                if (!String.IsNullOrEmpty(oItem.ColorInfo))
                {
                    sTemp = sTemp + "\nColor: " + oItem.ColorInfo;
                }
                if (!String.IsNullOrEmpty(oItem.StyleNo))
                {
                    sTemp = sTemp + "\nStyle No: " + oItem.StyleNo;
                }
                if (!String.IsNullOrEmpty(oItem.BuyerReference))
                {
                    sTemp = sTemp + " Buyer Ref: " + oItem.BuyerReference;
                }
                if (!String.IsNullOrEmpty(oItem.ExportQuality))
                {
                    sTemp = sTemp + ", Quality:" + oItem.ExportQuality;
                }

                if (oItem.SaleType == EnumProductionType.Commissioning)
                {
                    if (String.IsNullOrEmpty(oItem.ColorInfo))
                    {
                        sTemp = sTemp + "Average\n(Yarn Dyeing Charge)";
                    }
                }

                _oPhrase.Add(new Chunk("" + sTemp, FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL)));
                // _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + " [Dark Shade]", _oFontStyle));
                _oPdfPCell = new PdfPCell(_oPhrase);

                _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(System.Math.Round(oItem.Qty, 0).ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                sTemp = "";
                //oItem.Qty = (_bIsInKg == false ? Global.GetKG(oItem.Qty, 10) : oItem.Qty);
                nTotalQty = nTotalQty + oItem.Qty;
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //oItem.UnitPrice = (_bIsInKg == false ? Global.GetLBS(oItem.UnitPrice, 10) : oItem.UnitPrice);                

                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + System.Math.Round(oItem.UnitPrice, 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Global.GetKG(oItem.Qty, 10)), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //oItem.UnitPrice = (_bIsInKg == false ? Global.GetLBS(oItem.UnitPrice, 10) : oItem.UnitPrice);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + "" + System.Math.Round(Global.GetLBS(oItem.UnitPrice, 10), 4).ToString("#,##0.00##;(#,##0.00##)"), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((oItem.Qty * oItem.UnitPrice), 2), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                nTotalValue = nTotalValue + oItem.Qty * oItem.UnitPrice;

                sMUName = oItem.MUName;
                oPdfPTable.CompleteRow();

                //_oPdfPCell = new PdfPCell(oPdfPTable);
                //_oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
            }

            //oPdfPTable = new PdfPTable(7);
            //oPdfPTable.SetWidths(new float[] { 20f, 265f, 75f, 65f, 60f, 55f, 75f });


            #region Total Summary
            //   _oFontStyle = FontFactory.GetFont("Tahoma", 9.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle_Bold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle_Bold)); //sMUName
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Global.GetKG(nTotalQty, 10)), _oFontStyle_Bold)); //sMUName
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + Global.MillionFormat(System.Math.Round(System.Math.Round(nTotalValue, 2), 2)), _oFontStyle_Bold));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            if (_oExportPI.CurrencyID == _oCompany.BaseCurrencyID)/// Hard code-Please Change it Mamun
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word :  " + Global.TakaWords(nTotalValue), _oFontStyle_Bold));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("In Word : US " + Global.DollarWords(nTotalValue), _oFontStyle_Bold));
            }
            _oPdfPCell.Colspan = 9; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            #endregion


            return oPdfPTable;
        }
        #endregion

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

