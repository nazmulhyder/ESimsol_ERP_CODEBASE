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

    public class rptImportGRN
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        int _nTotalColumn = 1;
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        string _sIsCnf = "";
        Company _oCompany = new Company();
        Contractor _oContractor = new Contractor();
        bool _bIsInKg = true;
        int _nTitleTypeInImg = 0;
        ImportPI _oImportPI = new ImportPI();
        ImportInvoice _oImportInvoice = new ImportInvoice();
        List<GRN> _oGRNs = new List<GRN>();
        List<GRNDetail> _oGRNDetails = new List<GRNDetail>();
        Phrase _oPhrase = new Phrase();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<ImportInvoiceDetail> _oImportInvoiceDetails = new List<ImportInvoiceDetail>();
        List<ImportPack> _oImportPacks = new List<ImportPack>();
        List<ImportPackDetail> _oImportPackDetails = new List<ImportPackDetail>();
        ImportCnf _oImportCnf = new ImportCnf();
        string _sMUnit = "";
        #endregion

        #region Goods Receive Note
        public byte[] PrepareReport(List<GRN> oGRNs, List<GRNDetail> oGRNDetails, ImportInvoice oImportInvoice, Contractor oContractor, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = oBusinessUnit;
            _oContractor = oContractor;
            _oGRNs = oGRNs;
            _oGRNDetails = oGRNDetails;
            _oImportInvoice = oImportInvoice;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 3f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 
                                                    595f //Articale
                                              });
            #endregion

            this.PrintHeader();
            this.ReportHeaderForType();
            this.PrintBodyForType();
            if (_oGRNDetails.Count > 0)
            {
                this.SetDetail_GRN();
            }
            else
            {
                this.SetInvoiceDetail();
            }
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            _oPdfPCell = new PdfPCell(this.PrintHeader_Common());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
         
        }
        private void PrintHeader_Blank()
        {
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.FixedHeight = 150f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
     

        #endregion
        #region Report Body For Type
        private void ReportHeaderForType()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            if (_oImportInvoice.InvoiceType == EnumImportPIType.FancyYarn)
            {
                _oPdfPCell = new PdfPCell(new Phrase("FANCY YARN RECEIVE", _oFontStyle));
            }
            else if (_oImportInvoice.InvoiceType == EnumImportPIType.Servise)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Service Goods Receiving Report", _oFontStyle));
            }
            else 
            {
                _oPdfPCell = new PdfPCell(new Phrase("Goods Receiving Report", _oFontStyle));
            }
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1f; _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyForType()
        {
            string sGRNNo="", sChallanNo="";
            foreach (GRN oGRN in  _oGRNs)
            {
                sGRNNo = sGRNNo+oGRN.GRNNo + " DT: " + oGRN.GRNDateSt + ",";
                sChallanNo = sChallanNo + oGRN.ChallanNo + ",";
            }
            if (sGRNNo.Length>0)
            {
                sGRNNo = sGRNNo.Remove(sGRNNo.Length - 1, 1);
            }
            if (sChallanNo.Length > 0)
            {
                sChallanNo = sChallanNo.Remove(sChallanNo.Length - 1, 1);
            }

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 106f, 190f, 120f, 180f });

            oPdfPCell = new PdfPCell(new Phrase("GRN No & Date:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + sGRNNo, _oFontStyle));
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Invoice No & Date", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" "+_oImportInvoice.ImportInvoiceNo+" DT: "+ _oImportInvoice.InvoiceDateInString, _oFontStyleBold));
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("SUPPLIER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oContractor.Address))
            {
                oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oContractor.Address, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oContractor.Origin))
            {
                oPdfPCell = new PdfPCell(new Phrase("Country of Origin", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oContractor.Origin, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oImportPI.CPersonName) && !String.IsNullOrEmpty(_oImportPI.ConcernPersonName))
            {

                oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportPI.CPersonName, _oFontStyleBold));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportPI.ConcernPersonName, _oFontStyleBold));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

            }

            if (!String.IsNullOrEmpty(sChallanNo))
            {

                oPdfPCell = new PdfPCell(new Phrase("Challan No:", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + sChallanNo, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
            }
            
            if (!String.IsNullOrEmpty(_oImportPI.Note))
            {

                oPdfPCell = new PdfPCell(new Phrase("Remarks:", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportPI.Note, _oFontStyle));
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

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

             #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
                                   
        }

        private void SetDetail_GRN()
        {
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            GRNDetail oGRNDetail_Temp = new GRNDetail();
            List<ImportInvoiceDetail> oImportInvoiceDetails_Temp=new List<ImportInvoiceDetail>();
            oImportInvoiceDetails_Temp=_oImportInvoice.ImportInvoiceDetails;
            foreach (ImportInvoiceDetail oitem in _oImportInvoice.ImportInvoiceDetails)
            {
                ImportInvoiceDetail oImportInvoiceDetail = new ImportInvoiceDetail();
                oImportInvoiceDetail.ProductID = oitem.ProductID;
                oImportInvoiceDetail.ProductName = oitem.ProductName;
                oImportInvoiceDetail.UnitPrice = oitem.UnitPrice;
                oImportInvoiceDetail.MUName = oitem.MUName;
                oImportInvoiceDetail.MUSymbol = oitem.MUSymbol;
                oImportInvoiceDetail.Qty = oitem.Qty;
                _sMUnit = oImportInvoiceDetail.MUSymbol;
                _oImportInvoiceDetails.Add(oImportInvoiceDetail);
            }

            foreach (GRNDetail oitem in _oGRNDetails)
            {
                oImportInvoiceDetails_Temp.RemoveAll(x => x.ProductID == oitem.ProductID);
            }
            foreach(ImportInvoiceDetail oitem in   oImportInvoiceDetails_Temp)
            {
                oGRNDetail_Temp = new GRNDetail();
                oGRNDetail_Temp.ProductID = oitem.ProductID;
                oGRNDetail_Temp.ProductName = oitem.ProductName;
                oGRNDetail_Temp.QtyPerPack = 0;
                oGRNDetail_Temp.ReceivedQty = 0;
                oGRNDetail_Temp.ReceivedQty = 0;
                _oGRNDetails.Add(oGRNDetail_Temp);
            }

            _oGRNDetails = _oGRNDetails.OrderBy(o => o.ProductID).ToList();

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 200f, 80f, 90f, 75f, 80f, 85f});
            //oPdfPCell = new PdfPCell(new Phrase("Color Info", FontFactory.GetFont("Tahoma", 10f, 3)));
            //oPdfPCell.Colspan = 8;
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            #region Grid Column
            oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("INV.QTY\n(" + _sMUnit+")", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("LOT NO", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("RCVD.QTY\n(" + _sMUnit + ")", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("SHORT(-)\nEXCESS(+)", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

          
            oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            int nCount = 0;
            int nProductID = 0;
            int nRowSpan = 0;
            double nTempInvQty = 0;
            double nTempGRNQty = 0;
            string sGainLoss = "";
            
            if (_oGRNDetails.Count > 0)
            {
                foreach (GRNDetail oGRNDetail in _oGRNDetails)
                {

                    if (nProductID != oGRNDetail.ProductID)
                    {
                        nTempInvQty = 0;
                        nTempGRNQty = 0;
                        nRowSpan = _oGRNDetails.Where(P => P.ProductID == oGRNDetail.ProductID).ToList().Count;

                        if (nProductID>0 && nProductID != oGRNDetail.ProductID)
                        {
                            #region Sub Total
                         

                            oPdfPCell = new PdfPCell(new Phrase("  Total", _oFontStyleBold));
                            oPdfPCell.Colspan = 2;
                            //oPdfPCell.Rowspan = nRowSpan;
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            nTempInvQty = (_oImportInvoiceDetails.Where(c => c.ProductID == nProductID).Sum(x => x.Qty));

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            nTempGRNQty = (_oGRNDetails.Where(c => c.ProductID == nProductID).Sum(x => x.ReceivedQty));

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempGRNQty), _oFontStyleBold));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            if (Math.Round(nTempInvQty,3) < Math.Round(nTempGRNQty,3))
                            {
                               sGainLoss="+";
                            }
                            else if (Math.Round(nTempInvQty, 3) > Math.Round(nTempGRNQty, 3))
                            {
                                sGainLoss = "-";
                            }
                            else
                            {
                                sGainLoss = "";
                            }

                            oPdfPCell = new PdfPCell(new Phrase(sGainLoss+" "+Global.MillionFormat(Math.Abs(nTempInvQty - nTempGRNQty)), _oFontStyleBold));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                        }

                        /////
                        nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        oPdfPCell.Rowspan = nRowSpan; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oGRNDetail.ProductName, _oFontStyle));
                        //oPdfPCell.Colspan = 3;
                        oPdfPCell.Rowspan = nRowSpan;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        nTempInvQty = (_oImportInvoiceDetails.Where(c => c.ProductID == oGRNDetail.ProductID).Sum(x => x.Qty));

                        oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyle));
                        //oPdfPCell.Colspan = 2;
                        oPdfPCell.Rowspan = nRowSpan;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        oPdfPTable.AddCell(oPdfPCell);

                    }

                

                    oPdfPCell = new PdfPCell(new Phrase(oGRNDetail.LotNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oGRNDetail.ReceivedQty), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    nProductID = oGRNDetail.ProductID;
                }

                if (nProductID > 0 )
                {
                    #region Sub Total
                  

                    oPdfPCell = new PdfPCell(new Phrase("  Total", _oFontStyleBold));
                    oPdfPCell.Colspan = 2;
                    //oPdfPCell.Rowspan = nRowSpan;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    nTempInvQty = (_oImportInvoiceDetails.Where(c => c.ProductID == nProductID).Sum(x => x.Qty));

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    nTempGRNQty = (_oGRNDetails.Where(c => c.ProductID == nProductID).Sum(x => x.ReceivedQty));
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempGRNQty), _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    if (Math.Round(nTempInvQty, 3) < Math.Round(nTempGRNQty, 3))
                    {
                        sGainLoss = "+";
                    }
                    else if (Math.Round(nTempInvQty, 3) > Math.Round(nTempGRNQty, 3))
                    {
                        sGainLoss = "-";
                    }
                    else
                    {
                        sGainLoss = "";
                    }

                    oPdfPCell = new PdfPCell(new Phrase(sGainLoss+" "+Global.MillionFormat(Math.Abs(nTempInvQty - nTempGRNQty)), _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion 
                }

                #region Grand Total

                oPdfPCell = new PdfPCell(new Phrase("  Grand Total", _oFontStyleBold));
                oPdfPCell.Colspan = 2;
                //oPdfPCell.Rowspan = nRowSpan;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                nTempInvQty = _oImportInvoiceDetails.Sum(x => x.Qty);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty)+"\n"+_sMUnit, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                nTempGRNQty = _oGRNDetails.Sum(x => x.ReceivedQty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempGRNQty) + "\n" + _sMUnit, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);


                if (Math.Round(nTempInvQty, 3) < Math.Round(nTempGRNQty, 3))
                {
                    sGainLoss = "+";
                }
                else if (Math.Round(nTempInvQty, 3) > Math.Round(nTempGRNQty, 3))
                {
                    sGainLoss = "-";
                }
                else
                {
                    sGainLoss = "";
                }

                oPdfPCell = new PdfPCell(new Phrase(sGainLoss + " " + Global.MillionFormat(Math.Abs(nTempInvQty - nTempGRNQty)), _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion 

            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        private void SetInvoiceDetail()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            #region Detalil Print

            #region Hanger
            PdfPTable oDetailPdfPTable = new PdfPTable(4);
            oDetailPdfPTable.SetWidths(new float[] { 25f, 220f, 80f,  150f });

            if (_oImportInvoice.ImportInvoiceDetails.Count > 0)
            {
            #region Heading
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Description of Goods ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty (" + _oImportInvoice.ImportInvoiceDetails[0].MUName + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

            oDetailPdfPTable.CompleteRow();
            #endregion

           
                int nTempCount = 0; double nTotalAmount = 0, nTotalQty = 0;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                foreach (ImportInvoiceDetail oItem in _oImportInvoice.ImportInvoiceDetails)
                {
                    _oPdfPCell = new PdfPCell(new Phrase((++nTempCount).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);


                    nTotalQty += oItem.Qty;
                    nTotalAmount += oItem.Amount;
                    oDetailPdfPTable.CompleteRow();
                }

                int nRequiredRow = 5 - (_oImportInvoice.ImportInvoiceDetails.Count);
                for (int i = 1; i <= nRequiredRow; i++)
                {
                    #region Blank Row

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.MinimumHeight = 5f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.MinimumHeight = 15f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.MinimumHeight = 15f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                    _oPdfPCell.MinimumHeight = 15f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oDetailPdfPTable.AddCell(_oPdfPCell);

                    oDetailPdfPTable.CompleteRow();
                    #endregion
                }
                #region Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

              
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);


                oDetailPdfPTable.CompleteRow();
                #endregion
                
            }

            _oPdfPCell = new PdfPCell(oDetailPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #endregion



            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion



        }

        #endregion
        #endregion
        #region Packing List
        public byte[] Prepare_PackingList(ImportCnf oImportCnf, List<ImportPack> oImportPacks, List<ImportPackDetail> oImportPackDetails, ImportInvoice oImportInvoice, Contractor oContractor, Company oCompany, BusinessUnit oBusinessUnit, string sIsCnf)
        {
            _oBusinessUnit = oBusinessUnit;
            _oContractor = oContractor;
            _oImportInvoice = oImportInvoice;
            _oImportPacks = oImportPacks;
            _oImportPackDetails = oImportPackDetails;
            _oCompany = oCompany;
            _oImportCnf = oImportCnf;
            _sIsCnf = sIsCnf;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 3f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 
                                                    595f //Articale
                                              });
            #endregion

            this.PrintHeader();
            this.ReportHeaderForPackingList();
            if (_sIsCnf == "FromCNF")
            {
                this.PrintBodyForPackingList();
            }
            else if (_sIsCnf == "FromCustomInfo")
            {
                this.PrintBodyForPackingListCustomInfo();
            }
            //
            this.SetPackDetail();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void ReportHeaderForPackingList()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            

            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            if (_sIsCnf == "FromCNF")
            {
                _oPdfPCell = new PdfPCell(new Phrase("PACKING LIST", _oFontStyle));
            }
            else if (_sIsCnf == "FromCustomInfo")
            {
                _oPdfPCell = new PdfPCell(new Phrase("Delivery Notice", _oFontStyle));
            }
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1f; _oPdfPCell.FixedHeight = 20; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyForPackingListCustomInfo()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 106f, 220f, 120f, 150f });

            if (_oImportCnf.ImportCnfID > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase("Date:", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.DeliveryNoticeDateSt, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Document No:", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportCnf.FileNo, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            oPdfPCell = new PdfPCell(new Phrase("Invoice No & Date:", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ImportInvoiceNo + " DT: " + _oImportInvoice.InvoiceDateInString, _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Bill of Entry No:", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.BillofEntryNo, _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("L/C No & Date:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ImportLCNo + " DT: " + _oImportInvoice.ImportLCDateInString, _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Bill of Entry Date:", _oFontStyle));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.BillofEntryDateSt, _oFontStyleBold));
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("SUPPLIER NAME:", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ContractorName, _oFontStyleBold));
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            if (!String.IsNullOrEmpty(_oImportPI.CPersonName) && !String.IsNullOrEmpty(_oImportPI.ConcernPersonName))
            {
                oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportPI.CPersonName, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person", _oFontStyle));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportPI.ConcernPersonName, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void PrintBodyForPackingList()
        {
           

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 106f, 190f, 120f, 180f });

            if (_oImportCnf.ImportCnfID > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase("Date:", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportCnf.SendDateSt, _oFontStyle));
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
             
                oPdfPCell = new PdfPCell(new Phrase("Document No", _oFontStyle));
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
              

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportCnf.FileNo, _oFontStyle));
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            oPdfPCell = new PdfPCell(new Phrase("Invoice No & Date", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ImportInvoiceNo + " DT: " + _oImportInvoice.InvoiceDateInString, _oFontStyleBold));
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("L/C No & Date", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ImportLCNo + " DT: " + _oImportInvoice.ImportLCDateInString, _oFontStyleBold));
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("SUPPLIER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oContractor.Address))
            {
                oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oContractor.Address, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oContractor.Origin))
            {
                oPdfPCell = new PdfPCell(new Phrase("Country of Origin", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oContractor.Origin, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oImportPI.CPersonName) && !String.IsNullOrEmpty(_oImportPI.ConcernPersonName))
            {

                oPdfPCell = new PdfPCell(new Phrase("Concern Person", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportPI.CPersonName, _oFontStyleBold));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportPI.ConcernPersonName, _oFontStyleBold));
                //oPdfPCell.Border = 0;
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();

            }

      


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void SetPackDetail()
        {
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

          
            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 210f, 100f, 75f, 65f, 75f, 60f });
            //oPdfPCell = new PdfPCell(new Phrase("Color Info", FontFactory.GetFont("Tahoma", 10f, 3)));
            //oPdfPCell.Colspan = 8;
            //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();
            string sPack="Pack";
            if (_oImportInvoice.ImportInvoiceDetails.Count > 0)
            {
                if (_oImportPacks.Count > 0)
                {
                    sPack = _oImportPacks[0].PackCountByInString;
                    _sMUnit = _oImportPackDetails[0].MUName;
                }
            }
            if (_oImportInvoice.ImportInvoiceDetails.Count > 0)
            {
                _sMUnit = _oImportInvoice.ImportInvoiceDetails[0].MUSymbol;
            }

            #region Grid Column
            oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("LOT NO", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NO of " + sPack, _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("QTY/" + sPack, _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("QTY(" + _sMUnit+")", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            int nCount = 0;
            int nLotCount = 0;
            int nProductID = 0;
            int nRowSpan = 0;
            double nTempInvQty = 0;
        

            if (_oImportPackDetails.Count > 0)
            {
                foreach (ImportPackDetail oImportPackDetail in _oImportPackDetails)
                {

                    if (nProductID != oImportPackDetail.ProductID)
                    {
                        nTempInvQty = 0;
                       
                        nRowSpan = _oImportPackDetails.Where(P => P.ProductID == oImportPackDetail.ProductID).ToList().Count;


                        if (nProductID > 0 && nProductID != oImportPackDetail.ProductID && nLotCount>1)
                        {
                            #region Sub Total

                            oPdfPCell = new PdfPCell(new Phrase(" Total", _oFontStyleBold));
                            oPdfPCell.Colspan = 3;
                            //oPdfPCell.Rowspan = nRowSpan;
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            nTempInvQty = (_oImportPackDetails.Where(c => c.ProductID == nProductID).Sum(x => x.NumberOfPack));

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            nTempInvQty = (_oImportPackDetails.Where(c => c.ProductID == nProductID).Sum(x => x.Qty));

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);
                        

                            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                            nLotCount = 0;
                        }

                        /////
                        nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        oPdfPCell.Rowspan = nRowSpan; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oImportPackDetail.ProductName, _oFontStyle));
                        //oPdfPCell.Colspan = 3;
                        oPdfPCell.Rowspan = nRowSpan;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                    }

                    nLotCount++;
                    oPdfPCell = new PdfPCell(new Phrase(oImportPackDetail.LotNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oImportPackDetail.NumberOfPack), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oImportPackDetail.QtyPerPack), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oImportPackDetail.Qty), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oImportPackDetail.Remarks, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    nProductID = oImportPackDetail.ProductID;
                }

                if (nProductID > 0)
                {

                    #region Sub Total

                    oPdfPCell = new PdfPCell(new Phrase(" Total", _oFontStyleBold));
                    oPdfPCell.Colspan = 3;
                    //oPdfPCell.Rowspan = nRowSpan;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    nTempInvQty = (_oImportPackDetails.Where(c => c.ProductID == nProductID).Sum(x => x.NumberOfPack));

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    nTempInvQty = (_oImportPackDetails.Where(c => c.ProductID == nProductID).Sum(x => x.Qty));

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                }
                #region Grand Total

                oPdfPCell = new PdfPCell(new Phrase(" Grand Total", _oFontStyleBold));
                oPdfPCell.Colspan = 3;
                //oPdfPCell.Rowspan = nRowSpan;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                nTempInvQty = _oImportPackDetails.Sum(x => x.NumberOfPack);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty) + " " + sPack, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                nTempInvQty = _oImportPackDetails.Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty) + "\n" + _sMUnit, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion
              

            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


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
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
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
