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
    public class rptDORegisters
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
        List<DORegister> _oDORegisters = new List<DORegister>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(List<DORegister> oDORegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange)
        {
            _oDORegisters = oDORegisters;
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
            
            this.PrintHeader("", sDateRange);
            this.PrintBody(eReportLayout);
            _oPdfPTable.HeaderRows = 6;

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
            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = 0;
            oPdfPTable.SetWidths(new float[] { 
                                                    20f,  //SL 
                                                    40f,  //PI No
                                                    38f,  //PI Date
                                                    45f,  //LC File No
                                                    
                                                    70f,  //BuyerName
                                                    70f,  //CustomerName
                                                    70f,   //ProductName
                                                    35f,  //ColorName 
                                                    47f,   //StyleNo
                                                    35f,   //PIQty
                                                    35f,   //DOQty
                                                    35f,   //YetToDO
                                                    35f,   //ChallanQty
                                                    35f   //YetToChallan
                                             });
            return oPdfPTable;
        }

        #region Report Body
        private void PrintBody(EnumReportLayout eReportLayout)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            string sHeader = "", sHeaderCoulmn = "";
            #region Layout Wise Header
            if (eReportLayout == EnumReportLayout.ProductCatagoryWise)
            {
                sHeader = "Catagory Wise"; sHeaderCoulmn = "Catagory Name : ";
            }
            else
            {
                sHeader = "Buyer Wise"; sHeaderCoulmn = "Buyer Name : ";
            }
            #endregion

            List<DORegister> oTempDORegister = new List<DORegister>();

            var data = oTempDORegister.GroupBy(x => new { x.BuyerID, x.BuyerName }, (key, grp) => new
            {
                HeaderName = key.BuyerName,
                Total_Qty = grp.Sum(x => x.Qty),
                Total_DOQty = grp.Sum(x => x.DOQty),
                Total_ChallanQty = grp.Sum(x => x.ChallanQty),
                Total_YetToDO = grp.Sum(x => x.YetToDO),
                Total_YetToChallan = grp.Sum(x => x.YetToChallan),
                Results = grp.ToList()
            });

             #region Group By Layout Wise
             if (eReportLayout == EnumReportLayout.ProductCatagoryWise)
             {
                data = _oDORegisters.GroupBy(x => new { x.ProductCategoryName }, (key, grp) => new
                {
                    HeaderName = key.ProductCategoryName,
                    Total_Qty = grp.Sum(x => x.Qty),
                    Total_DOQty = grp.Sum(x => x.DOQty),
                    Total_ChallanQty = grp.Sum(x => x.ChallanQty),
                    Total_YetToDO = grp.Sum(x => x.YetToDO),
                    Total_YetToChallan = grp.Sum(x => x.YetToChallan),
                    Results = grp.ToList()
                });
            }
            else
            {
               data = _oDORegisters.GroupBy(x => new { x.BuyerID, x.BuyerName }, (key, grp) => new
               {
                   HeaderName = key.BuyerName,
                   Total_Qty = grp.Sum(x => x.Qty),
                   Total_DOQty = grp.Sum(x => x.DOQty),
                   Total_ChallanQty = grp.Sum(x => x.ChallanQty),
                   Total_YetToDO = grp.Sum(x => x.YetToDO),
                   Total_YetToChallan = grp.Sum(x => x.YetToChallan),
                   Results = grp.ToList()
               });
            }
            #endregion
            PdfPTable oPdfPTable = GetDetailsTable();
            oPdfPTable = GetDetailsTable();

            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE); 
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Purchase Order Register (" + sHeader + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);


            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "#SL", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "PI No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "PI Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "LC File No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Customer Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            if (eReportLayout == EnumReportLayout.ProductCatagoryWise)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Buyer Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            }
            else
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Catagory Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            }

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Product Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Color", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Style No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            ESimSolPdfHelper.AddCell(ref oPdfPTable, "PI Qty", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "DO Qty", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Due DO", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Challan Qty", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Pending C", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            #endregion
            
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

            #region Print Details
            foreach(var oItem in data)
            {
                #region Header
               
                int nCount = 0, ExportPIID = 0;

                oPdfPTable = GetDetailsTable();
                oPdfPTable = GetDetailsTable();

                #region Data
                foreach (var obj in oItem.Results)
                {
                    #region Order Wise Span
                    if (ExportPIID != obj.ExportPIID)
                    {
                        if (nCount > 0)
                        {
                            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                            PrintBlankRows(eReportLayout, ref oPdfPTable);

                            if (eReportLayout == EnumReportLayout.ProductCatagoryWise)
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);
                            else
                                ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 4, 0);

                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.DOQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.YetToDO)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.ChallanQty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.YetToChallan)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                            oPdfPTable.CompleteRow(); ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                            oPdfPTable = new PdfPTable(12);
                            oPdfPTable = GetDetailsTable();
                        }


                        ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                        ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeaderCoulmn + oItem.HeaderName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                        ESimSolPdfHelper.FontStyle = _oFontStyle;
                        //nRowSpan = oItem.Results.Where(OrderR => OrderR.ExportPIID == obj.ExportPIID).ToList().Count + 1;

                        ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.LEFT_BORDER);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.PINo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.LEFT_BORDER);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.PIDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.LEFT_BORDER);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.LCFileNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.LEFT_BORDER);

                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.CustomerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.LEFT_BORDER);
                        if (eReportLayout == EnumReportLayout.ProductCatagoryWise)
                        {
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.BuyerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.LEFT_BORDER);
                        }
                    }
                    else 
                    {
                        PrintBlankRows(eReportLayout,ref oPdfPTable);
                    }
                    #endregion
                    if (eReportLayout != EnumReportLayout.ProductCatagoryWise)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductCategoryName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    }
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ColorName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    if (obj.StyleNo.Length>25)
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.StyleNo.Substring(0,25), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    else
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.StyleNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.DOQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.YetToDO), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.ChallanQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.YetToChallan), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    oPdfPTable.CompleteRow();
                    if (ExportPIID != obj.ExportPIID)
                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                    else
                        ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable,0);
                    oPdfPTable = GetDetailsTable();

                    ExportPIID = obj.ExportPIID;
                }
                #endregion

                PrintBlankRows(eReportLayout, ref oPdfPTable);

                #region Sub Total
                ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                if (eReportLayout == EnumReportLayout.ProductCatagoryWise)
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);
                else
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 4, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.DOQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.YetToDO)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.ChallanQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.ExportPIID == ExportPIID).Sum(x => x.YetToChallan)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
               
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion

                #region Sub Total (Layout Wise)
                oPdfPTable = new PdfPTable(12);
                oPdfPTable = GetDetailsTable();

                ESimSolPdfHelper.AddCell(ref oPdfPTable, sHeader + " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 9, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Total_Qty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Total_DOQty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Total_YetToDO), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Total_ChallanQty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Total_YetToChallan), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion
            }
            #region Grand Total
            PdfPTable oPdfPTable_GT = new PdfPTable(12);
            oPdfPTable_GT = GetDetailsTable();

            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, " Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 9, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(_oDORegisters.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(_oDORegisters.Sum(x => x.DOQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(_oDORegisters.Sum(x => x.YetToDO)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(_oDORegisters.Sum(x => x.ChallanQty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(_oDORegisters.Sum(x => x.YetToChallan)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
         
            oPdfPTable_GT.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_GT);
            #endregion
            //_oPdfPTable.CompleteRow();
            #endregion
        }

        private void PrintBlankRows(EnumReportLayout eReportLayout, ref PdfPTable oPdfPTable)
        {
            int emptyRows = 5;
            if (eReportLayout == EnumReportLayout.ProductCatagoryWise)
                emptyRows = 6;

            while (emptyRows > 0)
            {
                emptyRows--;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, iTextSharp.text.Rectangle.LEFT_BORDER);
            }
        }
        #endregion
    }
}
