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
    public class rptProductionRegister
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
        List<ProductionRegister> _oProductionRegisters = new List<ProductionRegister>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(List<ProductionRegister> oProductionRegisters, Company oCompany, BusinessUnit oBusinessUnit, EnumReportLayout eReportLayout, string sReportHeading)
        {
            _oProductionRegisters = oProductionRegisters;
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

            #region Layout Wise Header
            string sHeader = "", sHeaderCoulmn="";
            if (eReportLayout == EnumReportLayout.ProductWise)
            {
                sHeader = "Product Wise"; sHeaderCoulmn = "Product Name : ";
            }
            else if (eReportLayout == EnumReportLayout.Machine_Wise)
            {
                sHeader = "Machine Wise"; sHeaderCoulmn = "Machine Name : ";
            }
            else if (eReportLayout == EnumReportLayout.DateWise)
            {
                sHeader = "Date Wise"; sHeaderCoulmn = "Transaction Date : ";
            }
            #endregion

            this.PrintHeader( oBusinessUnit, "Production Order Register (" + sHeader + ")", sReportHeading);
            this.PrintBody(eReportLayout, sHeader, sHeaderCoulmn);
            _oPdfPTable.HeaderRows = 3;

            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(BusinessUnit oBU, string sReportHeader, string sDateRange)
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
            _oPdfPCell = new PdfPCell(new Phrase(oBU.Name, _oFontStyle));
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
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody(EnumReportLayout eReportLayout, string sHeader, string sHeaderCoulmn)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Group By Layout Wises
            List<ProductionRegister> GroupWiseData = new List<ProductionRegister>();

            if (eReportLayout == EnumReportLayout.DateWise)
            {
                GroupWiseData = _oProductionRegisters.GroupBy(x => new { x.TransactionDateSt }, (key, grp) => new ProductionRegister
                {
                    RowHeader = key.TransactionDateSt,
                    MoldingProduction = grp.Sum(x => x.MoldingProduction),
                    Results = grp.ToList()
                }).ToList();
            }
            else if (eReportLayout == EnumReportLayout.ProductWise)
            {
                GroupWiseData = _oProductionRegisters.GroupBy(x => new { x.ProductID, x.ProductName }, (key, grp) => new ProductionRegister
                {
                    RowHeader = key.ProductName,
                    MoldingProduction = grp.Sum(x => x.MoldingProduction),
                    Results = grp.ToList()
                }).ToList();
            }
            else if (eReportLayout == EnumReportLayout.Machine_Wise)
            {
                GroupWiseData = _oProductionRegisters.GroupBy(x => new { x.MachineID, x.MachineName, }, (key, grp) => new ProductionRegister
                {
                    RowHeader = key.MachineName,
                    MoldingProduction = grp.Sum(x => x.MoldingProduction),
                    Results = grp.ToList()
                }).ToList();
            }
            #endregion

            //ESimSolPdfHelper.FontStyle = _oFontStyleBoldUnderLine;
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Production Order Register (" + sHeader + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);
            //ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);


            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
            PrintTableHeader(eReportLayout);

            #region Print Details
            foreach (ProductionRegister oItem in GroupWiseData)
            {
                //if (_oDocument.PageNumber > 0 && (595 - ESimSolPdfHelper.CalculatePdfPTableHeight(_oPdfPTable)) < 15)
                //{
                //    ESimSolPdfHelper.NewPageDeclaration(_oDocument, _oPdfPTable);
                //}

                ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeaderCoulmn + oItem.RowHeader, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                int nCount = 0, nRowSpan =0, PSID = 0;

                //PrintTableHeader(eReportLayout);
                PdfPTable oPdfPTable = new PdfPTable(12);
                oPdfPTable = GetDataTable(eReportLayout);

                #region Data
                foreach (var obj in oItem.Results)
                {
                    #region Order Wise Span
                    if (PSID != obj.ProductionSheetID)
                    {
                        if (nCount > 0)
                        {
                            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                            PrintTotal(oPdfPTable, oItem.Results.Where(x => x.ProductionSheetID == PSID).ToList(), " Sub Total ", eReportLayout);
                            oPdfPTable = new PdfPTable(12);
                            oPdfPTable = GetDataTable(eReportLayout);
                        }

                        ESimSolPdfHelper.FontStyle = _oFontStyle;
                        nRowSpan = oItem.Results.Where(Sheet => Sheet.ProductionSheetID == obj.ProductionSheetID).ToList().Count + 1;

                        ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.SheetNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ExportPINo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.CustomerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.BuyerName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);

                        if (eReportLayout != EnumReportLayout.ProductWise)
                        {
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductCode, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        }
                    }
                    #endregion
                    if (eReportLayout == EnumReportLayout.DateWise)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ShiftName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.MachineName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    }
                    else if (eReportLayout == EnumReportLayout.Machine_Wise)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.TransactionDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ShiftName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    }
                    else if (eReportLayout == EnumReportLayout.ProductWise)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.TransactionDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ShiftName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.MachineName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    }

                    if (PSID != obj.ProductionSheetID)
                    {
                        nRowSpan = nRowSpan - 1;
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.UnitSymbol, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.SheetQty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    }

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.MoldingProduction), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                    if (PSID != obj.ProductionSheetID)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.ActualMoldingProduction), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.YetToModling), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.ActualFinishGoods), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.ActualRejectGoods), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.YetToProduction), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    }

                    PSID = obj.ProductionSheetID;
                }
                #endregion

                #region Sub Total
                
                ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                PrintTotal(oPdfPTable, oItem.Results.Where(x => x.ProductionSheetID == PSID).ToList(), " Sub Total ", eReportLayout);
                #endregion

                #region Sub Total (Layout Wise)

                oPdfPTable = new PdfPTable(12);
                oPdfPTable = GetDataTable(eReportLayout);

                PrintTotal(oPdfPTable, oItem.Results.ToList(), sHeader + " Sub Total", eReportLayout);
                #endregion
            }

            #region Grand Total

            PrintTotal(GetDataTable(eReportLayout), _oProductionRegisters, sHeader + " Grand Total ", eReportLayout);
            #endregion

            #endregion
        }
        #endregion

        #region DataTables
        public PdfPTable GetProductTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(16);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            oPdfPTable.SetWidths(new float[] {
                                                22f,//SL
                                                50f,//Sheet Name
                                                58f, //PI No
                                                80f, //Customer Name
                                                80f, //Buyer
                                                55f,//Date
                                                50f, //Shift
                                                50f, //machine
                                                32f, //Unit
                                                48f,//Sheet
                                                45f,//tota Production
                                                47f,//ActMolding
                                                45f,//molding qty
                                                45f,//molding percen
                                                40f,//finish Good qty
                                                45f//remarks
                                        });
            return oPdfPTable;
        }
        public PdfPTable GetShiftOrMachineTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(17);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            oPdfPTable.SetWidths(new float[] {       
                                                20f,//SL
                                                45f,//Sheet Name
                                                55f, //PI No
                                                80f, //Customer Name
                                                80f, //Buyer
                                                45f,//Product
                                                65f, //Product
                                                50f, //Shift/Date
                                                50f, //Machine
                                                30f, //Unit
                                                45f,//Production hour
                                                48f,//tota Production
                                                48f,//molding qty
                                                48f,//molding qty
                                                45f,//molding percen
                                                41f,//finish Good qty
                                                45f//remarks
                                        });
            return oPdfPTable;
        }
        public PdfPTable GetDataTable(EnumReportLayout eReportLayout) 
        {
            if (eReportLayout == EnumReportLayout.ProductWise)
                return GetProductTable();
            else return GetShiftOrMachineTable();
        }
        #endregion
        
        #region Support Functions
        private void PrintTotal(PdfPTable oPdfPTable, List<ProductionRegister> oProductionRegisters, string sTotal, EnumReportLayout eReportLayout)
        {
            if (sTotal.Equals(" Sub Total "))
            {
                #region Total
                if (eReportLayout != EnumReportLayout.ProductWise)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, sTotal, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oProductionRegisters.Sum(x => x.MoldingProduction)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                }
                else if (eReportLayout == EnumReportLayout.ProductWise)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, sTotal, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 4, 0);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oProductionRegisters.Sum(x => x.MoldingProduction)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                }
                #endregion
            }
            else 
            {
                #region Total
                if (eReportLayout != EnumReportLayout.ProductWise)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, sTotal, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 10, 0);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oProductionRegisters.Sum(x => x.MoldingProduction)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);

                }
                else if (eReportLayout == EnumReportLayout.ProductWise)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, sTotal, Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 9, 0);

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oProductionRegisters.Sum(x => x.MoldingProduction)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                }
                #endregion
            }
           
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
        }
        private void PrintTableHeader(EnumReportLayout eReportLayout) 
        {
            #region Header
            PdfPTable oPdfPTable = GetDataTable(eReportLayout);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "#SL", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Sheet No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "PI No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Customer Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Buyer Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            if (eReportLayout != EnumReportLayout.ProductWise)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "P Code", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Product Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            }

            if (eReportLayout == EnumReportLayout.DateWise)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Shift", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Machine", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            }
            else if (eReportLayout == EnumReportLayout.Machine_Wise)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Transaction Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Shift", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            }
            else if (eReportLayout == EnumReportLayout.ProductWise)
            {
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Transaction Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Shift", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Machine", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            }
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Unit", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Sheet Qty", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Production Qty", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Total Production", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Pending Production", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "QC Pass", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "QC Reject", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
            ESimSolPdfHelper.AddCell(ref oPdfPTable, "Pending QC", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

            #endregion
            oPdfPTable.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
        }
        #endregion
    }
}




 