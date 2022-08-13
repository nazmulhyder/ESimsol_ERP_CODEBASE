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
    public class rptFabricYarnDeliveryOrder
    {
        #region Declaration
        private int _nColumn = 8;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(8);
        private PdfPCell _oPdfPCell;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();

        private System.Drawing.Image signature;
        private Company _oCompany = new Company();
        FabricSalesContract _oFEO = new FabricSalesContract();
        FabricYarnDeliveryOrder _oFYDO = new FabricYarnDeliveryOrder();
        List<FabricYarnDeliveryChallanDetail> _oFYDChallanDetails = new List<FabricYarnDeliveryChallanDetail>();


        bool _isInKg = true;
        bool _bIsInText = true;
        string _sMoneyReceipts = "";
        iTextSharp.text.Font _oFontStyle_UnLine;
        #endregion

        public byte[] PrepareReport(FabricSalesContract oFEO, FabricYarnDeliveryOrder oFYDO, List<FabricYarnDeliveryChallanDetail> oFYDChallanDetails, Company oCompany, System.Drawing.Image sig, bool bPrintFormat, bool bIsInText)
        {
            _isInKg = bPrintFormat;
            _bIsInText = bIsInText;

            _oFEO = oFEO;
            _oFYDO = oFYDO;
            _oFYDChallanDetails = oFYDChallanDetails;
            _oCompany = oCompany;
            signature = sig;

            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 30f, 72f, 72f, 72f, 72f, 72f, 72f, 72f });

            #endregion

            this.PrintHeader();
            ApplicationPart();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(this.PrintPdfHeader());
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 15f, 4);

            _oPdfPCell = new PdfPCell(new Phrase("Yarn Transer DO", _oFontStyle_UnLine));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            this.Blank(10);
        }
        #endregion

        private PdfPTable PrintPdfHeader()
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f, 600f });



            oPdfPCell = new PdfPCell(this.LoadCompanyTitle());
            oPdfPCell.Border = 0;
            oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.Colspan = _nColumn;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }
        private PdfPTable LoadCompanyTitle()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(1);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 400f });
            iTextSharp.text.Image oImag;

            if (!_bIsInText)
            {
                if (_oCompany.CompanyTitle != null)
                {
                    oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyTitle, System.Drawing.Imaging.ImageFormat.Jpeg);
                    oImag.ScaleAbsolute(530f, 100f);
                    oPdfPCell1 = new PdfPCell(oImag);
                    oPdfPCell1.Border = 0;
                    oPdfPCell1.FixedHeight = 100f;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                    oPdfPTable1.AddCell(oPdfPCell1);
                    oPdfPTable1.CompleteRow();
                }
                else
                {
                    PadFormat();
                }
            }
            else
            {
                PadFormat();
            }
            return oPdfPTable1;
        }

        private void PadFormat()
        {

            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 15f, 4);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.FixedHeight = 150f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        private void ApplicationPart()
        {
            #region ReportHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"), _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("To", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Factory Head Spinnning", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name + "(Factory)", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.FactoryAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("DO No: " + _oFYDO.FYDNo, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 10f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Exe Order No: " + _oFEO.Note, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn ;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 10f;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(((_oFYDO.ApproveBy!=0) ? "Approve Date: " + _oFYDO.ApproveDateStr :""), _oFontStyle));
            _oPdfPCell.Colspan = _nColumn ;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 10f;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(((_oFYDO.ReviseNo != 0) ? "Revise No: R- " + _oFYDO.ReviseNo + ((_oFYDO.ReviseDateStr != "") ? " ( " + _oFYDO.ReviseDateStr + " )" : "") : ""), _oFontStyle));
            _oPdfPCell.Colspan = _nColumn ;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 10f;
            _oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase(((_oFYDO.ApproveBy!=0) ? "Approve Date: " + _oFYDO.ApproveDateStr :""), _oFontStyle));
            //_oPdfPCell.Colspan = 3;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 10f;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase(((_oFYDO.ReviseNo != 0) ? "Revise No: R- " + _oFYDO.ReviseNo + " ( " + _oFYDO.ReviseDateStr + " )" : ""), _oFontStyle));
            //_oPdfPCell.Colspan = _nColumn - 3;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 10f;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Dear Sir,", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            Phrase oPhrase = new Phrase();
            oPhrase.Add(new Chunk("Please take necessary steps for delivery of the following count yarn.", FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL)));

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Buyer : " + _oFEO.BuyerName, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }

        #region Report Body
        private void PrintBody()
        {
            #region Table Header

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            string sUnit = (_isInKg) ? "(Kg)" : "(Lbs)";

            PdfPTable oPdfPTable = TableYarnTransferDO();
            string[] tableHeader = new string[] { "SL", "Count", "Construction", "Fabric Type", "FEO No", "FEO Date", "FEO Qty", "Fabric ID", "Qty" + sUnit, "Rate", "Amount" };
            foreach (string head in tableHeader)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, head, 0, 0, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, true, 0, _oFontStyle);
            }
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, false, true);
            #endregion

            #region Table Info
            int nCount = 0;
            bool hasSpan = true;
            int nSpanCount = _oFYDO.FYDODetails.Count();

            if (nSpanCount > 0)
            {
                oPdfPTable = TableYarnTransferDO();
                foreach (FabricYarnDeliveryOrderDetail oItem in _oFYDO.FYDODetails)
                {


                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nCount).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    if (hasSpan)
                    {
                        //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEO.Construction, nSpanCount, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEO.ProcessTypeName, nSpanCount, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEO.FEONo, nSpanCount, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEO.OrderDateInStr, nSpanCount, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oFEO.Qty, 2), nSpanCount, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                        //ESimSolItexSharp.SetCellValue(ref oPdfPTable, _oFEO.FabricNo, nSpanCount, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                        hasSpan = false;
                    }

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(((_isInKg) ? oItem.Qty : oItem.QtyInLBS), 2), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(((_isInKg) ? oItem.UnitPrice : oItem.UnitPriceInLBS), 2), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(((_isInKg) ? oItem.TotalPrice : oItem.TotalPriceInLBS), 2), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle);
                    oPdfPTable.CompleteRow();


                }

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, false, true);

                //Total
                oPdfPTable = TableYarnTransferDO();
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 8, Element.ALIGN_RIGHT, BaseColor.WHITE, false, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(((_isInKg) ? _oFYDO.FYDODetails.Sum(x => x.Qty) : _oFYDO.FYDODetails.Sum(x => x.QtyInLBS)), 2), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(((_isInKg) ? _oFYDO.FYDODetails.Sum(x => x.TotalPrice) : _oFYDO.FYDODetails.Sum(x => x.TotalPriceInLBS)), 2), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, 0, _oFontStyle);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.LIGHT_GRAY, false, true);
            }
            else
            {
                ESimSolItexSharp.SetCellValue(ref _oPdfPTable, "", 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyle, true);
            }


            #endregion

            #region
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Please do the needful for the above as early as possible.", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Thanking you in anticipation.", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 50f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            this.Blank(20);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(this.SignaturePartForYarn());
            _oPdfPCell.Colspan = _nColumn - 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(this.CCToForYarn());
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 10f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            this.Note();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(this.ChallanList());
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
        private PdfPTable ChallanList()
        {
            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 50f, 150f, 85f,85f,100f, 100f, 200f });


            #region Title
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Count", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Qty (Kgs)", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Lot", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            if (_oFYDChallanDetails.Count > 0)
            {
                #region Detail
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

                int nCount = 0;
                double nTotalQty = 0;
                foreach (FabricYarnDeliveryChallanDetail oItem in _oFYDChallanDetails)
                {
                    nCount++;

                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ChallanDateStr, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.FYDChallanNo, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                  

                    nTotalQty += oItem.Qty;
                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                }
                #endregion

                #region total
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase("Total : ", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.Colspan = 3;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                int nCount = 0;

                for (int i = 0; i < 3; i++)
                {
                    nCount++;

                    oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                   
                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                }
            }

            return oPdfPTable;
        }
        private PdfPTable SignaturePartForYarn()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f });
            iTextSharp.text.Image oImag;



            if (signature != null)
            {
                oImag = iTextSharp.text.Image.GetInstance(signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(80f, 60f);
                oPdfPCell = new PdfPCell(oImag);
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPCell.Border = 0;
                oPdfPCell.FixedHeight = 30f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 9f, 4);
            //oPdfPCell = new PdfPCell(new Phrase((!string.IsNullOrEmpty(_oSUDeliveryOrder.ApprovedByName) ? "(" + _oSUDeliveryOrder.ApprovedByName + ")" : ""), _oFontStyle_UnLine));
            oPdfPCell = new PdfPCell(new Phrase(((_oFYDO.ApproveBy != 0) ? _oFYDO.ApproveByName : ""), _oFontStyle_UnLine));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            //oPdfPCell = new PdfPCell(new Phrase("GM(Marketing & Sales)", _oFontStyle));
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPCell.Border = 0;
            //oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //oPdfPTable.AddCell(oPdfPCell);
            //oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private void Blank(int nFixedHeight)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void Note()
        {
            //this.Blank(10);
            #region Note
            Phrase oPhrase = new Phrase();
            oPhrase.Add(new Chunk("Note (If Any): " + _oFYDO.Remark, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            oPhrase.Add(new Chunk("", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell = new PdfPCell(oPhrase);
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.TOP_BORDER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 10f;
            _oPdfPCell.FixedHeight = 40f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 10f;
            //_oPdfPCell.FixedHeight = 50f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }


        private PdfPTable CCToForYarn()
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 100f });

            oPdfPCell = new PdfPCell(new Phrase("C.C:To:", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("1) Store Department of Spinning [ATML]", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("2) Manager Dyeing, ATML, Factory.", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("3) Manager Weaving, ATML, Factory.", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("4) Store Weaving & Dyeing, ATML, Factory.", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("5) Manager, Accounts, ATML H/O, Dhaka.", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private PdfPTable TableYarnTransferDO()
        {
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.SetWidths(new float[] { 25f, 150f, 130f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f });
            return oPdfPTable;
        }

    }
}
