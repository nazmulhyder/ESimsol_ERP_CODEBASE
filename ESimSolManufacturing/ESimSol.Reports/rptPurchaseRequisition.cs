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
    public class rptPurchaseRequisition
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        iTextSharp.text.Font _oFontStyle_UnLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Company _oCompany = new Company();
        PurchaseRequisition _oPurchaseRequisition = new PurchaseRequisition();
        List<PurchaseRequisitionDetail> _oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
        Contractor _oContractor = new Contractor();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        #endregion

        #region PurchaseRequisition LC
        public byte[] PrepareReport(PurchaseRequisition oPurchaseRequisition, BusinessUnit oBusinessUnit, Company oCompany, ClientOperationSetting oClientOperationSetting, List<SignatureSetup> oSignatureSetups)
        {
            _oPurchaseRequisition = oPurchaseRequisition;
            _oPurchaseRequisitionDetails = oPurchaseRequisition.PurchaseRequisitionDetails;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oClientOperationSetting = oClientOperationSetting;
            _oSignatureSetups = oSignatureSetups;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 
                                                    595f //Articale
                                                 
                                              });
            #endregion
         
            this.PrintHeader();
         
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
     
        #region Report Header
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
                _oImag.ScaleAbsolute(60f, 25f);
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
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Purchase Requisition", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintHeader_Blank()
        {
            
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 15f, 4);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.FixedHeight = 150f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Purchase Requisition", _oFontStyle_UnLine));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body

     
        private void PrintBody()
        {
         

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

             PdfPTable oPdfPTable = new PdfPTable(3);
             oPdfPTable.SetWidths(new float[] { 200f,200f,200f });


             _oPdfPCell = new PdfPCell(new Phrase("PR No: " + _oPurchaseRequisition.PRNo, _oFontStyle_Bold));
             _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("Date: " + _oPurchaseRequisition.PRDate.ToString("MMMM dd, yyyy"), _oFontStyle_Bold));
             _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();


             _oPdfPCell = new PdfPCell(new Phrase("Note: " + _oPurchaseRequisition.Note, _oFontStyle_Bold));
             _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();

             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
             oPdfPTable.CompleteRow();

           

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
         
            #region PI Details
            #region Detail Column Header
            if (_oClientOperationSetting.Value=="1")//if Apply Style No
            {
                oPdfPTable = new PdfPTable(8);
                oPdfPTable.SetWidths(new float[] { 25f, 135f, 100f, 80f, 80f, 80f, 50f, 50f });
            }
            else
            {
               
                oPdfPTable = new PdfPTable(5);
                oPdfPTable.SetWidths(new float[] { 25f, 235f, 130f, 50f, 60f });
            }
            string sUnitName = "";
            if (_oPurchaseRequisitionDetails.Count > 0)
            {
                sUnitName = _oPurchaseRequisitionDetails[0].UnitName;
            }
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Item Name", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_oClientOperationSetting.Value == "1")//if Apply Style No
            {
                _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style No", _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("PO No", _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("UNIT", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity", _oFontStyle_Bold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            sUnitName = "";
            #region Details Data
            int nCount = 0; double nTotalQty = 0;         
            foreach (PurchaseRequisitionDetail oItem in _oPurchaseRequisitionDetails)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (_oClientOperationSetting.Value == "1")//if Apply Style No
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderRecapNo, _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitName, _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                nTotalQty = nTotalQty + oItem.Qty;
        
                sUnitName = oItem.UnitName;
                oPdfPTable.CompleteRow();
            }

            int nRequiredRow = 5 - (_oPurchaseRequisitionDetails.Count);
            for (int i = 1; i <= nRequiredRow; i++)
            {
                #region Blank Row

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (_oClientOperationSetting.Value == "1")//if Apply Style No
                {
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                 _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion
            }

            #region Total Summary         

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion
            #endregion
            #endregion

            #region Authorized Signature
            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(535f, _oPurchaseRequisition, _oSignatureSetups, 30f));
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();            
            #endregion
        }
        #endregion
        #endregion
    }
}
