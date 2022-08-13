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
 
 

namespace ESimSol.Reports
{
    public class rptRecapOrderSheetPreviewFormat1
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        OrderRecap _oOrderRecap = new OrderRecap();
        List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
        List<OrderRecapDetail> _oOrderRecapDetails = new List<OrderRecapDetail>();
        List<SizeCategory> _oSizeCategories = new List<SizeCategory>();
        List<ColorCategory> _oColorCategories = new List<ColorCategory>();
        List<RecapBillOfMaterial> _oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
        List<ORAssortment> _oORAssortments = new List<ORAssortment>();
        Company _oCompany = new Company();
         int _nRowSpan = 18;//for Defult format
         double _nTotalAssortmentQty = 0;
           
        #endregion

    
        private PdfPTable GetSizeColumnName(int nWidth)
        {

            PdfPTable oPdfPTable = new PdfPTable(_oSizeCategories.Count);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] sizeLength = new float[_oSizeCategories.Count];
            for (int i = 0; i < _oSizeCategories.Count; i++)
            {
                sizeLength[i] = (nWidth / _oSizeCategories.Count);
            }
            oPdfPTable.SetWidths(sizeLength);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            foreach (SizeCategory oItem in _oSizeCategories)
            {

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SizeCategoryName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        private PdfPTable GetSizeBreakDownValue(int ColorID, int nWidth, bool bIsORAssortment, bool bIsSolidAssortment, bool bIsCalCulateSizeTotal)
        {

            PdfPTable oPdfPTable = new PdfPTable(_oSizeCategories.Count);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] sizeLength = new float[_oSizeCategories.Count];
            for (int i = 0; i < _oSizeCategories.Count; i++)
            {
                sizeLength[i] = (nWidth / _oSizeCategories.Count);
            }
            oPdfPTable.SetWidths(sizeLength);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            if (bIsCalCulateSizeTotal)
            {
                foreach (SizeCategory oItem in _oSizeCategories)
                {
                    double nSizeWiseTotal = GetTotalSizeValue(oItem.SizeCategoryID, bIsORAssortment, bIsSolidAssortment);

                    if (_oOrderRecap.QtyPerCarton > 0 && bIsSolidAssortment == true && nSizeWiseTotal>0)
                    {
                        nSizeWiseTotal =  Math.Ceiling(nSizeWiseTotal / _oOrderRecap.QtyPerCarton);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(nSizeWiseTotal + " " + _oOrderRecapDetails[0].UnitSymbol, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
            }
            else
            {
                foreach (SizeCategory oItem in _oSizeCategories)
                {

                    double nSizeValue =GetSizeValue(ColorID, oItem.SizeCategoryID, bIsORAssortment, bIsSolidAssortment);
                    string sizeValueInSring = "";
                    if (_oOrderRecap.QtyPerCarton > 0 && bIsSolidAssortment == true && nSizeValue > 0)
                    {
                        nSizeValue = Math.Ceiling(nSizeValue / _oOrderRecap.QtyPerCarton);
                    }
                    if(nSizeValue>0)
                    {
                        sizeValueInSring = nSizeValue.ToString();
                    }else
                    {
                        sizeValueInSring = "-";
                    }

                    _oPdfPCell = new PdfPCell(new Phrase((ColorID > 0) ? sizeValueInSring : " ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
            }
           
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        private double GetSizeValue(int ColorID, int SizeCategoryID, bool bIsORAssortment, bool bIsSolidAssortment)
        {
            if (bIsORAssortment == true && bIsSolidAssortment==false)
            {
                foreach (ORAssortment oItem in _oORAssortments)
                {
                    if (oItem.ColorID == ColorID && oItem.SizeID == SizeCategoryID)
                    {
                        return oItem.Qty;
                    }
                }
            }
            else
            {
                foreach (OrderRecapDetail oItem in _oOrderRecapDetails)
                {
                    if (oItem.ColorID == ColorID && oItem.SizeID == SizeCategoryID)
                    {
                        return oItem.Quantity;
                    }
                }
            }

            return 0;
        }

        private double GetTotalSizeValue(int SizeCategoryID, bool bIsORAssortment, bool bIsSolidAssortment)
        {
            double nTotalQty = 0;
            if (bIsORAssortment == true && bIsSolidAssortment==false)
            {
                foreach (ORAssortment oItem in _oORAssortments)
                {
                    if (oItem.SizeID == SizeCategoryID)
                    {
                        nTotalQty += oItem.Qty;
                    }
                }
            }
            else
            {
                foreach (OrderRecapDetail oItem in _oOrderRecapDetails)
                {
                    if ( oItem.SizeID == SizeCategoryID)
                    {
                        nTotalQty += oItem.Quantity;
                    }
                }
            }
          
            return nTotalQty;
        }


        public byte[] PrepareReportFormat1(OrderRecap oOrderRecap)// Report Format 1 
        {
            _oOrderRecap = oOrderRecap;
            _oOrderRecapDetails = oOrderRecap.OrderRecapDetails;
            _oSizeCategories = oOrderRecap.SizeCategories;
            _oColorCategories = oOrderRecap.ColorCategories;
            _oCompany = oOrderRecap.Company;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(25f, 25f, 15f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 495f});
            #endregion

            this.PrintHeaderFormat1();
            this.PrintBodyFormat1();
            _oDocument.Add(_oPdfPTable);       
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeaderFormat1()
        {
            #region ReportHeader
            _oPdfPCell = new PdfPCell(new Phrase("ORDER SHEET", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            #endregion
        }
        #endregion

        private void PrintBodyFormat1()
        {
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f,iTextSharp.text.Font.NORMAL );
            oPdfPTable.SetWidths(new float[] { 290f, 252f, 250f });

            #region Company Supplier Information

           #region 1st Row
           #region company Logo print
           if (_oCompany.CompanyLogo != null)
           {
               _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
               _oImag.ScaleAbsolute(150f,30f);
               _oPdfPCell = new PdfPCell(_oImag);
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
               _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
               _oPdfPCell.BorderWidthLeft = .5f;
               _oPdfPCell.BorderWidthRight = 0f; 
               _oPdfPCell.BorderWidthBottom = 0f;
               _oPdfPCell.Rowspan = 3;
               oPdfPTable.AddCell(_oPdfPCell);

           }
           else
           {
               _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
               _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
               _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
               _oPdfPCell.BorderWidthLeft = .5f;
               _oPdfPCell.BackgroundColor = BaseColor.WHITE;
               _oPdfPCell.Rowspan = 3;
               oPdfPTable.AddCell(_oPdfPCell);
    
           }
           #endregion
           _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
           _oPdfPCell = new PdfPCell(new Phrase("SUPPLIER", _oFontStyle));
           _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = 2; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
           _oPdfPCell = new PdfPCell(new Phrase("ORDER DATE :"+_oOrderRecap.OrderDateInString , _oFontStyle));
           _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

           oPdfPTable.CompleteRow();
           #endregion 

           #region 2nd Row
           _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT DATE: "+ _oOrderRecap.FactoryShipmentDateInString /*_oOrderRecap.ShipmentDate.Subtract(TimeSpan.FromDays(_oOrderRecap.ClientOperationSetting.OrderSheetShipmentDateCount)).ToString("dd MMM yyyy")*/, _oFontStyle));
           _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
           oPdfPTable.CompleteRow();
           #endregion

           #region 3rd Row
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TERMS OF DELIVERY: "+_oOrderRecap.DeliveryTerm, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
           #endregion

           #region 4th Row
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = 2; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.FactoryName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("PAYMENTS TERMS: " + _oOrderRecap.PaymentTerm, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

           #region 5th Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.FactoryAddress, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.BorderWidthTop = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("REQUIRED SAMPLES: " + _oOrderRecap.RequiredSample, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

           #region 6th Row
            _oPdfPCell = new PdfPCell(new Phrase( "Tel #"+_oCompany.Phone, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PACKING INSTRUCTION: " + _oOrderRecap.PackingInstruction, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

           #region 7th Row
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("E-mail: "+_oCompany.Email, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("ASSORTMENT: " + _oOrderRecap.Assortment, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border= 1;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 1;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
           #endregion

            #region Yarn and Accessorie Print
            oPdfPTable = new PdfPTable(11);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 80f, 50f, 70f, 90f, 90f, 50f, 50f, 50f, 50f });


            #region Accessoreis Caption Print
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.Colspan = 7; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ACCESSORIES", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.Colspan = 4; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion

            #region Customer Ref
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("CUSTOMER REF", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("STYLE NO:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ORDER NO:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RECAP S.L NO:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("REVISED ON: "+_oOrderRecap.DBDate.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("RESPONSIBLE PERSON", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MAIN LABEL", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("TBA", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 3; _oPdfPCell.Rowspan = 7; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  RECAP SL No
            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.BuyerName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.StyleNo, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.OrderRecapNo, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.SLNo, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.BorderWidthRight = 1f; _oPdfPCell.BorderWidthBottom = 1f; _oPdfPCell.BorderWidthTop = 1f; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;  _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("COUNTRY OF ORIGIN:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("NAME", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("SIZE LABEL", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();
            #endregion

            #region  FABRICATION
            _oPdfPCell = new PdfPCell(new Phrase("FABRICATION ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("YARN/COUNT", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLY", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("BARCODE NO:       "+_oOrderRecap.BarCodeNo, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 4; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("BANGLADESH", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 4; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("01." + _oOrderRecap.ClientOperationSetting.ValueInString, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("CARE LABEL", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  yarn Print 1
            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.FabricName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 4; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.OrderRecapYarns.Count > 0) ? _oOrderRecap.OrderRecapYarns[0].YarnName : "", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.OrderRecapYarns.Count > 0) ? _oOrderRecap.OrderRecapYarns[0].YarnPly : "", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("02." + _oOrderRecap.ApproveByName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("HANG TAG", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  yarn Print 2
            _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.OrderRecapYarns.Count > 1) ? _oOrderRecap.OrderRecapYarns[1].YarnName : "", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.OrderRecapYarns.Count > 1) ? _oOrderRecap.OrderRecapYarns[1].YarnPly : "", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("03." + _oOrderRecap.MerchandiserName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("POLLY BAG", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  yarn Print 3
            _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.OrderRecapYarns.Count > 2) ? _oOrderRecap.OrderRecapYarns[2].YarnName : "", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.OrderRecapYarns.Count > 2) ? _oOrderRecap.OrderRecapYarns[2].YarnPly : "", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BLISTER", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  yarn Print 4
            _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.OrderRecapYarns.Count > 3) ? _oOrderRecap.OrderRecapYarns[3].YarnName : "", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oOrderRecap.OrderRecapYarns.Count > 3) ? _oOrderRecap.OrderRecapYarns[3].YarnPly : "", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION OF GOODS : "+_oOrderRecap.StyleDescription, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 4; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("CARTON", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  Quantity and Kjnitting Machine
            _oPdfPCell = new PdfPCell(new Phrase("QUANTITY: " + _oOrderRecap.TotalQuantity.ToString("0"), _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("KNITTING MACHINE :"+_oOrderRecap.KnittingPatternName, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GAUGE: " + _oOrderRecap.GG, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("WEIGHT: " + _oOrderRecap.Weight, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ON BOARD DATE: " , _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.FactoryShipmentDateInString /*_oOrderRecap.ShipmentDate.Subtract(TimeSpan.FromDays(_oOrderRecap.ClientOperationSetting.OrderSheetShipmentDateCount)).ToString("dd MMM yyyy")*/, _oFontStyle));
            _oPdfPCell.BorderWidthLeft = 0f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region  Color Size Break Down Caption
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("COLOR & SIZE BREAKDOWN: ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 7; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;  _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border= 1;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 1;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            
            #region Color Size BrekDown Ratio Table
            oPdfPTable = new PdfPTable(7);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPTable.SetWidths(new float[] { 130f, 360f, 90f, 50f, 50f, 50f, 50f });

            #region 1st Row 
            _oPdfPCell = new PdfPCell(new Phrase("COLOR:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0f; _oPdfPCell.BorderWidthBottom = 0f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("SIZE", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;  _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion

            #region Size Name Print 
            _oPdfPCell = new PdfPCell(GetSizeColumnName(360));
            _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0;
            foreach (ColorCategory oItem in _oColorCategories)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetSizeBreakDownValueFormat1(oItem.ColorCategoryID));
                _oPdfPCell.Border = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(GetColorWiseTotalQtyFormat1(oItem.ColorCategoryID).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
               
                oPdfPTable.CompleteRow();
            }

            #region Blank Rows
            if (nCount < 6)
            {
                for (int i = nCount; i <= 6; i++)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(GetSizeBreakDownValueFormat1(0));
                    _oPdfPCell.Border = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = .5f; _oPdfPCell.BorderWidthRight = .5f; _oPdfPCell.BorderWidthBottom = .5f; _oPdfPCell.BorderWidthTop = .5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                }
            }
            #endregion

            #region total Print
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TOTAL = ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oOrderRecap.TotalQuantity.ToString()+" PCS", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
    
            #endregion


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 1;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 1;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Print Style Image 
            oPdfPTable = new PdfPTable(3);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            oPdfPTable.SetWidths(new float[] { 260f,260f,260f });

            #region Image Caption print

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("PICTURE IMAGE FRONT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.BorderWidthLeft = .5f;
            _oPdfPCell.BorderWidthBottom = 0.5f;
            _oPdfPCell.BorderWidthRight = 0.5f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PICTURE IMAGE BACK", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BorderWidthLeft = 0.5f;
            _oPdfPCell.BorderWidthTop = .5f;
            _oPdfPCell.BorderWidthBottom = 0.5f;
            _oPdfPCell.BorderWidthRight = 0.5f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("COMMENTS", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BorderWidthLeft = .5f;
            _oPdfPCell.BorderWidthBottom = 0.5f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

          
            #endregion

            #region Front Image print
            if (_oOrderRecap.TechnicalSheetFrontImage.FrontImage!= null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oOrderRecap.TechnicalSheetFrontImage.FrontImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(220f,160f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BorderWidthLeft = .5f;
                _oPdfPCell.BorderWidthRight = 0.5f;
                _oPdfPCell.BorderWidthBottom = 0.5f;
                _oPdfPCell.BorderWidthTop = 0f;
                _oPdfPCell.FixedHeight = 170f;
                oPdfPTable.AddCell(_oPdfPCell);

            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BorderWidthLeft = .5f;
                _oPdfPCell.BorderWidthRight = 0f;
                _oPdfPCell.FixedHeight = 170f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

            }
            #endregion

            #region Back Image print
            if (_oOrderRecap.TechnicalSheetBackImage.BackImage != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oOrderRecap.TechnicalSheetBackImage.BackImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(220f, 160f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.BorderWidthLeft = 0.5f;
                _oPdfPCell.BorderWidthRight = 0f;
                _oPdfPCell.BorderWidthBottom = 0.5f;
                _oPdfPCell.BorderWidthTop = 0f;
                _oPdfPCell.FixedHeight = 170f;
                oPdfPTable.AddCell(_oPdfPCell);

            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BorderWidthLeft = 0f;
                _oPdfPCell.BorderWidthTop = 0f;
                _oPdfPCell.BorderWidthRight = 0f;
                _oPdfPCell.FixedHeight = 170f;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

            }
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(GetCommentsFormat1(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BorderWidthLeft = .5f;
            _oPdfPCell.BorderWidthBottom = 0.5f;
            _oPdfPCell.BorderWidthTop = 0.5f;
            _oPdfPCell.BorderWidthRight = 0.5f;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Remarks :" + _oOrderRecap.Description, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 1;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 1;            
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            
        }

        private PdfPTable GetSizeBreakDownValueFormat1(int ColorID)
        {

            PdfPTable oPdfPTable = new PdfPTable(_oSizeCategories.Count);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] sizeLength = new float[_oSizeCategories.Count];
            for (int i = 0; i < _oSizeCategories.Count; i++)
            {
                sizeLength[i] = (360 / _oSizeCategories.Count);
            }
            oPdfPTable.SetWidths(sizeLength);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            foreach (SizeCategory oItem in _oSizeCategories)
            {
                _oPdfPCell = new PdfPCell(new Phrase((ColorID > 0) ? GetSizeValueInStringFormat1(ColorID, oItem.SizeCategoryID) : " ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        private string GetSizeValueInStringFormat1(int ColorID, int SizeCategoryID)
        {

            foreach (OrderRecapDetail oItem in _oOrderRecapDetails)
            {
                if (oItem.ColorID == ColorID && oItem.SizeID == SizeCategoryID)
                {
                    return oItem.Quantity.ToString();
                }
            }

            return "-";
        }

        private double GetColorWiseTotalQtyFormat1(int nColorID)
        {
            double nTotalQty = 0;
            foreach(OrderRecapDetail oItem in _oOrderRecapDetails)
            {
                if(oItem.ColorID ==  nColorID)
                {
                        nTotalQty +=oItem.Quantity;
                }
            }
            return nTotalQty;
        }

        private string GetCommentsFormat1()
        {
            string sComments = ""; int nCount =0;
            foreach (ImageComment oItem in _oOrderRecap.ImageComments)
            { 
                nCount++;
                sComments = sComments + nCount.ToString() + ") " + oItem.Comments + ".\n";
            }

            return sComments;
        }
      

   
       
    }
}
