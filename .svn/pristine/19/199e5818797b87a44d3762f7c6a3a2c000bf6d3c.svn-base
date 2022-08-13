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
    public class rptQCRegisters
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
        List<QCRegister> _oQCRegisters = new List<QCRegister>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(List<QCRegister> oQCRegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange)
        {
            _oQCRegisters = oQCRegisters;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
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
            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion

            #region Report Body & Header
            
            //this.PrintHeader("", sDateRange);
            //this.PrintBody(eReportLayout);
            //_oPdfPTable.HeaderRows = 3;

            if (eReportLayout == EnumReportLayout.ProductWise)
            {
                this.PrintHeader("Product Wise", sDateRange);
                this.PrintBody("Product Wise", "Product Name : ", eReportLayout);
                _oPdfPTable.HeaderRows = 4;
            }
            else if (eReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                this.PrintHeader("Buyer Wise", sDateRange);
                this.PrintBody("Buyer Wise", "Buyer Name : ", eReportLayout);
                _oPdfPTable.HeaderRows = 4;
            }
            else if (eReportLayout == EnumReportLayout.DateWiseDetails)
            {
                this.PrintHeader("Date Wise", sDateRange);
                this.PrintBody("Date Wise", "QC Date : ", eReportLayout);
                _oPdfPTable.HeaderRows = 4;
            }

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
            oPdfPTable.SetWidths(new float[] { 100f, 450f, 250f });

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
            _oPdfPCell = new PdfPCell(new Phrase("QC Register (" + sReportHeader + ")", _oFontStyle));
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

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        public PdfPTable GetDetailsTable() 
        {
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    60f,  //Sheet No
                                                    90f,  //Store- Date
                                                    90f,  //QCP
                                                    90f,  //Product
                                                    150f,  //Product Name
                                                    60f,   //M Unit
                                                    60f,   //Qty
                                                    60f,   //Qty
                                                    60f,   //Qty
                                                    90f,   //Amount
                                             });
            return oPdfPTable;
        }

        #region Report Body
        private void PrintBody(string sHeader, string sHeaderCoulmn, EnumReportLayout eReportLayout)  // EnumReportLayout eReportLayout as parameter
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            //string sHeader = "", sHeaderCoulmn = "";
            //#region Layout Wise Header
            //if (eReportLayout == EnumReportLayout.ProductWise)
            //{
            //    sHeader = "Product Wise"; sHeaderCoulmn = "Product Name : ";
            //}
            //else if (eReportLayout == EnumReportLayout.PartyWiseDetails)
            //{
            //    sHeader = "Buyer Wise"; sHeaderCoulmn = "Buyer Name : ";
            //}
            //else if (eReportLayout == EnumReportLayout.DateWiseDetails)
            //{
            //    sHeader = "Date Wise"; sHeaderCoulmn = "QC Date : ";
            //}
            //#endregion

            #region Group By Layout Wise
            var data = _oQCRegisters.GroupBy(x => new { x.BuyerName, x.BuyerID }, (key, grp) => new
            {
                HeaderName = key.BuyerName,
                TotalRejQty = grp.Sum(x => x.RejectQuantity),
                TotalPassQty = grp.Sum(x => x.PassQuantity),
                TotalAmount = grp.Sum(x => x.Amount),
                Results = grp.ToList()
            });

            if (eReportLayout == EnumReportLayout.ProductWise)
            {
                data = _oQCRegisters.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                {
                    HeaderName = key.ProductName,
                    TotalRejQty = grp.Sum(x => x.RejectQuantity),
                    TotalPassQty = grp.Sum(x => x.PassQuantity),
                    TotalAmount = grp.Sum(x => x.Amount),
                    Results = grp.ToList()
                });
            }
            else if (eReportLayout == EnumReportLayout.DateWiseDetails)
            {
                data = _oQCRegisters.GroupBy(x => new { x.OperationTimeInString }, (key, grp) => new
                {
                    HeaderName = key.OperationTimeInString,
                    TotalRejQty = grp.Sum(x => x.RejectQuantity),
                    TotalPassQty = grp.Sum(x => x.PassQuantity),
                    TotalAmount = grp.Sum(x => x.Amount),
                    Results = grp.ToList()
                });
            }
            #endregion

            //ESimSolPdfHelper.FontStyle = _oFontStyleBoldUnderLine;
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "QC Register (" + sHeader + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);

            #region Print Details

            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            #region Header
            PdfPTable oPdfPTable = GetDetailsTable();
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "#SL", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Sheet No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            if (eReportLayout == EnumReportLayout.DateWiseDetails)
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Store Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            else
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "QC Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "QC Person", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            if (eReportLayout == EnumReportLayout.ProductWise)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Lot No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Store Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            }
            else
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Product Code", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Product Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            }
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "M Unit", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Rej. Qty", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Pass Qty", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Unit Price", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Amount", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            #endregion
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            string sCurrencySymbol = "";
            foreach (var oItem in data)
            {
                ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeaderCoulmn+oItem.HeaderName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                

                int nCount = 0, nRowSpan =0, ProductionSheetID = 0;
                
                oPdfPTable = new PdfPTable(10);
                oPdfPTable = GetDetailsTable();

                #region Data
                foreach (var obj in oItem.Results)
                {
                    #region Order Wise Span
                    if (ProductionSheetID != obj.ProductionSheetID)
                    {
                        if (nCount > 0)
                        {
                            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);

                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.RejectQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.PassQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable,sCurrencySymbol+ Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.Amount)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            
                            oPdfPTable.CompleteRow();
                            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

                            oPdfPTable = new PdfPTable(10);
                            oPdfPTable = GetDetailsTable();
                        }

                        ESimSolPdfHelper.FontStyle = _oFontStyle;
                        nRowSpan = oItem.Results.Where(OrderR => OrderR.ProductionSheetID == obj.ProductionSheetID).ToList().Count+1;

                        ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.SheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        
                        if (eReportLayout == EnumReportLayout.DateWiseDetails)
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.StoreName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        else
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.OperationTimeInString, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);

                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.QCPersonName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    }
                    #endregion

                    if (eReportLayout == EnumReportLayout.ProductWise)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.LotNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.StoreName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    }
                    else
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductCode, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    }

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.MUName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.RejectQuantity), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.PassQuantity), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.CurrencySymbol + Global.MillionFormat(obj.UnitPrice), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.CurrencySymbol + Global.MillionFormat(obj.Amount), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                   
                    oPdfPTable.CompleteRow();
                    ProductionSheetID = obj.ProductionSheetID;
                    sCurrencySymbol = obj.CurrencySymbol;
                }
                #endregion

                #region Sub Total
                ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 3, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.RejectQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.PassQuantity)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sCurrencySymbol + Global.MillionFormat(oItem.Results.Where(x => x.ProductionSheetID == ProductionSheetID).Sum(x => x.Amount)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion

                #region Sub Total (Layout Wise)
                oPdfPTable = new PdfPTable(12);
                oPdfPTable = GetDetailsTable();

                ESimSolPdfHelper.AddCell(ref oPdfPTable, sHeader + " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 7, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.TotalRejQty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.TotalPassQty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, sCurrencySymbol + Global.MillionFormat(oItem.TotalAmount), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion
            }
            #region Grand Total
            PdfPTable oPdfPTable_GT = new PdfPTable(12);
            oPdfPTable_GT = GetDetailsTable();

            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, " Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 7, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(data.Sum(x=>x.TotalRejQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(data.Sum(x => x.TotalPassQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, sCurrencySymbol + Global.MillionFormat(data.Sum(x => x.TotalAmount)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

            oPdfPTable_GT.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_GT);
            #endregion

            #endregion
        }
        #endregion
    }
}
