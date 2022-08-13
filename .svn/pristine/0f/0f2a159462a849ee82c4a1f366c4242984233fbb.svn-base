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
    public class rptNOA
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_Bold;
        iTextSharp.text.Font _oFontStyle_UnLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Company _oCompany = new Company();
        NOA _oNOA = new NOA();
        List<NOADetail> _oNOADetails = new List<NOADetail>();
        Contractor _oContractor = new Contractor();
        List<SupplierRateProcess> _oSupplierRateProcess = new List<SupplierRateProcess>();
        #endregion

        #region NOA LC
        public byte[] PrepareReport(NOA oNOA, BusinessUnit oBusinessUnit, Company oCompany)
        {
            _oNOA = oNOA;
            _oNOADetails = oNOA.NOADetailLst;
            _oSupplierRateProcess = oNOA.SupplierRateProcess;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 
                                                    595f //Articale
                                                 
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

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });


            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Note of Approval", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintHeader_Blank()
        {

            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 15f, 4);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.FixedHeight = 150f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Note of Approval", _oFontStyle_UnLine));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body


        private void PrintBody()
        {


            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.BOLD);

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 200f, 200f });


            _oPdfPCell = new PdfPCell(new Phrase("NOA No: " + _oNOA.NOANo, _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date: " + _oNOA.NOADate.ToString("MMMM dd, yyyy"), _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Prepare By: " + _oNOA.PrepareByName, _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Note: " + _oNOA.Note, _oFontStyle_Bold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();



            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();



            #region NOA Details

          

            int nCoulumnLength = 6, ncolumncount = 1;
            int nMaxSupplierCount = GetMaxSupplierCount() * 2;//supplier and RAte so multiply with 2

            nCoulumnLength += nMaxSupplierCount;

            oPdfPTable = new PdfPTable(nCoulumnLength);
            float[] dCollumn = new float[nCoulumnLength];

            dCollumn[0] = 30;///SL
            dCollumn[1] = 100;//Particular
            for (int i = 0; i < nMaxSupplierCount; i++)
            {
                if (i % 2 == 0)
                {
                    ncolumncount++;
                    dCollumn[ncolumncount] = 340 / nMaxSupplierCount;
                }
                else
                {
                    ncolumncount++;
                    dCollumn[ncolumncount] = 290 / nMaxSupplierCount;
                }

            }
            ncolumncount++;
            dCollumn[ncolumncount] = 30f;//Unit
            ncolumncount++;
            dCollumn[ncolumncount] = 40f;//Qty
            ncolumncount++;
            dCollumn[ncolumncount] = 60f;//amount
            ncolumncount++;
            dCollumn[ncolumncount] = 80f;//remark
            
            oPdfPTable.SetWidths(dCollumn);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region 1st Row

            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Particular", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            for (int i = 0; i < nMaxSupplierCount / 2; i++)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Supplier #" + (i + 1), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Unit", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Remark", _oFontStyle));
            _oPdfPCell.Rowspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            for (int i = 0; i < nMaxSupplierCount / 2; i++)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Supplier", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            oPdfPTable.CompleteRow();

            #endregion

            double nTotalAmount = 0; string sValue = "", sFistValue = "", sSecondValue = "", nTemp = "";
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyle_Bold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            int nCount = 0;
            if (_oSupplierRateProcess.Count > 0)
            {

                foreach (SupplierRateProcess oItem in _oSupplierRateProcess)
                {

                    nCount++;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    for (int i = 1; i <= (nMaxSupplierCount / 2); i++)
                    {
                        if (oItem.DeliveryFromStockQty > 0 && i == 1)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(_oNOA.BUName, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            sValue = GetSupplierNameOrRate(i, oItem, true);
                            if (sValue != "")
                            {
                                sFistValue = sValue.Split('~')[0];
                                sSecondValue = sValue.Split('~')[1];
                            }
                            else
                            {
                                sFistValue = "0.00";
                                sSecondValue = "false";
                            }

                            if (sSecondValue == "true")
                            {
                                _oPdfPCell = new PdfPCell(new Phrase(sFistValue, _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);
                            }
                            else
                            {
                                _oPdfPCell = new PdfPCell(new Phrase(sFistValue, _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            }

                            sValue = GetSupplierNameOrRate(i, oItem, false);
                            if (sValue != "")
                            {
                                sFistValue = sValue.Split('~')[0];
                                sSecondValue = sValue.Split('~')[1];
                            }
                            else
                            {
                                sFistValue = "0.00";
                                sSecondValue = "false";
                            }
                            if (sSecondValue == "true")
                            {
                                nTemp = Global.MillionFormat_Round(Convert.ToDouble(sFistValue));
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Convert.ToDouble(sFistValue)), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.GRAY; oPdfPTable.AddCell(_oPdfPCell);
                            }
                            else
                            {
                                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Convert.ToDouble(sFistValue)), _oFontStyle));
                                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            }
                        }

                    }
                    if (oItem.DeliveryFromStockQty > 0)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitSymbol, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DeliveryFromStockQty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.UnitSymbol, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.RequiredQty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    }
                    _oPdfPCell = new PdfPCell(new Phrase((oItem.Amount).ToString("#,##0.####"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    nTotalAmount += oItem.Amount;
                    oPdfPTable.CompleteRow();


                }
                _oPdfPCell = new PdfPCell(new Phrase("Total:", _oFontStyle_Bold));
                _oPdfPCell.Colspan = nMaxSupplierCount + 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase((nTotalAmount).ToString("#,##0.####"), _oFontStyle_Bold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
           

            #endregion



            #region Authorized Signature

            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 100f, 100f, 100f, 100f });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("___________________", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Requistion By", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }


        #region Function

        private string GetSupplierNameOrRate(int nPostion, SupplierRateProcess sItem, bool bIsSupplierName)
        {


            if (nPostion == 1) { if (bIsSupplierName) { return sItem.SupplierName1; } else { return sItem.Rate1; } }
            else if (nPostion == 2) { if (bIsSupplierName) { return sItem.SupplierName2; } else { return sItem.Rate2; } }
            else if (nPostion == 3) { if (bIsSupplierName) { return sItem.SupplierName3; } else { return sItem.Rate3; } }
            else if (nPostion == 4) { if (bIsSupplierName) { return sItem.SupplierName4; } else { return sItem.Rate4; } }
            else if (nPostion == 5) { if (bIsSupplierName) { return sItem.SupplierName5; } else { return sItem.Rate5; } }
            else if (nPostion == 6) { if (bIsSupplierName) { return sItem.SupplierName6; } else { return sItem.Rate6; } }
            else if (nPostion == 7) { if (bIsSupplierName) { return sItem.SupplierName7; } else { return sItem.Rate7; } }
            else if (nPostion == 8) { if (bIsSupplierName) { return sItem.SupplierName8; } else { return sItem.Rate8; } }
            else if (nPostion == 9) { if (bIsSupplierName) { return sItem.SupplierName9; } else { return sItem.Rate9; } }
            else if (nPostion == 10) { if (bIsSupplierName) { return sItem.SupplierName10; } else { return sItem.Rate10; } }
            else if (nPostion == 11) { if (bIsSupplierName) { return sItem.SupplierName11; } else { return sItem.Rate11; } }
            else if (nPostion == 12) { if (bIsSupplierName) { return sItem.SupplierName12; } else { return sItem.Rate12; } }
            else if (nPostion == 13) { if (bIsSupplierName) { return sItem.SupplierName13; } else { return sItem.Rate13; } }
            else if (nPostion == 14) { if (bIsSupplierName) { return sItem.SupplierName14; } else { return sItem.Rate14; } }
            else if (nPostion == 15) { if (bIsSupplierName) { return sItem.SupplierName15; } else { return sItem.Rate15; } }

            return "";

        }
        private int GetMaxSupplierCount()
        {
            int nMexSupplier = 0;
            foreach (SupplierRateProcess oitem in _oSupplierRateProcess)
            {
                if (oitem.MaxSupplierCount > 0)
                {
                    nMexSupplier = oitem.MaxSupplierCount;
                }
            }


            return nMexSupplier;
        }

        #endregion
        #endregion
        #endregion
    }
}
