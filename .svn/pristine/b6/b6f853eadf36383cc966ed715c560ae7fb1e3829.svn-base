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
    public class rptDUDeliveryChallan
    {
        #region Declaration
        int _nTotalColumn = 1;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DUDeliveryChallan _oDUDeliveryChallan = new DUDeliveryChallan();
        List<DUDeliveryChallanDetail> _oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        DUOrderSetup _oDUOrderSetup = new DUOrderSetup();
        DeliverySetup _oDeliverySetup = new DeliverySetup();
        int _nTitleType = 0;
        Company _oCompany = new Company();
        string _sMUnit = "";
        Phrase _oPhrase = new Phrase();
        int _nCount = 0;
        double _nTotalBagQty = 0, _nTotalCone = 0;
        double _nTotalNoOfCone = 0;
        double _nTotalQty = 0;
        float _nUsagesHeight = 0;
        #endregion
        #region
        public byte[] PrepareReport(DUDeliveryChallan oDUDeliveryChallan, Company oCompany, BusinessUnit oBusinessUnit, int nTitleType, DUOrderSetup oDUOrderSetup, List<ApprovalHead> oApprovalHeads)
        {
            _oDUDeliveryChallan = oDUDeliveryChallan;
            _oBusinessUnit = oBusinessUnit;
            _nTitleType = nTitleType;
            _oDUOrderSetup = oDUOrderSetup;
            _oDUDeliveryChallanDetails = oDUDeliveryChallan.DUDeliveryChallanDetails;
            _oCompany = oCompany;
            _oDeliverySetup = oDUDeliveryChallan.DeliverySetup;
            if (_oDUDeliveryChallanDetails.Count>0)
            {
                _sMUnit = _oDUDeliveryChallanDetails[0].MUnit;
              
            }
           
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 10f, 30f);
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

            //this.PrintHeader();
            if (_oDUDeliveryChallan.ApproveBy == 0)
            {
                ReporttHeaderUnauthorised();
                PrintWaterMark();
            }
            else
            {
                this.HeaderWithThreeFormats();
                this.ReporttHeader();
            }
            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.SampleOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.SaleOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.LoanOrder)
            {
                this.PrintHead_Sample();
                this.Print_Body();
            }
            else if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.ClaimOrder)
            {
                this.PrintHead_Bulk();
                this.Print_Body();
            }
            else
            {
                this.PrintHead_Bulk();
                this.Print_Body_Bulk();
            }

            if (oApprovalHeads.Count<= 0)
            {
                this.PrintFooter();
            }
            else 
            {
                //PrintApprovalHead(oApprovalHeads);
                PrintFooter(oApprovalHeads);
            }
            //_oPdfPTable.HeaderRows = 4;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void HeaderWithThreeFormats()
        {
            //_nPrintType = 1 means Normal Format
            //_nPrintType = 2 means PAD Format
            //_nPrintType = 3 means Full Image Title with logo
            if (_nTitleType == 1)
            {
                this.PrintHeader();
            }
            else if (_nTitleType == 2)
            {
                #region PAD Format
                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //_oPdfPCell.Colspan = _nColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; ;
                _oPdfPCell.FixedHeight = 150f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else if (_nTitleType == 3)
            {
                #region Image Format (Title with LOGO)
                if (_oDeliverySetup.Image != null)
                {
                    _oImag = iTextSharp.text.Image.GetInstance(_oDeliverySetup.Image, System.Drawing.Imaging.ImageFormat.Jpeg);
                    _oImag.ScaleAbsolute(530f, 100f);
                    _oPdfPCell = new PdfPCell(_oImag);
                    _oPdfPCell.Border = 0;
                    //_oPdfPCell.Colspan = _nColumn;
                    _oPdfPCell.FixedHeight = 100f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                else
                {
                    this.PrintHeader_Blank();
                }
                #endregion
            }

            //_oFontStyle = FontFactory.GetFont("Tahoma", 14f, 1);
            //_oPdfPCell = new PdfPCell(new Phrase(sDocHeader, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
        }
        private void PrintHeader_Blank()
        {
            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 70f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #region Print Header
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


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void PrintHeader_Buyer()
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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.ContractorName, _oFontStyle));
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


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        #endregion

        #region Report Header
        private void ReporttHeader()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print
            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.BulkOrder)
            {
                sHeaderName = "BULK\nDELIVERY CHALLAN";
            }
            else if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.SampleOrder)
            {
                sHeaderName = "SAMPLE\nDELIVERY CHALLAN";
            }
          
            else
            {
                sHeaderName = "DELIVERY CHALLAN";
            }

            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
         
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            if (_oDUDeliveryChallan.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Challan", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
        }
        private void ReporttHeaderUnauthorised()
        {
            #region Proforma Invoice Heading Print

            _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Challan", FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
           

            #endregion
        }
        #endregion


        #region Report Body
        private void PrintHead_Bulk()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 196f, 120f, 180f });

            oPdfPCell = new PdfPCell(new Phrase("DATE:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.ChallanDateSt, _oFontStyle));
            //oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("CHALLAN NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.ChallanNo, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.FactoryAddress))
            {
                oPdfPCell = new PdfPCell(new Phrase("ADDRESS", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.FactoryAddress, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.MBuyer))
            {
                oPdfPCell = new PdfPCell(new Phrase("MASTER BUYER", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.MBuyer, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            //
            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.ContactPersonnelName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " CONCERN ", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.MKTPName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //

            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.ClaimOrder)
            {
                oPdfPCell = new PdfPCell(new Phrase("Claim Order No", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyle));
            }
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.OrderNos, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.LCNo))
            {

                oPdfPCell = new PdfPCell(new Phrase("L/C NO.", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.LCNo, FontFactory.GetFont("Tahoma", 11f, 1)));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" ", FontFactory.GetFont("Tahoma", 11f, 1)));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("DO NO.:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.DONos, _oFontStyle));
            //oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("CARRIER", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.VehicleName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("VEHICLE NO.:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.VehicleNo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("DRIVER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.ReceivedByName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.Note))
            {

                oPdfPCell = new PdfPCell(new Phrase("REMARKS:", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.Note, _oFontStyle));
                //oPdfPCell.Border = 0;
               oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintHead_Sample()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 196f, 120f, 180f });

            oPdfPCell = new PdfPCell(new Phrase("DATE:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.ChallanDateSt, _oFontStyle));
            //oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("CHALLAN NO", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.ChallanNo, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.FactoryAddress))
            {
                oPdfPCell = new PdfPCell(new Phrase("ADDRESS", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.FactoryAddress, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            //if (!String.IsNullOrEmpty(_oDUDeliveryChallan.ContactPersonnelName))
            //{
            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.ContactPersonnelName, _oFontStyleBold));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                //oPdfPTable.CompleteRow();

                oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " CONCERN ", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.MKTPName, _oFontStyleBold));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

             if (!String.IsNullOrEmpty(_oDUOrderSetup.ShortName))
            {_oDUOrderSetup.ShortName = _oDUOrderSetup.ShortName.Replace("Order", "");}
            oPdfPCell = new PdfPCell(new Phrase(_oDUOrderSetup.ShortName+ " Order No", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.OrderNos, FontFactory.GetFont("Tahoma", 11f, 1)));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.LCNo))
            {

                oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.LCNo, FontFactory.GetFont("Tahoma", 11f, 1)));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" ", FontFactory.GetFont("Tahoma", 11f, 1)));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 2;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("DO No:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.DONos, _oFontStyle));
            //oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("CARRIER", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.VehicleName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("VEHICLE NO.:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.VehicleNo, _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("DRIVER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.ReceivedByName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.Note))
            {

                oPdfPCell = new PdfPCell(new Phrase("REMARKS:", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.Note, _oFontStyle));
                //oPdfPCell.Border = 0;
               oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.DeliveryZone))
            {

                oPdfPCell = new PdfPCell(new Phrase("Delivery info:", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oDUDeliveryChallan.DeliveryZone, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void Print_Body()
        {
            _nTotalQty = 0;
            PdfPTable oPdfPTable = new PdfPTable(8);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 140f, 110f, 85f, 65f, 40f, 40f, 55f, });
            int nProductID = 0;
            _nTotalQty = 0;
            _nCount = 0;


            if (_oDUDeliveryChallanDetails.Count > 0)
            {
                _oDUDeliveryChallanDetails = _oDUDeliveryChallanDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Goods Description", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Bag", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Hank/Cone", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPTable.CompleteRow();

                foreach (DUDeliveryChallanDetail oItem in _oDUDeliveryChallanDetails)
                {
                    _nCount++;


                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oItem.ProductName, _oFontStyle));
                    if (!String.IsNullOrEmpty(oItem.Note))
                    {
                        _oPhrase.Add(new Chunk("\n"+oItem.Note, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                    }

                   // oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell = new PdfPCell(_oPhrase);
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    if(String.IsNullOrEmpty(oItem.RefNo))
                    {
                        oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    }
                    {
                        oPdfPCell = new PdfPCell(new Phrase(oItem.RefNo, _oFontStyle));
                    }
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    if (!string.IsNullOrEmpty(oItem.GYLotNo)) { oItem.LotNo = oItem.GYLotNo; }

                    oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.BagQty.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.HanksCone.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                    nProductID = oItem.ProductID;
                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalBagQty = _nTotalBagQty + oItem.BagQty;
                    _nTotalCone += oItem.HanksCone;

                }
                for (int i = _oDUDeliveryChallanDetails.Count + 1; i <= (8 - _oDUDeliveryChallanDetails.Count); i++)
                {
                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight; 
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = 20f; 
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();


                }
                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalBagQty.ToString()+"\n Bag", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalCone.ToString() + "\n Cone", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + "\n" + _sMUnit, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Total in World

                string sTemp = "";
                sTemp = Global.DollarWords(_nTotalQty);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    sTemp = sTemp.Replace("Dollar", "");
                    sTemp = sTemp.Replace("Only", "");
                    sTemp = sTemp.ToUpper();
                }
                _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
                _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region ACKNOWLEDGEMENT

                _oPdfPCell = new PdfPCell(new Phrase("GOODS RECEIVED IN GOOD ORDER, GOOD CONDITION & MENTIONED QUANTITY", _oFontStyleBold));
                _oPdfPCell.Colspan = oPdfPTable.NumberOfColumns;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void Print_Body_Bulk()
        {
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f, 175f, 90f,70f,  60f, 40f, 70f, });
            int nProductID = 0;
            _nTotalQty = 0;
            _nCount = 0;


            if (_oDUDeliveryChallanDetails.Count > 0)
            {
                _oDUDeliveryChallanDetails = _oDUDeliveryChallanDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Goods Description", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("COLOR", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(_oDUDeliveryChallan.OrderCode, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                //oPdfPCell = new PdfPCell(new Phrase("Buyer Ref", _oFontStyleBold));
                //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(" Bag", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPTable.CompleteRow();

                foreach (DUDeliveryChallanDetail oItem in _oDUDeliveryChallanDetails)
                {
                    _nCount++;


                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(oItem.ProductName, _oFontStyle));
                    if (!String.IsNullOrEmpty(oItem.RefNo))
                    {
                        _oPhrase.Add(new Chunk("\n Buyer Ref:" + oItem.RefNo, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.ITALIC)));
                    }
                    if (!String.IsNullOrEmpty(oItem.Note))
                    {
                        _oPhrase.Add(new Chunk("\n" + oItem.Note, FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL)));
                    }


                    // oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell = new PdfPCell(_oPhrase);
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    //if (String.IsNullOrEmpty(oItem.RefNo))
                    //{
                    //    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //}
                    //else
                    //{
                    //    oPdfPCell = new PdfPCell(new Phrase(oItem.RefNo, _oFontStyle));
                    //}
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    ////oPdfPCell.FixedHeight = 40f;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //oPdfPTable.AddCell(oPdfPCell);
                    if (!string.IsNullOrEmpty(oItem.GYLotNo)) { oItem.LotNo = oItem.GYLotNo; }
                    oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;

                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.BagQty.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    oPdfPTable.CompleteRow();

                    _nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);
                    _nUsagesHeight = _nUsagesHeight + CalculatePdfPTableHeight(oPdfPTable);
                    if (_nUsagesHeight > 750)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                        _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                        _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.UNDERLINE);
                     
                        _oPdfPCell = new PdfPCell();
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();


                        _nUsagesHeight = 0;
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();

                        oPdfPTable.DeleteBodyRows();
                        _oPdfPTable.DeleteBodyRows();

                        if (_oDUDeliveryChallan.ApproveBy == 0)
                        {
                            ReporttHeaderUnauthorised();
                            PrintWaterMark();
                        }
                        else
                        {
                            this.HeaderWithThreeFormats();
                            this.ReporttHeader();
                        }
          
                        oPdfPTable = new PdfPTable(7);
                        oPdfPTable.SetWidths(new float[] { 30f, 175f, 90f, 70f, 60f, 40f, 70f });

                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                        _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    }
                 

                  
                    #endregion
                    nProductID = oItem.ProductID;
                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalBagQty = _nTotalBagQty + oItem.BagQty;

                }
                for (int i = _oDUDeliveryChallanDetails.Count + 1; i <= (8 - _oDUDeliveryChallanDetails.Count); i++)
                {
                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = 20f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    //oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = _nfixedHight;
                    //oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();


                }
                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalBagQty.ToString() + "\n Bag", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + "\n" + _sMUnit, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Total in World

                string sTemp = "";
                sTemp = Global.DollarWords(_nTotalQty);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    sTemp = sTemp.Replace("Dollar", "");
                    sTemp = sTemp.Replace("Only", "");
                    sTemp = sTemp.ToUpper();
                }
                _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region ACKNOWLEDGEMENT

                _oPdfPCell = new PdfPCell(new Phrase("GOODS RECEIVED IN GOOD ORDER, GOOD CONDITION & MENTIONED QUANTITY", _oFontStyleBold));
                _oPdfPCell.Colspan = 7;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        private void PrintFooter()
        {
            //_oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 148f, 148f, 148f, 148f });




            oPdfPCell = new PdfPCell(new Phrase("WAREHOUSE INCHARGE", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("FACTORY MANAGER", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("FACTORY G.M", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("RECEIVED BY", _oFontStyleBold));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, 0)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);



            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintFooter(List<ApprovalHead> oApprovalHeads)
        {
            int nCoumn = oApprovalHeads.Count();
            PdfPTable oPdfPTable = new PdfPTable(nCoumn);
            PdfPCell oPdfPCell;

            float [] nWidths = new float [nCoumn];
            for (int i = 0; i < nCoumn; i++) nWidths[i] = 592 / nCoumn;
            oPdfPTable.SetWidths(nWidths);

            foreach (ApprovalHead oItem in oApprovalHeads)
            {
                oPdfPCell = new PdfPCell(new Phrase(oItem.Name, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;

                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
            }
            oPdfPTable.CompleteRow();

            foreach (ApprovalHead oItem in oApprovalHeads)
            {
                oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, 0)));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
                oPdfPCell.FixedHeight = 35f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
            }

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
        #endregion

        #region
        public byte[] PrepareReport_GatePass(DUDeliveryChallan oDUDeliveryChallan, Company oCompany, BusinessUnit oBusinessUnit, int nTitleType, DUOrderSetup oDUOrderSetup, List<ApprovalHead> oApprovalHeads)
        {
            _oDUDeliveryChallan = oDUDeliveryChallan;
            _oBusinessUnit = oBusinessUnit;
            _nTitleType = nTitleType;
            _oDUOrderSetup = oDUOrderSetup;
            _oDUDeliveryChallanDetails = oDUDeliveryChallan.DUDeliveryChallanDetails;
            _oDeliverySetup = oDUDeliveryChallan.DeliverySetup;
            _oCompany = oCompany;
            if (_oDUDeliveryChallanDetails.Count > 0)
            {
                _sMUnit = _oDUDeliveryChallanDetails[0].MUnit;

            }

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 10f, 30f);
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

            //this.PrintHeader();
            this.HeaderWithThreeFormats();
            this.ReporttHeader_GatePass();
            this.PrintHead_GatePass();
            Print_Body_GatePass();
            PrintApprovalHead(oApprovalHeads);
            //_oPdfPTable.HeaderRows = 4;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void ReporttHeader_GatePass()
        {
            #region Proforma Invoice Heading Print
            _oPdfPCell = new PdfPCell(new Phrase("GATE PASS", FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (_oDUDeliveryChallan.ApproveBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised Gate Pass", FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            
            #endregion
        }
        private void PrintHead_GatePass()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 106f, 190f, 120f, 180f });

            oPdfPCell = new PdfPCell(new Phrase("DATE:", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.ChallanDateSt, _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Gate Pass No.", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            if (string.IsNullOrEmpty(_oDUDeliveryChallan.GatePassNo)) { _oDUDeliveryChallan.GatePassNo = _oDUDeliveryChallan.ChallanNo; }

            oPdfPCell = new PdfPCell(new Phrase(": GPY-" + _oDUDeliveryChallan.GatePassNo, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            string sContractor = "BUYER NAME";
            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.LoanOrder)
                sContractor = "SUPPLIER NAME";
            oPdfPCell = new PdfPCell(new Phrase(sContractor, _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": "+ _oDUDeliveryChallan.ContractorName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("ChallanNo.", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": DCY-" + _oDUDeliveryChallan.ChallanNo, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.FactoryAddress))
            {
                oPdfPCell = new PdfPCell(new Phrase("ADDRESS", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.FactoryAddress, _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            //
            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.ContactPersonnelName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " CONCERN ", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.MKTPName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //

            if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.BulkOrder || _oDUDeliveryChallan.OrderType == (int)EnumOrderType.DyeingOnly)
            {
                oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyle));

                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.OrderNos, FontFactory.GetFont("Tahoma", 11f, 1)));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);




                oPdfPCell = new PdfPCell(new Phrase("DO No:", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.DONos, _oFontStyle));
                oPdfPCell.Border = 0;

                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            oPdfPCell = new PdfPCell(new Phrase("CARRIER", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": "+_oDUDeliveryChallan.VehicleName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DO No", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.DONos, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("VEHICLE NO.", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.VehicleNo, _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("DRIVER NAME", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": "+_oDUDeliveryChallan.ReceivedByName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.Note))
            {
                oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.Note, _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            //


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void Print_Body_GatePass()
        {
            _nTotalQty = 0;
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
            PdfPTable oPdfPTable = new PdfPTable(6);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 30f,110f, 250f, 60f, 70f, 80f, });
            int nProductID = 0;
            _nTotalQty = 0;
            _nCount = 0;

            _oDUDeliveryChallanDetails = _oDUDeliveryChallanDetails.GroupBy(x => new { x.ProductID, x.ProductName, x.OrderNo, x.MUnit }, (key, grp) =>
                                          new DUDeliveryChallanDetail
                                          {
                                              ProductID = key.ProductID,
                                              ProductName = key.ProductName,
                                              OrderNo = key.OrderNo,
                                              MUnit = key.MUnit,
                                              Qty = grp.Sum(p => p.Qty),
                                              BagQty = grp.Sum(p => p.BagQty),
                                          }).ToList();

            if (_oDUDeliveryChallanDetails.Count > 0)
            {
                _oDUDeliveryChallanDetails = _oDUDeliveryChallanDetails.OrderBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Goods Description", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);///////


                string sTypeOfCount = "Bag";
                if ((int)_oDUDeliveryChallan.PackCountBy > 0)
                    sTypeOfCount = _oDUDeliveryChallan.PackCountByInSt;
                oPdfPCell = new PdfPCell(new Phrase(sTypeOfCount, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty/" + sTypeOfCount + "(" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Qty (" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);



                oPdfPTable.CompleteRow();

           

                foreach (DUDeliveryChallanDetail oItem in _oDUDeliveryChallanDetails)
                {
                    _nCount++;

                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.BagQty.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty / oItem.BagQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                    nProductID = oItem.ProductID;
                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalBagQty = _nTotalBagQty + oItem.BagQty;

                }

                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 3;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalBagQty.ToString() + "\n" + sTypeOfCount, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) + "\n" + _sMUnit, _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                #region Total in World

                string sTemp = "";
                sTemp = Global.DollarWords(_nTotalQty);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    sTemp = sTemp.Replace("Dollar", "");
                    sTemp = sTemp.Replace("Only", "");
                    sTemp = sTemp.ToUpper();
                }
                _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
                _oPdfPCell.Colspan = 6;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

              

            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = 4;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            ////_oPdfPCell = new PdfPCell(oPdfPTable);
            ////_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            ////_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            ////_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
        }
        #endregion

        #region
        public byte[] PrepareReportNew(DUDeliveryChallan oDUDeliveryChallan, Company oCompany, BusinessUnit oBusinessUnit, int nTitleType, DUOrderSetup oDUOrderSetup, List<ApprovalHead> oApprovalHeads)
        {
            _oDUDeliveryChallan = oDUDeliveryChallan;
            _oBusinessUnit = oBusinessUnit;
            _nTitleType = nTitleType;
            _oDUOrderSetup = oDUOrderSetup;
            _oDUDeliveryChallanDetails = oDUDeliveryChallan.DUDeliveryChallanDetails;
            _oCompany = oCompany;
            if (_oDUDeliveryChallanDetails.Count > 0)
            {
                _sMUnit = _oDUDeliveryChallanDetails[0].MUnit;

            }

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            #endregion

            //this.PrintHeader();
            if (_oDUDeliveryChallan.ApproveBy == 0)
            {
              //  ReporttHeaderUnauthorised();
                this.ReporttHeader();
                PrintWaterMark();
            }
            else
            {
                this.HeaderWithThreeFormats();
                this.ReporttHeader();
            }
            this.PrintHead_SampleNew();
            this.Print_BodyNew();
         
            PrintApprovalHead(oApprovalHeads);
            //if (_oDUDeliveryChallan.ApproveBy > 0)
            //{
            //    this.PrintFooter();
            //}
            ////////////////_oPdfPTable.HeaderRows = 4;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void PrintApprovalHead(List<ApprovalHead> oApprovalHeads)
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            int tblCol=0;
            float colWidth;
            float[] tblColSizeArray = new float[] { };
            
            if (oApprovalHeads.Count <= 0)
            {
                ApprovalHead oApprovalHead = new ApprovalHead();
                oApprovalHead.Name = "Prepared by";
                oApprovalHeads.Add(oApprovalHead);
                oApprovalHead = new ApprovalHead();
                oApprovalHead.Name = "Approved by";
                oApprovalHeads.Add(oApprovalHead);
            }

            if (oApprovalHeads.Count <= 5)
            {
                tblColSizeArray = new float[oApprovalHeads.Count];
                colWidth = 595 / oApprovalHeads.Count;
                tblCol = oApprovalHeads.Count;
                for (int i = 0; i < oApprovalHeads.Count; i++)
                {
                    tblColSizeArray[i] = colWidth;
                }
                if (_oDUDeliveryChallan.OrderType == (int)EnumOrderType.LoanOrder)
                {
                    oApprovalHeads[1].Name = "Section In-charge";
                    oApprovalHeads[2].Name = "User Dept";
                    oApprovalHeads[3].Name = "M. M. (Dept)";
                }
            }
            else
            {
                tblColSizeArray = new float[5];
                colWidth = 595 / 5;
                tblCol = 5;
                tblColSizeArray = new float[] { colWidth, colWidth, colWidth, colWidth, colWidth };
            }
            

            PdfPTable oPdfPTable = new PdfPTable(tblCol);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(tblColSizeArray);


            for (int i = 0; i < oApprovalHeads.Count;i++ )
            {

                oPdfPCell = new PdfPCell(new Phrase("-----------------\n"+oApprovalHeads[i].Name, _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.FixedHeight = 50; oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            _oPdfPTable.AddCell(oPdfPTable);
            //_oDocument.Add(oPdfPTable);
        }
        private void PrintWaterMark()
        {
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                ESimSolWM_Footer.WMFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                ESimSolWM_Footer.WMFontSize = 75;
                ESimSolWM_Footer.WMRotation = 45;
                ESimSolWM_Footer PageEventHandler = new ESimSolWM_Footer();
                PageEventHandler.WaterMark = "Unauthorized"; //Footer print with page event handler
                PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            PdfContentByte cb = PDFWriter.DirectContent;
            _oDocument.NewPage();
        }
        
        

        


        #region Report Body
     
        private void PrintHead_SampleNew()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 102f, 205f, 120f, 165f });

            oPdfPCell = new PdfPCell(new Phrase("DATE", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.ChallanDateSt, _oFontStyle));
            oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("CHALLAN NO", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": DCY-" + _oDUDeliveryChallan.ChallanNo, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("BUYER NAME", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.ContractorName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("GATE PASS NO", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            if (string.IsNullOrEmpty(_oDUDeliveryChallan.GatePassNo)) { _oDUDeliveryChallan.GatePassNo = _oDUDeliveryChallan.ChallanNo; }
            oPdfPCell = new PdfPCell(new Phrase(": GPY-" + _oDUDeliveryChallan.GatePassNo, _oFontStyle));
            oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            string nDeliveryPoint = _oDUDeliveryChallanDetails.Select(x => x.DeliveryPoint).FirstOrDefault();

            if (!String.IsNullOrEmpty(nDeliveryPoint))
            {
                _oDUDeliveryChallan.FactoryAddress = nDeliveryPoint;
            }

            if (!String.IsNullOrEmpty(_oDUDeliveryChallan.FactoryAddress))
            {
                oPdfPCell = new PdfPCell(new Phrase("ADDRESS", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.FactoryAddress, _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            //if (!String.IsNullOrEmpty(_oDUDeliveryChallan.ContactPersonnelName))
            //{
            oPdfPCell = new PdfPCell(new Phrase("CONCERN PERSON", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.ContactPersonnelName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " CONCERN ", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.MKTPName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            //oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("DO NO.", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.DONos, _oFontStyle));
            oPdfPCell.Border = 0;

            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("CARRIER", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": "+_oDUDeliveryChallan.VehicleName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("VEHICLE NO.:", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.VehicleNo, _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("DRIVER NAME", _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(": "+_oDUDeliveryChallan.ReceivedByName, _oFontStyleBold));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //if (!String.IsNullOrEmpty(_oDUDeliveryChallan.Note))
            //{

                oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.Note, _oFontStyle));
                oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("DELIVERY MAN", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.DeliveryBy, _oFontStyleBold));
                oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            //}
                if (!String.IsNullOrEmpty(_oDUDeliveryChallan.DeliveryZone))
                {

                oPdfPCell = new PdfPCell(new Phrase("DELIVERY POINT", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.DeliveryZone, _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
              
                }
                //if (!String.IsNullOrEmpty(_oDUDeliveryChallan.Note))
                //{

                //    oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyle));
                //    oPdfPCell.Border = 0;
                //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //    oPdfPTable.AddCell(oPdfPCell);

                //    oPdfPCell = new PdfPCell(new Phrase(": " + _oDUDeliveryChallan.Note, _oFontStyle));
                //    oPdfPCell.Border = 0;
                //    oPdfPCell.Colspan = 3;
                //    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //    oPdfPTable.AddCell(oPdfPCell);

                //}

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void Print_BodyNew()
        {
            _nTotalQty = 0;

            PdfPTable oPdfPTable = new PdfPTable(9);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 70f, 90f, 90f, 60f, 95f, 50f,35f, 34f, 70f });   //595
            int nProductID = 0;
            string sOrderNo = "";
            string sRefNo = "";
            _nTotalQty = 0;
            _nCount = 0;
            int _nCount_P = 0;
            int _nCount_RefNo = 0;

            if (_oDUDeliveryChallanDetails.Count > 0)
            {
                _oDUDeliveryChallanDetails = _oDUDeliveryChallanDetails.OrderBy(o => o.OrderNo).ThenBy(o => o.ProductID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Count", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Shade", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Batch No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Client Ref.", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" Qty(" + _sMUnit + ")", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Bag", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("No of Cone", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPTable.CompleteRow();

                foreach (DUDeliveryChallanDetail oItem in _oDUDeliveryChallanDetails)
                {
                    _nCount++;
                    if (string.IsNullOrEmpty(oItem.OrderNo)) {oItem.OrderNo="00"; }

                    #region PrintDetail
                    if (oItem.OrderNo != sOrderNo)
                    {
                        _nCount = _oDUDeliveryChallanDetails.Where(x => x.OrderNo == oItem.OrderNo).Count();

                        oPdfPCell = new PdfPCell(new Phrase(oItem.OrderNo, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                        ////oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPCell.Rowspan = (_nCount > 0) ? _nCount : 1;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    if (oItem.OrderNo != sOrderNo || nProductID != oItem.ProductID)
                    {
                        _nCount_P = _oDUDeliveryChallanDetails.Where(x => x.ProductID == oItem.ProductID && x.OrderNo == oItem.OrderNo).Count();

                        oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                        //oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.Rowspan = (_nCount_P > 0) ? _nCount_P : 1;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }
                    oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
                    if (oItem.OrderNo != sOrderNo || nProductID != oItem.ProductID || sRefNo != oItem.RefNo)
                    {
                        _nCount_RefNo = _oDUDeliveryChallanDetails.Where(x => x.ProductID == oItem.ProductID && x.OrderNo == oItem.OrderNo && x.RefNo == oItem.RefNo).Count();

                        oPdfPCell = new PdfPCell(new Phrase(oItem.RefNo, _oFontStyle));
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                        //oPdfPCell.FixedHeight = 40f;
                        oPdfPCell.Rowspan = (_nCount_RefNo > 0) ? _nCount_RefNo : 1;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        oPdfPTable.AddCell(oPdfPCell);
                    }

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.BagQty.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.HanksCone.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    //oPdfPCell.FixedHeight = 40f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                    nProductID = oItem.ProductID;
                    sOrderNo = oItem.OrderNo;
                    sRefNo = oItem.RefNo;
                    _nTotalQty = _nTotalQty + oItem.Qty;
                    _nTotalBagQty = _nTotalBagQty + oItem.BagQty;
                    _nTotalNoOfCone = _nTotalNoOfCone + oItem.HanksCone;
                }
               
                for (int i = _oDUDeliveryChallanDetails.Count + 1; i <= (15 - _oDUDeliveryChallanDetails.Count); i++)
                {
                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = 20f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.FixedHeight = _nfixedHight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();


                }
                #region Total
                _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                _oPdfPCell.Colspan = 5;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalQty) , _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(_nTotalBagQty.ToString() + "", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_nTotalNoOfCone.ToString() + "", _oFontStyleBold));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Total in World

                string sTemp = "";
                sTemp = Global.DollarWords(_nTotalQty);
                if (!String.IsNullOrEmpty(sTemp))
                {
                    sTemp = sTemp.Replace("Dollar", "");
                    sTemp = sTemp.Replace("Only", "");
                    sTemp = sTemp.ToUpper();
                }
                _oPdfPCell = new PdfPCell(new Phrase("IN WORDS: " + sTemp + " " + _sMUnit + " ONLY", _oFontStyleBold));
                _oPdfPCell.Colspan = 9;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region ACKNOWLEDGEMENT

                _oPdfPCell = new PdfPCell(new Phrase("NB: If the any discrepancy between this delivery challan and actual receiving, immediately inform the undersigned in written", _oFontStyle));
                _oPdfPCell.Colspan = 8;
                //_oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

            }
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
       
        #endregion
        #endregion

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
