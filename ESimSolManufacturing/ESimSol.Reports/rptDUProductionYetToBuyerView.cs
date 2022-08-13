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
using ICS.Core.Framework;
using System.Linq;

namespace ESimSol.Reports
{
    public class rptDUProductionYetToBuyerView
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(11);
        PdfPCell _oPdfPCell;
        int _nColumns = 17;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        DUProductionYetTo _oDUProductionYetTo = new DUProductionYetTo();
        List<DUProductionYetTo> _oDUProductionYetTos = new List<DUProductionYetTo>();
        List<DUDyeingType> _oDyeingType = new List<DUDyeingType>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        int nOrderDate;
        DateTime date1, date2;


        #endregion

        public byte[] PrepareReport(List<DUProductionYetTo> oDUProductionYetTos, Company oCompany, BusinessUnit oBusinessUnit, int gOrderDate, DateTime D1, DateTime D2)
        {
            _oDUProductionYetTos = oDUProductionYetTos;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            nOrderDate = gOrderDate;
            date1 = D1;
            date2 = D2;
            

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            //SL No,    Bank No, Bank Name,   Narration,  Date,   Authorized By,  Amount,Con. Rate,   Amount
            _oPdfPTable.SetWidths(new float[] { 30f, 182f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = (nOrderDate == 5) ? 5 : 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
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
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nColumns; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nColumns; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Production Status(Buyer View)", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 5f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            if (nOrderDate == 5)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Production Status between " + date1.ToString("dd MMM yyyy") + " to " + date2.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 5f;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

        }
        #endregion

        #region Report Body


        int RowHeight = 20; int nCount = 0;
        double OrderQty = 0;
        double ProdQty = 0;
        double YetToProduct = 0;
        double DeliveryQty = 0;
        double YetToDelivery = 0;
        double StockInHand = 0;
        double ColorCount = 0;
        double UnitQty = 0;
        private void PrintBody()
        {
            


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Qty" + "(" + _oDUProductionYetTos[0].MUName + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Production Qty" + "(" + _oDUProductionYetTos[0].MUName + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Pending Production" + "(" + _oDUProductionYetTos[0].MUName + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Delivered Qty" + "(" + _oDUProductionYetTos[0].MUName + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Pending Delivery" + "(" + _oDUProductionYetTos[0].MUName + ")", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Stock In Hand", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color Count", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Unit Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Required Time", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            
            _oPdfPTable.CompleteRow();





            foreach (DUProductionYetTo oItem in _oDUProductionYetTos)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                nCount++;


                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty_Prod), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double ytp;
                if ((oItem.Qty - oItem.Qty_Prod) < 0)
                {
                    ytp = 0.0;
                }
                else ytp = Convert.ToDouble(oItem.YetToProduction);
                _oPdfPCell = new PdfPCell(new Phrase(Math.Ceiling(ytp).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_DC.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double ytd;
                if ((oItem.Qty - oItem.Qty_DC) < 0)
                {
                    ytd = 0;
                }
                else ytd = Convert.ToDouble(oItem.YetToDelivery);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Ceiling(ytd).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.StockInHand), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Qty_Unit.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((oItem.ReqTime < 0) ? 0 : oItem.ReqTime), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                OrderQty += oItem.Qty;
                ProdQty += oItem.Qty_Prod;
                if ((oItem.Qty - oItem.Qty_Prod) > 0)
                {
                    YetToProduct += (oItem.Qty - oItem.Qty_Prod);
                }
                DeliveryQty += oItem.Qty_DC;
                if ((oItem.Qty - oItem.Qty_DC) > 0)
                {
                    YetToDelivery += (oItem.Qty - oItem.Qty_DC);
                }
                StockInHand += oItem.StockInHand;
                ColorCount += oItem.ColorCount;
                UnitQty += oItem.Qty_Unit;
            }
            PrintGT();
        }
        private void PrintGT()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);


            _oPdfPCell = new PdfPCell(new Phrase("Grand TOTAL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(OrderQty).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(ProdQty).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(YetToProduct).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(DeliveryQty).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(YetToDelivery).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(StockInHand).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(ColorCount).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(UnitQty).ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }
        #endregion

        #region yarn wise pending production
        public byte[] PrepareReport_PendingDyeing(List<DUProductionYetTo> oDUProductionYetTos, Company oCompany, BusinessUnit oBusinessUnit, string sDateRange)
        {
            _oDUProductionYetTos = oDUProductionYetTos;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;



            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            //SL No,    Bank No, Bank Name,   Narration,  Date,   Authorized By,  Amount,Con. Rate,   Amount
            _oPdfPTable.SetWidths(new float[] { 30f, 182f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f, 70f });
            #endregion

            this.PrintHeader();
            this.PrintBody_PendingDyeing();
            _oPdfPTable.HeaderRows = (nOrderDate == 5) ? 5 : 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBody_PendingDyeing()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            //string Header = "Tips Bills for Bank Acceptances (Maturity Letters) collection :";
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            string[] Headers = new string[] {"SL#", 
                                             "Category", 
                                             "Yarn Type", 
                                             "Dyeing Type/nA", 
                                             "Dyeing Time/nB", 
                                             "Order Qty", 
                                             "Pro. Qty", 
                                             "Pending Pro./nC", 
                                             "Delivery Qty ", 
                                             "Pending Delivery", 
                                                    };

            ESimSolPdfHelper.PrintHeaders(ref _oPdfPTable, Headers);
            int nCount = 1;

            double nQty = 0;
            double nQtyGain = 0;
            double nQtyLoss = 0;
            var oDyeingTypes = _oDUProductionYetTos.GroupBy(x => new { x.HankorCone }, (key, grp) =>
                                      new DUProductionYetTo
                                      {
                                          HankorCone = key.HankorCone,
                                      }).ToList();


            foreach (var oDyeingType in oDyeingTypes)
            {
                ESimSolPdfHelper.FontStyle = _oFontStyle;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, _oDyeingType.Where(x => x.DyeingType == oDyeingType.HankorCone).Select(x => x.Name).FirstOrDefault(), Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 10, 0, 18, 8);

                #region Data
                ESimSolPdfHelper.FontStyle = _oFontStyle;
                var oDUProductionYetTos= _oDUProductionYetTos.Where(x => x.HankorCone == oDyeingType.HankorCone).ToList();
                foreach (var oItem in oDUProductionYetTos)
                {
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, (nCount++).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ContractorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((EumDyeingType)oItem.HankorCone).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.Qty_Unit, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.Qty, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.Qty_Prod), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((oItem.Qty - oItem.Qty_Prod) <= 0) ? "" : Global.MillionFormat(oItem.Qty - oItem.Qty_Prod), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((oItem.Qty_DC) <= 0) ? "" : Global.MillionFormat(oItem.Qty_DC), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((oItem.Qty - oItem.Qty_DC) <= 0) ? "" : Global.MillionFormat(oItem.Qty - oItem.Qty_DC), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                }
                #endregion

                //ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                //#region Sub Total
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Sub Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 6, 0);
                //nQty = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Qty_RS);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.FreshDyedYarnQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);

                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Loss)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);


                //nQtyGain = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Gain);
                //nQtyGain = nQtyGain * 100 / nQty;
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyGain), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //nQtyLoss = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Loss);
                //nQtyLoss = nQtyLoss * 100 / nQty;
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyLoss), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
               // #endregion
            }
            //#region Total
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 6, 0);
            //nQty = _oRSFreshDyedYarns.Sum(x => x.Qty_RS);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.Qty_RS)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.FreshDyedYarnQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ////ESimSolPdfHelper.AddCell(ref _oPdfPTable,_oRSFreshDyedYarns.Sum(x => x.BagCount).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);

            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.Loss)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //nQtyGain = _oRSFreshDyedYarns.Sum(x => x.Gain);
            //nQtyGain = nQtyGain * 100 / nQty;
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyGain), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //nQtyLoss = _oRSFreshDyedYarns.Sum(x => x.Loss);
            //nQtyLoss = nQtyLoss * 100 / nQty;
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyLoss), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //#endregion
        }
        private void PrintBody_PendingDyeingSumary()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            //string Header = "Tips Bills for Bank Acceptances (Maturity Letters) collection :";
            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            string[] Headers = new string[] {"SL#", 
                                             "Dyeing Type", 
                                             "Dyeing Time/nB", 
                                             "Order Qty", 
                                             "Pro. Qty", 
                                             "Pending Pro./nC", 
                                             "Delivery Qty ", 
                                             "Pending Delivery", 
                                                    };

            ESimSolPdfHelper.PrintHeaders(ref _oPdfPTable, Headers);
            int nCount = 1;

            double nQty = 0;
            double nQtyGain = 0;
            double nQtyLoss = 0;
            var oDyeingTypes = _oDUProductionYetTos.GroupBy(x => new { x.HankorCone }, (key, grp) =>
                                      new DUProductionYetTo
                                      {
                                          HankorCone = key.HankorCone,
                                      }).ToList();


            foreach (var oDyeingType in oDyeingTypes)
            {
                ESimSolPdfHelper.FontStyle = _oFontStyle;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, _oDyeingType.Where(x => x.DyeingType == oDyeingType.HankorCone).Select(x => x.Name).FirstOrDefault(), Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 10, 0, 18, 8);

                #region Data
                ESimSolPdfHelper.FontStyle = _oFontStyle;
                var oDUProductionYetTos = _oDUProductionYetTos.Where(x => x.HankorCone == oDyeingType.HankorCone).ToList();
                foreach (var oItem in oDUProductionYetTos)
                {
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, (nCount++).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ContractorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((EumDyeingType)oItem.HankorCone).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.Qty_Unit, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, oItem.Qty, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(oItem.Qty_Prod), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((oItem.Qty - oItem.Qty_Prod) <= 0) ? "" : Global.MillionFormat(oItem.Qty - oItem.Qty_Prod), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((oItem.Qty_DC) <= 0) ? "" : Global.MillionFormat(oItem.Qty_DC), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                    ESimSolPdfHelper.AddCell(ref _oPdfPTable, ((oItem.Qty - oItem.Qty_DC) <= 0) ? "" : Global.MillionFormat(oItem.Qty - oItem.Qty_DC), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 10);
                }
                #endregion

                //ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                //#region Sub Total
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Sub Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 6, 0);
                //nQty = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Qty_RS);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.FreshDyedYarnQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);

                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Loss)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);


                //nQtyGain = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Gain);
                //nQtyGain = nQtyGain * 100 / nQty;
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyGain), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //nQtyLoss = _oRSFreshDyedYarns.Where(x => x.RSShiftID == oRSShit).Sum(x => x.Loss);
                //nQtyLoss = nQtyLoss * 100 / nQty;
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyLoss), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
                // #endregion
            }
            //#region Total
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Total", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15, 0, 6, 0);
            //nQty = _oRSFreshDyedYarns.Sum(x => x.Qty_RS);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.Qty_RS)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.FreshDyedYarnQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            ////ESimSolPdfHelper.AddCell(ref _oPdfPTable,_oRSFreshDyedYarns.Sum(x => x.BagCount).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);

            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.RecycleQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.WastageQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(_oRSFreshDyedYarns.Sum(x => x.Loss)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //nQtyGain = _oRSFreshDyedYarns.Sum(x => x.Gain);
            //nQtyGain = nQtyGain * 100 / nQty;
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyGain), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //nQtyLoss = _oRSFreshDyedYarns.Sum(x => x.Loss);
            //nQtyLoss = nQtyLoss * 100 / nQty;
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, Global.MillionFormat(nQtyLoss), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.LIGHT_GRAY, 15);
            //#endregion
        }


        #endregion
    }

}
