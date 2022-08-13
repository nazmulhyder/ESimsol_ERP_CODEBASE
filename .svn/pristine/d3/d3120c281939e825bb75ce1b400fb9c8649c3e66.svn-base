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
    public class rptPurchaseOrder_Format2
    {
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        iTextSharp.text.Font _oFontStyle_UnLine;
        int _nTitleTypeInImg;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        string _Subject = "";
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        PurchaseOrder _oPurchaseOrder = new PurchaseOrder();
        List<PurchaseOrderDetail> _oPurchaseOrderDetails = new List<PurchaseOrderDetail>();
        List<PurchaseOrderDetail> _oPurchaseOrderDetails_Property = new List<PurchaseOrderDetail>();
        Contractor _oContractor = new Contractor();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        Contractor _oDeliveryTo = new Contractor();
        List<POSpec> _oPOSpecs = new List<POSpec>();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        List<ApprovalHead> _oApprovalHeads = new List<ApprovalHead>();

        public byte[] PrepareReport(PurchaseOrder oPurchaseOrder, Company oCompany, Contractor oContractor, BusinessUnit oBusinessUnit, ClientOperationSetting oClientOperationSetting, List<SignatureSetup> oSignatureSetups, Contractor oDeliveryTo, List<POSpec> oPOSpecs, string Subject, int nTitleTypeInImg, List<ApprovalHead> oApprovalHeads)
        {
            _oPurchaseOrder = oPurchaseOrder;
            _oPurchaseOrderDetails = oPurchaseOrder.PurchaseOrderDetails;
            _oBusinessUnit = oBusinessUnit;
            _oContractor = oContractor;
            _oClientOperationSetting = oClientOperationSetting;
            _oCompany = oCompany;
            _oSignatureSetups = oSignatureSetups;
            _oDeliveryTo = oDeliveryTo;
            _oPOSpecs = oPOSpecs;
            _Subject = Subject;
            _nTitleTypeInImg = nTitleTypeInImg;
            _oApprovalHeads = oApprovalHeads;
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(50f, 50f, 30f, 5f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 
                                                    492f //Articale
                                              });

            //this.PrintHeader();
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintHeader()
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
                _oImag.ScaleAbsolute(60f, 30f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }

        private void PrintBody()
        {
            if (_nTitleTypeInImg == 1)//Normal
            {
                //fixed Height 15 px
                this.PrintHeader();
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold)); _oPdfPCell.FixedHeight = 5;
                _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            if (_nTitleTypeInImg == 2)//Pad
            {
                PrintHeader_Blank();
            }
            if (_nTitleTypeInImg == 3)//Image
            {
                _oPdfPCell = new PdfPCell(this.LoadCompanyTitle());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 595f });
            PdfPCell oPdfPCell;

            oPdfPCell = new PdfPCell(new Phrase("Ref: " + _oPurchaseOrder.PONo, _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;  oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Date: " + _oPurchaseOrder.PODateSt, _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("To", _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.ContractorName, _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oContractor.Address, _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Attn: " + _oPurchaseOrder.ContactPersonName, _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            if (_Subject == "")
            {
                oPdfPCell = new PdfPCell(new Phrase("Sub: Work Order for Supply " + _oBusinessUnit.Name, _oFontStyle_Bold)); 
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("Sub: " + _Subject, _oFontStyle_Bold)); 
            }

            oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Dear " + _oPurchaseOrder.ContactPersonName, _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();



            Paragraph _oPdfParagraph;
            _oPdfParagraph = new Paragraph(new Phrase("Thank you very much for your quotation. We are pleased to place an order for the following item on the terms and conditions agreed upon. We would be grateful if you supply the item at our Factory office located at " + _oDeliveryTo.Address, _oFontStyle));
            _oPdfParagraph.SetLeading(0.5f, 1.4f);
            _oPdfParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            oPdfPCell = new PdfPCell();
            oPdfPCell.AddElement(_oPdfParagraph);
            oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("Thank you very much for your quotation. We are pleased to place an order for the following item on the terms and conditions agreed upon. We would be grateful if you supply the item at our Head office located at " + _oCompany.Address, _oFontStyle)); oPdfPCell.FixedHeight = 30;
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Description of Item:", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();




            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 40f, 140f, 70f, 70f, 50f, 100f });

            oPdfPCell = new PdfPCell(new Phrase("SL. NO", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("MPRNo", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Required", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Amount (" + _oPurchaseOrder.CurrencySymbol + ")", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            
            oPdfPTable.CompleteRow();
            int nCount = 0;
            double nTotal = 0.0;
            foreach (PurchaseOrderDetail oItem in _oPurchaseOrderDetails)
            {
                nCount++;
                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                string ProductName = oItem.ProductName;
                if (_oPOSpecs.Any())
                {
                    var oSpecList = _oPOSpecs.Where(x => x.PODetailID == oItem.PODetailID).ToList();
                    if (oSpecList.Any())
                    {
                        ProductName = oItem.ProductName + Environment.NewLine + string.Join(" , ", oSpecList.Select(x => x.SpecName + " : " + x.PODescription));
                    }

                }

                oPdfPCell = new PdfPCell(new Phrase(ProductName, _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(oItem.RefNo, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                
                oPdfPCell = new PdfPCell(new Phrase(oItem.QtySt+oItem.UnitSymbol, _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(oItem.UnitPriceSt, _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                nTotal += (oItem.Qty * oItem.UnitPrice);

                oPdfPCell = new PdfPCell(new Phrase((oItem.Qty * oItem.UnitPrice).ToString("#,##0.00;(#,##0.00)"), _oFontStyle)); 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            }

            oPdfPCell = new PdfPCell(new Phrase("Total ", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Colspan = 5; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nTotal.ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //if (_oPurchaseOrder.DiscountInAmount > 0)
            //{
            //    oPdfPCell = new PdfPCell(new Phrase("Discount ", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Colspan = 4; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            //    oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.DiscountInAmount.ToString(), _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            //    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            //    oPdfPTable.CompleteRow();
            //}

            //double nSubTotal = nTotal;
            if (_oPurchaseOrder.PurchaseCosts.Count > 0)
            {
                foreach (PurchaseCost oPC in _oPurchaseOrder.PurchaseCosts)
                {
                    oPdfPCell = new PdfPCell(new Phrase(oPC.Name, _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Colspan = 5; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oPC.ValueInAmount.ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();
                    //nSubTotal += ((int)oPC.CostHeadType == 1) ? nTotal+=oPC.ValueInAmount : nTotal-=oPC.ValueInAmount;
                }
                nTotal = nTotal + (_oPurchaseOrder.PurchaseCosts.Where(x => (int)x.CostHeadType == 1).ToList().Sum(x => x.ValueInAmount) - _oPurchaseOrder.PurchaseCosts.Where(x => (int)x.CostHeadType == 2).ToList().Sum(x => x.ValueInAmount));
                     
            }

            oPdfPCell = new PdfPCell(new Phrase("Total Payable Amount", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.Colspan = 5; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(nTotal.ToString("#,##0.00;(#,##0.00)"), _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold)); 
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.Colspan = 6; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("In Word: " + (_oPurchaseOrder.CurrencyID == 1 ? Global.TakaWords(nTotal) : Global.DollarWords(nTotal)), _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.Colspan = 6; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.Colspan = 6; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            //oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.Colspan = 5; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 492f });

            oPdfPCell = new PdfPCell(new Phrase("Terms & Condition:", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            int nClauseType = 0;
            POTandCClause oPOTandCClause = new POTandCClause();
            List<POTandCClause> oPOTandCClauses = new List<POTandCClause>();
            oPOTandCClauses = _oPurchaseOrder.POTandCClauses;
            oPOTandCClauses = oPOTandCClauses.OrderBy(a => a.POTandCClauseID).ToList();
            oPOTandCClauses = oPOTandCClauses.OrderBy(a => a.ClauseType).ToList();

            /// From PO TnC Setup terms
            nCount = 0;
            foreach (POTandCClause oItem in oPOTandCClauses)
            {
                nCount++;
                oPdfPCell = new PdfPCell(new Phrase(nCount.ToString() + ". " + oItem.TermsAndCondition, _oFontStyle)); oPdfPCell.FixedHeight = 15;
                oPdfPCell.Border = 0; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                nClauseType = oItem.ClauseType;
            }


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Your kind co-operation in this regard will be highly appreciated.", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Thanking you,", _oFontStyle)); oPdfPCell.FixedHeight = 15;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 80;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            //oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            //_oFontStyle_Bold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            //oPdfPCell = new PdfPCell(new Phrase("(Manager)", _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            //oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase(_oPurchaseOrder.ApprovedByName, _oFontStyle_Bold)); oPdfPCell.FixedHeight = 15;
            //oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();
            //_oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            #region Signature
            //string[] signatureList = new string[_oApprovalHeads.Count];
            //string[] dataList = new string[_oApprovalHeads.Count];

            //for (int j = 1; j <= _oApprovalHeads.Count; j++)
            //{
            //    signatureList[j - 1] = (_oApprovalHeads[j - 1].Name);
            //    dataList[j - 1] = "";
            //}
            
            #region Authorized Signature
            //_oPdfPCell = new PdfPCell(this.GetSignature(535f, dataList, signatureList, 20f)); _oPdfPCell.Border = 0;
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetDynamicSignatureWithImageAndDesignation(_oPurchaseOrder, _oApprovalHeads, 45f, true, true));
            _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion

        }

        private PdfPTable GetSignature(float nTableWidth, string[] oData, string[] oSignatureSetups, float nBlankRowHeight)
        {
            iTextSharp.text.Font _oFontStyle;
            PdfPCell _oPdfPCell;
            int nSignatureCount = oSignatureSetups.Length;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            if (nSignatureCount <= 0)
            {
                #region Blank Table
                PdfPTable oPdfPTable = new PdfPTable(1);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { nTableWidth });

                if (nBlankRowHeight <= 0)
                {
                    nBlankRowHeight = 10f;
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                return oPdfPTable;
                #endregion
            }
            else
            {

                PdfPTable oPdfPTable = new PdfPTable(nSignatureCount);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                #region

                #endregion

                int nColumnCount = -1;
                float[] columnArray = new float[nSignatureCount];
                foreach (string oItem in oSignatureSetups)
                {
                    nColumnCount++;
                    columnArray[nColumnCount] = nTableWidth / nSignatureCount;
                }
                oPdfPTable.SetWidths(columnArray);

                #region Blank Row
                if (nBlankRowHeight > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Colspan = nSignatureCount; _oPdfPCell.FixedHeight = nBlankRowHeight; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                #endregion

                nColumnCount = 0;
                for (int i = 0; i < oSignatureSetups.Length; i++)
                {
                    if (nSignatureCount == 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" " + oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else if (nSignatureCount == 2)
                    {
                        if (nColumnCount == 0)
                        {

                            _oPdfPCell = new PdfPCell(new Phrase(" " + oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oData[i] + "\n_________________\n" + oSignatureSetups[i] + " ", _oFontStyle));
                            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oData[i] + "\n_________________\n" + oSignatureSetups[i], _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    nColumnCount++;
                }
                return oPdfPTable;
            }
        }

        private void PrintHeader_Blank()
        {
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 100f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
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
    }
}
