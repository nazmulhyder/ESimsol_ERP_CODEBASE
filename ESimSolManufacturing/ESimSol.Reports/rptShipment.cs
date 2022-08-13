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

    public class rptShipment
    {
        #region Declaration
        int _nTotalColumn = 6;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle2;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPTable _oPdfPTableDetail = new PdfPTable(12);
        PdfPCell _oPdfPCell;
        PdfPCell _oPdfPCellDetail;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Shipment _oShipment = new Shipment();
        ShipmentDetail _oShipmentDetail = new ShipmentDetail();
        List<Shipment> _oShipments = new List<Shipment>();
        List<ShipmentDetail> _oShipmentDetails = new List<ShipmentDetail>();

        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(Shipment oShipment, Company oCompany)
        {
            _oShipment = oShipment;
            _oShipmentDetails = oShipment.ShipmentDetails;
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
            _oPdfPTable.SetWidths(new float[] { 98f, 99f, 98f, 99f, 98f, 99f });
            _oPdfPTableDetail.SetWidths(new float[] { 25f, 65f, 75f, 55f, 40f, 40f, 40f, 45f, 45f, 45f, 40f, 70f});
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
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
            _oPdfPCell = new PdfPCell(new Phrase("Shipment", _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Shipment Detail", _oFontStyle));
            _oPdfPCell.Colspan = 12;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
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

            #region Shipment
            _oPdfPCell = new PdfPCell(new Phrase("Challan No: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.ChallanNo, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipment Date: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.ShipmentDateInString, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipment Mode: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.ShipmentModeInString, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Buyer: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.BuyerName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Store: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.StoreName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 3; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Truck: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.TruckNo, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Driver Name: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.DriverName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Mobile No: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.DriverMobileNo, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Depo: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.Depo, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Escord: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.Escord, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Factory: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.FactoryName, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Security Lock: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.SecurityLock, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Empty CTN Qty: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.EmptyCTNQty.ToString(), _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gum Tape Qty: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.GumTapeQty.ToString(), _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Remarks: ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase(_oShipment.Remarks, _oFontStyle2));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Colspan = 5; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(); _oPdfPCell.FixedHeight = 25;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumn; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Another table

            #region header
            _oPdfPCellDetail = new PdfPCell(new Phrase("#SL", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Recap No", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Lot No", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Style No", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Country", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Total Qty", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Balance", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Already Shipment", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Yet To Shipment", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Shipment Qty", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("CTN Qty", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPCellDetail = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

            _oPdfPTableDetail.CompleteRow();
            #endregion

            int nCount = 0;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            if (_oShipmentDetails.Count > 0)
            {
                foreach (ShipmentDetail oItem in _oShipmentDetails)
                {
                    nCount++;
                    _oPdfPCellDetail = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.OrderRecapNo, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.CountryName, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.TotalQuantity.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.Balance.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.AlreadyShipmentQty.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.YetToShipmentQty.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.ShipmentQty.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.CTNQty.ToString(), _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

                    _oPdfPCellDetail = new PdfPCell(new Phrase(oItem.Remarks, _oFontStyle));
                    _oPdfPCellDetail.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCellDetail.BackgroundColor = BaseColor.WHITE; _oPdfPTableDetail.AddCell(_oPdfPCellDetail);

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
