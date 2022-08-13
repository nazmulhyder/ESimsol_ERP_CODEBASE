using System;
using System.Data;
using ESimSol.BusinessObjects;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ICS.Core;
using ICS.Core.Utility;


namespace ESimSol.Reports
{


    public class rptTransferRequisitionSlip
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleTwo;
        iTextSharp.text.Font _oFontStyleForCC;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPTable _oPdfPTableTemp = new PdfPTable(1);
        //PdfPTable _oPdfPTableTemp = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        PdfPCell _oPdfPCellTwo;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        TransferRequisitionSlip _oTransferRequisitionSlip = new TransferRequisitionSlip();
        List<TransferRequisitionSlipDetail> _oTransferRequisitionSlipDetails = new List<TransferRequisitionSlipDetail>();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();

        PdfWriter PDFWriter;
        Company _oCompany = new Company();
        Contractor _oContractor = new Contractor();
        ContactPersonnel _oContactPersonnel = new ContactPersonnel();

        double _nTotalQty = 0;
        double _nTotalBage = 0;
        string _sMUnitName = "";
        double _nCount = 0;
        #endregion

        public byte[] PrepareReport(TransferRequisitionSlip oTransferRequisitionSlip, List<SignatureSetup> oSignatureSetups, string sCopyFormat)
        {
            _oCompany = oTransferRequisitionSlip.Company;
            _oTransferRequisitionSlip = oTransferRequisitionSlip;
            _oSignatureSetups = oSignatureSetups;
            _oTransferRequisitionSlipDetails = oTransferRequisitionSlip.TransferRequisitionSlipDetails;
            if (_oTransferRequisitionSlipDetails.Count > 0)
            {
                _sMUnitName = _oTransferRequisitionSlipDetails[0].MUSymbol;
            }
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(53f, 35f, 50f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = new List<string>(new string[] { "Security", "Receiver Seal & Signature" });
            PageEventHandler.nFontSize = 9;
            PDFWriter.PageEvent = PageEventHandler;
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 525f });
            #endregion

            this.PrintHeader("Office Copy");
            this.PrintBody();
            NewPageDeclaration();
            _nTotalQty = 0; _nTotalBage = 0;
            this.PrintHeader("Client Copy");
            this.PrintBody();
            NewPageDeclaration();
            _nTotalQty = 0; _nTotalBage = 0;
            this.PrintHeader("Gate Pass");
            this.PrintBody();
            NewPageDeclaration();
            _nTotalQty = 0; _nTotalBage = 0;
            this.PrintHeader("Client Gate Pass");
            this.PrintBody();
            _nTotalQty = 0; _nTotalBage = 0;

