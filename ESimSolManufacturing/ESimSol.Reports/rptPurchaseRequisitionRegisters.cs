﻿using System;
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
    public class rptPurchaseRequisitionRegisters
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
        List<PurchaseRequisitionRegister> _oPurchaseRequisitionRegisters = new List<PurchaseRequisitionRegister>();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(List<PurchaseRequisitionRegister> oPurchaseRequisitionRegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange)
        {
            _oPurchaseRequisitionRegisters = oPurchaseRequisitionRegisters;
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
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    70f,  //Order No
                                                    50f,  //Order Status
                                                    120f,  //Order Date
                                                    70f,  //DateofBill
                                                    100f,  //ApprovedByName
                                                    65f,   //Code
                                                    155f,  //Product Name
                                                    40f,   //M Unit
                                                    40f,   //Qty
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
            if (eReportLayout == EnumReportLayout.ProductWise)
            {
                sHeader = "Product Wise"; sHeaderCoulmn = "Product Name : ";
            }
            else if (eReportLayout == EnumReportLayout.DepartmentWise)
            {
                sHeader = "Department Wise"; sHeaderCoulmn = "Department Name : ";
            }
            else if (eReportLayout == EnumReportLayout.DateWiseDetails)
            {
                sHeader = "Date Wise"; sHeaderCoulmn = "PR Date : ";
            }
            #endregion

            #region Group By Layout Wise
            var data = _oPurchaseRequisitionRegisters.GroupBy(x => new { x.DepartmentName, x.DepartmentID }, (key, grp) => new
            {
                HeaderName = key.DepartmentName,
                TotalQty = grp.Sum(x => x.Qty),
                Results = grp.ToList()
            });

            if (eReportLayout == EnumReportLayout.ProductWise)
            {
                data = _oPurchaseRequisitionRegisters.GroupBy(x => new { x.ProductName, x.ProductID }, (key, grp) => new
                {
                    HeaderName = key.ProductName,
                    TotalQty = grp.Sum(x => x.Qty),
                    Results = grp.ToList()
                });
            }
            else if (eReportLayout == EnumReportLayout.DateWiseDetails)
            {
                data = _oPurchaseRequisitionRegisters.GroupBy(x => new { x.PRDateSt }, (key, grp) => new
                {
                    HeaderName = key.PRDateSt,
                    TotalQty = grp.Sum(x => x.Qty),
                    Results = grp.ToList()
                });
            }
            #endregion

            ESimSolPdfHelper.FontStyle = _oFontStyleBoldUnderLine;
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Purchase Requisition Register (" + sHeader + ")", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0);

            #region Print Details
            foreach (var oItem in data)
            {
                ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                ESimSolPdfHelper.AddCell(ref _oPdfPTable, sHeaderCoulmn+oItem.HeaderName, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);

                #region Header
                PdfPTable oPdfPTable = GetDetailsTable();
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "#SL", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "PR No", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "PR Status", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

                if (eReportLayout == EnumReportLayout.DateWiseDetails)
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Supplier Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                else
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "PR Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Requirement Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Approved By Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);

                if (eReportLayout == EnumReportLayout.ProductWise)
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Approve Date", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Supplier Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                }
                else
                {
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Product Code", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, "Product Name", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                }
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "M Unit", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, "Qty", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 15);
                #endregion
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

                string sCurrencySymbol = "";
                int nCount = 0, nRowSpan =0, PRID = 0;
                
                oPdfPTable = new PdfPTable(10);
                oPdfPTable = GetDetailsTable();

                #region Data
                foreach (var obj in oItem.Results)
                {
                    #region Order Wise Span
                    if (PRID != obj.PRID)
                    {
                        if (nCount > 0)
                        {
                            ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15, 0, 3, 0);

                            ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.PRID == PRID).Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                            
                            oPdfPTable.CompleteRow();
                            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);

                            oPdfPTable = new PdfPTable(10);
                            oPdfPTable = GetDetailsTable();
                        }

                        ESimSolPdfHelper.FontStyle = _oFontStyle;
                        nRowSpan = oItem.Results.Where(OrderR => OrderR.PRID == obj.PRID).ToList().Count+1;

                        ESimSolPdfHelper.AddCell(ref oPdfPTable, (++nCount).ToString(), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.PRNo, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.StatusSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                
                        if (eReportLayout!= EnumReportLayout.DateWiseDetails)
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.DepartmentName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        else
                            ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.PRDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);

                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.RequirementDateInString, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ApprovedByName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15, nRowSpan, 0, 0);
                    }
                    #endregion

                    if (eReportLayout== EnumReportLayout.ProductWise)
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ApproveDateSt, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.DepartmentName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    }
                    else
                    {
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductCode, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                        ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.ProductName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    }

                    ESimSolPdfHelper.AddCell(ref oPdfPTable, obj.UnitName, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                    ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(obj.Qty), Element.ALIGN_RIGHT, Element.ALIGN_TOP, BaseColor.WHITE, 15);
                   
                    oPdfPTable.CompleteRow();
                    PRID = obj.PRID;
                }
                #endregion

                #region Sub Total
                ESimSolPdfHelper.FontStyle = _oFontStyleBold;
                ESimSolPdfHelper.AddCell(ref oPdfPTable, " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 3, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.Results.Where(x => x.PRID == PRID).Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion

                #region Sub Total (Layout Wise)
                oPdfPTable = new PdfPTable(12);
                oPdfPTable = GetDetailsTable();

                ESimSolPdfHelper.AddCell(ref oPdfPTable, sHeader + " Sub Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 9, 0);
                ESimSolPdfHelper.AddCell(ref oPdfPTable, Global.MillionFormatActualDigit(oItem.TotalQty), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
                
                oPdfPTable.CompleteRow();
                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable);
                #endregion
            }
            #region Grand Total
            PdfPTable oPdfPTable_GT = new PdfPTable(12);
            oPdfPTable_GT = GetDetailsTable();

            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, " Grand Total ", Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15, 0, 9, 0);
            ESimSolPdfHelper.AddCell(ref oPdfPTable_GT, Global.MillionFormatActualDigit(_oPurchaseRequisitionRegisters.Sum(x => x.Qty)), Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, BaseColor.WHITE, 15);
            
            oPdfPTable_GT.CompleteRow();
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_GT);
            #endregion

            #endregion
        }
        #endregion
    }
}
