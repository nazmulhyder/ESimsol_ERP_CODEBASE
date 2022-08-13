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

    public class rptGUQC
    {
        #region Declaration
        int _nTotalColumn = 6;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle2;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPTable _oPdfPTableDetail = new PdfPTable(8);
        PdfPCell _oPdfPCell;
        PdfPCell _oPdfPCellDetail;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        GUQC _oGUQC = new GUQC();
        GUQCDetail _oGUQCDetail = new GUQCDetail();
        List<GUQC> _oGUQCs = new List<GUQC>();
        List<GUQCDetail> _oGUQCDetails = new List<GUQCDetail>();

        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(GUQC oGUQC, Company oCompany)
        {
            _oGUQC = oGUQC;
            _oGUQCDetails = oGUQC.GUQCDetails;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(25f, 25f, 5f, 25f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oPdfPTableDetail.WidthPercentage = 100;
            _oPdfPTableDetail.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PdfWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();

            //PageEventHandler.signatures = signatureList;
            PageEventHandler.nFontSize = 9;
            PdfWriter.PageEvent = PageEventHandler; //Footer print with page event handler

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 65f, 140f, 65f, 60f, 60f, 200f});
            _oPdfPTableDetail.SetWidths(new float[] { 25f, 110f, 110f, 130f, 55f, 55f, 55f, 55f });
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
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("QC", _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("QC Detail", _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 3f;
            _oPdfPTableDetail.AddCell(_oPdfPCell);
            _oPdfPTableDetail.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle2 = FontFactory.GetFont("Tahoma", 8f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("QC No: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oGUQC.QCNo, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("QC Date: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oGUQC.QCDateInString, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("QC By: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oGUQC.QCByName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Buyer: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oGUQC.BuyerName, _oFontStyle2)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approve Date: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oGUQC.ApproveDateInString, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Store: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oGUQC.StoreName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Approve By: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oGUQC.ApproveByName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remarks: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oGUQC.Remarks, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            #region break
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle2)); _oPdfPCell.FixedHeight = 10; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 6; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            #endregion


            #region Another table
            _oPdfPCellDetail = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Order No", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Order Qty", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("QC Pass", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Reject Qty", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("QC Done", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);
            
            _oPdfPTableDetail.CompleteRow();


            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            if (_oGUQCDetails.Count > 0)
            {
                foreach (GUQCDetail oItem in _oGUQCDetails)
                {
                    nCount++;
                    _oPdfPCellDetail = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.OrderRecapNo, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.Remarks, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.TotalQuantity.ToString("#,###.##"), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.QCPassQty.ToString("#,###.##"), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.RejectQty.ToString("#,###.##"), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.AlredyQCQty.ToString("#,###.##"), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPTableDetail.CompleteRow();
                }
            }
            #endregion
            #region push into main table
            _oPdfPCell = new PdfPCell(_oPdfPTableDetail);
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

    }
}