            _oPdfPTable.HeaderRows = 9;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();


            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sCopyFormat)
        {
            #region CompanyHeader

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 267.5f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oTransferRequisitionSlip.BusinessUnit.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oTransferRequisitionSlip.BusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Copy Format
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.ITALIC);
            _oPdfPCell = new PdfPCell(new Phrase(sCopyFormat, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader            
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("DELIVERY CHALLAN(TRANSFER)  ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion
            #region ReportHeader
            if (_oTransferRequisitionSlip.AuthorisedByID == 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.ITALIC);
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorize TransferRequisitionSlip", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPTableTemp.AddCell(_oPdfPCell);
                _oPdfPTableTemp.CompleteRow();
            }
            #endregion
            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            _oPdfPTableTemp.AddCell(_oPdfPCell);
            _oPdfPTableTemp.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {

            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 300f, 75f, 150f });

            #region Report Vocher No, TransferRequisitionSlip Date & Currency
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Transfer Ref No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" : "+_oTransferRequisitionSlip.FullTRSNO, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Issue Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase( " : "+_oTransferRequisitionSlip.IssueDateTime.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Issue Store", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" : "+_oTransferRequisitionSlip.IssueStoreNameWitoutLocation, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Vehicle No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" : "+_oTransferRequisitionSlip.VehicleNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Destination Store", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" : "+_oTransferRequisitionSlip.ReceivedStoreNameWitoutLocation, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Driver Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" : "+_oTransferRequisitionSlip.DriverName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" : " + ((_oTransferRequisitionSlip.ChallanNo == "") ? _oTransferRequisitionSlip.FullTRSNO : _oTransferRequisitionSlip.ChallanNo), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Gate Pass No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            
            _oPdfPCell = new PdfPCell(new Phrase(" : " + ((_oTransferRequisitionSlip.GatePassNo == "") ? _oTransferRequisitionSlip.FullTRSNO : _oTransferRequisitionSlip.GatePassNo), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow(); ;

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        

            #region TransferRequisitionSlip Details
            oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 455.5f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oFontStyleTwo = FontFactory.GetFont("Tahoma", 9f,1);
       
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 30f, //SL
                                               200f, //Description 
                                               70f, //Supplier
                                               84f, //Lot
                                               70f, //qty
                                               60f, //bag
                                               80f //Remarks
                                                });

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Lot/Batch ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Quantity(" + _sMUnitName+")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bag", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

         

       
            int nCount = 0;
            int nRawCount= 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleForCC = FontFactory.GetFont("Tahoma", 8f, 0);
            foreach (TransferRequisitionSlipDetail oItem in _oTransferRequisitionSlipDetails)
            {
                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                oPdfPTable.SetWidths(new float[] {
                                                30f, //SL
                                               200f, //Description 
                                               70f, //Supplier
                                               84f, //Lot
                                               70f, //qty
                                               60f, //bag
                                               80f //Remarks
                                                });

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                string sProductName = "";
                sProductName = oItem.ProductName;
                if (oItem.StyleID > 0)
                {
                    sProductName = sProductName + ",\nStyle No : " + oItem.StyleNo + ", Buyer : " + oItem.BuyerName;
                }
                _oPdfPCell = new PdfPCell(new Phrase(sProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.SupplierSName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
               
                 _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.QTY), _oFontStyle));
                 _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BagBales.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Remark, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                
                _nTotalQty = _nTotalQty + oItem.QTY;
                _nTotalBage=_nTotalBage+ oItem.BagBales;

                #region For Page brack if raw count >45
               
                if (_oPdfPTable.Rows.Count >= 45)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase("Continued ...", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();

                    _oPdfPCell = new PdfPCell(_oPdfPTableTemp);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    nCount = 0;

                }
                #endregion Page brack
            }
             nRawCount = _oPdfPTable.Rows.Count;
            #region For Print blank raw if (40-raw count)>0
             for (int i = 0; i <= (30 - nRawCount); i++)
            {

                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                oPdfPTable.SetWidths(new float[] { 30f, //SL
                                               200f, //Description 
                                               70f, //Supplier
                                               84f, //Lot
                                               70f, //qty
                                               60f, //bag
                                               80f //Remarks
                                                });


                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("  ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                 oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            for (int i = 1; i <= 1; i++)
            {

                oPdfPTable = new PdfPTable(7);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                oPdfPTable.SetWidths(new float[] { 30f, //SL
                                               200f, //Description 
                                               70f, //Supplier
                                               84f, //Lot
                                               70f, //qty
                                               60f, //bag
                                               80f //Remarks
                                                });

                nCount++;
              
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion 

            #region Total TransferRequisitionSlip Amount
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 300f, 84f, 70f, 60f,80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("In Words: "+GlobalObject.QtyInWords(_nTotalQty, _sMUnitName), _oFontStyle));
            _oPdfPCell.ExtraParagraphSpace = 2f; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                       
            _oPdfPCell = new PdfPCell(new Phrase("Total( "+_sMUnitName+" )", _oFontStyle));
            _oPdfPCell.ExtraParagraphSpace = 2f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
       
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty), _oFontStyle));
            _oPdfPCell.ExtraParagraphSpace = 2f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_nTotalBage.ToString(), _oFontStyle));
            _oPdfPCell.ExtraParagraphSpace = 2f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.ExtraParagraphSpace = 2f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Goods received in good condition & mentioned quantity ", _oFontStyle));
            _oPdfPCell.Colspan =5; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);          
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Narration Print
            oPdfPTable = new PdfPTable(2);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 50f, 475f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Remark", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oTransferRequisitionSlip.Remark, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region print Signature Captions
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(ESimSolSignature.GetSignature(525f, (object)_oTransferRequisitionSlip, _oSignatureSetups, 15f));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        public void NewPageDeclaration()
        {
            #region New Page Declare
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion
        }

    }
}
