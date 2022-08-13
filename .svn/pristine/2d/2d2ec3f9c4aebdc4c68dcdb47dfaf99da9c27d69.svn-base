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
    public class rptBookingStatus
    {
        #region Declaration

        int _nTotalColumn = 5;

        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(5);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        CapacityAllocation _oCapacityAllocation = new CapacityAllocation();
        List<CapacityAllocation> _oCapacityAllocations = new List<CapacityAllocation>();

        Company _oCompany = new Company();
        string _sMessage = "";

        #endregion

        public byte[] PrepareReport(CapacityAllocation oCapacityAllocation, Company oCompany)
        {
            _oCapacityAllocation = oCapacityAllocation;
            _oCapacityAllocations = oCapacityAllocation.CapacityAllocations;
            _oCompany = oCompany;


            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] {
                                                105f,  //Month
                                                120f, //Byer
                                                100f, //Allocated
                                                100f, //Order qty
                                                100f //total VAlue
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

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Booking Status", _oFontStyle));
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

        #region Report Body
        private void PrintBody()
        {
            #region Heading
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Month", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Allocated Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order/Booking Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Value", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Details Load
            DateTime dShipmentDate = DateTime.MinValue;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            if (_oCapacityAllocations.Count > 0)
            {
                foreach (CapacityAllocation oItem in _oCapacityAllocations)
                {
                    if (dShipmentDate.Month != oItem.ShipmentDate.Month || dShipmentDate == DateTime.MinValue)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentDateSt, _oFontStyle));
                        _oPdfPCell.Rowspan = _oCapacityAllocations.Where(x => x.ShipmentDate.Month == oItem.ShipmentDate.Month).Count(); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPTable.AddCell(_oPdfPCell);
                        dShipmentDate = oItem.ShipmentDate;
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Quantity), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OrderQty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderValueSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();
                }
            }
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.FixedHeight = 20; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Periodic Summery for 3 Months
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] {
                                                150f,  //Summary
                                                200f, //capacity
                                                180f //Value
                                                });

            #region 1st row
            _oPdfPCell = new PdfPCell(new Phrase("Periodic Summery ("+DateTime.Now.ToString("MMM yy")+"-"+DateTime.Now.AddMonths(2).ToString("MMM yy")+")", _oFontStyle));
            _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Capacity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oCapacityAllocations.Sum(x=>x.Quantity))+" Pcs", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 2nd row
            _oPdfPCell = new PdfPCell(new Phrase("Total Booking", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oCapacityAllocations.Sum(x => x.OrderQty)) + " Pcs", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region 3rd row
            _oPdfPCell = new PdfPCell(new Phrase("Free Capacity", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oCapacityAllocations.Sum(x => x.Quantity) - _oCapacityAllocations.Sum(x => x.OrderQty)) + " Pcs", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 5; _oPdfPCell.FixedHeight = 20; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Buyer Wise Summary
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] {
                                                30f,//SL
                                                150f, //Byer
                                                100f,  //Qty
                                                100f, //Valeu
                                                70f, //Avg FOB
                                                70f //%
                                                });


            #region Heading
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        

            _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Avg. FOB", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            List<CapacityAllocation> oDistinctBuyers = new List<CapacityAllocation>();
            oDistinctBuyers = _oCapacityAllocations.GroupBy(x=>x.BuyerID).Select(group => new CapacityAllocation
            {

                BuyerName = group.First().BuyerName,
                OrderQty = group.Sum(x=>x.OrderQty),
                OrderValue = group.Sum(x => x.OrderValue)
                }).ToList();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            int nCount = 0;
            foreach (CapacityAllocation oItem in oDistinctBuyers)
            {

                #region tabl Initialize
                oPdfPTable = new PdfPTable(6);
                oPdfPTable.SetWidths(new float[] {
                                                30f,//SL
                                                150f, //Byer
                                                100f,  //Qty
                                                100f, //Valeu
                                                70f, //Avg FOB
                                                70f //%
                                                });
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OrderQty)+" Pcs", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OrderValueSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("$"+Global.MillionFormat(oItem.OrderValue/oItem.OrderQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((oItem.OrderQty / oDistinctBuyers.Sum(x => x.OrderQty))*100), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
            }
            #region Total Print

            #region tabl Initialize
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] {
                                                30f,//SL
                                                150f, //Byer
                                                100f,  //Qty
                                                100f, //Valeu
                                                70f, //Avg FOB
                                                70f //%
                                                });
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oDistinctBuyers.Sum(x => x.OrderQty)) + " Pcs", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0;
             _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
             oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("$" + Global.MillionFormat(oDistinctBuyers.Sum(x => x.OrderValue)), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 2; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #endregion
        }
        #endregion

    }
}
