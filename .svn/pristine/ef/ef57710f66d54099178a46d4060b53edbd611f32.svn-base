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
using ESimSol.Reports;

namespace ESimSol.BusinessObjects
{
    public class rptSalePerformaceReport
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyleBoldUnderLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<SaleInvoice> _oSaleInvoices = new List<SaleInvoice>();
        List<SalesQuotation> _oSalesQuotations = new List<SalesQuotation>();
        Company _oCompany = new Company();

        int _nColspan = 10;
        #endregion

        public byte[] PrepareReport(List<SaleInvoice> oSaleInvoices, List<SalesQuotation> oSalesQuotation, Company oCompany, int eReportLayout, int nReportType, string sDateRange)
        {
            _oSaleInvoices = oSaleInvoices;
            _oSalesQuotations = oSalesQuotation;
            _oCompany = oCompany;

            if (eReportLayout != (int)EnumReportLayout.Month_Wise)
                _nColspan = 9;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(10f, 10f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842 });
            #endregion

            #region Report Body & Header

            if (nReportType==(int)EnumReportType.Sale_Invoice_Report)
                this.PrintBody(eReportLayout);
            else this.PrintBody_SQ(eReportLayout);
            _oPdfPTable.HeaderRows = 3;

            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(string sReportHeader, string sDateRange)
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 400f, 300f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #endregion
        }
        #endregion

        public PdfPTable GetDetailsTable(int eReportLayout) 
        {
                PdfPTable oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                if (eReportLayout != (int)EnumReportLayout.Month_Wise)
                {
                    oPdfPTable.SetWidths(new float[] { 
                                                    40f,  //SL 
                                                    80f,  //Ref Date
                                                    80f,  //Ref- NO
                                                    90f,  //Komm-File
                                                    120f,  //Customer
                                                    100f,  //Chessis
                                                    100f,   //Engine
                                                    60f,   //Status
                                                    80f,   //Price
                                                    90f,   //Mkt Person
                                             });
                }
                else
                {
                    oPdfPTable = new PdfPTable(11);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    70f,  //Ref Date
                                                    70f,  //Ref- NO
                                                    90f,   //Mkt Person

                                                    70f,  //Komm-File
                                                    120f,  //Customer
                                                    90f,  //Model
                                                    90f,  //Chessis
                                                    90f,   //Engine
                                                    60f,   //Status
                                                    60f,   //Price
                                             });
                }
            return oPdfPTable;
        }
      
        #region Report Body Sale Invoice
        private void PrintBody(int eReportLayout)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            string sHeader = "", sHeaderCoulmn = "";

            #region Layout Wise Header
            if (eReportLayout == (int)EnumReportLayout.Month_Wise)
            {
                sHeader = "Month Wise"; sHeaderCoulmn = "Month : ";
            }
            else if (eReportLayout == (int)EnumReportLayout.Marteking_Person_Wise)
            {
                sHeader = "Marteking Person Wise"; sHeaderCoulmn = "Marteking Person : ";
            }
            else if (eReportLayout == (int)EnumReportLayout.Vechile_Model_Wise)
            {
                sHeader = "Model Wise"; sHeaderCoulmn = "Model No : ";
            }
            #endregion

            #region Group By Layout Wise
            var data = _oSaleInvoices.GroupBy(x => new { x.MarketingAccountName, x.MarketingAccountID }, (key, grp) => new
            {
                HeaderName = key.MarketingAccountName,
                TotalNetAmount = grp.Sum(x => x.OTRAmount-(x.VatAmount+x.RegistrationFee)),
                Results = grp.ToList()
            });

            if (eReportLayout == (int)EnumReportLayout.Vechile_Model_Wise)
            {
                data = _oSaleInvoices.GroupBy(x => new { x.ModelNo, x.VehicleModelID }, (key, grp) => new
                {
                    HeaderName = key.ModelNo,
                    TotalNetAmount = grp.Sum(x => x.OTRAmount - (x.VatAmount + x.RegistrationFee)),
                    Results = grp.ToList()
                });
            }
            else if (eReportLayout == (int)EnumReportLayout.Month_Wise)
            {
                data = _oSaleInvoices.GroupBy(x => new { x.InvoiceDate.Year, x.InvoiceDate.Month }, (key, grp) => new
                {
                    HeaderName = grp.Select(x => x.InvoiceDate).FirstOrDefault().ToString("MMMM-yyyy"),
                    TotalNetAmount = grp.Sum(x => x.OTRAmount - (x.VatAmount + x.RegistrationFee)),
                    Results = grp.ToList()
                });
            }
            #endregion

            ESimSolPdfHelper.FontStyle = _oFontStyleBoldUnderLine;
            this.PrintHeader("Performance Report (" + sHeader + ")", "");
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Performance Report (" + sHeader + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);

            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            #region Header
            PdfPTable oPdfPTable = GetDetailsTable(eReportLayout);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "#SL", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Invoice No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Invoice Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            if (eReportLayout != (int)EnumReportLayout.Marteking_Person_Wise) //Customer, Model, Product
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Marteking Person", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Komm No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Customer Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            if (eReportLayout != (int)EnumReportLayout.Vechile_Model_Wise)
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Model No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Chassis No/ VIN", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Engine No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Status", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Amount", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            #endregion

            #region Print Details

            string sCurrencySymbol = "";
            foreach (var oItem in data)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeaderCoulmn+oItem.HeaderName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                int nCount = 0, nRowSpan =0, ProductionSheetID = 0;
                
                #region Data
                foreach (var obj in oItem.Results)
                {

                    ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable = GetDetailsTable(eReportLayout);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.InvoiceNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.InvoiceDateST, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                  
                    if (eReportLayout != (int)EnumReportLayout.Marteking_Person_Wise) //Customer, Model, Product
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.MarketingAccountName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.KommNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.CustomerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    if (eReportLayout != (int)EnumReportLayout.Vechile_Model_Wise)
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ModelNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ChassisNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.EngineNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.InvoiceStatusST, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, sCurrencySymbol+" "+Global.MillionFormatActualDigit((obj.OTRAmount-(obj.VatAmount+obj.RegistrationFee))), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);//requirement of sobuj vai

                    #region Order Wise Span
                    //if (ProductionSheetID != obj.ProductionSheetID)
                    //{
                    //    if (nCount > 0)
                    //    {
                    //        ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);

                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.RejectQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.PassQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable,sCurrencySymbol+ Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.Amount)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            
                    //        oPdfPTable.CompleteRow();
                    //        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

                    //        oPdfPTable = new PdfPTable(10);
                    //        oPdfPTable = GetDetailsTable(eReportLayout );
                    //    }

                    //    ESimSolPdfHelper.FontStyle = _oFontStyle;
                    //    nRowSpan = oItem.Results.Where(OrderR => OrderR.ProductionSheetID == obj.ProductionSheetID).ToList().Count+1;

                    //    ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    //    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.SheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        
                    //    if (eReportLayout == EnumReportLayout.Month_Wise)
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.StoreName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    //    else
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.OperationTimeInString, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);

                    //    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.QCPersonName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    //}
                    #endregion

                    oPdfPTable.CompleteRow();
                    //ProductionSheetID = obj.ProductionSheetID;
                    sCurrencySymbol = obj.CurrencySymbol;

                    oPdfPTable.CompleteRow();
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

                }
                #endregion

                #region Sub Total
                //ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 3, 0);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.RejectQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.PassQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, sCurrencySymbol + Global.MillionFormat(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.Amount)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                #endregion

                #region Sub Total (Layout Wise)
                oPdfPTable = new PdfPTable(11);
                oPdfPTable = GetDetailsTable(eReportLayout );
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sHeader + " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, _nColspan, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.TotalNetAmount), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion
            }
            #region Grand Total
            PdfPTable oPdfPTable_GT = new PdfPTable(12);
            oPdfPTable_GT = GetDetailsTable(eReportLayout );
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, " Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, _nColspan, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(data.Sum(x => x.TotalNetAmount)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            oPdfPTable_GT.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_GT);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            #endregion

            #endregion
        }
        #endregion

        #region Report Body SQ
        private void PrintBody_SQ(int eReportLayout)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            string sHeader = "", sHeaderCoulmn = "";
            #region Layout Wise Header
            if (eReportLayout == (int)EnumReportLayout.Month_Wise)
            {
                sHeader = "Month Wise"; sHeaderCoulmn = "Month : ";
            }
            else if (eReportLayout == (int)EnumReportLayout.Marteking_Person_Wise)
            {
                sHeader = "Marteking Person Wise"; sHeaderCoulmn = "Marteking Person : ";
            }
            else if (eReportLayout == (int)EnumReportLayout.Vechile_Model_Wise)
            {
                sHeader = "Model Wise"; sHeaderCoulmn = "Model No : ";
            }
            #endregion

            #region Group By Layout Wise
            var data = _oSalesQuotations.GroupBy(x => new { x.MarketingAccountName, x.MarketingPerson }, (key, grp) => new
            {
                HeaderName = key.MarketingAccountName,
                TotalOfferPrice = grp.Sum(x => x.OTRAmount - (x.VatAmount + x.RegistrationFee)),
                Results = grp.ToList()
            });

            if (eReportLayout == (int)EnumReportLayout.Vechile_Model_Wise)
            {
                data = _oSalesQuotations.GroupBy(x => new { x.ModelNo, x.VehicleModelID }, (key, grp) => new
                {
                    HeaderName = key.ModelNo,
                    TotalOfferPrice = grp.Sum(x => x.OTRAmount - (x.VatAmount + x.RegistrationFee)),
                    Results = grp.ToList()
                });
            }
            else if (eReportLayout == (int)EnumReportLayout.Month_Wise)
            {
                data = _oSalesQuotations.GroupBy(x => new { x.QuotationDate.Year, x.QuotationDate.Month }, (key, grp) => new
                {
                    HeaderName = grp.Select(x => x.QuotationDate).FirstOrDefault().ToString("MMMM-yyyy"),
                    TotalOfferPrice = grp.Sum(x => x.OTRAmount - (x.VatAmount + x.RegistrationFee)),
                    Results = grp.ToList()
                });
            }
            #endregion

            ESimSolPdfHelper.FontStyle = _oFontStyleBoldUnderLine;
            this.PrintHeader("Performance Report (" + sHeader + ")", "");
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Performance Report (" + sHeader + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);

            #region Header
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            PdfPTable oPdfPTable = GetDetailsTable(eReportLayout);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "#SL", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "SQ No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "SQ Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            
            if (eReportLayout != (int)EnumReportLayout.Marteking_Person_Wise) //Customer, Model, Product
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Marteking Person", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Komm No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Customer Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            if (eReportLayout != (int)EnumReportLayout.Vechile_Model_Wise)
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Model No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Chassis No/ VIN", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Engine No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Status", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Amount", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.LIGHT_GRAY, 15);

            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
            #endregion

            #region Print Details
            string sCurrencySymbol = "";
            foreach (var oItem in data)
            {
                ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeaderCoulmn + oItem.HeaderName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                int nCount = 0;
                #region Data
                foreach (var obj in oItem.Results)
                {
                    oPdfPTable = new PdfPTable(10);
                    oPdfPTable = GetDetailsTable(eReportLayout);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.FileNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.QuotationDateInString, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                   
                    if (eReportLayout != (int)EnumReportLayout.Marteking_Person_Wise) //Customer, Model, Product
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.MarketingAccountName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.KommNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.BuyerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                   
                    if (eReportLayout != (int)EnumReportLayout.Vechile_Model_Wise)
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ModelNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ChassisNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.EngineNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.SalesStatusInString, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit((obj.OTRAmount - (obj.VatAmount + obj.RegistrationFee))), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);//requirement of sobuj vai

                    #region Order Wise Span
                    //if (ProductionSheetID != obj.ProductionSheetID)
                    //{
                    //    if (nCount > 0)
                    //    {
                    //        ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);

                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.RejectQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.PassQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable,sCurrencySymbol+ Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.Amount)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    //        oPdfPTable.CompleteRow();
                    //        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

                    //        oPdfPTable = new PdfPTable(10);
                    //        oPdfPTable = GetDetailsTable(eReportLayout );
                    //    }

                    //    ESimSolPdfHelper.FontStyle = _oFontStyle;
                    //    nRowSpan = oItem.Results.Where(OrderR => OrderR.ProductionSheetID == obj.ProductionSheetID).ToList().Count+1;

                    //    ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    //    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.SheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);

                    //    if (eReportLayout == EnumReportLayout.Month_Wise)
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.StoreName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    //    else
                    //        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.OperationTimeInString, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);

                    //    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.QCPersonName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    //}
                    #endregion

                    oPdfPTable.CompleteRow();
                    //ProductionSheetID = obj.ProductionSheetID;
                    sCurrencySymbol = obj.CurrencyName;

                    oPdfPTable.CompleteRow();
                    ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

                }
                #endregion

                #region Sub Total
                //ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 3, 0);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.RejectQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.PassQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                //ESimSolPdfHelper.AddCell(ref oPdfPTable, sCurrencySymbol + Global.MillionFormat(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.Amount)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                #endregion

                #region Sub Total (Layout Wise)
                oPdfPTable = new PdfPTable(11);
                oPdfPTable = GetDetailsTable(eReportLayout );

                ESimSolPdfHelper.AddCell(ref oPdfPTable, sHeader + " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, _nColspan, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.TotalOfferPrice), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion
            }
            #region Grand Total
            PdfPTable oPdfPTable_GT = new PdfPTable(12);
            oPdfPTable_GT = GetDetailsTable(eReportLayout );

            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, " Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, _nColspan, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(data.Sum(x => x.TotalOfferPrice)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            oPdfPTable_GT.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_GT);
            #endregion

            #endregion
        }
        #endregion
    }
}
